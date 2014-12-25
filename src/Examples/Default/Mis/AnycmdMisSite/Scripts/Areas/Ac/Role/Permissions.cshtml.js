/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window, $) {
    mini.namespace("Role.Permissions");
    var self = Role.Permissions;
    self.prifix = "Ac_Role_Permissions_";
    self.loadData = loadData;
    var currentNode;

    self.areaFilters = {};

    mini.parse();

    var btnSearch = mini.get(self.prifix + "btnSearch");
    btnSearch.on("click", search);
    var rbIsAssigned = mini.get(self.prifix + "rbIsAssigned");
    rbIsAssigned.on("valuechanged", loadData);
    var keyResource = mini.get(self.prifix + "keyResource");
    var btnSearchResource = mini.get(self.prifix + "btnSearchResource");
    keyResource.on("enter", searchResource);
    btnSearchResource.on("click", searchResource);
    var keyPermission = mini.get(self.prifix + "keyPermission");
    var btnSearchPermission = mini.get(self.prifix + "btnSearchPermission");
    keyPermission.on("enter", searchPermission);
    btnSearchPermission.on("click", searchPermission);
    var btnSave = mini.get(self.prifix + "btnSave");
    if (btnSave) {
        btnSave.on("click", saveData);
    }
    var key = mini.get(self.prifix + "key");
    key.on("enter", search);
    var dgAppSystem = mini.get(self.prifix + "dgAppSystem");
    var splitter = mini.get(self.prifix + "splitter");
    var tabs1 = mini.get(self.prifix + "tabs1");
    var dgResource = mini.get(self.prifix + "dgResource");
    dgResource.on("selectionchanged", onDgResourceSelectionChanged);
    dgResource.on("drawcell", onDgResourceDrawcell);
    dgResource.on("load", function (e) {
        if (e.data.length != 0) {
            e.sender.addRow({ Name: '全部', SortCode: '-1', Code: '' }, 0);
        }
        var record = e.sender.getSelected();
        if (!record) {
            tabs1.hide();
        }
        else {
            tabs1.show();
        }
        helper.onGridLoad(e);
    });
    var grid = mini.get(self.prifix + "datagrid1");
    grid.on("load", helper.onGridLoad);
    dgAppSystem.on("load", helper.onGridLoad);
    dgAppSystem.on("selectionchanged", onDgAppSystemSelectionChanged);
    dgAppSystem.load();
    dgAppSystem.sortBy("SortCode", "asc");

    function search() {
        var data = { key: key.getValue() };
        dgAppSystem.load(data);
    }

    function loadData() {
        searchPermission();
    }

    function getParams() {
        data = { isAssigned: rbIsAssigned.getValue() };
        var resourceRecord = dgResource.getSelected();
        var appSystemRecord = dgAppSystem.getSelected();
        if (resourceRecord) {
            data.appSystemId = appSystemRecord.Id;
            data.resourceTypeId = resourceRecord.Id;
        }
        if (self.params && self.params.roleId) {
            data.roleId = self.params.roleId;
        }
        else {
            data.roleId = $.deparam.fragment().roleId || $.deparam.querystring().roleId;
        }
        return data;
    }

    function onDgResourceSelectionChanged(e) {
        loadData();
    }

    function onDgAppSystemSelectionChanged(e) {
        searchResource();
    }

    function searchResource() {
        var appSystemRecord = dgAppSystem.getSelected();
        var data = { appSystemId: appSystemRecord.Id, key: keyResource.getValue(), IsAssigned: true };
        if (!dgResource.sortField) {
            dgResource.sortBy("SortCode", "asc");
        }
        dgResource.setPageIndex(0);
        dgResource.load(data);
    }

    function searchPermission() {
        var data = getParams();
        data.key = keyPermission.getValue();
        if (data.appSystemId) {
            tabs1.show();
            if (!grid.sortField) {
                grid.sortBy("SortCode", "asc");
            }
            grid.load(data);
        }
        else {
        }
    }

    function saveData() {
        var data = grid.getChanges();
        var json = mini.encode(data);

        grid.loading("保存中，请稍后......");
        $.ajax({
            url: bootPATH + "../Ac/Role/GrantOrDenyRoleFunctions",
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
    function onDgResourceDrawcell(e) {
        var field = e.field;
        var value = e.value;
        var record = e.record;
        if (field) {
            switch (field) {
                case "IsEnabled":
                    if (value == "正常" || value == "1" || value == "是" || value == "true") {
                        e.cellHtml = "<span class='icon-enabled width16px'></span>";
                    } else if (value == "禁用" || value == "0" || value == "否" || value == "false") {
                        e.cellHtml = "<span class='icon-disabled width16px'></span>";
                    } break;
                case "Icon":
                    if (value) {
                        e.cellHtml = "<img src='" + bootPATH + "../Content/icons/16x16/" + value + "'></img>";
                    }
                    break;
            }
        }
    }
})(window, jQuery);