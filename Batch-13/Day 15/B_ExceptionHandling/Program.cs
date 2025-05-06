void Main() {
	Console.WriteLine("Program running...");
	try {
		int[] myArray = {1,2,3};
		myArray[4].Dump();
	}
	catch(Exception e) {
		Console.WriteLine("Error happened");
		Console.WriteLine(e.Message);
	}
	Console.WriteLine("Program finished!");
}