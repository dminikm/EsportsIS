﻿using BusinessLayer;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DesktopApp
{
    public partial class AddEditTournamentEventForm : Form
    {
        public AddEditTournamentEventForm(Option<TournamentEvent> evt, User coach)
        {
            InitializeComponent();
            this.Evt = evt;

            this.Text = this.nameLabel.Text = (evt.IsNone) ?
                "🏆 Add new tournament event!" :
                "🏆 Edit tournament event!";

            this.cmds = new CommandQueue();
            this.cmds.Add(new Command(() =>
            {
                this.Evt.IfNone(() => this.Evt = TournamentEvent.Create(
                    this.nameTextBox.Text,
                    this.descriptionTextBox.Text,
                    this.locationTextBox.Text,
                    this.fromDateTimePicker.Value.ToUniversalTime(),
                    this.toDateTimePicker.Value.ToUniversalTime(),
                    this.participants
                ));
            }));

            this.name = "";
            this.description = "";
            this.location = "";
            this.from = DateTime.UtcNow;
            this.to = DateTime.UtcNow;
            this.participants = new List<User>();
            this.participants.Add(coach);

            evt.IfSome((x) =>
            {
                this.name = x.Name;
                this.description = x.Description;
                this.location = x.Location;
                this.from = x.From;
                this.to = x.To;
                this.participants = x.Participants.ToList();
            });
        }

        private void PopulateFields()
        {
            nameTextBox.Text = name;
            descriptionTextBox.Text = description;
            locationTextBox.Text = location;
            fromDateTimePicker.Value = from.ToLocalTime();
            toDateTimePicker.Value = to.ToLocalTime();
        }

        private void PopulateList()
        {
            participantListView.Items.Clear();

            foreach (var participant in participants)
            {
                var item = new ListViewItem(new string[] {
                    $"{participant.FirstName} {participant.LastName} ({participant.Login})"
                });

                item.Tag = participant;

                participantListView.Items.Add(item);
            }
        }

        private void SetupButtons()
        {
            this.addParticipantButton.Enabled = true;
            this.removeParticipantButton.Enabled = false;

            if (participantListView.SelectedItems.Count > 0)
            {
                this.removeParticipantButton.Enabled = true;

                if (((User)participantListView.SelectedItems[0].Tag).Role == DataTypes.UserRole.Coach)
                {
                    this.removeParticipantButton.Enabled = false;
                }
            }
        }

        private void AddEditTournamentEvent_Load(object sender, EventArgs e)
        {
            PopulateFields();
            PopulateList();
            SetupButtons();
        }

        public Option<TournamentEvent> Evt { get; set; }
        private CommandQueue cmds;

        private List<User> participants;
        private string name;
        private string description;
        private string location;
        private DateTime from;
        private DateTime to;

        enum VerificationState
        {
            Ok,
            InvalidName,
            InvalidFromDate,
            InvalidToDate,
            ConflictWithRequired,
            ConflictWithOptional,
        }

        struct VerificationResult
        {
            public VerificationState State { get; set; }
            public object Value { get; set; }
        }

        private VerificationResult DoVerification()
        {
            if (nameTextBox.Text.Length <= 1 || nameTextBox.Text.Length >= 40)
                return new VerificationResult()
                {
                    State = VerificationState.InvalidName,
                    Value = nameTextBox.Text,
                };

            if (fromDateTimePicker.Value < DateTime.UtcNow)
                return new VerificationResult()
                {
                    State = VerificationState.InvalidFromDate,
                    Value = fromDateTimePicker.Value,
                };

            if (toDateTimePicker.Value < fromDateTimePicker.Value)
                return new VerificationResult()
                {
                    State = VerificationState.InvalidToDate,
                    Value = toDateTimePicker.Value,
                };

            var requiredConflicts = participants
                .Map((x) =>
                {
                    var evts = x.GetEventsOverlappingWithNotOfType(
                        fromDateTimePicker.Value,
                        toDateTimePicker.Value,
                        Evt.Map((x) => x.EventID.IfNone(-1)),
                        "custom"
                    );
                    
                    return new KeyValuePair<User, List<Event>>(
                        x, evts
                    );
                }).Filter((x) => x.Value.Count > 0).ToList();

            if (requiredConflicts.Count > 0)
                return new VerificationResult()
                {
                    State = VerificationState.ConflictWithRequired,
                    Value = requiredConflicts,
                };

            var optionalConflicts = participants.
                Map((x) =>
                {
                    var evts = x.GetEventsOverlappingWithOfType(
                        fromDateTimePicker.Value,
                        toDateTimePicker.Value,
                        Evt.Map((x) => x.EventID.IfNone(-1)),
                        "custom"
                    );

                    return new KeyValuePair<User, List<Event>>(
                        x, evts
                    );
                }).Filter((x) => x.Value.Count > 0).ToList();

            if (optionalConflicts.Count > 0)
                return new VerificationResult()
                {
                    State = VerificationState.ConflictWithOptional,
                    Value = optionalConflicts,
                };

            return new VerificationResult()
            {
                State = VerificationState.Ok,
                Value = null,
            };
        }

        private void participantListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetupButtons();
        }

        private void addParticipantButton_Click(object sender, EventArgs e)
        {
            var form = new SelectUserForm(DataTypes.UserRole.Player, participants);
            var result = form.ShowDialog();

            if (result == DialogResult.OK)
            {
                var newPlayer = form.SelectedUser;

                this.participants.Add(newPlayer);

                if (this.Evt.IsSome)
                {
                    this.cmds.Add(new Command(() =>
                    {
                        this.Evt.IfSome((evt) => evt.Participants.Add(newPlayer));
                    }));
                }

                PopulateList();
                SetupButtons();
            }
        }

        private void removeParticipantButton_Click(object sender, EventArgs e)
        {
            var participant = (User)this.participantListView.SelectedItems[0].Tag;
            this.participants.Remove(participant);

            if (this.Evt.IsSome)
            {
                this.cmds.Add(new Command(() => 
                {
                    this.Evt.IfSome((evt) => evt.Participants.Remove(participant));
                }));
            }

            PopulateList();
            SetupButtons();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            var result = DoVerification();

            if (result.State == VerificationState.InvalidName)
            {
                errorProvider.SetError(nameTextBox, "Name must be between 2 and 40 characters long!");
                return;
            }

            if (result.State == VerificationState.InvalidFromDate)
            {
                errorProvider.SetError(fromDateTimePicker, "Date cannot be set in the past!");
                return;
            }

            if (result.State == VerificationState.InvalidToDate)
            {
                errorProvider.SetError(toDateTimePicker, "To date cannot be before From date!");
                return;
            }

            if (result.State == VerificationState.ConflictWithRequired)
            {
                var conflicts = (List<KeyValuePair<User, List<Event>>>)result.Value;
                var res = MessageBox.Show(
                    $"The following users have a scheduling conflict:\n" + 
                    $"{String.Join("\n", conflicts.Map((x) => $"{x.Key.FirstName} {x.Key.LastName} ({x.Key.Login})"))}",
                    "Scheduling error!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                return;
            }

            if (result.State == VerificationState.ConflictWithOptional)
            {
                var conflicts = (List<KeyValuePair<User, List<Event>>>)result.Value;
                var res = MessageBox.Show(
                    $"The following users have a scheduling conflict with optional events:\n" + 
                    $"{String.Join("\n", conflicts.Map((x) => $"{x.Key.FirstName} {x.Key.LastName} ({x.Key.Login})"))}\n" +
                    $"Do you want to clear their schedule?",
                    "Scheduling error!",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Error
                );

                if (res == DialogResult.Yes)
                {
                    conflicts.ForEach((x) => x.Value.ForEach((y) => { y.RemoveParticipant(x.Key); y.Save(); }));
                }
                else
                {
                    return;
                }
            }

            this.name = this.nameTextBox.Text;
            this.description = this.descriptionTextBox.Text;
            this.location = this.locationTextBox.Text;
            this.from = this.fromDateTimePicker.Value.ToUniversalTime();
            this.to = this.toDateTimePicker.Value.ToUniversalTime();

            if (this.Evt.IsSome)
            {
                this.cmds.Add(new Command(() =>
                {
                    this.Evt.IfSome((evt) => 
                    {
                        evt.Name = this.name;
                        evt.Description = this.description;
                        evt.Location = this.location;
                        evt.From = this.from.ToUniversalTime();
                        evt.To = this.to.ToUniversalTime();

                        evt.Save();
                    });
                }));
            }

            cmds.Do();
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
