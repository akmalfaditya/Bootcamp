void Main() {
	string myString = "40z";
	bool status = int.TryParse(myString, out int value);
	if(status) {
		Console.WriteLine(value);
	}
	else {
		Console.WriteLine($"{value} is 0");
	}
}