namespace Cotizaciones.Dtos
{
    public class UserToAddDto
    {
    public string FullName { get; set; }
    public string Email { get; set; }
    public UserToAddDto()
    {
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