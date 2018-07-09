using System;
using System.Collections.Generic;
using EzySQB.Clauses;
using EzySQB.Contracts;
using EzySQB.Exceptions;
using EzySQB.Formatters;

namespace EzySQB.Statements
{

    public enum OrderDirection {
        Asc,
        Desc
    }

    public class Select : AbstractStatement<Select>, IWhereable<Select>, IJoinable<Select>
    {

        protected bool IsDistinct = false;

        protected List<string> Froms = new List<string>();

        protected List<string> ColumnValues { get; set; }

        protected List<Join> Joins = new List<Join>();

        protected List<Where> WhereClauses = new List<Where>();

        protected List<string> GroupBys = new List<string>();

        protected List<Having> Havings = new List<Having>();

        protected Dictionary<string, OrderDirection> OrderBys = new Dictionary<string, OrderDirection>();

        protected int OffsetValue = -1;

        protected int LimitValue = -1;

        public Select(IStatementFormatter formatter) : base(formatter)
        {
            ColumnValues = new List<string> { "*" };
        }

        public bool IsDistincted() => IsDistinct;

        public List<string> GetFrom() => Froms;

        public List<string> GetColumns() => ColumnValues;

        public List<Join> GetJoinClauses() => Joins;

        public List<Where> GetWhereClauses() => WhereClauses;

        public List<Having> GetHavingClauses() => Havings;

        public List<string> GetGroupByClauses() => GroupBys;

        public Dictionary<string, OrderDirection> GetOrderByClauses() => OrderBys;

        public int GetOffsetClause() => OffsetValue;

        public int GetLimitClause() => LimitValue;

        public Select Distinct(bool distinct = true)
        {
            IsDistinct = distinct;

            return this;
        }

        public Select From(string value)
        {
            Froms.Add(value);

            return this;
        }

        public Select From(IEnumerable<string> values)
        {
            Froms = new List<string>(values);

            return this;
        }

        public Select Columns(string value)
        {
            if (value != null)
            {
                ColumnValues.Add(value);
            }

            return this;
        }

        public Select Columns(IEnumerable<string> values)
        {
            if (values != null)
            {
                 ColumnValues = new List<string>(values);
            }

            return this;
        }

        public Select Offset(int offset)
        {
            OffsetValue = offset;

            return this;
        }

        public Select Limit(int limit)
        {
            LimitValue = limit;

            return this;
        }

        public Select OrderBy(string key, OrderDirection direction = OrderDirection.Asc)
        {
            OrderBys.Add(key, direction);

            return this;
        }

        public Select OrderBy(Dictionary<string, OrderDirection> orderBys)
        {
            OrderBys = orderBys;

            return this;
        }

        public Select GroupBy(string key)
        {
            GroupBys.Add(key);

            return this;
        }

        public Select GroupBy(IEnumerable<string> groupBys)
        {
            GroupBys = new List<string>(groupBys);

            return this;
        }

        public Select Having(string condition, dynamic value)
        {
            Havings.Add(new Having(condition, value, NextHavingLogicOperator.And));

            return this;
        }

         public Select AndHaving(string condition, dynamic value)
        {
            return Having(condition, value);
        }

        public Select OrHaving(string condition, dynamic value)
        {
            Havings.Add(new Having(condition, value, NextHavingLogicOperator.And));

            return this;
        }

        public override string Assemble()
        {
            Validate();
            return Formatter.FormatSelect(this);
        }

        public override string PrettyAssemble()
        {
            Validate();
            return Formatter.PrettyFormatSelect(this);
        }

        public override string ToString() => Assemble();

        protected override void Validate()
        {
            if (Froms == null || Froms.Count == 0)
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
