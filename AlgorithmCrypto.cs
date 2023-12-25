using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Digital_Signature.Exceptions;

namespace Digital_Signature;

internal abstract class AlgorithmCrypto
{
    public static void MakeKeys()
    {
        using var rsa = new RSA();
        var privateKey = rsa.PrivateKey;
        var saveFileDialog = Extensions.SaveFileDialog();
        if (saveFileDialog.ShowDialog() != true) return;
        var savePath = Path.GetDirectoryName(saveFileDialog.FileName);
        var streamWriter = File.CreateText(savePath + "\\private_key.pem");
        streamWriter.Write(privateKey);
        streamWriter.Close();
        var publicKey = rsa.PublicKey;
        streamWriter = File.CreateText(savePath + "\\public_key.pem");
        streamWriter.Write(publicKey);
        streamWriter.Close();
        MessageBox.Show("Ключи созданы!");
    }



    internal static string Crypt(string content, string privateKey)
    {
        long E, n;
        try
        {
            var parameters = privateKey.Split("\n");
            E = long.Parse(parameters[0]);
            n = long.Parse(parameters[1]);
        }
        catch
        {
            throw new NotPrivateKeyException();
        }
        using var rsa = new RSA(E, n, false);
        var hash = ComputeHashContent(content);
        var signature = rsa.Encrypt(Convert.ToBase64String(hash));
        return signature;
    }

    public static bool Check(string content, string publicKey, string signature)
    {
        long D, n;
        try
        {
            var parameters = publicKey.Split("\n");
            D = long.Parse(parameters[0]);
            n = long.Parse(parameters[1]);
        }
        catch
        {
            throw new NotPublicKeyException();
        }
        using var rsa = new RSA(D, n);
        var hash = rsa.Decrypt(signature);
        var hash2 = ComputeHashContent(content);
        var hashStr = Convert.ToBase64String(hash2);
        return hash.SequenceEqual(hashStr);
    }

    private static byte[] ComputeHashContent(string content)
    {
        using var sha1 = SHA1.Create();
        return sha1.ComputeHash(Encoding.UTF8.GetBytes(content));
    }
}