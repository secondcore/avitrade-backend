using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vimba.AviTrade.Models
{
    public class Airport
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string City { get; set; }
        //.. other attributes will be added later

        public Country Country { get; set; }
    }
}
