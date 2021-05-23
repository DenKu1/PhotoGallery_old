namespace PhotoGallery.API.Models.Out
{
    public class UserWithTokenModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
        public string Token { get; set; }
    }
}
