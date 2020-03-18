using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ContractDapper.Models;
using Dapper;
using System.Data;

namespace ContractDapper.Interfaces
{
    public interface IContract
    {
        Task<IEnumerable<Contract>> GetAllContracts();
        Task<Contract> GetContractById(int id);
        Task<IEnumerable<RFQ>> GetAllRFQs();
        Task<IEnumerable<RFQ>> GetAllRFQTest100K();
        Task<IEnumerable<Test>> GetTestN();
        Task<RFQ> GetRFQById(int id); 
    }

    public class ContractRepo : IContract
    {
        private readonly ConnectionString connectionString;

        public ContractRepo(ConnectionString connection)
        {
            connectionString = connection;
        }

        public async Task<IEnumerable<Contract>> GetAllContracts()
        {
            const string query = "SP_CONTRACTALL_GET";

            using (var conn = new SqlConnection(connectionString.Value))
            {
                var result = await conn.QueryAsync<Contract>(query, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<IEnumerable<RFQ>> GetAllRFQTest100K()
        {
            const string query = "100KtestSingleOBJ";

            using (var conn = new SqlConnection(connectionString.Value))
            {
                var result = await conn.QueryAsync<RFQ>(query, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<Contract> GetContractById(int id)
        {
            const string query = "SP_CONTRACT_GET_ID";

            using (var conn = new SqlConnection(connectionString.Value))
            {
                var result = await conn.QueryFirstOrDefaultAsync<Contract>(query, new { ContractId = id }, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<RFQ> GetRFQById(int id)
        {
            const string query = @"SELECT *  
                            FROM RFQ r  
                            INNER JOIN CONTRACT C  
                            ON r.RFQID = c.RFQID
                            WHERE R.RFQID = @id";

            using (var conn = new SqlConnection(connectionString.Value))
            {
                var rfqDictionary = new Dictionary<int, RFQ>();

                var result = await conn.QueryAsync<RFQ, Contract, RFQ>(
                    query,
                    (rfq, contr) =>
                    {
                        if (!rfqDictionary.TryGetValue(rfq.RFQID, out RFQ r))
                        {
                            r = rfq;
                            r.Contracts = new List<Contract>();
                            rfqDictionary.Add(r.RFQID, r);
                        }
                        r.Contracts.Add(contr);
                        return r;
                    },
                    new {id = id},
                    splitOn: "RFQID");

                return result.FirstOrDefault();
            }
        }

        public async Task<IEnumerable<RFQ>> GetAllRFQs()
        {
            const string query = @"SELECT *  
                            FROM RFQ r  
                            INNER JOIN CONTRACT C  
                            ON r.RFQID = c.RFQID";

            using (var conn = new SqlConnection(connectionString.Value))
            {
                var rfqDictionary = new Dictionary<int, RFQ>();

                var result = await conn.QueryAsync<RFQ, Contract, RFQ>(
                    query,
                    (rfq, contr) =>
                    {
                        if (!rfqDictionary.TryGetValue(rfq.RFQID, out RFQ r))
                        {
                            r = rfq;
                            r.Contracts = new List<Contract>();
                            rfqDictionary.Add(r.RFQID, r);
                        }
                        r.Contracts.Add(contr);
                        return r;
                    },
                    splitOn: "RFQID, ContractID");

                return result.Distinct();
            }
        }

        public async Task<IEnumerable<Test>> GetTestN()
        {
            const string query = "100Ktest";

            using (var conn = new SqlConnection(connectionString.Value))
            {
                var testDictionary = new Dictionary<int, Test>();
                var result = await conn.QueryAsync<Test, RFQ, Test>(
                    query,
                    (t, r) =>
                    {
                        if (!testDictionary.TryGetValue(t.Number, out Test f))
                        {
                            f = t;
                            f.RFQs = new List<RFQ>();
                            testDictionary.Add(f.Number, f);
                        }
                        f.RFQs.Add(r);
                        return f;
                    },
                    splitOn: "Number,RFQID", commandType: CommandType.StoredProcedure);
                return result.Distinct();
            }
        }
    }
}
