abstract class Engine { //Base class
	public string type;
	public abstract void Start();
	public abstract void Stop();
	public void GenerateRotation() {
		Console.WriteLine("Generate rotation...");
	}
}
class Diesel : Engine {
	public override void Start() {
		Console.WriteLine("Diesel start");
	}
	public override void Stop() {
		Console.WriteLine("Diesel stop");
	}
}
class V8 : Engine {
	public override void Start() {
		Console.WriteLine("V8 start");
	}
	public override void Stop() {
		Console.WriteLine("V8 stop");
	}
}
class Electric : Engine {
	public override void Start() {
		Console.WriteLine("Electric start");
	}
	public override void Stop() {
		Console.WriteLine("Electric stop");
	}
}
class Car {
	public Engine engine;
	public Car(Engine engine) {
		this.engine = engine;
	}
}
class Program {
	static void Main() {
		Electric electric = new Electric();
		Car car = new Car(electric);
	}
}