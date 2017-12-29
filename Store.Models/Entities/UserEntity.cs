

using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Store.Models.Entities
{
    public class UserEntity : IdentityUser
    {
        public virtual Customer Customer { get; set; }
    }
   
}


