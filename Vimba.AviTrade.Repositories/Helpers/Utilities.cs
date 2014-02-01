using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vimba.AviTrade.Models;

namespace Vimba.AviTrade.Repositories.Helpers
{
    public static class Utilities
    {
        public static double GenerateRandomAmount(double max = 3500)
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            double range = rnd.NextDouble();
            double rndValue = range * max;
            return rndValue;
        }

        public static double GenerateRandomAmount(int seed, double max = 3500)
        {
            Random rnd = new Random(seed * DateTime.Now.Millisecond);
            double range = rnd.NextDouble();
            double rndValue = range * max;
            return rndValue;
        }

        public static string GenerateRandomName(int nLength)
        {
            char[] chars = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            int charsNo = 26;
            int length = nLength;
            Random rnd = new Random(DateTime.Now.Millisecond);
            String rndString = "";

            for (int i = 0; i < length; i++)
                rndString += chars[rnd.Next(charsNo)];

            return rndString;
        }

        public static string GenerateRandomNumber(int nLength)
        {
            char[] chars = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            int charsNo = 9;
            int length = nLength;
            Random rnd = new Random(DateTime.Now.Millisecond);
            String rndString = "";

            for (int i = 0; i < length; i++)
                rndString += chars[rnd.Next(charsNo)];

            return rndString;
        }

        public static string PadString(string str, int len)
        {
            if (str.Length >= len)
                return str;

            int appLen = len - str.Length;
            string newStr = "";
            for (int i = 0; i < appLen; i++)
                newStr += "0";

            return newStr + str;
        }

        public static double[] GetExchangeRates(Currency transactionCurrency, Currency pivotCurrency, Currency globalPivotCurrency)
        {
            double[] rates = new double[2];

            if (transactionCurrency.Id == pivotCurrency.Id)
                rates[0] = 1.00;
            else
            {
                if (transactionCurrency.Rate != 0)
                    rates[0] = pivotCurrency.Rate * transactionCurrency.Rate;
                else
                    rates[0] = 0;
            }

            if (transactionCurrency.Id == globalPivotCurrency.Id)
                rates[1] = 1.00;
            else
            {
                if (transactionCurrency.Rate != 0)
                    rates[1] = globalPivotCurrency.Rate * transactionCurrency.Rate;
                else
                    rates[1] = 0;
            }

            return rates;
        }
    }
}
