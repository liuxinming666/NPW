var menu = menu||{};
menu.settings = {
    MenuClassName: 'nav',
    MenuContainerID: 'page_container',
    ContentID: 'mainF',
    LocID: 'menu_loc',
    HomeUrl: '/SQWEB/'
}
menu.InitSettings=function(options){
    menu.settings = $.extend(menu.settings, options);
    
}
menu.ShowMenu=function(options){
    this.InitSettings(options); 
    
    var pc_id = this.settings.MenuContainerID;
    $("#" + pc_id).layout("hidden", "west");
    this.BindTopMeu();
}
//初始化主页面容器
menu.SetMainPage = function (href) {
    var contentId = this.settings.ContentID;
    $("#" + contentId).html(href);
}
menu.SetLoc = function (menuLinkText) {
    var menuLoc = this.settings.LocID;
     
    $("#" + menuLoc).html(menuLinkText);
}
menu.BindTopMeu = function () {
    var self = this;
    var clsName = this.settings.MenuClassName;
    self.SetMainPage(self.CreatePage(this.settings.HomeUrl));
    //顶部导航切换
    $("." + clsName + " li a").click(function () {
        
        $("." + clsName + " li a.selected").removeClass("selected");
        $(this).addClass("selected");
       
        self.SetMainPage("");
        
        var submenuId = "menu_" + $(this).attr("tag"); 
        self.SetLoc("");
        if ($("#" + submenuId).length > 0) {
             
            //有子菜单显示左边菜单
            $("#" + self.settings.MenuContainerID).layout("show", "west");

            //隐藏所有子菜单
            $(".left_menu").hide();

            //展开第一个子菜单
            $('#' + submenuId + ' ul').slideUp("fast", function () {
                $('#' + submenuId + ' ul:first').slideDown();
            });
           
            //显示当前二级菜单
            $("#" + submenuId).show();
            
            self.BindSubMenu(submenuId);
        }
        else {
            //无子菜单隐藏左边菜单
            $("#" + self.settings.MenuContainerID).layout("hidden", "west");
            var href = $(this).attr("href");
            self.SetMainPage(self.CreatePage(href));
        }

        return false;
    });
            
}
menu.BindSubMenu = function (id) {
    var self = this;
    //首次进入系统初始化为第一个菜单的位置信息
    this.SetLoc($('#' + id + ' ul:first li a').first(0).attr("tag"));
    
    $('#' + id + ' li a').click(
      function () {
          var checkElement = $(this).next();
          var tag = $(this).attr("tag");
          self.SetLoc(tag);
          if ((checkElement.is('ul')) && (checkElement.is(':visible'))) {
              return false;
          }
          if ((checkElement.is('ul')) && (!checkElement.is(':visible'))) {
              $('#' + id + ' ul:visible').slideUp('normal');
              checkElement.slideDown('normal');
              return false;
          }
          var href = $(this).attr("href");

          if (href != "javacript:void(0)" && href != "#") {
              //$("#mainF").html(CreatePage(href)); 
              self.SetMainPage(self.CreatePage(href));
          }
          return false;
      }
    );
}
menu.CreatePage = function (url) {
    return "<iframe src='" + url + "' width='100%' height='100%' frameborder='0'></iframe>";
}