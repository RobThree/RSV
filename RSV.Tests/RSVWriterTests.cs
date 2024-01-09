namespace RSV.Tests;

[TestClass]
public class RSVWriterTests
{
    [TestMethod]
    public void WritesFilesCorrectly()
    {
        var reader = new RSVReader();
        var target = new RSVWriter();
        foreach (var file in Directory.GetFiles("Testfiles/Valid/", "*.rsv"))
        {
            var checkfilename = file.Replace(".rsv", ".check");
            using var rsvfile = File.OpenRead(file);
            using var checkfile = File.Create(checkfilename);
            target.Write(checkfile, reader.Read(rsvfile));
            rsvfile.Dispose();
            checkfile.Dispose();

            Assert.IsTrue(Enumerable.SequenceEqual(File.ReadAllBytes(file), File.ReadAllBytes(checkfilename)), $"File {file} wrote incorrectly");
        }
    }

    [TestMethod]
    public async Task WritesFilesAsyncCorrectly()
    {
        var reader = new RSVReader();
        var target = new RSVWriter();
        foreach (var file in Directory.GetFiles("Testfiles/Valid/", "*.rsv"))
        {
            var checkfilename = file.Replace(".rsv", ".checkasync");
            using var rsvfile = File.OpenRead(file);
            using var checkfile = File.Create(checkfilename);
            await target.WriteAsync(checkfile, reader.Read(rsvfile)).ConfigureAwait(false);
            rsvfile.Dispose();
            checkfile.Dispose();

            Assert.IsTrue(Enumerable.SequenceEqual(File.ReadAllBytes(file), File.ReadAllBytes(checkfilename)), $"File {file} wrote incorrectly");
        }
    }
}