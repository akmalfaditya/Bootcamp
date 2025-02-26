class Program {
	static void Main() {
		StatusChecker stats = new();
		Status status = Status.lI1l1i1;
		stats.Check(status);
	}
}
public enum Status {
	NotFound,
	Redirected,
	WrongPassword,
	lI1l1i1
}
class StatusChecker {
	public void Check(Status status) {
		if(status == Status.NotFound) {
			Console.WriteLine("Error : Not Found");
		}
		else if(status == Status.Redirected) {
			Console.WriteLine("Warning : Redirected");
		}
		else if(status == Status.WrongPassword) {
			Console.WriteLine("Error : Wrong Password");
		}
		else if(status == Status.lI1l1i1) {
			Console.WriteLine("Error : Wrong lI1l1i1");
		}
		else {
			Console.WriteLine("Status not found");
		}
	}
}