/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../helper.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window, $) {
    mini.namespace("Role.Accounts");
    var self = Role.Accounts;
    self.prifix = "Ac_Role_Accounts_";
    self.loadData = loadData;
    self.remove = remove;

    window.onSelectOk = onSelectOk;

    mini.parse();

    var btnSearch = mini.get(self.prifix + "btnSearch");
    btnSearch.on("click", loadData);
    var btnAddTo = mini.get(self.prifix + "btnAddTo");
    if (btnAddTo) {
        btnAddTo.on("click", function () {
            var win = mini.get("Select_win1");
            win.show();
        });
    }
    var btnRemoveFrom = mini.get(self.prifix + "btnRemoveFrom");
    if (btnRemoveFrom) {
        btnRemoveFrom.on("click", function () {
            var records = grid.getSelecteds();
            if (records.length > 0) {
                mini.confirm("确定移除选中记录？", "确定？", function (action) {
                    if (action == "ok") {
                        self.remove(records);
                    }
                });
            } else {
                mini.alert("请先选中记录");
            }
        });
    }
    var key = mini.get(self.prifix + "key");
    key.on("enter", loadData);
    var tabs1 = mini.get(self.prifix+"tabs1");
    var grid = mini.get(self.prifix + "datagrid1");
    grid.on("drawcell", ondrawcell);
    grid.on("load", helper.onGridLoad);

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
            $.post(bootPATH + "../Ac/Role/RemoveRoleAccounts", { id: id }, function (result) {
                helper.response(result, function () {
                    grid.reload();
                }, function () {
                    grid.unmask();
                });
            }, "json");
        }
    }

    function onSelectOk(accountDic) {
        var data = getParams();
        data.accountIds = accountDic.accountIds;
        $.post(bootPATH + "../Ac/Role/AddRoleAccounts"
            , data
            , function (result) {
                helper.response(result, function () {
                    grid.reload();
                });
            }, "json");
    }

    function loadData() {
        var data = getParams();
        if (!grid.sortField) {
            grid.sortBy("CreateOn", "asc");
        }
        grid.load(data);
    }

    function getParams() {
        data = { key: key.getValue() };
        if (self.params && self.params.roleId) {
            data.roleId = self.params.roleId;
        }
        else {
            data.roleId = $.deparam.fragment().roleId || $.deparam.querystring().roleId;
        }
        return data;
    }

    function ondrawcell(e) {
        var field = e.field;
        var value = e.value;
        var columnName = e.column.name;
        if (field) {
            switch (field) {
                case "IsEnabled":
                    if (value == "正常" || value == "1") {
                        e.cellHtml = "<span class='icon-enabled width16px'></span>";
                    } else if (value == "禁用" || value == "0") {
                        e.cellHtml = "<span class='icon-disabled width16px'></span>";
                    } break;
            }
        }
        if (columnName && columnName == "action") {
            var record = e.record;
            e.cellHtml = '<a title="删除" href="javascript:Role.Accounts.remove(\'' + record.Id + '\')"><img alt="删除" border="0" src="' + bootPATH + '../Scripts/miniui/themes/icons/remove.gif" /></a>';
        }
    };
})(window, jQuery);