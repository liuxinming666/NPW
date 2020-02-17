(function ($) {
    var printAreaCount = 0;    $.fn.printArea = function () { 
        var ele = $(this);        var idPrefix = "printArea_";        removePrintArea(idPrefix + printAreaCount);        printAreaCount++;        var iframeId = idPrefix + printAreaCount;        var iframeStyle = 'position:absolute;width:0px;height:0px;left:-500px;top:-500px;';        iframe = document.createElement('IFRAME');        $(iframe).attr({
            style: iframeStyle,            id: iframeId
        });        document.body.appendChild(iframe);                var doc = iframe.contentWindow.document;        //doc.designMode = "on";          //doc.open();        doc.write('<style type="text/css">.pb{width:100%;border-collapse: collapse;} .pb tr td{border:1px solid #ccc} .pb tr th{border:1px solid #ccc;background:#F2F2F2; }</style>');        //$(document).find("link").filter(function () {
        //    return $(this).attr("rel").toLowerCase() == "stylesheet";
        //}).each(        //        function () {
        //            doc.write('<link type="text/css" rel="stylesheet" href="'        //                    + $(this).attr("href") + '" >');
        //        });        //iframe.contentDocument.documentElement.innerHTML = CreateFormPage("打印测试", $(this));                doc.write(CreateFormPage("打印测试", $(this)));        doc.close();        //doc.designMode = "off";        var frameWindow = iframe.contentWindow;        frameWindow.close();        frameWindow.focus();        frameWindow.print();
    }    var removePrintArea = function (id) {
        $("iframe#" + id).remove();
    };    // strPrintName 打印任务名    // printDatagrid 要打印的datagrid    function CreateFormPage(strPrintName, printDatagrid) {
        var tableString = '<table cellspacing="0" class="pb">';        var frozenColumns = printDatagrid.datagrid("options").frozenColumns;  // 得到frozenColumns对象        var columns = printDatagrid.datagrid("options").columns;    // 得到columns对象        var nameList = '';        // 载入title        if (typeof columns != 'undefined' && columns != '') {
            $(columns).each(function (index) {
                tableString += '\n<tr>';                if (typeof frozenColumns != 'undefined' && typeof frozenColumns[index] != 'undefined') {
                    for (var i = 0; i < frozenColumns[index].length; ++i) {
                        if (!frozenColumns[index][i].hidden) {
                            tableString += '\n<th width="' + frozenColumns[index][i].width + '"';                            if (typeof frozenColumns[index][i].rowspan != 'undefined' && frozenColumns[index][i].rowspan > 1) {
                                tableString += ' rowspan="' + frozenColumns[index][i].rowspan + '"';
                            }                            if (typeof frozenColumns[index][i].colspan != 'undefined' && frozenColumns[index][i].colspan > 1) {
                                tableString += ' colspan="' + frozenColumns[index][i].colspan + '"';
                            }                            if (typeof frozenColumns[index][i].field != 'undefined' && frozenColumns[index][i].field != '') {
                                nameList += ',{"f":"' + frozenColumns[index][i].field + '", "a":"' + frozenColumns[index][i].align + '"}';
                            }                            tableString += '>' + frozenColumns[0][i].title + '</th>';
                        }
                    }
                }                for (var i = 0; i < columns[index].length; ++i) {
                    if (!columns[index][i].hidden) {
                        tableString += '\n<th width="' + columns[index][i].width + '"';                        if (typeof columns[index][i].rowspan != 'undefined' && columns[index][i].rowspan > 1) {
                            tableString += ' rowspan="' + columns[index][i].rowspan + '"';
                        }                        if (typeof columns[index][i].colspan != 'undefined' && columns[index][i].colspan > 1) {
                            tableString += ' colspan="' + columns[index][i].colspan + '"';
                        }                        if (typeof columns[index][i].field != 'undefined' && columns[index][i].field != '') {
                            nameList += ',{"f":"' + columns[index][i].field + '", "a":"' + columns[index][i].align + '"}';
                        }                        tableString += '>' + columns[index][i].title + '</th>';
                    }
                }                tableString += '\n</tr>';
            });
        }        // 载入内容        var rows = printDatagrid.datagrid("getRows"); // 这段代码是获取当前页的所有行        var nl = eval('([' + nameList.substring(1) + '])');        for (var i = 0; i < rows.length; ++i) {
            tableString += '\n<tr>';            $(nl).each(function (j) {
                var e = nl[j].f.lastIndexOf('_0');                tableString += '\n<td';                if (nl[j].a != 'undefined' && nl[j].a != '') {
                    tableString += ' style="text-align:' + nl[j].a + ';"';
                }                tableString += '>';                //if (e + 2 == nl[j].f.length) {
                //    tableString += (rows[i][nl[j].f.substring(0, e)]==null?"":rows[i][nl[j].f.substring(0, e)]);
                //}                //else                tableString += (rows[i][nl[j].f] == null ? "" : rows[i][nl[j].f].toString().replace("T", " "));                tableString += '</td>';
            });            tableString += '\n</tr>';
        }        tableString += '\n</table>';        return tableString;
    }
})(jQuery);