namespace EzySQB.Clauses
{
    public enum JoinType
    {
        Left,
        Right,
        Inner,
        Full,
    }

    public class Join
    {

        public Join(string name, string condition, JoinType type)
        {
            Name = name;
            Condition = condition;
            Type = type;
        }

        public string Name { get; set; }
        public string Condition { get; set; }
        public JoinType Type { get; set; }
    }

}