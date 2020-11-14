using System;
using System.Collections.Generic;

namespace DataTypes
{
    public static class UserRoleStrings {
        public static string Player = "player";
        public static string Manager = "manager";
        public static string Coach = "coach";

        public static Dictionary<UserRole, string> roles = new Dictionary<UserRole, string>() {
            { UserRole.Player, UserRoleStrings.Player },
            { UserRole.Manager, UserRoleStrings.Manager },
            { UserRole.Coach, UserRoleStrings.Coach }
        };

        public static Dictionary<string, UserRole> rolesReverse = new Dictionary<string, UserRole>() {
            { UserRoleStrings.Player, UserRole.Player },
            { UserRoleStrings.Manager, UserRole.Manager },
            { UserRoleStrings.Coach, UserRole.Coach }
        };
    }

    public enum UserRole {
        Player,
        Manager,
        Coach
    }

    public class User
    {
        public int? ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
}
