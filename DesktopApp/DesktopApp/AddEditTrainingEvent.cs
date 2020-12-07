using BusinessLayer;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DesktopApp
{
    public partial class AddEditTrainingEvent : Form
    {
        public AddEditTrainingEvent(Option<TrainingEvent> evt, User coach)
        {
            InitializeComponent();
            this.Evt = evt;

            this.Text = this.nameLabel.Text = (evt.IsNone) ?
                "🏋️ Add new training event!" :
                "🏋️ Edit training event!";

            this.cmds = new CommandQueue();
            this.cmds.Add(new Command(() =>
            {
                this.Evt.IfNone(() => this.Evt = TrainingEvent.Create(
                    this.nameTextBox.Text,
                    this.descriptionTextBox.Text,
                    this.fromDateTimePicker.Value,
                    this.toDateTimePicker.Value,
                    this.participants
                ));
            }));

            this.name = "";
            this.description = "";
            this.from = DateTime.Now;
            this.to = DateTime.Now;
            this.participants = new List<User>();
            this.participants.Add(coach);

            evt.IfSome((x) =>
            {
                var unix = new DateTime(1970, 1, 1, 0, 0, 0);

                this.name = x.Name;
                this.description = x.Description;
                this.from = unix.AddMilliseconds(x.From);
                this.to = unix.AddMilliseconds(x.To);
                this.participants = x.Participants.ToList();
            });
        }

        private void PopulateFields()
        {
            nameTextBox.Text = name;
            descriptionTextBox.Text = description;
            fromDateTimePicker.Value = from;
            toDateTimePicker.Value = to;
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

        private void AddEditTrainingEvent_Load(object sender, EventArgs e)
        {
            PopulateFields();
            PopulateList();
            SetupButtons();
        }

        public Option<TrainingEvent> Evt { get; set; }
        private CommandQueue cmds;

        private List<User> participants;
        private string name;
        private string description;
        private DateTime from;
        private DateTime to;

        private void participantListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetupButtons();
        }

        private void addParticipantButton_Click(object sender, EventArgs e)
        {
            var participant = (User)this.participantListView.SelectedItems[0].Tag;
            this.participants.Add(participant);

            if (this.Evt.IsSome)
            {
                this.cmds.Add(new Command(() => 
                {
                    this.Evt.IfSome((evt) => evt.Participants.Add(participant));
                }));
            }

            PopulateList();
            SetupButtons();
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
            // TODO: Validate

            this.name = this.nameTextBox.Text;
            this.description = this.descriptionTextBox.Text;
            this.from = this.fromDateTimePicker.Value;
            this.to = this.toDateTimePicker.Value;

            if (this.Evt.IsSome)
            {
                this.cmds.Add(new Command(() =>
                {
                    this.Evt.IfSome((evt) => 
                    {
                        evt.Name = this.name;
                        evt.Description = this.description;
                        evt.From = ((DateTimeOffset)this.from).ToUnixTimeMilliseconds();
                        evt.To = ((DateTimeOffset)this.to).ToUnixTimeMilliseconds();
                    });
                }));
            }

            cmds.Do();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
