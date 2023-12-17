using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Digital_Signature;

internal abstract class AlgorithmCrypto
{
    public static void MakeKeys()
    {
        using var rsa = new RSACryptoServiceProvider();
        var privateKey = rsa.ExportRSAPrivateKey();
        var saveFileDialog = Extensions.SaveFileDialog();
        if (saveFileDialog.ShowDialog() != true) return;
        var savePath = Path.GetDirectoryName(saveFileDialog.FileName);
        var streamWriter = File.CreateText(savePath + "\\private_key.pem");
        streamWriter.Write(Convert.ToBase64String(privateKey));
        streamWriter.Close();
        var publicKey = rsa.ExportRSAPublicKey();
        streamWriter = File.CreateText(savePath + "\\public_key.pem");
        streamWriter.Write(Convert.ToBase64String(publicKey));
        streamWriter.Close();
        MessageBox.Show("Ключи созданы!");
    }

    internal static string Crypt(string content, string privateKey)
    {
        using var rsa = new RSACryptoServiceProvider();
        var key = Convert.FromBase64String(privateKey);
        try
        {
            rsa.ImportRSAPrivateKey(key, out _);
        }
        catch
        {
            throw new NotPrivateKeyException();
        }

        var hash = ComputeHashContent(content);
        var signature = rsa.SignHash(hash, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
        return Convert.ToBase64String(signature);
    }

    public static bool Check(string content, string publicKey, string signature)
    {
        using var rsa = new RSACryptoServiceProvider();
        var key = Convert.FromBase64String(publicKey);
        try
        {
            rsa.ImportRSAPublicKey(key, out _);
        }
        catch
        {
            throw new NotPublicKeyException();
        }

        var hash = ComputeHashContent(content);
        var isSignatureValid = rsa.VerifyHash(
            hash, Convert.FromBase64String(signature), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);

        return isSignatureValid;
    }

    private static byte[] ComputeHashContent(string content)
    {
        using var sha1 = SHA1.Create();
        return sha1.ComputeHash(Encoding.UTF8.GetBytes(content));
    }
}