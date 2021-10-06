using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Core.Domain.BookAggregate;
using WebApi.Core.Domain.BookAggregate.ValueObjects;

namespace WebApi.Infrastructures.Data.Configuration
{
    public class BookItemConfig : IEntityTypeConfiguration<BookItem>
    {
        public void Configure(EntityTypeBuilder<BookItem> builder)
        {
            builder.ToTable("BookItems");
            builder.HasKey(ci => ci.Id);
            builder.Property(ci => ci.Id)
                .ValueGeneratedNever();

            builder.Property(c => c.Name)
                .HasMaxLength(150)
                .HasConversion(c => c.Value, d => new BookName(d));

            builder.Property(c => c.Price)
                .HasConversion(c => c.Value, d => new BookPrice(d));
            
            builder.Property(c => c.Text)
                .HasMaxLength(1000)
                .HasConversion(c => c.Value, d => new BookText(d));

            builder.Metadata
                .FindNavigation(nameof(BookItem.Subscriptions))
                .SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasData(new List<BookItem>
            {
                BookItem.Create(new Guid("67e577d6-c1b7-4516-ab5f-885b3df1d22b"), new BookName("C#"),
                    new BookText("C# is a general-purpose, multi-paradigm programming language"),
                    BookPrice.Create((decimal) 14.2)),
                BookItem.Create(new Guid("49d8d8bb-a49b-4d1c-a6e6-1a3660fb9771"), new BookName("Asp.net"),
                    new BookText("ASP.NET is an open-source, server-side web-application framework"),
                    BookPrice.Create((decimal) 31.5)),
                BookItem.Create(new Guid("291dca11-cdad-4863-8d01-265ce768cea8"), new BookName("Angular"),
                    new BookText("Angular is a TypeScript-based free and open-source web application framework"),
                    BookPrice.Create((decimal) 23.9))
            });
        }
    }
}
