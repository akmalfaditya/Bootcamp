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
		XmlSerializer serializer = new(typeof(Human));
		Human human;
		using (FileStream fs = new("./human.txt", FileMode.Open))
		{
			human = (Human)serializer.Deserialize(fs);
		}
		Console.WriteLine(human.Name);
		Console.WriteLine(human.Age);
	}
}