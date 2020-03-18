using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ContractDapper.Models
{
    public class RFQ
    {
        [Key]
        public int RFQID { get; set; }
        public String LegacyRFQNumber { get; set; }
        public DateTime RequestDate { get; set; }
        public int DistrictDivisionID { get; set; }
        public int Agency { get; set; }
        public int RFQValue { get; set; }
        public String RFQDescription { get; set; }
        public IList<Contract> Contracts { get; set; }
	}
}
