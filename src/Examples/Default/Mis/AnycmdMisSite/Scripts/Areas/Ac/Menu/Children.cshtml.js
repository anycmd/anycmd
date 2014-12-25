/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../helper.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
// 接口：loadData
(function (window) {
    mini.namespace("Menu.Children");
    mini.namespace("Menu");
    var self = Menu.Children;
    self.prifix = "Ac_Menu_Children_";
    self.sortUrl = bootPATH + "../Ac/Menu/UpdateSortCode";
    self.loadData = loadData;
    self.add = add;
    self.remove = remove;
    mini.namespace("Menu.Children.Edit");
    var edit = Menu.Children.Edit;
    edit.prifix = "Ac_Menu_Children_Edit_";
    edit.SetData = SetData;
    edit.SaveData = SaveData;
    var faceInitialized = false;

    self.gridReload = function () {
        grid.reload();
    };

    var tabConfigs = {
        infoTab: {
            url: bootPATH + "../Ac/Menu/Details",
            params: [{ "pName": 'id', "pValue": "Id" }, { "pName": 'code', "pValue": "Code" }],
            namespace: "Menu.Details"
        },
        operationLogTab: {
            url: bootPATH + "../Ac/OperationLog/Index",
            params: [{ "pName": 'targetId', "pValue": "Id" }],
            namespace: "Ac.OperationLog.Index"
        }
    };

    mini.parse();

    var win = mini.get(edit.prifix + "win1");
    var form;
    if (win) {
        form = new mini.Form(edit.prifix + "form1");
    }

    var tabs1 = mini.get(self.prifix + "tabs1");
    var grid = mini.get(self.prifix + "datagrid1");
    grid.on("drawcell", ondrawcell);
    var helperDrawcell = helper.ondrawcell(self, "Menu.Children");
    grid.on("load", helper.onGridLoad);
    search();

    function loadData() {
        search();
    }

    function add() {
        var fragment = $.deparam.fragment();
        var data = { action: "new", ParentId: fragment.parentId, AppSystemId: fragment.appSystemId };
        grid.loading("使劲加载中...");
        helper.index.winReady(edit, function (win) {
            win.setTitle("添加");
            win.setIconCls("icon-add");
            win.show();
            edit.SetData(data);
            grid.unmask();
        });
    }

    function remove(records) {
        var id;
        if (typeof records == typeof []) {
            var ids = [];
            for (var i = 0, l = records.length; i < l; i++) {
                var r = records[i];
                ids.push(r.Id);
            }
            id = ids.join(',');
            directRemove(id);
        }
        else {
            mini.confirm("确定删除选中记录？", "确定？", function (action) {
                if (action == "ok") {
                    if (typeof records == "string") {
                        id = records;
                    }
                    else if (records && records.Id) {
                        id = records.Id;
                    }
                    directRemove(id);
                }
            });
        }
    }

    function directRemove(id) {
        if (id) {
            grid.loading("操作中，请稍后......");
            $.post(bootPATH + "../Ac/Menu/Delete", { id: id }, function (result) {
                helper.response(result, function () {
                    grid.reload();
                    window.parent.removeChildNode(id);
                }, function () {
                    grid.unmask();
                });
            }, "json");
        }
    }

    function search(callBack) {
        var fragment = $.deparam.fragment();
        data = { parentId: fragment.parentId, appSystemId: fragment.appSystemId };
        if (!grid.sortField) {
            grid.sortBy("SortCode", "asc");
        }
        grid.load(data, function () {
            var record = grid.getSelected();
            if (!record) {
                tabs1.hide();
            }
        });
    }

    helper.index.allInOne(
        edit,
        grid,
        bootPATH + "../Ac/Menu/Edit",
        bootPATH + "../Ac/Menu/Edit",
        bootPATH + "../Ac/Menu/Delete",
        self);
    helper.index.tabInOne(grid, tabs1, tabConfigs, self);

    function SaveData() {
        var parentId = $.deparam.fragment().parentId;
        if (parentId) {
            $("#" + edit.prifix + "form1 input[name='ParentId']").val(parentId);
        }
        var data = $("#" + edit.prifix + "form1").serialize();
        var id = $("#" + edit.prifix + "form1 input[name='Id']").val();
        var url = bootPATH + "../Ac/Menu/Create";
        if (id) {
            url = bootPATH + "../Ac/Menu/Update";
        }
        form.validate();
        if (form.isValid() == false) return;

        $.post(url, data, function (result) {
            helper.response(result, function () {
                var name = $("#" + edit.prifix + "form1 input[name='Name']").val();
                if (id) {
                    if (window.updateCurrentNodeName) {
                        window.updateCurrentNodeName(name);
                    }
                    if (window.parent.updateChildNodeName) {
                        window.parent.updateChildNodeName(id, name);
                    }
                }
                else {
                    if (window.addNewNode) {
                        window.addNewNode({ Id: result.id, Name: name, isLeaf: true });
                    }
                    if (window.parent.addNewNode) {
                        window.parent.addNewNode({ Id: result.id, Name: name, isLeaf: true });
                    }
                }
                edit.CloseWindow("save");
            });
        }, "json");
    };
    function ondrawcell(e) {
        var field = e.field;
        var value = e.value;
        var record = e.record;
        if (field) {
            switch (field) {
                case "Icon":
                    if (value) {
                        e.cellHtml = "<img src='" + bootPATH + "../Content/icons/16x16/" + value + "'></img>";
                    }
                    else {
                        e.cellHtml = "<span class=\"gridCellIcon mini-tree-folder\"></span>";
                    }
                    break;
            }
        }
        helperDrawcell(e);
    }

    function SetData(data) {
        //跨页面传递的数据对象，克隆后才可以安全使用
        data = mini.clone(data);
        if (data.action == "edit") {
            $.ajax({
                url: bootPATH + "../Ac/Menu/Get",
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

    helper.edit.allInOne(
        self,
        win,
        bootPATH + "../Ac/Menu/Create",
        bootPATH + "../Ac/Menu/Update",
        bootPATH + "../Ac/Menu/Get",
        form, edit);
})(window);