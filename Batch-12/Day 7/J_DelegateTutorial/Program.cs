
public delegate void MyDelegate();

class Program {
	static void Main() {
		Radio radio = new();
		MyDelegate myDel = radio.TurnOn;
		myDel();
	}
}
class Radio {
	public void TurnOn() {
        Console.WriteLine("On");
	}
}