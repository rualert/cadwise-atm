namespace Cadwise.ATM.Strategies;

/// <summary>
/// Стратегия выбора купюр без размена
/// </summary>
public class RecursiveStrategy : IBanknotesSelectionStrategy
{
    public Dictionary<int, int> CalculateBanknotesForAmount(int amount, IEnumerable<AtmCashCassette> cashCassettes)
    {
        var possibleResults = new Dictionary<int, int>{ { 0, 0 } };
        
        // вычисляем возможные комбинации 
        foreach (var cassette in cashCassettes.OrderByDescending(x => x.BanknoteDenomination))
        {
            for (var i = 0; i < cassette.BanknoteCount; i++)
            {
                foreach (var possibleResultKey in possibleResults.Keys.ToList())
                {
                    var newResult = possibleResultKey + cassette.BanknoteDenomination;

                    if (newResult > amount)
                    {
                        continue;
                    }
                    
                    if (!possibleResults.ContainsKey(newResult))
                    {
                        possibleResults[newResult] = cassette.BanknoteDenomination;
                    }

                    if (newResult == amount)
                    {
                        break;
                    }
                }
            }
        }

        var result = new Dictionary<int, int> ();
        if (possibleResults.ContainsKey(amount))
        {
            while (amount > 0)
            {
                var denomenation = possibleResults[amount];
                result.TryAdd(denomenation, 0);
                
                result[denomenation]++;
                amount -= denomenation;
            }
        }

        return result;
    }
}