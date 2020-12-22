using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LanguageExt;

namespace DesktopApp
{
    public partial class EventListForm : Form
    {
        public EventListForm(User user)
        {
            InitializeComponent();
            this.user = user;

            addTrainingEventMenu.Click += new EventHandler(this.NewTrainingEvent);
            addMatchEventMenu.Click += new EventHandler(this.NewMatchEvent);
            addTournamentEventMenu.Click += new EventHandler(this.NewTournamentEvent);
            addCustomEventMenu.Click += new EventHandler(this.NewCustomEvent);
        }

        private void LoadData()
        {
            this.events = this.user.GetUpcomingEvents();
        }

        private void NewTrainingEvent(object sender, EventArgs e)
        {
            var form = new AddEditTrainingEventForm(null, user);
            form.ShowDialog();

            this.LoadData();
            this.PopulateList();
            this.SetupButtons();
        }

        private void NewMatchEvent(object sender, EventArgs e)
        {
            var form = new AddEditMatchEventForm(null, user);
            form.ShowDialog();

            this.LoadData();
            this.PopulateList();
            this.SetupButtons();
        }

        private void NewTournamentEvent(object sender, EventArgs e)
        {
            var form = new AddEditTournamentEventForm(null, user);
            form.ShowDialog();

            this.LoadData();
            this.PopulateList();
            this.SetupButtons();
        }

        private void NewCustomEvent(object sender, EventArgs e)
        {
            var form = new AddEditCustomEventForm(null, user);
            form.ShowDialog();

            this.LoadData();
            this.PopulateList();
            this.SetupButtons();
        }

        private void PopulateList()
        {
            var selectedID = (eventListView.SelectedItems.Count > 0) ? ((Event)eventListView.SelectedItems[0].Tag).EventID : Option<int>.None;
            Option<ListViewItem> selectedItem = null;

            eventListView.Items.Clear();
            foreach (var evt in this.events)
            {
                var from = evt.From.ToShortDateString();
                var to = evt.To.ToShortDateString(); 

                var item = new ListViewItem(new string[] { evt.Name, $"{from} - {to}", evt.Type });
                item.Tag = evt;

                eventListView.Items.Add(item);

                if (evt.EventID == selectedID)
                {
                    selectedItem = item;
                }
            }

            eventListView.SelectedItems.Clear();
            selectedItem.IfSome((item) => item.Selected = item.Focused = true);

            Common.ResizeColumns(eventListView);
        }

        private void SetupButtons()
        {
            addEventButton.Enabled = true;
            refreshEventsButton.Enabled = true;

            removeEventButton.Enabled = false;
            editEventButton.Enabled = false;

            if (eventListView.SelectedIndices.Count > 0)
            {
                removeEventButton.Enabled = true;
                editEventButton.Enabled = true;
            }
        }

        private User user;
        private List<Event> events;

        private void EventListForm_Load(object sender, EventArgs e)
        {
            LoadData();

            PopulateList();
            SetupButtons();

            titleLabel.Text = $"Events for: {user.Login}";
        }

        private void addEventButton_Click(object sender, EventArgs e)
        {
            contextMenuStrip.Show(addEventButton, addEventButton.PointToClient(Cursor.Position));
        }

        private void editEventButton_Click(object sender, EventArgs e)
        {
            var evt = (Event)eventListView.SelectedItems[0].Tag;

            DialogResult res = DialogResult.None;
            if (evt.Type == "training")
            {
                var form = new AddEditTrainingEventForm((TrainingEvent)evt, user);
                res = form.ShowDialog();
            }
            else if (evt.Type == "match")
            {
                var form = new AddEditMatchEventForm((MatchEvent)evt, user);
                res = form.ShowDialog();
            }
            else if (evt.Type == "tournament")
            {
                var form = new AddEditTournamentEventForm((TournamentEvent)evt, user);
                res = form.ShowDialog();
            }
            else
            {
                var form = new AddEditCustomEventForm((CustomEvent)evt, user);
                res = form.ShowDialog();
            }

            if (res == DialogResult.OK)
            {
                this.PopulateList();
                this.SetupButtons();
            }
        }

        private void removeEventButton_Click(object sender, EventArgs e)
        {
            var evt = (Event)eventListView.SelectedItems[0].Tag;
            events.Remove(evt);
            evt.Delete();

            PopulateList();
            SetupButtons();
        }

        private void refreshEventsButton_Click(object sender, EventArgs e)
        {
            this.LoadData();
            this.PopulateList();
            this.SetupButtons();
        }

        private void eventListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetupButtons();
        }

        private void EventListForm_ResizeEnd(object sender, EventArgs e)
        {
            Common.ResizeColumns(eventListView);
        }
    }
}
