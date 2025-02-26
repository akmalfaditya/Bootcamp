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
	public override string Start() {
		return "Electric Engine Start...";
	}
}
class Program {
	static void Main() {
		Engine engine = new Engine();
		Car car = new Car();
		car.Run(engine);
	}
}