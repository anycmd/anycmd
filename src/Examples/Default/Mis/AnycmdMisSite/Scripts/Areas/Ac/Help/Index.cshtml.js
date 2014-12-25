/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    mini.namespace("Ac.Help.Index");
    var self = Ac.Help.Index;
    self.prifix = "Ac_Help_Index_";
    self.sortUrl = bootPATH + "../Ac/Help/UpdateSortCode";
    self.help = { appSystemCode: "Anycmd", areaCode: "Ac", resourceCode: "Help", functionCode: "Index" };
    helper.helperSplitterInOne(self);
    self.loadData = search;
    self.search = search;
    self.edit = edit;
    self.add = add;

    self.gridReload = function () {
        grid.reload();
    };
    mini.namespace("Help.Edit");
    var editView = Help.Edit;
    editView.prifix = "Ac_Help_Index_Edit_";
    editView.SaveData = SaveData;
    editView.SetData = SetData;

    var tabConfigs = {
        infoTab: {
            url: bootPATH + "../Ac/Help/Details",
            params: [{ "pName": 'id', "pValue": "Id" }],
            namespace: "Help.Details"
        },
        operationLogTab: {
            url: bootPATH + "../Ac/OperationLog/Index",
            params: [{ "pName": 'targetId', "pValue": "Id" }],
            namespace: "Ac.OperationLog.Index"
        }
    };
    var editor;
    var restoreHeight;

    $().ready(function () {
        KindEditor.ready(function (K) {
            editor = K.create("#" + editView.prifix + 'contentEditor', {
                allowFileManager: true,
                width: "100%",
                uploadJson: bootPATH + '../Scripts/kindeditor/asp.net/upload_json.ashx',
                fileManagerJson: bootPATH + '../Scripts/kindeditor/asp.net/file_manager_json.ashx',
                afterCreate: function () {
                    restoreHeight = getEditorAutoHeight();
                    this.edit.setHeight(restoreHeight);
                }
            });
        });
    });

    mini.parse();
    var win = mini.get(editView.prifix + "win1");
    var form = new mini.Form(editView.prifix + "form1");
    win.on("resize", function (e) {
        editor.edit.setHeight(getEditorAutoHeight());
    });
    win.on("buttonclick", function (e) {
        if (e.name == "max") {
            if (e.source.state == "restore") {
                //放大
                editor.edit.setHeight((document.body.clientHeight - 270) + "px");
            } else {
                //还原
                editor.edit.setHeight(restoreHeight);
            }
        }
    });

    function getEditorAutoHeight() {
        var winHeight = parseInt(win.height);
        return (winHeight - 270) + "px";
    }

    var hdContent = mini.get(editView.prifix + "hdContent");
    var key = mini.get(self.prifix + "key");
    key.on("enter", search);
    var currentDicRecord;

    var tabs1 = mini.get(self.prifix + "tabs1");
    var grid = mini.get(self.prifix + "datagrid1");
    grid.on("drawcell", helper.ondrawcell(self, "Ac.Help.Index"));
    grid.on("load", helper.onGridLoad);
    grid.sortBy("SortCode", "asc");

    function onNodeSelect(e) {
        var node = e.node;
        grid.load({ categoryId: node.Id });
    }

    function add() {
        var data = { action: "new", oldWidth: parseInt(win.width.replace("px", "")), oldHeight: parseInt(editor.edit.height.replace("px", "")) };
        if (win) {
            win.setTitle("添加");
            win.setIconCls("icon-add");
            win.show();
            editView.SetData(data, editor);
        }
    }

    function edit() {
        var row = grid.getSelected();
        if (row) {
            var data = { id: row.Id, action: "edit", oldWidth: parseInt(win.width.replace("px", "")), oldHeight: parseInt(editor.edit.height.replace("px", "")) };
            if (win) {
                win.setTitle("编辑");
                win.setIconCls("icon-edit");
                editView.SetData(data, editor);
                win.show();
            }
        } else {
            mini.alert("请选中一条记录");
        }
    }

    function search() {
        var data = { key: key.getValue() };
        grid.load(data, function () {
            var record = grid.getSelected();
            if (!record) {
                tabs1.hide();
            }
        });
    }

    function SetData(data, editor) {
        //跨页面传递的数据对象，克隆后才可以安全使用
        data = mini.clone(data);
        editor = editor;
        editor.html("");
        if (data.action == "edit") {
            $.ajax({
                url: bootPATH + "../Ac/Help/Get?id=" + data.id,
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
        else if (data.action == "new") {
            data["DicId"] = "";
            data["OntologyId"] = $.deparam.fragment().ontologyId;
            form.setData(data);
        }
    }

    function SaveData() {
        hdContent.setValue(editor.html());
        var data = $("#" + editView.prifix + "form1").serialize();
        var id = $("#" + editView.prifix + "form1" + " input[name='Id']").val();
        var url = "../Help/Create";
        if (id) {
            url = "../Help/Update";
        }
        form.validate();
        if (form.isValid() == false) return;

        $.post(url, data, function (result) {
            helper.response(result, function () {
                editView.CloseWindow("save");
            });
        }, "json");
    }
    helper.index.allInOne(
        editView,
        grid,
        bootPATH + "../Ac/Help/Edit",
        bootPATH + "../Ac/Help/Edit",
        bootPATH + "../Ac/Help/Delete",
        self);
    helper.index.tabInOne(grid, tabs1, tabConfigs, self);

    helper.edit.allInOne(
        self,
        win,
        bootPATH + "../Ac/Help/Create",
        bootPATH + "../Ac/Help/Update",
        bootPATH + "../Ac/Help/Get",
        form, editView);
})(window);