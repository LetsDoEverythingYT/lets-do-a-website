namespace lets_do_a_website.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Displayname { get; set; }
        public string ReturnUrl { get; set; }

        public LoginModel()
        {
            Username= string.Empty;
            Displayname= string.Empty;
            ReturnUrl= string.Empty;
        }
    }
}
