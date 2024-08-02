namespace Esercizio_Pizzeria_In_Forno.Models.ViewModels
{
    public class UserViewModel
    {
        public User User { get; set; }
        public IEnumerable<Role> Roles { get; set; }
        public int SelectedRoleId { get; set; }

    }
}
