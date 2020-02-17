// JScript 文件
    var change = function(obj1,obj2){
        document.all[obj1].src = document.all[obj2].value;
    };
    var curr_Index = 0,play_space = 800,play_state = -1;
    var first = function(obj1,obj2){
        curr_Index = 0;
        document.all[obj2].selectedIndex = 0;
        document.all[obj1].src = document.all[obj2].options[0].value;
    };
    var last = function(obj1,obj2){
        curr_Index = document.all[obj2].length - 1;
        document.all[obj2].selectedIndex = curr_Index;
        document.all[obj1].src = document.all[obj2].options[document.all[obj2].length-1].value;
    };
    var play = function(obj1,obj2){
//        var tem = document.all["aaa"].innerHTML;
//        if(tem == "播放"){
//            document.all["aaa"].innerHTML = "停止";
            if(play_state == -1 || play_state == 1){
                play_state = 0;
                play_loop(obj1,obj2);
            }
//        }
//        else if(tem == "停止"){
//            document.all["aaa"].innerHTML = "播放";
//            play_loop();
//        }
    };
    var suspend = function(obj1,obj2){
        if(play_state == 0){
            play_state = 1;
            play_loop(obj1,obj2);
        }
        
    };
    var stop = function(obj1,obj2){
        if(play_state == 0 || play_state == 1){
            play_state = -1;
            play_loop(obj1,obj2);
        }
    };
    var play_loop = function(obj1,obj2){
        if(play_state == 0){//document.all["aaa"].innerHTML == "停止"
            if(curr_Index < document.all[obj2].length){
                document.all[obj2].selectedIndex = curr_Index;
                document.all[obj1].src = document.all[obj2].options[curr_Index].value;
                window.setTimeout("play_loop('"+obj1+"','"+obj2+"')",play_space);
                if(curr_Index == document.all[obj2].length - 1){
//                    document.all["aaa"].innerHTML = "播放";
                    play_state = -1;
                    curr_Index = -1;
                }
                curr_Index++;
            }
        }
        else if(play_state == -1){
            curr_Index = document.all[obj2].length - 1;
            document.all[obj2].selectedIndex = curr_Index;
            document.all[obj1].src = document.all[obj2].options[curr_Index].value;
            curr_Index = 0;
        }
        
    };
    var next = function(obj1,obj2){
        if(document.all[obj2][curr_Index + 1]){
            document.all[obj1].src = document.all[obj2][curr_Index + 1].value;
            document.all[obj2].selectedIndex = ++curr_Index;
        }
        else
            first(obj1,obj2);
    };
    var previous = function(obj1,obj2){
        if(curr_Index -1 >= 0){
            document.all[obj1].src = document.all[obj2][curr_Index - 1].value;
            document.all[obj2].selectedIndex = --curr_Index;
        }
        else
            last(obj1,obj2);
    };
    var b_speed = function(objName){
        if(objName == "a") play_space = 500;
        else if(objName == "b") play_space = 800;
        else play_space = 1000;
    };
    var close_self = function(){
        window.parent.document.getElementById("span3DWnd").style.display="inline";
    };