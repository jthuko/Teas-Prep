using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MblexApp.StaticMethods
{
    public static class CollectionShuffler
    {
        static Random random = new Random();

        public static void Shuffle<T>(ObservableCollection<T> collection)
        {

            int n = collection.Count;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(0, i + 1);

                // Swap elements at i and j
                T temp = collection[i];
                collection[i] = collection[j];
                collection[j] = temp;
            }
        }
    }
}
