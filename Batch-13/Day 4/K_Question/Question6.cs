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
		ElectricEngine ee = new ElectricEngine();
		ee.Start(); //Electric Engine Start (Hanun Bima) | Engine Start (sisanya) | ProgramBingung (Maruf)
		
		// ElectricEngine ee2 = new Engine(); //Maruf
		// ee2.Start();
		
		Engine engine = new Engine();
		engine.Start(); //Engine Start (7) benar semua
		
		Engine engine2 = new ElectricEngine();
		engine2.Start(); //Engine Start (7) benar semua
		

	}
}