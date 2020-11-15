using System;
using System.Collections.Generic;
using DataTypes;

namespace DatabaseService {
    public class EventGateway {
        public static TrainingEvent CreateTrainingEvent(string description, int from, int to, List<Participant> participants) {
            var db = JSONDatabase.Instance;

            var participantsT = participants.Map(
                (x) => new Dictionary<string, object>() {
                    { "type", x.Type },
                    { "participantID", x.ParticipantID }
                }
            );

            db.BeginTransaction();
            var newId = db.CreateInTable("events", new Dictionary<string, object>() {
                { "type", "training" },
                { "description", description },
                { "from", from },
                { "to", to },
                { "color", "#00FF00" },
                { "participants", participantsT }
            });
            db.EndTransaction();

            return new TrainingEvent() {
                EventID = newId,
                Type = "training",
                Description = description,
                From = from,
                To = to,
                Color = "#00FF00",
                Participants = participants
            };
        }
    }
}