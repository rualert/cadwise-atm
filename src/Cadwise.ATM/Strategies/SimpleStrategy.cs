namespace Cadwise.ATM.Strategies;

/// <summary>
/// Пытаемся найти подходящую выдачу путём подбора наибольших банкнот
/// </summary>
public class SimpleStrategy:IBanknotesSelectionStrategy
{
    public Dictionary<int, int> CalculateBanknotesForAmount(int amount, IEnumerable<AtmCashCassette> cashCassettes)
    {
        var result = new Dictionary<int, int>();
        
        foreach (var cassette in cashCassettes.OrderByDescending(x=>x.BanknoteDenomination))
        {
            var allowed = cassette.GetBanknotesForAmount(amount);

            if (allowed > 0)
            {
                result.Add(cassette.BanknoteDenomination, allowed);
                
                amount -= allowed * cassette.BanknoteDenomination;
            }
        }

        if (amount != 0)
        {
            result.Clear();
        }

        return result;
    }
}