using System.Collections.Generic;
using LanguageExt;

namespace DataTypes {
    public class Participant {
        public string Type { get; set; }
        public int ParticipantID { get; set; }
    }

    public class Event {
        public Option<int> EventID { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public string Color { get; set; }
        public List<Participant> Participants { get; set; }
    }

    public class TrainingEvent : Event {

    }

    public class MatchEvent : Event {
        public string Server { get; set; }
    }

    public class TournamentEvent : Event {
        public string location { get; set; }
    }

    public class CustomEvent : Event {
        public int MaxParticipants { get; set; }
    }
}