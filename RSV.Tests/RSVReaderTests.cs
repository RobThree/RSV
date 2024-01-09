using System.Text.Json;

namespace RSV.Tests;

[TestClass]
public class RSVReaderTests
{
    [TestMethod]
    public void ReadsValidFilesCorrectly()
    {
        foreach (var file in Directory.GetFiles("Testfiles/Valid/", "*.rsv"))
        {
            using var rsvstream = File.OpenRead(file);
            using var checkfile = File.OpenRead(file.Replace(".rsv", ".json"));
            var target = new RSVReader(rsvstream);

            var data = target.Read().ToArray();

            var serialized = JsonSerializer.Serialize(data);
            var check = JsonSerializer.Serialize(JsonSerializer.Deserialize<object>(checkfile));
            Assert.AreEqual(check, serialized, $"File {file} deserialized incorrectly");
        }
    }

    [TestMethod]
    public void ThrowsOnInvalidFiles()
    {
        foreach (var file in Directory.GetFiles("Testfiles/Invalid/", "*.rsv"))
        {
            using var rsvstream = File.OpenRead(file);
            var target = new RSVReader(rsvstream);
            var thrown = false;

            try
            {
                var _ = target.Read().ToArray();
            }
            catch
            {
                thrown = true;
            }


            Assert.IsTrue(thrown, $"File {file} did not fail validation while it was expected to");
        }
    }
}
