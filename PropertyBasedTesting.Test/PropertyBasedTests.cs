
using FsCheck;
using FsCheck.Xunit;

namespace PropertyBasedTesting.Test;

public class PropertyBasedTests
{
    private readonly Calculator calc = new();
    
    [Property(MaxTest = 10000, StartSize = -10000, EndSize = 10000, QuietOnSuccess = false, Verbose = false)]
    public Property TestAddition(int x)
    {
        return
            (calc.Add(1, x) == calc.Add(x, 1))
            .Classify(x < 0, "Negative numbers'")
            .Classify(x >= 0, "Positive numbers");
    }

    [Property(MaxTest = 10000, StartSize = -10000, EndSize = 10000, QuietOnSuccess = false, Replay = "1128017333,297241173")]
    public Property TestAdditionObscureBugScenario(int x)
    {
        return (calc.Add(1, x) == calc.Add(x, 1)).ToProperty();
    }

    // Adding zero always same value
    // Adding negative equivalent always comes out at 0
    // Add 1 by 1 approach to verify same result

    [Property(MaxTest = 10000, Verbose = false)]
    public Property TestDivisionToRequiredAccuracy(NormalFloat x, NormalFloat y)
    {
        return ValidateDecimalAccuracy(x.Get, y.Get, 12);
    }

    [Property(MaxTest = 10000, Verbose = false, Replay = "260480725,297241547")]
    public Property TestDivisionToRequiredAccuracyTicklesFpError(NormalFloat x, NormalFloat y)
    {
        return ValidateDecimalAccuracy(x.Get, y.Get, 10);
    }

    private Property ValidateDecimalAccuracy(double x, double y, int decimalPlaces = 10)
    {
        var decimalDivide = () => Decimal.Divide(new Decimal(x), new Decimal(y == 0 ? 1.0 : y)).ToString($"F{decimalPlaces}");

        var prop = () => (calc.Divide(x, y, decimalPlaces) == decimalDivide());
        return prop
            .When(y != 0)
            .Label($"{x} / {y} = {calc.Divide(x, y == 0 ? 1 : y, decimalPlaces)} vs {decimalDivide()}");

    }
}