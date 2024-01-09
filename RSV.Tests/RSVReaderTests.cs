using System.Text.Json;

namespace RSV.Tests;

[TestClass]
public class RSVReaderTests
{
    [TestMethod]
    public void ReadsValidFilesCorrectly()
    {
        var target = new RSVReader();
        foreach (var file in Directory.GetFiles("Testfiles/Valid/", "*.rsv"))
        {
            using var rsvfile = File.OpenRead(file);
            using var checkfile = File.OpenRead(file.Replace(".rsv", ".json"));
            var data = target.Read(rsvfile).ToArray();

            var serialized = JsonSerializer.Serialize(data);
            var check = JsonSerializer.Serialize(JsonSerializer.Deserialize<object>(checkfile));
            Assert.AreEqual(check, serialized, $"File {file} deserialized incorrectly");
        }
    }

    [TestMethod]
    public void ThrowsOnInvalidFiles()
    {
        var target = new RSVReader();
        foreach (var file in Directory.GetFiles("Testfiles/Invalid/", "*.rsv"))
        {
            using var rsvfile = File.OpenRead(file);
            var thrown = false;

            try
            {
                var _ = target.Read(rsvfile).ToArray();
            }
            catch
            {
                thrown = true;
            }


            Assert.IsTrue(thrown, $"File {file} did not fail validation while it was expected to");
        }
    }
}
