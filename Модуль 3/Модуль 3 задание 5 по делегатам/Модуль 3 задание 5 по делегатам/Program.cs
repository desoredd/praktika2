public static class Sort
{
    public static void ShelSort<T>(IList<T> list) where T : IComparable
    {
        int n = list.Count;
        int h = 1;
        while (h < (n >> 1))
        {
            h = (h << 1) + 1;
        }
        while (h >= 1)
        {
            for (int i = h; i < n; i++)
            {
                int k = i - h;
                for (int j = i; j >= h && list[j].CompareTo(list[k]) < 0; k -= h)
                {
                    T temp = list[j];
                    list[j] = list[k];
                    list[k] = temp;
                    j = k;
                }
            }
            h >>= 1;
        }
    }
    public static void BubbleSort(int[] list)
    {
        bool madeChanges;
        int itemCount = list.Length;
        do
        {
            madeChanges = false;
            itemCount--;
            for (int i = 0; i < itemCount; i++)
            {
                if (list[i].CompareTo(list[i + 1]) > 0)
                {
                    int temp = list[i + 1];
                    list[i + 1] = list[i];
                    list[i] = temp;
                    madeChanges = true;
                }
            }
        } while (madeChanges);
    }
    public static void gnomeSort(int[] anArray)
    {
        int first = 1;
        int second = 2;

        while (first < anArray.Length)
        {
            if (anArray[first - 1] <= anArray[first])
            {
                first = second;
                second++;
            }
            else
            {
                int tmp = anArray[first - 1];
                anArray[first - 1] = anArray[first];
                anArray[first] = tmp;
                first -= 1;
                if (first == 0)
                {
                    first = 1;
                    second = 2;
                }
            }

        }
    }
    public static void SelectionSort(int[] list)
    {
        int k;
        int temp;

        for (int i = 0; i < list.Length; i++)
        {
            k = i;
            for (int j = i + 1; j < list.Length; j++)
            {
                if (list[j].CompareTo(list[k]) < 0)
                {
                    k = j;
                }
            }
            temp = list[i];
            list[i] = list[k];
            list[k] = temp;
        }
    }
}
class Program
{
    public static void Pr(int[] arr)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            Console.Write($"{arr[i]} ");
        }
    }
    public static void Zap(int[] arr, Random random)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = random.Next(1, 100);
        }
    }
    static void Main()
    {
        Random random = new Random();
        Console.Write("Введите размерность массива: ");
        int N = System.Convert.ToInt32(Console.ReadLine());
        int[] arr = new int[N];
        Zap(arr, random);
        Pr(arr);
        bool running = true;
        Action<int[]>[] sorters =
        {
            Sort.ShelSort,
            Sort.BubbleSort,
            Sort.gnomeSort,
            Sort.SelectionSort
        };
        while (running)
        {
            Console.WriteLine("\nВыберите способ сортировки");
            Console.WriteLine("1. Shell Sort (сортировка шелла)");
            Console.WriteLine("2. Bubble Sort (пузырьковая сортировка)");
            Console.WriteLine("3. Gnome Sort (гномья сортировка)");
            Console.WriteLine("4. Selection Sort (сортировка выбором)");
            Console.WriteLine("5. Intelligent Design Sort (сортировка божественного замысла)");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите пункт: ");
            string choice = Console.ReadLine();
            Console.WriteLine();
            if (int.TryParse(choice, out int idx) && idx >= 1 && idx <= sorters.Length)
            {
                sorters[idx - 1](arr);   // вызов через делегат
                Pr(arr);
                Zap(arr, random);
            }
            else if (System.Convert.ToInt32(choice) == 5) Console.WriteLine("Массив отсортирован, просто вы не поняли как"); Pr(arr);
        }
    }
}