void Main() {
	Console.WriteLine("Program running...");
	try {
		string a = "40z";
		int result = int.Parse(a);
		
		int[] myArray = null;
		myArray[4].Dump();
		
		int[] myArray2 = {1,2,3,4,5};
		myArray2[5].Dump();
	}
	catch(NullReferenceException e) {
		Console.WriteLine("NullReferenceException ...");
		Console.WriteLine(e.Message);
	}
	catch(IndexOutOfRangeException e) {
		Console.WriteLine("IndexOutOfRangeException ...");
		Console.WriteLine(e.Message);
	}
	catch(Exception e) {
		Console.WriteLine("General Exception");
		Console.WriteLine(e.Message);
	}
	Console.WriteLine("Program finished!");
}