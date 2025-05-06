using System.Numerics;

class Calculator
{
	public T Add<T>(T a, T b)  where T : IAdditionOperators<T, T, T> 
    {
		return a + b;
	}
}
class Program {
	static void Main() {
		Calculator calc = new();
		calc.Add<int>(3,4);
		calc.Add<float>(3.0f, 2.0f);
		calc.Add<decimal>(3.0M, 2.0M);
		calc.Add<double>(3.0, 2.0);
	}	
}