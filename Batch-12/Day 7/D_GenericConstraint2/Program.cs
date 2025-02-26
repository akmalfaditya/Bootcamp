using System.Numerics;

class Calculator<T> where T : INumber<T>
{
	public T Add(T a, T b) {
		return a + b;
	}
	public T Substract(T a, T b) {
		return a - b;
	}
}
class Program {
	static void Main() {
		Calculator<int> calc = new();
		calc.Add(3,4);
		Calculator<float> calc2 = new();
		calc2.Add(3.0f, 2.0f);
		Calculator<decimal> calc3 = new();
		calc3.Add(3.0M, 2.0M);
		Calculator<double> calc4 = new();
		calc4.Add(3.0, 2.0);
	}	
}
class Cat {}