using EzySQB.Contracts;

namespace EzySQB.Statements
{
    public abstract class AbstractStatement<T> : IStatement where T : AbstractStatement<T>
    {

        protected IStatementFormatter Formatter;

        protected bool ExplainQuery = false;

        public AbstractStatement(IStatementFormatter formatter)
        {
            Formatter = formatter;
        }

        public T Explain(bool isExplain)
        {
            ExplainQuery = isExplain;

            return (T)this;
        }

        public bool ShouldExplain() => ExplainQuery;

        public abstract string Assemble();

        public abstract string PrettyAssemble();

        protected virtual void Validate() {}
    }
}