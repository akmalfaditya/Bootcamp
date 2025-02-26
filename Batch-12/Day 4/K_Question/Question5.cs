class Car {
	public void Run(Engine engine) {
		engine.Start();
	}
}
class Engine {
	public void Start() {
		Console.WriteLine("Engine Start...");
	}
}
class ElectricEngine : Engine {
	public void Start() {
		Console.WriteLine("Electric Engine Start...");
	}
}
class Program {
	static void Main() {
		Engine engine = new Engine();
		Car car = new Car();
		car.Run(engine);
	}
}