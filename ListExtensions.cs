using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeasPrep
{
    public static class ListExtensions
    {
        // Create a static random number generator
        private static Random rng = new Random();

        // Define an extension method for shuffling a list
        public static void Shuffle<T>(this IList<T> list)
        {
            // Get the number of elements in the list
            int n = list.Count;

            // Loop through the list from the last element to the first
            while (n > 1)
            {
                // Decrement n
                n--;

                // Generate a random index between 0 and n
                int k = rng.Next(n + 1);

                // Swap the current element with the randomly chosen one
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
