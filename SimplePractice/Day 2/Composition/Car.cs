using CarComponents;

namespace Automotive;

public class Car
{
	public Engine engine;
	public Tire tire;
	public Car(Engine engine, Tire tire) 
	{
		this.engine = engine;
		this.tire = tire;
	}
}
