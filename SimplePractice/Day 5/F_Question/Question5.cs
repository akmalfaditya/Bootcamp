interface IEngine {
	void Start();
}
interface IElectric {
	void Start();
}
class Tesla : IEngine, IElectric {
	public void Start() {
		Console.WriteLine("Start");
	}
}