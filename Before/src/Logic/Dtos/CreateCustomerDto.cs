using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Dtos
{
    public class CreateCustomerDto
    {
        [Required]
        [MaxLength(100, ErrorMessage = "Name is too long")]
        public virtual string Name { get; set; }

        [Required]
        [RegularExpression(@"^(.+)@(.+)$", ErrorMessage = "Email is invalid")]
        public virtual string Email { get; set; }

    }
}
