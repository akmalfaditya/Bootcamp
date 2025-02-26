//Private Naming Convention
public class Car {
	private int _price;
	public Car(int price) {
		_price = price;
	}
}
class Program {
	static void Main() {
		Car car = new Car(2000);
	}
}