using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthPortFront.Models
{
    public class Item
    {
        public int id {get; set;}
        public string? firstName {get; set;}
        public string? lastName {get; set;}
        public string? claim {get; set;}
        public string? phoneNumber {get; set;}
        public string? email {get; set;}
        public string? claim_details {get; set;}
        public int age {get; set;}
        public float priceusd {get; set;}
        public float course {get; set;}
        public float priceuah {get; set;}
    }
}