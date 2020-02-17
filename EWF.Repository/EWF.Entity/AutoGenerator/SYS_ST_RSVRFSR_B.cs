using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EWF.Entity
{
    [Table("ST_RSVRFSR_B")]
    public class SYS_ST_RSVRFSR_B : IEntity
    {
        /// <summary>
        ///  
        /// </summary>
        [Key]
        public String STCD { get; set; }
        /// <summary>
        ///  
        /// </summary>
        [Key]
        public Int32? ACTYR { get; set; }
        /// <summary>
        ///  
        /// </summary>
        [Key]
        public String BGMD { get; set; }
        /// <summary>
		///  
		/// </summary>
		[MaxLength(4)]
        public String EDMD { get; set; }
        /// <summary>
        ///  
        /// </summary>
        
        public double FSLTDZ { get; set; }
        /// <summary>
        ///  
        /// </summary>
        public String FSTP { get; set; }
    }
}
