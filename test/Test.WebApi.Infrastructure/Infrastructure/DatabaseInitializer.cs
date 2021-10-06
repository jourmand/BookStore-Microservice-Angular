using System;
using System.Linq;
using WebApi.Core.Domain.BookAggregate;
using WebApi.Core.Domain.BookAggregate.ValueObjects;
using WebApi.Infrastructures.Data.Commons;

namespace Test.WebApi.Infrastructure.Infrastructure
{
    public class DatabaseInitializer
    {
        public static void Initialize(BookStoreDbContext context)
        {
            if (context.BookItems.Any())
            {
                return;
            }

            Seed(context);
        }

        private static void Seed(BookStoreDbContext context)
        {
            var bookItems = new[]
            {
                BookItem.Create(Guid.Parse("654b7573-9501-436a-ad36-94c5696ac28f"), new BookName("jQuery"), new BookText("OfnerOfnerOfnerOfnerOfnerOfner"), new BookPrice(11)),
                BookItem.Create(Guid.Parse("971316e1-4966-4426-b1ea-a36c9dde1066"), new BookName("Asp.Net"), new BookText("OfnerOfnerOfnerOfnOfnerer"), new BookPrice((decimal)45.3)),
                BookItem.Create(Guid.Parse("9f35b48d-cb87-4783-bfdb-21e36012930a"), new BookName("Java"), new BookText("OfnerOfnerOfnerOfnerOfnerOfner"), new BookPrice(85)),
            };

            context.BookItems.AddRange(bookItems);
            context.SaveChanges();
        }
    }
}
