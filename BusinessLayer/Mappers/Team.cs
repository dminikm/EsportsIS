using LanguageExt;
using DatabaseService.Gateway;

namespace BusinessLayer {
    public class Team : DataTypes.Team {
        public void Save() {
            this.players.Do();
            TeamGateway.Update(this);
        }

        public void Delete() {
            TeamToPlayerGateway.DeleteAllForTeam(this);
            TeamGateway.Delete(this);
        }

        private PreviewList<User> players;

        public PreviewList<User> Players { get => players; }

        public Team(DataTypes.Team team) {
            this.TeamID = team.TeamID;
            this.Game = team.Game;
            this.Name = team.Name;
            this.CoachID = team.CoachID;

            this.players = new PreviewList<User>(() => {
                var ttps = TeamToPlayerGateway.FindByTeam(this);
                return ttps
                    .Map((ttp) => User.Find(ttp.PlayerID.Value))
                    .Somes()
                    .ToArr()
                    .ToList();
            }, (value) => new Command(() => {
                
                TeamToPlayerGateway.Create(this, value);
            }, Command.Blank), (value) => new Command(() => {
                TeamToPlayerGateway.Delete(this, value);
            }, Command.Blank));
        }

        public static Team Create(string name, string game) {
            return new Team(TeamGateway.Create(name, game, Option<DataTypes.User>.None));
        }

        public static Team Create(string name, string game, Option<User> coach) {
            return new Team(TeamGateway.Create(name, game, coach.Map<DataTypes.User>((x) => x)));
        }

        public static Option<User> Find(int id) {
            var user = UserGateway.Select(id);
            return user.Map((usr) => new User(usr));
        }
    }
}