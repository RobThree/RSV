using RSV.Resources;
using System;
using System.Collections.Generic;
using System.IO;

namespace RSV;

public class RSVReader(Stream stream)
{
    private readonly MemoryStream _buffer = new(4096);
    private readonly Stream _stream = stream ?? throw new ArgumentNullException(nameof(stream));

    public IEnumerable<string?[]> Read()
    {
        var rowdata = new List<string?>(50);
        int lastchar;
        var c = _stream.ReadByte();
        _buffer.SetLength(0);
        while (c >= 0)
        {
            if (c == RSVConstants.VALUETERMINATOR)
            {
                rowdata.Add(RSVConstants.Encoding.GetString(_buffer.ToArray()));
                _buffer.SetLength(0);
            }
            else if (c == RSVConstants.NULLVALUE)
            {
                if (_buffer.Length > 0)
                {
                    throw new RSVException(Translations.UNEXPECTEDNULLTERMINATOR, _stream.Position);
                }
                c = _stream.ReadByte();
                if (c == RSVConstants.VALUETERMINATOR)
                {
                    rowdata.Add(null);
                }
                else
                {
                    throw new RSVException(Translations.EXPECTEDROWTERMINATOR, _stream.Position);
                }
            }
            else if (c == RSVConstants.ROWTERMINATOR)
            {
                yield return rowdata.ToArray();
                rowdata.Clear();
            }
            else
            {
                _buffer.WriteByte((byte)c);
            }
            lastchar = c;
            c = _stream.ReadByte();
            if (c < 0 && lastchar != RSVConstants.ROWTERMINATOR)
            {
                throw new RSVException(Translations.UNEXPECTEDENDOFFILE, _stream.Position);
            }
        }
        if (_buffer.Length > 0)
        {
            throw new RSVException(Translations.EXPECTEDVALUETERMINATOR, _stream.Position);
        }
    }
}