/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    mini.namespace("Ac.SsdSet.Roles");
    var self = Ac.SsdSet.Roles;
    self.prifix = "Ac_SsdSet_Roles_";
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
    grid.on("drawcell", helper.ondrawcell(self, "Ac.SsdSet.Roles"));
    grid.on("load", helper.onGridLoad);
    loadData();

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
        var data = { key: key.getValue(), isAssigned: rbIsAssigned.getValue() };
        if (self.params && self.params.ssdSetId) {
            data.ssdSetId = self.params.ssdSetId;
        }
        else {
            data.ssdSetId = $.deparam.fragment().ssdSetId || $.deparam.querystring().ssdSetId;
        }
        return data;
    }

    function saveData() {
        var data = grid.getChanges();
        var json = mini.encode(data);

        grid.loading("保存中，请稍后......");
        $.ajax({
            url: bootPATH + "../Ac/SsdSet/AddOrDeleteRoleMembers",
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
})(window);