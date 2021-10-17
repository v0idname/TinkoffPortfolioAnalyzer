using System;
using Tinkoff.Trading.OpenApi.Models;

namespace TinkoffPortfolioAnalyzer.Models
{
    public class TinkoffAccount : Account, IEquatable<TinkoffAccount>
    {
        public TinkoffAccount(Account acc) : base(acc.BrokerAccountType, acc.BrokerAccountId)
        {
        }

        public TinkoffAccount(BrokerAccountType accType, string accId) : base(accType, accId)
        {
        }

        public bool Equals(TinkoffAccount other)
        {
            return BrokerAccountType == other.BrokerAccountType
                && BrokerAccountId == other.BrokerAccountId;
        }

        public override string ToString()
        {
            return $"{BrokerAccountType} - {BrokerAccountId}";
        }
    }
}
