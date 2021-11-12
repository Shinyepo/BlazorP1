using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorP1.Shared
{
    public class UserUnit
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }

        public int UnitId { get; set; }
        public Unit Unit { get; set; }
        public int HitPoints { get; set; }
    }
}
