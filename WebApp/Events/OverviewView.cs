using System;
using System.Collections.Generic;
using BusinessLayer;
using System.Linq;
using static System.Linq.Enumerable;

class OverviewView : View<EventOverviewModel>
{
    private string RenderMiniEvent(Event evt)
    {
        var time = new DateTime(1970, 1, 1, 0, 0, 0, 0)
            .AddMilliseconds(evt.From)
            .ToShortTimeString();

        return $@"
<div  class=""overview-event-mini"">
    <div style=""background-color: {evt.Color}"" class=""overview-event-mini-inner"">
        <div>
            {evt.Name}
        </div>
        <div class=""overview-event-mini-icon"">i</div>
    </div>
    <div class=""overview-event-mini-time"">{time}</div>
</div>
        ";
    }

    private string RenderMiniEvents(List<Event> events)
    {
        return string.Join("", events.Map((x) => RenderMiniEvent(x)));
    }

    private string RenderDaySchedule(List<Event> events, int day)
    {
        return $@"
        <div class=""overview-table-outer"">
            <div class=""overview-table-padder""></div>
            <div class=""overview-table-inner""></div>
            <div class=""overview-table-padder""></div>
        </div>
        ";
    }

    private string RenderTimeColumn()
    {
        return String.Join("",
            Range(0, 23).Map((x) => $@"
                { (x != 0 ? "<div class=\"overview-table-time-padder\"></div>" : "") }
                <div class=""overview-table-time-container"">{ x.ToString().PadLeft(2, '0') }:00</div>
            ")
        );
    }

    private string RenderScheduleTable(List<Event> events)
    {
        return $@"
<div class=""overview-table-container"">
    <table class=""overview-table"">
        <thead>
            <tr>
                <th>Time</th>
                <th>Monday</th>
                <th>Tuesday</th>
                <th>Wednesday</th>
                <th>Thursday</th>
                <th>Friday</th>
                <th>Saturday</th>
                <th>Sunday</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <th>{ RenderTimeColumn() }</th>
                <th>{ RenderDaySchedule(events, 0) }</th>
                <th>{ RenderDaySchedule(events, 0) }</th>
                <th>{ RenderDaySchedule(events, 0) }</th>
                <th>{ RenderDaySchedule(events, 0) }</th>
                <th>{ RenderDaySchedule(events, 0) }</th>
                <th>{ RenderDaySchedule(events, 0) }</th>
                <th>{ RenderDaySchedule(events, 0) }</th>
            </tr>
        </tbody>
    </table>
</div>
        ";
    }

    public override string Render(EventOverviewModel model)
    {
        return new Layout(ViewBag).Render($@"
<div class=""overview-container"">
    <div class=""content-section"">
        <div class=""content-section-header"">
        </div>
        <div class=""content-section-body"">
            { RenderScheduleTable(model.Events) }
        </div>
    </div>
    <div class=""overview-upcoming"">
        <div class=""content-section"">
            <div class=""content-section-header"">
                Upcoming Events
            </div>
            <div class=""content-section-body"">
                { RenderMiniEvents(model.UpcomingEvents) }
            </div>
        </div>

        <div class=""content-section"">
            <div class=""content-section-header"">
                Attend an Event
            </div>
            <div class=""content-section-body"">
                { RenderMiniEvents(model.UpcomingNotJoinedEvents.Map((x) => (Event)x).ToList()) }
            </div>
        </div>
    </div>
</div>
        ");
    }
}