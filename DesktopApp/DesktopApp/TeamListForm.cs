using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BusinessLayer;
using LanguageExt;

namespace DesktopApp
{
    public partial class TeamListForm : Form
    {
        public TeamListForm()
        {
            InitializeComponent();
        }

        private void TeamListForm_Load(object sender, EventArgs e)
        {
            this.LoadData();
            this.PopulateList();
            this.SetupButtons();
        }

        private void LoadData()
        {
            this.teams = Team.All();
        }

        private void SetupButtons()
        {
            addTeamButton.Enabled = true;
            refreshTeamsButton.Enabled = true;

            removeTeamButton.Enabled = false;
            editTeamButton.Enabled = false;

            if (teamListView.SelectedIndices.Count > 0)
            {
                removeTeamButton.Enabled = true;
                editTeamButton.Enabled = true;
            }
        }

        private void PopulateList()
        {
            var selectedID = (teamListView.SelectedItems.Count > 0) ? ((Team)teamListView.SelectedItems[0].Tag).TeamID : Option<int>.None;
            Option<ListViewItem> selectedItem = null;

            // Populate with items
            teamListView.Items.Clear();
            foreach (var team in teams)
            {
                var item = new ListViewItem(new string[] { team.Name, team.Game });
                item.Tag = team;

                teamListView.Items.Add(item);

                if (team.TeamID == selectedID)
                {
                    selectedItem = item;
                }
            }

            // Select items
            teamListView.SelectedItems.Clear();
            selectedItem.IfSome((item) => item.Selected = item.Focused = true);
        }

        private List<Team> teams;

        private void refreshTeamsButton_Click(object sender, EventArgs e)
        {
            this.LoadData();
            this.PopulateList();
            this.SetupButtons();
        }

        private void teamListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetupButtons();
        }

        private void removeTeamButton_Click(object sender, EventArgs e)
        {
            if (this.teamListView.SelectedItems.Count == 0)
                throw new ApplicationException("Attempted to delete a team without having one selected!");

            var selectedIndex = this.teamListView.SelectedIndices[0];

            // Remove from list of teams
            var team = (Team)this.teamListView.SelectedItems[0].Tag;
            this.teams.Remove(team);

            // Repopulate the listView and fix selection
            this.PopulateList();
            this.teamListView.SelectedIndices.Clear();

            if (this.teams.Count > 0)
            {
                this.teamListView.SelectedIndices.Add(Math.Max(0, Math.Min(this.teams.Count - 1, selectedIndex)));
            }

            // Enable/disable buttons
            this.SetupButtons();

            // Delete the actual team (consider async?)
            team.Delete();
        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            var dialog = new AddEditTeamForm(Option<Team>.None);
            dialog.ShowDialog();

            this.LoadData();
            this.PopulateList();
            this.SetupButtons();
        }

        private void editTeamButton_Click(object sender, EventArgs e)
        {
            var team = (Team)teamListView.SelectedItems[0].Tag;
            var dialog = new AddEditTeamForm(team);
            dialog.ShowDialog();

            this.LoadData();
            this.PopulateList();
            this.SetupButtons();
        }
    }
}
