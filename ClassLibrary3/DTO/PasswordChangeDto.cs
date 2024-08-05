using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastucture.DTO
{
    public class PasswordChangeDto
    {
        [Required]
        [DataType(DataType.Password,ErrorMessage = "Password must be at least 8 characters, at least one digit, at least one lowercase, at least one upper case and at least one special character needed.")]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password, ErrorMessage = "Password must be at least 8 characters, at least one digit, at least one lowercase, at least one upper case and at least one special character needed.")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password, ErrorMessage = "Password must be at least 8 characters, at least one digit, at least one lowercase, at least one upper case and at least one special character needed.")]
        public string ConfirmPassword { get; set; }

    }
}
