//dynamic (forbidden)
class Program {
	static void Main() {
		dynamic value = 3;
		value = true;
		value = new Car();
		value.EngineRun();
		value.Yanto();
		value.Bima();
		value.Sanan();
		
	}
}
class Car {
	public void EngineRun() {}
}