namespace EzySQB.Clauses
{

    public enum NextHavingLogicOperator
    {
        And,
        Or
    }

    public class Having
    {

        public Having(string condition, dynamic value, NextHavingLogicOperator nextOperator = NextHavingLogicOperator.And)
        {
            Condition = condition;
            Value = value;
            NextOperator = nextOperator;
        }

        public string Condition { get; set; }
        public dynamic Value { get; set; }
        public NextHavingLogicOperator NextOperator { get; set; }

    }

}