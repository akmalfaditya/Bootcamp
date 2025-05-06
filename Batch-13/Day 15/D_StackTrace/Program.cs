void Main() {
	Console.WriteLine("Program running...");
	ArrayCreator arrayCreator = new();
	int result = arrayCreator.GetData();
	Console.WriteLine("Program finished!");
}
class ArrayCreator {
	public int GetData() 
	{
		int[] myArray = ReallyCreator();
		return myArray[2];
	}
	public int[] ReallyCreator() {
		return null;
	}
}