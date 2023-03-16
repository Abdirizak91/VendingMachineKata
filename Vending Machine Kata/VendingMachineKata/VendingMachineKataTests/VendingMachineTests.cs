using AutoFixture.Xunit2;
using Moq;
using Shouldly;
using VendingMachineKata;
using VendingMachineKata.Models;
using VendingMachineKata.Models.Coins;
using VendingMachineKata.Models.Products;
using VendingMachineKata.Services;
using Xunit;
using VendingMachineKataTests.AutoData;

namespace VendingMachineKataTests;

public class VendingMachineTests
{
    // Select A Product Happy Path
    [Theory]
    [AutoMoqData]
    public void Selection_WhenRightProductIsSelected_ReturnVendOutcomeWithProduct([Frozen] Mock<ISelectionService> selectionService,
        VendingMachine sut)
    {
        //Arrange
        string productName = "Cola";
        double productCost = 1.00;
        
        VendOutcome vendOutcome = new()
        {
            Product = new()
            {
                Name = productName,
                Cost = productCost
            },
            Message = $"{productName} -- {productCost}"
        };
        
        selectionService.Setup(x => x.Selection(productName)).Returns(vendOutcome);
        
        //Act 
        var result = sut.SelectProduct(productName);
        
        //Arrange
        result.ShouldBeEquivalentTo(vendOutcome);
    }
    
    // Select A Product Sad Path
    [Theory]
    [AutoMoqData]
    public void Selection_WhenWrongProductIsSelected_ReturnVendOutcomeWithNoProduct([Frozen] Mock<ISelectionService> selectionService,
        VendingMachine sut)
    {
        //Arrange
        var wrongItemName = "Popcorn";
        
        VendOutcome vendOutcome = new()
        {
            Product = null,
            Message = "Item Unavailable"
        };

        selectionService.Setup(x => x.Selection(wrongItemName)).Returns(vendOutcome);
        
        //Act 
        var result = sut.SelectProduct(wrongItemName);
        
        //Arrange
        result.ShouldBeEquivalentTo(vendOutcome);
    }
    
    
    // Pay For Product Happy Path - Exact Coins - Returns Product
    [Theory]
    [AutoMoqData]
    public void PayForProduct_WhenExactCoins_ReturnVendOutcomeWithProduct(
        [Frozen] Mock<ICashierService> cashierService,
        VendingMachine sut, UnitedKingdomCoins ukCoins)
    {
        // Arrange
        var onePound = ukCoins.OnePound;
        
        var product = new Product()
        {
            Name = "Cola",
            Cost = 1.00
        };
        
        var vendOutcome = new VendOutcome()
        {
            Product = product,
            Message = "Item Dropped, Thank You",
            ReturnedChange = 00.00
        };
        
        cashierService.Setup(x => x.Insert(onePound, product)).Returns((true, true, 00.00));
        cashierService.Setup(x => x.CheckTransaction(true, true, 00.00, product)).Returns(vendOutcome);
        sut.SetCurrentProduct(product);
        
        //Act
        var sutResult = sut.PayForProduct(onePound);

        //Assert
        sutResult.ShouldBeEquivalentTo(vendOutcome);
    }
    
    // Pay For Product Sad Path - Less Coins - Returns No Product
    [Theory]
    [AutoMoqData]
    public void PayForProduct_WhenLessCoins_ReturnVendOutcomeWithNoProduct(
        [Frozen] Mock<ICashierService> cashierService,
        VendingMachine sut, UnitedKingdomCoins ukCoins)
    {
        // Arrange
        var fiftyPence = ukCoins.FiftyPence;
        
        var product = new Product()
        {
            Name = "Cola",
            Cost = 1.00
        };
        var figureRequired = product.Cost - fiftyPence.Value;
        
        var vendOutcome = new VendOutcome()
        {
            Product = null,
            Message = $"Required Amount: {figureRequired}",
            ReturnedChange = 00.00
        };
        
        cashierService.Setup(x => x.Insert(fiftyPence, product)).Returns((false, true, fiftyPence.Value));
        cashierService.Setup(x => x.CheckTransaction(false, true, fiftyPence.Value, product)).Returns(vendOutcome);
        sut.SetCurrentProduct(product);
        
        //Act
        var sutResult = sut.PayForProduct(fiftyPence);

        //Assert
        sutResult.ShouldBeEquivalentTo(vendOutcome);
    }
    
    // Pay For Product Happy Path - Over Paid - Returns Product & Change
    [Theory]
    [AutoMoqData]
    public void PayForProduct_WhenOverPaid_ReturnVendOutcomeWithProduct(
        [Frozen] Mock<ICashierService> cashierService,
        VendingMachine sut, UnitedKingdomCoins ukCoins)
    {
        // Arrange
        var twoPound = ukCoins.TwoPound;
        
        var product = new Product()
        {
            Name = "Cola",
            Cost = 1.00
        };

        var requiredFigure = twoPound.Value - product.Cost;
        
        var vendOutcome =  new VendOutcome()
        {
            Product = product,
            Message = $"{product.Name} -- {product.Cost}",
            ReturnedChange = requiredFigure
        };
        
        cashierService.Setup(x => x.Insert(twoPound, product)).Returns((true, true, twoPound.Value));
        cashierService.Setup(x => x.CheckTransaction(true, true, twoPound.Value, product)).Returns(vendOutcome);
        sut.SetCurrentProduct(product);
        
        //Act
        var sutResult = sut.PayForProduct(twoPound);

        //Assert
        sutResult.ShouldBeEquivalentTo(vendOutcome);
    }
    
    // Pay For Product Sad Path - Invalid Coin - Returns No Product
    [Theory]
    [AutoMoqData]
    public void PayForProduct_WhenCoinIsInvalid_ReturnVendOutcomeWithNoProduct(
        [Frozen] Mock<ICashierService> cashierService,
        VendingMachine sut, UnitedKingdomCoins ukCoins)
    {
        // Arrange
        var invalidCoin = ukCoins.Invalid;
        
        var product = new Product()
        {
            Name = "Cola",
            Cost = 1.00
        };

        var requiredFigure = invalidCoin.Value - product.Cost;
        
        var vendOutcome =  new VendOutcome()
        {
            Product = product,
            Message = $"{product.Name} -- {product.Cost}",
            ReturnedChange = requiredFigure
        };
        
        cashierService.Setup(x => x.Insert(invalidCoin, product)).Returns((false, false, invalidCoin.Value));
        cashierService.Setup(x => x.CheckTransaction(false, false, invalidCoin.Value, product)).Returns(vendOutcome);
        sut.SetCurrentProduct(product);
        
        //Act
        var sutResult = sut.PayForProduct(invalidCoin);

        //Assert
        sutResult.ShouldBeEquivalentTo(vendOutcome);
    }
}