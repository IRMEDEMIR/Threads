using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

//211229040_İrem_DEMİR
class Program
{
    static List<int> numberList = new List<int>(); //bütün sayılar
    static List<int> primeNumbers = new List<int>();  //asal sayılar
    static List<int> evenNumbers = new List<int>();  //çift sayılar
    static List<int> oddNumbers = new List<int>();  //tek sayılar

    static void Main(string[] args)
    {

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        // 1'den 1.000.000'a kadar sayıları listeye ekle
        for (int i = 1; i <= 1000000; i++)
        {
            numberList.Add(i);
        }

        // 4 eşit parçaya bölecek bir fonksiyon yaz
        List<List<int>> dividedLists = DivideList(numberList, 4);  //bölünmüş liste

        // Her bir parça için bir Thread oluştur
        List<Thread> threads = new List<Thread>();
        Thread t = new Thread(() => FindPrimes(dividedLists[0]));
        threads.Add(t);
        t.Start();
        //t.Join();
        Thread k = new Thread(() => FindPrimes(dividedLists[1]));
        threads.Add(k);
        k.Start();
       //k.Join();
        Thread l = new Thread(() => FindPrimes(dividedLists[2]));
        threads.Add(l);
        l.Start();
        //l.Join();
        Thread m = new Thread(() => FindPrimes(dividedLists[3]));
        threads.Add(m);
        m.Start();
        //m.Join();

        Console.WriteLine("Asal sayılar: ");
        List<int> prime = primeNumbers.Take(50).ToList();
        foreach (int number in prime)
        {
            Console.Write(number + ", ");
        }

        Console.WriteLine("\n-------------------------------");

        Console.WriteLine("Çift sayılar: ");
        List<int> even = evenNumbers.Take(50).ToList();
        foreach (int number in even)
        {
            Console.Write(number + ", ");
        }

        Console.WriteLine("\n-------------------------------");

        Console.WriteLine("Tek sayılar: ");
        List<int> odd = oddNumbers.Take(50).ToList();
        foreach (int number in odd)
        {
            Console.Write(number + ", ");
        }

        Console.WriteLine("İşlem tamamlandı.");
        stopwatch.Stop();
        Console.WriteLine($"İşlem süresi: {stopwatch.Elapsed.TotalSeconds} saniye");
    }

    //dörde bölünmüş bir liste oluşturmak için
    static List<List<int>> DivideList(List<int> list, int parts)
    {
        List<List<int>> dividedLists = new List<List<int>>();
        int chunkSize = list.Count / parts;
        int remainder = list.Count % parts; // Kalan elemanları son parçaya eklemek için

        int index = 0;
        for (int i = 0; i < parts; i++)
        {
            int size = chunkSize + (i < remainder ? 1 : 0); // Kalan elemanları son parçaya ekle
            dividedLists.Add(list.GetRange(index, size));
            index += size;
        }

        //int listCount = dividedLists.Count;  //kaç alt liste var?
        //Console.WriteLine(listCount);

        /*foreach (int number in dividedLists[0]) //numberlistin ilk sublistini gösterir
        {
            Console.Write(number + ", ");
        }*/

        return dividedLists;
    }

    // İlk parçaya erişim:
    //List<int> firstPart = dividedLists[0];

    // İkinci parçaya erişim:
    //List<int> secondPart = dividedLists[1];

    // Üçüncü parçaya erişim:
    //List<int> thirdPart = dividedLists[1];

    // Son parçaya erişim (parça sayısı 4 olarak belirlendiği için 3. indeks):
    //List<int> lastPart = dividedLists[3];


    //sayıları ayrıştırıyor
    static void FindPrimes(List<int> subList)
    {
        foreach (int number in subList)
        {
            if (IsPrime(number))  //asal mı bakıyo
            {
                lock (primeNumbers)
                {
                    primeNumbers.Add(number);
                }
            }
            else if (number % 2 == 0)  //asal değilse çift mi diye bakıyo 
            {
                lock (evenNumbers)
                {
                    evenNumbers.Add(number);
                }
            }
            else
            {
                lock (oddNumbers)  //asal ve çift değilse direkt tek listesine ekliyor
                {
                    oddNumbers.Add(number);
                }
            }
        }
    }

    //asal sayıları buluyor
    static bool IsPrime(int number)  
    {
        if (number <= 1)
        {
            return false;
        }
        for (int i = 2; i * i <= number; i++)
        {
            if (number % i == 0)
            {
                return false;
            }
        }
        return true;
    }
}
