using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;

namespace VendingMachine
{
    class VendingMachine
    {
        // Dictionary so we can easily add new coins (key)
        private Dictionary<double, int> _coins;

        public VendingMachine()
        {
            _coins = new Dictionary<double, int>();
        }

        public void loadCoins(int amount, double denominations)
        {
            if (_coins.ContainsKey(denominations))
            {
                _coins[denominations] += amount;
            }
            else
            {
                _coins.Add(denominations, amount);
            }
        }

        /*
         *  Display the amount of coins per denominations
         */
        public void printMoney()
        {
            foreach (double key in _coins.Keys)
            {
                Debug.WriteLine(key + ":" + _coins[key].ToString());
            }
        }

        private double getMoneyGiven(List<double[]> money)
        {
            double totalMoneyGiven = 0;

            foreach (double[] tabMoney in money)
            {
                if (tabMoney.Length == 2) {
                    totalMoneyGiven += tabMoney[0] * tabMoney[1];
                } else
                {
                    return -1;
                }
            }
            return totalMoneyGiven;
        }

        /*
         * money is a list of tab representing the given money, we assume that we give first the amount and then the quantity of coins.
         */
        public void buyItem(double priceItem, List<double[]> money)
        {
            int changeForSpecificCoin = 0;

            if (priceItem > 0 && money != null)
            {
                // Get the total money given
                double totalMoneyGiven = getMoneyGiven(money);

                double changeToGive = totalMoneyGiven - priceItem;

                if (changeToGive > 0)
                {
                    // We get our dictionary ordered (descending) so we give less change
                    var moneyOrdered = from pair in _coins orderby pair.Key descending select pair;

                    foreach (KeyValuePair<double, int> pair in moneyOrdered)
                    {
                        changeForSpecificCoin = 0;
                        while (changeToGive >= pair.Key && _coins[pair.Key] > 0)
                        {
                            changeToGive -= pair.Key;
                            _coins[pair.Key] -= 1;
                            changeForSpecificCoin += 1;
                        }
                        if (changeForSpecificCoin > 0) {
                            Debug.WriteLine("Give change " + changeForSpecificCoin.ToString() + " x " + pair.Key.ToString() + "€");
                        }
                    }
                }
            }
            
        }
    }
}
