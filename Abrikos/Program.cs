using System;
using System.Text;
using System.Numerics;

class Program
{
    static void DoBlock_1()
    {
        int[] array;

        Console.Write("Бажаєте випадкове заповнення масиву? (так[1]/ні[2]): ");
        string userInput = Console.ReadLine().ToLower();

        if (userInput == "1")
        {
            array = GenerateRandomArray();
        }
        else
        {
            Console.Write("Бажаєте вводити кожен елемент у окремому рядку? (так[1]/ні[2]): ");
            userInput = Console.ReadLine().ToLower();

            if (userInput == "1")
            {
                array = InputArraySeparateLines();
            }
            else
            {
                array = InputArraySingleLine();
            }
        }

        Console.WriteLine("\nВаш масив:");
        PrintArray(array);

        int minIndex = FindMinIndex(array);
        Swap(array, 0, minIndex);

        int maxIndex = FindMaxIndex(array);
        Swap(array, array.Length - 1, maxIndex);

        Console.WriteLine("\nМасив після вставки мінімуму в початок та максимуму в кінець:");
        PrintArray(array);
    }
    static int[] GenerateRandomArray()
    {
        Console.Write("Введіть кількість елементів масиву: ");
        int length = int.Parse(Console.ReadLine());

        Random random = new Random();
        int[] array = new int[length];
        Console.Write("Введіть мінімальне число масиву: ");
        int min = int.Parse(Console.ReadLine());
        Console.Write("Введіть максимальне число масиву: ");
        int max = int.Parse(Console.ReadLine());

        for (int i = 0; i < length; i++)
        {
            array[i] = random.Next(min, max);
        }

        return array;
    }

    static int[] InputArraySeparateLines()
    {
        Console.Write("Введіть кількість елементів масиву: ");
        int length = int.Parse(Console.ReadLine());

        int[] array = new int[length];

        Console.WriteLine("Введіть елементи масиву по одному у кожному рядку:");
        for (int i = 0; i < length; i++)
        {
            Console.Write($"Елемент {i + 1}: ");
            array[i] = int.Parse(Console.ReadLine());
        }

        return array;
    }

    static int[] InputArraySingleLine()
    {
        Console.Write("Введіть елементи масиву, розділені пробілами або табуляціями: ");
        string[] input = Console.ReadLine().Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

        int[] array = new int[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            array[i] = int.Parse(input[i]);
        }

        return array;
    }

    static void PrintArray(int[] array)
    {
        Console.WriteLine(string.Join(", ", array));
    }

    static int FindMinIndex(int[] array)
    {
        int minIndex = 0;
        int minValue = array[0];

        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] < minValue)
            {
                minValue = array[i];
                minIndex = i;
            }
        }

        return minIndex;
    }

    static int FindMaxIndex(int[] array)
    {
        int maxIndex = 0;
        int maxValue = array[0];

        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] > maxValue)
            {
                maxValue = array[i];
                maxIndex = i;
            }
        }

        return maxIndex;
    }

    static void Swap(int[] array, int index1, int index2)
    {
        int temp = array[index1];
        array[index1] = array[index2];
        array[index2] = temp;
    }
    static void DoBlock_2()
    {
        Console.WriteLine("Введіть натуральне число n:");
        int n = Convert.ToInt32(Console.ReadLine());

        // Вимірювання пам'яті для пункту а)
        long memoryBefore = GC.GetTotalMemory(true);
        int[][] jaggedArray = CreateJaggedArray(n);
        long memoryAfter = GC.GetTotalMemory(true);
        long memoryUsedA = memoryAfter - memoryBefore;

        Console.WriteLine("Пункт а):");
        PrintJaggedArray2(jaggedArray);

        // Вимірювання пам'яті для пункту б)
        memoryBefore = GC.GetTotalMemory(true);
        var optimizedJaggedArray = CreateOptimizedJaggedArray(n);
        memoryAfter = GC.GetTotalMemory(true);
        long memoryUsedB = memoryAfter - memoryBefore;

        Console.WriteLine("Пункт б):");
        PrintOptimizedJaggedArray(n, optimizedJaggedArray);

        // Виведення використаної пам'яті
        Console.WriteLine("Використання пам'яті в пункті а): " + memoryUsedA + " байт");
        Console.WriteLine("Використання пам'яті в пункті б): " + memoryUsedB + " байт");
    }
    // Функція для обчислення суми цифр числа
    static int SumOfDigits(int number)
    {
        int sum = 0;
        while (number > 0)
        {
            sum += number % 10;
            number /= 10;
        }
        return sum;
    }

    // Пункт а): Створення зубчастого масиву
    static int[][] CreateJaggedArray(int n)
    {
        int[][] jaggedArray = new int[n][];
        for (int i = 0; i < n; i++)
        {
            int sumDigits = SumOfDigits(i);
            if (sumDigits == 0)
            {
                jaggedArray[i] = new int[] { 0 };
                continue;
            }

            List<int> multiples = new List<int>();
            for (int j = 1; j <= n; j++)
            {
                if (j % sumDigits == 0)
                {
                    multiples.Add(j);
                }
            }
            jaggedArray[i] = multiples.ToArray();
        }
        return jaggedArray;
    }

    // Пункт б): Створення зубчастого масиву з оптимізацією пам'яті
    static Dictionary<int, int[]> CreateOptimizedJaggedArray(int n)
    {
        Dictionary<int, List<int>> sequences = new Dictionary<int, List<int>>();
        for (int i = 0; i < n; i++)
        {
            int sumDigits = SumOfDigits(i);
            if (sumDigits == 0) continue;

            if (!sequences.ContainsKey(sumDigits))
            {
                sequences[sumDigits] = new List<int>();
                for (int j = 1; j <= n; j++)
                {
                    if (j % sumDigits == 0)
                    {
                        sequences[sumDigits].Add(j);
                    }
                }
            }
        }

        Dictionary<int, int[]> optimizedJaggedArray = new Dictionary<int, int[]>();
        foreach (var entry in sequences)
        {
            optimizedJaggedArray[entry.Key] = entry.Value.ToArray();
        }

        return optimizedJaggedArray;
    }

    // Виведення зубчастого масиву
    static void PrintJaggedArray2(int[][] jaggedArray)
    {
        for (int i = 0; i < jaggedArray.Length; i++)
        {
            Console.Write(i + ": ");
            foreach (var num in jaggedArray[i])
            {
                Console.Write(num + " ");
            }
            Console.WriteLine();
        }
    }

    // Виведення оптимізованого зубчастого масиву
    static void PrintOptimizedJaggedArray(int n, Dictionary<int, int[]> optimizedJaggedArray)
    {
        for (int i = 0; i < n; i++)
        {
            int sumDigits = SumOfDigits(i);
            if (sumDigits == 0)
            {
                Console.WriteLine(i + ": 0");
                continue;
            }
            if (optimizedJaggedArray.ContainsKey(sumDigits))
            {
                Console.Write(i + ": ");
                foreach (var num in optimizedJaggedArray[sumDigits])
                {
                    Console.Write(num + " ");
                }
                Console.WriteLine();
            }
        }
    }

    static void DoBlock_3()
    {
        int[][] jaggedArray = InputJaggedArray();

        int maxRowIndex = FindRowWithMaxElement(jaggedArray);

        jaggedArray = AddRowAfterMaxRow(jaggedArray, maxRowIndex);

        Console.WriteLine("\nЗубчастий масив після додавання рядка:");
        PrintJaggedArray(jaggedArray);
    }

    static int[][] ReadJaggedArrayManually()
    {
        Console.WriteLine("Введіть розмірність зубчастого масиву (кількість рядків):");
        int rows = int.Parse(Console.ReadLine());

        int[][] jaggedArray = new int[rows][];

        for (int i = 0; i < rows; i++)
        {
            Console.WriteLine($"Введіть рядок {i + 1} (елементи через пробіл):");
            string[] elements = Console.ReadLine().Split(' ');
            jaggedArray[i] = Array.ConvertAll(elements, int.Parse);
        }

        return jaggedArray;
    }

    static int[][] GenerateRandomJaggedArray()
    {
        Random random = new Random();

        Console.WriteLine("Введіть мінімальне значення:");
        int minValue = int.Parse(Console.ReadLine());

        Console.WriteLine("Введіть максимальне значення:");
        int maxValue = int.Parse(Console.ReadLine());

        Console.WriteLine("Введіть кількості елементів кожного рядка через пробіл:");
        string[] elementsCountsStr = Console.ReadLine().Split(' ');

        int rows = elementsCountsStr.Length;
        int[][] jaggedArray = new int[rows][];

        for (int i = 0; i < rows; i++)
        {
            int elementsCount = int.Parse(elementsCountsStr[i]);
            jaggedArray[i] = new int[elementsCount];

            for (int j = 0; j < elementsCount; j++)
            {
                jaggedArray[i][j] = random.Next(minValue, maxValue + 1);
            }
        }

        return jaggedArray;
    }

    static int FindRowWithMaxElement(int[][] jaggedArray)
    {
        int maxRowIndex = 0;
        int maxElement = jaggedArray[0][0];

        for (int i = 0; i < jaggedArray.Length; i++)
        {
            for (int j = 0; j < jaggedArray[i].Length; j++)
            {
                if (jaggedArray[i][j] > maxElement)
                {
                    maxElement = jaggedArray[i][j];
                    maxRowIndex = i;
                }
            }
        }

        return maxRowIndex;
    }

    static int[][] AddRowAfterMaxRow(int[][] jaggedArray, int maxRowIndex)
    {
        int[][] newArray = new int[jaggedArray.Length + 1][];

        Array.Copy(jaggedArray, 0, newArray, 0, maxRowIndex + 1);

        newArray[maxRowIndex + 1] = new int[] { 0 };

        Array.Copy(jaggedArray, maxRowIndex + 1, newArray, maxRowIndex + 2, jaggedArray.Length - maxRowIndex - 1);

        return newArray;
    }

    static void PrintJaggedArray(int[][] jaggedArray)
    {
        for (int i = 0; i < jaggedArray.Length; i++)
        {
            for (int j = 0; j < jaggedArray[i].Length; j++)
            {
                Console.Write(jaggedArray[i][j] + " ");
            }
            Console.WriteLine();
        }
    }
    static int[][] InputJaggedArray()
    {
        int[][] jaggedArray;

        Console.WriteLine("Оберіть спосіб введення зубчастого масиву:");
        Console.WriteLine("1. Вручну.");
        Console.WriteLine("2. Випадково введені значення.");

        int choice = int.Parse(Console.ReadLine());

        if (choice == 1)
            jaggedArray = ReadJaggedArrayManually();
        else
            jaggedArray = GenerateRandomJaggedArray();

        Console.WriteLine("\nВаш зубчастий масив:");
        PrintJaggedArray(jaggedArray);

        return jaggedArray;
    }

    static void DoBlock_4()
    {
        int[][] jaggedArray = InputJaggedArray();

        // Переписати з кожного рядка зубчастого массива P у відповідні рядки зубчастого массива Q лише непарні елементи.
        int[][] jaggedArrayQ = ExtractOddElements(jaggedArray);

        // Відсортувати (методом вибору) рядки отриманого зубчастого массива Q за зростанням.
        SortJaggedArrayRows(jaggedArrayQ);

        Console.WriteLine("\nЗубчастий масив Q з непарними елементами:");
        PrintJaggedArray(jaggedArrayQ);

        // Сформувати з рядків зубчастого массива Q одновимірний масив
        int[] oneDimensionalArray = FlattenJaggedArray(jaggedArrayQ);

        Console.WriteLine("\nОдновимірний масив:");
        Console.WriteLine(string.Join(" ", oneDimensionalArray));

        // Визначити ті його елементи, значення яких співпадають із власним індексом
        List<int> matchingElements = FindElementsMatchingIndices(oneDimensionalArray);

        Console.WriteLine("\nЕлементи, значення яких співпадають із власним індексом:");
        Console.WriteLine(string.Join(" ", matchingElements));
    }

    static int[][] ExtractOddElements(int[][] jaggedArray)
    {
        int[][] result = new int[jaggedArray.Length][];

        for (int i = 0; i < jaggedArray.Length; i++)
        {
            List<int> oddElements = new List<int>();
            for (int j = 0; j < jaggedArray[i].Length; j++)
            {
                if (jaggedArray[i][j] % 2 != 0)
                {
                    oddElements.Add(jaggedArray[i][j]);
                }
            }
            result[i] = oddElements.ToArray();
        }

        return result;
    }

    static void SortJaggedArrayRows(int[][] jaggedArray)
    {
        for (int i = 0; i < jaggedArray.Length; i++)
        {
            SelectionSort(jaggedArray[i]);
        }
    }

    static void SelectionSort(int[] array)
    {
        for (int i = 0; i < array.Length - 1; i++)
        {
            int minIndex = i;
            for (int j = i + 1; j < array.Length; j++)
            {
                if (array[j] < array[minIndex])
                {
                    minIndex = j;
                }
            }
            int temp = array[minIndex];
            array[minIndex] = array[i];
            array[i] = temp;
        }
    }

    static int[] FlattenJaggedArray(int[][] jaggedArray)
    {
        List<int> list = new List<int>();
        foreach (var array in jaggedArray)
        {
            list.AddRange(array);
        }
        return list.ToArray();
    }

    static List<int> FindElementsMatchingIndices(int[] array)
    {
        List<int> matchingElements = new List<int>();
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == i)
            {
                matchingElements.Add(array[i]);
            }
        }
        return matchingElements;
    }

    static void Main(string[] args)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        Console.OutputEncoding = UTF8Encoding.UTF8;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.BackgroundColor = ConsoleColor.White;
        Console.Clear();
        int choice;
        do
        {
            Console.WriteLine("-Для виконання блоку 1 (11. Вставити в початок масиву мінімум з усіх значень масиву, а в кінець — максимум введіть 1");
            Console.WriteLine("-Для виконання блоку 2 введіть 2");
            Console.WriteLine("-Для виконання блоку 3 (11. Додати рядок після рядка, що містить максимальний елемент (якщо у різних місцях є кілька елементів з однаковим максимальним значенням, то брати перший з них) введіть 3");
            Console.WriteLine("-Для виконання блоку 4 (11. Переписати з кожного рядка матриці P у відповідні рядки матриці Q лише непарні елементи.Відсортувати рядки отриманої матриці Q за зростанням. Сформувати з рядків матриці Q одновимірний масив та визначити ті його елементи, значення яких співпадають із власним індексом) введіть 4");
            Console.WriteLine("-Для виходу з програми введіть 0");

            choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.WriteLine("Виконую блок 1");
                    DoBlock_1();
                    break;
                case 2:
                    Console.WriteLine("Виконую блок 2");
                    DoBlock_2();
                    break;
                case 3:
                    Console.WriteLine("Виконую блок 3");
                    DoBlock_3();
                    break;
                case 4:
                    Console.WriteLine("Виконую блок 4");
                    DoBlock_4();
                    break;
                case 0:
                    Console.WriteLine("Зараз завершимо, тільки натисніть будь ласка ще раз Enter");
                    Console.ReadLine();
                    break;
                default:
                    Console.WriteLine("Команда ``{0}'' не розпізнана. Зробіь, будь ласка, вибір із 1, 2, 3, 4, 0.", choice);
                    break;
            }
        } while (choice != 0);
    }
}