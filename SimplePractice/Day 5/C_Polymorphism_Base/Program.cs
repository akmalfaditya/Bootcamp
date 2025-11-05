class Parent {
	public virtual void Kuliah() {
		Console.WriteLine("UGM");
	}
}
class Child : Parent {
	public override void Kuliah() {
		Console.WriteLine("Undip");
	}
	public void CheckParentKuliah() {
		base.Kuliah();
	}
}
class Program {
	static void Main() {
		Child child  = new Child();
		child.Kuliah(); //Undip
		child.CheckParentKuliah(); //UGM
	}
}