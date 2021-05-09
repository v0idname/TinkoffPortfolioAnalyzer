using System.Linq;
using System.Threading.Tasks;
using Tinkoff.Trading.OpenApi.Models;
using Tinkoff.Trading.OpenApi.Network;

namespace NewTinkSecsConsoleTests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await GetAllBonds();
        }

        private static async Task GetAllBonds()
        {
            // токен аутентификации
            var sandboxToken = "t.qnjT83GCBu8TWdiXRcuqCnUXoh_Z-sstY0eyhq9duzZ8uKqZfOzLeculB8A3lou_Rl7pfMF6k8wvIp-6kNVBxQ";
            var tradingToken = "t.zbKUmxP9SRmY14raZfJewpclgi0It5QO53eSoAalFM8uuTN82d--xg8H-swG61jd0z0lx2F9-0B8kGYLKhWzxw";

            // для работы в песочнице используйте GetSandboxConnection
            var connection = ConnectionFactory.GetConnection(tradingToken);
            var context = connection.Context;
            //var sandboxAccount = await context.RegisterAsync(BrokerAccountType.Tinkoff);
            var accounts = await context.AccountsAsync();
            var iisAccountId = accounts.First(x => x.BrokerAccountType == BrokerAccountType.TinkoffIis).BrokerAccountId;

            // вся работа происходит асинхронно через объект контекста
            var portfolio = await context.PortfolioAsync(iisAccountId);
            //var bonds = await context.MarketBondsAsync();
        }
    }
}
