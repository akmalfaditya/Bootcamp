class HTTPRequestLibrary
{
	#region for email purpose
	public bool SendSMTP(string email, string message) => true;
	public string GetSMTP(string address, string email, string pw) => "string";
	public bool ReplyEmail(int id, string message) => true;
	public bool LoginSMTP(string username, string password) => true;
	public bool LogoutSMTP(string username) => true;
	#endregion
	
	#region for database
	public bool PostData(byte[] data, string address) => true;
	public byte[] GetData(string address) => new byte[3];
	public bool ChangeData(int id, byte[] data, string address) => true;
	public bool DeleteData(int id) => true;
	#endregion
	public Employee GetEmployeeInfo(User auth, int id)
}
class User 
{
	public string Email { get; }
	public string Password { get; }
}