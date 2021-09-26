// <copyright file="NameGeneratorBuilder.cs" company="Luke Parker">
// Copyright (c) Luke Parker. All rights reserved.
// </copyright>

namespace LPSoft.NameGenerator
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
    using System.Threading.Tasks;

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
        public async Task<NameGeneratorBuilder> FromJsonFile(string path)
        {
            if (string.IsNullOrWhiteSpace(path) || !path.EndsWith(".json"))
            {
                throw new ArgumentException("Invalid path provided.");
            }

            using (var reader = File.OpenText(path))
            {
                var dataToAdd = JsonSerializer.Deserialize<IDictionary<string, string[]>>(await reader.ReadToEndAsync());
                foreach (var key in dataToAdd.Keys)
                {
                    _currentData.Add(key, dataToAdd[key]);
                }
            }

            return this;
        }

        /// <summary>
        /// Builds a new <see cref="NameGenerator"/>.
        /// </summary>
        /// <returns>A new <see cref="NameGenerator"/>.</returns>
        public NameGenerator Build() => new NameGenerator(_currentData);
    }
}
