using System;
using System.Diagnostics;

namespace Menu
{
    internal class Program
    {
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
        enum MenuStatus
        {
            Finish,
            Start
        }
        static string[] MainMenu =
        {
            "Q - выход из программы",
            "1 - сгенерировать случайные матрицы",
            "2 - сложить две матрицы",
            "3 - умножить две матрицы",
            "4 - возвести матрицу в степень",
            "5 - транспонировать матрицу",
            "6 - посчитать матричный полином с коэффициентам (1, 1, 1)", 
            "f1 - сложить две матрицы параллельно",
            "f2 - умножить две матрицы параллельно",
            "f3 - возвести матрицу в степень параллельно",
            "f4 - возвести матрицу в степень параллельно бинарным методом",
            "f5 - сложить две матрицы параллельно с тредпулом",
            "f6 - умножить две матрицы параллельно с тредпулом",
            "f7 - возвести матрицу в степень параллельно с тредпулом",
            "f8 - возвести матрицу в степень параллельно бинарным методом с тредпулом"
        };
        static void PrintArray<T>(T[] array)
        {
            foreach (var item in array)
                Console.WriteLine(item);
        }
        static void Main()
        {
            int pow = 0;
            MenuStatus menuStatus = MenuStatus.Start;
            Console.WriteLine("Введите размерность квадратных матриц А и B: ");
            uint dim = uint.Parse(Console.ReadLine());
            Matrix matrixB = Matrix.GetRandomMatrix(dim, dim), matrixA = Matrix.GetRandomMatrix(dim, dim), result = Matrix.E(dim, dim);
            Console.WriteLine("Матрица А:\n");
            PrintMatrix(matrixA);
            Console.WriteLine("Матрица B:\n");
            PrintMatrix(matrixB);
            var sw = new Stopwatch();
            while (menuStatus == MenuStatus.Start)
            {
                sw.Reset();
                PrintArray(MainMenu);
                ConsoleKey consoleKey = Console.ReadKey().Key;
                Console.WriteLine("\n");
                switch (consoleKey)
                {
                    case ConsoleKey.Backspace:
                        break;
                    case ConsoleKey.Tab:
                        break;
                    case ConsoleKey.Clear:
                        break;
                    case ConsoleKey.Enter:
                        break;
                    case ConsoleKey.Pause:
                        break;
                    case ConsoleKey.Escape:
                        break;
                    case ConsoleKey.Spacebar:
                        break;
                    case ConsoleKey.PageUp:
                        break;
                    case ConsoleKey.PageDown:
                        break;
                    case ConsoleKey.End:
                        break;
                    case ConsoleKey.Home:
                        break;
                    case ConsoleKey.LeftArrow:
                        break;
                    case ConsoleKey.UpArrow:
                        break;
                    case ConsoleKey.RightArrow:
                        break;
                    case ConsoleKey.DownArrow:
                        break;
                    case ConsoleKey.Select:
                        break;
                    case ConsoleKey.Print:
                        break;
                    case ConsoleKey.Execute:
                        break;
                    case ConsoleKey.PrintScreen:
                        break;
                    case ConsoleKey.Insert:
                        break;
                    case ConsoleKey.Delete:
                        break;
                    case ConsoleKey.Help:
                        break;
                    case ConsoleKey.D0:
                        break;
                    case ConsoleKey.D1:
                        break;
                    case ConsoleKey.D2:
                        break;
                    case ConsoleKey.D3:
                        break;
                    case ConsoleKey.D4:
                        break;
                    case ConsoleKey.D5:
                        break;
                    case ConsoleKey.D6:
                        break;
                    case ConsoleKey.D7:
                        break;
                    case ConsoleKey.D8:
                        break;
                    case ConsoleKey.D9:
                        break;
                    case ConsoleKey.A:
                        break;
                    case ConsoleKey.B:
                        break;
                    case ConsoleKey.C:
                        break;
                    case ConsoleKey.D:
                        break;
                    case ConsoleKey.E:
                        break;
                    case ConsoleKey.F:
                        break;
                    case ConsoleKey.G:
                        break;
                    case ConsoleKey.H:
                        break;
                    case ConsoleKey.I:
                        break;
                    case ConsoleKey.J:
                        break;
                    case ConsoleKey.K:
                        break;
                    case ConsoleKey.L:
                        break;
                    case ConsoleKey.M:
                        break;
                    case ConsoleKey.N:
                        break;
                    case ConsoleKey.O:
                        break;
                    case ConsoleKey.P:
                        break;
                    case ConsoleKey.Q:
                        menuStatus = MenuStatus.Finish;
                        Console.WriteLine("Завершение работы. Нажмите любую клавишу");
                        break;
                    case ConsoleKey.R:
                        break;
                    case ConsoleKey.S:
                        break;
                    case ConsoleKey.T:
                        break;
                    case ConsoleKey.U:
                        break;
                    case ConsoleKey.V:
                        break;
                    case ConsoleKey.W:
                        break;
                    case ConsoleKey.X:
                        break;
                    case ConsoleKey.Y:
                        break;
                    case ConsoleKey.Z:
                        break;
                    case ConsoleKey.LeftWindows:
                        break;
                    case ConsoleKey.RightWindows:
                        break;
                    case ConsoleKey.Applications:
                        break;
                    case ConsoleKey.Sleep:
                        break;
                    case ConsoleKey.NumPad0:
                        break;
                    case ConsoleKey.NumPad1:
                        matrixA = Matrix.GetRandomMatrix(dim, dim);
                        matrixB = Matrix.GetRandomMatrix(dim, dim);
                        break;
                    case ConsoleKey.NumPad2:
                        sw.Start();
                        result = matrixA + matrixB;
                        sw.Stop();
                        break;
                    case ConsoleKey.NumPad3:
                        sw.Start();
                        result = matrixA * matrixB;
                        sw.Stop();
                        break;
                    case ConsoleKey.NumPad4:
                        Console.WriteLine("Введите степень в которую нужно возвести матрицу А: ");
                        pow = int.Parse(Console.ReadLine());
                        sw.Start();
                        result = matrixA^pow;
                        sw.Stop();
                        Console.WriteLine("Посчитан результат возведения матрицы А в степень " + pow);
                        break;
                    case ConsoleKey.NumPad5:
                        sw.Start();
                        result = matrixA.T();
                        sw.Stop();
                        Console.WriteLine("Посчитан результат транспонирования матрицы А " + pow);
                        break;
                    case ConsoleKey.NumPad6:
                        double[] coefficienst = new double[] { 1, 1, 1 };
                        MatrixPolynomial mp = new MatrixPolynomial(coefficienst);
                        sw.Start();
                        result = mp.Calculate(matrixA);
                        sw.Stop();
                        Console.WriteLine("Посчитан результат матричного полинома А " + pow);
                        break;
                    case ConsoleKey.NumPad7:
                        break;
                    case ConsoleKey.NumPad8:
                        break;
                    case ConsoleKey.NumPad9:
                        break;
                    case ConsoleKey.Multiply:
                        break;
                    case ConsoleKey.Add:
                        break;
                    case ConsoleKey.Separator:
                        break;
                    case ConsoleKey.Subtract:
                        break;
                    case ConsoleKey.Decimal:
                        break;
                    case ConsoleKey.Divide:
                        break;
                    case ConsoleKey.F1:
                        sw.Start();
                        result = matrixA.ParallelSum(matrixB);
                        sw.Stop();
                        break;
                    case ConsoleKey.F2:
                        sw.Start();
                        result = matrixA.ParallelProduct(matrixB);
                        sw.Stop();
                        break;
                    case ConsoleKey.F3:
                        sw.Start();
                        result = matrixA.ParallelPow(pow);
                        sw.Stop();
                        break;
                    case ConsoleKey.F4:
                        sw.Start();
                        result = matrixA.ParallelPowBin(pow);
                        sw.Stop();
                        break;
                    case ConsoleKey.F5:
                        sw.Start();
                        result = matrixA.ParallelSumThreadPool(matrixB);
                        sw.Stop();
                        break;
                    case ConsoleKey.F6:
                        sw.Start();
                        result = matrixA.ParallelProductThreadPool(matrixB);
                        sw.Stop();
                        break;
                    case ConsoleKey.F7:
                        sw.Start();
                        result = matrixA.ParallelPowThreadPool(pow);
                        sw.Stop();
                        break;
                    case ConsoleKey.F8:
                        sw.Start();
                        result = matrixA.ParallelPowBinThreadPool(pow);
                        sw.Stop();
                        break;
                    case ConsoleKey.F9:
                        break;
                    case ConsoleKey.F10:
                        break;
                    case ConsoleKey.F11:
                        break;
                    case ConsoleKey.F12:
                        break;
                    case ConsoleKey.F13:
                        break;
                    case ConsoleKey.F14:
                        break;
                    case ConsoleKey.F15:
                        break;
                    case ConsoleKey.F16:
                        break;
                    case ConsoleKey.F17:
                        break;
                    case ConsoleKey.F18:
                        break;
                    case ConsoleKey.F19:
                        break;
                    case ConsoleKey.F20:
                        break;
                    case ConsoleKey.F21:
                        break;
                    case ConsoleKey.F22:
                        break;
                    case ConsoleKey.F23:
                        break;
                    case ConsoleKey.F24:
                        break;
                    case ConsoleKey.BrowserBack:
                        break;
                    case ConsoleKey.BrowserForward:
                        break;
                    case ConsoleKey.BrowserRefresh:
                        break;
                    case ConsoleKey.BrowserStop:
                        break;
                    case ConsoleKey.BrowserSearch:
                        break;
                    case ConsoleKey.BrowserFavorites:
                        break;
                    case ConsoleKey.BrowserHome:
                        break;
                    case ConsoleKey.VolumeMute:
                        break;
                    case ConsoleKey.VolumeDown:
                        break;
                    case ConsoleKey.VolumeUp:
                        break;
                    case ConsoleKey.MediaNext:
                        break;
                    case ConsoleKey.MediaPrevious:
                        break;
                    case ConsoleKey.MediaStop:
                        break;
                    case ConsoleKey.MediaPlay:
                        break;
                    case ConsoleKey.LaunchMail:
                        break;
                    case ConsoleKey.LaunchMediaSelect:
                        break;
                    case ConsoleKey.LaunchApp1:
                        break;
                    case ConsoleKey.LaunchApp2:
                        break;
                    case ConsoleKey.Oem1:
                        break;
                    case ConsoleKey.OemPlus:
                        break;
                    case ConsoleKey.OemComma:
                        break;
                    case ConsoleKey.OemMinus:
                        break;
                    case ConsoleKey.OemPeriod:
                        break;
                    case ConsoleKey.Oem2:
                        break;
                    case ConsoleKey.Oem3:
                        break;
                    case ConsoleKey.Oem4:
                        break;
                    case ConsoleKey.Oem5:
                        break;
                    case ConsoleKey.Oem6:
                        break;
                    case ConsoleKey.Oem7:
                        break;
                    case ConsoleKey.Oem8:
                        break;
                    case ConsoleKey.Oem102:
                        break;
                    case ConsoleKey.Process:
                        break;
                    case ConsoleKey.Packet:
                        break;
                    case ConsoleKey.Attention:
                        break;
                    case ConsoleKey.CrSel:
                        break;
                    case ConsoleKey.ExSel:
                        break;
                    case ConsoleKey.EraseEndOfFile:
                        break;
                    case ConsoleKey.Play:
                        break;
                    case ConsoleKey.Zoom:
                        break;
                    case ConsoleKey.NoName:
                        break;
                    case ConsoleKey.Pa1:
                        break;
                    case ConsoleKey.OemClear:
                        break;
                    default:
                        break;
                }
                Console.WriteLine("Result: \n");
                PrintMatrix(result);
                Console.WriteLine($"Затраченное на операцию время: {sw.Elapsed}");
            }
            Console.ReadLine();
        }
    }
}
