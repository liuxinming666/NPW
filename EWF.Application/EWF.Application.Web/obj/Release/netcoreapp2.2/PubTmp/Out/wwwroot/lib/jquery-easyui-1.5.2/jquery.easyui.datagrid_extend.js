(function (){  
    $.extend($.fn.datagrid.methods, {  
        //显示遮罩  
        loading: function (jq) {
            var loadmsg = jq.datagrid("options").loadMsg; 
            if (loadmsg) {
                $.showLoading();
            }
            
            //alert("Loading");
            return false;
        },  
    //隐藏遮罩  
        loaded: function (jq) {
            var loadmsg = jq.datagrid("options").loadMsg;
            if (loadmsg) {
                $.hideLoading();
            }
            //alert("loaded");
        }
    });
    //$.extend($.fn.datagrid.defaults, {
    //    rowStyler: function (rowIndex, row) {
    //        if (rowIndex % 2 == 1) {
    //            return 'background:#dce8f3;';
    //        }
    //    }
    //});
})(jQuery);  
