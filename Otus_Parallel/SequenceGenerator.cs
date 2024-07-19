using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus_Parallel
{
    public class SequenceGenerator
    {
        readonly Random random = new Random();
        public long[] Generate(int count)
        {
            var result = new long[count];

            for (int i = 0; i < count; i++)
            {
                result[i] = random.Next();
            }

            return result;
        }
    }
}
