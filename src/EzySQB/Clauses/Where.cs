using System.Collections.Generic;
using EzySQB.Contracts;

namespace EzySQB.Clauses
{

    public enum NextWhereLogicOperator
    {
        And,
        Or
    }

    public class Where
    {
        public Where(string condition, NextWhereLogicOperator nextOperator = NextWhereLogicOperator.And)
        {
            Condition = condition;
            NextOperator = nextOperator;
        }

        public Where(List<Where> subClauses, NextWhereLogicOperator nextOperator = NextWhereLogicOperator.And)
        {
            SubClauses = subClauses;
            NextOperator = nextOperator;
        }

        public dynamic Condition { get; set; }
        public NextWhereLogicOperator NextOperator { get; set; }
        public List<Where> SubClauses { get; set; } = new List<Where>();
        public bool HasSubClauses {
            get {
                return SubClauses.Count > 0;
            }
        }
    }
}