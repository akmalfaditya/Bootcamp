class Car {
	public int productionNumber;
	public static int count = 0 ;
	public Car() {
		count++;
		productionNumber = count;
	}
	public int GetNumber() {
		return count;
	}
	public static int GetNumbers() {
		//return productionNumber; it will be error
		return count;
	}
}
class Program {
	static void Main() {
		Car car = new();
		Console.WriteLine(car.productionNumber);
		Console.WriteLine(Car.count);
		
		Car car2 = new();
		Console.WriteLine(car2.productionNumber);
		Console.WriteLine(Car.count);
		
		Car car3 = new();
		Console.WriteLine(car3.productionNumber);
		Console.WriteLine(Car.count);
	}
}