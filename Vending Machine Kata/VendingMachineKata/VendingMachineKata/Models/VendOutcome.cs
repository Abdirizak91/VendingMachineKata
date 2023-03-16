using VendingMachineKata.Models.Products;

namespace VendingMachineKata.Models;

public class VendOutcome
{
    public Product Product { get; set; }
    public string Message { get; set; } = string.Empty;
    
    public double ReturnedChange { get; set; }
}