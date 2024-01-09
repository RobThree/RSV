using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace RSV;

public class RSVWriter : RSVBase
{
    public void Write(Stream stream, IEnumerable<string?[]> rows)
    {
        foreach (var row in rows)
        {
            foreach (var value in row)
            {
                if (value == null)
                {
                    stream.WriteByte(NULLVALUE);
                }
                else
                {
                    stream.Write(Encoding.GetBytes(value));
                }
                stream.WriteByte(VALUETERMINATOR);
            }
            stream.WriteByte(ROWTERMINATOR);
        }
    }

    public async Task WriteAsync(Stream stream, IEnumerable<string?[]> rows, CancellationToken cancellationToken = default)
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
                    stream.WriteByte(NULLVALUE);
                }
                else
                {
                    await stream.WriteAsync(Encoding.GetBytes(value), cancellationToken).ConfigureAwait(false);
                }
                stream.WriteByte(VALUETERMINATOR);
            }
            stream.WriteByte(ROWTERMINATOR);
        }
    }
}
