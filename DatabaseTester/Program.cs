using System;
using DatabaseService;
using DatabaseService.Gateway;
using DataTypes;
using LanguageExt;

namespace DatabaseTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var user = UserGateway.Create("John", "Wick", "Password123", UserRole.Player);
            var user2 = UserGateway.Create("John", "Wick", "Password456", UserRole.Player);

            user2.FirstName = "Jane";
            user2.LastName = "Doe";
            user2.Role = UserRole.Coach;

            UserGateway.Delete(user);
            UserGateway.Update(user2);

            var team = TeamGateway.Create("The Pros", "Counter-Strike: Global Offensive", Option<User>.Some(user2));
        }
    }
}
