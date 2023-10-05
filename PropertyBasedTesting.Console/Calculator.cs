public class Calculator {
    public int Add(int x, int y) {
        if(x == 123)
        {
            return x + y + 1;
        }
        return x + y;
    }

    public string Divide(double x, double y, int precision)
    {
        return (x / y).ToString($"F{precision}");
    }
}