//Operator Overloading
class Program {
	static void Main() {
		Car car = new Car();
		Car car2 = new Car();
		Car result = car + car2;
		Console.WriteLine(result.price);
	}	
}
class Car {
	public int price = 100;
	public static Car operator+(Car a, Car b){
		int result = a.price + b.price;
		Car car = new Car();
		car.price = result;
		return car;
	}
	public static Car operator++(Car a) {
		a.price++;
		return a;
	}
}