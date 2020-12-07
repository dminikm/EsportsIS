using System;
using DataTypes;
using LanguageExt;
using DatabaseService.Gateway;
using System.Collections.Generic;
using System.Linq;

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
            ).ToList();
        }

        public List<Event> GetUpcomingEvents()
        {
            return EventGateway
                .FindEventsForUser(this)
                .Filter((x) => x.From > DateTimeOffset.Now.ToUnixTimeMilliseconds())
                .Map((x) => new Event(x))
                .ToList();
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
            var user = UserGateway.Find(id);
            return user.Map((usr) => new User(usr));
        }

        public static Option<User> FindByUsernamePassword(string login, string password)
        {
            var user = UserGateway.FindByUsernamePassword(login, password);
            return user.Map((usr) => new User(usr));
        }

        public static List<User> FindByRole(UserRole role)
        {
            var users = UserGateway.FindByRole(role);
            return users.Map((user) => new User(user)).ToList();
        }

        public static List<User> All()
        {
            var users = UserGateway.FindAll();
            return users.Map((user) => new User(user)).ToList();
        }
    }
}