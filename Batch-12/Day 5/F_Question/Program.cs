interface IEngine {
	public int Size; //error
	void Start();
}
class Engine : IEngine {
	public void Start() {
		Console.WriteLine("Start");
	}
}