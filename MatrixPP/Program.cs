using System;
using System.Threading;
using System.Diagnostics;


// класс с методами расширения



class Program
{
    
    // метод для получения матрицы из консоли
    static double[,] GetMatrixFromConsole(string name)
    {
        Console.Write("Количество строк матрицы {0}:    ", name);
        var n = int.Parse(Console.ReadLine() + "");
        Console.Write("Количество столбцов матрицы {0}: ", name);
        var m = int.Parse(Console.ReadLine() + "");

        var matrix = new double[n, m];
        for (var i = 0; i < n; i++)
        {
            for (var j = 0; j < m; j++)
            {
                Console.Write("{0}[{1},{2}] = ", name, i, j);
                matrix[i, j] = double.Parse(Console.ReadLine() + "");
            }
        }

        return matrix;
    }

    // метод для печати матрицы в консоль
    static void PrintMatrix(Matrix matrix)
    {
        for (var i = 0; i < matrix.GetDimension()[0]; i++)
        {
            Console.Write("[");
            for (var j = 0; j < matrix.GetDimension()[1]; j++)
            {
                Console.Write(matrix.Data[i, j].ToString().PadLeft(10));
                if (j + 1 < matrix.GetDimension()[1])
                {
                    Console.Write(",");
                }
            }
            Console.Write("]");

            Console.WriteLine();
        }
    }

    static void Main(string[] args)
    {
        bool checkSum = false;
        bool checkProduct = true;
        bool checkPow = false;
        bool checkTranspose = false;
        bool timeTest = false;

        Console.WriteLine("Программа для умножения матриц");

        //var a = GetMatrixFromConsole("A");
        //var b = GetMatrixFromConsole("B");

        uint dim1 = 100;
        
        Matrix matrixB = Matrix.GetRandomMatrix(dim1, dim1);
        Matrix matrixA = Matrix.GetRandomMatrix(dim1, dim1);


        //Matrix elem = new Matrix(a);
        
        if (timeTest)
        {
            
            uint[] from = new uint[] {100, 100};
            uint[] to = new uint[] {1000, 1000};
            uint count = 10;
            TimeTest.Sum(count, from, to);
            TimeTest.Product(count, from, to);
            TimeTest.Power(count, from, to, 10);
        }

        if (checkProduct)
        {
            InstanceThreadPool thread_pool = new InstanceThreadPool(4, Name: "Обработчик матриц");
            
            var sw = new Stopwatch();

            sw.Reset();
            sw.Start();
            var result2 = matrixA * matrixB;
            sw.Stop();
            Console.WriteLine($"Product Ordinary: {sw.Elapsed}");

            sw.Reset();
            sw.Start();
            var result3 = matrixA.ParallelProduct(matrixB);
            sw.Stop();
            Console.WriteLine($"Product Parallel: {sw.Elapsed}");

            sw.Reset();
            sw.Start();
            var result1 = matrixA.ParallelProductThreadPool(matrixB, thread_pool);
            sw.Stop();
            Console.WriteLine($"Product Parallel + ThreadPool: {sw.Elapsed}");
            //Console.WriteLine(result1.Data[4, 4]);
            //Console.WriteLine(result2.Data[4, 4]);
            //Console.WriteLine(result3.Data[4, 4]);
        }

        if (checkSum)
        {
            
            InstanceThreadPool thread_pool = new InstanceThreadPool(4, Name: "Обработчик матриц");
            
            var sw = new Stopwatch();

            sw.Reset();
            sw.Start();
            var result2 = matrixA + matrixB;
            sw.Stop();
            Console.WriteLine($"Sum Ordunary: {sw.Elapsed}");

            sw.Reset();
            sw.Start();
            var result3 = matrixA.ParallelSum(matrixB);
            sw.Stop();
            Console.WriteLine($"Sum Parallel: {sw.Elapsed}");

            sw.Reset();
            sw.Start();
            var result1 = matrixA.ParallelSumThreadPool(matrixB, thread_pool);
            sw.Stop();
            Console.WriteLine($"Sum Parallel + ThreadPool: {sw.Elapsed}");
            //Console.WriteLine(result1.Data[4, 4]);
            //Console.WriteLine(result2.Data[4, 4]);
            //Console.WriteLine(result3.Data[4, 4]);
        }

        if (checkPow)
        {
            InstanceThreadPool thread_pool = new InstanceThreadPool(4, Name: "Обработчик матриц");

            uint power = 10;
            var sw = new Stopwatch();

            sw.Reset();
            sw.Start();
            var result1 = matrixA^power;
            sw.Stop();
            Console.WriteLine($"Pow Ordinary: {sw.Elapsed}");

            sw.Reset();
            sw.Start();
            var result21 = matrixA.ParallelPow(power);
            sw.Stop();
            Console.WriteLine($"Pow Parallel: {sw.Elapsed}");

            sw.Reset();
            sw.Start();
            var result22 = matrixA.ParallelPowThreadPool(power, thread_pool);
            sw.Stop();
            Console.WriteLine($"Pow Parallel + ThreadPool: {sw.Elapsed}");

            sw.Reset();
            sw.Start();
            var result31 = matrixA.ParallelPowBin(power);
            sw.Stop();
            Console.WriteLine($"Pow Parallel + Bin: {sw.Elapsed}");

            sw.Reset();
            sw.Start();
            var result32 = matrixA.ParallelPowBinThreadPool(power, thread_pool);
            sw.Stop();
            Console.WriteLine($"Pow Parallel + Bin + ThreadPool: {sw.Elapsed}");
            //Console.WriteLine(result1.Data[4, 4]);
            //Console.WriteLine(result21.Data[4, 4]);
            //Console.WriteLine(result22.Data[4, 4]);
            //Console.WriteLine(result31.Data[4, 4]);
            //Console.WriteLine(result32.Data[4, 4]);

            

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
    }
}