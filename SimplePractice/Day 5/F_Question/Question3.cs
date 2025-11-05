interface IEngine {
	private void Start();
}
class Engine : IEngine {
	public void Start() {
		Console.WriteLine("Start");
	}
}