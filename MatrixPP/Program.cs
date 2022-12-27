using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class Matrix
{
    public double[,] Data;
    private static uint[] _dimension = new uint[2];
    // метод расширения для получения количества строк матрицы
    public uint[] GetDimension()
    {
        return _dimension;
    }

    // 1. Конструкторы матриц
    public Matrix(uint n, uint m)
    {
        Data = new double[n, m];
        _dimension[0] = (uint)Data.GetUpperBound(0) + 1;
        _dimension[1] = (uint)Data.GetUpperBound(1) + 1;
    }

    public Matrix(Matrix matrix)
    {
        double[,] newData = new double[matrix.GetDimension()[0], matrix.GetDimension()[1]];
        for (var i = 0; i < newData.GetUpperBound(0) + 1; i++)
        {
            for (var j = 0; j < newData.GetUpperBound(1) + 1; j++)
            {
                newData[i, j] = matrix.Data[i, j];
            }
        }
        Data = newData;
    }

    public Matrix(double[,] data)
    {
        Data = data;
        _dimension[0] = (uint)Data.GetUpperBound(0) + 1;
        _dimension[1] = (uint)Data.GetUpperBound(1) + 1;
    }

    public static Matrix GetRandomMatrix(uint n, uint m)
    {
        
        //var new_matrix = new Matrix(n, m);
        var data = new double[n, m];
        Random rnd = new Random();
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                data[i, j] = rnd.Next() % 256;
                //Console.WriteLine($"New value is {data[i, j]}");
            }
        }
        var new_matrix = new Matrix(data);
        return new_matrix;
    }

    // 2. Матричные операции
    public Matrix ParallelSum(Matrix another)
    {
        /* 
        Метод параллельного сложения двух матриц
        */

        // исключение в случае несовпадения размерностей матриц
        if (another.GetDimension()[0] != GetDimension()[0] || another.GetDimension()[1] != GetDimension()[1])
        {
            throw new Exception("Сложение не возможно! Размерность первой матрицы не равно размерности второй матрицы.");
        }

        // результирующая матрица
        var res_matrix = new Matrix(another.GetDimension()[0], another.GetDimension()[1]);

        // параллелим для каждой строчки
        Parallel.For(0, another.GetDimension()[0], i =>
        {
            for (int j = 0; j < another.GetDimension()[1]; j++){
                res_matrix.Data[i, j] = another.Data[i, j] + Data[i, j];
            }
        });

        return res_matrix;
    }

    void WorkSum(int i, ref Matrix res_matrix, ref Matrix another)
    {
        for (int j = 0; j < another.GetDimension()[1]; j++)
        {
            res_matrix.Data[i, j] = another.Data[i, j] + Data[i, j];
        }
    }
    public Matrix ParallelSumThreadPool(Matrix another, InstanceThreadPool thread_pool)
    {
        /* 
        Метод параллельного сложения двух матриц
        */

        // исключение в случае несовпадения размерностей матриц
        if (another.GetDimension()[0] != GetDimension()[0] || another.GetDimension()[1] != GetDimension()[1])
        {
            throw new Exception("Сложение не возможно! Размерность первой матрицы не равно размерности второй матрицы.");
        }

        // результирующая матрица
        var res_matrix = new Matrix(another.GetDimension()[0], another.GetDimension()[1]);

        int a = 0;

        // параллелим для каждой строчки
        for (var i = 0; i < another.GetDimension()[0]; i++)
        {
            a++;
            thread_pool.Execute(i, obj =>
            {
                WorkSum((int)obj, ref res_matrix, ref another);
                a--;
            });
            
        }
        while (a > 0)
            ;
        return res_matrix;
    }

    public static Matrix operator+(Matrix matrixA, Matrix matrixB)
    {
        /* 
        Перегрузка оператора сложения для двух матриц
        */

        // исключение в случае несовпадения размерностей матриц
        if (matrixA.GetDimension()[0] != matrixB.GetDimension()[0] || matrixA.GetDimension()[1] != matrixB.GetDimension()[1])
        {
            throw new Exception("Сложение не возможно! Размерность первой матрицы не равно размерности второй матрицы.");
        }

        var matrixC = new Matrix(matrixA.GetDimension()[0], matrixB.GetDimension()[1]);

        for (var i = 0; i < matrixA.GetDimension()[0]; i++)
        {
            for (var j = 0; j < matrixB.GetDimension()[1]; j++)
            {
                matrixC.Data[i, j] = matrixA.Data[i, j] + matrixB.Data[i, j];
            }
        }

        return matrixC;
    }

    public Matrix ParallelProduct(Matrix another)
    {
        /* 
        Метод параллельного умножения двух матриц
        */

        if (GetDimension()[1] != another.GetDimension()[0])
        {
            throw new Exception("Умножение не возможно! Количество столбцов первой матрицы не равно количеству строк второй матрицы.");
        }

        var matrixC = new Matrix(GetDimension()[0], another.GetDimension()[1]);

        Parallel.For(0, GetDimension()[0], i =>
        {
            for (var j = 0; j < another.GetDimension()[1]; j++)
            {
                //matrixC.Data[i, j] = 0;
                for (var k = 0; k < GetDimension()[1]; k++)
                {
                    matrixC.Data[i, j] += Data[i, k] * another.Data[k, j];
                }
            }
        });



        return matrixC;
    }

    void WorkProduct(int i, ref Matrix matrixC, ref Matrix another)
    {
        for (var j = 0; j < another.GetDimension()[1]; j++)
        {
            //matrixC.Data[i, j] = 0;
            for (var k = 0; k < GetDimension()[1]; k++)
            {
                matrixC.Data[i, j] += Data[i, k] * another.Data[k, j];
            }
        }
    }

    public Matrix ParallelProductThreadPool(Matrix another, InstanceThreadPool thread_pool)
    {
        /* 
        Метод параллельного умножения двух матриц
        */

        if (GetDimension()[1] != another.GetDimension()[0])
        {
            throw new Exception("Умножение не возможно! Количество столбцов первой матрицы не равно количеству строк второй матрицы.");
        }

        var matrixC = new Matrix(GetDimension()[0], another.GetDimension()[1]);
        int a = 0;
        foreach (var i in Enumerable.Range(0, (int)GetDimension()[0]))
        {
            a++;
            thread_pool.Execute(i, obj =>
            {
                WorkProduct((int)obj, ref matrixC, ref another);
                a--;
            });
        }
        while (a > 0)
        {
            Thread.Sleep(500);
            Console.WriteLine("Mem");
        }
        return matrixC;
    }

    public static Matrix operator *(Matrix matrixA, Matrix matrixB)
    {
        /* 
        Перегрузка оператора умножения для двух матриц
        */

        if (matrixA.GetDimension()[1] != matrixB.GetDimension()[0])
        {
            throw new Exception("Умножение не возможно! Количество столбцов первой матрицы не равно количеству строк второй матрицы.");
        }

        var matrixC = new Matrix(matrixA.GetDimension()[0], matrixB.GetDimension()[1]);

        for (var i = 0; i < matrixA.GetDimension()[0]; i++)
        {
            for (var j = 0; j < matrixB.GetDimension()[1]; j++)
            {
                matrixC.Data[i, j] = 0;

                for (var k = 0; k < matrixA.GetDimension()[1]; k++)
                {
                    matrixC.Data[i, j] += matrixA.Data[i, k] * matrixB.Data[k, j];
                }
            }
        }

        return matrixC;
    }

    public static Matrix E(uint n, uint m)
    {
        /*
        Метод, возвращающий единичную матрицу заданной размерности
        */
        Matrix res = new Matrix(n, m);
        for (var i = 0; i < res.GetDimension()[0]; i++)
        {
            for (var j = 0; j < res.GetDimension()[1]; j++)
            {
                if (i == j)
                {
                    res.Data[i, j] = 1;
                }
            }
        }
        return res;
    }
    
    public Matrix T()
    {
        Matrix res_matrix = new Matrix(GetDimension()[0], GetDimension()[1]);

        for (int i = 0; i < res_matrix.GetDimension()[0]; i++)
        {
            for (int j = 0; j < res_matrix.GetDimension()[1]; j++)
            {
                res_matrix.Data[i, j] = Data[j, i];
            }
        }
        return res_matrix;
    }
    
    public static Matrix operator^(Matrix matrix, uint n)
    {
        if (n == 0) {
            return E(matrix.GetDimension()[0], matrix.GetDimension()[1]);
        }

        var res = new Matrix(matrix);

        for (var i = 0; i < n - 1; i++)
        {
            res = res * matrix;
        }
        return res;
    }

    public Matrix ParallelPow(uint n)
    {
        /*
        Параллельное умножение матриц 
        */
        if (n == 0) {
            return E(GetDimension()[0], GetDimension()[1]);
        }

        var res = new Matrix(this);

        for (var i = 0; i < n - 1; i++)
        {
            res = res.ParallelProduct(this);
        }
        return res;
    }

    public Matrix ParallelPowThreadPool(uint n, InstanceThreadPool thread_pool)
    {
        /*
        Параллельное умножение матриц 
        */
        if (n == 0)
        {
            return E(GetDimension()[0], GetDimension()[1]);
        }

        var res = new Matrix(this);

        for (var i = 0; i < n - 1; i++)
        {
            res = res.ParallelProductThreadPool(this, thread_pool);
        }
        return res;
    }

    public Matrix ParallelPowBin(uint n)
    {
        /*
        Считаем матрицы во всем степенях двойки, не превосходящие максимальной степени лвойки в двоичном 
        разложении. После чего происходит параллельное умножение полученных матриц.
        */
        if (n == 0) {
            return E(GetDimension()[0], GetDimension()[1]);
        }

        var res = new Matrix(this);
        string stack1 = Convert.ToString(n, 2);
        Matrix[] cache = new Matrix[stack1.Length];
        // Console.WriteLine(stack1.Substring(2,2));

        for (int i = 0; i < stack1.Length; i++){
            if (i == 0)
            {
                cache[i] = res;
            }
            else
            {
                cache[i] = cache[i - 1].ParallelProduct(cache[i - 1]);
            }
        }

        bool check = false;
        for (int j = 0; j < stack1.Length; j++)
        {
            if (stack1.Substring(stack1.Length - j - 1, 1) == "1")
            {
                if (check)
                {
                    res = res.ParallelProduct(cache[j]);
                }
                else
                {
                    res = cache[j];
                    check = true;
                }
                
            }
        }
        
        return res;
    }

    // 3. Линейные преобразования
    //TODO: умножение на число всей матрицы
}


class Program
{
    // метод для получения матрицы из консоли
    static double[,] GetMatrixFromConsole(string name)
    {
        Console.Write("Количество строк матрицы {0}:    ", name);
        var n = int.Parse(Console.ReadLine());
        Console.Write("Количество столбцов матрицы {0}: ", name);
        var m = int.Parse(Console.ReadLine());

        var matrix = new double[n, m];
        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < m; j++)
            {
                Console.Write("{0}[{1},{2}] = ", name, i, j);
                matrix[i, j] = double.Parse(Console.ReadLine());
            }
        }

        return matrix;
    }

    // метод для печати матрицы в консоль
    static void PrintMatrix(Matrix matrix)
    {
        for (var i = 0; i < matrix.GetDimension()[0]; i++)
        {
            for (var j = 0; j < matrix.GetDimension()[1]; j++)
            {
                Console.Write(matrix.Data[i, j].ToString().PadLeft(10));
            }

            Console.WriteLine();
        }
    }

    static void Main(string[] args)
    {
        bool checkProduct = false;
        bool checkSum = true;
        bool checkPow = true;
        bool checkTranspose = false;

        // It's a creating a ThreadPool
        var thread_pool = new InstanceThreadPool(100, Name: "Обработчик матриц");
        for (int i = 0;i < 100; i++)
            thread_pool.Execute(i, obj => { /* nothing*/});
        Console.WriteLine("Программа для умножения матриц");


        Matrix matrixB = Matrix.GetRandomMatrix(200, 200);
        Matrix matrixA = Matrix.GetRandomMatrix(200, 200);

        
        if (checkProduct)
        {
            var sw = new Stopwatch();
            sw.Start();
            var result = matrixA.ParallelProduct(matrixB);
            sw.Stop();
            Console.WriteLine($"Параллельное умножение матриц: {sw.Elapsed}");

            sw.Reset();
            sw.Start();
            result = matrixA * matrixB;
            sw.Stop();
            Console.WriteLine($"Обычное умножение матриц: {sw.Elapsed}");
            //PrintMatrix(result);

            //Console.ReadLine();
        }

        if (checkSum)
        {
            var sw = new Stopwatch();
            sw.Start();
            var result = matrixA.ParallelSum(matrixB);
            sw.Stop();
            Console.WriteLine($"Параллельное сложение матриц: {sw.Elapsed}");

            sw.Reset();
            sw.Start();
            result = matrixA + matrixB;
            sw.Stop();
            Console.WriteLine($"Обычное сложение матриц: {sw.Elapsed}");
            sw.Reset();
            sw.Start();
            // А теперь с ThreadPool
            var result4 = matrixA.ParallelSumThreadPool(matrixB, thread_pool);
            sw.Stop();
            Console.WriteLine($"Параллельное сложение матриц c ThreadPool: {sw.Elapsed}");
        }

        if (checkPow)
        {
            uint power = 5;
            var sw = new Stopwatch();
            Console.WriteLine("До возведение матриц в степень: ");
            //PrintMatrix(matrixA);
            sw.Start();
            var result1 = matrixA.ParallelPowBin(power);
            sw.Stop();
            Console.WriteLine($"Параллельное возведение матриц в степень: {sw.Elapsed}");
            //PrintMatrix(result1);

            var result3 = matrixA^power;

            sw.Reset();
            sw.Start();
            var result2 = matrixA^power;
            sw.Stop();
            Console.WriteLine($"Обычное возведение матриц в степень: {sw.Elapsed}");
            sw.Reset();
            sw.Start();
            // А теперь с ThreadPool
            var result4 = matrixA.ParallelPowThreadPool(power, thread_pool);
            sw.Stop();
            Console.WriteLine($"Параллельное возведение матриц в степень c ThreadPool: {sw.Elapsed}");
        }

        if (checkTranspose)
        {
            Matrix before_transpose = Matrix.GetRandomMatrix(3, 3);
            Console.WriteLine("Матрица до транспонирования: ");
            PrintMatrix(before_transpose);
            Matrix after_transpose = before_transpose.T();
            Console.WriteLine("Матрица до транспонирования: ");
            PrintMatrix(after_transpose);
        }

        
        Console.ReadLine();
        Console.WriteLine("And again");
        Console.ReadLine();
    }
}
