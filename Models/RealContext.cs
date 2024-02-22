
using CP.Models;
using Microsoft.EntityFrameworkCore;

namespace CP;

public partial class RealContext : DbContext
{
    public RealContext()
    {
    }

    public RealContext(DbContextOptions<RealContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BathroomType> BathroomTypes { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Deal> Deals { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<ObjectType> ObjectTypes { get; set; }

    public virtual DbSet<Passport> Passports { get; set; }

    public virtual DbSet<Photo> Photos { get; set; }

    public virtual DbSet<Proof> Proofs { get; set; }

    public virtual DbSet<Realtor> Realtors { get; set; }

    public virtual DbSet<Realty> Realties { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

        => optionsBuilder.UseSqlite("Filename=real.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<BathroomType>(entity =>
        {
            entity.ToTable("BathroomType");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Type).HasColumnName("type");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.ToTable("Client");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Firstname).HasColumnName("firstname");
            entity.Property(e => e.Lastname).HasColumnName("lastname");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Numberphone).HasColumnName("numberphone");
            entity.Property(e => e.PasswordFk).HasColumnName("passwordFK");

            entity.HasOne(d => d.PasswordFkNavigation).WithMany(p => p.Clients).HasForeignKey(d => d.PasswordFk);
        });

        modelBuilder.Entity<Deal>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Commission)
                .HasColumnType("agencies REAL")
                .HasColumnName("commission");
            entity.Property(e => e.Dealtype)
                .HasDefaultValueSql("'Продажа'")
                .HasColumnName("dealtype");
            entity.Property(e => e.OfferCode).HasColumnName("offerCode");
            entity.Property(e => e.Realtor).HasColumnName("realtor");
            entity.Property(e => e.TransactionDate).HasColumnName("transactionDate");

            entity.HasOne(d => d.BuyerNavigation).WithMany(p => p.Deals).HasForeignKey(d => d.Buyer);

            entity.HasOne(d => d.OfferCodeNavigation).WithMany(p => p.Deals).HasForeignKey(d => d.OfferCode);

            entity.HasOne(d => d.RealtorNavigation).WithMany(p => p.Deals).HasForeignKey(d => d.Realtor);
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<ObjectType>(entity =>
        {
            entity.ToTable("ObjectType");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Passport>(entity =>
        {
            entity.ToTable("passport");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Dateof).HasColumnName("dateof");
            entity.Property(e => e.Isby).HasColumnName("isby");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.Serial).HasColumnName("serial");
        });

        modelBuilder.Entity<Photo>(entity =>
        {
            entity.ToTable("Photo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Image1).HasColumnName("image1");
            entity.Property(e => e.Image10).HasColumnName("image10");
            entity.Property(e => e.Image11).HasColumnName("image11");
            entity.Property(e => e.Image12).HasColumnName("image12");
            entity.Property(e => e.Image2).HasColumnName("image2");
            entity.Property(e => e.Image3).HasColumnName("image3");
            entity.Property(e => e.Image4).HasColumnName("image4");
            entity.Property(e => e.Image5).HasColumnName("image5");
            entity.Property(e => e.Image6).HasColumnName("image6");
            entity.Property(e => e.Image7).HasColumnName("image7");
            entity.Property(e => e.Image8).HasColumnName("image8");
            entity.Property(e => e.Image9).HasColumnName("image9");
        });

        modelBuilder.Entity<Proof>(entity =>
        {
            entity.ToTable("Proof");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Dateof).HasColumnName("dateof");
            entity.Property(e => e.Number).HasColumnName("number");
            entity.Property(e => e.Registr).HasColumnName("registr");
            entity.Property(e => e.Serial).HasColumnName("serial");
        });

        modelBuilder.Entity<Realtor>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Firstname).HasColumnName("firstname");
            entity.Property(e => e.Lastname).HasColumnName("lastname");
            entity.Property(e => e.Login).HasColumnName("login");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Numberphone).HasColumnName("numberphone");
            entity.Property(e => e.Password).HasColumnName("password");
        });

        modelBuilder.Entity<Realty>(entity =>
        {
            entity.ToTable("Realty");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Actual)
                .HasDefaultValueSql("1")
                .HasColumnName("actual");
            entity.Property(e => e.Adress).HasColumnName("adress");
            entity.Property(e => e.NameReal).HasColumnName("nameReal");
            entity.Property(e => e.Area).HasColumnName("area");
            entity.Property(e => e.BalconyGlazing).HasColumnName("balconyGlazing");
            entity.Property(e => e.BathroomId).HasColumnName("bathroomId");
            entity.Property(e => e.Certificate).HasColumnName("certificate");
            entity.Property(e => e.Description)
                .HasDefaultValueSql("''")
                .HasColumnName("description");
            entity.Property(e => e.Finishing).HasColumnName("finishing");
            entity.Property(e => e.Floor).HasColumnName("floor");
            entity.Property(e => e.IdPhoto).HasColumnName("idPhoto");
            entity.Property(e => e.Material).HasColumnName("material");
            entity.Property(e => e.NumberOfRooms)
                .HasDefaultValueSql("1")
                .HasColumnName("numberOfRooms");
            entity.Property(e => e.NumberOfStoreys)
                .HasDefaultValueSql("1")
                .HasColumnName("numberOfStoreys");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.ProOrAre)
                .HasDefaultValueSql("'Продажа'")
                .HasColumnName("proOrAre");
            entity.Property(e => e.Salesman).HasColumnName("salesman");
            entity.Property(e => e.Square).HasColumnName("square");
            entity.Property(e => e.TypeObject).HasColumnName("typeObject");
            entity.Property(e => e.YearOfConstruction).HasColumnName("yearOfConstruction");

            entity.HasOne(d => d.AreaNavigation).WithMany(p => p.Realties).HasForeignKey(d => d.Area);

            entity.HasOne(d => d.Bathroom).WithMany(p => p.Realties).HasForeignKey(d => d.BathroomId);

            entity.HasOne(d => d.CertificateNavigation).WithMany(p => p.Realties).HasForeignKey(d => d.Certificate);

            entity.HasOne(d => d.IdPhotoNavigation).WithMany(p => p.Realties).HasForeignKey(d => d.IdPhoto);

            entity.HasOne(d => d.TypeObjectNavigation).WithMany(p => p.Realties).HasForeignKey(d => d.TypeObject);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
