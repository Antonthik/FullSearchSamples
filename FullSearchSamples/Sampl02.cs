
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using FullSearchSamples.Services;
using FullSearchSamples.Services.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FullSearchSamples
{
    internal class Sampl02
    {
        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {

                    #region Configure EF DBContext Service (CardStorageService Database)

                    services.AddDbContext<DocumentDbContext>(options =>
                    {
                        options.UseSqlServer(@"data source=BOOK-2\SQLEXPRESS;initial catalog=DocumentsDatabase;User Id=DocumentsDatabaseUser;Password=12345;MultipleActiveResultSets=True;App=EntityFramework");
                    });//add-migration DocumentsAdd,update-database

                    #endregion

                    #region Configure Repositories


                    #endregion

                })
                .Build();

            //var documentsSet = DocumentExtractor.DocumentsSet().Take(10000).ToArray();//
            //new SimpleSearcher().Search("Monday", documentsSet);//Выводим все статьи где содержится значение поиска
            //new SimpleSearcherV2().SearchV1("Monday", documentsSet);//Выводим все строки где содержится значение поиска
            //new SimpleSearcherV2().SearchV2("Monday", documentsSet);

            //Пуск измерения производительности
            BenchmarkSwitcher.FromAssembly(typeof(Sampl02).Assembly).Run(args, new BenchmarkDotNet.Configs.DebugInProcessConfig());
            BenchmarkRunner.Run<SearchBenchmarkV1>();//запуск         }
        }


    }

    [MemoryDiagnoser]//атрибут - анализ памяти
    [WarmupCount(1)]//атрибут -прогрев)-первый запуск с кешем и дольше грузиться, и мы не учитываем время пргрева-первый раз в хлостую,а потом выполняем
    [IterationCount(5)]//атрибут -выполняем 5 раз
    public class SearchBenchmarkV1
    {
        private readonly string[] _documentsSet;

        public SearchBenchmarkV1()
        {
            _documentsSet = DocumentExtractor.DocumentsSet().Take(10000).ToArray();//В конструкторе не учитываются время проиизводительности
        }

        [Benchmark]//атрибут
        public void SimpleSearch()
        {
            new SimpleSearcherV2().SearchV3("Monday", _documentsSet).ToArray();//Такой же,как и второй,но без вывода в консолю
        }

    }
}


