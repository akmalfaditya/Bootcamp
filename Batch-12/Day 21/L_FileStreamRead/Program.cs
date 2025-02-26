using System.Text;

class Program 
{
	static void Main() 
	{
		string path = @".\nyFile.txt";
		using (FileStream fs = new(path,FileMode.OpenOrCreate)) 
		{
			byte[] myBytes = new byte[fs.Length];
			fs.Read(myBytes, 0, myBytes.Length);
			string result = Encoding.UTF8.GetString(myBytes);
			Console.WriteLine(result);
		}	
	}
}