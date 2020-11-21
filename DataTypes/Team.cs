using LanguageExt;

namespace DataTypes
{
    public class Team
    {
        public Option<int> TeamID { get; set; }
        public Option<int> CoachID { get; set; }
        public string Name { get; set; }
        public string Game { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Team team &&
                   TeamID == team.TeamID &&
                   TeamID != null;
        }
    }
}