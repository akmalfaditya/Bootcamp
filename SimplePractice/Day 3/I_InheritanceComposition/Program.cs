//Inheritance Composition
class Car {
	public Engine engine;
	public Car(Engine engine) {
		this.engine = engine;
	}
}
class Engine {
	public int size;
	public string brand;
}
class ElectricEngine : Engine {
}
class DieselEngine : Engine {} 
class PistonEngine : Engine {}
class Program {
	static void Main() {
		Engine engine = new Engine();
		Car car = new Car(engine);
		
		ElectricEngine electricEngine = new ElectricEngine();
		Car car2 = new Car(electricEngine);
	}
}