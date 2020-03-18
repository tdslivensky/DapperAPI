using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContractDapper.Models
{
    public class Test
    {
        [Key]
        public int Number { get; set; }

        public IList<RFQ> RFQs { get; set; }
    }
}
