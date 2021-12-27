namespace PhotoGallery.API.Models.Out
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
        public string[] Tags { get; set; }
    }
}