namespace Cadwise.ATM.Strategies;

/// <summary>
/// Более оптимальное решение, предоставленное коллегами из codewise
/// </summary>
public class RecursiveStrategyCodewise : IBanknotesSelectionStrategy
{
    public Dictionary<int, int> CalculateBanknotesForAmount(int amount, IEnumerable<AtmCashCassette> cashCassettes)
    {
        var result = new Dictionary<int, int>();

        if (!TryFindBanknotes(amount, cashCassettes, out var possibleResults))
        {
            return result;
        }

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

    private static bool TryFindBanknotes(int amount, IEnumerable<AtmCashCassette> cashCassettes,
        out Dictionary<int, int> possibleResults)
    {
        possibleResults = new Dictionary<int, int> { { 0, 0 } };
        var sortedCassettes = cashCassettes.OrderByDescending(x => x.BanknoteDenomination).ToList();

        foreach (var cassette in sortedCassettes)
        {
            var denomination = cassette.BanknoteDenomination;
            var count = cassette.BanknoteCount;

            foreach (var currentAmount in possibleResults.Keys.ToList())
            {
                for (var i = 1; i <= count; i++)
                {
                    var newAmount = currentAmount + denomination * i;
                    if (newAmount > amount)
                    {
                        break;
                    }

                    possibleResults.TryAdd(newAmount, denomination);

                    if (newAmount == amount)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }
}