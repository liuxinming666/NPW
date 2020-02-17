function ExportExcel(dg) {
    var columns = dg.datagrid("options").columns;
    var frzenColumns = dg.datagrid("options").frozenColumns;
    console.log(dg.datagrid("getData"));
    var data = GetTableData(dg);
    var obj = { title: "测试", gdata: data, gcols: JSON.stringify(columns), frzenCol: JSON.stringify(frzenColumns) };

    com.ajax({
        type: 'post',
        url: '/Home/ExportExcelFile',
        dataType: "json",
        contentType: 'application/json',
        data: JSON.stringify(obj),
        success: function (data) {

            if (data.fileName != undefined) {
                window.location.href = "/Home/Download/?path=" + data.fileName;
            } else {
                alert("导出数据失败！");
            }

        }
    });
    //$.post('/Home/ExportExcelFile', obj, function (data) {
    //    if (data.fileName != undefined) {
    //        window.location.href = "/Home/Download/?path=" + data.fileName;
    //    } else {
    //        alert("导出数据失败！");
    //    }
        
    //}, "json");
    
}

function GetTableData(printDatagrid) {
    var tmp = { "total": 0, "rows": [] };

    var rows = printDatagrid.datagrid("getRows");
    tmp.total = rows.length;
    /// <summary>
    /// 提交DataGrid数据
    /// </summary>
    var frozenColumns = printDatagrid.datagrid("options").frozenColumns;  // 得到frozenColumns对象 
    var columns = printDatagrid.datagrid("options").columns;  // 得到frozenColumns对象 

    columns = (frozenColumns[0] || []).concat(columns[0] || []).concat(columns[1] || []);
    $.map(rows, function (r) {
        var rr = {};
        $.map(columns, function (c) {
            var field;
            if (!c.hidden && c.field) {
                field = c.field;
                if (c.formatter) {
                    var v = c.formatter.call(this, r[c.field], r) || "";
                    if (validateHtmlTag(v)) {
                        v = $(v).text()
                    }
                    if (v.toString().indexOf("<a") >= 0)
                        rr[field] = fixGetValue(r[c.field]);
                        //tmp.push(fixGetValue(r[c.field]));
                    else
                        rr[field] = v;
                    //tmp.push(v);
                } else {
                    rr[field] = fixGetValue(r[c.field]);
                    //tmp.push(fixGetValue(r[c.field]));
                }
            }
        });
        if (JSON.stringify(rr) != "{}") {
            tmp.rows.push(rr);
        }

    });

    return JSON.stringify(tmp);
}

function validateHtmlTag(str) {//验证字符串中是否存在html的标签  
    var reg = /<(?:(?:\/?[A-Za-z]\w*\b(?:[=\s](['"]?)[\s\S]*?\1)*)|(?:!--[\s\S]*?--))\/?>/g;//验证规则  
    return str.match(reg);
}
function ChangeToTable(printDatagrid) {
    /// <summary>
    /// 提交DataGrid数据
    /// </summary>
    var frozenColumns = printDatagrid.datagrid("options").frozenColumns;  // 得到frozenColumns对象 
    var columns = printDatagrid.datagrid("options").columns;  // 得到frozenColumns对象 

    columns = (frozenColumns[0] || []).concat(columns[0] || []).concat(columns[1] || []);

    var rows = printDatagrid.datagrid("getRows");
    var data = new Array();
    var tmp = new Array();
    //列名
    $.map(columns, function (a) {
        if (!a.hidden && a.field) {
            tmp.push(a.title);
        }
    });
    data.push(tmp);
    $.map(rows, function (r) {
        tmp = new Array();
        $.map(columns, function (c) {
            if (!c.hidden && c.field) {
                if (c.formatter) {
                    var v = c.formatter.call(this, r[c.field], r) || "";
                    v = $(v).text()
                    if (v.toString().indexOf("<a") >= 0)
                        tmp.push(fixGetValue(r[c.field]));
                    else
                        tmp.push(v);
                } else {
                    tmp.push(fixGetValue(r[c.field]));
                }
            }
        });
        data.push(tmp);
    });
    return JSON.stringify(data);
}
function ChangeToTableNormal(printDatagrid) {
    /// <summary>
    /// 提交DataGrid数据
    /// </summary>
    var frozenColumns = printDatagrid.datagrid("options").frozenColumns;  // 得到frozenColumns对象 
    var columns = printDatagrid.datagrid("options").columns;  // 得到frozenColumns对象 
    columns = (frozenColumns[0] || []).concat(columns[0] || []).concat(columns[1] || []);
    var rows = printDatagrid.datagrid("getRows");
    var data = new Array();
    var tmp = new Array();
    //列名
    $.map(columns, function (a) {
        if (!a.hidden && a.field) {
            tmp.push(a.title);
        }
    });
    data.push(tmp);
    $.map(rows, function (r) {
        tmp = new Array();
        $.map(columns, function (c) {
            if (!c.hidden && c.field) {
                if (c.formatter) {
                    var v = c.formatter.call(this, r[c.field], r) || "";
                    v = $(v).text()
                    if (v.toString().indexOf("<a") >= 0)
                        tmp.push(fixGetValue(r[c.field]));
                    else
                        tmp.push(v);
                } else {
                    tmp.push(fixGetValue(r[c.field]));
                }
            }
        });
        data.push(tmp);
    });
    return JSON.stringify(data);
}
function fixGetValue(obj) {
    return obj ? obj : "";
}