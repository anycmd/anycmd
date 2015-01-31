/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../helper.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    mini.namespace("Catalog.Children");
    var self = Catalog.Children;
    self.prifix = "Ac_Catalog_Children_";
    self.sortUrl = bootPATH + "../Ac/Catalog/UpdateSortCode";
    self.loadData = loadData;
    self.remove = remove;
    self.search = search;

    self.gridReload = function () {
        grid.reload();
    };
    mini.namespace("Catalog.Children.Edit");
    var edit = Catalog.Children.Edit;
    edit.prifix = "Ac_Catalog_Children_Edit_";
    edit.SaveData = SaveData;
    edit.SetData = SetData;

    var tabConfigs = {
        infoTab: {
            url: bootPATH + "../Ac/Catalog/Details",
            params: [{ "pName": 'id', "pValue": "Id" }, { "pName": 'code', "pValue": "Code" }],
            namespace: "Catalog.Details"
        },
        managerTab: {
            url: bootPATH + "../Ac/Catalog/Accounts",
            params: [{ "pName": 'catalogId', "pValue": "Id" }, { "pName": 'catalogCode', "pValue": "Code" }],
            namespace: "Catalog.Accounts"
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
    grid.on("drawcell", helper.ondrawcell(self, "Catalog.Children"));
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
            $.post(bootPATH + "../Ac/Catalog/Delete", { id: id }, function (result) {
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
        bootPATH + "../Ac/Catalog/Edit",
        bootPATH + "../Ac/Catalog/Edit",
        bootPATH + "../Ac/Catalog/Delete",
        self);
    helper.index.tabInOne(grid, tabs1, tabConfigs, self);

    function SaveData() {
        var parentId = $.deparam.fragment().parentId;
        if (parentId) {
            $("#" + edit.prifix + "form1 input[name='ParentId']").val(parentId);
        }
        var data = $("#" + edit.prifix + "form1").serialize();
        var id = $("#" + edit.prifix + "form1 input[name='Id']").val();
        var url = bootPATH + "../Ac/Catalog/Create";
        if (id) {
            url = bootPATH + "../Ac/Catalog/Update";
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
                url: bootPATH + "../Ac/Catalog/Get",
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
        bootPATH + "../Ac/Catalog/Create",
        bootPATH + "../Ac/Catalog/Update",
        bootPATH + "../Ac/Catalog/Get",
        form, edit);
})(window);