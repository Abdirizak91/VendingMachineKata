using Shouldly;
using VendingMachineKata.Models;
using VendingMachineKata.Services;
using VendingMachineKataTests.AutoData;
using Xunit;

namespace VendingMachineKataTests;

public class SelectionServiceTests
{
    [Theory]
    [AutoMoqData]
    public void Selection_WhenSelectionIsSoldOut_ReturnVendOutcomeWithMessage(SelectionService selectionService)
    {
        //Arrange
        var vendSelection = "Chocolate";
        
        var vendOutcome = new VendOutcome()
        {
            Product = null,
            Message = "Item Unavailable"
        };
        
        // Act
        var selectionResult = selectionService.Selection(vendSelection);
        
        // Assert 
        selectionResult.ShouldBeEquivalentTo(vendOutcome);
    }
}