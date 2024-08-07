namespace SosyalMedyaMVC.Models
{
    public class SendMessageViewModel
    {
        public string Receiver { get; set; }
        public string Content { get; set; }
        public List<UserViewModel> AllUsers { get; set; }
    }
}
