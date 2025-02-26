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
	public string defaultt = "default"; //will not written
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
		Human human = new Human();
		human.Name = "Joko W";
		human.Age = 63;
		human.HumanType = HumanType.Dinasti;

		string result = JsonSerializer.Serialize(human);
		Console.WriteLine(result);
		using(FileStream fs = new("./human.json",FileMode.OpenOrCreate)) 
		{
			byte[] bytes = Encoding.UTF8.GetBytes(result);
			fs.Write(bytes, 0, bytes.Length);
		}
		
		using(StreamWriter sw = new("./humansw.json")) 
		{
			sw.WriteLine(result);
		}
	}
}