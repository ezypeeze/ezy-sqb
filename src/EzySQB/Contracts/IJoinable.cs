using System.Collections.Generic;
using EzySQB.Clauses;

namespace EzySQB.Contracts
{
    public interface IJoinable<T>
    {
        List<Join> GetJoinClauses();

    }
}