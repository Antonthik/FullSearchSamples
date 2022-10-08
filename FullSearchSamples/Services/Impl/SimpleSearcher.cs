using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullSearchSamples.Services.Impl
{
    public class SimpleSearcher
    {
        public void Search(string word, IEnumerable<string> data)//Строка поиска,Источник
        {
            foreach (var item in data)
            {
                if (item.Contains(word, StringComparison.InvariantCultureIgnoreCase))//Поиск в цикле без учета регистртра
                    Console.WriteLine(item);
            }
        }
    }
}
