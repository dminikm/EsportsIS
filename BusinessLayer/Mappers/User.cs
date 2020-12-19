using System;
using DataTypes;
using LanguageExt;
using DatabaseService.Gateway;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer
{
    public class User
    {
        public void Save()
        {
            UserGateway.Update(this.user);
        }

        public void Delete()
        {
            UserGateway.Delete(this.user);
        }

        public List<Team> GetTeams()
        {
            var ttps = TeamToPlayerGateway.FindByPlayer(this.user);
            return ttps.Map(
                (ttp) => TeamGateway.Find(ttp.TeamID.Value)
            ).Somes().Map(
                (x) => new Team(x)
            ).ToList();
        }

        public List<Event> GetUpcomingEvents()
        {
            return EventGateway
                .FindEventsForUser(this.user)
                .Filter((x) => x.From >= DateTimeOffset.UtcNow.ToUnixTimeMilliseconds())
                .Map((x) => Event.FromType(x))
                .ToList();
        }

        public List<Event> GetEventsAvailableFromTo(DateTime from, DateTime to)
        {
            return EventGateway
                .All()
                .Filter((x) =>
                    x.From <= ((DateTimeOffset)to).ToUnixTimeMilliseconds() &&
                    x.To >= ((DateTimeOffset)from).ToUnixTimeMilliseconds()
                )
                .Filter((x) => x.ParticipantIDs.Contains(this.UserID.IfNone(-1)) || x.Type == "custom")
                .Map((x) => (Event)Event.FromType(x))
                .ToList();
        }

        public List<Event> GetEventsOverlappingWith(DateTime from, DateTime to)
        {
            return EventGateway
                .FindEventsForUser(this.user)
                .Filter((x) =>
                    x.From <= ((DateTimeOffset)to).ToUnixTimeMilliseconds() &&
                    x.To >= ((DateTimeOffset)from).ToUnixTimeMilliseconds()
                ).Map((x) => Event.FromType(x))
                .ToList();
        }

        public List<Event> GetEventsOverlappingWithOfType(Event evt, string type)
        {
            return EventGateway
                .FindEventsForUser(this.user)
                .Map((x) => Event.FromType(x))
                .Filter((x) =>
                    x.From <= evt.To &&
                    x.To >= evt.From
                )
                .Filter((x) => x.EventID != evt.EventID)
                .Filter((x) => x.Type == type)
                .ToList();
        }

        public List<Event> GetEventsOverlappingWithOfType(DateTime start, DateTime end, Option<int> filterID, string type)
        {
            return this.GetEventsOverlappingWith(start, end)
                .Filter((x) => x.Type == type)
                .Filter((x) => !x.EventID.Equals(filterID))
                .ToList();
        }

        public List<Event> GetEventsOverlappingWithNotOfType(DateTime start, DateTime end, Option<int> filterID, string type)
        {
            return this.GetEventsOverlappingWith(start, end)
                .Filter((x) => x.Type != type)
                .Filter((x) => !x.EventID.Equals(filterID))
                .ToList();
        }

        public List<Event> GetEventsOverlappingWithNotOfType(Event evt, string type)
        {
            return EventGateway
                .FindEventsForUser(this.user)
                .Map((x) => Event.FromType(x))
                .Filter((x) =>
                    x.From <= evt.To &&
                    x.To >= evt.From
                )
                .Filter((x) => x.EventID != evt.EventID)
                .Filter((x) => x.Type != type)
                .ToList();
        }

        public DataTypes.User GetDTO()
        {
            return this.user;
        }

        public User(DataTypes.User user)
        {
            this.user = user;
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

        public Option<int> UserID { get => this.user.UserID; set => this.user.UserID = value; }
        public string FirstName { get => this.user.FirstName; set => this.user.FirstName = value; }
        public string LastName { get => this.user.LastName; set => this.user.LastName = value; }
        public string Login { get => this.user.Login; set => this.user.Login = value; }
        public string Password { get => this.user.Password; set => this.user.Password = value; }
        public UserRole Role { get => this.user.Role; set => this.user.Role = value; }

        private DataTypes.User user;
    }
}