using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Transacao.API.Data.Entities;

namespace Transacao.API.Repositories.Dapper
{
    public class DapperDB : IDapperDB, IDisposable
    {
        private readonly IDbConnection _db;
        public DapperDB(IDbConnection db) => _db = db;

        public void Dispose()
        {
            if(_db.State != ConnectionState.Closed)
                _db.Close();
        }

        public List<Cidade> Search(string query)
        {
            return _db.Query<Cidade>(query).ToList();
        }
    }
}
