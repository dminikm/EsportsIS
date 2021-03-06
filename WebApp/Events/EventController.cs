using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer;

class EventController : BaseController
{
    private DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
    {
        int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
        return dt.AddDays(-1 * diff).Date;
    }

    public ControllerAction Overview()
    {
        if (!LoggedIn)
            return Redirect("/login");

        var upcoming = LoggedUser
            .GetUpcomingEvents()
            .Take(5)
            .ToList();

        var upcomingNotJoined = CustomEvent.GetUpcomingNotJoinedBy(DateTime.UtcNow, LoggedUser);

        var from = StartOfWeek(DateTime.Today, DayOfWeek.Monday);
        var to = StartOfWeek(DateTime.Today, DayOfWeek.Monday).AddDays(7).AddMilliseconds(-1);
        var available = LoggedUser.GetEventsAvailableFromTo(from, to);

        return View<OverviewView, EventOverviewModel>(new EventOverviewModel() {
            WeekStart = from,
            Events = available,
            UpcomingEvents = upcoming,
            UpcomingNotJoinedEvents = upcomingNotJoined
        });
    }

    public ControllerAction Detail(int eventID)
    {
        if (!LoggedIn)
            return Redirect("/login");

        return Event
            .Find(eventID)
            .Match(
                (x) =>
                {
                    ViewBag.RequiredConflicts = LoggedUser.GetEventsOverlappingWithNotOfType(x, "custom");
                    ViewBag.OptionalConflicts = LoggedUser.GetEventsOverlappingWithOfType(x, "custom");

                    return View<DetailView, Event>(x);
                },
                () => Redirect("/")
            );
    }

    public ControllerAction Join(int eventID)
    {
        var evt = Event
            .Find(eventID)
            .IfSome(
                (x) =>
                {
                    x.Participants.Add(LoggedUser);
                    x.Save();
                }
            );

        return Redirect($"/event/{eventID}");
    }

    public ControllerAction JoinAndLeaveConflicting(int eventID)
    {
        var evt = Event
            .Find(eventID)
            .IfSome(
                (x) =>
                {
                    // Leave conflicting events
                    var conflicting = LoggedUser.GetEventsOverlappingWithOfType(x, "custom");
                    conflicting.ForEach((y) => { y.Participants.Remove(LoggedUser); y.Save(); });

                    // Join this event
                    x.Participants.Add(LoggedUser);
                    x.Save();
                }
            );

        return Redirect($"/event/{eventID}");
    }
}