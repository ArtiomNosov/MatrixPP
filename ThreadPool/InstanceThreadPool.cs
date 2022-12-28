using System.Diagnostics;

// ReSharper disable once CheckNamespace
namespace System.Threading;

public class InstanceThreadPool : IDisposable
{
    private readonly ThreadPriority _Prioroty;
    private readonly string _Name;
    private readonly Thread[] _Threads;
    private readonly Queue<(Action<object?> Work, object? Parameter)> _Works = new();
    private volatile bool _CanWork = true;

    private readonly AutoResetEvent _WorkingEvent = new(false);
    private readonly AutoResetEvent _ExecuteEvent = new(true);

    public string Name => _Name;

    public InstanceThreadPool(int MaxThreadsCount, ThreadPriority Prioroty = ThreadPriority.Normal, string? Name = null)
    {
        if (MaxThreadsCount <= 0)
            throw new ArgumentOutOfRangeException(nameof(MaxThreadsCount), MaxThreadsCount, "Число потоков в пуле должно быть больше, либо равно 1");

        _Prioroty = Prioroty;
        _Threads = new Thread[MaxThreadsCount];
        // ReSharper disable once VirtualMemberCallInConstructor
        _Name = Name ?? GetHashCode().ToString("x");
        Initialize();
    }

    private void Initialize()
    {
        var thread_pool_name = Name;
        for (var i = 0; i < _Threads.Length; i++)
        {
            var name = $"{nameof(InstanceThreadPool)}[{thread_pool_name}]-Thread[{i}]";
            var thread = new Thread(WorkingThread)
            {
                Name = name,
                IsBackground = true,
                Priority = _Prioroty
            };
            _Threads[i] = thread;
            thread.Start();
        }
    }

    public void Execute(Action Work) => Execute(null, _ => Work());

    public void Execute(object? Parameter, Action<object?> Work)
    {
        if (!_CanWork) throw new InvalidOperationException("Попытка передать задание уничтоженному пулу потоков");

        _ExecuteEvent.WaitOne(); // запрашиваем доступ к очереди
        if (!_CanWork) throw new InvalidOperationException("Попытка передать задание уничтоженному пулу потоков");

        _Works.Enqueue((Work, Parameter));
        _ExecuteEvent.Set();    // разрешаем доступ к очереди

        _WorkingEvent.Set();
    }

    private void WorkingThread()
    {
        var thread_name = Thread.CurrentThread.Name;
        Trace.TraceInformation("Поток {0} запущен с id:{1}", thread_name, Environment.CurrentManagedThreadId);

        try
        {
            while (_CanWork)
            {
                _WorkingEvent.WaitOne();
                if (!_CanWork) break;

                _ExecuteEvent.WaitOne(); // запрашиваем доступ к очередя

                while (_Works.Count == 0) // если (до тех пор пока) в очереди нет заданий
                {
                    _ExecuteEvent.Set(); // освобождаем очередь
                    _WorkingEvent.WaitOne(); // дожидаемся разрешения на выполнение
                    if (!_CanWork) break;

                    _ExecuteEvent.WaitOne(); // запрашиваем доступ к очереди вновь
                }

                var (work, parameter) = _Works.Dequeue();
                if (_Works.Count > 0) // если после изъятия из очереди задания там осталось ещё что-то
                    _WorkingEvent.Set(); //  то запускаем ещё один поток на выполнение

                _ExecuteEvent.Set(); // разрешаем доступ к очереди

                Trace.TraceInformation("Поток {0}[id:{1}] выполняет задание", thread_name, Environment.CurrentManagedThreadId);
                try
                {
                    var timer = Stopwatch.StartNew();
                    work(parameter);
                    timer.Stop();

                    Trace.TraceInformation(
                        "Поток {0}[id:{1}] выполнил задание за {2}мс",
                        thread_name, Environment.CurrentManagedThreadId, timer.ElapsedMilliseconds);
                }
                catch (ThreadInterruptedException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    Trace.TraceError("Ошибка выполнения задания в потоке {0}:{1}", thread_name, e);
                }
            }
            
        }
        catch (ThreadInterruptedException)
        {
            Trace.TraceWarning(
                "Поток {0} был принудительно прерван при завершении работы пула {1}",
                thread_name, Name);
        }
        finally
        {
            Trace.TraceInformation("Поток {0} завершил свою работу", thread_name);
            if (!_WorkingEvent.SafeWaitHandle.IsClosed)
                _WorkingEvent.Set();
        }
    }

    public int GetWorkCount()
    {
        return _Works.Count;
    }

    private const int _DisposeThreadJoinTimeout = 100;
    public void Dispose()
    {
        _CanWork = false;

        _WorkingEvent.Set();
        foreach (var thread in _Threads)
            if (!thread.Join(_DisposeThreadJoinTimeout))
                thread.Interrupt();

        _ExecuteEvent.Dispose();
        _WorkingEvent.Dispose();
        Trace.TraceInformation("Пул потоков {0} уничтожен", Name);
    }
}
