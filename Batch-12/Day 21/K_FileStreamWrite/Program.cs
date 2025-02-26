using System.Text;

class Program 
{
	static void Main() 
	{
		string path = @".\nyFile.txt";
		using (FileStream fs = new(path,FileMode.OpenOrCreate)) 
		{
			string myString = "uhui";
			byte[] bytes = Encoding.UTF8.GetBytes(myString);
			fs.Write(bytes,0, bytes.Length);
		}	
	}
}