using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullSearchSamples.Services.Impl
{
    public class Lexer
    {//Принимаем на вход статью и посимвольно перебираем и формиру
        public IEnumerable<string> GetTokens(string text)
        {
            int start = -1;

            for (int i = 0; i < text.Length; i++)
            {

                if (char.IsLetterOrDigit(text[i]))  //если символ буква или число то позицию берем
                {
                    if (start == -1) 
                        start = i;
                }
                else
                {
                    if (start >= 0)
                    {
                        yield return GetToken(text, i, start);// и стартовая позиция> = 0,то берем слово
                        start = -1;
                    }
                }

            }
        }

        private string GetToken(string text, int i, int start)
        {
            return text.Substring(start, i - start).Normalize().ToLowerInvariant();//в нижнем регистре берем слово
        }
    }
}
