/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../helper.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    mini.namespace("Organization.Children");
    var self = Organization.Children;
    self.prifix = "Ac_Organization_Children_";
    self.sortUrl = bootPATH + "../Ac/Organization/UpdateSortCode";
    self.loadData = loadData;
    self.remove = remove;
    self.search = search;

    self.gridReload = function () {
        grid.reload();
    };
    mini.namespace("Organization.Children.Edit");
    var edit = Organization.Children.Edit;
    edit.prifix = "Ac_Organization_Children_Edit_";
    edit.SaveData = SaveData;
    edit.SetData = SetData;

    var tabConfigs = {
        infoTab: {
            url: bootPATH + "../Ac/Organization/Details",
            params: [{ "pName": 'id', "pValue": "Id" }, { "pName": 'code', "pValue": "Code" }],
            namespace: "Organization.Details"
        },
        managerTab: {
            url: bootPATH + "../Ac/Organization/Accounts",
            params: [{ "pName": 'organizationId', "pValue": "Id" }, { "pName": 'organizationCode', "pValue": "Code" }],
            namespace: "Organization.Accounts"
        },
        operationLogTab: {
            url: bootPATH + "../Ac/OperationLog/Index",
            params: [{ "pName": 'targetId', "pValue": "Id" }],
            namespace: "Ac.OperationLog.Index"
        }
    };
    self.filters = {
        Name: {
            type: 'string',
            comparison: 'like'
        },
        Code: {
            type: 'string',
            comparison: 'like'
        },
        ParentCode: {
            type: 'string',
            comparison: 'like'
        },
        ParentName: {
            type: 'string',
            comparison: 'like'
        },
        IsEnabled: {
            type: 'numeric',
            comparison: 'eq'
        },
        CategoryCode: {
            type: 'string',
            comparison: 'eq'
        }
    };

    mini.parse();

    var win = mini.get(edit.prifix + "win1");
    var form;
    if (win) {
        form = new mini.Form(edit.prifix + "form1");
    }

    var chkbIncludedescendants = mini.get(self.prifix + "chkbIncludedescendants");
    chkbIncludedescendants.on("checkedchanged", function () {
        search();
    });
    var tabs1 = mini.get(self.prifix + "tabs1");
    var grid = mini.get(self.prifix + "datagrid1");
    grid.on("drawcell", helper.ondrawcell(self, "Organization.Children"));
    grid.on("load", helper.onGridLoad);
    search();

    function loadData() {
        search();
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
            $.post(bootPATH + "../Ac/Organization/Delete", { id: id }, function (result) {
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
        data = {};
        var fragment = $.deparam.fragment();
        data.parentId = fragment.parentId;
        if (fragment.parentCode) {
            data.parentCode = fragment.parentCode;
        }
        if (chkbIncludedescendants.getValue() == "1") {
            data.includedescendants = true;
        }
        else {
            data.includedescendants = false;
        }
        if (!grid.sortField) {
            grid.sortBy("SortCode", "asc");
        }
        var filterArray = [];
        for (var k in self.filters) {
            var filter = self.filters[k];
            if (filter.value) {
                filterArray.push({ field: k, type: filter.type, comparison: filter.comparison, value: filter.value });
            }
        }
        data.filters = JSON.stringify(filterArray);
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
        bootPATH + "../Ac/Organization/Edit",
        bootPATH + "../Ac/Organization/Edit",
        bootPATH + "../Ac/Organization/Delete",
        self);
    helper.index.tabInOne(grid, tabs1, tabConfigs, self);

    function SaveData() {
        var parentId = $.deparam.fragment().parentId;
        if (parentId) {
            $("#" + edit.prifix + "form1 input[name='ParentId']").val(parentId);
        }
        var data = $("#" + edit.prifix + "form1").serialize();
        var id = $("#" + edit.prifix + "form1 input[name='Id']").val();
        var url = bootPATH + "../Ac/Organization/Create";
        if (id) {
            url = bootPATH + "../Ac/Organization/Update";
        }
        form.validate();
        if (form.isValid() == false) return;

        $.post(url, data, function (result) {
            helper.response(result, function () {
                var name = $("#" + edit.prifix + "form1 input[name='Name']").val();
                var categoryId = $("#" + edit.prifix + "form1 input[name='CategoryId']").val();
                var code = $("#" + edit.prifix + "form1 input[name='Code']").val();
                var parentCode = $("#" + edit.prifix + "form1 input[name='ParentCode']").val();
                var newNode = { Id: result.id, Name: name, isLeaf: true, ParentCode: parentCode, Code: code, CategoryId: categoryId };

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
                        window.addNewNode(newNode);
                    }
                    if (window.parent.addNewNode) {
                        window.parent.addNewNode(newNode);
                    }
                }
                edit.CloseWindow("save");
            });
        }, "json");
    };
    function SetData(data) {
        //跨页面传递的数据对象，克隆后才可以安全使用
        data = mini.clone(data);
        if (data.action == "edit") {
            $.ajax({
                url: bootPATH + "../Ac/Organization/Get",
                data: { id: data.id },
                cache: false,
                success: function (result) {
                    helper.response(result, function () {
                        form.setData(result);
                        form.validate();
                    });
                }
            });
        }
        else if (data.action == "new") {
            data.CategoryId = "";
            form.setData(data);
        }
    }
    helper.edit.allInOne(
        self,
        win,
        bootPATH + "../Ac/Organization/Create",
        bootPATH + "../Ac/Organization/Update",
        bootPATH + "../Ac/Organization/Get",
        form, edit);
})(window);