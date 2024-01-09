using RSV.Resources;
using System.Collections.Generic;
using System.IO;

namespace RSV;

public class RSVReader : RSVBase
{
    private readonly MemoryStream _buffer = new();

    public IEnumerable<string?[]> Read(Stream stream)
    {
        var rowdata = new List<string?>();
        int lastchar;
        var c = stream.ReadByte();
        _buffer.SetLength(0);
        while (c >= 0)
        {
            if (c == VALUETERMINATOR)
            {
                rowdata.Add(Encoding.GetString(_buffer.ToArray()));
                _buffer.SetLength(0);
            }
            else if (c == NULLVALUE)
            {
                if (_buffer.Length > 0)
                {
                    throw new RSVException(Translations.UNEXPECTEDNULLTERMINATOR, stream.Position);
                }
                c = stream.ReadByte();
                if (c == VALUETERMINATOR)
                {
                    rowdata.Add(null);
                }
                else
                {
                    throw new RSVException(Translations.EXPECTEDROWTERMINATOR, stream.Position);
                }
            }
            else if (c == ROWTERMINATOR)
            {
                yield return rowdata.ToArray();
                rowdata.Clear();
            }
            else
            {
                _buffer.WriteByte((byte)c);
            }
            lastchar = c;
            c = stream.ReadByte();
            if (c < 0 && lastchar != ROWTERMINATOR)
            {
                throw new RSVException(Translations.UNEXPECTEDENDOFFILE, stream.Position);
            }
        }
        if (_buffer.Length > 0)
        {
            throw new RSVException(Translations.EXPECTEDVALUETERMINATOR, stream.Position);
        }
    }
}