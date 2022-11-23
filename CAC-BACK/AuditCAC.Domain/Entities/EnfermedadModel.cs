namespace AuditCAC.Domain.Entities
{
    public class EnfermedadModel
    {
        public int IdEnfermedad { get; set; }
        public int IdCobertura { get; set; }
        public string Nombre { get; set; }
        public bool Status { get; set; }
    }
}
