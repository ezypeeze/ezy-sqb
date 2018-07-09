using System.Collections.Generic;
using EzySQB.Clauses;

namespace EzySQB.Contracts
{
    public interface IWhereable<T>
    {
        List<Where> GetWhereClauses();

    }
}