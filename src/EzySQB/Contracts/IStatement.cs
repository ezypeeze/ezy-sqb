using System.Collections.Generic;

namespace EzySQB.Contracts
{
    public interface IStatement
    {
        string Assemble();

        string PrettyAssemble();
    }
}