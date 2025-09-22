//Constructor Overloading
class Home 
{
	public int door;
	public int window;
	public string color;
	public string address;
	public Home(int door, 
				int window, 
				string color, 
				string address
				) 
	{
		this.door = door;
		this.window = window;
		this.color = color;
		this.address = address;
		Console.WriteLine($"Home Created : {door} {window} {color} {address}");
	}
	public Home( int door, int window ) {
		this.door = door;
		this.window = window;
		Console.WriteLine("Home Created");
	}
	public Home() { 
		Console.WriteLine("Home Created");
		}
}
class Program {
	static void Main() {
		Home home = new Home(2,4,"black","salatiga");
		//Console.WriteLine(home.door);
		//Console.WriteLine(home.window);
		//Console.WriteLine(home.color);
		//Console.WriteLine(home.address);
		//
		//Home home2 = new Home(2,4);
		//
		//Home home3 = new Home();
		
	}
}