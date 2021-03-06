﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            this.coach = Option<User>.None;
            this.players = new List<User>();

            this.Text = this.nameLabel.Text = (team.IsNone) ?
                "👥 Add new team!" :
                "👥 Edit team!";

            this.cmds = new CommandQueue();

            // If team is null, first thing to do is create a new one
            this.cmds.Add(new Command(() =>
            {
                this.team.IfNone(() => this.team = Team.Create("", ""));
            }));

            team.IfSome((t) =>
            {
                players = t.Players.ToList();
                nameTextBox.Text = t.Name;
                gameTextBox.Text = t.Game;
                coach = t.CoachID.Match((coachID) => User.Find(coachID), () => Option<User>.None);
            });
        }

        private void AddEditTeam_Load(object sender, EventArgs e)
        {
            this.Populate();
            this.SetupButtons();
        }

        private void Populate()
        {
            // Populate coach field
            coach.Match((coach) => coachTextBox.Text = coach.FirstName + " " + coach.LastName, () => coachTextBox.Text = "");

            // Populate listView
            playerListView.Items.Clear();
            foreach (var player in players)
            {
                var item = new ListViewItem(new string[] { player.FirstName + " " + player.LastName });
                item.Tag = player;

                playerListView.Items.Add(item);
            }
        }

        private void SetupButtons()
        {
            addPlayerButton.Enabled = true;
            removePlayerButton.Enabled = true;

            if (playerListView.SelectedIndices.Count != 0)
                removePlayerButton.Enabled = false;
        }

        private Option<Team> team;
        private CommandQueue cmds;

        private Option<User> coach;
        private List<User> players;

        private void linkCoachButton_Click(object sender, EventArgs e)
        {
            if (coach.IsSome)
            {
                coach = Option<User>.None;
                this.Populate();
                this.SetupButtons();
            }

            var form = new SelectUserForm(DataTypes.UserRole.Coach);
            var result = form.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.coach = form.SelectedUser;
                this.Populate();
                this.SetupButtons();
            }
        }

        private void addPlayerButton_Click(object sender, EventArgs e)
        {
            var form = new SelectUserForm(DataTypes.UserRole.Player, players);
            var result = form.ShowDialog();

            if (result == DialogResult.OK)
            {
                var newPlayer = form.SelectedUser;

                this.players.Add(newPlayer);
                this.cmds.Add(new Command(() =>
                {
                    this.team.IfSome((team) => team.Players.Add(newPlayer));
                }));

                this.Populate();
                this.SetupButtons();
            }

        }

        private void removePlayerButton_Click(object sender, EventArgs e)
        {
            var player = (User)this.playerListView.SelectedItems[0].Tag;
            this.players.Remove(player);
            
            this.cmds.Add(new Command(() =>
            {
                this.team.IfSome((team) => team.Players.Remove(player));
            }));

            this.Populate();
            this.SetupButtons();
        }

        enum VerificationState
        {
            Ok,
            NoCoach,
            CoachAlreadyUsed,
            PlayerAlreadyUsed,
        }

        struct VerificationResult
        {
            public VerificationState State { get; set; }
            public object Value { get; set; }
        }

        private VerificationResult DoVerification()
        {
            if (coach.IsNone)
            {
                return new VerificationResult() {
                    State = VerificationState.NoCoach,
                    Value = null,
                };
            }

            var conflictingTeamForCoach = Team.FindByCoach(coach.IfNone(() => throw new ApplicationException("This cannot happen!")));

            if (conflictingTeamForCoach.IsSome)
            {
                var team = conflictingTeamForCoach.IfNone(() => throw new ApplicationException("This cannot happen!"));
                if (team != this.team)
                {
                    return new VerificationResult()
                    {
                        State = VerificationState.CoachAlreadyUsed,
                        Value = team
                    };
                }
            }

            var conflictingPlayers = new List<KeyValuePair<User, List<Team>>>();
            foreach (var player in players)
            {
                var teams = player.GetTeams().Filter((team) => team.TeamID != this.team.Match((tm) => tm.TeamID, () => Option<int>.None));

                if (teams.Length() != 0)
                {
                    conflictingPlayers.Add(new KeyValuePair<User, List<Team>>(player, teams.ToList()));
                }
            }

            if (conflictingPlayers.Count > 0)
            {
                return new VerificationResult()
                {
                    State = VerificationState.PlayerAlreadyUsed,
                    Value = conflictingPlayers
                };
            }

            return new VerificationResult()
            {
                State = VerificationState.Ok,
                Value = null,
            };
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            var state = DoVerification();

            if (state.State == VerificationState.NoCoach)
            {
                errorProvider.SetError(coachTextBox, "Team must have a coach!");
                return;
            }

            if (state.State == VerificationState.CoachAlreadyUsed)
            {
                MessageBox.Show("Coach is already coaching another team!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (state.State == VerificationState.PlayerAlreadyUsed)
            {
                var dialog = new RemoveUserFromConfligtingTeamsForm((List<KeyValuePair<User, List<Team>>>)state.Value);
                var result = dialog.ShowDialog();

                if (result == DialogResult.Cancel)
                    return;

                dialog.Cmds.ForEach((cmd) => cmds.Add(cmd));
            }

            cmds.Add(new Command(() =>
            {
                this.team.IfSome((team) =>
                {
                    team.Game = gameTextBox.Text;
                    team.Name = nameTextBox.Text;
                    team.CoachID = this.coach.Match((coach) => coach.UserID, () => Option<int>.None);

                    team.Save();
                });
            }));

            cmds.Do();
            this.DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
