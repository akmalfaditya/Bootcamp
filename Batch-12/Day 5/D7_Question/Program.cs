abstract class Engine { 
	public string type;
	protected abstract void Start();
	public void GenerateRotation() {
		Console.WriteLine("Generate rotation...");
	}
}
class Electric {
	public void Charge() {
		Console.WriteLine("Electric Charge");
	}
}
class Tesla : Engine, Electric {
	
}