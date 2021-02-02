using System.Collections.Generic;
using System.Linq;
using EFCore.BulkExtensions;
using NetCoreAPI_Template_v2.Data;
using NetCoreAPI_Template_v2.Models;

namespace NetCoreAPI_Template_v2.Services
{
    public class BuikService : IBulkService
    {
        private readonly AppDBContext _dbContext;
        public BuikService(AppDBContext dbContext)
        {
            _dbContext = dbContext;

        }
        public List<Bulk> BulkDelete()
        {
            List<Bulk> bulk = new List<Bulk>();
            bulk = _dbContext.Bulk.ToList();
            _dbContext.BulkDelete(bulk);
            return bulk;
        }

        public List<Bulk> BulkInsert()
        {
            List<Bulk> bulk = new List<Bulk>();
            bulk = GetDataForInsert();
            _dbContext.BulkInsert(bulk);
            return bulk;
        }

        public List<Bulk> BulkUpdate()
        {
            List<Bulk> bulk = new List<Bulk>();
            bulk = GetDataForUpdate();
            _dbContext.BulkUpdate(bulk);
            return bulk;
        }

        public List<Bulk> GetBulks()
        {
            return _dbContext.Bulk.ToList();
        }

        private static List<Bulk> GetDataForInsert()
        {
            List<Bulk> bulk = new List<Bulk>();
            for(int i = 0; i <= 100; i++)
            {
                bulk.Add(new Bulk() { BulkId = i + 1, BulkName = "Insert BulkName" + i, BulkCode = "InsertBulkCode" + i }); //gen id
            }
            return bulk;
        }

        private static List<Bulk> GetDataForUpdate()
        {
            List<Bulk> bulk = new List<Bulk>();
            for(int i = 0; i <= 100; i++)
            {
                bulk.Add(new Bulk() { BulkId = i + 1, BulkName = "Update BulkName" + i, BulkCode = "UpdateBulkCode" + i }); //gen id
            }
            return bulk;
        }
    }
}