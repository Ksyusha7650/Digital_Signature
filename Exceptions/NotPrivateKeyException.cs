using System;

namespace Digital_Signature;

public class NotPrivateKeyException : Exception
{
    public NotPrivateKeyException()
    {
        Message = "Неправильный приватный ключ";
    }

    public override string Message { get; }
}