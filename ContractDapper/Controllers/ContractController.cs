using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ContractDapper.Interfaces;

namespace ContractDapper.Controllers
{
    [ApiController]
    public class ContractController : ControllerBase
    {
        private readonly IContract _contract;

        public ContractController(IContract contract)
        {
            _contract = contract;
        }

        [HttpGet("api/contracts/all")]
        public async Task<IActionResult> GetContracts()
        {
            return Ok(await _contract.GetAllContracts());
        }

        [HttpGet("api/contracts/{id}")]
        public async Task<IActionResult> GetContractId(int id)
        {
            var contract = await _contract.GetContractById(id);

            if (contract != null)
            {
                return Ok(contract);
            }

            return NotFound(new { Message = $"Movie with id {id} is not available." });
        }

        [HttpGet("api/RFQ/Contracts/{id}")]
        public async Task<IActionResult> GetRFQsId(int id)
        {
            var rfq = await _contract.GetRFQById(id);

            if (rfq != null)
            {
                return Ok(rfq);
            }

            return NotFound(new { Message = $"Movie with id {id} is not available." });
        }

        [HttpGet("api/RFQ/Contracts")]
        public async Task<IActionResult> GetRFQs()
        {
            return Ok(await _contract.GetAllRFQs());
        }

        [HttpGet("api/n/all")]
        public async Task<IActionResult> GetTest()
        {
            return Ok(await _contract.GetTestN());
        }

        [HttpGet("api/n/single")]
        public async Task<IActionResult> GetTestSingle()
        {
            return Ok(await _contract.GetAllRFQTest100K());
        }
    }
}
