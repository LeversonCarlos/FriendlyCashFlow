using Microsoft.EntityFrameworkCore;
namespace Lewio.CashFlow.Common;

public partial class DataContext : DbContext
{

   public DataContext(DbContextOptions<DataContext> options) : base(options) { }

}
