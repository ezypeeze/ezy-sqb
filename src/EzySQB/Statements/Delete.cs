using System.Collections.Generic;
using EzySQB.Clauses;
using EzySQB.Contracts;
using EzySQB.Exceptions;

namespace EzySQB.Statements
{
    public class Delete : AbstractStatement<Delete>, IWhereable<Delete>
    {
        protected string Table;

        protected List<Where> WhereClauses = new List<Where>();

        public Delete(IStatementFormatter formatter) : base(formatter) {}

        public string GetTableName() => Table;

        public List<Where> GetWhereClauses() => WhereClauses;

        public Delete From(string table)
        {
            Table = table;

            return this;
        }

        public override string Assemble()
        {
            return Formatter.FormatDelete(this);
        }

        public override string PrettyAssemble()
        {
            Validate();
            return Formatter.PrettyFormatDelete(this);
        }

        public override string ToString() => Assemble();

        protected override void Validate()
        {
            if (Table == null || Table.Length == 0)
            {
                throw new MissingFromClauseException();
            }

            base.Validate();
        }

    }
}