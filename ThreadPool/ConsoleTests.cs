using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ThreadPool
{
    internal class ConsoleTests
    {


        static void Main(string[] args)
        {
            //System.Diagnostics.Trace.Listeners.Add(new System.Diagnostics.ConsoleTraceListener());
            var messages = Enumerable.Range(1, 1000).Select(i => $"Message-{i}");

            using (var thread_pool = new InstanceThreadPool(10, Name: "Обработчик сообщений"))
            {
                //thread_pool.Execute("123", obj => { });

                foreach (var message in messages)
                    thread_pool.Execute(message, obj =>
                    {
                        var msg = (string)obj;
                        Console.WriteLine(">> Обработка сообщения {0} начата...", msg);
                        Thread.Sleep(5000);
                        Console.WriteLine(">> Обработка сообщения {0} выполнена", msg);
                    });

                Console.ReadLine();
            }


            Console.ReadLine();
        }
        
    }
}
