using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace Identity
{
    public class myIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public myIdentityDbContext()
        {
        }

        public myIdentityDbContext(DbContextOptions<myIdentityDbContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
