/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../helper.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    mini.namespace("Group.Roles");
    var self = Group.Roles;
    self.prifix = "Ac_Group_Roles_";
    self.loadData = loadData;

    mini.parse();

    var rbIsAssigned = mini.get(self.prifix + "rbIsAssigned");
    rbIsAssigned.on("valuechanged", loadData);
    var btnSave = mini.get(self.prifix + "btnSave");
    if (btnSave) {
        btnSave.on("click", saveData);
    }
    var btnSearch = mini.get(self.prifix + "btnSearch");
    btnSearch.on("click", loadData);
    var key = mini.get(self.prifix + "key");
    key.on("enter", loadData);
    var grid = mini.get(self.prifix + "datagrid1");
    var helperDrawcell = helper.ondrawcell(self, "Ac.Group.Roles");
    grid.on("drawcell", ondrawcell);
    grid.on("load", helper.onGridLoad);

    function onNodeSelect(e) {
        loadData();
    }

    function loadData() {
        if (!grid.sortField) {
            grid.sortBy("SortCode", "asc");
        }
        grid.load(getParams());
    }

    function getParams() {
        var data = { key: key.getValue(), isAssigned: rbIsAssigned.getValue() };
        if (self.params && self.params.groupId) {
            data.groupId = self.params.groupId;
        }
        else {
            data.groupId = $.deparam.fragment().groupId || $.deparam.querystring().groupId;
        }
        return data;
    }
    function saveData() {
        var data = grid.getChanges();
        var json = mini.encode(data);

        grid.loading("保存中，请稍后......");
        $.ajax({
            url: bootPATH + "../Ac/Group/GrantOrDenyRoles",
            data: { data: json },
            type: "post",
            success: function (result) {
                helper.response(result, function () {
                    grid.reload();
                }, function () {
                    grid.unmask();
                });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                grid.unmask();
                mini.alert(jqXHR.responseText);
            }
        });
    }
    function ondrawcell(e) {
        var field = e.field;
        var value = e.value;
        var record = e.record;
        if (field) {
            switch (field) {
                case "Name":
                    if (record.Icon) {
                        e.cellHtml = "<img src='" + bootPATH + "../Content/icons/16x16/" + record.Icon + "'></img>" + value;
                    }
                    else {
                        e.cellHtml = value;
                    }
                    break;
            }
        }
        helperDrawcell(e);
    }
})(window);