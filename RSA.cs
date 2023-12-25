using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace Digital_Signature;

public class RSA : IDisposable
{
    private readonly long _m;
    private readonly long _n;
    public readonly string PublicKey;
    public readonly string PrivateKey;
    private readonly long _E, _D;

    public RSA()
    {
        long p = AlgorithmExtensions.GeneratePrimeNumber(10, 100);
        long q = AlgorithmExtensions.GeneratePrimeNumber(10, 100);
        _n = p * q;
        _m = (p - 1) * (q - 1);
        _E = CalculateE();
        _D = CalculateD();
        PublicKey = _E + "\n" + _n;
        PrivateKey = _D + "\n" + _n;
    }
    
    public RSA(long parameter, long n, bool isPublic = true)
    {
        if (!isPublic)
            _E = parameter;
        else
            _D = parameter;
        _n = n;
        
    }

    private long CalculateD()
    {
        long d = 10;

        while (true)
        {
            if (d * _E % _m == 1)
                break;
            else
                d++;
        }

        return d;
    }
    
    private long CalculateE()
    {
        var e = 3;

        for (var i = 1; i < _m; i++)
        {
            if (_m % i != 0 || e % i != 0) continue;
            e++;
            i = 1;
        }

        return e;
    }
    
    public string Encrypt(string s)
    {
        var list = (from index in s.Select(t => t)
            select new BigInteger(index)
            into xi
            select BigInteger.Pow(xi, (int)_E)
            into ci
            select (ci % _n).ToString()).ToList();

        return list.Aggregate("", (current, ci) =>
        {
            var delimiter = (current is not "") ? "-" : "";
            return current + delimiter + ci;
        });

    }
    
    public string Decrypt(string s)
    {
        var list = (from index in s.Split('-')
            select new BigInteger(long.Parse(index))
            into ci
            select BigInteger.Pow(ci, (int)_D)
            into pi
            select pi % _n
            into pi
            select Convert.ToChar((long)pi)).ToList();

        return list.Aggregate("", (current, pi) => current + pi);
    }
    
    private string RSA_Dedoce(List<string> input, long d, long n)
    {
        string result = "";

        BigInteger bi;

        foreach (var item in input)
        {
            bi = new BigInteger(Convert.ToDouble(item));
            bi = BigInteger.Pow(bi, (int)d);

            BigInteger n_ = new BigInteger((int)n);

            bi = bi % n_;

            int index = Convert.ToInt32(bi.ToString());

            //result += characters[index].ToString();
        }

        return result;
    }

    public void Dispose() { }
}