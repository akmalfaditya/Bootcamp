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
		
		House house2 = new House();
		house2.ReplaceDoor(door);
		Console.WriteLine(house2.totalDoor); //OUTPUT:
	}
}