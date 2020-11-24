using System;
using System.Collections.Generic;
using BusinessLayer;
using DatabaseService.Gateway;

namespace DatabaseTester
{
    class Program
    {
        static void Main(string[] args)
        {
            User.Create("Dominik", "Guy", "abc123", DataTypes.UserRole.Coach);
            User.Create("Adam", "Guy", "abc123", DataTypes.UserRole.Coach);
            User.Create("Adam", "Test", "abc123", DataTypes.UserRole.Coach);
        }
    }
}
