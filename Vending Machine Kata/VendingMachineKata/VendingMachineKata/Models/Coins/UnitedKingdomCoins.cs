namespace VendingMachineKata.Models.Coins;

public class UnitedKingdomCoins
{
    public Coin OncePence { get;} = new() { Value = 0.01 };
    
    public Coin TwoPence { get;} = new() { Value = 0.02 };
    
    public Coin FivePence { get;} = new() { Value = 0.05 };
    
    public Coin TenPence { get;} = new() { Value = 0.10 };
    
    public Coin TwentyPence { get;} = new() { Value = 0.20 };
    
    public Coin FiftyPence { get;} = new() { Value = 0.50 };
    
    public Coin OnePound { get;} = new() { Value = 1.00 };
    
    public Coin TwoPound { get;} = new() { Value = 2.00 };

    public Coin Invalid { get; } = new() { Value = 10.00 };
}