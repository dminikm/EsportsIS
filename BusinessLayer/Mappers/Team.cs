using LanguageExt;
using DatabaseService.Gateway;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer
{
    public class Team
    {
        public void Save()
        {
            this.players.Do();
            TeamGateway.Update(this.team);
        }

        public void Delete()
        {
            TeamToPlayerGateway.DeleteAllForTeam(this.team);
            TeamGateway.Delete(this.team);
        }

        public Team(DataTypes.Team team)
        {
            this.team = team;

            this.players = new PreviewList<User>(() =>
            {
                var ttps = TeamToPlayerGateway.FindByTeam(this.team);
                return ttps
                    .Map((ttp) => User.Find(ttp.PlayerID.Value))
                    .Somes()
                    .ToList();
            }, (value) => new Command(() =>
            {
                TeamToPlayerGateway.Create(this.team, value.GetDTO());
            }, Command.Blank), (value) => new Command(() =>
            {
                TeamToPlayerGateway.Delete(this.team, value.GetDTO());
            }, Command.Blank));
        }

        public static Team Create(string name, string game)
        {
            return new Team(TeamGateway.Create(name, game, Option<DataTypes.User>.None));
        }

        public static Team Create(string name, string game, Option<User> coach)
        {
            return new Team(TeamGateway.Create(name, game, coach.Map<DataTypes.User>((x) => x.GetDTO())));
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
            var team = TeamGateway.FindByCoach(user.GetDTO());
            return team.Map((tm) => new Team(tm));
        }

        public Option<int> TeamID { get => this.team.TeamID; set => this.team.TeamID = value; }
        public Option<int> CoachID { get => this.team.CoachID; set => this.team.CoachID = value; }
        public string Name { get => this.team.Name; set => this.team.Name = value; }
        public string Game { get => this.team.Game; set => this.team.Game = value; }
        private PreviewList<User> players;
        public PreviewList<User> Players { get => players; }

        private DataTypes.Team team;
    }
}