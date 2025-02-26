using System.Numerics;

class Calculator
{
	public T Add<T>(T a, T b)  where T : IAdditionOperators<T, T, T> 
    {
		return a + b;
	}
	public T Substraction<T>(T a, T b)  where T : ISubtractionOperators<T, T, T> 
    {
		return a - b;
	}
}
class Program {
	static void Main() {
		Calculator calc = new();
		Car cara = new();
		Car carb = new();
		Car result = calc.Substraction(cara, carb);
		Car result2 = cara+carb;
	}	
}
class Car : IAdditionOperators<Car,Car,Car>, ISubtractionOperators<Car,Car,Car>,IMultiplyOperators<Car,Car,Car> {
	public int price = 100;
	public static Car operator+(Car a, Car b){
		int result = a.price + b.price;
		Car car = new Car();
		car.price = result;
		return car;
	}

    public static Car operator -(Car left, Car right)
    {
        int result = left.price - right.price;
		Car car = new Car();
		car.price = result;
		return car;
    }

    public static Car operator *(Car left, Car right)
    {
        int result = left.price * right.price;
		Car car = new Car();
		car.price = result;
		return car;
    }
}