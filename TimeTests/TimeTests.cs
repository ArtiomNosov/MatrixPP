using System.Diagnostics;

class TimeTest
{

    public static void SumForAll(uint count_iterations, uint[] min_dimention, uint[] max_dimention)
    {
        InstanceThreadPool thread_pool = new InstanceThreadPool(4, Name: "Обработчик матриц");
        System.IO.StreamWriter file = new System.IO.StreamWriter("TimeTests/SumTest.txt");
        uint[] dif_dim = new uint[] {(uint)((max_dimention[0] - min_dimention[0]) / count_iterations), (uint)((max_dimention[1] - min_dimention[1]) / count_iterations)};
        for (int i = 0; i < count_iterations; i++)
        {
            Matrix m1 = Matrix.GetRandomMatrix((uint)(min_dimention[0] + dif_dim[0] * i), (uint)(min_dimention[1] + dif_dim[1] * i));
            Matrix m2 = Matrix.GetRandomMatrix((uint)(min_dimention[0] + dif_dim[0] * i), (uint)(min_dimention[1] + dif_dim[1] * i));

            var sw = new Stopwatch();
            sw.Start();
            Matrix res1 = m1 + m2;
            sw.Stop();
            file.WriteLine($"Iteration: {i}, sizes: ({min_dimention[0] + dif_dim[0] * i}, {min_dimention[1] + dif_dim[1] * i})");
            Console.WriteLine($"Time test for sum 2 matrixes ({min_dimention[0] + dif_dim[0] * i}, {min_dimention[1] + dif_dim[1] * i})");
            Console.WriteLine($"        Ordinary:              {sw.Elapsed}");
            file.WriteLine($"Ordinary: {sw.Elapsed}");

            sw.Reset();
            sw.Start();
            Matrix res2 = m1.ParallelSum(m2);
            sw.Stop();
            Console.WriteLine($"        Parallel:              {sw.Elapsed}");
            file.WriteLine($"Parallel: {sw.Elapsed}");

            sw.Reset();
            sw.Start();
            Matrix res3 = m1.ParallelSumThreadPool(m2, thread_pool);
            sw.Stop();
            Console.WriteLine($"        Parallel + ThreadPool: {sw.Elapsed}");
            file.WriteLine($"Parallel + ThreadPool: {sw.Elapsed}");
        }
        file.Close();
    }

    public static void ProductForAll(uint count_iterations, uint[] min_dimention, uint[] max_dimention)
    {
        InstanceThreadPool thread_pool = new InstanceThreadPool(4, Name: "Обработчик матриц");
        System.IO.StreamWriter file = new System.IO.StreamWriter("TimeTests/ProductTest.txt");
        uint[] dif_dim = new uint[] {(uint)((max_dimention[0] - min_dimention[0]) / count_iterations), (uint)((max_dimention[1] - min_dimention[1]) / count_iterations)};
        for (int i = 0; i < count_iterations; i++)
        {
            Matrix m1 = Matrix.GetRandomMatrix((uint)(min_dimention[0] + dif_dim[0] * i), (uint)(min_dimention[1] + dif_dim[1] * i));
            Matrix m2 = Matrix.GetRandomMatrix((uint)(min_dimention[0] + dif_dim[0] * i), (uint)(min_dimention[1] + dif_dim[1] * i));
            var sw = new Stopwatch();
            sw.Start();
            Matrix res1 = m1 * m2;
            sw.Stop();
            file.WriteLine($"Iteration: {i}, sizes: ({min_dimention[0] + dif_dim[0] * i}, {min_dimention[1] + dif_dim[1] * i})");
            Console.WriteLine($"Time test for product 2 matrixes ({min_dimention[0] + dif_dim[0] * i}, {min_dimention[1] + dif_dim[1] * i})");
            Console.WriteLine($"        Ordinary:              {sw.Elapsed}");
            file.WriteLine($"Ordinary: {sw.Elapsed}");

            sw.Reset();
            sw.Start();
            Matrix res2 = m1.ParallelProduct(m2);
            sw.Stop();
            Console.WriteLine($"        Parallel:              {sw.Elapsed}");
            file.WriteLine($"Parallel: {sw.Elapsed}");

            sw.Reset();
            sw.Start();
            Matrix res3 = m1.ParallelProductThreadPool(m2, thread_pool);
            sw.Stop();
            Console.WriteLine($"        Parallel + ThreadPool: {sw.Elapsed}");
            file.WriteLine($"Parallel + ThreadPool: {sw.Elapsed}");
        }
        file.Close();
    }

    public static void PowerForAll(uint count_iterations, uint[] min_dimention, uint[] max_dimention, uint power)
    {
        InstanceThreadPool thread_pool = new InstanceThreadPool(4, Name: "Обработчик матриц");
        System.IO.StreamWriter file = new System.IO.StreamWriter("TimeTests/PowerTest.txt");
        uint[] dif_dim = new uint[] {(uint)((max_dimention[0] - min_dimention[0]) / count_iterations), (uint)((max_dimention[1] - min_dimention[1]) / count_iterations)};
        for (int i = 0; i < count_iterations; i++)
        {
            Matrix m1 = Matrix.GetRandomMatrix((uint)(min_dimention[0] + dif_dim[0] * i), (uint)(min_dimention[1] + dif_dim[1] * i));
            var sw = new Stopwatch();
            sw.Start();
            Matrix res1 = m1^power;
            sw.Stop();
            file.WriteLine($"Iteration: {i}, sizes: ({min_dimention[0] + dif_dim[0] * i}, {min_dimention[1] + dif_dim[1] * i})");
            Console.WriteLine($"Time test for power matrix ({min_dimention[0] + dif_dim[0] * i}, {min_dimention[1] + dif_dim[1] * i})");
            Console.WriteLine($"        Ordinary:                      {sw.Elapsed}");
            file.WriteLine($"Ordinary: {sw.Elapsed}");
            sw.Reset();
            sw.Start();
            Matrix res2 = m1.ParallelPow(power);
            sw.Stop();
            Console.WriteLine($"        Parallel:                      {sw.Elapsed}");
            file.WriteLine($"Parallel: {sw.Elapsed}");

            sw.Reset();
            sw.Start();
            Matrix res3 = m1.ParallelPowBin(power);
            sw.Stop();
            Console.WriteLine($"        Parallel + Cache:              {sw.Elapsed}");
            file.WriteLine($"Parallel + Cache: {sw.Elapsed}");

            sw.Reset();
            sw.Start();
            Matrix res4 = m1.ParallelPowThreadPool(power, thread_pool);
            sw.Stop();
            Console.WriteLine($"        Parallel + ThreadPool:         {sw.Elapsed}");
            file.WriteLine($"Parallel + ThreadPool: {sw.Elapsed}");

            sw.Reset();
            sw.Start();
            Matrix res5 = m1.ParallelPowBinThreadPool(power, thread_pool);
            sw.Stop();
            Console.WriteLine($"        Parallel + Cache + ThreadPool: {sw.Elapsed}");
            file.WriteLine($"Parallel + Cache + ThreadPool: {sw.Elapsed}");
            
        }
        file.Close();
    }

    public static void SumForParallels(uint count_iterations, uint[] min_dimention, uint[] max_dimention)
    {
        InstanceThreadPool thread_pool = new InstanceThreadPool(4, Name: "Обработчик матриц");
        System.IO.StreamWriter file = new System.IO.StreamWriter("TimeTests/SumTestParallel.txt");
        uint[] dif_dim = new uint[] {(uint)((max_dimention[0] - min_dimention[0]) / count_iterations), (uint)((max_dimention[1] - min_dimention[1]) / count_iterations)};
        for (int i = 0; i < count_iterations; i++)
        {
            Matrix m1 = Matrix.GetRandomMatrix((uint)(min_dimention[0] + dif_dim[0] * i), (uint)(min_dimention[1] + dif_dim[1] * i));
            Matrix m2 = Matrix.GetRandomMatrix((uint)(min_dimention[0] + dif_dim[0] * i), (uint)(min_dimention[1] + dif_dim[1] * i));

            var sw = new Stopwatch();
            
            file.WriteLine($"Iteration: {i}, sizes: ({min_dimention[0] + dif_dim[0] * i}, {min_dimention[1] + dif_dim[1] * i})");
            Console.WriteLine($"Time test for sum 2 matrixes ({min_dimention[0] + dif_dim[0] * i}, {min_dimention[1] + dif_dim[1] * i})");

            sw.Start();
            Matrix res2 = m1.ParallelSum(m2);
            sw.Stop();
            Console.WriteLine($"        Parallel:              {sw.Elapsed}");
            file.WriteLine($"Parallel: {sw.Elapsed}");

            sw.Reset();
            sw.Start();
            Matrix res3 = m1.ParallelSumThreadPool(m2, thread_pool);
            sw.Stop();
            Console.WriteLine($"        Parallel + ThreadPool: {sw.Elapsed}");
            file.WriteLine($"Parallel + ThreadPool: {sw.Elapsed}");
        }
        file.Close();
    }

    public static void ProductForParallel(uint count_iterations, uint[] min_dimention, uint[] max_dimention)
    {
        InstanceThreadPool thread_pool = new InstanceThreadPool(4, Name: "Обработчик матриц");
        System.IO.StreamWriter file = new System.IO.StreamWriter("TimeTests/ProductTestParallel.txt");
        uint[] dif_dim = new uint[] {(uint)((max_dimention[0] - min_dimention[0]) / count_iterations), (uint)((max_dimention[1] - min_dimention[1]) / count_iterations)};
        for (int i = 0; i < count_iterations; i++)
        {
            Matrix m1 = Matrix.GetRandomMatrix((uint)(min_dimention[0] + dif_dim[0] * i), (uint)(min_dimention[1] + dif_dim[1] * i));
            Matrix m2 = Matrix.GetRandomMatrix((uint)(min_dimention[0] + dif_dim[0] * i), (uint)(min_dimention[1] + dif_dim[1] * i));
            var sw = new Stopwatch();

            file.WriteLine($"Iteration: {i}, sizes: ({min_dimention[0] + dif_dim[0] * i}, {min_dimention[1] + dif_dim[1] * i})");
            Console.WriteLine($"Time test for product 2 matrixes ({min_dimention[0] + dif_dim[0] * i}, {min_dimention[1] + dif_dim[1] * i})");
            sw.Start();
            Matrix res2 = m1.ParallelProduct(m2);
            sw.Stop();
            Console.WriteLine($"        Parallel:              {sw.Elapsed}");
            file.WriteLine($"Parallel: {sw.Elapsed}");

            sw.Reset();
            sw.Start();
            Matrix res3 = m1.ParallelProductThreadPool(m2, thread_pool);
            sw.Stop();
            Console.WriteLine($"        Parallel + ThreadPool: {sw.Elapsed}");
            file.WriteLine($"Parallel + ThreadPool: {sw.Elapsed}");
        }
        file.Close();
    }
    
    public static void PowerForParallel(uint count_iterations, uint[] min_dimention, uint[] max_dimention, uint power)
    {
        InstanceThreadPool thread_pool = new InstanceThreadPool(4, Name: "Обработчик матриц");
        System.IO.StreamWriter file = new System.IO.StreamWriter("TimeTests/PowerTestParallel.txt");
        uint[] dif_dim = new uint[] {(uint)((max_dimention[0] - min_dimention[0]) / count_iterations), (uint)((max_dimention[1] - min_dimention[1]) / count_iterations)};
        for (int i = 0; i < count_iterations; i++)
        {
            Matrix m1 = Matrix.GetRandomMatrix((uint)(min_dimention[0] + dif_dim[0] * i), (uint)(min_dimention[1] + dif_dim[1] * i));
            var sw = new Stopwatch();
            
            file.WriteLine($"Iteration: {i}, sizes: ({min_dimention[0] + dif_dim[0] * i}, {min_dimention[1] + dif_dim[1] * i})");
            Console.WriteLine($"Time test for power matrix ({min_dimention[0] + dif_dim[0] * i}, {min_dimention[1] + dif_dim[1] * i})");

            sw.Start();
            Matrix res2 = m1.ParallelPow(power);
            sw.Stop();
            Console.WriteLine($"        Parallel:                      {sw.Elapsed}");
            file.WriteLine($"Parallel: {sw.Elapsed}");

            sw.Reset();
            sw.Start();
            Matrix res3 = m1.ParallelPowBin(power);
            sw.Stop();
            Console.WriteLine($"        Parallel + Cache:              {sw.Elapsed}");
            file.WriteLine($"Parallel + Cache: {sw.Elapsed}");

            sw.Reset();
            sw.Start();
            Matrix res4 = m1.ParallelPowThreadPool(power, thread_pool);
            sw.Stop();
            Console.WriteLine($"        Parallel + ThreadPool:         {sw.Elapsed}");
            file.WriteLine($"Parallel + ThreadPool: {sw.Elapsed}");

            sw.Reset();
            sw.Start();
            Matrix res5 = m1.ParallelPowBinThreadPool(power, thread_pool);
            sw.Stop();
            Console.WriteLine($"        Parallel + Cache + ThreadPool: {sw.Elapsed}");
            file.WriteLine($"Parallel + Cache + ThreadPool: {sw.Elapsed}");
            
        }
        file.Close();
    }
}