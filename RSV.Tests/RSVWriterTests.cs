namespace RSV.Tests;

[TestClass]
public class RSVWriterTests
{
    [TestMethod]
    public void WritesFilesCorrectly()
    {
        foreach (var file in Directory.GetFiles("Testfiles/Valid/", "*.rsv"))
        {
            var checkfilename = file.Replace(".rsv", ".check");
            using var rsvstream = File.OpenRead(file);
            var reader = new RSVReader(rsvstream);

            using var outstream = File.Create(checkfilename);
            var target = new RSVWriter(outstream);

            target.Write(reader.Read());
            rsvstream.Dispose();
            outstream.Dispose();

            Assert.IsTrue(Enumerable.SequenceEqual(File.ReadAllBytes(file), File.ReadAllBytes(checkfilename)), $"File {file} wrote incorrectly");
        }
    }

    [TestMethod]
    public async Task WritesFilesAsyncCorrectly()
    {
        foreach (var file in Directory.GetFiles("Testfiles/Valid/", "*.rsv"))
        {
            var checkfilename = file.Replace(".rsv", ".checkasync");
            using var rsvstream = File.OpenRead(file);
            var reader = new RSVReader(rsvstream);

            using var outstream = File.Create(checkfilename);
            var target = new RSVWriter(outstream);

            await target.WriteAsync(reader.Read()).ConfigureAwait(false);
            rsvstream.Dispose();
            outstream.Dispose();

            Assert.IsTrue(Enumerable.SequenceEqual(File.ReadAllBytes(file), File.ReadAllBytes(checkfilename)), $"File {file} wrote incorrectly");
        }
    }
}