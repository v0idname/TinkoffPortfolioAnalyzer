using System;
using System.Collections.Generic;
using System.Linq;
using Tinkoff.Trading.OpenApi.Models;

namespace TinkoffPortfolioAnalyzer.Models
{
    [Serializable]
    public class SecurityInfoList
    {
        List<SecurityInfo> _list = new();

        public void AddMarketInstList(MarketInstrumentList list)
        {
            _list.AddRange(
                list.Instruments.Select(item => new SecurityInfo()
                {
                    Name = item.Name,
                    Ticker = item.Ticker,
                    InstrumentType = item.Type,
                    Currency = item.Currency
                }).ToList());
        }

        public List<SecurityInfo> List => _list;
    }
}
