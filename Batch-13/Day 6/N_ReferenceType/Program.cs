//Reference Type
class Program {
	static void Main() {
		Car car = new Car();
		car.price = 3;
		Adder add = new();
		add.Add(car);
		Console.WriteLine(car.price);
	}
}
class Adder {
	public int Add(Car x) {
		return x.price++;
	}
}
class Car {
	public int price;
}