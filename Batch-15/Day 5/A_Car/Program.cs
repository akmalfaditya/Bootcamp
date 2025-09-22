class  Car 
{
	public int price;
	public Car(int price) {
		this.price = price;
	}
}
class Program 
{
	static void Main() {
		Car car = new Car(3);
		Car car2 = car;
		car2.price += 2;
		
		Console.WriteLine(car.price); // 5
		Console.WriteLine(car2.price); // 5
	}
}