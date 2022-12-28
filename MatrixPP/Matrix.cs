class Matrix
{
    /*
    Класс матрицы с различными видами вычислений: последовательным и параллельным
    */

    public double[,] Data;
    private static uint[] _dimension = new uint[2];


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

    // 2. Геттеры и сеттеры
    public uint[] GetDimension()
    {
        return _dimension;
    }

    // 3. Матричные операции
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
        /*
        Метод, транспонирующий матрицу
        */

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

    //3.1 Сумма двух матриц
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
        Метод параллельного сложения двух матриц с использованием кастомного threadpool
        */

        // исключение в случае несовпадения размерностей матриц
        if (another.GetDimension()[0] != GetDimension()[0] || another.GetDimension()[1] != GetDimension()[1])
        {
            throw new Exception("Сложение не возможно! Размерность первой матрицы не равно размерности второй матрицы.");
        }

        // результирующая матрица
        var res_matrix = new Matrix(another.GetDimension()[0], another.GetDimension()[1]);

        // параллелим для каждой строчки
        for (var i = 0; i < another.GetDimension()[0]; i++)
        {
            thread_pool.Execute(i, obj =>
            {
                WorkSum((int)obj, ref res_matrix, ref another);
            });
            
        }
        while (thread_pool.GetWorkCount() > 0);
        return res_matrix;
    }

    

    
    // 3.2 Умножение матрицы на матрицу
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
        Метод параллельного умножения двух матриц с использованием кастомного threadpool
        */

        if (GetDimension()[1] != another.GetDimension()[0])
        {
            throw new Exception("Умножение не возможно! Количество столбцов первой матрицы не равно количеству строк второй матрицы.");
        }

        var matrixC = new Matrix(GetDimension()[0], another.GetDimension()[1]);
        foreach (var i in Enumerable.Range(0, (int)GetDimension()[0]))
        {
            thread_pool.Execute(i, obj =>
            {
                WorkProduct((int)obj, ref matrixC, ref another);
            });
        }
        while (thread_pool.GetWorkCount() > 0){
            //Console.WriteLine(thread_pool.GetWorkCount());
        }
        ;
        return matrixC;
    }

    
    //3.3 Возведение матрицы в степень (наиболее важный метод для матричных полиномов)
    public static Matrix operator^(Matrix matrix, uint n)
    {
        /*
        Метод непараллельного возведения матрицы в степень
        */

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
        Параллельное умножение матриц с использованием кастомного threadpool
        */
        if (n == 0) {
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

    public Matrix ParallelPowBinThreadPool(uint n, InstanceThreadPool thread_pool)
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
        
        for (int i = 0; i < stack1.Length; i++){
            if (i == 0)
            {
                cache[i] = res;
            }
            else
            {
                cache[i] = cache[i - 1].ParallelProductThreadPool(cache[i - 1], thread_pool);
            }
        }

        bool check = false;
        for (int j = 0; j < stack1.Length; j++)
        {
            if (stack1.Substring(stack1.Length - j - 1, 1) == "1")
            {
                if (check)
                {
                    res = res.ParallelProductThreadPool(cache[j], thread_pool);
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

    

    

    // 4. Линейные преобразования
    //TODO: умножение на число всей матрицы

    public Matrix ParallelProduct(double number)
    {
        Matrix res = new Matrix(GetDimension()[0], GetDimension()[1]);

        Parallel.For(0, GetDimension()[0], i =>
        {
            for (var j = 0; j < GetDimension()[1]; j++)
            {
                res.Data[i, j] = Data[i, j] * number;
            }
        });
        return res;
    }
}