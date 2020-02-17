/**
 *水文业务脚本
 *add by SUn
 * Date:2019-05-31 11:00
 **/
var hydg = {}
/**
 * 计算径流量
 * @param swData：水文数据(系列数据)
 */
hydg.RunOff = function (swData) {
    var r_RunOffValue = 0;
    var fluxs = 0;//通量，（梯形的上底+下底）/2
    var zTime = 0;//起始时间
    var sTime = 0;//结束时间
    for (var i = 0; i < swData.length - 1; i++) {
        //将线划分成多个梯形，前后两个流量数据作为上底和下底
        if (swData[i].Q && swData[i + 1].Q) {
            fluxs = (Number(swData[i].Q) + Number(swData[i + 1].Q))/2;
            sTime = new Date(swData[i].TM.replace(/-/g, " / ")).getTime() / 1000;//距离1970-01-01的秒数
            eTime = new Date(swData[i + 1].TM.replace(/-/g, " / ")).getTime() / 1000;//距离1970-01-01的秒数
            r_RunOffValue += (fluxs * (eTime - sTime));
        }
    }

    r_RunOffValue = r_RunOffValue / 1000000;//转换单位为百万立方米
    return r_RunOffValue.toFixed(2);
}
/**
 * 含沙量计算
 * add by SUN
 * Date:2019-05-31 12:00
 * @param sedData 水流沙数据[{TM:'2019-01-01 08:00',Q:15,Z:15,S:23}]
 */
hydg.SedDscg = function (sedData) { 

    var minTemp = Number.MAX_VALUE;
    var maxTemp = Number.MIN_VALUE;
    //构造结果对象
    var objResult = {
        startDate: '',
        endDate: '',
        timeSpan:0,
        minQ: Number.MAX_VALUE,//最小流量
        minQTM: '', //最小流量出现时间
        maxQ: Number.MIN_VALUE,//最大流量
        maxQTM: '',//最大流量出现时间
        avQ: 0,//平均流量
        minZ: Number.MAX_VALUE,//最低水位
        minZTM: '',//最低水位出现时间
        maxZ: Number.MIN_VALUE,//最高水位
        maxZTM: '',//最高水位出现时间
        avZ:0,//平均水位
        minS: Number.MAX_VALUE,//最小含沙量
        minSTM:'',//最小含沙量出现时间
        maxS: Number.MIN_VALUE,//最大含沙量
        maxSTM: '',//最大含沙量出现时间
        maxQZ: 0,//最大流量对应水位
        maxZQ:0,//最高水位对应流量
        avS: 0,//平均含沙量,
        r_RunOffValue:0,//径流总量
        r_SedDscg:0//输沙总量
    };
    if (sedData.length > 0) {
        objResult.startDate = sedData[0].TM;
        objResult.endDate = sedData[sedData.length - 1].TM;
        objResult.timeSpan = (new Date(objResult.endDate.replace(/-/g, " / ")).getTime() - new Date(objResult.startDate.replace(/-/g, " / ")).getTime()) / 1000 / 3600;
    }
    
    //求最大最小含沙量流量
    for (var i = 0; i < sedData.length; i++) {
        //流量
        if (sedData[i].Q) {
             //求最小流量和对应时间
            if (objResult.minQ > sedData[i].Q) {
                objResult.minQ = sedData[i].Q;
                objResult.minQTM = sedData[i].TM;
            }
            //求最大流量和对应时间
            if (objResult.maxQ < sedData[i].Q) {
                objResult.maxQ = sedData[i].Q;
                objResult.maxQTM = sedData[i].TM;
                objResult.maxQZ = sedData[i].Z;//最大流量对应水位
            }
        }
        
        //水位
        if (sedData[i].Z) {
            //求最低水位和对应时间
            if (objResult.minZ > sedData[i].Z) {
                objResult.minZ = sedData[i].Z;
                objResult.minZTM = sedData[i].TM;
            }
            //求最高水位和对应时间
            if (objResult.maxZ < sedData[i].Z) {
                objResult.maxZ = sedData[i].Z;
                objResult.maxZTM = sedData[i].TM;
                objResult.maxZQ = sedData[i].Q;//最高水位对应流量
            }
        }
        
       
        //含沙量
        if (sedData[i].S) {
            //求最小含沙量和对应时间
            if (objResult.minS > sedData[i].S) {
                objResult.minS = sedData[i].S;
                objResult.minSTM = sedData[i].TM;
            }
            //求最大含沙量和对应时间
            if (objResult.maxS < sedData[i].S) {
                objResult.maxS = sedData[i].S;
                objResult.maxSTM = sedData[i].TM;
            }
        }
        
        var fsuxs = 0;//输沙量通量
        var sTM = 0;
        var eTM = 0;
        var flq = 0;
       
        //计算输沙总量
        if (i < sedData.length - 1) {
            if (sedData[i].S && Number(sedData[i].S) > 0) {
                if (sedData[i].TM) {
                    sTM = new Date(sedData[i].TM.replace(/-/g, " / ")).getTime() / 1000;
                    
                }
                if (sedData[i + 1].TM) {
                    eTM = new Date(sedData[i + 1].TM.replace(/-/g, " / ")).getTime() / 1000;
                }
                if (sedData[i].Q) {
                    fsuxs = Number(sedData[i].Q) * Number(sedData[i].S); 
                }
                if (sedData[i + 1].S && Number(sedData[i].S) > 0) {
                    if (sedData[i + 1].Q) {
                        flq = (Number(sedData[i].Q) + Number(sedData[i + 1]).Q) / 2;
                        fsuxs = (fsuxs + Number(sedData[i + 1].Q) * Number(sedData[i + 1].S)) / 2;
                        if (sTM != 0 && eTM != 0) {
                            objResult.r_SedDscg += fsuxs * (eTM - sTM);
                        }
                        
                    }
                }
            }
            
        }
    }
    objResult.r_SedDscg = objResult.r_SedDscg / 10000000;//单位转换位万吨
    objResult.r_SedDscg = objResult.r_SedDscg.toFixed(2);
    objResult.r_RunOffValue = this.RunOff(sedData);

    if (objResult.maxQ == Number.MIN_VALUE) {
        objResult.maxQ = "";
    }
    if (objResult.minQ == Number.MAX_VALUE) {
        objResult.minQ = "";
    }

    if (objResult.maxZ == Number.MIN_VALUE) {
        objResult.maxZ = "";
    }
    if (objResult.minZ == Number.MAX_VALUE) {
        objResult.minZ = "";
    }

    if (objResult.maxS == Number.MIN_VALUE) {
        objResult.maxS = "";
    }
    if (objResult.minS == Number.MAX_VALUE) {
        objResult.minS = "";
    }
    return objResult;
}

/**
 * 冲淤变化计算
 * add by SUN
 * Date:2019-06-05 16:00
 * @param sectData 格式：[{SCSJ:'2018-01-01 08:00',QDJ:50,GC:100}]，其中SCSJ:施测时间，QDJ:起点距，GC:高程
 * @param SetGC 设置的高程线
 * @param clickPosition 点击位置
 */
hydg.SumSect = function (sectData,SetGC,clickPosition) {
    //返回结果序列，数组，里面是对象{TM:'',GC:100,AREA:0,LeftQDJ:0,RightQDJ:0,AvgWaterDepth:0}
    var objResult = [];
    var startIndex = 0;//起点位置
    var endIndex = 0;//结束点位置
    var lastIndex = hydg.GetNearIndex(sectData, clickPosition.X, "QDJ");
    for (var k = lastIndex; k >= 0; k--) {

    }
}
/**
 * 获取距离目标值最近点的索引位置
 * add by SUN
 * Date:2019-06-05 17:00
 * @param {any} data
 * @param {any} targetV
 * @param {any} fieldName
 */
hydg.GetNearIndex = function (data, targetV, fieldName) {
    var dv = Number.MAX_VALUE;//记录差值
    var v_Index = -1;
    for (var j = 0; j < data.length; j++) {
        if (data[j][fieldName]) {
            if (Number(data[j][fieldName])) {
                if (dv > Math.abs(Number(data[j][fieldName]) - targetV)) {
                    dv = Math.abs(Number(data[j][fieldName]) - targetV);
                    v_Index = j;
                }
            }
        }
    }
    return v_Index;
}