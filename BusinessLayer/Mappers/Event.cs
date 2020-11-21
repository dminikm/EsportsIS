using DatabaseService.Gateway;
using System;

namespace BusinessLayer {
    public class Event : DataTypes.Event {
        public void Save() {
            EventGateway.Update(this);
        }

        public void Delete() {
            EventGateway.Delete(this);
        }

        public PreviewList<User> Participants { get; set; }

        private Event(DataTypes.Event evt) {
            Participants = new PreviewList<User>(() => {
                return ParticipantIDs.Map((x) => User.Find(x)).Somes().ToArr().ToList();
            }, (User usr) => new Command(() => {
                ParticipantIDs.Add(usr.UserID.IfNone(() => throw new ArgumentException("User must have an ID!")));
            }), (User usr) => new Command(() => {
                ParticipantIDs.Remove(usr.UserID.IfNone(() => throw new ArgumentException("User must have an ID!")));
            }), true);
        }
    }
}