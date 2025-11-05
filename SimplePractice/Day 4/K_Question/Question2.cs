class Car {
	public void Run(Engine engine) {
		engine.Start();
	}
}
class Engine {
	public virtual void Start(string a) {
		Console.WriteLine(a);
	}
}
class ElectricEngine : Engine {
	public override void Start(string b) {
		Console.WriteLine(b);
	}
}
class Program {
	static void Main() {
		Engine engine = new Engine();
		Car car = new Car();
		car.Run(engine);
	}
}