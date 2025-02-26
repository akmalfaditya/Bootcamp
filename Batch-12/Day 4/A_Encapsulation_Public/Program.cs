//Public
public class Car {
	public int price;
	public string brand;
	public string colour;
	public void Start() {
		Console.WriteLine("Start");
	}
}
class Program {
	static void Main() {
		Car car = new Car();
		car.price = 20;
		Console.WriteLine(car.price);
	}
}