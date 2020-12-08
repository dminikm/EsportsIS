using DatabaseService.Gateway;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer {
    public class Event
    {
        public void Save()
        {
            EventGateway.Update(evt);
        }

        public void Delete()
        {
            EventGateway.Delete(evt);
        }

        public void RemoveParticipant(User usr)
        {
            this.Participants.Remove(usr);
        }

        public PreviewList<User> Participants { get; set; }

        protected Event(DataTypes.Event evt)
        {
            this.evt = evt;

            Participants = new PreviewList<User>(() => {
                return ParticipantIDs.Map((x) => User.Find(x)).Somes().ToList();
            }, (User usr) => new Command(() => {
                ParticipantIDs.Add(usr.UserID.IfNone(() => throw new ArgumentException("User must have an ID!")));
            }), (User usr) => new Command(() => {
                ParticipantIDs.Remove(usr.UserID.IfNone(() => throw new ArgumentException("User must have an ID!")));
            }), true);
        }

        public static Event FromType(DataTypes.Event evt)
        {
            if (evt.Type == "training")
            {
                return new TrainingEvent((DataTypes.TrainingEvent)evt);
            }
            else if (evt.Type == "match")
            {
                return new MatchEvent((DataTypes.MatchEvent)evt);
            }
            else if (evt.Type == "tournament")
            {
                return new TournamentEvent((DataTypes.TournamentEvent)evt);
            }
            else
            {
                return new CustomEvent((DataTypes.CustomEvent)evt);
            }
        }

        protected DataTypes.Event evt;

        public string Type { get => evt.Type; set => evt.Type = value; }
        public Option<int> EventID { get => evt.EventID; set => evt.EventID = value; }
        public string Name { get => evt.Name; set => evt.Name = value; }
        public string Description { get => evt.Description; set => evt.Description = value; }
        public long From { get => evt.From; set => evt.From = value; }
        public long To { get => evt.To; set => evt.To = value; }
        public List<int> ParticipantIDs { get => evt.ParticipantIDs; set => evt.ParticipantIDs = value; }
        public string Color { get => evt.Color; set => evt.Color = value; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            
            return evt.Equals(((Event)obj).evt);
        }
        
        public override int GetHashCode()
        {
            return evt.GetHashCode();
        }
    }

    public class TrainingEvent : Event
    {
        public TrainingEvent(DataTypes.TrainingEvent evt) : base(evt)
        {
        }

        public static TrainingEvent Create(string name, string description, DateTime from, DateTime to, List<User> participants)
        {
            return new TrainingEvent(EventGateway.CreateTrainingEvent(
                name,
                description,
                ((DateTimeOffset)from).ToUnixTimeMilliseconds(),
                ((DateTimeOffset)to).ToUnixTimeMilliseconds(),
                participants.Map((x) => x.UserID.IfNone(() => -1)).ToList()
            ));
        }
    }

    public class MatchEvent : Event
    {
        public MatchEvent(DataTypes.MatchEvent evt) : base(evt)
        {

        }

        public static MatchEvent Create(string name, string description, string server, DateTime from, DateTime to, List<User> participants)
        {
            return new MatchEvent(EventGateway.CreateMatchEvent(
                name,
                description,
                ((DateTimeOffset)from).ToUnixTimeMilliseconds(),
                ((DateTimeOffset)to).ToUnixTimeMilliseconds(),
                server, 
                participants.Map((x) => x.UserID.IfNone(() => -1)).ToList()
            ));
        }

        public string Server { get => ((DataTypes.MatchEvent)evt).Server; set => ((DataTypes.MatchEvent)evt).Server = value; }
    }

    public class TournamentEvent : Event
    {
        public TournamentEvent(DataTypes.TournamentEvent evt) : base(evt)
        {

        }

        public static TournamentEvent Create(string name, string description, string location, DateTime from, DateTime to, List<User> participants)
        {
            return new TournamentEvent(EventGateway.CreateTournamentEvent(
                name,
                description,
                ((DateTimeOffset)from).ToUnixTimeMilliseconds(),
                ((DateTimeOffset)to).ToUnixTimeMilliseconds(),
                location, 
                participants.Map((x) => x.UserID.IfNone(() => -1)).ToList()
            ));
        }

        public string Location { get => ((DataTypes.TournamentEvent)evt).Location; set => ((DataTypes.TournamentEvent)evt).Location = value; }
    }

    public class CustomEvent : Event
    {
        public CustomEvent(DataTypes.CustomEvent evt) : base(evt)
        {

        }

        public static CustomEvent Create(string name, string description, int maxParticipants, string color, DateTime from, DateTime to, List<User> participants)
        {
            return new CustomEvent(EventGateway.CreateCustomEvent(
                name,
                description,
                ((DateTimeOffset)from).ToUnixTimeMilliseconds(),
                ((DateTimeOffset)to).ToUnixTimeMilliseconds(),
                maxParticipants,
                color,
                participants.Map((x) => x.UserID.IfNone(() => -1)).ToList()
            ));
        }

        public int MapParticipants { get => ((DataTypes.CustomEvent)evt).MaxParticipants; set => ((DataTypes.CustomEvent)evt).MaxParticipants = value; }
    }
}