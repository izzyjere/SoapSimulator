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
    public virtual DbSet<RequestFormat> RequestFormats { get; set; }
    public virtual DbSet<ResponseFormat> ResponseFormats { get; set; }
    public virtual DbSet<SystemConfiguration> SystemConfigurations { get; set; }
    public virtual DbSet<SoapAction> SoapActions { get; set;}
    public virtual DbSet<ActionParameter> ActionParameters { get; set; }
}
