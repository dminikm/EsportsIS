
namespace DesktopApp
{
    partial class EventListForm
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.eventListView = new System.Windows.Forms.ListView();
            this.nameColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.fromColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.typeColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.panel2 = new System.Windows.Forms.Panel();
            this.refreshEventsButton = new System.Windows.Forms.Button();
            this.removeEventButton = new System.Windows.Forms.Button();
            this.editEventButton = new System.Windows.Forms.Button();
            this.addEventButton = new System.Windows.Forms.Button();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addTrainingEventMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.addMatchEventMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.addTournamentEventMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.addCustomEventMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.button1 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 450);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 53);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(794, 394);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.eventListView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(708, 388);
            this.panel1.TabIndex = 0;
            // 
            // eventListView
            // 
            this.eventListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumnHeader,
            this.fromColumnHeader,
            this.typeColumnHeader});
            this.eventListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventListView.FullRowSelect = true;
            this.eventListView.GridLines = true;
            this.eventListView.HideSelection = false;
            this.eventListView.Location = new System.Drawing.Point(0, 0);
            this.eventListView.MultiSelect = false;
            this.eventListView.Name = "eventListView";
            this.eventListView.Size = new System.Drawing.Size(708, 388);
            this.eventListView.TabIndex = 0;
            this.eventListView.UseCompatibleStateImageBehavior = false;
            this.eventListView.View = System.Windows.Forms.View.Details;
            this.eventListView.SelectedIndexChanged += new System.EventHandler(this.eventListView_SelectedIndexChanged);
            // 
            // nameColumnHeader
            // 
            this.nameColumnHeader.Text = "Name";
            // 
            // fromColumnHeader
            // 
            this.fromColumnHeader.Text = "From - To";
            // 
            // typeColumnHeader
            // 
            this.typeColumnHeader.Text = "Event Type";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.refreshEventsButton);
            this.panel2.Controls.Add(this.removeEventButton);
            this.panel2.Controls.Add(this.editEventButton);
            this.panel2.Controls.Add(this.addEventButton);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(717, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(74, 388);
            this.panel2.TabIndex = 1;
            // 
            // refreshEventsButton
            // 
            this.refreshEventsButton.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.refreshEventsButton.Location = new System.Drawing.Point(8, 198);
            this.refreshEventsButton.Name = "refreshEventsButton";
            this.refreshEventsButton.Size = new System.Drawing.Size(60, 60);
            this.refreshEventsButton.TabIndex = 7;
            this.refreshEventsButton.Text = "🔄";
            this.refreshEventsButton.UseVisualStyleBackColor = true;
            this.refreshEventsButton.Click += new System.EventHandler(this.refreshEventsButton_Click);
            // 
            // removeEventButton
            // 
            this.removeEventButton.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.removeEventButton.Location = new System.Drawing.Point(8, 132);
            this.removeEventButton.Name = "removeEventButton";
            this.removeEventButton.Size = new System.Drawing.Size(60, 60);
            this.removeEventButton.TabIndex = 6;
            this.removeEventButton.Text = "🗑️";
            this.removeEventButton.UseVisualStyleBackColor = true;
            this.removeEventButton.Click += new System.EventHandler(this.removeEventButton_Click);
            // 
            // editEventButton
            // 
            this.editEventButton.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.editEventButton.Location = new System.Drawing.Point(8, 66);
            this.editEventButton.Name = "editEventButton";
            this.editEventButton.Size = new System.Drawing.Size(60, 60);
            this.editEventButton.TabIndex = 5;
            this.editEventButton.Text = "🔧";
            this.editEventButton.UseVisualStyleBackColor = true;
            this.editEventButton.Click += new System.EventHandler(this.editEventButton_Click);
            // 
            // addEventButton
            // 
            this.addEventButton.ContextMenuStrip = this.contextMenuStrip;
            this.addEventButton.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.addEventButton.Location = new System.Drawing.Point(8, 0);
            this.addEventButton.Name = "addEventButton";
            this.addEventButton.Size = new System.Drawing.Size(60, 60);
            this.addEventButton.TabIndex = 4;
            this.addEventButton.Text = "➕";
            this.addEventButton.UseVisualStyleBackColor = true;
            this.addEventButton.Click += new System.EventHandler(this.addEventButton_Click);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addTrainingEventMenu,
            this.addMatchEventMenu,
            this.addTournamentEventMenu,
            this.addCustomEventMenu});
            this.contextMenuStrip.Name = "contextMenuStrip1";
            this.contextMenuStrip.Size = new System.Drawing.Size(198, 92);
            // 
            // addTrainingEventMenu
            // 
            this.addTrainingEventMenu.Name = "addTrainingEventMenu";
            this.addTrainingEventMenu.Size = new System.Drawing.Size(197, 22);
            this.addTrainingEventMenu.Text = "New Training Event";
            // 
            // addMatchEventMenu
            // 
            this.addMatchEventMenu.Name = "addMatchEventMenu";
            this.addMatchEventMenu.Size = new System.Drawing.Size(197, 22);
            this.addMatchEventMenu.Text = "New Match Event";
            // 
            // addTournamentEventMenu
            // 
            this.addTournamentEventMenu.Name = "addTournamentEventMenu";
            this.addTournamentEventMenu.Size = new System.Drawing.Size(197, 22);
            this.addTournamentEventMenu.Text = "New Tournament Event";
            // 
            // addCustomEventMenu
            // 
            this.addCustomEventMenu.Name = "addCustomEventMenu";
            this.addCustomEventMenu.Size = new System.Drawing.Size(197, 22);
            this.addCustomEventMenu.Text = "New Custom Event";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(73, 144);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.titleLabel);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(794, 44);
            this.panel3.TabIndex = 1;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.titleLabel.Location = new System.Drawing.Point(9, 6);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(255, 37);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Events for: mec0037";
            // 
            // EventListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "EventListForm";
            this.ShowIcon = false;
            this.Text = "📅 Events";
            this.Load += new System.EventHandler(this.EventListForm_Load);
            this.ResizeEnd += new System.EventHandler(this.EventListForm_ResizeEnd);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.contextMenuStrip.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button refreshEventsButton;
        private System.Windows.Forms.Button removeEventButton;
        private System.Windows.Forms.Button editEventButton;
        private System.Windows.Forms.Button addEventButton;
        private System.Windows.Forms.ListView eventListView;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.ColumnHeader nameColumnHeader;
        private System.Windows.Forms.ColumnHeader fromColumnHeader;
        private System.Windows.Forms.ColumnHeader typeColumnHeader;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addTrainingEventMenu;
        private System.Windows.Forms.ToolStripMenuItem addMatchEventMenu;
        private System.Windows.Forms.ToolStripMenuItem addTournamentEventMenu;
        private System.Windows.Forms.ToolStripMenuItem addCustomEventMenu;
    }
}