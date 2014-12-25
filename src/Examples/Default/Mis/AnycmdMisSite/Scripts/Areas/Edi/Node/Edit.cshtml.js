/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    var nodeId = $.deparam.fragment().nodeId || $.deparam.querystring().nodeId || '';

    mini.namespace("Node.Edit");
    var self = Node.Edit;
    self.prifix = "Edi_Node_Edit_";
    self.SetData = SetData;
    self.loadData = loadData;
    var faceInitialized = false;

    mini.parse();

    var form = new mini.Form(self.prifix + "form1");
    var btnSave = mini.get(self.prifix + "btnSave");
    if (btnSave) {
        btnSave.on("click", SaveData);
    }

    function SetData(data) {
        //跨页面传递的数据对象，克隆后才可以安全使用
        data = mini.clone(data);
        if (data.action == "edit") {
            $.ajax({
                url: bootPATH + "../Edi/Node/Get",
                data: { id: data.id },
                cache: false,
                success: function (result) {
                    helper.response(result, function () {
                        form.setData(result);
                        form.validate();
                        if (result.Icon) {
                            $("#msg").html("<img style='margin-top:3px;' src='" + bootPATH + "../Content/icons/16x16/" + result.Icon + "' alt='图标' />");
                        } else {
                            $("#msg").html("");
                        }
                    });
                }
            });
        }
        else if (data.action == "new") {
            form.setData(data);
        }
        if (!faceInitialized) {
            $.getJSON(bootPATH + "../Home/GetIcons", null, function (result) {
                $("#message_face").jqfaceedit({ txtAreaObj: $("#msg"), containerObj: $(mini.get('faceWindow').getBodyEl()), emotions: result, top: 25, left: -27, width: 658, height: 420 });
            });
            faceInitialized = true;
        }
    }

    function loadData() {
        var data = getParams();
        if (data["id"] && data["id"] != "-1") {
            data.action = 'edit';
        }
        else {
            data.action = 'new';
        }
        SetData(data);
    }

    function getParams() {
        var data = {};
        if (self.params && self.params.id) {
            data.id = self.params.id;
        }
        else {
            data.id = $.deparam.fragment().id || $.deparam.querystring().id;
        }
        return data;
    }

    function SaveData() {
        var data = $("#" + self.prifix + "form1").serialize();

        form.validate();
        if (form.isValid() == false) return;
        var href = bootPATH + "../Edi/Node/Create?nodeId=" + nodeId;
        if ($("#" + self.prifix + "form1 input[name='Id']").val()) {
            href = bootPATH + "../Edi/Node/Update?nodeId=" + nodeId;
        }

        $.post(href, data, function (result) {
            helper.response(result, function () {
                Edi.Node.Index.gridReload();
            });
        }, "json");
    }
})(window);