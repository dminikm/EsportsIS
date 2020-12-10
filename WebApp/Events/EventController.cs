using System;
using System.Collections.Generic;
using System.Linq;
using BusinessLayer;

class EventController : BaseController
{
    public ControllerAction Overview()
    {
        if (!LoggedIn)
            return Redirect("/login");

        var upcoming = LoggedUser
            .GetUpcomingEvents()
            .Take(5)
            .ToList();

        var upcomingNotJoined = CustomEvent
            .GetUpcoming(DateTime.Now)
            .Filter((x) => !x.ParticipantIDs.Contains(LoggedUser.UserID.IfNone(-1)))
            .ToList();

        var time = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek);
        var available = LoggedUser.GetEventsAvailableFrom(time);

        return View<OverviewView, EventOverviewModel>(new EventOverviewModel() {
            Events = available,
            UpcomingEvents = upcoming,
            UpcomingNotJoinedEvents = upcomingNotJoined
        });
    }
}