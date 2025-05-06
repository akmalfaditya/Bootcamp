class ParentKandung {
	public void Running() {
		Console.WriteLine("Parent Running");
	}
}
class ParentAngkat {
	public void Running() {
		Console.WriteLine("Parent Angkat Running");
	}
}
class Child : ParentKandung, ParentAngkat '
{

}
Child child = new Child();
child.Running() ;