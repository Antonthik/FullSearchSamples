using FullSearchSamples.Services.Impl;
using FullSearchSamples.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace FullSearchSamples
{
    internal class Sampl03
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
                    });

                    #endregion

                    #region Configure Repositories

                    #endregion


                })
                .Build();

           // FullTextIndexV1 fullTextIndexV1  = new FullTextIndexV1(host.Services.GetService<DocumentDbContext>());
           // fullTextIndexV1.BuildIndex();
            BenchmarkSwitcher.FromAssembly(typeof(Sampl03).Assembly).Run(args, new BenchmarkDotNet.Configs.DebugInProcessConfig());
            BenchmarkRunner.Run<SearchBenchmarkV2>();
        }
    }
    [MemoryDiagnoser]
    [WarmupCount(1)]
    [IterationCount(5)]
    public class SearchBenchmarkV2
    {

        private readonly FullTextIndexV3 _index;
        private readonly string[] _documentsSet;

        [Params("intercontinental", "monday", "not")]//атрибут-выполним тест для трех слов
        public string Query { get; set; }

        public SearchBenchmarkV2()
        {
            _documentsSet = DocumentExtractor.DocumentsSet().Take(10000).ToArray();
            _index = new FullTextIndexV3();
            foreach (var item in _documentsSet)
                _index.AddStringToIndex(item);//В конструкторе не учитывается

        }

        [Benchmark(Baseline = true)]//Baseline-тестируем относительно SimpleSearch,а FullTextIndexSearch -второй
        public void SimpleSearch()
        {
            new SimpleSearcherV2().SearchV3(Query, _documentsSet).ToArray();
        }

        [Benchmark]
        public void FullTextIndexSearch()
        {
            _index.SearchTest(Query).ToArray();
        }

    }
}
