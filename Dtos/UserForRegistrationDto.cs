namespace Cotizaciones
{
public partial class UserForRegistrationDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
        public UserForRegistrationDto(){
            if (Email == null){
                Email = "";
            }
            if (Password == null){
                Password = "";
            }
            if (PasswordConfirm == null){
                PasswordConfirm = "";
            }
            if (FullName == null){
                FullName = "";
            }
        }
}
}