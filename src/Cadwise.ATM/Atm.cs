using Cadwise.ATM.Exceptions;
using Cadwise.ATM.Strategies;

namespace Cadwise.ATM;

/// <summary>
/// Основной класс банкомата
/// </summary>
public sealed class Atm
{
    private readonly IReadOnlyCollection<AtmCashCassette> cashCassetes;

    private IBanknotesSelectionStrategy _defaultStrategy = new RecursiveStrategy();
    private IBanknotesSelectionStrategy _withExchangeStrategy = new WithExchangeStrategy();

    public Atm()
    {
        cashCassetes =
        [
            new(5000, 100),
            new(2000, 100),
            new(1000, 100),
            new(500, 100),
            new(200, 100),
            new(100, 100),
            new(50, 100),
            new(10, 100)
        ];
    }

    /// <summary>
    /// Загрузит в банкомат банкноты определённого номинала
    /// </summary>
    /// <param name="banknoteDenomination"></param>
    /// <param name="banknoteCount"></param>
    /// <exception cref="BusinessLogicException"></exception>
    public int Load(int banknoteDenomination, int banknoteCount)
    {
        var cassete = cashCassetes.FirstOrDefault(x => x.BanknoteDenomination == banknoteDenomination);

        if (cassete == null)
        {
            throw new BusinessLogicException($"Банкноты номиналом {banknoteDenomination} не поддерживаются банкоматом");
        }

        return cassete.Load(banknoteCount);
    }

    /// <summary>
    /// Вернёт купюры для заданной суммы 
    /// </summary>
    /// <param name="amount">Сумма к возврату</param>
    /// <param name="withExchange">Вернёт купюры наименьшего достоинства</param>
    /// <returns></returns>
    public Dictionary<int, int> GetCash(int amount, bool withExchange = false)
    {
        var strategy = withExchange ? _withExchangeStrategy : _defaultStrategy;

        var result = strategy.CalculateBanknotesForAmount(amount, cashCassetes);

        if (result.Any())
        {
            //если удалось найти подходящий набор банкнот, то выгрузим их из кассет и направим пользователю
            foreach (var den2count in result)
            {
                cashCassetes.First(x => x.BanknoteDenomination == den2count.Key).Unload(den2count.Value);
            }
        }

        return result;
    }

    /// <summary>
    /// Вернёт все кассеты банкомата
    /// </summary>
    /// <returns></returns>
    public IEnumerable<AtmCashCassette> GetCassettes()
    {
        return cashCassetes;
    }
}