using Cadwise.ATM.Exceptions;
using FluentAssertions;

namespace Cadwise.ATM.Test;

public partial class ATMTests
{
    /// <summary>
    /// Проверим что загрузка купюр проходит успешно
    /// </summary>
    [Test]
    public void AtmWillAcceptCash()
    {
        //Arrange
        var sut = new Atm();
        
        //Act
        sut.Load(10, 1);
        
        //Assert
        sut.GetCassettes().First(x => x.BanknoteDenomination == 10).BanknoteCount.Should().Be(1);
    }

    /// <summary>
    /// Проверим что лишние купюры при загрузке отвергаются
    /// </summary>
    [Test]
    public void AtmWillNotAcceptMoreThenCapacity()
    {
        //Arrange
        var sut = new Atm();
        
        //Act
        sut.Load(10, 101);
        
        //Assert
        sut.GetCassettes().First(x => x.BanknoteDenomination == 10).BanknoteCount.Should().Be(100);
    }
    
    /// <summary>
    /// Проверим, что банкомат не принимает всякую ересь
    /// </summary>
    /// <param name="denomination"></param>
    /// <param name="count"></param>
    /// <param name="message"></param>
    [TestCase(1, 100, "Не верный номинал")]
    [TestCase(10, -100, "Отрицательное колличество")]
    public void AtmWillNotLoadWrongParameters(int denomination, int count, string message)
    {
        //Arrange
        var sut = new Atm();
        
        //Act
        FluentActions.Invoking(()=>
        {
            sut.Load(denomination, count);
        })
            //Assert
            .Should().Throw<BusinessLogicException>(message);
    }
}