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
            var db = JSONDatabase.Instance;

            db.BeginTransaction();
            db.EndTransaction();
        }
    }
}
