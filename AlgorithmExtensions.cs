using System;
using System.Collections.Generic;

namespace Digital_Signature;

public static class AlgorithmExtensions
{
    private static bool IsTheNumberSimple(long n)
    {
        switch (n)
        {
            case < 2:
                return false;
            case 2:
                return true;
        }

        for (long i = 2; i < n; i++)
            if (n % i == 0)
                return false;

        return true;
    }

    private static bool Ferma(long p)
    {
        var random = new Random();
        var k = random.Next();
        return Math.Abs((Math.Pow(k, p - 1)) % p - 1) < 1;
    }

    private static List<int> SieveOfEratosthenes(int upperLimit)
    {
        var isComposite = new bool[upperLimit + 1];

        for (var i = 2; i * i <= upperLimit; i++)
        {
            if (isComposite[i]) continue;
            for (var j = i * i; j <= upperLimit; j += i) isComposite[j] = true;
        }

        var primes = new List<int>();
        for (var i = 2; i <= upperLimit; i++)
            if (!isComposite[i])
                primes.Add(i);

        return primes;
    }

    public static int GeneratePrimeNumber(int lowerLimit, int upperLimit)
    {
        var primes = SieveOfEratosthenes(upperLimit);

        var random = new Random();
        int primeNumber;

        do
        {
            primeNumber = primes[random.Next(primes.Count)];
        }
        while (primeNumber < lowerLimit);

        return primeNumber;
    }
}