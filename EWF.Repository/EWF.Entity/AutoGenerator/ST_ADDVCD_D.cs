/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-05-23 16:35:45                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: EWF.Entity                                  
*│　类    名：ST_ADDVCD_POINT                                     
*└──────────────────────────────────────────────────────────────┘
*/


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EWF.Entity
{
    [Table("ST_ADDVCD_D")]
    public class ST_ADDVCD_D:IEntity
    {
        [Key]
        public string ADDVCD { get; set; }

        [MaxLength(50)]
        public string ADDVNM { get; set; }

        [MaxLength(200)]
        public string COMMENTS { get; set; }

        [MaxLength(23)]
        public DateTime? MODITIME { get; set; }

        [MaxLength(1)]
        public Int32? TYPE { get; set; }

    }
}
