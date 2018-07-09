using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using EzySQB.Clauses;
using EzySQB.Contracts;
using EzySQB.Statements;

namespace EzySQB.Formatters
{
    public class StandardStatementFormatter : IStatementFormatter
    {

        public string FormatInsert(Insert insert)
        {
            return String.Join(
                " ",
                new string[] {
                    RenderExplain(insert.ShouldExplain()),
                    $"INSERT INTO {insert.GetTableName()}",
                    "(" + String.Join(", ", insert.GetValues().Keys.ToArray()) + ")",
                    "VALUES",
                    "(" + String.Join(", ", insert.GetValues().Values.ToArray()) + ")",
                    insert.GetReturningValues().Count > 0 ? "RETURNING" : "",
                    String.Join(", ", insert.GetReturningValues())
                }.Where(part => part != null && part.Length > 0 && part != "\n")
            );
        }

        public string PrettyFormatInsert(Insert insert)
        {
           return String.Join(
                "\n",
                new string[] {
                    RenderExplain(insert.ShouldExplain()),
                    $"INSERT INTO {insert.GetTableName()}",
                    "(" + String.Join(", ", insert.GetValues().Keys.ToArray()) + ")",
                    "VALUES " + "(" + String.Join(", ", insert.GetValues().Values.ToArray()) + ")",
                    insert.GetReturningValues().Count > 0 ? $"RETURNING {String.Join(", ", insert.GetReturningValues())}" : "",
                }.Where(part => part != null && part.Length > 0 && part != "\n")
            );
        }

        public string FormatUpdate(Update update)
        {
            return String.Join(
                " ",
                new string[] {
                    RenderExplain(update.ShouldExplain()),
                    $"UPDATE FROM {update.GetTableName()}",
                    "SET " + String.Join(", ", update.GetValues().Select(item => $"{item.Key} = {item.Value}")),
                    RenderWhereClauses(update.GetWhereClauses(), " ")
                }.Where(part => part != null && part.Length > 0 && part != "\n")
            );
        }

        public string PrettyFormatUpdate(Update update)
        {
            return String.Join(
                "\n",
                new string[] {
                    RenderExplain(update.ShouldExplain()),
                    $"UPDATE FROM {update.GetTableName()}",
                    "SET " + String.Join(", ", update.GetValues().Select(item => $"{item.Key} = {item.Value}")),
                    RenderWhereClauses(update.GetWhereClauses(), "\n\t")
                }.Where(part => part != null && part.Length > 0 && part != "\n")
            );
        }

        public string FormatDelete(Delete delete)
        {
            return String.Join(
                " ",
                new string[] {
                    RenderExplain(delete.ShouldExplain()),
                    $"DELETE FROM {delete.GetTableName()}",
                    RenderWhereClauses(delete.GetWhereClauses(), " ")
                }.Where(part => part != null && part.Length > 0 && part != "\n")
            );
        }

        public string PrettyFormatDelete(Delete delete)
        {
            return String.Join(
                "\n",
                new string[] {
                    RenderExplain(delete.ShouldExplain()),
                    $"DELETE FROM {delete.GetTableName()}",
                    RenderWhereClauses(delete.GetWhereClauses(), "\n\t")
                }.Where(part => part != null && part.Length > 0 && part != "\n")
            );
        }

        public string FormatSelect(Select select)
        {
            return JoinQuery(select);
        }

        public string PrettyFormatSelect(Select select)
        {
            return PrettyJoinQuery(select);
        }

        public string AssembleJoinType(JoinType joinType)
        {
            switch (joinType)
            {
                case JoinType.Full:
                    return "FULL JOIN";
                case JoinType.Inner:
                    return "INNER JOIN";
                case JoinType.Left:
                    return "LEFT JOIN";
                case JoinType.Right:
                    return "RIGHT JOIN";
                default:
                    return "JOIN";
            }
        }

        protected string RenderDistinct(bool isDistinct)
        {
            return isDistinct ? "DISTINCT" : "";
        }

        protected string RenderColumns(List<string> columns)
        {
            return String.Join(", ", columns);
        }

        protected string RenderFrom(List<string> froms)
        {
            return String.Join(", ", froms);
        }

        protected string RenderJoins(List<Join> joins, string delimiter = " ")
        {
            string result = "";
            for (int i = 0; i < joins.Count; i++)
            {
                Join join = joins[i];
                result += $"{(AssembleJoinType(join.Type))} {join.Name} ON {join.Condition}";

                if (i + 1 < joins.Count)
                {
                    result += delimiter;
                }
            }

            return result;
        }

        protected string RenderWhereClauses(List<Where> whereClauses, string delimiter, bool injectKeyword = true)
        {
            string result = "";
            for (int i=0; i < whereClauses.Count; i++)
            {
                Where clause = whereClauses[i];

                // Add where since we have clauses
                if (i == 0 && injectKeyword)
                {
                    result += "WHERE ";
                }

                // If subClauses are found, recursively render where clauses again, else adds condition.
                result += clause.HasSubClauses ?
                    $"({RenderWhereClauses(clause.SubClauses, delimiter, false)})" :
                    clause.Condition;

                // If there is a next clause, we add the next clause operator.
                if (i + 1 < whereClauses.Count)
                {
                    Where nextClause = whereClauses.ElementAt(i + 1);
                    result += $" {(nextClause.NextOperator == NextWhereLogicOperator.And ? "AND" : "OR")}" + delimiter;
                }
            }

            return result;
        }

        protected string RenderHavings(List<Having> havings, string delimiter)
        {
            string result = "";
            for (int i=0; i < havings.Count; i++)
            {
                Having clause = havings[i];

                // Add where since we have clauses
                if (i == 0)
                {
                    result += "HAVING ";
                }

                result += clause.Condition;

                // If there is a next clause, we add the next clause operator.
                if (i + 1 < havings.Count)
                {
                    Having nextClause = havings.ElementAt(i + 1);
                    result += $" {(nextClause.NextOperator == NextHavingLogicOperator.And ? "AND" : "OR")}" + delimiter;
                }
            }

            return result;
        }

        protected string RenderGroupBys(List<string> groupBys)
        {
            return groupBys.Count > 0 ? $"GROUP BY {(String.Join(", ", groupBys))}" : "";
        }

        protected string RenderOrderBys(Dictionary<string, OrderDirection> orderBys)
        {
            return orderBys.Count > 0 ?
                "ORDER BY " + String.Join(
                    ", ", orderBys.Select(order => {
                        return $"{order.Key} {(order.Value.Equals(OrderDirection.Asc) ? "ASC" : "DESC")}";
                    }).ToArray()
                )
                :
                "";
        }

        protected string RenderOffset(int offset)
        {
            return offset > -1 ? $"OFFSET {offset}" : "";
        }

        protected string RenderLimit(int limit)
        {
            return limit > -1 ? $"LIMIT {limit}" : "";
        }

        protected string RenderExplain(bool explain)
        {
            return explain ? "EXPLAIN" : "";
        }

        protected string JoinQuery(Select select)
        {
            return String.Join(
                " ",
                new string[] {
                    RenderExplain(select.ShouldExplain()),
                    "SELECT",
                    RenderDistinct(select.IsDistincted()),
                    RenderColumns(select.GetColumns()),
                    "FROM",
                    RenderFrom(select.GetFrom()),
                    RenderJoins(select.GetJoinClauses()),
                    RenderWhereClauses(select.GetWhereClauses(), " "),
                    RenderHavings(select.GetHavingClauses(), " "),
                    RenderGroupBys(select.GetGroupByClauses()),
                    RenderOrderBys(select.GetOrderByClauses()),
                    RenderOffset(select.GetOffsetClause()),
                    RenderLimit(select.GetLimitClause())
                }.Where(part => part != null && part.Length > 0)
            );
        }

        protected string PrettyJoinQuery(Select select)
        {
           return String.Join(
                " ",
                new string[] {
                    RenderExplain(select.ShouldExplain()),
                    "SELECT",
                    RenderDistinct(select.IsDistincted()),
                    RenderColumns(select.GetColumns()),
                    "\nFROM",
                    RenderFrom(select.GetFrom()),
                    "\n" + RenderJoins(select.GetJoinClauses(), "\n"),
                    "\n" + RenderWhereClauses(select.GetWhereClauses(), "\n\t"),
                    "\n" + RenderHavings(select.GetHavingClauses(), "\n\t"),
                    "\n" + RenderGroupBys(select.GetGroupByClauses()),
                    "\n" + RenderOrderBys(select.GetOrderByClauses()),
                    "\n" + RenderOffset(select.GetOffsetClause()),
                    RenderLimit(select.GetLimitClause())
                }.Where(part => part != null && part.Length > 0 && part != "\n")
            );
        }
    }
}