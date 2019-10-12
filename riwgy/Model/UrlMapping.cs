using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace riwgy.Model
{
    public class UrlMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public virtual Guid Id { get; set; }
        public virtual string Riwgy { get; set; }
        public virtual string OriginalUrl { get; set; }
    }
}
