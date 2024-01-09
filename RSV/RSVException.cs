using System;

namespace RSV;

public class RSVException(string message, long position, Exception? innerException = null) : Exception(message, innerException)
{
    public long Position { get; } = position;
}
