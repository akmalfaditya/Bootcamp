using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

[DataContract]
public class Human
{
	[DataMember]
	private string _name;
	[DataMember]
	private int _age;
	[DataMember]
	public HumanType Type { get; set; }
	public Human(string name, int age, HumanType humanType)
	{
		_name = name;
		_age = age;
		Type = humanType;
	}
	public string GetName() => _name;
	public int GetAge() => _age;
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
		Human human = new Human("Prabowi", 72, HumanType.Dinasti);

		DataContractJsonSerializer dataContract = new(typeof(Human
		));
		//Write
		using (FileStream fs = new("./myPresident.json", FileMode.Create))
		{
			dataContract.WriteObject(fs, human);
		}

		//Read
		Human human2;
		using (FileStream fs = new("./myPresident.json", FileMode.Open))
		{
			human2 = (Human)dataContract.ReadObject(fs);
		}
		Console.WriteLine(human.GetName());
		Console.WriteLine(human.GetAge());

	}
}