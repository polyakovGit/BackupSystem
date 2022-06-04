using System;
using RIS.Unions;
using RIS.Unions.Types;

namespace ServerService
{
    [GenerateUnion(nameof(Error), nameof(Success))]
    public partial class CommandResult : UnionBase<Error, Success>
    {

    }
}
