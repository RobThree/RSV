using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace RSV;

public class RSVWriter(Stream stream)
{
    private readonly Stream _stream = stream ?? throw new ArgumentNullException(nameof(stream));

    public void Write(IEnumerable<string?[]> rows)
    {
        foreach (var row in rows)
        {
            foreach (var value in row)
            {
                if (value == null)
                {
                    _stream.WriteByte(RSVConstants.NULLVALUE);
                }
                else
                {
                    _stream.Write(RSVConstants.Encoding.GetBytes(value));
                }
                _stream.WriteByte(RSVConstants.VALUETERMINATOR);
            }
            _stream.WriteByte(RSVConstants.ROWTERMINATOR);
        }
    }

    public async Task WriteAsync(IEnumerable<string?[]> rows, CancellationToken cancellationToken = default)
    {
        foreach (var row in rows)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            foreach (var value in row)
            {
                if (value == null)
                {
                    _stream.WriteByte(RSVConstants.NULLVALUE);
                }
                else
                {
                    await _stream.WriteAsync(RSVConstants.Encoding.GetBytes(value), cancellationToken).ConfigureAwait(false);
                }
                _stream.WriteByte(RSVConstants.VALUETERMINATOR);
            }
            _stream.WriteByte(RSVConstants.ROWTERMINATOR);
        }
    }
}
