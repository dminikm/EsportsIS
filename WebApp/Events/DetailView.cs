using BusinessLayer;
using System;

class DetailView : View<Event>
{
    public string RenderCapacityJoin(Event model)
    {
        if (model.Type != "custom")
            return "";

        var evt = (CustomEvent)model;

        var participants = evt.ParticipantIDs.Count;
        var maxParticipants = evt.MaxParticipants <= 0 ? "∞" : $"{evt.MaxParticipants}";

        return $@"
        <span>Capacity</span> <br>
        <form action=""/event/{model.EventID.IfNone(-1)}/join"" method=""POST"">
            <input type=""text"" value="" {participants} / {maxParticipants} "" readonly>
            <input type=""submit"" value=""Join"">
        </form>
        ";
    }

    public override string Render(Event model)
    {
        var from = new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(model.From).ToLocalTime().ToLongDateString();
        var to = new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(model.To).ToLocalTime().ToLongDateString();

        return new Layout(ViewBag).Render($@"
            <div class=""content-section"">
                <div class=""content-section-header"">
                    {model.Name}
                </div>
                <div class=""content-section-body"">
                    <div style=""padding: 15px;"">
                        <span>Time of event</span> <br>
                        <div>
                            <input type=""text"" value=""{from}"" readonly> - 
                            <input type=""text"" value=""{to}"" readonly>
                        </div>

                        <br><br><br><br>

                        <span>Description</span> <br>
                        <textarea cols=""80"" rows=""24"">{model.Description}</textarea>

                        <br><br><br><br>

                        { RenderCapacityJoin(model) }
                    </div>
                </div>
            </div>
        ");
    }
}