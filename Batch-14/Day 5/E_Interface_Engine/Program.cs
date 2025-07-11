interface IEngine {
	void Start();
	void Stop();
}
class ElectricEngine : IEngine {
	public void Start() {
		
	}
	public void Stop() {
	
	}
}
class Program {
	static void Main() {
		IEngine engine = new ElectricEngine();
	}
}