namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblFirstName = new Label();
            tbFirstName = new TextBox();
            lblLastName = new Label();
            tbLastName = new TextBox();
            lblAge = new Label();
            tbAge = new TextBox();
            lblEmail = new Label();
            tbEmail = new TextBox();
            btnAdd = new Button();
            btnEdit = new Button();
            btnSave = new Button();
            btnCancel = new Button();
            btnDelete = new Button();
            dgvPersons = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dgvPersons).BeginInit();
            SuspendLayout();
            // 
            // lblFirstName
            // 
            lblFirstName.AutoSize = true;
            lblFirstName.Location = new Point(20, 20);
            lblFirstName.Name = "lblFirstName";
            lblFirstName.Size = new Size(80, 20);
            lblFirstName.TabIndex = 0;
            lblFirstName.Text = "First name";
            // 
            // tbFirstName
            // 
            tbFirstName.Location = new Point(110, 17);
            tbFirstName.Name = "tbFirstName";
            tbFirstName.Size = new Size(180, 27);
            tbFirstName.TabIndex = 1;
            // 
            // lblLastName
            // 
            lblLastName.AutoSize = true;
            lblLastName.Location = new Point(20, 55);
            lblLastName.Name = "lblLastName";
            lblLastName.Size = new Size(79, 20);
            lblLastName.TabIndex = 2;
            lblLastName.Text = "Last name";
            // 
            // tbLastName
            // 
            tbLastName.Location = new Point(110, 52);
            tbLastName.Name = "tbLastName";
            tbLastName.Size = new Size(180, 27);
            tbLastName.TabIndex = 3;
            // 
            // lblAge
            // 
            lblAge.AutoSize = true;
            lblAge.Location = new Point(20, 90);
            lblAge.Name = "lblAge";
            lblAge.Size = new Size(34, 20);
            lblAge.TabIndex = 4;
            lblAge.Text = "Age";
            // 
            // tbAge
            // 
            tbAge.Location = new Point(110, 87);
            tbAge.Name = "tbAge";
            tbAge.Size = new Size(60, 27);
            tbAge.TabIndex = 5;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(20, 125);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(45, 20);
            lblEmail.TabIndex = 6;
            lblEmail.Text = "Email";
            // 
            // tbEmail
            // 
            tbEmail.Location = new Point(110, 122);
            tbEmail.Name = "tbEmail";
            tbEmail.Size = new Size(220, 27);
            tbEmail.TabIndex = 7;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(350, 17);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(94, 29);
            btnAdd.TabIndex = 8;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnEdit
            // 
            btnEdit.Location = new Point(350, 52);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(94, 29);
            btnEdit.TabIndex = 10;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += btnEdit_Click;
            // 
            // btnSave
            // 
            btnSave.Location = new Point(460, 17);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(94, 29);
            btnSave.TabIndex = 11;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(460, 52);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(94, 29);
            btnCancel.TabIndex = 12;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(570, 35);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(94, 29);
            btnDelete.TabIndex = 13;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // dgvPersons
            // 
            dgvPersons.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPersons.Location = new Point(20, 170);
            dgvPersons.Name = "dgvPersons";
            dgvPersons.Size = new Size(760, 260);
            dgvPersons.TabIndex = 9;
            dgvPersons.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPersons.AllowUserToAddRows = false;
            dgvPersons.ReadOnly = true;
            dgvPersons.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPersons.MultiSelect = false;
            dgvPersons.SelectionChanged += dgvPersons_SelectionChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(dgvPersons);
            Controls.Add(btnDelete);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(btnEdit);
            Controls.Add(btnAdd);
            Controls.Add(tbEmail);
            Controls.Add(lblEmail);
            Controls.Add(tbAge);
            Controls.Add(lblAge);
            Controls.Add(tbLastName);
            Controls.Add(lblLastName);
            Controls.Add(tbFirstName);
            Controls.Add(lblFirstName);
            Name = "Form1";
            Text = "Persons";
            ((System.ComponentModel.ISupportInitialize)dgvPersons).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblFirstName;
        private TextBox tbFirstName;
        private Label lblLastName;
        private TextBox tbLastName;
        private Label lblAge;
        private TextBox tbAge;
        private Label lblEmail;
        private TextBox tbEmail;
        private Button btnAdd;
        private Button btnEdit;
        private Button btnSave;
        private Button btnCancel;
        private Button btnDelete;
        private DataGridView dgvPersons;
    }
}
