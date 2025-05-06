using System.IO;
//XMLSerializer
//Public
//Serialize Variable / Property
//Method ? Tidak bisa
using System.Xml.Serialization;

public class Human 
{
	public string Name { get; set; }
	public int Age { get; set; }
	
}
class Program 
{
	static void Main()
	{
		Human human = new Human();
		human.Name = "Joko W";
		human.Age = 63;

		XmlSerializer serializer = new(typeof(Human));

		using (FileStream fs = new("./human.txt", FileMode.Create))
		{
			serializer.Serialize(fs, human);
		}
		
	}
}