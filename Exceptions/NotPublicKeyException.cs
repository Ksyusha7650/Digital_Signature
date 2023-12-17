using System;

namespace Digital_Signature;

public class NotPublicKeyException : Exception
{
    public NotPublicKeyException()
    {
        Message = "Неправильный публичный ключ";
    }

    public override string Message { get; }
}