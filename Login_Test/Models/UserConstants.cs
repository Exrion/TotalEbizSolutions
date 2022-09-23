namespace Login_Test.Models
{
    public class UserConstants
    {
        public static List<User> Users = new List<User>()
        {
            new User() { UserName = "titus_admin", Email = "titus.lim@totalebizsolutions.com", Password = "Password123",
            GivenName = "Titus", Surname = "Lim", Role = "Admin"},
            new User() { UserName = "john_seller", Email = "john.doe@totalebizsolutions.com", Password = "Password123",
            GivenName = "John", Surname = "Doe", Role = "Seller"}
        };
    }
}
