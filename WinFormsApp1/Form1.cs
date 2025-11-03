using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
//using System.Data.Entity;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private BindingList<Person> people = new BindingList<Person>();
        private Person? editingPerson = null;
        private bool isEditing = false;

        // TODO: update this connection string to point to your SQL Server and database
        private string connectionString = @"Data Source=DESKTOP-LB9BVSJ\SQLEXPRESS;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Command Timeout=0;Database=Persons";

        public Form1()
        {
            InitializeComponent();
            dgvPersons.DataSource = people;

            // initial button states
            btnEdit.Enabled = false;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;

            // load from database
            LoadPersonsFromDatabase();
        }

        private void LoadPersonsFromDatabase()
        {
            try
            {
                using var db = new AppDbContext(connectionString);
                var list = db.Persons.AsNoTracking().ToList();
                people.Clear();
                foreach (var e in list)
                {
                    people.Add(new Person
                    {
                        Id = e.Id,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        Age = e.Age,
                        Email = e.Email ?? string.Empty
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load persons from database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (isEditing)
            {
                MessageBox.Show("Finish editing before adding a new person.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // validate
            var first = tbFirstName.Text.Trim();
            var last = tbLastName.Text.Trim();
            var email = tbEmail.Text.Trim();
            if (string.IsNullOrEmpty(first) || string.IsNullOrEmpty(last))
            {
                MessageBox.Show("First and last name are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(tbAge.Text.Trim(), out var age) || age < 0)
            {
                MessageBox.Show("Age must be a valid non-negative integer.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var p = new Person
            {
                FirstName = first,
                LastName = last,
                Age = age,
                Email = string.IsNullOrEmpty(email) ? string.Empty : email
            };

            // attempt to save to database using EF Core
            try
            {
                SavePersonToDatabase(p);
                people.Add(p);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save person to database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // clear inputs
            ClearInputs();
            tbFirstName.Focus();
        }

        private void SavePersonToDatabase(Person p)
        {
            // Map the UI Person to the EF entity and save via DbContext
            using var db = new AppDbContext(connectionString);

            var entity = new PersonEntity
            {
                FirstName = p.FirstName,
                LastName = p.LastName,
                Age = p.Age,
                Email = string.IsNullOrEmpty(p.Email) ? null : p.Email
            };

            db.Persons.Add(entity);
            db.SaveChanges();

            // set the assigned Id back to the UI model
            p.Id = entity.Id;
        }

        private void UpdatePersonInDatabase(Person p)
        {
            using var db = new AppDbContext(connectionString);
            var entity = db.Persons.FirstOrDefault(x => x.Id == p.Id);
            if (entity == null) throw new InvalidOperationException("Person not found in database.");

            entity.FirstName = p.FirstName;
            entity.LastName = p.LastName;
            entity.Age = p.Age;
            entity.Email = string.IsNullOrEmpty(p.Email) ? null : p.Email;

            db.SaveChanges();
        }

        private void DeletePersonFromDatabase(Person p)
        {
            using var db = new AppDbContext(connectionString);
            var entity = db.Persons.FirstOrDefault(x => x.Id == p.Id);
            if (entity == null) throw new InvalidOperationException("Person not found in database.");
            db.Persons.Remove(entity);
            db.SaveChanges();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvPersons.CurrentRow == null) return;
            var selected = dgvPersons.CurrentRow.DataBoundItem as Person;
            if (selected == null) return;

            editingPerson = selected;
            isEditing = true;
            PopulateInputsFromPerson(editingPerson);
            SetEditingState(true);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!isEditing || editingPerson == null) return;

            // validate
            var first = tbFirstName.Text.Trim();
            var last = tbLastName.Text.Trim();
            var email = tbEmail.Text.Trim();
            if (string.IsNullOrEmpty(first) || string.IsNullOrEmpty(last))
            {
                MessageBox.Show("First and last name are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(tbAge.Text.Trim(), out var age) || age < 0)
            {
                MessageBox.Show("Age must be a valid non-negative integer.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // apply changes to the in-memory model
            editingPerson.FirstName = first;
            editingPerson.LastName = last;
            editingPerson.Age = age;
            editingPerson.Email = string.IsNullOrEmpty(email) ? string.Empty : email;

            try
            {
                UpdatePersonInDatabase(editingPerson);
                // refresh grid
                var bs = (BindingList<Person>)dgvPersons.DataSource;
                bs.ResetBindings();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update person in database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // end editing
            editingPerson = null;
            isEditing = false;
            SetEditingState(false);
            ClearInputs();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            editingPerson = null;
            isEditing = false;
            SetEditingState(false);
            ClearInputs();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPersons.CurrentRow == null) return;
            var selected = dgvPersons.CurrentRow.DataBoundItem as Person;
            if (selected == null) return;

            var confirm = MessageBox.Show($"Delete {selected.FirstName} {selected.LastName}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                if (selected.Id != 0)
                {
                    DeletePersonFromDatabase(selected);
                }

                people.Remove(selected);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete person: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvPersons_SelectionChanged(object? sender, EventArgs e)
        {
            if (isEditing) return; // don't change inputs while editing
            if (dgvPersons.CurrentRow == null)
            {
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                ClearInputs();
                return;
            }

            var selected = dgvPersons.CurrentRow.DataBoundItem as Person;
            if (selected == null)
            {
                btnEdit.Enabled = false;
                btnDelete.Enabled = false;
                ClearInputs();
                return;
            }

            btnEdit.Enabled = true;
            btnDelete.Enabled = true;
            PopulateInputsFromPerson(selected);
        }

        private void PopulateInputsFromPerson(Person p)
        {
            tbFirstName.Text = p.FirstName;
            tbLastName.Text = p.LastName;
            tbAge.Text = p.Age.ToString();
            tbEmail.Text = p.Email;
        }

        private void ClearInputs()
        {
            tbFirstName.Text = string.Empty;
            tbLastName.Text = string.Empty;
            tbAge.Text = string.Empty;
            tbEmail.Text = string.Empty;
        }

        private void SetEditingState(bool editing)
        {
            btnAdd.Enabled = !editing;
            btnEdit.Enabled = !editing && dgvPersons.CurrentRow != null;
            btnDelete.Enabled = !editing && dgvPersons.CurrentRow != null;
            btnSave.Enabled = editing;
            btnCancel.Enabled = editing;
        }
    }

    // EF Core entity mapped to dbo.Person
    public class PersonEntity
    {
        public int Id { get; set; } // assumes the table has an identity PK column named Id
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int Age { get; set; }
        public string? Email { get; set; }
    }

    // Simple DbContext that uses the provided connection string
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString;

        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public DbSet<PersonEntity> Persons { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonEntity>(entity =>
            {
                entity.ToTable("Person", "dbo");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();
                entity.Property(e => e.FirstName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.LastName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Age).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(255).IsRequired(false);
            });
        }
    }
}
