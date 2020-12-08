using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TruYumClient.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public bool Active { get; set; }
        public DateTime DateOflaunch { get; set; }
        public string category { get; set; }
        public bool FreeDelivery { get; set; }
    }
}
