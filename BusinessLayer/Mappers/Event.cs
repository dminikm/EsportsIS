using DatabaseService.Gateway;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessLayer {
    public class Event : DataTypes.Event
    {
        public void Save()
        {
            EventGateway.Update(this);
        }

        public void Delete()
        {
            EventGateway.Delete(this);
        }

        public void RemoveParticipant(User usr)
        {
            this.Participants.Remove(usr);
        }

        public PreviewList<User> Participants { get; set; }

        public Event(DataTypes.Event evt)
        {
            this.Color = evt.Color;
            this.EventID = evt.EventID;
            this.From = evt.From;
            this.Name = evt.Name;
            this.ParticipantIDs = evt.ParticipantIDs;
            this.To = evt.To;
            this.Type = evt.Type;

            Participants = new PreviewList<User>(() => {
                return ParticipantIDs.Map((x) => User.Find(x)).Somes().ToList();
            }, (User usr) => new Command(() => {
                ParticipantIDs.Add(usr.UserID.IfNone(() => throw new ArgumentException("User must have an ID!")));
            }), (User usr) => new Command(() => {
                ParticipantIDs.Remove(usr.UserID.IfNone(() => throw new ArgumentException("User must have an ID!")));
            }), true);
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
}