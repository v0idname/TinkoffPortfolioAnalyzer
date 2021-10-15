using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using TinkoffPortfolioAnalyzer.Models;

namespace TinkoffPortfolioAnalyzer.Converters
{
    [ValueConversion(typeof(TinkoffToken), typeof(string))]
    internal class TokenHiderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var token = value as TinkoffToken;
            if (token == null)
                return null;
            return token.ToString().Substring(0, 20) + "*****";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
