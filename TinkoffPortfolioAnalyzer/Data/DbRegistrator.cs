using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;

namespace TinkoffPortfolioAnalyzer.Data
{
    static internal class DbRegistrator
    {
        public static IServiceCollection AddDatabase(this IServiceCollection serices, IConfiguration config)
        {
            var s = serices.AddDbContext<PortfolioAnalyzerDb>(optAction =>
            {
                var dbType = config["Type"];
                Debug.WriteLine(dbType);
                switch(dbType)
                {
                    case "MSSQL": 
                        optAction.UseSqlServer(config.GetConnectionString(dbType));
                        break;
                    default: 
                        throw new InvalidOperationException($"Тип подключения {dbType} не поддерживается");
                }
            });

            return s;
        }
    }
}
