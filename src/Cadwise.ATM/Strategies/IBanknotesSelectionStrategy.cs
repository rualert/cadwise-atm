namespace Cadwise.ATM.Strategies;

/// <summary>
/// Стратегии подбора купюр для выдачи клиенту
/// </summary>
public interface IBanknotesSelectionStrategy
{
    /// <summary>
    /// Вернёт словать достоинство - колличество вадющий необходимую сумму 
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="cashCassettes"></param>
    /// <returns></returns>
    Dictionary<int, int> CalculateBanknotesForAmount(int amount, IEnumerable<AtmCashCassette> cashCassettes);
}