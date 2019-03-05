using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradeScales.Entities
{
    public class Vehicle : IEntityBase
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Make { get; set; }
        public string Registration { get; set; }
    }
}
