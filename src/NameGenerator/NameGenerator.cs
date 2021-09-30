// <copyright file="NameGenerator.cs" company="Luke Parker">
// Copyright (c) Luke Parker. All rights reserved.
// </copyright>

namespace LPSoft.NameGenerator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Generates names based on the provided data.
    /// </summary>
    public class NameGenerator
    {
        private Random _rng;

        /// <summary>
        /// Initializes a new instance of the <see cref="NameGenerator"/> class.
        /// </summary>
        /// <param name="data">The data for the generator.</param>
        public NameGenerator(IReadOnlyDictionary<string, string[]> data)
        {
            AvailableData = data;
            _rng = new Random();
        }

        /// <summary>
        /// Gets the data available to the <see cref="NameGenerator"/> class.
        /// </summary>
        public IReadOnlyDictionary<string, string[]> AvailableData { get; }

        /// <summary>
        /// Gets a value from the provided key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>A random string value from the provided key.</returns>
        public string Get(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new ArgumentException("Key cannot be null, empty or whitespace.");
            }

            return AvailableData[key][_rng.Next(0, AvailableData[key].Length)];
        }

        /// <summary>
        /// Gets multiple values from the provided keys.
        /// </summary>
        /// <param name="keys">The keys of the values to get.</param>
        /// <returns>  random string value from the provided keys.</returns>
        public string GetMultiple(params string[] keys)
        {
            return string.Join(" ", keys.Select(k => Get(k)).ToArray());
        }
    }
}
