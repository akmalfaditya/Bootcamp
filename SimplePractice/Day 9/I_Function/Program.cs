//public delegate int MyDelegate(int a, int b);
class Program {
	static void Main() {
		Func<int,int,int> del = Add;
		int result = del.Invoke(3,4);
	}
	static int Add(int a, int b) {
		return (a+b);
	}
}