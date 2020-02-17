using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EWF.Entity
{
    [Table("TBL_EVENT_YLMODAL")]
    public class TBL_EVENT_YLMODAL
    {
        [Key]
        public Int32 YLBH { get; set; }
        public Int32? YLJB { get; set; }
        public string JBMC { get; set; }
        public decimal? THRESHOLD { get; set; }
        public string LEGEND { get; set; }
        public string YLFLAG { get; set; }
        public string REMARKS { get; set; }
        public decimal? DURATION { get; set; }
        public Int32? YXQ { get; set; }
        public string MOTYPE { get; set; }
        public decimal? QJFZ { get; set; }
        public Int32? TYPE { get; set; }
        public string ADDVCD { get; set; }
    }
}
