using System;
using DataTypes;
using LanguageExt;
using DatabaseService.Gateway;
using System.Collections.Generic;

namespace BusinessLayer
{
    public class User : DataTypes.User
    {
        public void Save()
        {
            UserGateway.Update(this);
        }

        public void Delete()
        {
            UserGateway.Delete(this);
        }

        public List<Team> GetTeams()
        {
            var ttps = TeamToPlayerGateway.FindByPlayer(this);
            return ttps.Map(
                (ttp) => TeamGateway.Find(ttp.TeamID.Value)
            ).Somes().Map(
                (x) => new Team(x)
            ).ToArr().ToList();
        }

        public User(DataTypes.User user)
        {
            this.UserID = user.UserID;
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Login = user.Login;
            this.Password = user.Password;
            this.Role = user.Role;
        }

        public static User Create(string firstName, string lastName, string password, UserRole role)
        {
            var login = UserGateway.GetLoginFor(firstName, lastName);
            return new User(UserGateway.Create(login, firstName, lastName, password, role));
        }

        public static Option<User> Find(int id)
        {
            var user = UserGateway.Select(id);
            return user.Map((usr) => new User(usr));
        }
    }
}