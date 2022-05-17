using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyMaterialShare.Data
{
    public class LoginDTO
    {
        public String UserName { get; set; } = String.Empty;

        public String Password { get; set; } = String.Empty;
    }
}
