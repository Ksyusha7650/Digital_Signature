using System;

namespace Digital_Signature.Exceptions;

public class NotPrivateKeyException : Exception
{
    public NotPrivateKeyException()
    {
        Message = "Неправильный закрытый ключ!";
    }

    public override string Message { get; }
}