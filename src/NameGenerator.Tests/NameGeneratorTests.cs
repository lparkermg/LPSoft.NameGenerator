using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace LPSoft.NameGenerator.Tests
{
    public class NameGeneratorTests
    {
        [TestCase("")]
        [TestCase(null)]
        [TestCase("     ")]
        public void Get_GivenNullOrWhitespaceKey_ThrowsArgumentException(string invalidKey)
        {
            var generator = new NameGeneratorBuilder()
                .FromDictionary(new Dictionary<string, string[]>() { { "test", new[] { "test 1", "test 2"} } })
                .Build();

            Assert.That(() => generator.Get(invalidKey), Throws.ArgumentException.With.Message.EqualTo("Key cannot be null, empty or whitespace."));
        }

        [Test]
        public void Get_GivenKeyThatDoesntExist_ThrowsKeyNotFoundException()
        {
            var generator = new NameGeneratorBuilder()
                .FromDictionary(new Dictionary<string, string[]>() { { "test", new[] { "test 1", "test 2" } } })
                .Build();

            Assert.That(() => generator.Get("not the right key"), Throws.TypeOf<KeyNotFoundException>());
        }

        [Test]
        public void Get_GivenKeyThatExists_ReturnsAValueFromAvailableData()
        {
            var key = "test";
            var generator = new NameGeneratorBuilder()
                .FromDictionary(new Dictionary<string, string[]>() { { key, new[] { "test 1", "test 2" } } })
                .Build();

            Assert.That(generator.Get(key), Is.AnyOf(generator.AvailableData[key]));
        }

        [Test]
        public void GetMultiple_GivenMultipleKeysWhereOneDoesnt_ThrowsKeyNotFoundException()
        {
            var key = "test";
            var generator = new NameGeneratorBuilder()
                .FromDictionary(new Dictionary<string, string[]>() { { key, new[] { "test 1", "test 2" } }, { "another key", new[] { "another test", "more tests" } } })
                .Build();

            Assert.That(() => generator.GetMultiple(key, "not the key"), Throws.TypeOf<KeyNotFoundException>());
        }

        [TestCase("")]
        [TestCase(null)]
        [TestCase("     ")]
        public void GetMultiple_GivenMultipleKeyWhereOneIsInvalid_ThrowsArgumentException(string invalidKey)
        {
            var key = "test";
            var generator = new NameGeneratorBuilder()
                .FromDictionary(new Dictionary<string, string[]>() { { key, new[] { "test 1", "test 2" } }, { "another key", new[] { "another test", "more tests" } } })
                .Build();

            Assert.That(() => generator.GetMultiple(key, invalidKey), Throws.ArgumentException.With.Message.EqualTo("Key cannot be null, empty or whitespace."));
        }

        [Test]
        public void GetMultiple_GivenMultipleValidKeys_ReturnsMergedValuesOfAllKeys()
        {
            var key = "test";
            var secondKey = "another key";
            var generator = new NameGeneratorBuilder()
                .FromDictionary(new Dictionary<string, string[]>() { { key, new[] { "test", "wah" } }, { secondKey, new[] { "another", "more" } } })
                .Build();
            var result = generator.GetMultiple(key, secondKey).Split(" ");
            Assert.Multiple(() =>
            {
                Assert.That(result[0], Is.AnyOf(generator.AvailableData[key]));
                Assert.That(result[1], Is.AnyOf(generator.AvailableData[secondKey]));
            });
        }
    }
}
