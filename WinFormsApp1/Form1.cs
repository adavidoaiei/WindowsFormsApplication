using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
//using System.Data.Entity;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private BindingList<Person> people = new BindingList<Person>();

        // TODO: update this connection string to point to your SQL Server and database
        private string connectionString = @"Data Source=DESKTOP-LB9BVSJ\SQLEXPRESS;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Command Timeout=0;Database=Persons";

        public Form1()
        {
            InitializeComponent();
            dgvPersons.DataSource = people;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
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
                Email = string.IsNullOrEmpty(email) ? null : email
            };

            people.Add(p);

            // attempt to save to database using EF Core
            try
            {
                SavePersonToDatabase(p);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save person to database: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // clear inputs
            tbFirstName.Text = "";
            tbLastName.Text = "";
            tbAge.Text = "";
            tbEmail.Text = "";

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
                Email = p.Email
            };

            db.Persons.Add(entity);
            db.SaveChanges();
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
