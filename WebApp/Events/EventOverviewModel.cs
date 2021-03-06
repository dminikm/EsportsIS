using System;
using System.Collections.Generic;
using BusinessLayer;

class EventOverviewModel
{
    public DateTime WeekStart { get; set; }
    public List<Event> Events { get; set; }
    public List<Event> UpcomingEvents { get; set; }
    public List<CustomEvent> UpcomingNotJoinedEvents { get; set; }
}