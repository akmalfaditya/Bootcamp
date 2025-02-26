public interface IEngine {
	void Start();
}
class Engine : IEngine {
	public void Start() {
		Console.WriteLine("Engine Start");
	}
	public void Stop() { }
}
class Program 
{
	static void Main() 
	{
		IEngine engine = new Engine();
		engine.Start();
		engine.Stop();
	}
}