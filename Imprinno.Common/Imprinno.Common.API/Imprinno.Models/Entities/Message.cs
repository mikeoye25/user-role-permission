using Imprinno.Models.Enums;
using Imprinno.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Imprinno.Models.Entities
{
    [Table("messages", Schema = "mess")]
    public class Message : BaseEntity
    {
        [Required]
        [StringLength(20, MinimumLength = 3)]
        public string To { get; set; }
        [StringLength(20, MinimumLength = 5)]
        public string From { get; set; }
        [StringLength(100)]
        public string SenderName { get; set; }
        [StringLength(255)]
        public string Subject { get; set; }
        [Required]
        [StringLength(8192, MinimumLength = 3)]
        public string Body { get; set; }
        public MessageType MessageType { get; set; }
        public bool? Status { get; set; }
    }
}
