// <copyright file="NameGenerator.cs" company="Luke Parker">
// Copyright (c) Luke Parker. All rights reserved.
// </copyright>

namespace LPSoft.NameGenerator
{
    using System.Collections.Generic;

    /// <summary>
    /// Generates names based on the provided data.
    /// </summary>
    public class NameGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NameGenerator"/> class.
        /// </summary>
        /// <param name="data">The data for the generator.</param>
        public NameGenerator(IDictionary<string, string[]> data) => AvailableData = data;

        /// <summary>
        /// Gets the data available to the <see cref="NameGenerator"/> class.
        /// </summary>
        public IDictionary<string, string[]> AvailableData { get; }
    }
}
