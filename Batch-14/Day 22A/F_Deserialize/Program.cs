using System.IO;
using System.Text;
using System.Text.Json;

//XMLSerializer
//Public
//Serialize Variable / Property
//Method ? Tidak bisa
using System.Xml.Serialization;

public class Human 
{
	public string Name { get; set; }
	public int Age { get; set; }
	public HumanType HumanType { get; set; }
}
public enum HumanType 
{
	Manusia,
	Dinasti
}
class Program 
{
	static void Main()
	{
		string result;
		using(StreamReader sr = new("./humansw.json")) 
		{
			result = sr.ReadLine();
		}
		Human human = JsonSerializer.Deserialize<Human>(result);
		Console.WriteLine(human.Name);
		Console.WriteLine(human.Age);
	}
}