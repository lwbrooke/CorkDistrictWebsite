using CorkDistrict.Models;


namespace CorkDistrict.Migrations.MigrationsUsers
{
    using System.Data.Entity.Migrations;

    internal sealed class UserConfig : DbMigrationsConfiguration<CorkDistrict.Models.ApplicationDbContext>
    {
        public UserConfig()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "CorkDistrict.Models.ApplicationDbContext";
        }

        protected override void Seed(CorkDistrict.Models.ApplicationDbContext context)
        {

            var idManager = new IdentityManager();
            idManager.CreateRole("Admin");
            idManager.CreateRole("Winery");
            idManager.CreateRole("User");

            for (int i = 1; i <= 10; i++ )
            {
                AddUserAndRoles("Cust", i, new string[] { "User" } );
                AddUserAndRoles("Winery", i, new string[] { "Winery" });
            }

            AddUserAndRoles("Admin", 0, new string[] { "Admin", "User"});

            AddUserAndRoles(new ApplicationUser() { UserName = "01A", Name = "Arbor Crest", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "01B", Name = "Arbor Crest", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "002", Name = "Barili Cellars", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "003", Name = "Barrister Winery", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "004", Name = "Bridge Press Cellars", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "005", Name = "Cougar Crest Winery", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "006", Name = "Emvy Cellars", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "007", Name = "Grande Ronde Cellars", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "008", Name = "Knipprath Cellars", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "009", Name = "Latah Creek Wine Cellars", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "010", Name = "Liberty Lake Wine Cellars", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "011", Name = "Nodland Cellars", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "012", Name = "Overbluff Cellars", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "013", Name = "Patit Creek Cellars", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "014", Name = "Robert Karl Cellars", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "015", Name = "Townshend Cellar", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "016", Name = "V du V Wines", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "017", Name = "Vintage Hill Cellars", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "018", Name = "Whitestone Winery", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "019", Name = "Nectar Tasting Room", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "020", Name = "Anelare Winery", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "021", Name = "Hard Row To Hoe", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "022", Name = "Northwest Cellars", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "023", Name = "Skylite Cellars", Email = "todo@example.com" }, new string[] { "Winery" });
            AddUserAndRoles(new ApplicationUser() { UserName = "024", Name = "Terra Blanca Winery", Email = "todo@example.com" }, new string[] { "Winery" });
        }

        bool AddUserAndRoles(string name, int i, string[] roles)
        {

            bool success = false;

            var idManager = new IdentityManager();

            var newUser = new ApplicationUser()
            {
                UserName = string.Format("{0}{1}", name, i.ToString()),
                Name = string.Format("{0}{1}Name", name, i.ToString()),
                Email = string.Format("{0}{1}Email@example.com", name, i.ToString())                
            };

            // Be careful here - you  will need to use a password which will 
            // be valid under the password rules for the application, 
            // or the process will abort:
            success = idManager.CreateUser(newUser, "password");

            if (!success) return success;

            foreach(var role in roles)
            {
                success = idManager.AddUserToRole(newUser.Id, role);
                if (!success) return success;
            }
            
            return success;
        }

        bool AddUserAndRoles(ApplicationUser user, string[] roles)
        {

            bool success = false;

            var idManager = new IdentityManager();

            // Be careful here - you  will need to use a password which will 
            // be valid under the password rules for the application, 
            // or the process will abort:
            success = idManager.CreateUser(user, "password");

            if (!success) return success;

            foreach (var role in roles)
            {
                success = idManager.AddUserToRole(user.Id, role);
                if (!success) return success;
            }

            return success;
        }
    }
}
