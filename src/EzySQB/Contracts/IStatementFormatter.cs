using System;
using EzySQB.Clauses;
using EzySQB.Statements;

namespace EzySQB.Contracts
{
    public interface IStatementFormatter
    {

        // Statements Format
        string FormatInsert(Insert insert);
        string PrettyFormatInsert(Insert insert);
        string FormatUpdate(Update update);
        string PrettyFormatUpdate(Update update);
        string FormatDelete(Delete delete);
        string PrettyFormatDelete(Delete delete);
        string FormatSelect(Select select);
        string PrettyFormatSelect(Select select);

    }
}