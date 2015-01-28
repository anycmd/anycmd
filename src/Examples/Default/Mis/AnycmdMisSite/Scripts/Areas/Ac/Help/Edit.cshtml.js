/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window, $) {
    mini.parse();

    var form = new mini.Form("form1");
    var hdContent = mini.get("hdContent");

    $().ready(function () {
        KindEditor.ready(function (K) {
            var editor = K.create('#contentEditor', {
                allowFileManager: true,
                width: "98%",
                height: "500px",
                uploadJson: bootPATH + '../Scripts/kindeditor/asp.net/upload_json.ashx',
                fileManagerJson: bootPATH + '../Scripts/kindeditor/asp.net/file_manager_json.ashx'
            });
            $("#wrapper").show();
            var data = getParams();
            $(".btnOk").click(function () {
                hdContent.setValue(editor.html());
                var o = $("#form1").serialize();
                form.validate();
                if (form.isValid() == false) return;
                var p = getParams();
                var url = bootPATH + "../Ac/Help/SaveHelp?uiViewId=" + p.id + "&appSystemCode=" + p.appSystemCode + "&resourceCode=" + p.resourceCode + "&functionCode=" + p.functionCode;
                $.ajax({
                    url: url,
                    type: "post",
                    data: o,
                    cache: false,
                    success: function (result) {
                        helper.response(result, function () {
                            mini.alert("保存成功");
                        });
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        alert(jqXHR.responseText);
                    }
                });
            });
            SetData(data, editor);
        });
    });

    function getParams() {
        var fragment = $.deparam.fragment();
        var querystring = $.deparam.querystring();
        return {
            id: fragment.id || querystring.id,
            appSystemCode: fragment.appSystemCode || querystring.appSystemCode,
            resourceCode: fragment.resourceCode || querystring.resourceCode,
            functionCode: fragment.functionCode || querystring.functionCode
        };
    }

    function SetData(data, editor) {
        //跨页面传递的数据对象，克隆后才可以安全使用
        data = mini.clone(data);
        editor = editor;
        editor.html("");
        $.ajax({
            url: bootPATH + "../Ac/Help/Get",
            data: data,
            cache: false,
            success: function (result) {
                helper.response(result, function () {
                    form.setData(result);
                    editor.html(hdContent.getValue());
                    form.validate();
                });
            }
        });
    }
})(window, $);