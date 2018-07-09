using System.Collections.Generic;
using EzySQB.Clauses;
using EzySQB.Contracts;
using EzySQB.Exceptions;

namespace EzySQB.Statements
{
    public class Update : AbstractStatement<Update>, IWhereable<Update>, IJoinable<Update>
    {

        protected string Table;

        protected Dictionary<string, dynamic> ColumnValues = new Dictionary<string, dynamic>();

        protected List<Where> WhereClauses = new List<Where>();

        protected List<Join> Joins = new List<Join>();

        public Update(IStatementFormatter formatter) : base(formatter) {}

        public string GetTableName() => Table;

        public Dictionary<string, dynamic> GetValues() => ColumnValues;

        public List<Where> GetWhereClauses() => WhereClauses;

        public List<Join> GetJoinClauses() => Joins;

        public Update From(string table)
        {
            Table = table;

            return this;
        }

        public Update Values(Dictionary<string, dynamic> values)
        {
            if (values != null)
            {
                ColumnValues = values;
            }

            return this;
        }

        public Update Value(string key, dynamic value)
        {
            ColumnValues.Add(key, value);

            return this;
        }

        public override string Assemble()
        {
            Validate();
            return Formatter.FormatUpdate(this);
        }

        public override string PrettyAssemble()
        {
            Validate();
            return Formatter.PrettyFormatUpdate(this);
        }

        public override string ToString() => Assemble();

        protected override void Validate()
        {
            if (Table == null || Table.Length == 0)
            {
                throw new MissingFromClauseException();
            }

            if (ColumnValues == null || ColumnValues.Count == 0)
            {
                throw new MissingValuesException();
            }

            base.Validate();
        }
    }
}