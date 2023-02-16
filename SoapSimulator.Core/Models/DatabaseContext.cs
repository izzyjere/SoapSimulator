using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

namespace SoapSimulator.Core.Models;
public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    public virtual DbSet<RequestFormat> RequestFormatConfigurations { get; set; }
    public virtual DbSet<ResponseFormat> ResponseFormatConfigurations { get; set; }
}
