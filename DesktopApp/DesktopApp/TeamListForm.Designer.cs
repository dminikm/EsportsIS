
namespace DesktopApp
{
    partial class TeamListForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.teamListView = new System.Windows.Forms.ListView();
            this.NameColumn = new System.Windows.Forms.ColumnHeader();
            this.GameColumn = new System.Windows.Forms.ColumnHeader();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.refreshTeamsButton = new System.Windows.Forms.Button();
            this.removeTeamButton = new System.Windows.Forms.Button();
            this.editTeamButton = new System.Windows.Forms.Button();
            this.addTeamButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // teamListView
            // 
            this.teamListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.NameColumn,
            this.GameColumn});
            this.teamListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.teamListView.FullRowSelect = true;
            this.teamListView.GridLines = true;
            this.teamListView.HideSelection = false;
            this.teamListView.Location = new System.Drawing.Point(3, 3);
            this.teamListView.MultiSelect = false;
            this.teamListView.Name = "teamListView";
            this.teamListView.Size = new System.Drawing.Size(714, 444);
            this.teamListView.TabIndex = 0;
            this.teamListView.UseCompatibleStateImageBehavior = false;
            this.teamListView.View = System.Windows.Forms.View.Details;
            this.teamListView.SelectedIndexChanged += new System.EventHandler(this.teamListView_SelectedIndexChanged);
            // 
            // NameColumn
            // 
            this.NameColumn.Text = "Name";
            // 
            // GameColumn
            // 
            this.GameColumn.Text = "Game";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel1.Controls.Add(this.teamListView, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.refreshTeamsButton);
            this.panel1.Controls.Add(this.removeTeamButton);
            this.panel1.Controls.Add(this.editTeamButton);
            this.panel1.Controls.Add(this.addTeamButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(723, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(74, 444);
            this.panel1.TabIndex = 1;
            // 
            // refreshTeamsButton
            // 
            this.refreshTeamsButton.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.refreshTeamsButton.Location = new System.Drawing.Point(5, 198);
            this.refreshTeamsButton.Name = "refreshTeamsButton";
            this.refreshTeamsButton.Size = new System.Drawing.Size(60, 60);
            this.refreshTeamsButton.TabIndex = 3;
            this.refreshTeamsButton.Text = "🔄";
            this.refreshTeamsButton.UseVisualStyleBackColor = true;
            this.refreshTeamsButton.Click += new System.EventHandler(this.refreshTeamsButton_Click);
            // 
            // removeTeamButton
            // 
            this.removeTeamButton.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.removeTeamButton.Location = new System.Drawing.Point(5, 132);
            this.removeTeamButton.Name = "removeTeamButton";
            this.removeTeamButton.Size = new System.Drawing.Size(60, 60);
            this.removeTeamButton.TabIndex = 2;
            this.removeTeamButton.Text = "🗑️";
            this.removeTeamButton.UseVisualStyleBackColor = true;
            this.removeTeamButton.Click += new System.EventHandler(this.removeTeamButton_Click);
            // 
            // editTeamButton
            // 
            this.editTeamButton.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.editTeamButton.Location = new System.Drawing.Point(5, 66);
            this.editTeamButton.Name = "editTeamButton";
            this.editTeamButton.Size = new System.Drawing.Size(60, 60);
            this.editTeamButton.TabIndex = 1;
            this.editTeamButton.Text = "🔧";
            this.editTeamButton.UseVisualStyleBackColor = true;
            // 
            // addTeamButton
            // 
            this.addTeamButton.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.addTeamButton.Location = new System.Drawing.Point(5, 0);
            this.addTeamButton.Name = "addTeamButton";
            this.addTeamButton.Size = new System.Drawing.Size(60, 60);
            this.addTeamButton.TabIndex = 0;
            this.addTeamButton.Text = "➕";
            this.addTeamButton.UseVisualStyleBackColor = true;
            this.addTeamButton.Click += new System.EventHandler(this.addTeamButton_Click);
            // 
            // TeamListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "TeamListForm";
            this.ShowIcon = false;
            this.Text = "👥 Teams";
            this.Load += new System.EventHandler(this.TeamListForm_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView teamListView;
        private System.Windows.Forms.ColumnHeader NameColumn;
        private System.Windows.Forms.ColumnHeader Game;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button refreshTeamsButton;
        private System.Windows.Forms.Button removeTeamButton;
        private System.Windows.Forms.Button editTeamButton;
        private System.Windows.Forms.Button addTeamButton;
        private System.Windows.Forms.ColumnHeader GameColumn;
    }
}