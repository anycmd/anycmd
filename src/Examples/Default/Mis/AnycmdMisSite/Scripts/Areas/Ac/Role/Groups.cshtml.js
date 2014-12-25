/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window, $) {
    mini.namespace("Role.Groups");
    var self = Role.Groups;
    self.prifix = "Ac_Role_Groups_";
    self.loadData = loadData;

    mini.parse();

    var btnSave = mini.get(self.prifix + "btnSave");
    if (btnSave) {
        btnSave.on("click", saveData);
    }
    var rbIsAssigned = mini.get(self.prifix + "rbIsAssigned");
    rbIsAssigned.on("valuechanged", loadData);
    var grid = mini.get(self.prifix + "datagrid1");
    grid.on("drawcell", helper.ondrawcell(self, "Role.Groups"));
    grid.on("load", helper.onGridLoad);

    function onCategoryValuechanged(e) {
        loadData();
    }

    function loadData() {
        if (!grid.sortField) {
            grid.sortBy("SortCode", "asc");
        }
        var data = getParams();
        grid.load(data);
    }

    function getParams() {
        data = { isAssigned: rbIsAssigned.getValue() };
        if (self.params && self.params.roleId) {
            data.roleId = self.params.roleId;
        }
        else {
            data.roleId = $.deparam.fragment().roleId || $.deparam.querystring().roleId;
        }
        return data;
    }

    function saveData() {
        var data = grid.getChanges();
        var json = mini.encode(data);

        grid.loading("保存中，请稍后......");
        $.ajax({
            url: bootPATH + "../Ac/Role/GrantOrDenyGroups",
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
})(window, jQuery);