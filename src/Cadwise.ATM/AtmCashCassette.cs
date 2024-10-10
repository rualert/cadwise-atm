using Cadwise.ATM.Exceptions;

namespace Cadwise.ATM;

public class AtmCashCassette
{
    /// <summary>
    /// Достоинство банкноты
    /// </summary>
    public int BanknoteDenomination { get; }

    /// <summary>
    /// Ёмкость кссеты
    /// </summary>
    public int Capacity { get; }

    /// <summary>
    /// Количество банкнот в кассете
    /// </summary>
    public int BanknoteCount { get; private set; }

    /// <summary>
    /// Касета в банкомате
    /// </summary>
    /// <param name="banknoteDenomination"></param>
    /// <param name="capacity">Ёмкость кассеты</param>
    internal AtmCashCassette(int banknoteDenomination, int capacity)
    {
        BanknoteDenomination = banknoteDenomination;
        Capacity = capacity;
    }

    /// <summary>
    /// Загрузит банкноты в кссету. Будем считать, что лишние банкноты банкомат отрыгнёт.
    /// </summary>
    /// <param name="banknoteCount"></param>
    /// <exception cref="BusinessLogicException"></exception>
    internal int Load(int banknoteCount)
    {
        if (banknoteCount < 0)
        {
            throw new BusinessLogicException("Колличество купюр не может быть отризацтельным");
        }

        var notLoaded = banknoteCount + BanknoteCount - Capacity;
        
        BanknoteCount = Math.Min(Capacity, BanknoteCount + banknoteCount);

        return Math.Max(0, notLoaded);
    }

    /// <summary>
    /// Выгрузим банкноты из кассеты
    /// </summary>
    /// <param name="banknoteCount">Колличество банкнот которые наобходимо выгрузить</param>
    internal void Unload(int banknoteCount)
    {
        BanknoteCount -= banknoteCount;
    }

    /// <summary>
    /// Вернёт банкноты для заданной суммы
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    internal int GetBanknotesForAmount(int amount)
    {
        if (amount < BanknoteDenomination)
        {
            return 0;
        }

        return Math.Min(BanknoteCount, (int)Math.Truncate((float)amount / BanknoteDenomination));
    }
}