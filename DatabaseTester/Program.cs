using System;
using BusinessLayer;

namespace DatabaseTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var user = User.Create("Adam", "Dobry", "123456", DataTypes.UserRole.Player);
            var team = Team.Create("Bobri", "CS:GO");

            team.Players.Add(user);

            team.Save();
        }
    }
}
