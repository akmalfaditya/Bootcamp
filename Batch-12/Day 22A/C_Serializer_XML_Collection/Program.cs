using System.IO;
using System.Xml.Serialization;

public class Human 
{
	public string Name { get; set; }
	public int Age { get; set; }
	public Human(string name, int age) 
	{
		Name = name;
		Age = age;
	}
	public Human() { }
}
class Program 
{
	static void Main()
	{

		Human human = new Human("Jan", 2);
		Human human2 = new Human("Gibr4n", 36);
		Human human3 = new Human("Kaseang", 29);

		List<Human> futurePresident = new();
		futurePresident.Add(human);
		futurePresident.Add(human2);
		futurePresident.Add(human3);
		
		XmlSerializer serializer = new(typeof(List<Human>));

		using (FileStream fs = new("./human.txt", FileMode.Create))
		{
			serializer.Serialize(fs, futurePresident);
		}
		
	}
}