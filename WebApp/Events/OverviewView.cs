using System;
using System.Collections.Generic;
using BusinessLayer;
using System.Linq;
using static System.Linq.Enumerable;

class OverviewView : View<EventOverviewModel>
{
    private DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
    {
        int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
        return dt.AddDays(-1 * diff).Date;
    }

    private string RenderMiniEvent(Event evt)
    {
        var time = evt.From
            .ToLocalTime()
            .ToShortTimeString();

        return $@"
<div  class=""overview-event-mini"">
    <div style=""background-color: {evt.Color}"" class=""overview-event-mini-inner"">
        <div>
            {evt.Name}
        </div>
        <a href=""/event/{evt.EventID.IfNone(-1)}"" class=""overview-event-mini-icon"">i</a>
    </div>
    <div class=""overview-event-mini-time"">{time}</div>
</div>
        ";
    }

    private string RenderMiniEvents(List<Event> events)
    {
        return string.Join("", events.OrderBy((e) => e.From).Map((x) => RenderMiniEvent(x)));
    }

    private string RenderEvent(Event evt, DateTime weekStart, int day)
    {
        var dayStart = (double)((DateTimeOffset)weekStart.AddDays(day).ToLocalTime()).ToUnixTimeMilliseconds();
        var dayEnd = (double)((DateTimeOffset)weekStart.AddDays(day + 1).AddMilliseconds(-1).ToLocalTime()).ToUnixTimeMilliseconds();

        var fromMS = ((DateTimeOffset)evt.From.ToLocalTime()).ToUnixTimeMilliseconds();
        var toMS = ((DateTimeOffset)evt.To.ToLocalTime()).ToUnixTimeMilliseconds();

        var startPerc = Math.Min(1, Math.Max(0, ((double)fromMS - dayStart ) / (dayEnd - dayStart))) * 100;
        var endPerc = 100 - (Math.Min(1, Math.Max(0, ((double)toMS - dayStart ) / (dayEnd - dayStart))) * 100);

        var startStr = startPerc.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture);
        var endStr = endPerc.ToString("0.0000", System.Globalization.CultureInfo.InvariantCulture);

        return $@"
            <div style=""width: 100%; top: {startStr}%; bottom: {endStr}%; background-color: {evt.Color};"" class=""overview-table-event-outer"">
                <div class=""overview-table-event-inner"">
                    <div class=""overview-table-event-name"">{evt.Name}</div>
                    <a href=""/event/{evt.EventID.IfNone(-1)}"" class=""overview-table-event-info"">i</a>
                </div>
            </div>
        ";
    }

    private bool IsInDay(DateTime week, int day, DateTime from, DateTime to)
    {
        return
            from <= ((DateTimeOffset)week.AddDays(day + 1)).ToUniversalTime() &&
            to >= ((DateTimeOffset)week.AddDays(day).AddMilliseconds(-1)).ToUniversalTime();
    }

    private string RenderDaySchedule(List<Event> events, DateTime weekStart, int day)
    {
        var todayEvents = events
            .Filter((x) => IsInDay(weekStart, day, x.From.ToLocalTime(), x.To.ToLocalTime()))
            .ToList();

        var eventColumns = new List<List<Event>>();
        eventColumns.Add(new List<Event>());

        todayEvents.ForEach((x) =>
        {
            foreach (var column in eventColumns)
            {
                if (column.Count == 0) {
                    column.Add(x);

                    return;
                }

                bool canPlaceHere = true;
                foreach (var evt in column)
                {
                    if (evt.From <= x.To && evt.To >= x.From)
                    {
                        canPlaceHere = false;
                        break;
                    }
                }

                if (!canPlaceHere) {
                    continue;
                }

                column.Add(x);
                return;
            }

            eventColumns.Add(new List<Event>() { x });
        });

        return $@"
<td style=""min-width: {200 * eventColumns.Count}px; max-width: {200 * eventColumns.Count}px; width: {200 * eventColumns.Count}px;"">
    <div class=""overview-table-outer"">
        <div class=""overview-table-padder""></div>
        <div class=""overview-table-inner"">
            {
                String.Join(
                    "",
                    eventColumns.Map(
                        (x) => 
                            "<div style=\"flex-grow: 1;\">" +
                            String.Join(
                                "",
                                x.Map((y) =>
                                    RenderEvent(y, weekStart, day)
                                )
                            ) +
                            "</div>"
                    )
                )
            }
        </div>
        <div class=""overview-table-padder""></div>
    </div>
</td>
        ";
    }

    private string RenderTimeColumn()
    {
        return String.Join("",
            Range(0, 25).Map((x) => $@"
                { (x != 0 ? "<div class=\"overview-table-time-padder\"></div>" : "") }
                <div class=""overview-table-time-container"">{ x.ToString().PadLeft(2, '0') }:00</div>
            ")
        );
    }

    private string RenderScheduleTable(List<Event> events, DateTime weekStart)
    {
        return $@"
<div class=""overview-table-container"">
    <table class=""overview-table"">
        <thead>
            <tr>
                <th style=""position: sticky; left: 0;"">Time</th>
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
                <th style=""position: sticky; left: 0; width: 200px;"">{ RenderTimeColumn() }</th>
                { RenderDaySchedule(events, weekStart, 0) }
                { RenderDaySchedule(events, weekStart, 1) }
                { RenderDaySchedule(events, weekStart, 2) }
                { RenderDaySchedule(events, weekStart, 3) }
                { RenderDaySchedule(events, weekStart, 4) }
                { RenderDaySchedule(events, weekStart, 5) }
                { RenderDaySchedule(events, weekStart, 6) }
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
            { RenderScheduleTable(model.Events, model.WeekStart) }
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