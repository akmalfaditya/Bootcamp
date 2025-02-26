class Program 
{
	static async void Main() 
	{
		DatabaseManagement databaseManagement = new();
        string x = await databaseManagement.ReaderAsync();
	}
}
class DatabaseManagement {
	public async Task<string> ReaderAsync() {
		await Task.Delay(5000);
		return "database data";
	}
}