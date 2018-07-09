using System;
using System.Collections.Generic;
using EzySQB.Contracts;
using EzySQB.Formatters;
using EzySQB.Statements;

namespace EzySQB.Clauses
{
    public class ScopedWhere : IWhereable<ScopedWhere>
    {
        protected List<Where> WhereClauses = new List<Where>();

        public List<Where> GetWhereClauses() => WhereClauses;
    }
}