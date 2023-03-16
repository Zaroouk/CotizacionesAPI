namespace Cotizaciones.Models
{
    public class User{
    public int UserId { get; set; }
    public int Avatar { get; set; }
    public string FullName { get; set; }
    public int Active {get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public User()
    {
        if(Role == null)
        {
            Role = "";
        }
        if (FullName == null)
        {
            FullName = "";
        }
        if (Email == null)
        {
            Email = "";
        }
}
}
}