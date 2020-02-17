using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace EWF.Entity
{
    [Table("TBL_SYS_SYSCONFIG")]
    public class SYS_Config : IEntity
    {
        /// <summary>
        ///  
        /// </summary>
        [Key]
        public Int32 SYSID { get; set; }

        /// <summary>
        ///  
        /// </summary>
        [MaxLength(50)]
        public String SYSNAME { get; set; }

        /// <summary>
        ///  
        /// </summary>
        [MaxLength(50)]
        public String SYSLOGO { get; set; }

        /// <summary>
        ///  
        /// </summary>
        [MaxLength(50)]
        public String SYSBGPIC { get; set; }

        /// <summary>
        ///  
        /// </summary>
        [MaxLength(1000)]
        public String SYSCONTENT { get; set; }
        /// <summary>
        ///  
        /// </summary>
        [MaxLength(50)]
        public String SYSCOL { get; set; }
        /// <summary>
        ///  
        /// </summary>
        [MaxLength(10)]
        public String ADDVCD { get; set; }
        /// <summary>
        ///  
        /// </summary>
        [MaxLength(200)]
        public String STCD { get; set; }
        /// <summary>
        ///  经纬度以逗号分隔
        /// </summary>
        [MaxLength(100)]
        public String LGLT { get; set; }
        /// <summary>
        ///  关注的视频的站点
        /// </summary>
        [MaxLength(300)]
        public String VIDEOSTCD { get; set; }
        /// <summary>
        ///  摄像头名字
        /// </summary>
        [MaxLength(1000)]
        public String VIDEONAME { get; set; }
    }
}
