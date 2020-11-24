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
    public partial class SelectUserForm : Form
    {
        public SelectUserForm()
        {
            InitializeComponent();
        }

        private void SelectUserForm_Load(object sender, EventArgs e)
        {
            this.LoadData();
            this.PopulateList();
            this.SetupButtons();
        }

        private void LoadData()
        {

        }

        private void PopulateList()
        {

        }

        private void SetupButtons()
        {

        }

        private List<User> users;
    }
}
