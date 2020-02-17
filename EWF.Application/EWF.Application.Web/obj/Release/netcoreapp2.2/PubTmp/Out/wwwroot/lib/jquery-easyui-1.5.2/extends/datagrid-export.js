(function ($) {
    function getRows(target) {
        var state = $(target).data('datagrid');
        if (state.filterSource) {
            return state.filterSource.rows;
        } else {
            return state.data.rows;
        }
    }
    function toHtml(target, rows) { 
        rows = rows || getRows(target);
        
        var dg = $(target);
        var data = ['<table border="1" rull="all" style="border-collapse:collapse">'];
        var fields = dg.datagrid('getColumnFields', true).concat(dg.datagrid('getColumnFields', false));
        var trStyle = 'height:32px';
        var tdStyle0 = 'vertical-align:middle;padding:0 4px;';
        data.push('<tr style="' + trStyle + '">');
        var objFormatter = {};
        var objAlign = {};
        for (var i = 0; i < fields.length; i++) {
            var col = dg.datagrid('getColumnOption', fields[i]);
            if (col.formatter) {
                objFormatter[fields[i]] = col.formatter;
            }
            if (col.align) {
                objAlign[fields[i]] = col.align;
            }
            var tdStyle = tdStyle0 + ';width:' + col.boxWidth + 'px;';
            data.push('<th style="' + tdStyle + '">' + col.title + '</th>');
        }
        data.push('</tr>');
        $.map(rows, function (row) {
            data.push('<tr style="' + trStyle + '">');
            for (var i = 0; i < fields.length; i++) {
                var field = fields[i];
               /*
                * 根据单元格设置的格式化方法，格式化导出后的单元格值 BEGIN
                *  add by SUN
                *  Date:2019-08-08 17:00
                * */
                var tdstyleV = tdStyle0;
                var cellV;
                
                if (objFormatter.hasOwnProperty(field) && typeof objFormatter[field] == "function") {
                    cellV = objFormatter[field].call(this, row[field], row);
                } else {
                    cellV = (row[field] == null ? "" : row[field])
                }
                //针对数值类型，有保留1位小数，2位小数，或3位有效数字的，按照原格式化后字符格式输出，保持数值的所见即所得，并在excel单元格中使数值格式仍为数值格式(可以直接进行数学运算)
                if (!isNaN(cellV)) {
                    var decimalL = GetDecimalLength(cellV);
                    var numfromat = "mso-number-format:0";
                    for (var k = 0; k < decimalL; k++) {
                        if (k == 0) {
                            numfromat += "\\.0";
                        } else {
                            numfromat += "0";
                        }

                    }
                    tdstyleV += numfromat + ";";
                }
                //根据原表中单元格对齐方式设置导出excel后单元格对齐方式
                if (objAlign.hasOwnProperty(field)) {
                    if (objAlign[field] == "center") {
                        tdstyleV += "text-align:center;";
                    } else if (objAlign[field] == "right") {
                        console.log("field:" + field);
                        tdstyleV += "text-align:right;";
                    }
                    
                }
                //根据单元格设置的格式化方法，格式化导出后的单元格值 END
                data.push(
                    '<td style="' + tdstyleV + '">' + cellV + '</td>'
                );

            }
            data.push('</tr>');
        });
        data.push('</table>');
        return data.join('');
    }
    /**
     * 获取数值的小数位数，整数返回0
     * add by SUN
     * Date:2019-08-08 17:00
     * @param {any} num
     */
    function GetDecimalLength(num) {
        var dotIndex = String(num).indexOf('.') + 1; //小数点位置
        if (dotIndex != 0) {
            var decimalLength = String(num).length - dotIndex;
            if (decimalLength > 0) {
                return decimalLength;
            }
        }
        return 0;
    }
    function toArray(target, rows) {
        rows = rows || getRows(target);
        var dg = $(target);
        var fields = dg.datagrid('getColumnFields', true).concat(dg.datagrid('getColumnFields', false));
        var data = [];
        var r = [];
        for (var i = 0; i < fields.length; i++) {
            var col = dg.datagrid('getColumnOption', fields[i]);
            r.push(col.title);
        }
        data.push(r);
        $.map(rows, function (row) {
            var r = [];
            for (var i = 0; i < fields.length; i++) {
                r.push(row[fields[i]]);
            }
            data.push(r);
        });
        return data;
    }

    function print(target, param) {
        var title = null;
        var rows = null;
        if (typeof param == 'string') {
            title = param;
        } else {
            title = param['title'];
            rows = param['rows'];
        }
        var newWindow = window.open('', '', 'width=800, height=500');
        var document = newWindow.document.open();
        var content =
            '<!doctype html>' +
            '<html>' +
            '<head>' +
            '<meta charset="utf-8">' +
            '<title>' + title + '</title>' +
            '</head>' +
            '<body>' + toHtml(target, rows) + '</body>' +
            '</html>';
        document.write(content);
        document.close();
        newWindow.print();
    }

    function b64toBlob(data) {
        var sliceSize = 512;
        var chars = atob(data);
        var byteArrays = [];
        for (var offset = 0; offset < chars.length; offset += sliceSize) {
            var slice = chars.slice(offset, offset + sliceSize);
            var byteNumbers = new Array(slice.length);
            for (var i = 0; i < slice.length; i++) {
                byteNumbers[i] = slice.charCodeAt(i);
            }
            var byteArray = new Uint8Array(byteNumbers);
            byteArrays.push(byteArray);
        }
        return new Blob(byteArrays, {
            type: ''
        });
    }

    function toExcel(target, param) {
        var filename = null;
        var rows = null;
        var worksheet = 'Worksheet';
        if (typeof param == 'string') {
            filename = param;
        } else {
            filename = param['filename'];
            rows = param['rows'];
            worksheet = param['worksheet'] || 'Worksheet';
        }
        var dg = $(target);
        var uri = 'data:application/vnd.ms-excel;base64,'
            , template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><meta http-equiv="content-type" content="application/vnd.ms-excel; charset=UTF-8"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body>{table}</body></html>'
            , base64 = function (s) { return window.btoa(unescape(encodeURIComponent(s))) }
            , format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) }

        var table = toHtml(target, rows);
        var ctx = { worksheet: worksheet, table: table };
        var data = base64(format(template, ctx));
        if (window.navigator.msSaveBlob) {
            var blob = b64toBlob(data);
            window.navigator.msSaveBlob(blob, filename);
        } else {
            var alink = $('<a style="display:none"></a>').appendTo('body');
            alink[0].href = uri + data;
            alink[0].download = filename;
            alink[0].click();
            alink.remove();
        }
    }

    $.extend($.fn.datagrid.methods, {
        toHtml: function (jq, rows) {
            return toHtml(jq[0], rows);
        },
        toArray: function (jq, rows) {
            return toArray(jq[0], rows);
        },
        toExcel: function (jq, param) {
            return jq.each(function () {
                toExcel(this, param);
            });
        },
        print: function (jq, param) {
            return jq.each(function () {
                print(this, param);
            });
        }
    });
})(jQuery);
