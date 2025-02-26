void Main() {
	Console.WriteLine("Program running...");
	int[] myArray = OtherMethod();
	if(!(myArray == null)) {	
		myArray[4].Dump();
	}
	Console.WriteLine("Program finished!");
}
int[] OtherMethod() {
	return null;
}