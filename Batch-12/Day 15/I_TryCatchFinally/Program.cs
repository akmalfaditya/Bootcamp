void Main() {
	Console.WriteLine("Program running...");
	try {
		int[] myArray = null;
		myArray[4].Dump();
	}
	catch(FormatException e) {
		Console.WriteLine("General Exception");
		Console.WriteLine(e.Message);
	}
	finally {
		"...".Dump();
	}

	Console.WriteLine("Program finished!");
}