using VendingMachineKata.Models;
using VendingMachineKata.Models.Coins;
using VendingMachineKata.Models.Products;
using VendingMachineKata.Services;
using ISelectionService = VendingMachineKata.Services.ISelectionService;

namespace VendingMachineKata;

public class VendingMachine
{
    private Product CurrentSelectedProduct { get; set; }
    private string CurrentMessage { get; set; }
    
    private readonly ISelectionService _selectionService;
    
    private readonly ICashierService _cashierService;
    
    public VendingMachine(ICashierService cashierService, ISelectionService selectionService)
    {
        _cashierService = cashierService;
        _selectionService = selectionService;
        DefaultScreen();
    }
    
    private void DefaultScreen()
    {
        CurrentMessage = "INSERT COIN";
    }
    
    public VendOutcome SelectProduct(string productName)
    {
        var selectedProduct = _selectionService.Selection(productName);
        SetCurrentProduct(selectedProduct.Product);
        CurrentMessage = selectedProduct.Message;
        return selectedProduct;
    }

    public VendOutcome PayForProduct(Coin insertedCoin)
    {
        var insertCoinResponse = _cashierService.Insert(insertedCoin, CurrentSelectedProduct);

        var vendOutcome = _cashierService.CheckTransaction(insertCoinResponse.releaseProduct, insertCoinResponse.validCoin,
            insertCoinResponse.figureRequired, CurrentSelectedProduct);

        CurrentSelectedProduct = null;
        
        DefaultScreen();

        return vendOutcome;
    }

    public void SetCurrentProduct(Product product)
    {
        CurrentSelectedProduct = product;
    }
}