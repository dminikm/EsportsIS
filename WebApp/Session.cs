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
                EndTime = (DateTimeOffset.UtcNow).AddDays(1).ToUnixTimeMilliseconds(),
                Obj = obj,
            });
            return str;
        }
    }

    public static bool HasSession(string sessionID)
    {
        RemoveInvalidSessions();
        return sessions.ContainsKey(sessionID);
    }

    public static ExpandoObject GetSession(string sessionID)
    {
        // Refresh the session expire time
        sessions[sessionID].EndTime = (DateTimeOffset.UtcNow).AddDays(1).ToUnixTimeMilliseconds();
        return sessions[sessionID].Obj;
    }

    public static void RemoveSession(string sessionID)
    {
        sessions.Remove(sessionID);
    }

    private static void RemoveInvalidSessions()
    {
        sessions = new Dictionary<string, Session>(sessions.Filter((x) => x.Value.EndTime > DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()));
    }

    private static Dictionary<string, Session> sessions = new Dictionary<string, Session>();
}