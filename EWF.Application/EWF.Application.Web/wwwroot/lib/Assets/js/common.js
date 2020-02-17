jQuery(document).ready(function () { 
    if (window.top != window.self) {
        layer = top.layer;
    }
});
function fix_url(url) {
    var ss = url.split('?');
    url = ss[0] + "?";
    for (var i = 1; i < ss.length; i++) {
        url += ss[i] + "&";
    }
    if (ss.length > 0) {
        url = url.substring(0, url.length - 1);
    }
    return url;
}
function _winopen(title, content_url, width, height, post_url, callback) {
    layer.open({
        type: 2,
        title: title,
        content: content_url,
        area: [width + 'px', height + 'px'],
        fixed: false, //不固定
        maxmin: false,
        success: function (layero, index) {
            layer.iframeAuto(index);
        },
        btn: ['确定', '关闭'],
        yes: function (index, layero) {
            var vars = $(layero).find("iframe")[0].contentWindow.formData();
            if (vars) {
                sendAjax(post_url, vars, function (data) {
                    if (data.state) {
                        ui_alert(data.message, function () {
                            if (callback) {
                                callback(data);
                            } else {
                                location.reload(true);
                            }
                            layer.close(index);
                        });
                    } else {
                        ui_error(data.message);
                    }
                });
            }
        },
        btn2: function (index) {
            layer.close(index);
        },
        closeBtn: 0
    });
}
function _customopen(title, url, width, height) {
    var index = layer.open({
        type: 2,
        title: title,
        content: url,
        area: [width + 'px', height + 'px'],
        fixed: false, //不固定
        maxmin: false,
        success: function (layero, index) {
            layer.iframeAuto(index);
           
        }
    });
}
function _fullopen(title, url) {
    var index = layer.open({
        type: 2,
        title: title,
        content: url
    });
    layer.full(index);
}
/* 关闭弹出窗口*/
function layerclose() {
    var index = parent.layer.getFrameIndex(window.name); //获取窗口索引
    parent.layer.close(index);
}
/* ajax提交*/
function sendAjax(url, vars, callback) {
    return $.ajax({
        type: "POST",
        url: url,
        data: vars + "&ajax=1",
        dataType: "json",
        success: callback
    });
}
function ui_info(msg, callback) {
    layer.msg(msg, {
        icon: 1,
        area: ['200px', '80px'],
        time: 1500 //3秒关闭（如果不配置，默认是3秒）
    }, function (index) {
        if (callback != undefined) {
            callback();
        }
        layer.close(index);
    });
}

function ui_error(msg) {
    layer.alert(msg, {
        icon: 0
    });
}
function ui_alert(msg, callback) {
    layer.alert(
        msg,
        { icon: 1 },
        function (index) {
            if (callback != undefined) {
                callback();
            }
            layer.close(index);
        });
}
function ui_confirm(msg, callback) {
    layer.confirm(msg, {
        icon: 4,
        btn: ['确定', '取消']
    }, function (index, layero) {
        callback();
    }, function (index) {
        layer.close(index);
    });
}
function loadmsg(msg) {
    layer.msg(msg, { icon: 16, shade: [0.8, '#393D49'], time: 0, shadeClose: false });
}
function _undef(val, defaultVal) {
    return val === undefined ? defaultVal : val;
}
/*提交表单*/
function sendForm(formId, post_url, return_url) {
    if (check_form(formId)) {
        //绑定beforeunload事件
        $(window).unbind('beforeunload', null);
        if ($("#ajax").val() == 1) {
            var vars = $("#" + formId).serialize();
            $.ajax({
                type: "POST",
                url: post_url,
                data: vars,
                dataType: "json",
                success: function (data) {
                    if (data.state) {
                        if (data.message) {
                            ui_alert(data.message, function () {
                                if (return_url) {
                                    location.href = return_url;
                                }
                            });
                        } else {
                            if (return_url) {
                                location.href = return_url;
                            }
                        }

                    } else {
                        ui_error(data.message);
                    }
                }
            });
        } else {
            $("#" + formId).attr("action", post_url);
            $("#" + formId).submit();
        }
    }
}
function sendForm2(options) {
    var formId = options.form_Id;
    var post_url = options.post_url;
    var return_url = options.return_url;
    var load_msg = _undef(options.load_msg, "加载中");
    var is_finalclose = _undef(options.final_close, true);
    if (check_form(formId)) {
        //绑定beforeunload事件
        $(window).unbind('beforeunload', null);
        var msgindex = loadmsg(load_msg);
        if ($("#ajax").val() == 1) {
            var vars = $("#" + formId).serialize();
            $.ajax({
                type: "POST",
                url: post_url,
                data: vars,
                dataType: "json",
                success: function (data) {
                    if (data.state) {
                        if (data.message) {
                            ui_alert(data.message, function () {
                                if (return_url) {
                                    location.href = return_url;
                                }
                            });
                        } else {
                            if (return_url) {
                                location.href = return_url;
                            }
                        }

                    } else {
                        ui_error(data.message);
                    }
                },
                complete: function () {
                    layer.close(msgindex);
                }
            });
        } else {
            $("#" + formId).attr("action", post_url);
            $("#" + formId).submit();
        }
    }
}
/* 验证数据类型*/
function validate(data, datatype) {
    var tmp, data2;
    if (datatype.indexOf("|")) {
        tmp = datatype.split("|");
        datatype = tmp[0];
        data2 = tmp[1];
    }
    switch (datatype) {
        case "require":
            if (data == "") {
                return false;
            } else {
                return true;
            }
            break;
        case "email":
            var reg = /^([0-9A-Za-z\-_\.]+)@([0-9a-z]+\.[a-z]{2,3}(\.[a-z]{2})?)$/g;
            return reg.test(data);
            break;
        case "number":
            var reg = /^[0-9]+\.{0,1}[0-9]{0,3}$/;
            return reg.test(data);
            break;
        case "html":
            var reg = /<...>/;
            return reg.test(data);
            break;
        case "eqt":
            data2 = $("#" + data2).val();
            return data == data2;
            break;
    }
}
function check_form(form_id) {
    var last_submit = $('#' + form_id).data('last_submit');
    var current_submit = new Date().getTime();
    if (last_submit == undefined) {
        $('#' + form_id).data('last_submit', new Date().getTime());
    } else {
        if (current_submit - last_submit > 600) {
            $('#' + form_id).data('last_submit', new Date().getTime());
        } else {
            return false;
        }
    }
    if (typeof (tinyMCE) != 'undefined') {
        tinyMCE.triggerSave(true);
    }

    var check_flag = true;
    $("#" + form_id + " :input").each(function (i) {
        if ($(this).attr("check")) {
            if (!validate($(this).val(), $(this).attr("check"))) {
                ui_error($(this).attr("msg"));
                $(this).focus();
                check_flag = false;
                return check_flag;
            }
        }
    });
    return check_flag;
}
/*赋值*/

function set_val(name, val) {
    if (val == "") {
        return;
    }
    if ($("#" + name + " option").length > 0) {
        $("#" + name).val(val);
        return;
    }

    if (($("#" + name).attr("type")) === "checkbox") {
        if (val == 1) {
            $("#" + name).attr("checked", true);
            return;
        }
    }

    if ($("." + name).length > 0) {
        if (($("." + name).first().attr("type")) === "checkbox") {
            var arr_val = val.split(",");
            for (var s in arr_val) {
                $("input." + name + "[value=" + arr_val[s] + "]").attr("checked", true);
            }
        }
    }

    if (($("#" + name).attr("type")) === "text") {
        $("#" + name).val(val);
        return;
    }
    if (($("#" + name).attr("type")) === "hidden") {
        $("#" + name).val(val);
        return;
    }
    if (($("#" + name).attr("rows")) > 0) {
        $("#" + name).text(val);
        return;
    }
}
//序列化表单拓展方法
(function ($) {
    $.fn.extend({
        serializeObject: function () {
            var o = {};
            var a = this.serializeArray();
            $.each(a, function () {
                if (o[this.name] !== undefined) {
                    if (!o[this.name].push) {
                        o[this.name] = [o[this.name]];
                    }
                    o[this.name].push(this.value || '');
                } else {
                    o[this.name] = this.value || '';
                }
            });
            return o;
        }
    });
})(jQuery);