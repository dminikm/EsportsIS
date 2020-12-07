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
        }

        private void LoadData()
        {
            this.events = this.user.GetUpcomingEvents();
        }

        private void PopulateList()
        {
            var selectedID = (eventListView.SelectedItems.Count > 0) ? ((Event)eventListView.SelectedItems[0].Tag).EventID : Option<int>.None;
            Option<ListViewItem> selectedItem = null;

            eventListView.Items.Clear();
            foreach (var evt in this.events)
            {
                var unix = new DateTime(1970, 1, 1, 0, 0, 0);
                var from = unix.AddMilliseconds(evt.From).ToShortDateString();
                var to = unix.AddMilliseconds(evt.To).ToShortDateString(); 

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
        }

        private void addEventButton_Click(object sender, EventArgs e)
        {
            // TODO: Use context menu
            var form = new AddEditTrainingEvent(null, user);
            form.ShowDialog();

            this.LoadData();
            this.PopulateList();
            this.SetupButtons();
        }

        private void editEventButton_Click(object sender, EventArgs e)
        {

        }

        private void removeEventButton_Click(object sender, EventArgs e)
        {

        }

        private void refreshEventsButton_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        private void eventListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetupButtons();
        }
    }
}
