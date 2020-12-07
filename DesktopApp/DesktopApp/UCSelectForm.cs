using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopApp
{
    public partial class UCSelectForm : Form
    {
        public UCSelectForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var teamForm = new TeamListForm();
            teamForm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var loginForm = new LoginForm();
            var result = loginForm.ShowDialog();

            if (result != DialogResult.OK)
            {
                return;
            }

            var form = new EventListForm(loginForm.user);
            form.Show();
        }
    }
}
