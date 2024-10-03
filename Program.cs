using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main(string[] args)
    {
        DatabaseService db = new DatabaseService();

        db.AddGroupInDatabase(new Group { Name = "Math" });
        db.AddGroupInDatabase(new Group { Name = "Music" });
        db.AddGroupInDatabase(new Group { Name = "Art" });

        db.AddStudentInDatabase(new Student { FullName = "John Doe", Age = 18 });
        db.AddStudentInDatabase(new Student { FullName = "Alice Mace", Age = 18 });
        db.AddStudentInDatabase(new Student { FullName = "Josh Marlie", Age = 19 });

        db.AddStudentInGroup(1, 1);
        db.AddStudentInGroup(3, 2);

        db.DeleteStudentById(2);

        db.EditStudentById(new Student { Id = 1, FullName = "Edd Johnson", Age = 20 });

        db.RemoveStudentFromGroup(1, 1);
    }
}

class Student
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public int Age { get; set; }
    public List<Group> Groups { get; set; } = new();
}

class Group
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<Student>? Students { get; set; }
}

class ApplicationContext : DbContext
{
    public DbSet<Student> Students { get; set; }
    public DbSet<Group> Groups { get; set; }

    public ApplicationContext() => Database.EnsureCreated();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=EFCoreDB;Trusted_Connection=True;");
    }
}

class DatabaseService
{
    public void AddGroupInDatabase(Group group)
    {
        using (var db = new ApplicationContext())
        {
            db.Groups.Add(group);
            db.SaveChanges();
        }
    }
    public void AddStudentInDatabase(Student student)
    {
        using (var db = new ApplicationContext())
        {
            db.Students.Add(student);
            db.SaveChanges();
        }
    }
    public void AddStudentInGroup(int studentId, int groupId)
    {
        using (var db = new ApplicationContext())
        {
            var student = db.Students.Include(s => s.Groups).FirstOrDefault(s => s.Id == studentId);
            var group = db.Groups.Find(groupId);

            if (student != null && group != null)
            {
                if (!student.Groups.Any(g => g.Id == groupId))
                {
                    student.Groups.Add(group);

                    db.SaveChanges();
                }
            }
        }
    }
    public void RemoveStudentFromGroup(int studentId, int groupId)
    {
        using (var db = new ApplicationContext())
        {
            var student = db.Students.Include(s => s.Groups).FirstOrDefault(s => s.Id == studentId);
            var group = db.Groups.Find(groupId);

            if (student != null && group != null)
            {
                student.Groups.Remove(group);

                db.SaveChanges();
            }
        }
    }
    public void AddStudentWithGroups(Student student, List<int> groupIds)
    {
        using (var db = new ApplicationContext())
        {
            var groups = db.Groups.Where(g => groupIds.Contains(g.Id)).ToList();
            student.Groups = groups;

            db.Students.Add(student);

            db.SaveChanges();
        }
    }
    public List<Group> GetGroupsByStudent(int studentId)
    {
        using (var db = new ApplicationContext())
        {
            var student = db.Students.Include(s => s.Groups).FirstOrDefault(s => s.Id == studentId);
            return student?.Groups.ToList();
        }
    }
    public List<Student> GetStudentsByGroup(int groupId)
    {
        using (var db = new ApplicationContext())
        {
            var group = db.Groups.Include(g => g.Students).FirstOrDefault(g => g.Id == groupId);
            return group?.Students.ToList();
        }
    }
    public void DeleteStudentById(int studentId)
    {
        using (var db = new ApplicationContext())
        {
            var student = db.Students.FirstOrDefault(s => s.Id == studentId);
            if (student != null)
            {
                db.Students.Remove(student);
                db.SaveChanges();
            }
        }
    }
    public void EditStudentById(Student editStudent)
    {
        using (var db = new ApplicationContext())
        {
            var student = db.Students.Find(editStudent.Id);
            if (student != null)
            {
                student.FullName = editStudent.FullName;
                student.Age = editStudent.Age;
            }
            db.SaveChanges();
        }
    }
}