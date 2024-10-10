using FluentAssertions;

namespace Cadwise.ATM.Test;

public partial class ATMTests
{
    /// <summary>
    /// Проверим что выдача работает верно
    /// </summary>
    [TestCase(10, 10, 10, 500, 5, 0, 0)]
    [TestCase(10, 10, 10, 500, 5, 0, 0)]
    [TestCase(10, 10, 10, 520, 5, 0, 2)]
    [TestCase(10, 10, 10, 550, 5, 1, 0)]
    [TestCase(10, 10, 10, 560, 5, 1, 1)]
    [TestCase(10, 10, 10, 20, 0, 0, 2)]
    [TestCase(10, 10, 10, 0, 0, 0, 0)]
    [TestCase(10, 10, 10, 5, 0, 0, 0)]
    [TestCase(10, 10, 10, 121, 0, 0, 0)]
    [TestCase(10, 10, 10, 2000, 0, 0, 0)]
    [TestCase(10, 10, 10, 1600, 10, 10, 10)]
    [TestCase(10, 10, 10, 1610, 0, 0, 0)]
    [TestCase(10, 10, 10, -10, 0, 0, 0)]
    public void AtmWillReturnBanknotes(int init100, int init50, int init10, int amount, int exp100, int exp50, int exp10)
    {
        //Arrange
        var sut = new Atm();
        sut.Load(100, init100);
        sut.Load(50, init50);
        sut.Load(10, init10);
        
        //Act
        var result = sut.GetCash(amount);
        
        //Assert
        var checkDenomination = (int denomination, int expected) =>
        {
            if (expected > 0)
            {
                result.Should().ContainKey(denomination, $"Содержит {denomination}\u20bd");
                result![denomination].Should().Be(expected);
            }
            else
            {
                result.Should().NotContainKey(denomination, $"Не содержит {denomination}\u20bd");
            }
        };

        checkDenomination(100, exp100);
        checkDenomination(50, exp50);
        checkDenomination(10, exp10);

        var cassetes = sut.GetCassettes();
        cassetes.First(x => x.BanknoteDenomination == 100).BanknoteCount.Should().Be(init100 - exp100);
        cassetes.First(x => x.BanknoteDenomination == 50).BanknoteCount.Should().Be(init50 - exp50);
        cassetes.First(x => x.BanknoteDenomination == 10).BanknoteCount.Should().Be(init10 - exp10);
    }
    
    /// <summary>
    /// Проверим что выдача работает верно
    /// </summary>
    [TestCase(10, 10, 10, 500, 0, 8, 10)]
    [TestCase(10, 10, 10, 0, 0, 0, 0)]
    [TestCase(10, 10, 10, 5, 0, 0, 0)]
    [TestCase(10, 10, 10, 121, 0, 0, 0)]
    [TestCase(10, 10, 10, 2000, 0, 0, 0)]
    [TestCase(10, 10, 10, 1600, 10, 10, 10)]
    [TestCase(10, 10, 10, 1610, 0, 0, 0)]
    [TestCase(10, 10, 10, -10, 0, 0, 0)]
    public void AtmWillReturnBanknotesWithExchange(int init100, int init50, int init10, int amount, int exp100, int exp50, int exp10)
    {
        //Arrange
        var sut = new Atm();
        sut.Load(100, init100);
        sut.Load(50, init50);
        sut.Load(10, init10);
        
        //Act
        var result = sut.GetCash(amount, withExchange: true);
        
        //Assert
        var checkDenomination = (int denomination, int expected) =>
        {
            if (expected > 0)
            {
                result.Should().ContainKey(denomination, $"Содержит {denomination}\u20bd");
                result[denomination].Should().Be(expected);
            }
            else
            {
                result.Should().NotContainKey(denomination, $"Не содержит {denomination}\u20bd");
            }
        };

        checkDenomination(100, exp100);
        checkDenomination(50, exp50);
        checkDenomination(10, exp10);

        var cassetes = sut.GetCassettes();
        cassetes.First(x => x.BanknoteDenomination == 100).BanknoteCount.Should().Be(init100 - exp100);
        cassetes.First(x => x.BanknoteDenomination == 50).BanknoteCount.Should().Be(init50 - exp50);
        cassetes.First(x => x.BanknoteDenomination == 10).BanknoteCount.Should().Be(init10 - exp10);
    }
    
    /// <summary>
    /// Сложные случаи для которых не работает метод разложения 
    /// </summary>
    [TestCase(1, 10, 0, 600, 0, 3, 0)]
    public void AtmWillReturnBanknotesForDifficultCases(int init500, int init200, int init100, int amount, int exp500, int exp200, int exp100)
    {
        //Arrange
        var sut = new Atm();
        sut.Load(500, init500);
        sut.Load(200, init200);
        sut.Load(100, init100);
        
        //Act
        var result = sut.GetCash(amount);
        
        //Assert
        var checkDenomination = (int denomination, int expected) =>
        {
            if (expected > 0)
            {
                result.Should().ContainKey(denomination, $"Содержит {denomination}\u20bd");
                result![denomination].Should().Be(expected);
            }
            else
            {
                result.Should().NotContainKey(denomination, $"Не содержит {denomination}\u20bd");
            }
        };

        checkDenomination(500, exp500);
        checkDenomination(200, exp200);
        checkDenomination(100, exp100);

        var cassetes = sut.GetCassettes();
        cassetes.First(x => x.BanknoteDenomination == 500).BanknoteCount.Should().Be(init500 - exp500);
        cassetes.First(x => x.BanknoteDenomination == 200).BanknoteCount.Should().Be(init200 - exp200);
        cassetes.First(x => x.BanknoteDenomination == 100).BanknoteCount.Should().Be(init100 - exp100);
    }
}