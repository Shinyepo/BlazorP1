using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorP1.Shared
{
    public class UserUnitResponse
    {
        public int UnitId { get; set; }
        public double HitPoints {  get; set; }
    }
}
