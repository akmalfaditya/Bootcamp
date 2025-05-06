// Multicast Delegate
public delegate void MyDelegate();

class Program {
	static void Main() {
		Radio radio = new();
		Radio radio2 = new();
		TV tv = new();
		MyDelegate myDel = radio.TurnOn;
		myDel += radio2.TurnOn;
		myDel += tv.On;
		myDel();
	}
}
class Radio {
	public void TurnOn() {
		"On".Dump();
	}
}
class TV {
	public void On() {
		"TV On".Dump();
	}
}