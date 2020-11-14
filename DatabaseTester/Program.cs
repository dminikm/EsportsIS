using System;
using DatabaseService;
using DatabaseService.Gateway;

namespace DatabaseTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var user = UserGateway.Create("John", "Wick", "Password123", DataTypes.UserRole.Player);
            UserGateway.Create("John", "Wick", "Password123", DataTypes.UserRole.Player);
            UserGateway.Create("John", "Wick", "Password123", DataTypes.UserRole.Player);
            UserGateway.Create("John", "Wick", "Password123", DataTypes.UserRole.Player);
            UserGateway.Create("John", "Wick", "Password123", DataTypes.UserRole.Player);
            UserGateway.Create("John", "Wick", "Password123", DataTypes.UserRole.Player);
            UserGateway.Create("John", "Wick", "Password123", DataTypes.UserRole.Player);
            UserGateway.Create("John", "Wick", "Password123", DataTypes.UserRole.Player);
            UserGateway.Create("John", "Wick", "Password123", DataTypes.UserRole.Player);
            UserGateway.Create("John", "Wick", "Password123", DataTypes.UserRole.Player);
            UserGateway.Create("John", "Wick", "Password123", DataTypes.UserRole.Player);
            UserGateway.Create("John", "Wick", "Password123", DataTypes.UserRole.Player);
            UserGateway.Create("John", "Wick", "Password123", DataTypes.UserRole.Player);
            /*var user2 = UserGateway.Select(user.ID.Value).IfNone(new DataTypes.User());

            Console.WriteLine($"user: {user.FirstName} {user.LastName}, user2: {user2.FirstName} {user2.LastName}");*/
        }
    }
}
