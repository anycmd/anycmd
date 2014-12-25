(function (window) {
    if (!window.__CreateJSPath) {
        var alertProx = window.alert;
        window.alert = function (msg) {
            if (typeof (msg) != "string") {
                alertProx(msg);
                return;
            }
            if (msg && msg.substring && msg.substring(0, "试用到期 www.miniui.com".length) != "试用到期 www.miniui.com") {
                alertProx(msg);
                return;
            }
            return false;
        };
        window.getCookie = function (sName) {
            var aCookie = document.cookie.split("; ");
            var lastMatch = null;
            for (var i = 0; i < aCookie.length; i++) {
                var aCrumb = aCookie[i].split("=");
                if (sName == aCrumb[0]) {
                    lastMatch = aCrumb;
                }
            }
            if (lastMatch) {
                var v = lastMatch[1];
                if (v === undefined) return v;
                return unescape(v);
            }
            return null;
        }
        window.wallpaper = function (imgID, url) {
            function fun() {
                var e = $(imgID);
                if (e) {
                    if (!url) {
                        url = e.attr("src");
                    }
                    if (url) {
                        e.show();
                    }
                    var cw, ch, iw, ih, img;
                    cw = $(window).width();
                    ch = $(window).height();
                    img = new Image();
                    // url 是你的图片地址
                    img.src = url;
                    img.onload = function () {
                        iw = img.width;
                        ih = img.height;
                        if (cw / ch >= iw / ih) {
                            var new_h = parseInt(cw / iw * ih, 10);
                            var imgTop = parseInt((ch - new_h) / 2, 10);
                            // 假设图片的 id 为 J-img
                            e.css({
                                "width": cw + "px",
                                "height": new_h + "px",
                                "top": imgTop + "px",
                                "left": ""
                            });
                        }
                        if (cw / ch < iw / ih) {
                            var new_w = parseInt(ch / ih * iw, 10);
                            var imgLeft = parseInt((cw - new_w) / 2, 10);
                            // 假设图片的 id 为 J-img
                            e.css({
                                "width": new_w + "px",
                                "height": ch + "px",
                                "top": "",
                                "left": imgLeft + "px"
                            });
                        }
                        e.attr("src", url);
                    }
                }
            }
            fun();
            $("body").css({ position: "relative", overflow: "hidden" });
            $(imgID).css({ position: "absolute" });
            $(window).resize(fun);
        }
        window.__CreateJSPath = function (js) {
            var scripts = document.getElementsByTagName("script");
            var path = "";
            for (var i = 0, l = scripts.length; i < l; i++) {
                var src = scripts[i].src;
                if (src.indexOf(js) != -1) {
                    var ss1 = src.split(js);
                    path = ss1[0];
                    break;
                }
            }
            var href = location.href;
            href = href.split("#")[0];
            href = href.split("?")[0];
            var ss = href.split("/");
            ss.length = ss.length - 1;
            href = ss.join("/");
            if (path.indexOf("https:") == -1 && path.indexOf("http:") == -1 && path.indexOf("file:") == -1 && path.indexOf("\/") != 0) {
                path = href + "/" + path;
            }
            return path;
        };
        window.onSkinChange = function (skin) {
            mini.Cookie.set("miniuiSkin", skin, 100); //100天过期的话，可以保持皮肤切换
            var skinEl = document.getElementById(LINKSKIN_ID);
            if (skinEl) {
                skinEl.parentNode.removeChild(skinEl);
            }

            var url = bootPATH + "miniui/themes/" + skin + "/skin.css";
            if (skin) {
                addCSSLink(LINKSKIN_ID, url);
            }

            //处理iframe
            try {
                var wins = $("iframe");
                for (var i = 0; i < wins.length; i++) {
                    var win = wins.get(i).contentWindow;
                    var doc = win.document;
                    var iframeSkinEl = doc.getElementById(LINKSKIN_ID);
                    if (iframeSkinEl) {
                        iframeSkinEl.parentNode.removeChild(iframeSkinEl);
                    }
                    if (skin) {
                        addCSSLink(LINKSKIN_ID, url, doc);
                    }
                    if (win.mini) {
                        win.mini.repaint();
                    }
                    if (win.onSkinChange) {
                        win.onSkinChange(skin);
                    }
                }
            } catch (e) {
            }
        }
        function addCSSLink(id, url, doc) {
            doc = doc || document;
            var link = doc.createElement("link");
            link.id = id;
            link.setAttribute("rel", "stylesheet");
            link.setAttribute("type", "text/css");
            link.setAttribute("href", url);

            var heads = doc.getElementsByTagName("head");
            if (heads.length)
                heads[0].appendChild(link);
            else
                doc.documentElement.appendChild(link);
        }

        window.bootPATH = __CreateJSPath("boot.js");
        document.write('<link href="' + bootPATH + 'libs/qtip2/jquery.qtip.min.css" rel="stylesheet" type="text/css" />');
        document.write('<link href="' + bootPATH + 'miniui/themes/default/miniui.css" rel="stylesheet" type="text/css" />');
        var LINKSKIN_ID = "miniuiSkin";
        var skin = getCookie("miniuiSkin");
        if (skin) {
            document.write('<link id="' + LINKSKIN_ID + '" href="' + bootPATH + 'miniui/themes/' + skin + '/skin.css" rel="stylesheet" type="text/css" />');
        }
        document.write('<link href="' + bootPATH + 'miniui/themes/icons.css" rel="stylesheet" type="text/css" />');
        document.write('<link href="' + bootPATH + '../Content/Site.css" rel="stylesheet" type="text/css" />');

        document.write('<script src="' + bootPATH + 'json2.js" type="text/javascript"></sc' + 'ript>');
        document.write('<script src="' + bootPATH + 'jquery-1.8.3.min.js" type="text/javascript"></sc' + 'ript>');
        document.write('<script src="' + bootPATH + 'jquery-bbq/jquery.ba-bbq.min.js" type="text/javascript"></sc' + 'ript>');
        document.write('<script src="' + bootPATH + 'miniui/miniui.js" type="text/javascript"></sc' + 'ript>');
        document.write('<script src="' + bootPATH + 'libs/qtip2/jquery.qtip.min.js" type="text/javascript"></sc' + 'ript>');
        document.write('<script src="' + bootPATH + 'helper.js" type="text/javascript"></sc' + 'ript>');
        document.write('<script src="' + bootPATH + 'tooltipBoot.js" type="text/javascript"></sc' + 'ript>');
    }
})(window);
