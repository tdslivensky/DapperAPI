using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContractDapper.Models
{
    public class Contract
    {  
        [Key]
        public int ContractId { get; set; }

        public int TotalValue { get; set; }

        public DateTime ContractExecutedDate { get; set; }

        public DateTime ContractTerminatedDate { get; set; }

        public int ServiceLineId { get; set; }

        public String ContractDescription { get; set; }

        public int Agency { get; set; }

        public Boolean Executed { get; set; }

        public int DistrictDivisionID { get; set; }

        public String ERPContractNumber { get; set; }

        public int RFQID { get; set; }

        public int TeamID { get; set; }
    }
}
