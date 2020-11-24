using BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DesktopApp
{
    public partial class RemoveUserFromConfligtingTeamsForm : Form
    {
        public RemoveUserFromConfligtingTeamsForm(List<KeyValuePair<User, List<Team>>> conflicting)
        {
            InitializeComponent();

            conflictingPlayers = conflicting;
            Cmds = new List<Command>();
        }

        public void SetupButtons()
        {
            this.removeFromTeamButton.Enabled = true;

            if (listView1.SelectedItems.Count == 0)
                this.removeFromTeamButton.Enabled = false;
        }

        public void PopulateList()
        {
            listView1.Clear();

            foreach (var pair in conflictingPlayers)
            {
                var user = pair.Key;

                foreach (var team in pair.Value)
                {
                    var item = new ListViewItem(new string[] { user.FirstName + " " + user.LastName, team.Name + "( " + team.Game + " )" });
                    item.Tag = new KeyValuePair<User, Team>(user, team);

                    listView1.Items.Add(item);
                }
            }
        }

        private List<KeyValuePair<User, List<Team>>> conflictingPlayers;
        public List<Command> Cmds { get; set; }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void removeFromTeamButton_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                throw new ApplicationException("Attempted to remove a player from team without having one selected!");

            var pair = (KeyValuePair<User, Team>)listView1.SelectedItems[0].Tag;

            Cmds.Add(new Command(() => {
                pair.Value.Players.Remove(pair.Key);
                pair.Value.Players.Do();                // Dont save the whole team
            }));

            PopulateList();
            SetupButtons();
        }

        private void RemoveUserFromConfligtingTeamsForm_Load(object sender, EventArgs e)
        {
            PopulateList();
            SetupButtons();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetupButtons();
        }
    }
}
