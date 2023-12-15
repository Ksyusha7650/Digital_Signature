using System;
using System.Security.Cryptography;
using System.Text;

namespace Digital_Signature
{
    internal class AlgorithmCrypting
    {
        internal static String Crypt(string content)
        {
            // Создаем объект для генерации ключа
            using var rsa = new RSACryptoServiceProvider();
            // Вычисляем хэш от содержимого файла
            var hash = ComputeHashContent(content);

            // Создаем цифровую подпись
            var signature = rsa.SignHash(hash, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);

            // Преобразуем подпись в строку
            return Convert.ToBase64String(signature);

            // Записываем подписью и содержимым в файл
            //File.WriteAllText("signedFile.txt", signatureString + Environment.NewLine + content);
        }

        public static bool Check(string content)
        {

            // Разделяем содержимое на подпись и оригинальное сообщение
            var parts = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            var signatureString = parts[0];
            var originalContent = parts[1];

            // Создаем объект для верификации
            using var rsa = new RSACryptoServiceProvider();
            // Парсим подпись из строки
            var signature = Convert.FromBase64String(signatureString);

            // Вычисляем хэш оригинального сообщения
            var hash = ComputeHashContent(originalContent);

            // Проверяем подпись
            var isSignatureValid = rsa.VerifyHash(hash, signature, HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);

            return isSignatureValid;
        }

        private static byte[] ComputeHashContent(string content)
        {
            using var sha1 = SHA1.Create();
            return sha1.ComputeHash(Encoding.UTF8.GetBytes(content));
        }
    }
}
