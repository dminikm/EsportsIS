using System;
using System.Collections.Generic;
using DataTypes;
using LanguageExt;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Linq.Expressions;

namespace DatabaseService
{
    namespace Gateway
    {
        public class EventGateway
        {
            public static TrainingEvent CreateTrainingEvent(string name, string description, int from, int to, List<int> participants)
            {
                var db = JSONDatabase.Instance;

                db.BeginTransaction();
                var newId = db.CreateInTable("events", new Dictionary<string, object>() {
                    { "name", name },
                    { "type", "training" },
                    { "description", description },
                    { "from", from },
                    { "to", to },
                    { "color", "#00FF00" },
                    { "participants", participants }
                });
                db.EndTransaction();

                return new TrainingEvent()
                {
                    EventID = newId,
                    Name = name,
                    Type = "training",
                    Description = description,
                    From = from,
                    To = to,
                    Color = "#00FF00",
                    ParticipantIDs = participants
                };
            }

            public static MatchEvent CreateMatchEvent(string name, string description, int from, int to, string server, List<int> participants)
            {
                var db = JSONDatabase.Instance;

                db.BeginTransaction();
                var newId = db.CreateInTable("events", new Dictionary<string, object>() {
                    { "type", "match" },
                    { "name", "Name" },
                    { "server", server },
                    { "description", description },
                    { "from", from },
                    { "to", to },
                    { "color", "#00FF00" },
                    { "participants", participants }
                });
                db.EndTransaction();

                return new MatchEvent()
                {
                    EventID = newId,
                    Name = name,
                    Type = "match",
                    Description = description,
                    From = from,
                    To = to,
                    Color = "#FF0000",
                    ParticipantIDs = participants,
                    Server = server
                };
            }

            public static TournamentEvent CreateTournamentEvent(string name, string description, int from, int to, string location, List<int> participants)
            {
                var db = JSONDatabase.Instance;

                db.BeginTransaction();
                var newId = db.CreateInTable("events", new Dictionary<string, object>() {
                    { "type", "tournament" },
                    { "name", name },
                    { "location", location },
                    { "description", description },
                    { "from", from },
                    { "to", to },
                    { "color", "#00FF00" },
                    { "participants", participants }
                });
                db.EndTransaction();

                return new TournamentEvent()
                {
                    EventID = newId,
                    Name = name,
                    Type = "tournament",
                    Description = description,
                    From = from,
                    To = to,
                    Color = "#FF0000",
                    ParticipantIDs = participants,
                    Location = location
                };
            }

            public static CustomEvent CreateCustomEvent(string name, string description, int from, int to, int maxParticipants, string color, List<int> participants)
            {
                var db = JSONDatabase.Instance;

                db.BeginTransaction();
                var newId = db.CreateInTable("events", new Dictionary<string, object>() {
                    { "type", "custom" },
                    { "name", name },
                    { "description", description },
                    { "from", from },
                    { "to", to },
                    { "color", color },
                    { "participants", participants },
                    { "maxParticipants", maxParticipants },
                });
                db.EndTransaction();

                return new CustomEvent()
                {
                    EventID = newId,
                    Name = name,
                    Type = "custom",
                    Description = description,
                    From = from,
                    To = to,
                    Color = color,
                    ParticipantIDs = participants,
                    MaxParticipants = maxParticipants
                };
            }

            // TODO: Add updates
            public static void Update(Event evt)
            {
                var db = JSONDatabase.Instance;

                var id = evt.EventID.IfNone(() => throw new ArgumentException("Event must have an ID!"));
                var dict = new Dictionary<string, object>() {
                    { "id", id },
                    { "name", evt.Name },
                    { "type", evt.Type },
                    { "description", evt.Description },
                    { "from", evt.From },
                    { "to", evt.To },
                    { "color", evt.Color },
                    { "participants", evt.ParticipantIDs },
                };

                if (evt.Type == "match")
                {
                    dict.Add("server", (evt as MatchEvent).Server);
                }
                else if (evt.Type == "tournament")
                {
                    dict.Add("server", (evt as TournamentEvent).Location);
                }
                else if (evt.Type == "custom")
                {
                    dict.Add("maxParticipants", (evt as CustomEvent).MaxParticipants);
                }

                db.BeginTransaction();
                db.SetInTable("events", id, dict);
                db.EndTransaction();
            }

            public static Option<Event> Find(int id)
            {
                var db = JSONDatabase.Instance;

                db.BeginTransaction();
                var rowOption = db.GetInTable("events", id);
                db.EndTransaction();
                
                return rowOption.Match((row) => ParseEvent(row), () => Option<Event>.None);
            }

            public static List<Event> All()
            {
                var db = JSONDatabase.Instance;

                db.BeginTransaction();
                var rowOption = db.GetAllInTable("events");
                db.EndTransaction();
                
                return rowOption.Map((x) => ParseEvent(x)).Somes().ToList();
            }

            public static List<Event> FindEventsForUser(User usr)
            {
                return All().Filter((x) => x.ParticipantIDs.Contains(usr.UserID.IfNone(() => -1))).ToList();
            }

            private static Option<Event> ParseEvent(Dictionary<string, object> row)
            {
                if (!row.ContainsKey("id"))
                {
                    return Option<Event>.None;
                }

                var id = Helpers.ConvertType<int>(row["id"]);

                if (!row.ContainsKey("name"))
                {
                    return Option<Event>.None;
                }

                var name = Helpers.ConvertType<string>(row["name"]);

                if (!row.ContainsKey("type"))
                {
                    return Option<Event>.None;
                }

                var type = Helpers.ConvertType<string>(row["type"]);

                if (!row.ContainsKey("description"))
                {
                    return Option<Event>.None;
                }

                var description = Helpers.ConvertType<string>(row["description"]);

                if (!row.ContainsKey("from"))
                {
                    return Option<Event>.None;
                }

                var from = Helpers.ConvertType<int>(row["from"]);

                if (!row.ContainsKey("to"))
                {
                    return Option<Event>.None;
                }

                var to = Helpers.ConvertType<int>(row["to"]);

                if (!row.ContainsKey("color"))
                {
                    return Option<Event>.None;
                }

                var color = Helpers.ConvertType<string>(row["color"]);

                if (!row.ContainsKey("participants"))
                {
                    return Option<Event>.None;
                }

                var participants = (row["participants"] as JArray).Map((x) => x.ToObject<int>()).ToList();

                if (type == "training")
                {
                    return new TrainingEvent()
                    {
                        EventID = id,
                        Name = name,
                        Type = type,
                        Description = description,
                        From = from,
                        To = to,
                        Color = color,
                        ParticipantIDs = participants,
                    };
                }
                else if (type == "match")
                {
                    if (!row.ContainsKey("server"))
                    {
                        return Option<Event>.None;
                    }

                    var server = Helpers.ConvertType<string>(row["server"]);

                    return new MatchEvent()
                    {
                        EventID = id,
                        Name = name,
                        Type = type,
                        Description = description,
                        From = from,
                        To = to,
                        Color = color,
                        ParticipantIDs = participants,
                        Server = server,
                    };
                }
                else if (type == "tournament")
                {
                    if (!row.ContainsKey("location"))
                    {
                        return Option<Event>.None;
                    }

                    var location = Helpers.ConvertType<string>(row["location"]);

                    return new TournamentEvent()
                    {
                        EventID = id,
                        Name = name,
                        Type = type,
                        Description = description,
                        From = from,
                        To = to,
                        Color = color,
                        ParticipantIDs = participants,
                        Location = location,
                    };
                }
                else if (type == "custom")
                {
                    return new CustomEvent()
                    {
                        EventID = id,
                        Name = name,
                        Type = type,
                        Description = description,
                        From = from,
                        To = to,
                        Color = color,
                        ParticipantIDs = participants,
                    };
                }
                else
                {
                    return Option<Event>.None;
                }
            }

            public static void Delete(Event evt) {
                var db = JSONDatabase.Instance;

                db.BeginTransaction();
                db.RemoveFromTable("events", evt.EventID.IfNone(() => throw new ArgumentException("Event needs to have an ID to be removed!")));
                db.EndTransaction();
            }
        }
    }
}