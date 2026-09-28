class Program
{
    static void Main()
    {
        Console.Write("Введите количество элементов K: ");
        int k = System.Convert.ToInt32(Console.ReadLine());
        string alp = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
        string glas = "аеёиоуыэюя";
        char[] arr = new char[k];
        Random rnd = new Random();
        for (int i = 0; i < k; i++)
        {
            arr[i] = alp[rnd.Next(alp.Length)];
        }
        Console.WriteLine("\nИсходный массив:");
        for (int i = 0; i < k; i++)
        {
            Console.Write(arr[i] + " ");
        }
        Console.WriteLine();
        int soglcount = 0;
        for (int i = 0; i < k; i++)
        {
            if (glas.IndexOf(arr[i]) == -1 && arr[i] != 'ь' && arr[i] != 'ъ')
                soglcount++;
        }
        char[] sogl = new char[soglcount];
        int j = 0;
        for (int i = 0; i < k; i++)
        {
            if (glas.IndexOf(arr[i]) == -1 && arr[i] != 'ь' && arr[i] != 'ъ')
            {
                sogl[j] = arr[i];
                j++;
            }
        }
        Console.WriteLine("\nМассив согласных букв:");
        for (int i = 0; i < sogl.Length; i++)
        {
            Console.Write(sogl[i] + " ");
        }
        Console.WriteLine();
    }
}