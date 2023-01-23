using Microsoft.EntityFrameworkCore;

namespace EFLoadDatabaseApp
{
    public class Company
    {
        public int Id { set; get; }
        public string? Title { set; get; } = null!;
        public List<Employe> Employes { get; set; } = new List<Employe>();
    }
    public class Employe
    {
        public int Id { set; get; }
        public string? Name { set; get; } = null!;
        public DateTime BirthDate { set; get; }
        public int? CompanyId { set; get; } // свойство - внешний ключ
        public Company? Company { set; get; } // навигационное свойство
    }

    public class AppContext : DbContext
    {
        public DbSet<Employe> Employes { get; set; }
        public DbSet<Company> Companies { get; set; }
        public AppContext()
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CompaniesDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            using(AppContext context = new())
            {
                Console.WriteLine("--- Companies: -----");
                foreach (Company company in context.Companies)
                {
                    Console.WriteLine($"{company.Title}");
                    foreach (Employe employe in company.Employes)
                        Console.WriteLine($"\t{employe.Name}");
                }
                Console.WriteLine("\n--- Employes: -----");

                foreach (Employe employe in context.Employes)
                {
                    Console.WriteLine($"{employe.Name} {employe?.Company?.Title}");
                }
            }
        }
    }
}