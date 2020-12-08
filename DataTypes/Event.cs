using System.Collections.Generic;
using LanguageExt;

namespace DataTypes
{
    public class Event
    {
        public Option<int> EventID { get; set; }
        public string Name { get; set;}
        public string Type { get; set; }
        public string Description { get; set; }
        public long From { get; set; }
        public long To { get; set; }
        public string Color { get; set; }
        public List<int> ParticipantIDs { get; set; }

        public override bool Equals(object obj)
        {            
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            return this.EventID.Equals(((Event)obj).EventID);
        }
        
        public override int GetHashCode()
        {
            return this.EventID.GetHashCode();
        }
    }

    public class TrainingEvent : Event
    {

    }

    public class MatchEvent : Event
    {
        public string Server { get; set; }
    }

    public class TournamentEvent : Event
    {
        public string Location { get; set; }
    }

    public class CustomEvent : Event
    {
        public int MaxParticipants { get; set; }
    }
}