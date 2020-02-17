/* **********************************************
 * 版  权：亿水泰科（EWater）
 * 创建人：zhujun
 * 日  期：2019/5/29 17:01:27
 * 描  述：ST_SEDRFDayViewModel
 * *********************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace EWF.ViewModels
{
    public class ST_SEDRFDayViewModel
    {
      public  string STCD { get; set; }
      public  string STNM { get; set; }
      public  DateTime IDTM { get; set; }
      public  double? WRNF { get; set; }
      public  double? STW { get; set; }
    }
}
