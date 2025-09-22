interface IEngine {
	void Start();
}
interface IElectric {
	void Charge();
}
class Tesla : IEngine, IElectric {
	public void Start() {
		Console.WriteLine("Start");
	}
	public void Charge() {
		Console.WriteLine("Charge");
	}
}