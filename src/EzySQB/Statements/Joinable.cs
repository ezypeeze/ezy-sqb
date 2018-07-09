using System;
using EzySQB.Clauses;
using EzySQB.Contracts;

namespace EzySQB.Statements
{
    public static class Joinable
    {

        public static T Join<T>(this IJoinable<T> joinable, string name, string condition, JoinType type = JoinType.Left)
        {
            joinable.GetJoinClauses().Add(new Join(name, condition, type));

            return (T)joinable;
        }

        public static T LeftJoin<T>(this IJoinable<T> joinable, string name, string condition)
        {
            return Join(joinable, name, condition, JoinType.Left);
        }

        public static T RightJoin<T>(this IJoinable<T> joinable, string name, string condition)
        {
            return Join(joinable, name, condition, JoinType.Right);
        }

        public static T FullJoin<T>(this IJoinable<T> joinable, string name, string condition)
        {
            return Join(joinable, name, condition, JoinType.Full);
        }

        public static T InnerJoin<T>(this IJoinable<T> joinable, string name, string condition)
        {
            return Join(joinable, name, condition, JoinType.Inner);
        }

    }
}