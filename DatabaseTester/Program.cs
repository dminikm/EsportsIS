using System;
using System.Collections.Generic;
using BusinessLayer;
using DatabaseLayer.Gateway;

namespace DatabaseTester
{
    class Program
    {
        static void Main(string[] args)
        {
            // Management

            User.Create("Radka", "Studena", "abc123", DataTypes.UserRole.Manager);
            User.Create("Jiri", "Otevrel", "abc123", DataTypes.UserRole.Manager);

            // Team 1
            var c1 = User.Create("Vladimir", "Hronek", "abc123", DataTypes.UserRole.Coach);
            var t1 = Team.Create("Bobri", "CS:GO", c1);

            t1.Players.Add(User.Create("Jaroslav", "Schreier", "abc123", DataTypes.UserRole.Player));
            t1.Players.Add(User.Create("Jaroslav", "Nemec", "abc123", DataTypes.UserRole.Player));
            t1.Players.Add(User.Create("Josef", "Tobias", "abc123", DataTypes.UserRole.Player));
            t1.Players.Add(User.Create("Robert", "Smrcka", "abc123", DataTypes.UserRole.Player));
            t1.Players.Add(User.Create("Josef", "Krotky", "abc123", DataTypes.UserRole.Player));

            t1.Save();

            // Team 2
            var c2 = User.Create("Jana", "Jelinkova", "abc123", DataTypes.UserRole.Coach);
            var t2 = Team.Create("Jesterky", "LOL", c2);

            t2.Players.Add(User.Create("Viktorie", "Jindrova", "abc123", DataTypes.UserRole.Player));
            t2.Players.Add(User.Create("Alena", "Knotova", "abc123", DataTypes.UserRole.Player));
            t2.Players.Add(User.Create("Renata", "Patkova", "abc123", DataTypes.UserRole.Player));
            t2.Players.Add(User.Create("Zdenka", "Berkova", "abc123", DataTypes.UserRole.Player));
            t2.Players.Add(User.Create("Sarka", "Jarkovska", "abc123", DataTypes.UserRole.Player));

            t2.Save();

            // Team 3

            User.Create("Vera", "Nguyenova", "abc123", DataTypes.UserRole.Coach);
            User.Create("Sarlota", "Valesova", "abc123", DataTypes.UserRole.Player);
            User.Create("Hana", "Vykoukalova", "abc123", DataTypes.UserRole.Player);
            User.Create("Daniela", "Sehnalova", "abc123", DataTypes.UserRole.Player);
            User.Create("Jana", "Krejcova", "abc123", DataTypes.UserRole.Player);
            User.Create("Martin", "Januska", "abc123", DataTypes.UserRole.Player);
        }
    }
}
