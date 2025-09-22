interface IEngine {
	void Start();
}
class Engine : IEngine {
	public void Start() { }
	public void Stop() { }
}
class Car {
	public IEngine engine;
	public Car(IEngine engine) {
		this.engine = engine;
	}
	public void StartEngine() {
		engine.Start();
	}
}
class Program {
	static void Main( ){
		Car car = new Car( new Engine () );
		car.StartEngine();
	}
}