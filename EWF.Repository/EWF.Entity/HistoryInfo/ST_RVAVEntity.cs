/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/25 16:56:05
 * 描  述：ST_RVAVEntity
 * *********************************************/
//using Dapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EWF.Util.Data;

namespace EWF.Entity
{
    [Table(EWFConsts.RTDB_SchemaName+"ST_RVAV_R")]
    public class ST_RVAVEntity
    {
        [Key]
        public string STCD { get; set; }
        [Key]
        [Required]
        public DateTime IDTM { get; set; }
        [Key]
        public string STTDRCD { get; set; }

        //[Editable(false)]//在插入中忽略
        //[IgnoreInsert]//在插入中忽略
        //[IgnoreUpdate]//在更新中忽略
        [ReadOnly(true)]//只有在Select中使用
        public string STNM { get; set; }
        public double? AVZ { get; set; }
        public double? AVQ { get; set; }
    }
}
