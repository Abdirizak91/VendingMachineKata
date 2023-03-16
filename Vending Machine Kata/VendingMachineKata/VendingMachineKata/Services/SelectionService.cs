using VendingMachineKata.Models;
using VendingMachineKata.Models.Products;

namespace VendingMachineKata.Services;

public interface ISelectionService
{
    public VendOutcome Selection(string product);

    VendOutcome DropSelection(Product product, double requiredFigure);
}

public class SelectionService : ISelectionService
{
    public readonly Basket _basket;

    public SelectionService(Basket basket)
    {
        _basket = basket;
        DummyStock();
    }
    
    public VendOutcome Selection(string product)
    {
        // Check if selection is available
        Product selectedProduct = _basket.Products.Where(x => x.Name == product).FirstOrDefault();
        
        if (selectedProduct != null)
        {
            // if available show cost
            VendOutcome vendOutcome = new()
            {
                Product = selectedProduct,
                Message = $"{selectedProduct.Name} -- {selectedProduct.Cost}",
            };

            return vendOutcome;
        }
        
        return new VendOutcome()
        {
            Product = null,
            Message = "Item Unavailable"
        };
    }

    public VendOutcome DropSelection(Product product, double requiredFigure)
    {
        try
        {
            // remove selected product from the list
            _basket.Products.Remove(product);
            
            // return selected item
            return new VendOutcome()
            {
                Product = product,
                Message = "Item Dropped, Thank You",
                ReturnedChange = requiredFigure
            };
        }
        catch (Exception e)
        {
            return new VendOutcome()
            {
                Product = null,
                Message = e.Message,
                ReturnedChange = requiredFigure
            };
        }
    }

    private void DummyStock()
    { 
        // 10 Colas
        for (var i = 0; i < 4; i++)
        {
            Product cola = new()
            {
                Name = "Cola",
                Cost = 1.00
            };
            _basket.Products.Add(cola);
        }
        
        // 8 Crisps
        for (var i = 0; i < 9; i++)
        {
            Product crisp = new()
            {
                Name = "Crisp",
                Cost = 0.50
            };
            _basket.Products.Add(crisp);
        }
        
        // Chocolates Sold Out
        for (var i = 0; i < 0; i++)
        {
            Product chocolate = new()
            {
                Name = "Chocolate",
                Cost = 0.65
            };
            _basket.Products.Add(chocolate);
        }
    }
}