using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Class_Lib.Backend.Serialization.DTO
{
    public class UserDTO
    {
        public uint ID { get; set; }
        public string Username { get; set; }
        public RoleDTO Role { get; set; }
        public EmployeeDTO Employee { get; set; }
    }
}