using System;

namespace Digital_Signature.Exceptions;

public class NotPublicKeyException : Exception
{
    public NotPublicKeyException()
    {
        Message = "Неправильный открытый ключ!";
    }

    public override string Message { get; }
}