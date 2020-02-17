/**
*┌──────────────────────────────────────────────────────────────┐
*│　描    述：                                                    
*│　作    者：zhujun                                              
*│　版    本：1.0   模板代码自动生成                                              
*│　创建时间：2019-05-23 16:35:45                            
*└──────────────────────────────────────────────────────────────┘
*┌──────────────────────────────────────────────────────────────┐
*│　命名空间: EWF.Entity                                  
*│　类    名：SYS_USER                                     
*└──────────────────────────────────────────────────────────────┘
*/
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EWF.Entity
{
	/// <summary>
	/// zhujun
	/// 2019-05-23 16:35:45
	/// 
	/// </summary>
	[Table("TBL_SYS_USER")]
	public class SYS_USER:IEntity
	{
		/// <summary>
		///  
		/// </summary>
		[Key]
        public String USERID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String UNITID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String DEPARTMENTID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(10)]
		public Int32? INNERUSER {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String UACCOUNT {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String UPWD {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String SECRETKEY {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String REALNAME {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(200)]
		public String SPELL {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String GENDER {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(23)]
		public DateTime? BIRTHDAY {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String MOBILE {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String TELEPHONE {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String QQ {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String EMAIL {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(23)]
		public DateTime? CHANGEPASSWORDDATE {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(10)]
		public Int32? OPENID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(10)]
		public Int32? LOGONCOUNT {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(23)]
		public DateTime? FIRSTVISIT {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(23)]
		public DateTime? PREVIOUSVISIT {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(23)]
		public DateTime? LASTVISIT {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String AUDITSTATUS {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String AUDITUSERID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String AUDITUSERNAME {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(23)]
		public DateTime? AUDITDATETIME {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(10)]
		public Int32? UONLINE {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(200)]
		public String REMARK {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(10)]
		public Int32? ISENABLED {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(10)]
		public Int32? SORTCODE {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(10)]
		public Int32? DELETEMARK {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(23)]
		public DateTime? CREATEDATE {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String CREATEUSERID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String CREATEUSERNAME {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(23)]
		public DateTime? MODIFYDATE {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String MODIFYUSERID {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(50)]
		public String MODIFYUSERNAME {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(10)]
		public Int32? DUTYROLE {get;set;}

		/// <summary>
		///  
		/// </summary>
		[MaxLength(10)]
		public Int32? ISDUTY {get;set;}
        /// <summary>
		///  
		/// </summary>
		[MaxLength(6)]
        public string ADDVCD { get; set; }
        /// <summary>
		///  
		/// </summary>
		[MaxLength(10)]
        public Int32? TYPE { get; set; }


    }
}
