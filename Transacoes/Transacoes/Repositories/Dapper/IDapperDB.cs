using System.Collections.Generic;
using Transacao.API.Data.Entities;

namespace Transacao.API.Repositories.Dapper
{
    public interface IDapperDB
    {
        List<Cidade> Search(string query);
    }
}
