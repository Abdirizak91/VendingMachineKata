using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace VendingMachineKataTests.AutoData;

public class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute() : base(
            () =>
            {
                var fixture = new Fixture();
                fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
                return fixture;
            }){ }
    

    internal class InlineAutoMoqDataAttribute : InlineAutoDataAttribute
    {
        public InlineAutoMoqDataAttribute(params object[]? objects) : base(new AutoMoqDataAttribute(), objects) { }
    }
}