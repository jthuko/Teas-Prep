using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MblexApp.StaticMethods
{
    public static class KeepElements
    {
        public static void KeepAtMostNElements<T>(ObservableCollection<T> source, int n)
        {
            if (n < source.Count)
            {
                for (int i = source.Count - 1; i >= n; i--)
                {
                    source.RemoveAt(i);
                }
            }
        }
    }
}
