using NUnit.Framework;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace LPSoft.NameGenerator.Tests
{
    public class NameGeneratorBuilderTests
    {
        [TestCase("")]
        [TestCase(null)]
        [TestCase("         ")]
        [TestCase("./valid/path.notjson")]

        public void FromJsonFile_GivenInvalidFilePath_ThrowsArgumentException(string path)
        {
            var builder = new NameGeneratorBuilder();
            Assert.That(() => builder.FromJsonFile(path), Throws.ArgumentException.With.Message.EqualTo("Invalid path provided."));
        }

        [Test]
        public void FromJsonFile_GivenMissingFile_ThrowsFileNotFoundException()
        {
            var filepath = "./test.json";

            var builder = new NameGeneratorBuilder();
            Assert.That(async () => await builder.FromJsonFile(filepath), Throws.TypeOf<FileNotFoundException>());
        }

        [Test]
        public async Task FromJsonFile_GivenInvalidJson_ThrowsSerialisationException()
        {
            var filepath = "./test2.json";
            var invalidJson = "{not valid json}";

            using (var tw = File.CreateText(filepath))
            {
                await tw.WriteAsync(invalidJson);
            }

            var builder = new NameGeneratorBuilder();
            Assert.That(() => builder.FromJsonFile(filepath), Throws.TypeOf<JsonException>());

            File.Delete(filepath);
        }

        [Test]
        public async Task FromJsonFile_GivenValidJson_ReturnsNameGeneratorBuilder()
        {
            var filepath = "./test3.json";
            var invalidJson = "{\"test\":[\"test1\", \"test2\"]}";

            using (var tw = File.CreateText(filepath))
            {
                await tw.WriteAsync(invalidJson);
            }

            var builder = new NameGeneratorBuilder();
            var result = await builder.FromJsonFile(filepath);

            Assert.That(result, Is.TypeOf<NameGeneratorBuilder>());

            File.Delete(filepath);
        }

        [Test]
        public async Task FromJsonFile_GivenValidJsonAddedTwice_ThrowsArgumentException()
        {
            var filepath = "./test3.json";
            var invalidJson = "{\"test\":[\"test1\", \"test2\"]}";

            using (var tw = File.CreateText(filepath))
            {
                await tw.WriteAsync(invalidJson);
            }

            var builder = new NameGeneratorBuilder();
            var result = await builder.FromJsonFile(filepath);

            Assert.That(() => builder.FromJsonFile(filepath), Throws.ArgumentException);

            File.Delete(filepath);
        }

        [Test]
        public void Build_GivenNoData_ReturnsNameGeneratorWithNoData()
        {
            var builder = new NameGeneratorBuilder();

            var result = builder.Build();

            Assert.That(result.AvailableData, Is.Empty);
        }

        [Test]
        public async Task Build_GivenFromJsonFile_ReturnsNameGeneratorWithData()
        {
            var filepath = "./test4.json";
            var invalidJson = "{\"test\":[\"test1\", \"test2\"]}";

            using (var tw = File.CreateText(filepath))
            {
                await tw.WriteAsync(invalidJson);
            }

            var builder = new NameGeneratorBuilder();
            var result = (await builder.FromJsonFile(filepath)).Build();

            Assert.That(result.AvailableData, Is.Not.Empty);
        }
    }
}