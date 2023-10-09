namespace PropertyBasedTesting.Test;

public class UnitTests
{
    private readonly Calculator calc = new();
    
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
}