using System.Collections.Generic;
using EzySQB.Statements;
using EzySQB.Contracts;

namespace EzySQB
{
    public class QueryBuilder
    {

        protected IStatementFormatter Formatter;

        public QueryBuilder(IStatementFormatter grammar)
        {
            Formatter = grammar;
        }

        public Insert Insert(string from, Dictionary<string, dynamic> values = null)
        {
            return new Insert(Formatter)
                .Into(from)
                .Values(values);
        }

        public Update Update(string from, Dictionary<string, dynamic> values = null)
        {
            return new Update(Formatter)
                .From(from)
                .Values(values);
        }

        public Delete Delete(string from)
        {
            return new Delete(Formatter)
                .From(from);
        }

        public Select Select(string from, string[] columns = null)
        {
            return new Select(Formatter)
                .From(from)
                .Columns(columns);
        }

        public Select Select(string[] from, string[] columns = null)
        {
            return new Select(Formatter)
                .From(from)
                .Columns(columns);
        }

    }
}
