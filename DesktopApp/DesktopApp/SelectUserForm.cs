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
    public partial class SelectUserForm : Form
    {
        public SelectUserForm(Option<DataTypes.UserRole> role)
        {
            InitializeComponent();
            this.role = role;
            this.filter = new List<User>();
        }

        public SelectUserForm(Option<DataTypes.UserRole> role, List<User> filter)
        {
            InitializeComponent();
            this.role = role;
            this.filter = filter;
        }

        private void SelectUserForm_Load(object sender, EventArgs e)
        {
            this.LoadData();
            this.PopulateList();
            this.SetupButtons();
        }

        private void LoadData()
        {
            role.Match((role) => {
                this.users = User.FindByRole(role).Filter((user) => !this.filter.Contains(user)).ToArr().ToList();
            }, () => {
                this.users = User.All().Filter((user) => !this.filter.Contains(user)).ToArr().ToList();
            });
        }

        private void PopulateList()
        {
            userListView.Items.Clear();
            foreach (var user in users)
            {
                var item = new ListViewItem(new string[] { user.FirstName, user.LastName, DataTypes.UserRoleStrings.roles[user.Role] });
                item.Tag = user;

                userListView.Items.Add(item);
            }
        }

        private void SetupButtons()
        {
            this.okButton.Enabled = true;
            this.cancelButton.Enabled = true;

            if (userListView.SelectedItems.Count == 0)
                this.okButton.Enabled = false;
        }

        private Option<DataTypes.UserRole> role;
        private List<User> users;
        private List<User> filter;

        public User SelectedUser { get; set; }

        private void userListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SetupButtons();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (this.userListView.SelectedItems.Count == 0)
                throw new ApplicationException("Attempted to confirm with no user selected!");

            this.SelectedUser = (User)this.userListView.SelectedItems[0].Tag;
            this.DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.SelectedUser = null;
            this.DialogResult = DialogResult.Cancel;
        }

        private void userListView_DoubleClick(object sender, EventArgs e)
        {
            if (this.userListView.SelectedItems.Count == 0)
                throw new ApplicationException("Attempted to confirm with no user selected!");

            this.SelectedUser = (User)this.userListView.SelectedItems[0].Tag;
            this.DialogResult = DialogResult.OK;
        }
    }
}
