using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

class Session
{
    public string ID { get; set; }
    public long EndTime { get; set; }
    public ExpandoObject Obj { get; set; }
}

class SessionManager
{
    public static string AddSession(ExpandoObject obj)
    {
        while (true)
        {
            var guid = Guid.NewGuid();
            var str = guid.ToString();

            if (sessions.ContainsKey(str))
            {
                continue;
            }

            sessions.Add(str, new Session() {
                ID = str,
                EndTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeMilliseconds(),
                Obj = obj,
            });
            return str;
        }
    }

    public bool HasSession(string sessionID)
    {
        RemoveInvalidSessions();
        return sessions.ContainsKey(sessionID);
    }

    public static ExpandoObject GetSession(string sessionID)
    {
        return sessions[sessionID].Obj;
    }

    public static void RemoveSession(string sessionID)
    {
        sessions.Remove(sessionID);
    }

    private void RemoveInvalidSessions()
    {
        sessions = new Dictionary<string, Session>(sessions.Filter((x) => true));
    }

    private static Dictionary<string, Session> sessions = new Dictionary<string, Session>();
}