using FullSearchSamples.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullSearchSamples.Services.Impl
{
    public class FullTextIndexV1
    {
        private readonly DocumentDbContext _context;
        private readonly Lexer _lexer = new Lexer();

        public FullTextIndexV1(DocumentDbContext context = null)
        {
            _context = context;
        }
        //Обращаемся к БД и ко всем документам(абзацам)
        public void BuildIndex()
        {
            foreach (var document in _context.Documents.ToArray())//Перебираем документы
            {
                foreach (var token in _lexer.GetTokens(document.Content))//разбиваем на токены -слова
                {
                    var word = _context.Words.FirstOrDefault(w => w.Text == token);//берем слово и проверяем в БД
                    int wordId = 0;
                    if (word == null)
                    {
                        var wordObj = new Word
                        {
                            Text = token
                        };
                        _context.Words.Add(wordObj);//добавляем в БД слово
                        _context.SaveChanges();
                        wordId = wordObj.Id;//запоминаем id после добавления
                    }
                    else
                        wordId = word.Id;

                    var wordDocument = _context.WordDocuments.FirstOrDefault(wd => wd.WordId == wordId && wd.DocumentId == document.Id);
                    if (wordDocument == null)
                    {
                        _context.WordDocuments.Add(new WordDocument
                        {
                            DocumentId = document.Id,
                            WordId = wordId
                        });
                        _context.SaveChanges();
                    }
                }
            }
        }
    }
}
