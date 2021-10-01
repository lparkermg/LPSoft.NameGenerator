// <copyright file="NameGeneratorBuilder.cs" company="Luke Parker">
// Copyright (c) Luke Parker. All rights reserved.
// </copyright>

namespace LPSoft.NameGenerator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;

    /// <summary>
    /// Builder for the <see cref="NameGenerator"/>.
    /// </summary>
    public class NameGeneratorBuilder
    {
        private IDictionary<string, string[]> _currentData = new Dictionary<string, string[]>();

        /// <summary>
        /// Provides names from a json file.
        /// </summary>
        /// <param name="path">The path of the json file.</param>
        /// <returns>The <see cref="NameGeneratorBuilder"/> itself.</returns>
        public NameGeneratorBuilder FromJsonFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !path.EndsWith(".json"))
            {
                throw new ArgumentException("Invalid path provided.");
            }

            using (var reader = File.OpenText(path))
            {
                var dataToAdd = JsonSerializer.Deserialize<IDictionary<string, string[]>>(reader.ReadToEnd());
                foreach (var key in dataToAdd.Keys)
                {
                    _currentData.Add(key, dataToAdd[key]);
                }
            }

            return this;
        }

        /// <summary>
        /// Provides names from a dictionary.
        /// </summary>
        /// <param name="data">The provided data.</param>
        /// <returns>The <see cref="NameGeneratorBuilder"/> itself.</returns>
        public NameGeneratorBuilder FromDictionary(IDictionary<string, string[]> data)
        {
            if (data == null)
            {
                throw new ArgumentException("Dictionary cannot be null.");
            }

            foreach (var key in data.Keys)
            {
                _currentData.Add(key, data[key]);
            }

            return this;
        }

        /// <summary>
        /// Builds a new <see cref="NameGenerator"/>.
        /// </summary>
        /// <returns>A new <see cref="NameGenerator"/>.</returns>
        public NameGenerator Build() => new NameGenerator((IReadOnlyDictionary<string, string[]>)_currentData);
    }
}
