using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Study
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
