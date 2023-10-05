
using FsCheck;
using FsCheck.Xunit;

namespace PropertyBasedTesting.Test;

public class PropertyBasedTest
{
    private readonly Calculator calc = new Calculator();
    
    [Fact]
    public void BasicAdditionTest()
    {
        Assert.Equal(5, calc.Add(4, 1));
    }

    [Theory]
    [InlineData(5,4,1)]
    [InlineData(5,1,4)]
    [InlineData(-10,-6,-4)]
    [InlineData(-12,-16,4)]
    public void BasicAdditionTests(int expected, int x, int y)
    {
        Assert.Equal(expected, calc.Add(x, y));
    }


    [Property(MaxTest = 100, StartSize = -10000, EndSize = 10000, QuietOnSuccess = false, Verbose = false)]
    public Property TestAddition(int x)
    {
        return
            (calc.Add(1, x) == calc.Add(x, 1))
            .Classify(x > 1, "Greater than 1'")
            .Classify(x < 50, "Less than 50");
    }

    [Property(MaxTest = 10000, StartSize = -10000, EndSize = 10000, QuietOnSuccess = false, Replay = "1128017333,297241173")]
    public Property TestAdditionObscureBugScenario(int x)
    {
        return (calc.Add(1, x) == calc.Add(x, 1)).ToProperty();
    }

    [Property(MaxTest = 10000, Verbose = false)]
    public Property TestDivisionToRequiredAccuracy(NormalFloat x, NormalFloat y)
    {
        return ValidateDecimalAccuracy(x.Get, y.Get, 8);
    }

    [Property(MaxTest = 10000, Verbose = false, Replay = "260480725,297241547")]
    public Property TestDivisionToRequiredAccuracyTicklesFPError(NormalFloat x, NormalFloat y)
    {
        return ValidateDecimalAccuracy(x.Get, y.Get, 10);
    }

    private Property ValidateDecimalAccuracy(double x, double y, int decimalPlaces = 10)
    {
        var decimalDivide = () => Decimal.Divide(new Decimal(x), new Decimal(y == 0 ? 1.0 : y)).ToString($"F{decimalPlaces}");

        var prop = () => (calc.Divide(x, y, decimalPlaces) == decimalDivide());
        return prop
            .When(y != 0 && y != -0)
            .Label($"{x} / {y} = {calc.Divide(x, y == 0 ? 1 : y, decimalPlaces)} vs {decimalDivide()}");

    }
}