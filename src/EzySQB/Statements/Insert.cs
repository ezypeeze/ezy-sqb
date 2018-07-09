using System.Collections.Generic;
using EzySQB.Contracts;
using EzySQB.Exceptions;
using EzySQB.Formatters;

namespace EzySQB.Statements
{
    public class Insert : AbstractStatement<Insert>
    {

        protected string Table;

        protected Dictionary<string, dynamic> ColumnValues = new Dictionary<string, dynamic>();

        protected List<string> ReturnValues = new List<string>();

        public Insert(IStatementFormatter formatter) : base(formatter) {}

        public Insert Into(string table)
        {
            Table = table;

            return this;
        }

        public Insert Values(Dictionary<string, dynamic> values)
        {
            if (values != null)
            {
                ColumnValues = values;
            }

            return this;
        }

        public Insert Value(string key, dynamic value)
        {
            ColumnValues.Add(key, value);

            return this;
        }

        public Insert Returning(string column)
        {
            ReturnValues.Add(column);

            return this;
        }

        public Insert Returning(List<string> columns)
        {
            ReturnValues = columns;

            return this;
        }

        public Insert ReturningId(string column = "id") => Returning(column);

        public string GetTableName() => Table;

        public Dictionary<string, dynamic> GetValues() => ColumnValues;

        public List<string> GetReturningValues() => ReturnValues;

        public override string Assemble()
        {
            Validate();
            return Formatter.FormatInsert(this);
        }

        public override string PrettyAssemble()
        {
            Validate();
            return Formatter.PrettyFormatInsert(this);
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