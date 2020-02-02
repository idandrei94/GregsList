using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gregslist.Models
{
    public class User : IdentityUser
    {
        public int score { get; set; } = 0;
    }
}
