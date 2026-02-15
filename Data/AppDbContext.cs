using Microsoft.EntityFrameworkCore;
using LibrariesWebApp.Models;

namespace LibrariesWebApp.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<Library> Libraries { get; set; }

    public virtual DbSet<Loan> Loans { get; set; }

    public virtual DbSet<Publisher> Publishers { get; set; }

    public virtual DbSet<Reader> Readers { get; set; }

    public virtual DbSet<Subject> Subjects { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=libraries;Username=libraryuser;Password=library");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("ru_RU.UTF-8");

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => new { e.LibraryId, e.BookCode }).HasName("books_pkey");

            entity.ToTable("books", tb => tb.HasComment("Экземпляры книг в библиотеках"));

            entity.Property(e => e.LibraryId)
                .HasComment("Код библиотеки (часть составного ключа)")
                .HasColumnName("library_id");
            entity.Property(e => e.BookCode)
                .HasComment("Внутренний код книги в библиотеке")
                .HasColumnName("book_code");
            entity.Property(e => e.Author)
                .HasMaxLength(100)
                .HasComment("Автор")
                .HasColumnName("author");
            entity.Property(e => e.PublisherId)
                .HasComment("Издательство")
                .HasColumnName("publisher_id");
            entity.Property(e => e.Quantity)
                .HasDefaultValue(0)
                .HasComment("Количество экземпляров в данной библиотеке")
                .HasColumnName("quantity");
            entity.Property(e => e.SubjectId)
                .HasComment("Тематика книги")
                .HasColumnName("subject_id");
            entity.Property(e => e.Title)
                .HasMaxLength(200)
                .HasComment("Название")
                .HasColumnName("title");
            entity.Property(e => e.Year)
                .HasComment("Год издания")
                .HasColumnName("year");

            entity.HasOne(d => d.Library).WithMany(p => p.Books)
                .HasForeignKey(d => d.LibraryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("books_library_id_fkey");

            entity.HasOne(d => d.Publisher).WithMany(p => p.Books)
                .HasForeignKey(d => d.PublisherId)
                .HasConstraintName("books_publisher_id_fkey");

            entity.HasOne(d => d.Subject).WithMany(p => p.Books)
                .HasForeignKey(d => d.SubjectId)
                .HasConstraintName("books_subject_id_fkey");
        });

        modelBuilder.Entity<Library>(entity =>
        {
            entity.HasKey(e => e.LibraryId).HasName("libraries_pkey");

            entity.ToTable("libraries", tb => tb.HasComment("Филиалы библиотек"));

            entity.HasIndex(e => e.Name, "libraries_name_key").IsUnique();

            entity.Property(e => e.LibraryId)
                .ValueGeneratedNever()
                .HasComment("Уникальный идентификатор библиотеки")
                .HasColumnName("library_id");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasComment("Адрес")
                .HasColumnName("address");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasComment("Название библиотеки")
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasComment("Контактный телефон")
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(e => e.LoanId).HasName("loans_pkey");

            entity.ToTable("loans", tb => tb.HasComment("Выдачи книг читателям"));

            entity.Property(e => e.LoanId)
                .ValueGeneratedNever()
                .HasComment("Уникальный номер записи о выдаче")
                .HasColumnName("loan_id");
            entity.Property(e => e.Advance)
                .HasPrecision(10, 2)
                .HasDefaultValue(0.00m)
                .HasComment("Аванс (залог)")
                .HasColumnName("advance");
            entity.Property(e => e.BookCode).HasColumnName("book_code");
            entity.Property(e => e.IssueDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasComment("Дата выдачи")
                .HasColumnName("issue_date");
            entity.Property(e => e.LibraryId).HasColumnName("library_id");
            entity.Property(e => e.ReaderId).HasColumnName("reader_id");
            entity.Property(e => e.ReturnDate)
                .HasComment("Дата возврата (NULL, если ещё не возвращена)")
                .HasColumnName("return_date");

            entity.HasOne(d => d.Reader).WithMany(p => p.Loans)
                .HasForeignKey(d => d.ReaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("loans_reader_id_fkey");

            entity.HasOne(d => d.Book).WithMany(p => p.Loans)
                .HasForeignKey(d => new { d.LibraryId, d.BookCode })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("loans_library_id_book_code_fkey");
        });

        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.PublisherId).HasName("publishers_pkey");

            entity.ToTable("publishers", tb => tb.HasComment("Издательства"));

            entity.HasIndex(e => e.Name, "publishers_name_key").IsUnique();

            entity.Property(e => e.PublisherId)
                .ValueGeneratedNever()
                .HasColumnName("publisher_id");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Reader>(entity =>
        {
            entity.HasKey(e => e.ReaderId).HasName("readers_pkey");

            entity.ToTable("readers", tb => tb.HasComment("Читатели"));

            entity.HasIndex(e => e.Phone, "readers_phone_key").IsUnique();

            entity.Property(e => e.ReaderId)
                .ValueGeneratedNever()
                .HasColumnName("reader_id");
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .HasColumnName("address");
            entity.Property(e => e.FullName)
                .HasMaxLength(150)
                .HasComment("ФИО полностью")
                .HasColumnName("full_name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("subjects_pkey");

            entity.ToTable("subjects", tb => tb.HasComment("Тематические рубрики книг"));

            entity.HasIndex(e => e.Name, "subjects_name_key").IsUnique();

            entity.Property(e => e.SubjectId)
                .ValueGeneratedNever()
                .HasColumnName("subject_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
