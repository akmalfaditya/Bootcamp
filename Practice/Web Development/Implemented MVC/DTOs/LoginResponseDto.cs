namespace Implemented_MVC.DTOs
{
    public class LoginResponseDto
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? Message { get; set; }
        public UserDto? User { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
