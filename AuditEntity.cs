using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class AuditEntity
    {
        [Key]
        public long AuditEntityId { get; set; }
        public string EntityName { get; set; }
        public string EntityId { get; set; }
        public string ActionType { get; set; }
        public DateTime TimeStamp { get; set; }
        public Dictionary<string, object> Changes { get; set; } = new Dictionary<string, object>();
        [NotMapped]
        // TempProperties are used for properties that are only generated on save, e.g. ID's
        public List<PropertyEntry> TempProperties { get; set; }
    }
}
