/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
(function (window) {
    mini.namespace("Edi.Node.Index");
    var self = Edi.Node.Index;
    self.prifix = "Edi_Node_Index_";
    self.sortUrl = bootPATH + "../Edi/Node/UpdateSortCode";
    self.help = { appSystemCode: "Anycmd", areaCode: "Edi", resourceCode: "Node", functionCode: "Index" };
    helper.helperSplitterInOne(self);
    self.gridReload = function () {
        grid.reload();
    };
    self.search = search;
    self.edit = function () {
        tabs1.activeTab(tabs1.getTab("editTab"));
    }
    self.add = function () {
        var newRow = grid.getRow(0);
        if (!newRow || !newRow.isNewRow) {
            grid.deselectAll();
            newRow = { isNewRow: true, Id: "-1" };
            grid.addRow(newRow, 0);
            grid.select(newRow);
        }
    };

    var nodeId = $.deparam.fragment().nodeId || $.deparam.querystring().nodeId || '';

    var tabConfigs = {
        infoTab: {
            url: bootPATH + "../Edi/Node/Details",
            params: [{ "pName": 'id', "pValue": "Id" }],
            namespace: "Node.Details"
        },
        editTab: {
            url: bootPATH + "../Edi/Node/Edit",
            params: [{ "pName": 'id', "pValue": "Id" }],
            namespace: "Node.Edit"
        },
        elementCareTab: {
            url: bootPATH + "../Edi/Node/NodeElementCares",
            params: [{ "pName": 'nodeId', "pValue": "Id" }],
            namespace: "Node.ElementCares"
        },
        permissionTab: {
            url: bootPATH + "../Edi/Node/Permissions",
            params: [{ "pName": 'nodeId', "pValue": "Id" }],
            namespace: "Node.Permissions"
        },
        catalogTab: {
            url: bootPATH + "../Edi/Node/Catalogs",
            params: [{ "pName": 'nodeId', "pValue": "Id" }],
            namespace: "Node.Catalogs"
        },
        operationLogTab: {
            url: bootPATH + "../Ac/OperationLog/Index",
            params: [{ "pName": 'targetId', "pValue": "Id" }],
            namespace: "Ac.OperationLog.Index"
        }
    };

    mini.parse();

    var btnSave = mini.get(self.prifix + "btnSave");
    if (btnSave) {
        btnSave.on("click", saveData);
    }
    var tabs1 = mini.get(self.prifix + "tabs1");
    var grid = mini.get(self.prifix + "datagrid1");
    grid.on("drawcell", ondrawcell);
    var helperDrawcell = helper.ondrawcell(self, "Edi.Node.Index");
    grid.on("load", helper.onGridLoad);
    grid.load();
    grid.sortBy("SortCode", "asc");

    helper.index.allInOne(
        null,
        grid,
        bootPATH + "../Edi/Node/Edit",
        bootPATH + "../Edi/Node/Edit",
        bootPATH + "../Edi/Node/Delete",
        self);
    helper.index.tabInOne(grid, tabs1, tabConfigs, self);

    function search() {
        var data = { };
        grid.load(data, function () {
            var record = grid.getSelected();
            if (!record) {
                tabs1.hide();
            }
        });
    }

    function saveData() {
        var data = grid.getChanges();
        var json = mini.encode(data);

        grid.loading("保存中，请稍后......");
        $.ajax({
            url: bootPATH + "../Edi/Node/UpdateNodes",
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
                case "IsEnabled":
                case "IsServiceAlive":
                    if (value == "正常" || value == "1" || value == "是" || value == "true") {
                        e.cellHtml = "<span class='icon-enabled width16px'></span>";
                    } else if (value == "禁用" || value == "0" || value == "否" || value == "false") {
                        e.cellHtml = "<span class='icon-disabled width16px'></span>";
                    } break;
                case "Name":
                    if (record.IsSelf) {
                        value = "<span title='自己' class='icon-pkey width16px'></span>" + value;
                    }
                    if(record.IsCenter) {
                        value = "<span title='中心' class='icon-pkey width16px'></span>" + value;
                    }
                    e.cellHtml = value;
                    break;
                case "Icon":
                    if (value) {
                        e.cellHtml = "<img src='" + bootPATH + "../Content/icons/16x16/" + value + "'></img>";
                    }
                    break;
            }
        }
        helperDrawcell(e);
    }

    function GetIconUrl(value) {
        var i = value.lastIndexOf('/');
        if (i > 0) {
            value = value.substring(0, value.lastIndexOf('/') + 1);
        }
        return value + "isOnLine.png?t=" + Math.random().toString();
    }
})(window);