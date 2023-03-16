using Shouldly;
using VendingMachineKata.Models.Coins;
using VendingMachineKata.Models.Products;
using VendingMachineKata.Services;
using VendingMachineKataTests.AutoData;
using Xunit;

namespace VendingMachineKataTests;

public class CashierServiceTests
{
    // Insert Happy Path
    // Valid Coin
    [Theory]
    [AutoMoqData]
    public void Insert_WhenValidCoinIsInserted_ReleaseProductShouldBeTrueValidCoinShouldBeTrueAndShowAmountToBePaid(
        CashierService cashierService, UnitedKingdomCoins ukCoins, Product product)
    {
        //Arrange
        var invalidCoin = ukCoins.TwentyPence;
        
        //Act
        var insertResult = cashierService.Insert(invalidCoin, null);

        //Assert
        insertResult.ShouldBe((false, true, 00.20));
    }
    
    // Insert Sad Path
    // Invalid Coin
    [Theory]
    [AutoMoqData]
    public void Insert_WhenInvalidCoinIsInserted_ReleaseProductShouldBeFalseValidCoinShouldBeFalseAndShowAmountToBePaid(
        CashierService cashierService, UnitedKingdomCoins ukCoins, Product product)
    {
        //Arrange
        var invalidCoin = ukCoins.Invalid;
        
        //Act
        var insertResult = cashierService.Insert(invalidCoin, product);

        //Assert
        insertResult.ShouldBe((false,false,product.Cost));
    }
    
    // Return Coins Inserted
    [Theory]
    [AutoMoqData]
    public void ReturnAllInsertedCoins_WhenInsertedCoins_ReturnSameAmountInserted(CashierService cashierService, UnitedKingdomCoins insertedCoin)
    {
        // Arrange
        cashierService.Insert(insertedCoin.OnePound, null);

        // Act
        var returnedCoinsResult = cashierService.ReturnAllInsertedCoins();

        // Assert
        insertedCoin.OnePound.Value.ShouldBeEquivalentTo(returnedCoinsResult);
    }
    
}