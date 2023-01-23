using Microsoft.EntityFrameworkCore;

namespace EFLoadDatabaseApp
{
    public class City
    {
        public int Id { set; get; }
        public string? Title { set; get; } = null!;
    }
    public class Country
    {
        public int Id { set; get; }
        public string? Title { set; get; } = null!;
        public int CapitalId { set; get; }
        public City? Capital { set; get; }
        public List<Company> Companies { get; set; } = new List<Company>();
    }
    public class Company
    {
        public int Id { set; get; }
        public string? Title { set; get; } = null!;
        public int CountryId { set; get; }
        public Country? Country { set; get; }
        public List<Employe> Employes { get; set; } = new List<Employe>();
    }

    public class Position
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
        public int? PositionId { set; get; } // свойство - внешний ключ
        public Position? Position { set; get; } // навигационное свойство

    }

    public class AppContext : DbContext
    {
        public DbSet<Employe> Employes { get; set; } = null!;
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<Country> Countries { get; set; } = null!;
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<Position> Positions { get; set; } = null!;
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
                /*
                City moscow = new() { Title = "Moscow" };
                context.Cities.Add(moscow);
                context.SaveChanges();

                Country russia = new() { Title = "Russia", Capital = moscow };
                context.Countries.Add(russia);
                context.SaveChanges();

                Company yandex = new() { Title = "Yandex", Country = russia };
                Company mail = new() { Title = "Mail Group", Country = russia };
                Company ozon = new() { Title = "Ozon", Country = russia };
                context.Companies.AddRange(new Company[] { yandex, mail, ozon });
                context.SaveChanges();

                Position manager = new() { Title = "Manager" };
                Position developer = new() { Title = "Developer" };
                context.Positions.AddRange(new Position[] { manager, developer });
                context.SaveChanges();

                Employe[] employes = new Employe[]
                {
                    new(){ Name = "Bob", BirthDate = new DateTime(2002, 2, 12), Position = manager, Company = yandex },
                    new(){ Name = "Tom", BirthDate = new DateTime(1998, 4, 22), Position = developer, Company = yandex },
                    new(){ Name = "Sam", BirthDate = new DateTime(2001, 11, 5), Position = manager, Company = mail },
                    new(){ Name = "Jim", BirthDate = new DateTime(1988, 6, 9), Position = developer, Company = mail},
                    new(){ Name = "Leo", BirthDate = new DateTime(1996, 1, 21), Position = manager, Company = ozon },
                    new(){ Name = "Tim", BirthDate = new DateTime(2000, 4, 30), Position = developer, Company = ozon },
                };     
                context.Employes.AddRange(employes);
                context.SaveChanges();
                */

                //var employes = context.Employes
                //                      .Include(e => e.Company)
                //                      .ToList();
                //var employes = context.Employes;

                //var companies = context.Companies
                //                       .Include(c => c.Country)
                //                       .Include(c => c.Employes)
                //                       .ToList();

                //Console.WriteLine("--- Companies: -----");
                //foreach (Company company in companies)
                //{
                //    Console.WriteLine($"{company.Title} {company?.Country?.Title}");
                //    foreach (Employe employe in company.Employes)
                //        Console.WriteLine($"\t{employe.Name}");
                //}


                var employes = context.Employes
                                      .Include(empl => empl.Company)
                                        .ThenInclude(comp => comp.Country)
                                            .ThenInclude(cntr => cntr.Capital)
                                      .Include(empl => empl.Position)
                                      .ToList();

                Console.WriteLine("\n--- Employes: -----");
                foreach (Employe employe in employes)
                {
                    Console.WriteLine($"{employe.Name} {employe?.Position?.Title}");
                    Console.WriteLine($"{employe?.Company?.Title} {employe?.Company?.Country?.Title} {employe?.Company?.Country?.Capital?.Title}");
                    Console.WriteLine();

                        
                }
            }
        }
    }
}