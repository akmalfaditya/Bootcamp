//Private
public class Human {
	private int NIK;
	public int CheckNIK(string password) {
		if(password == "Admin#1234") {
			return NIK;
		}
		return 0;
	}
	public void SetNIK(int NIKBaru) {
		if(!(NIKBaru < 0)) {
			NIK = NIKBaru;
		}
	}
}
class Program {
	static void Main() {
		Human human = new Human();
		Console.WriteLine(human.CheckNIK("Admin#1234"));
		human.SetNIK(2000);
		Console.WriteLine(human.CheckNIK("Admin#1234"));
	}
}