/**
* jQuery's jqfaceedit Plugin
*
* @author cdm
* @version 0.2
* @copyright Copyright(c) 2012.
* @date 2012-08-09
*/
(function ($) {
    $.fn.extend({
        jqfaceedit: function (options) {
            var defaults = {
                txtAreaObj: '', //TextArea对象
                containerObj: '', //表情框父对象
                textareaid: 'msg', //textarea元素的id
                popName: '', //iframe弹出框名称,containerObj为父窗体时使用
                top: 0, //相对偏移
                left: 0, //相对偏移
                width: 0, //宽度
                height: 0//高度
            };

            var options = $.extend(defaults, options);
            var cpos = 0; //光标位置，支持从光标处插入数据
            var textareaid = options.textareaid;

            return this.each(function () {
                var Obj = $(this);
                var container = options.containerObj;
                if (document.selection) {//ie
                    options.txtAreaObj.bind("click keyup", function (e) {//点击或键盘动作时设置光标值
                        e.stopPropagation();
                        cpos = getPositionForTextArea(document.getElementById(textareaid) ? document.getElementById(textareaid) : window.frames[options.popName].document.getElementById(textareaid));
                    });
                }
                var width = options.width;
                var height = options.height;
                $(Obj).bind("click", function (e) {
                    var faceWindow = mini.get("faceWindow");
                    faceWindow.allowResize = true;

                    e.stopPropagation();
                    var faceHtml = [];
                    faceHtml.push('<div id="face" style="position:raletive;margin:2px 0px 0px 3px;padding:0px;width:');
                    faceHtml.push(width);
                    faceHtml.push('px;">');
                    faceHtml.push('<div id="texttb" style="margin:0px auto;padding:0px;display:none;"><a class="f_close" title="关闭" href="javascript:void(0);"></a></div>');
                    faceHtml.push('<div id="facebox" style="border-top:0px;margin:0px auto;padding:0px;width:');
                    faceHtml.push(width - 6);
                    faceHtml.push('px;">');
                    faceHtml.push('<div id="face_detail" class="facebox clearfix" style="margin:0px auto;padding:0px;overflower:hidden"><ul>');


                    for (i = 0; i < options.emotions.length; i++) {
                        faceHtml.push('<li text=');
                        faceHtml.push(options.emotions[i].icon);
                        faceHtml.push(' type=');
                        faceHtml.push(i);
                        faceHtml.push('><img title=');
                        faceHtml.push(options.emotions[i].phrase);
                        faceHtml.push(' src="');
                        faceHtml.push(bootPATH);
                        faceHtml.push("../");
                        faceHtml.push(options.emotions[i].url);
                        faceHtml.push('"  style="cursor:pointer; position:relative;margin-top:5px;" /></li>');
                    }
                    faceHtml.push('</ul></div>');
                    faceHtml.push('</div><div class="arrow arrow_t" style="display:none"></div></div>');

                    container.find('#face').remove(); faceWindow.hide()
                    container.html(faceHtml.join(""));

                    var faceboxDv = document.getElementById("facebox");

                    faceWindow.show();

                    container.find("#face_detail ul >li").bind("click", function (e) {
                        var txt = $(this).html();
                        mini.get("hdIcon").setValue($(this).attr("text"));
                        var faceText = txt;

                        var tclen = options.txtAreaObj.val().length;

                        var tc = document.getElementById(textareaid);
                        if (options.popName) {
                            tc = window.frames[options.popName].document.getElementById(textareaid);
                        }
                        var pos = 0;
                        if (typeof document.selection != "undefined") {//IE
                            tc.innerHTML = faceText;
                        } else {//火狐
                            tc.innerHTML = faceText;
                        }
                        container.find("#face").remove();
                    });
                    //关闭表情框
                    container.find(".f_close").bind("click", function () {
                        container.find("#face").remove();
                    });
                    //处理js事件冒泡问题
                    $('body').bind("click", function (e) {
                        e.stopPropagation();
                        container.find('#face').remove(); faceWindow.hide()
                        $(this).unbind('click');
                    });
                    if (options.popName != '') {
                        $(window.frames[options.popName].document).find('body').bind("click", function (e) {
                            e.stopPropagation();
                            container.find('#face').remove(); faceWindow.hide()
                        });
                    }
                    container.find('#face').bind("click", function (e) {
                        e.stopPropagation();
                    });
                    var offset = $(e.target).offset();
                    offset.top += options.top;
                    offset.left += options.left;
                    container.find("#face").show();
                });
            });
        }
    });
})(jQuery);
