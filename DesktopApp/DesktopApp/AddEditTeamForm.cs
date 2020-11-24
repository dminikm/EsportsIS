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
    public partial class AddEditTeamForm : Form
    {
        public AddEditTeamForm(Option<Team> team)
        {
            InitializeComponent();
            this.team = team;

            this.Text = this.nameLabel.Text = (team.IsNone) ?
                "👥 Add new team!" :
                "👥 Edit team!";
        }

        private void AddEditTeam_Load(object sender, EventArgs e)
        {

        }

        private Option<Team> team;
    }
}
