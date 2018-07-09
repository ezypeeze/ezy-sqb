using System;

namespace EzySQB.Exceptions
{
    public class MissingFromClauseException : Exception
    {
        public MissingFromClauseException(
            string message = "The clause from is missing (hint: did you set the table from the statement?)"
        ) : base(message) {}
    }

    public class MissingValuesException : Exception
    {
        public MissingValuesException(
            string message = "The insert statement must have values (hint: did you use .Values() in your insert statement?)"
        ) : base(message) {}
    }
}