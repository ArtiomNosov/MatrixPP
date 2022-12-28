class MatrixPolynomial
{
    /*
    Класс Матричный полином
    */

    private double[] _data = new double[0] {};

    // 1. Конструктор полиномов
    public MatrixPolynomial(double[] coefficient)
    {
        this._data = coefficient;
    }

    static void PrintMatrix(Matrix matrix)
    {
        Console.Write("[");
        for (var i = 0; i < matrix.GetDimension()[0]; i++)
        {
            if (i != 0)
            {
                Console.Write(" ");
            }
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
            if (i + 1 == matrix.GetDimension()[0])
            {
                Console.Write("]");
            }
            else
            {
                Console.Write(",");
            }
            Console.WriteLine();
        }
    }


    public Matrix Calculate(Matrix matrix)
    {
        if (matrix.GetDimension()[0] != matrix.GetDimension()[1])
        {
            throw new Exception("Матричный полном должен состоять из квадратных матриц!");
        }


        Matrix res = new Matrix(matrix.GetDimension()[0], matrix.GetDimension()[1]);
        InstanceThreadPool thread_pool = new InstanceThreadPool(4, Name: "Обработчик матриц");
        for (int i = 0; i < this._data.Length; i++)
        {            
            res = res.ParallelSumThreadPool(matrix.ParallelPowBinThreadPool(i, thread_pool).ParallelProduct(this._data[this._data.Length - i - 1]), thread_pool);
            
            //Console.WriteLine($"Матрица: ");
        }
        return res;
    }

}
