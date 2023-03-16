using VendingMachineKata.Models;
using VendingMachineKata.Models.Coins;
using VendingMachineKata.Models.Coins.Validation;
using VendingMachineKata.Models.Products;

namespace VendingMachineKata.Services;

public interface ICashierService
{
    (bool releaseProduct, bool validCoin, double figureRequired) Insert(Coin insertedCoins, Product itemSelected);

    VendOutcome CheckTransaction(bool releaseProduct, bool validCoin, double figureRequired, Product currentProduct);
}

public class CashierService : ICashierService
{
    private double _chamberCount;
    
    private readonly ISelectionService _selectionService;

    public CashierService(ISelectionService selectionService)
    {
        _selectionService = selectionService;
        _chamberCount = 00.00;
    }
    
    public (bool releaseProduct, bool validCoin, double figureRequired) Insert(Coin insertedCoin, Product? itemSelected)
    {
        if (!CoinValidation.ValidCoins().Where(x => x.Value == insertedCoin.Value).Any())
        {
            return (false, false, itemSelected!.Cost);
        }
        
        // Update Inserted Amount
        UpdateInsertedAmount(insertedCoin.Value);

        if (itemSelected != null)
        { 
            // Check Inserted Amount Against Product Price
            var paidAmount = CheckIfChamberCountEqualsProductValue(itemSelected);

            if (paidAmount == "More" || paidAmount == "Less")
            {
                return (CalculateTheDifference(itemSelected, paidAmount).releaseProduct, true, CalculateTheDifference(itemSelected, paidAmount).figureRequired);
            }
            return (true, true, 00.00);
        }
        
        return (false, true, _chamberCount);
    }

    private string CheckIfChamberCountEqualsProductValue(Product itemSelected)
    {
        if (_chamberCount == itemSelected.Cost)
        {
            return "Same";
        }

        if (_chamberCount < itemSelected.Cost)
        {
            return "Less";
        }

        if (_chamberCount > itemSelected.Cost)
        {
            return "More";
        }

        return "";
    }

    private (bool releaseProduct, double figureRequired) CalculateTheDifference(Product itemSelected, string paidAmount)
    {
        if (paidAmount == "More")
        {
            return (true, _chamberCount - itemSelected.Cost);
        }

        return (false, itemSelected.Cost - _chamberCount);
    }

    private double ReturnChange(Product product)
    {
        return _chamberCount - product.Cost;
    }

    public VendOutcome CheckTransaction(bool releaseProduct, bool validCoin, double figureRequired, Product currentProduct)
    {
        // Release Product, Exact Amount is Given
        if (releaseProduct && figureRequired == 00.00)
        {
            return _selectionService.DropSelection(currentProduct, figureRequired);
        }

        // Release Product & Return Change
        if (releaseProduct && figureRequired > 00.00)
        {
            ReturnChange(currentProduct);
            ResetCoinChamberCount();
            return _selectionService.DropSelection(currentProduct, figureRequired);
        }
        
        // Dont release, amount inserted is not enough
        if (!releaseProduct && validCoin &&
            figureRequired > 00.00)
        {
            return new VendOutcome()
            {
                Product = null,
                Message = $"Required Amount: {figureRequired}",
                ReturnedChange = 00.00
            };
        }

        // Dont release, invalid coin inserted
        if (!releaseProduct && !validCoin)
        {
            return new VendOutcome()
            {
                Product = null,
                Message = "Invalid Insertion",
                ReturnedChange = figureRequired
            };
        }

        return default;
    }

    public double ReturnAllInsertedCoins()
    {
        return _chamberCount;
    }

    private void UpdateInsertedAmount(double value)
    {
        _chamberCount =+ value;
    }

    private void ResetCoinChamberCount()
    {
        _chamberCount = 00.00;
    }
}