namespace VendingMachineKata.Models.Coins.Validation;

public static class CoinValidation
{
    public static List<Coin> ValidCoins()
    {
        var unitedKingdomCoins = new UnitedKingdomCoins();
        
        var listOfAcceptableCoins = new List<Coin>();
        
        listOfAcceptableCoins.Add(unitedKingdomCoins.OncePence);
        listOfAcceptableCoins.Add(unitedKingdomCoins.TwoPence);
        listOfAcceptableCoins.Add(unitedKingdomCoins.FivePence);
        listOfAcceptableCoins.Add(unitedKingdomCoins.TenPence);
        listOfAcceptableCoins.Add(unitedKingdomCoins.TwentyPence);
        listOfAcceptableCoins.Add(unitedKingdomCoins.FiftyPence);
        listOfAcceptableCoins.Add(unitedKingdomCoins.OnePound);
        listOfAcceptableCoins.Add(unitedKingdomCoins.TwoPound);

        return listOfAcceptableCoins;
    }
}