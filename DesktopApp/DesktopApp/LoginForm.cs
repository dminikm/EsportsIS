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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

            this.user = null;
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            var user = User.FindByUsernamePassword(usernameTextBox.Text, passwordTextBox.Text);

            if (user.IsNone)
            {
                errorProvider1.SetError(this, "Username or Password doesn't match!");
                return;
            }

            this.user = user.IfNoneUnsafe(() => null);

            if (this.user.Role != DataTypes.UserRole.Coach)
            {
                errorProvider1.SetError(this, "User isn't a coach!");
                this.user = null;
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        public User user { get; set; }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // DEBUG:
            usernameTextBox.Text = "tes0001";
            passwordTextBox.Text = "abc123";

            loginButton.PerformClick();
        }
    }
}
