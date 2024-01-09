using System.Text;

namespace RSV;

public static class RSVConstants
{
    public static readonly UTF8Encoding Encoding = new(false, true);
    public const byte ROWTERMINATOR = 0xFD;
    public const byte NULLVALUE = 0xFE;
    public const byte VALUETERMINATOR = 0xFF;
}