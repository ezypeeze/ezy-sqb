using EzySQB;
using EzySQB.Clauses;
using EzySQB.Formatters;

namespace EzySQB.Tests
{
    abstract public class TestBase
    {

        protected QueryBuilder QueryBuilder;

        protected QueryBuilder GetQueryBuilder()
        {
            if (QueryBuilder == null)
            {
                // Will use ADO binding format
                QueryBuilder = new QueryBuilder(new StandardStatementFormatter());
            }

            return QueryBuilder;
        }

    }
}