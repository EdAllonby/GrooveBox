using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities
{
    public static class ListExtensions
    {
        private static readonly Random RandomNumberGenerator = new Random();

        /// <summary>
        /// Get a random element from a collection.
        /// </summary>
        /// <param name="elements">The collection of elements.</param>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <returns>A random element from the collection of elements.</returns>
        public static T RandomElement<T>(this List<T> elements)
        {
            int randomIndex = RandomNumberGenerator.Next(elements.Count());

            return elements[randomIndex];
        }
    }
}