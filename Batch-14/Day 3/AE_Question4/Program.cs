class House 
{
	public int totalDoor = 1;
	public void ReplaceDoor(int door) {
		totalDoor += door;
	}
}
class Program {
	static void Main() {
		int door = 5;
		House house = new House();
		house.ReplaceDoor(door);
		Console.WriteLine(house.totalDoor); //OUTPUT:
		
		door = 3;
		house = new House();
		house.ReplaceDoor(door);
		Console.WriteLine(house.totalDoor); //OUTPUT:
	}
}