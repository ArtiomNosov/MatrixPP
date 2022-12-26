using System;

// класс с методами расширения
class Matrix
{
    public double[,] Data;
    private static uint[] _dimension = new uint[2];
    // метод расширения для получения количества строк матрицы
    public uint Dimension(uint dimension)
    {
        _dimension[0] = (uint)Data.GetUpperBound(0) + 1;
        _dimension[1] = (uint)Data.GetUpperBound(1) + 1;
        return _dimension[dimension];
    }

    public Matrix(uint n, uint m)
    {
        Data = new double[n, m];
    }

    public Matrix(Matrix matrix)
    {
        double[,] newData = new double[matrix.Dimension(0), matrix.Dimension(1)];
        for (var i = 0; i < newData.GetUpperBound(0); i++)
        {
            for (var j = 0; j < newData.GetUpperBound(1); j++)
            {
                newData[i, j] = matrix.Data[i, j];
            }
        }
        Data = newData;
    }

    public Matrix(double[,] data)
    {
        Data = data;
    }
    public static Matrix operator+(Matrix matrixA, Matrix matrixB)
    {
        if (matrixA.Dimension(0) != matrixB.Dimension(0) || matrixA.Dimension(1) != matrixB.Dimension(1))
        {
            throw new Exception("Сложение не возможно! Размерность первой матрицы не равно размерности второй матрицы.");
        }

        var matrixC = new Matrix(matrixA.Dimension(0), matrixB.Dimension(1));

        for (var i = 0; i < matrixA.Dimension(0); i++)
        {
            for (var j = 0; j < matrixB.Dimension(1); j++)
            {
                matrixC.Data[i, j] = matrixA.Data[i, j] + matrixB.Data[i, j];
            }
        }

        return matrixC;
    }

    public static Matrix operator *(Matrix matrixA, Matrix matrixB)
    {
        if (matrixA.Dimension(1) != matrixB.Dimension(0))
        {
            throw new Exception("Умножение не возможно! Количество столбцов первой матрицы не равно количеству строк второй матрицы.");
        }

        var matrixC = new Matrix(matrixA.Dimension(0), matrixB.Dimension(1));

        for (var i = 0; i < matrixA.Dimension(0); i++)
        {
            for (var j = 0; j < matrixB.Dimension(1); j++)
            {
                matrixC.Data[i, j] = 0;

                for (var k = 0; k < matrixA.Dimension(1); k++)
                {
                    matrixC.Data[i, j] += matrixA.Data[i, k] * matrixB.Data[k, j];
                }
            }
        }

        return matrixC;
    }

    public static Matrix E(uint n, uint m)
    {
        Matrix res = new Matrix(n, m);
        for (var i = 0; i < res.Dimension(0); i++)
        {
            for (var j = 0; j < res.Dimension(1); j++)
            {
                res.Data[i, j] = 1;
            }
        }
        return res;
    }
    public static Matrix operator^(Matrix matrix, uint n)
    {
        if (n == 0) {
            return E(matrix.Dimension(0), matrix.Dimension(1));
        }
        var res = new Matrix(matrix);

        for (var i = 0; i < n - 1; i++)
        {
            res = res * matrix;
        }

        return res;
    }
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
        for (var i = 0; i < matrix.Dimension(0); i++)
        {
            for (var j = 0; j < matrix.Dimension(1); j++)
            {
                Console.Write(matrix.Data[i, j].ToString().PadLeft(10));
            }

            Console.WriteLine();
        }
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Программа для умножения матриц");

        var a = GetMatrixFromConsole("A");
        var b = GetMatrixFromConsole("B");

        Console.WriteLine("Матрица A:");
        Matrix matrixA = new Matrix(a);
        PrintMatrix(matrixA);

        Console.WriteLine("Матрица B:");
        Matrix matrixB = new Matrix(b);
        PrintMatrix(matrixB);

        uint pow = 0;
        var result = matrixA + matrixB;
        Console.WriteLine("Возведение матрицы в степень {0}:", pow);
        PrintMatrix(result);

        Console.ReadLine();
    }
}