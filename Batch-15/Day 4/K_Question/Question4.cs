class Car {
	public void Run(Engine engine) {
		engine.Start();
	}
}
class Engine {
	public virtual void Start() {
		Console.WriteLine("Engine Start...");
	}
}
class ElectricEngine : Engine {
	public override void Start() {
		Console.WriteLine("Electric Engine Start...");
	}
}
class Program {
	static void Main() {
		ElectricEngine engine = new ElectricEngine();
		Car car = new Car();
		car.Run(engine);
	}
}