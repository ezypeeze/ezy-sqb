using System;
using EzySQB.Clauses;
using EzySQB.Contracts;

namespace EzySQB.Statements
{
    public static class Whereable
    {

        public static T Where<T>(this IWhereable<T> whereable, string condition, NextWhereLogicOperator nextOperator = NextWhereLogicOperator.And)
        {
            whereable.GetWhereClauses().Add(new Where(condition, nextOperator));

            return (T)whereable;
        }

        public static T Where<T>(this IWhereable<T> whereable, Func<ScopedWhere, ScopedWhere> groupedClauseFunc, NextWhereLogicOperator nextOperator = NextWhereLogicOperator.And)
        {
            ScopedWhere scopedWhere = new ScopedWhere();
            groupedClauseFunc(scopedWhere);
            whereable.GetWhereClauses().Add(new Where(scopedWhere.GetWhereClauses()));

            return (T)whereable;
        }

        public static T AndWhere<T>(this IWhereable<T> whereable, string condition)
        {
            return Where(whereable, condition);
        }

        public static T AndWhere<T>(this IWhereable<T> whereable, Func<ScopedWhere, ScopedWhere> groupedClauseFunc)
        {
             return Where(whereable, groupedClauseFunc);
        }

        public static T OrWhere<T>(this IWhereable<T> whereable, string condition)
        {
            return Where(whereable, condition, NextWhereLogicOperator.Or);
        }

        public static T OrWhere<T>(this IWhereable<T> whereable, Func<ScopedWhere, ScopedWhere> groupedClauseFunc)
        {
             return Where(whereable, groupedClauseFunc, NextWhereLogicOperator.Or);
        }
    }
}