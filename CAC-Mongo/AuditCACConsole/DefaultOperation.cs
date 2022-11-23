using ApiAuditCAC.Core.MongoCollection;
using static System.Guid;

namespace AuditCACConsole
{
    public class DefaultOperation :
        ITransientOperation,
        IScopedOperation,
        ISingletonOperation
    {
        public string OperationId { get; } = NewGuid().ToString()[^4..];
    }
}
