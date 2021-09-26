using NUnit.Framework;
using System;
using System.Collections.Generic;
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
            Assert.That(() => builder.FromJsonFile(filepath), Throws.TypeOf<FileNotFoundException>());
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
            var result = builder.FromJsonFile(filepath);

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
            var result = builder.FromJsonFile(filepath);

            Assert.That(() => result.FromJsonFile(filepath), Throws.ArgumentException);

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
            var result = builder.FromJsonFile(filepath).Build();

            Assert.That(result.AvailableData, Is.Not.Empty);
        }

        [Test]
        public void FromDictionary_GivenNullDictionary_ThrowsArgumentException()
        {
            var builder = new NameGeneratorBuilder();
            Assert.That(() => builder.FromDictionary(null), Throws.ArgumentException.With.Message.EqualTo("Dictionary cannot be null."));
        }

        [Test]
        public void FromDictionary_GivenEmptyDictionary_ReturnsNameGeneratorBuilder()
        {
            var builder = new NameGeneratorBuilder();
            var result = builder.FromDictionary(new Dictionary<string, string[]>());

            Assert.That(result, Is.TypeOf<NameGeneratorBuilder>());
        }

        [Test]
        public void Build_GivenFromDictionary_ReturnsNameGeneratorWithData()
        {
            var data = new Dictionary<string, string[]>();
            data.Add("test", new[] { "Test 1", "test 2", "test 3" });
            var builder = new NameGeneratorBuilder();
            var result = builder.FromDictionary(data).Build();

            Assert.That(result.AvailableData, Is.Not.Empty);
        }
    }
}