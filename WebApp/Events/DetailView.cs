using BusinessLayer;
using System;
using System.Collections.Generic;

class DetailView : View<Event>
{
    public string RenderCapacityJoin(Event model)
    {
        if (model.Type != "custom")
            return "";

        var evt = (CustomEvent)model;

        var participants = evt.ParticipantIDs.Count;
        var maxParticipants = evt.MaxParticipants <= 0 ? "∞" : $"{evt.MaxParticipants}";

        var currentTime = DateTime.UtcNow;

        var joinable = !model.ParticipantIDs.Contains(((User)ViewBag.User).UserID.IfNone(() => -1)) && currentTime < model.From;

        return $@"
        <span>Capacity</span> <br>
        <div>
            <input type=""text"" value="" {participants} / {maxParticipants} "" readonly>

            {(joinable ? "<button id=\"button-join\">Join</button>" : "<button disabled>Leave</button>")}
        </duv>
        ";
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
        return string.Join("", events.Map((x) => RenderMiniEvent(x)));
    }

    public override string Render(Event model)
    {
        var from = model.From.ToLocalTime().ToShortDateString() + " " + model.From.ToLocalTime().ToLongTimeString();
        var to = model.To.ToLocalTime().ToShortDateString() + " " + model.To.ToLocalTime().ToLongTimeString();

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

            <!-- Required scheduling conflict -->
            <div class=""dialog-backdrop"" id=""required-dialog"">
                <div class=""content-section dialog""open>
                    <div class=""content-section-header"">Scheduling conflict!</div>
                    <div class=""content-section-body"">
                        You have a scheduling conflict with these events: <br><br>

                        { RenderMiniEvents(ViewBag.RequiredConflicts) }

                        <button id=""button-required-ok"">Ok</button>
                    </div>
                </div>
            </div>

            <!-- Optional scheduling conflict -->
            <div class=""dialog-backdrop"" id=""optional-dialog"">
                <div class=""content-section dialog""open>
                    <div class=""content-section-header"">Scheduling conflict!</div>
                    <div class=""content-section-body"">
                        You have a scheduling conflict with these optional events: <br><br>

                        { RenderMiniEvents(ViewBag.OptionalConflicts) }

                        <div>
                            <button id=""optional-join"">Join anyways</button>
                            <button id=""button-optional-cancel"">Cancel</button>
                        </div>
                    </div>
                </div>
            </div>

            <form action=""/event/{model.EventID.IfNone(-1)}/join"" method=""POST"" style=""display: none;"" id=""join-form"">
            </form>

            <script>
                window.addEventListener('load', () => {"{"}
                    let requiredDialog = document.querySelector('#required-dialog');
                    let optionalDialog = document.querySelector('#optional-dialog');

                    let form = document.querySelector('#join-form');

                    let joinButton = document.querySelector('#button-join');
                    let requiredOkButton = document.querySelector('#button-required-ok');
                    let optionalJoinButton = document.querySelector('#optional-join');
                    let optionalCancelButton = document.querySelector('#button-optional-cancel');

                    document.querySelectorAll('.dialog-backdrop').forEach((x) => x.addEventListener('click', () => x.classList.remove('open')));
                    requiredOkButton.addEventListener('click', () => document.querySelectorAll('.dialog-backdrop').forEach((x) => x.click()));
                    optionalCancelButton.addEventListener('click', () => document.querySelectorAll('.dialog-backdrop').forEach((x) => x.click()));

                    if ({( ViewBag.RequiredConflicts.Count > 0 ? "true" : "false" )}) {"{"}
                        joinButton.addEventListener('click', () => requiredDialog.classList.add('open'));
                    {"}"} else if ({( ViewBag.OptionalConflicts.Count > 0 ? "true" : "false" )}) {"{"}
                        joinButton.addEventListener('click', () => optionalDialog.classList.add('open'));
                        optionalJoinButton.addEventListener('click', () => form.submit());
                    {"}"} else {"{"}
                        joinButton.addEventListener('click', () => form.submit());
                    {"}"}
                {"}"});
            </script>
        ");
    }
}