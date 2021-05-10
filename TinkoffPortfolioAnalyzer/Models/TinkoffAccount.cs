using Tinkoff.Trading.OpenApi.Models;

namespace TinkoffPortfolioAnalyzer.Models
{
    internal class TinkoffAccount : Account
    {
        public TinkoffAccount(Account acc) : base(acc.BrokerAccountType, acc.BrokerAccountId)
        {
        }

        public TinkoffAccount(BrokerAccountType accType, string accId) : base(accType, accId)
        {
        }

        public override string ToString()
        {
            return $"{BrokerAccountType} - {BrokerAccountId}";
        }
    }
}
