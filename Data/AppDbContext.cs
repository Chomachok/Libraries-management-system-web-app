using Microsoft.EntityFrameworkCore;
using LibrariesWebApp.Models;

namespace LibrariesWebApp.Data;

/// <summary>
/// Контекст базы данных для приложения "Библиотеки".
/// Управляет подключением к PostgreSQL и конфигурацией сущностей.
/// </summary>
public partial class AppDbContext : DbContext
{
    /// <summary>
    /// Конструктор, принимающий параметры контекста.
    /// Используется при внедрении зависимостей (DI) в ASP.NET Core.
    /// </summary>
    /// <param name="options">Параметры контекста, содержащие строку подключения и провайдера.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Экземпляры книг в библиотеках (таблица books).
    /// </summary>
    public virtual DbSet<Book> Books { get; set; }

    /// <summary>
    /// Филиалы библиотек (таблица libraries).
    /// </summary>
    public virtual DbSet<Library> Libraries { get; set; }

    /// <summary>
    /// Выдачи книг читателям (таблица loans).
    /// </summary>
    public virtual DbSet<Loan> Loans { get; set; }

    /// <summary>
    /// Издательства (таблица publishers).
    /// </summary>
    public virtual DbSet<Publisher> Publishers { get; set; }

    /// <summary>
    /// Читатели (таблица readers).
    /// </summary>
    public virtual DbSet<Reader> Readers { get; set; }

    /// <summary>
    /// Тематические рубрики книг (таблица subjects).
    /// </summary>
    public virtual DbSet<Subject> Subjects { get; set; }

    /// <summary>
    /// Настраивает модель сущностей (схему базы данных, ограничения, связи, комментарии).
    /// Вызывается один раз при первом обращении к контексту для построения модели.
    /// </summary>
    /// <param name="modelBuilder">Построитель модели.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Устанавливаем локаль для сортировки строк (используется при создании миграций)
        modelBuilder.UseCollation("ru_RU.UTF-8");

        // Настройка сущности Book (книги)
        modelBuilder.Entity<Book>(entity =>
        {
            // Составной первичный ключ: library_id + book_code
            entity.HasKey(e => new { e.LibraryId, e.BookCode }).HasName("books_pkey");

            entity.ToTable("books", tb => tb.HasComment("Экземпляры книг в библиотеках"));

            // Свойства (колонки)
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

            // Связи
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

        // Настройка сущности Library (библиотеки/филиалы)
        modelBuilder.Entity<Library>(entity =>
        {
            entity.HasKey(e => e.LibraryId).HasName("libraries_pkey");

            entity.ToTable("libraries", tb => tb.HasComment("Филиалы библиотек"));

            // Уникальный индекс на названии
            entity.HasIndex(e => e.Name, "libraries_name_key");

            entity.Property(e => e.LibraryId)
                .ValueGeneratedOnAdd()
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

        // Настройка сущности Loan (выдачи)
        modelBuilder.Entity<Loan>(entity =>
        {
            entity.HasKey(e => e.LoanId).HasName("loans_pkey");

            entity.ToTable("loans", tb => tb.HasComment("Выдачи книг читателям"));

            entity.Property(e => e.LoanId)
                .ValueGeneratedOnAdd()
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

            // Связи
            entity.HasOne(d => d.Reader).WithMany(p => p.Loans)
                .HasForeignKey(d => d.ReaderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("loans_reader_id_fkey");

            entity.HasOne(d => d.Book).WithMany(p => p.Loans)
                .HasForeignKey(d => new { d.LibraryId, d.BookCode })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("loans_library_id_book_code_fkey");
        });

        // Настройка сущности Publisher (издательства)
        modelBuilder.Entity<Publisher>(entity =>
        {
            entity.HasKey(e => e.PublisherId).HasName("publishers_pkey");

            entity.ToTable("publishers", tb => tb.HasComment("Издательства"));

            entity.HasIndex(e => e.Name, "publishers_name_key").IsUnique();

            entity.Property(e => e.PublisherId)
                .ValueGeneratedOnAdd()
                .HasColumnName("publisher_id");
            entity.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        // Настройка сущности Reader (читатели)
        modelBuilder.Entity<Reader>(entity =>
        {
            entity.HasKey(e => e.ReaderId).HasName("readers_pkey");

            entity.ToTable("readers", tb => tb.HasComment("Читатели"));

            entity.HasIndex(e => e.Phone, "readers_phone_key").IsUnique();

            entity.Property(e => e.ReaderId)
                .ValueGeneratedOnAdd()
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

        // Настройка сущности Subject (тематические рубрики)
        modelBuilder.Entity<Subject>(entity =>
        {
            entity.HasKey(e => e.SubjectId).HasName("subjects_pkey");

            entity.ToTable("subjects", tb => tb.HasComment("Тематические рубрики книг"));

            entity.HasIndex(e => e.Name, "subjects_name_key").IsUnique();

            entity.Property(e => e.SubjectId)
                .ValueGeneratedOnAdd()
                .HasColumnName("subject_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        // Вызов частичного метода для дополнительной конфигурации в разделяемых классах
        OnModelCreatingPartial(modelBuilder);
    }

    /// <summary>
    /// Частичный метод, который может быть реализован в другом файле для расширения конфигурации модели.
    /// </summary>
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}