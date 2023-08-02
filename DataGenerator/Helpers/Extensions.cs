using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGenerator.Helpers
{
    public static class ListExtensions
    {

        public static List<T> GetRandomElements<T>(this List<T> source, int count)
        {
            Random random = new Random();

            List<T> result = new List<T>();

            HashSet<int> selectedIndices = new HashSet<int>();

            while (result.Count < count)
            {
                int index = random.Next(source.Count);

                if (!selectedIndices.Contains(index))
                {
                    result.Add(source[index]);

                    selectedIndices.Add(index);
                }
            }

            return result;
        }
    }
}
