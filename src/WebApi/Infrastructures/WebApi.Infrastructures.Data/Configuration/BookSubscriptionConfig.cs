using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Core.Domain.BookAggregate;

namespace WebApi.Infrastructures.Data.Configuration
{
    public class BookSubscriptionConfig : IEntityTypeConfiguration<BookSubscription>
    {
        public void Configure(EntityTypeBuilder<BookSubscription> builder)
        {
            builder.ToTable("BookSubscriptions");
            builder.HasKey(ci => ci.Id);
            builder.Property(ci => ci.Id)
                .ValueGeneratedNever();

        }
    }
}
