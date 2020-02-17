/// <reference path="utils.js" /> 
var baseMapUrl = "http://220.176.162.63:6080/ArcGIS/rest/services/ycmap/MapServer";
var URL_WEBAPI_ROOT = 'http://localhost:16344/api';
var com = {};
com.vm = {};
//格式化时间
com.formatDate = function (value) { 
    return utils.formatDate(value, 'yyyy-MM-dd');
};

com.formatTime = function (value) {
    return utils.formatDate(value, 'yyyy-MM-dd hh:mm:ss');
};
com.formatTimemm = function (value) {
    return utils.formatDate(value, 'yyyy-MM-dd hh:mm');
};
com.formatDDHHMM = function (value) {
    return utils.formatDate(value, 'MM-dd hh:mm');
};
//格式化金额
com.formatMoney = function (value) {
    var sign = value < 0 ? '-' : '';
    return sign + utils.formatNumber(Math.abs(value), '#,##0.00');
};

//格式化checkbox
com.formatCheckbox = function (value) {
    var checked = (value || 'false').toString() == 'true';
    return utils.formatString('<img value={0} src="/images/{1}"/>', checked, checked ? "checkmark.gif" : "checknomark.gif");
};

//弹messagee
com.message = function (type, message, callback) {
    switch (type) {
        case "success":
            if (parent == window) return alert(message);
            parent.$('#notity').jnotifyAddMessage({ text: message, permanent: false });
            break;
        case "error":
            if (parent == window) return alert(message);
            parent.$.messager.alert('错误', message);
            break;
        case "warning":
            if (parent == window) return alert(message);
            parent.$('#notity').jnotifyAddMessage({ text: message, permanent: false, type: 'warning' });
            break;
        case "information":
            parent.$.messager.show({
                title: '消息',
                msg: message
                //,showType: 'show'
            });
            break;
        case "confirm":
            return parent.$.messager.confirm('确认', message, callback);
    }

    if (callback) callback();
    return null;
};

com.messageif = function (condition, type, message, callback) {
    if (condition) 
        com.message(type, message, callback);
};

com.openTab = function () {
    parent.wrapper.addTab.apply(this,arguments);
}

com.setLocationHashId = function (id) {
    var hash = parent.location.hash.split('/');
    hash[hash.length-1] = id;
    parent.location.hash = hash.join('/');
};

com.ajax = function (options) {
    options = $.extend({
        showLoading:true
    }, options);

    var ajaxDefaults = {
        type: 'POST',
        dataType: 'json',
        //contentType: 'application/json',
        error: function (e) {
            var msg = e.responseText;
           
            var ret = msg.match(/^{\"Message\":\"(.*)\",\"ExceptionMessage\":\"(.*)\",\"ExceptionType\":.*/);
            if (ret != null) {
                msg = (ret[1] + ret[2]).replace(/\\"/g, '"').replace(/\\r\\n/g, '<br/>').replace(/dbo\./g, '');
            }
            else {
                try{msg = $(msg).text()||msg;}
                catch(ex){}
            }
           
            com.message('error', msg);
        }
    };

    if (options.showLoading) { 
        ajaxDefaults.beforeSend = $.showLoading;
        ajaxDefaults.complete = $.hideLoading;
    }

    $.ajax($.extend(ajaxDefaults, options));
};

com.dialog = function (opts) {
    var query = parent.$, fnClose = opts.onClose;
    opts = query.extend({
        title: 'My Dialog',
        width: 400,
        height: 220,
        closed: false,
        cache: false,
        modal: true,
        html: '',
        url: '',
        viewModel: query.noop
    }, opts);

    opts.onClose = function () {
        if (query.isFunction(fnClose)) fnClose();
        query(this).dialog('destroy');
    };
     
    if (query.isFunction(opts.html))
        opts.html = utils.functionComment(opts.html);
    else if (/^\#.*\-template$/.test(opts.html))
        opts.html = $(opts.html).html();
 
    var win = query('<div></div>').appendTo('body').html(opts.html);
    if (opts.url) 
        query.ajax({async: false,url: opts.url,success: function (d) { win.empty().html(d); }});

    win.dialog(opts);
    query.parser.onComplete = function () {
        if ("undefined" === typeof ko)
            opts.viewModel(win);
        else
            ko.applyBindings(new opts.viewModel(win), win[0]);

        query.parser.onComplete = query.noop;
    };
    query.parser.parse(win);
    return win;
};

com.auditDialog = function () {
    var query = parent.$;
    var winAudit = query('#w_audit_div'), args = arguments;
    if (winAudit.length == 0) {
        var html = '<div id="w_audit_wrapper">'
        html += '    <div id="w_audit_div" class="easyui-dialog"  title="审核" data-options="modal:true,closed:true,iconCls:\'icon-user-accept\'" style="width:400px;height:210px;" buttons="#w_audit_div_button">'
        html += '        <div class="container_16" style="width:90%;margin:5%;">                                                                                  '
        html += '            <div class="grid_3 lbl" style="font-weight: bold;color:#7e7789">审核状态</div>                                                       '
        html += '            <div class="grid_13 val">                                                                                                            '
        html += '                通过<input type="radio" name="AuditStatus" value="passed" data-bind="checked:form.status" style="margin-right:10px;" />          '
        html += '                不通过<input type="radio" name="AuditStatus" value="reject" data-bind="checked:form.status" />                                   '
        html += '            </div>                                                                                                                               '
        html += '            <div class="grid_3 lbl" style="margin-top:5px;font-weight: bold;color:#7e7789" style="font-weight: bold;">审核意见</div>             '
        html += '            <div class="grid_13 val"><textarea style="width:272px;height:60px;" class="z-text" data-bind="value:form.comment" ></textarea></div> '
        html += '            <div class="clear"></div>                                                                                                            '
        html += '        </div>                                                                                                                                   '
        html += '    </div>                                                                                                                                       '
        html += '    <div id="w_audit_div_button" class="audit_button">                                                                                           '
        html += '        <a href="javascript:void(0)" data-bind="click:confirmClick" class="easyui-linkbutton" iconCls="icon-ok" >确定</a>                        '
        html += '        <a href="javascript:void(0)" data-bind="click:cancelClick" class="easyui-linkbutton" iconCls="icon-cancel" >取消</a>                     '
        html += '    </div>                                                                                                                                       '
        html += '</div>';
        var wrapper = query(html).appendTo("body");
        wrapper.find(".easyui-linkbutton").linkbutton();
        winAudit = wrapper.find(".easyui-dialog").dialog();
    }
    
    var viewModel = function() {
        var self = this;
        this.form = {
            status: ko.observable('passed'),
            comment: ko.observable('')
        };
        this.confirmClick = function () {
            winAudit.dialog('close');
            if (typeof args[0] === 'function') {
                args[0].call(this, ko.toJS(self.form));
            }
        };
        this.cancelClick = function () {
            winAudit.dialog('close');
        };
    }

    var node = winAudit.parent()[0];
    winAudit.dialog('open');
    ko.cleanNode(node);
    ko.applyBindings(new viewModel(), node);
};

com.auditDialogForEditVM = function () {
    var query = parent.$;
    var winAudit = query('#w_audit_div'), args = arguments;
    if (winAudit.length == 0) {
        var html = utils.functionComment(function () {/*
            <div id="w_audit_wrapper">
                <div id="w_audit_div" class="easyui-dialog"  title="审核" data-options="modal:true,closed:true,iconCls:'icon-user-accept'" style="width:400px;height:210px;" buttons="#w_audit_div_button"> 
                    <div class="container_16" style="width:90%;margin:5%;">  
                        <div class="grid_3 lbl" style="font-weight: bold;color:#7e7789">审核状态</div>  
                        <div class="grid_13 val">
                            通过审核<input type="radio" name="AuditStatus" value="passed" data-bind="checked:form.status,disable:disabled" style="margin-right:10px;" /> 
                            取消审核<input type="radio" name="AuditStatus" value="reject" data-bind="checked:form.status,disable:disabled" />
                        </div>
                        <div class="grid_3 lbl" style="margin-top:5px;font-weight: bold;color:#7e7789" style="font-weight: bold;">审核意见</div>  
                        <div class="grid_13 val"><textarea style="width:272px;height:60px;" class="z-text" data-bind="value:form.comment" ></textarea></div>
                        <div class="clear"></div>
                    </div> 
                </div> 
                <div id="w_audit_div_button" class="audit_button">  
                    <a href="javascript:void(0)" data-bind="click:confirmClick" class="easyui-linkbutton" iconCls="icon-ok" >确定</a>  
                    <a href="javascript:void(0)" data-bind="click:cancelClick" class="easyui-linkbutton" iconCls="icon-cancel" >取消</a>  
                </div> 
            </div>
            */});
        var wrapper = query(html).appendTo("body");
        wrapper.find(".easyui-linkbutton").linkbutton();
        winAudit = wrapper.find(".easyui-dialog").dialog();
    }

    var viewModel = function () {
        var self = this;
        this.disabled = ko.observable(true);
        this.form = {
            status: args[0].ApproveState() == "passed" ? "reject" : "passed",
            comment: args[0].ApproveRemark()
        };
        this.confirmClick = function () {
            winAudit.dialog('close');
            if (typeof args[1] === 'function') {
                args[0].ApproveState(this.form.status);
                args[0].ApproveRemark(this.form.comment);
                args[1].call(this, ko.toJS(self.form));
            }
        };
        this.cancelClick = function () {
            winAudit.dialog('close');
        };
    }

    var node = winAudit.parent()[0];
    winAudit.dialog('open');
    ko.cleanNode(node);
    ko.applyBindings(new viewModel(), node);
};

com.readOnlyHandler = function (type) {
    //readonly
    _readOnlyHandles = {};
    _readOnlyHandles.defaults = _readOnlyHandles.input = function (obj, b) {
        b ? obj.addClass("readonly").attr("readonly", true) : obj.removeClass("readonly").removeAttr("readonly");
    };
    _readOnlyHandles.combo = function (obj, b) {
        var combo = obj.data("combo").combo;
        _readOnlyHandles.defaults(combo.find(".combo-text"), b);
        if (b) {
            combo.unbind(".combo");
            combo.find(".combo-arrow,.combo-text").unbind(".combo");
            obj.data("combo").panel.unbind(".combo");
        }
    };
    _readOnlyHandles.spinner = function (obj, b) {
        _readOnlyHandles.defaults(obj, b);
        if (b) {
            obj.data("spinner").spinner.find(".spinner-arrow-up,.spinner-arrow-down").unbind(".spinner");
        }
    };
    return _readOnlyHandles[type || "defaults"];
};

com.loadCss = function (url, doc, reload) {
    var links = doc.getElementsByTagName("link");
    for (var i = 0; i < links.length; i++)
        if (links[i].href.indexOf(url) > -1) {
            if (reload)
                links[i].parentNode.removeChild(links[i])
            else 
                return;
        }
    var container = doc.getElementsByTagName("head")[0];
    var css = doc.createElement("link");
    css.rel = "stylesheet";
    css.type = "text/css";
    css.media = "screen";
    css.href = url;
    container.appendChild(css);
};

//add by sun 2017/5/24 19:06
com.parser = function (s) {
    if (!s) { return null; }
    var ss = s.split('-');
    var y = parseInt(ss[0], 10);
    var m = parseInt(ss[1], 10);
    var d = parseInt(ss[2], 10);
    if (!isNaN(y) && !isNaN(m) && !isNaN(d)) {
        return new Date(y, m - 1, d);
    } else {
        return new Date();
    }
};
com.getCookie = function (name) {
    //document.cookie.setPath("/");  
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg)) {
        return unescape(arr[2]);
    }
    else {
        return null;
    }
};
//设置cookie值  
com.setCookie = function (name, value) {
    //document.cookie.setPath("/");  
    var hour = 8;
    var exp = new Date();
    exp.setTime(exp.getTime() + hour * 60 * 60 * 1000);
    document.cookie = name + "=" + escape(value) + ";expires=" + exp.toGMTString() + ";path=/";
};
//删除cookies
com.delCookie = function (name) {
    var exp = new Date();
    exp.setTime(exp.getTime() - 1); 
    var cval = com.getCookie(name); 
    if (cval != null)
        document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
};
//add by ym 2017/6/21 11:59
/*实现DateAdd功能*/
function DateAdd(interval, number, date) {

    number = parseInt(number);
    if (typeof (date) == "string") {
        //date = date.split(/\D/);
        //date[1];
        //eval("var date = new Date(" + date.join(",") + ")");
        date = date.replace(/-/g, "/");
       var  date = new Date(date);
    }
    if (typeof (date) == "object") {
        var date = date
    }
    switch (interval) {
        case "y": {
            date.setFullYear(date.getFullYear() + number);
            return date;
            break;
        }
        case "q": {
            date.setMonth(date.getMonth() + number * 3);
            return date;
            break;
        }
        case "m": {
            date.setMonth(date.getMonth() + number);
            return date;
            break;
        }
        case "w": {
            date.setDate(date.getDate() + number * 7);
            return date;
            break;
        }
        case "d": {
            date.setDate(date.getDate() + number);
            return date;
            break;
        }
        case "hh": {
            date.setHours(date.getHours() + number);
            return date;
            break;
        }
        case "mm": {
            date.setMinutes(date.getMinutes() + number);
            return date;
            break;
        }
        case "ss": {
            date.setSeconds(date.getSeconds() + number);
            return date;
            break;
        }
        default: {
            date.setDate(date.getDate() + number);
            return date;
            break;
        }
    }
}
//add by ym 2017/6/21 11:59
/*保留2位小数*/
function changevalue(val, row) {
    //var s = "";
    //if ( val != null) {
        
    //        var f = parseFloat(val);
    //        if (isNaN(f)) {
    //            return "";
    //        }
    //        var f = Math.round(val * 100) / 100;
    //        s = f.toString();
    //        var rs = s.indexOf('.');
    //        if (rs < 0) {
    //            rs = s.length;
    //            s += '.';
    //        }
    //        while (s.length <= rs + 2) {
    //            s += '0';
            
    //    }
    //}
    //return s;
    return toDecimal(val, 2);
}
//add by ym 2017/6/21 11:59
/*保留1位小数*/
function Round1(val, row) {
    //var s = "";
    //if ( val != null ) {
    //    var f = parseFloat(val);
    //    if (isNaN(f)) {
    //        return "";
    //    }
    //    var f = Math.round(val * 10) / 10;
    //    s = f.toString();
    //    var rs = s.indexOf('.');
    //    if (rs < 0) {
    //        rs = s.length;
    //        s += '.';
    //    }
    //    while (s.length <= rs + 1) {
    //        s += '0';
    //    }
    //}
    //return s;
    return toDecimal(val, 1);
}
function Round1New(val, row) {
    var s = "";
    if ( val != null ) {
        var f = parseFloat(val);
        if (isNaN(f) ||  f<0) {
            return "";
        }
        var f = Math.round(val * 10) / 10;
        s = f.toString();
        var rs = s.indexOf('.');
        if (rs < 0) {
            rs = s.length;
            s += '.';
        }
        while (s.length <= rs + 1) {
            s += '0';
        }
    }
    return s;
 }
function toDecimal(x, dotnum) {
    
    if (x) {
        if (isNaN(x)) {
            return "";
        }
        var num = parseFloat(x);

        var f = Math.round(num * Math.pow(10, dotnum)) / Math.pow(10, dotnum);
        var s = f.toString();
        var rs = s.indexOf('.');
        if (rs < 0) {
            rs = s.length;
            s += '.';
        }
        while (s.length <= rs + dotnum) {
            s += '0';
        }
        //console.log(x + "," + s);
        return s;
    }
    return "";
}
 
//add by ym 2017/8/15 14:20
/*保留有效数*/
/*v值*/
/*sgnDgt保留有效数字个数*/
function changeQvalue(val, row) {
    rval = SaveThreeN(val, 3);
    return rval;
}
/*保留N位有效数字
  add by sun 2017/8/26
*/
function SaveThreeN(v, sgnDgt) {

    if (v == null) {
        return "";
    }
    var d = parseFloat(v);
    if (isNaN(d)) {
        return "";
    }
    var forated = d.toPrecision(sgnDgt);
    if (forated.indexOf(".") != -1) {
        if (forated.length > sgnDgt + 1) {
            return Number(forated);
        }
    } else {
        if (forated.length > sgnDgt) {
            return Number(forated);
        }
    }
    return forated;
     
}
//add by ym 2017/6/28 14:20
/**
   * EasyUI DataGrid根据字段动态合并单元格
   * 参数 tableID 要合并table的id
   * 参数 colList 要合并的列,用逗号分隔(例如："name,department,office");
    */
function mergeCellsByField(tableID, colList) {
    var ColArray = colList.split(",");
    var tTable = $("#" + tableID);
    var TableRowCnts = tTable.datagrid("getRows").length;
    var tmpA;
    var tmpB;
    var PerTxt = "";
    var CurTxt = "";
    var alertStr = "";
    for (j = ColArray.length - 1; j >= 0; j--) {
        PerTxt = "";
        tmpA = 1;
        tmpB = 0;

        for (i = 0; i <= TableRowCnts; i++) {
            if (i == TableRowCnts) {
                CurTxt = "";
            }
            else {
                CurTxt = tTable.datagrid("getRows")[i][ColArray[j]];
            }
            if (PerTxt == CurTxt) {
                tmpA += 1;
            }
            else {
                tmpB += tmpA;

                tTable.datagrid("mergeCells", {
                    index: i - tmpA,
                    field: ColArray[j],　　//合并字段
                    rowspan: tmpA,
                    colspan: null
                });
                tTable.datagrid("mergeCells", { //根据ColArray[j]进行合并
                    index: i - tmpA,
                    field: "Ideparture",
                    rowspan: tmpA,
                    colspan: null
                });

                tmpA = 1;
            }
            PerTxt = CurTxt;
        }
    }
}
//add by ym 2017/6/30 17:20
///手动给datagrid添加分页数据data为表格的数据
function pagerFilter(data) {

    if (typeof data.length == 'number' && typeof data.splice == 'function') {	// is array
        data = {
            total: data.length,
            rows: data
        }
    }
    var dg = $(this);
    var opts = dg.datagrid('options');
    var pager = dg.datagrid('getPager');
    pager.pagination({
        onSelectPage: function (pageNum, pageSize) {
            opts.pageNumber = pageNum;
            opts.pageSize = pageSize;
            pager.pagination('refresh', {
                pageNumber: pageNum,
                pageSize: pageSize
            });
            dg.datagrid('loadData', data);
        }
    });
    if (!data.originalRows) {
        data.originalRows = (data.rows);
    }
    var start = (opts.pageNumber - 1) * parseInt(opts.pageSize);
    var end = start + parseInt(opts.pageSize);
    data.rows = (data.originalRows.slice(start, end));
    return data;
}
//add by ym 2017/7/18 
/*显示站类*/
function changesttp(val, row) {
    if (val != "" && val != null && val != undefined) {
        switch (val) {
            case "PP":
                val = "雨量站";
                break;
            case "DD":
                val = "堰闸水文站";
                break;
            case "RR":
                val = "水库水文站";
                break;
            case "ZZ":
                val = "河道水位水文站";
                break;
            case "ZQ":
                val = "河道水文站";
                break;
            case "MM":
                val = "气象站";
                break;
            case "BB":
                val = "蒸发站";
                break;
            case "DP":
                val = "泵站";
                break;
            case "ZG":
                val = "地下水";
                break;
            case "SS":
                val = "墒情站";
          }
       
    }
    else {
        val = "";
    }
    return val;
}
//add by ym 2017/7/19 
/*根据数字显示天气中文*/
function changewather(val, row) {
    if (val != "" && val != null && val != undefined) {
        switch (val) {
            case "5":
                val = "雪";
                break;
            case "6":
                val = "雨夹雪";
                break;
            case "7":
                val = "雨";
                break;
            case "8":
                val = "阴";
                break;
            case "9":
                val = "晴";
                break;


        }

    }
    else {
        val = "";
    }
    return val;
}
//add by ym 2017/7/19 
/*根据数字显示水势中文*/
function changewptn(val, row) {
    if (val != "" && val != null && val != undefined) {
        switch (val) {
            case "4":
                val = "<img src=\"/images/j.gif\">";
                break;
            case "5":
                val = "<img src=\"/images/s.gif\">";
                break;
            case "6":
                val = "<img src=\"/images/p.gif\">";
                break;
      }

    }
    else {
        val = "缺测";
    }
    return val;
}
//add by ym 2017/7/19 
/*根据数字显示堰闸闸水特征*/
function changeswchrcd(val, row) {
    if (val != "" && val != null && val != undefined) {
        switch (val) {
            case "1":
                val = "干涸";
                break;
            case "2":
                val = "断流";
                break;
            case "3":
                val = "流向不定";
                break;
            case "4":
                val = "逆流";
                break;
            case "5":
                val = "起涨";
                break;
            case "6":
                val = "洪峰";
                break; 
            case "P":
                val = "水电厂发电流量";
                break; 
        }

    }
    else {
        val = "";
    }
    return val;
}
/*转换UTC*/
com.ToUTC =function (date) {
    date = date.replace(/-/g, "/");
    var dt = new Date(date); //StringToDate(date);
    
    return Date.UTC(dt.getFullYear(), dt.getMonth(), dt.getDate(), dt.getHours(), dt.getMinutes());
}
//add by ym 2017-11-01
//按照浮点数真实大小排序
function numberSort(a, b) {
    var number1 = parseFloat(a);
    var number2 = parseFloat(b);

    return (number1 > number2 ? 1 : -1);
}
//点击站点弹出站点详细的弹窗
function formatstcdOper(stcd) {
    //com.dialog({
    //    title: "查看站点基本信息",
    //    url: "/SQWEB/Home/STCDInfo?stcd=" + stcd,
    //    width: 750,
    //    height: 450
    //});
    _customopen("查看站点基本信息", "/SQWEB/Home/STCDInfo?stcd=" + stcd, 750, 450);
}
//超警戒水位正值为红色，负值为蓝色
function changecolor(val, row) {
    if (val > 0)
        return 'color:red';
    else
        return 'color:blue';
}
//超警戒、汛限水位(大于0在前面放个加号，否则是减号)
function changesupervalue(val) {   
    if (val) {
        if (isNaN(val)) {
            return "";
        }
        var num = parseFloat(val);

        var f = Math.round(num * Math.pow(10, 2)) / Math.pow(10, 2);
        var s = f.toString();
        var rs = s.indexOf('.');
        if (rs < 0) {
            rs = s.length;
            s += '.';
        }
        while (s.length <= rs + 2) {
            s += '0';
        }
        if (s > 0)
        {
            return "+" + s;
        }
        else
            return s;
    }
    return "";
}
//post提交参数并打开新页面
function openPostWindow(url, params) {
    var tempForm = document.createElement("form");
    tempForm.id = "tempForm1";
    tempForm.method = "post";
    tempForm.action = url;
    tempForm.target = "_blank"; //打开新页面
    for (var key in params) {
        var hideInput1 = document.createElement("input");
        hideInput1.type = "hidden";
        hideInput1.name = key; //后台要接受这个参数来取值
        hideInput1.value = params[key]; //后台实际取到的值
        tempForm.appendChild(hideInput1);
    }
    if (document.all) {
        tempForm.attachEvent("onsubmit", function () { });        //IE
    } else {
        var subObj = tempForm.addEventListener("submit", function () { }, false);    //firefox
    }
    document.body.appendChild(tempForm);
    //if (document.all) {
    //    tempForm.fireEvent("onsubmit");
    //} else {
    //    tempForm.dispatchEvent(new Event("submit"));
    //}
    tempForm.submit();
    document.body.removeChild(tempForm);
}
//----------------------------------------------------------------
//功能：去除字符串右边空格
//----------------------------------------------------------------
function rTrim(s) {
    s = s + "";
    return s.replace(/\s*$/, "");
}

//----------------------------------------------------------------
//功能：去除字符串左边空格
//----------------------------------------------------------------
function lTrim(s) {
    s = s + "";
    return s.replace(/^\s*/, "");
}

//----------------------------------------------------------------
//功能：去除字符串左右边空格
//---------------------------------------------------------------- 
function trim(s) {
    if (s == null) {
        return "";
    }
    else {
        return rTrim(lTrim(s));
    }
}
//----------------------------------------------------------------
//功能：获取当前URL中传递的参数
//----------------------------------------------------------------
function GetURLParameter(name) {
    var url = window.location.href;

    var strs, s;

    if (url.indexOf("?") != -1) {
        var str = url.substr(1)
        strs = str.split("?");
        strs = strs[1].split("&");
        for (i = 0; i < strs.length; i++) {
            if ([strs[i].split("=")[0]] == name) { s = unescape(strs[i].split("=")[1]); }
        }
    }
    return s;
}
(function ($) {
    $.getUrlParam = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return r[2]; return null;//return unescape(r[2]); return null;
    }
})(jQuery);
