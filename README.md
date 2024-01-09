# RSV

This library, [available on NuGet](https://www.nuget.org/packages/RSV), provides a simple way to parse and generate [RSV (Row of String Values) files](https://github.com/Stenway/RSV-Specification). This library reads / writes in a streaming way so memory usage is minimal.

## Quickstart

### Reading RSV file:

```c#
using var stream = File.OpenRead("test.rsv");
var reader = new RsvReader(stream);

// Dump data
foreach (var row in reader.Read()) {
	Console.WriteLine(string.Join("\t", row));
}
```

### Writing RSV file:

```c#

using var stream = File.Create("test.rsv");
var writer = new RsvWriter(stream);
string[][] data = [
    ["A", "B", "C"],
    ["1", "2", "3"]
];

// Write data
writer.Write(data);

// ...or write data async (you can optionally pass a CancellationToken):
await writer.WriteAsync(data);
```

## Attibution

Logo based on [CSV Icon by Freepik](https://www.freepik.com/icon/csv_6133923).