using System.Text;

namespace RSV;

public class RSVBase
{
    protected readonly UTF8Encoding Encoding = new(false, true);
    protected const byte ROWTERMINATOR = 0xFD;
    protected const byte NULLVALUE = 0xFE;
    protected const byte VALUETERMINATOR = 0xFF;
}
