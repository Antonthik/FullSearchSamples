
using FullSearchSamples.Services;
using FullSearchSamples.Services.Impl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FullSearchSamples
{
    internal class Sampl01
    {
        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services => {

                    #region Configure EF DBContext Service (CardStorageService Database)

                    services.AddDbContext<DocumentDbContext>(options =>
                    {
                        options.UseSqlServer(@"data source=BOOK-2\SQLEXPRESS;initial catalog=DocumentsDatabase;User Id=DocumentsDatabaseUser;Password=12345;MultipleActiveResultSets=True;App=EntityFramework");
                    });//add-migration DocumentsAdd,update-database

                    #endregion

                    #region Configure Repositories

                    services.AddTransient<IDocumentRepository, DocumentRepository>();

                    #endregion

                })
                .Build();

            // Сохраним документы в БД 
            //
            host.Services.GetRequiredService<IDocumentRepository>().LoadDocuments();


        }
    }
}