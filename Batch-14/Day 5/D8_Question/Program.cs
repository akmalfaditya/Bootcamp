abstract class Engine { 
	public string type;
	public int x;
	public abstract void Start();
	public void GenerateRotation() {
		Console.WriteLine("Generate rotation...");
	}
}
abstract class Electric : Engine {
	public abstract void Charge();
}
class Tesla : Electric {
}