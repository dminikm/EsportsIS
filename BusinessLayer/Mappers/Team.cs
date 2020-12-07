using LanguageExt;
using DatabaseService.Gateway;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer
{
    public class Team : DataTypes.Team
    {
        public void Save()
        {
            this.players.Do();
            TeamGateway.Update(this);
        }

        public void Delete()
        {
            TeamToPlayerGateway.DeleteAllForTeam(this);
            TeamGateway.Delete(this);
        }

        private PreviewList<User> players;

        public PreviewList<User> Players { get => players; }

        public Team(DataTypes.Team team)
        {
            this.TeamID = team.TeamID;
            this.Game = team.Game;
            this.Name = team.Name;
            this.CoachID = team.CoachID;

            this.players = new PreviewList<User>(() =>
            {
                var ttps = TeamToPlayerGateway.FindByTeam(this);
                return ttps
                    .Map((ttp) => User.Find(ttp.PlayerID.Value))
                    .Somes()
                    .ToList();
            }, (value) => new Command(() =>
            {
                TeamToPlayerGateway.Create(this, value);
            }, Command.Blank), (value) => new Command(() =>
            {
                TeamToPlayerGateway.Delete(this, value);
            }, Command.Blank));
        }

        public static Team Create(string name, string game)
        {
            return new Team(TeamGateway.Create(name, game, Option<DataTypes.User>.None));
        }

        public static Team Create(string name, string game, Option<User> coach)
        {
            return new Team(TeamGateway.Create(name, game, coach.Map<DataTypes.User>((x) => x)));
        }

        public static Option<Team> Find(int id)
        {
            var team = TeamGateway.Find(id);
            return team.Map((tm) => new Team(tm));
        }

        public static List<Team> All()
        {
            var teams = TeamGateway.FindAll();
            return teams.Map((tm) => new Team(tm)).ToList();
        }

        public static Option<Team> FindByCoach(User user)
        {
            var team = TeamGateway.FindByCoach(user);
            return team.Map((tm) => new Team(tm));
        }
    }
}