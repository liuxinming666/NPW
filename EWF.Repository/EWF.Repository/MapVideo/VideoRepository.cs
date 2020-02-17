/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：JinJianPing
 * 日  期：2019/5/31 10:36:04
 * 描  述：VideoRepository
 * *********************************************/
using EWF.Data;
using EWF.Data.Repository;
using EWF.IRepository;
using EWF.Util.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using EWF.Util;
using EWF.Util.Page;
using System.Data;

namespace EWF.Repository
{
    public class VideoRepository : DefaultRepository, IVideoRepository
    {
		private readonly string TBL_SYS_CAMERA;
		private readonly string TBL_SYS_VIDEOMANAGE;
		private readonly string ST_STBPRP_V;
		private readonly string tbl_sys_video;
		public VideoRepository(IOptionsSnapshot<DbOption> options) : base(options)
		{
			ST_STBPRP_V = $"{Default_Schema}ST_STBPRP_V";
			tbl_sys_video = $"{Default_Schema}tbl_sys_video";
			TBL_SYS_VIDEOMANAGE = $"{Default_Schema}TBL_SYS_VIDEOMANAGE";
			TBL_SYS_CAMERA= $"{Default_Schema}TBL_SYS_CAMERA";

		}
		/// <summary>
		/// 获取视频站点列表
		/// </summary>
		/// <param name="ids">自增ID</param>
		/// <returns></returns>
		public DataTable GetVideoList(string ids, int type, string addvcd)
        {
			////先执行存储过程例子
			//using (var db = database.Connection)
			//{                
			//    db.Execute("PR_WARN_LIST", null, null, null, CommandType.StoredProcedure); //0
			//}
			//database.ExecuteByProc("PR_WARN_LIST");

			//strSql = "select a.*,b.ADDRESSNAME,b.IP,b.PORT,b.USERNAME,b.PASSWORD,b.CHANNELID from tbl_sys_video a left join [TBL_SYS_CAMERA] b on a.ID=b.VID where id in ('" + ids+"')";

			//strSql = "select a.*,b.ADDRESSNAME,b.IP,b.PORT,b.USERNAME,b.PASSWORD,b.CHANNELID from tbl_sys_video a left join [TBL_SYS_CAMERA] b on a.ID=b.VID";

			var strSql = "";
			if (!ids.IsEmpty())
                strSql = $"select AA.*,b.ADDRESSNAME,b.IP,b.PORT,b.USERNAME,b.PASSWORD,b.CHANNELID from (select a.* from {tbl_sys_video} a inner join {ST_STBPRP_V} c on a.STCD = c.STCD and c.type= {type} and c.ADDVCD='{addvcd}') AA left join {TBL_SYS_CAMERA}  b on AA.ID=b.VID where id in ('" + ids + "')";
            else
                strSql = $"select AA.*,b.ADDRESSNAME,b.IP,b.PORT,b.USERNAME,b.PASSWORD,b.CHANNELID from (select a.* from {tbl_sys_video} a inner join {ST_STBPRP_V} c on a.STCD = c.STCD and c.type= {type} and c.ADDVCD='{addvcd}') AA left join {TBL_SYS_CAMERA} b on AA.ID=b.VID";

			var dt = database.FindTable(strSql);
            return dt;
        }

		/// <summary>
		/// 获取视频站点列表
		/// </summary>
		/// <param name="STCD">站点编码</param>
		/// <returns></returns>
		public DataTable GetVideoManageList(string STCD)
		{
			if (STCD.IsEmpty())
			{
				return null;
			}
			var strSql = $"select * from {TBL_SYS_VIDEOMANAGE} where stcd='" + STCD + "'";
			var dt = database.FindTable(strSql);
			return dt;
		}
    }
}
