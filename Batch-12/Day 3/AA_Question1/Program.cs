class House 
{
	public int totalDoor = 2;
	public void ReplaceDoor(int door) {
		totalDoor = door;
	}
}
class Program {
	static void Main() {
		int totalDoor = 10;
		int door = 5;
		House house = new House();
		house.ReplaceDoor(door);
		Console.WriteLine(house.totalDoor);
	}
}