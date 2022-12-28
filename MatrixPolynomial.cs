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
