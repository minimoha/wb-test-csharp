namespace TestApi.Models
{
    public enum AuditType
    {
        Create = 1,
        Update = 2,
        Delete = 3
    }

    public class Audit
    {
        public long Id { get; set; }
        public string User { get; set; }
        public string Type { get; set; }
        public string TableName { get; set; }
        public DateTime DateTime { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string AffectedColumns { get; set; }
        public string PrimaryKey { get; set; }
    }
}
