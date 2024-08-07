namespace SosyalMedyaMVC.Models
{

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Bio { get; set; }
        public string FullName => $"{Name} {SurName}"; //İsim soyismi birleştirip kullanmak için oluşturdum.Databaseye kayıt olmuyor.
    }

}
