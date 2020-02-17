
/**
 * JSON�����۵��˵�
 * @constructor {AccordionMenu}
 * @param {options} ����
 */

function AccordionMenu(options) {
    this.config = {
        containerCls: '.nav-left',                // �������
        menuArrs: '',                         //  JSON������������
        type: 'click',                    // Ĭ��Ϊclick Ҳ����mouseover
        renderCallBack: null,                       // ��Ⱦhtml�ṹ��ص�
        clickItemCallBack: null,                         // ÿ���ĳһ��ʱ��ص�
        topMenuText: "",
        defaultUrl: ""
    };
    this.cache = {

    };
    this.init(options);
}


AccordionMenu.prototype = {

    constructor: AccordionMenu,

    init: function (options) {
        this.config = $.extend(this.config, options || {});
        var self = this,
			_config = self.config,
			_cache = self.cache;

        // ��Ⱦhtml�ṹ
        $(_config.containerCls).each(function (index, item) {
            self._renderHTML(item);

            // �������¼�
            self._bindEnv(item);
        });
    },
    _renderHTML: function (container) {
        
        $(container).html("");
        var self = this,
			_config = self.config,
			_cache = self.cache;
        var ulhtml = $('<ul class="left_menu"></ul>');
        $(_config.menuArrs).each(function (index, item) {

            var lihtml = $('<li><a opentype=\"' + item.Description + '\"  href=\"' + item.url + '\"><span style="display:inline-block;position:relative;"><span class="l-text">' + item.name + '</span></span></a></li>');

            if (item.submenu && item.submenu.length > 0) {
                if (index == 0) {
                    $(lihtml).children('a').prepend('<img src="/js/Menu/images/blank.gif" class="unfold" alt=""/>');
                } else {
                    $(lihtml).children('a').prepend('<img src="/js/Menu/images/blank.gif" alt=""/>');
                }
                self._createSubMenu(item.submenu, lihtml, index);
            } else {
                var defaultClass = '';
                if (item.url) {
                    var defaultUrl = _config.defaultUrl;
                    if (defaultUrl) {
                        defaultClass = defaultUrl.toLowerCase() == item.url.toLowerCase() ? "left_menu_on" : "";
                    }
                }
                if (defaultClass) {
                    $(lihtml).attr('data-class', defaultClass);
                }
                $(lihtml).children('a').prepend('<img src="/js/Menu/images/blank.gif" alt=""/>');
                $(lihtml).children('a').addClass("leaf");
            }
            $(ulhtml).append(lihtml);
        });
        $(container).append(ulhtml);
        if (_config.defaultUrl && $(container).find(".left_menu_on").length > 0) {
            currNode = $(container).find(".left_menu_on").first();
        }
        var currNode = $(container).find("a[class='leaf']").first();
        if (_config.defaultUrl) {
            if ($(container).find(".left_menu_on").length > 0) {
                currNode = $(container).find(".left_menu_on").first();
            } else {
                currNode = '';
                
                $("#mainF").attr("src", _config.defaultUrl);
                //self._openPage(self._createPage(_config.defaultUrl));
            }
        }
        if (currNode.length > 0) {
           
            self._setShow(currNode);
            //self._setLoc(currNode);

            var href = currNode.attr("href");

            if (href == undefined || href == "" || href == "#") {
                return false;
            }
            var openType = currNode.attr("opentype");
            if (openType == "blank") {
                window.open(href);
            } else if (openType == 'mainiframe') {//�м�����IFrame
                $("#mainMap").css("display", "none");
                $("#mainF").css("display", "");
                $("#mainF").attr("src", href);
            } else if (openType == 'mainiframeMap') {//�м�����IFrame Map--ˮ����ҳ
                ToggleMapFrame("");
                ToggleNonMapFrame("none");
                setTimeout(FirstQuery, 3000);
            } else {
                $("#mainMap").css("display", "none");
                $("#mainF").css("display", "");
                $("#mainF").attr("src", href);
                //self._openPage(self._createPage(href));
            }
        }
        _config.renderCallBack && $.isFunction(_config.renderCallBack) && _config.renderCallBack();

        // ����㼶����
        self._levelIndent(ulhtml);
        //
        self._setDefault();
    },
    /**
	 * �����Ӳ˵�
	 * @param {array} �Ӳ˵�
	 * @param {lihtml} li��
	 */
    _createSubMenu: function (submenu, lihtml, i) {
        var self = this,
			_config = self.config,
			_cache = self.cache;
        var subUl = $('<ul></ul>'),
			callee = arguments.callee,
			subLi;

        $(submenu).each(function (index, item) {
            var url = item.url || 'javascript:void(0)';
            var defaultClass = '';
            var defaultUrl = _config.defaultUrl;
            if (defaultUrl) {
                defaultClass = defaultUrl.toLowerCase() == url.toLowerCase() ? "left_menu_on" : "";
            }
           
            subLi = $('<li><a opentype=\"' + item.Description + '\" href="' + url + '">' + item.name + '</a></li>');
            if (item.submenu && item.submenu.length > 0) {
                $(subLi).children('a').prepend('<img src="/js/Menu/images/blank.gif" alt=""/>');
                callee(item.submenu, subLi);
            } else {
                $(subLi).children('a').addClass("leaf");
                if (defaultClass) {
                    $(subLi).attr('data-class', defaultClass);
                }
            }
            $(subUl).append(subLi);
        });
        $(lihtml).append(subUl);
    },
    /**
	 * ����㼶����
	 */
    _levelIndent: function (ulList) {
        var self = this,
			_config = self.config,
			_cache = self.cache,
			callee = arguments.callee;

        var initTextIndent = 2,
			lev = 1,
			$oUl = $(ulList);

        while ($oUl.find('ul').length > 0) {
            initTextIndent = parseInt(initTextIndent, 10) + 1;
            $oUl.children().children('ul').addClass('lev-' + lev).children('li').css('text-indent', initTextIndent + 'em');
            $oUl = $oUl.children().children('ul');
            lev++;
        }

        $(ulList).find('ul').hide();

        $(ulList).find('ul:first').slideToggle('slow');


    },
    /**
	 * ���¼�
	 */
    _bindEnv: function (container) {
        var self = this,
			_config = self.config;
        //debugger;
        $('h2,a', container).unbind(_config.type);
        $('h2,a', container).bind(_config.type, function (e) {
            $("#mainF").html("");
            if ($(this).siblings('ul').length > 0) {
                $(this).siblings('ul').slideToggle('slow').end().children('img').toggleClass('unfold');
            }
            $(this).parent('li').siblings().find('ul').hide().end().find('img.unfold').removeClass('unfold');
            _config.clickItemCallBack && $.isFunction(_config.clickItemCallBack) && _config.clickItemCallBack($(this));
            var href = $(this).attr("href");
            var currNode;
            if ($(this).attr("class") == "leaf") {
                currNode = $(this);
            } else {
                currNode = $(this).next().find("a[class='leaf']").first();
            }
            self._setShow(currNode);
            //self._setLoc(currNode);

            href = currNode.attr("href");

            if (href == undefined || href == "" || href == "#") {
                return false;
            }
            var openType = currNode.attr("opentype");
             
            if (openType == "blank") {
                window.open(href);
            } else if (openType == 'mainiframe') {//�м�����IFrame
                $("#mainMap").css("display", "none");
                $("#mainF").css("display", "");
                $("#mainF").attr("src", href); 
            } else if (openType == 'menuiframe') {//���IFrame
                $("#leftContent").html("<iframe src='" + href + "' width='100%' height='100%' frameborder='0'></iframe>");

                $("#page_container").layout("show", "west");
                ToggleMapFrame("");
                ToggleNonMapFrame("none");

            } else if (openType == 'mainiframeMap') {//�м�����IFrame Map--��������˵��л�
                ToggleMapFrame("");
                ToggleNonMapFrame("none");
            } else {
                $("#mainMap").css("display", "none");
                $("#mainF").css("display", "");
                $("#mainF").attr("src", href);
                //self._openPage(self._createPage(href));
            }
            return false;
        });

    },
    _openPage: function (href) {
        $("#mainF").html(href);

    },
    _createPage: function (url) {
        return "<iframe src='" + url + "' width='100%' height='100%' frameborder='0'></iframe>";
    },
    _setShow: function (currNode) {
        $(".left_menu li").removeClass("left_menu_on");
        currNode.closest("li").addClass("left_menu_on");
    },
    _setDefault: function () {
        $(".left_menu").find('li').each(function () {
            var cl = $(this).attr('data-class');
            if (cl) {
                $(this).addClass(cl);
                $(this).removeAttr('data-class');
                $(".left_menu").find('ul').hide().find('img.unfold').removeClass('unfold');
                //console.log($(this).closest());
                $(this).closest('ul').show().siblings().find('img').addClass('unfold');
            }
        });
    },
    _setLoc: function (currNode) {
        var self = this;
        var loc = "";
        var arr = currNode.parents("li");
        for (var k = arr.length - 1; k >= 0; k--) {
            loc += $(arr[k]).find("a:first").text() + ">";
        }
        loc = loc.substr(0, loc.length - 1);
        if (loc == "") {
            $("#menu_loc").html(self.config.topMenuText);
        } else {
            $("#menu_loc").html(self.config.topMenuText + ">" + loc);
        }

    }
};
