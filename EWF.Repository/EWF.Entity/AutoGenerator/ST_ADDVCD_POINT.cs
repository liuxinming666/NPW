using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EWF.Entity
{

    [Table("ST_ADDVCD_POINT")]
    public class ST_ADDVCD_POINT : IEntity
    {
        [Key]
        public string ADDVCD { get; set; }

        [Key]
        public string STCD { get; set; }

        [MaxLength(1)]
        public Int32? TYPE { get; set; }

    }
}
