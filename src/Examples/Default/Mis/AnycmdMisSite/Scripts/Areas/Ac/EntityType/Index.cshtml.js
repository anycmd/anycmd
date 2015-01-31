/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
(function (window) {
    mini.namespace("Ac.EntityType.Index");
    var self = Ac.EntityType.Index;
    self.prifix = "Ac_EntityType_Index_";
    self.sortUrl = bootPATH + "../Ac/EntityType/UpdateSortCode";
    self.help = { appSystemCode: "Anycmd", areaCode: "Ac", resourceCode: "EntityType", functionCode: "Index" };
    helper.helperSplitterInOne(self);
    self.search = search;
    self.gridReload = function () {
        grid.reload();
    };
    mini.namespace("EntityType.Edit");
    var edit = EntityType.Edit;
    edit.prifix = "Ac_EntityType_Index_Edit_";

    var tabConfigs = {
        infoTab: {
            url: bootPATH + "../Ac/EntityType/Details",
            params: [{ "pName": 'id', "pValue": "Id" }],
            namespace: "EntityType.Details"
        },
        propertyTab: {
            url: bootPATH + "../Ac/Property/Index",
            params: [{ "pName": 'entityTypeId', "pValue": "Id" }],
            namespace: "Ac.Property.Index"
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
        Codespace: {
            type: 'string',
            comparison: 'like'
        },
        Code: {
            type: 'string',
            comparison: 'like'
        },
        ClrTypeFullName: {
            type: 'string',
            comparison: 'like'
        },
        DeveloperId: {
            type: 'guid',
            comparison: 'eq'
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
    var helperDrawcell = helper.ondrawcell(self, "Ac.EntityType.Index");
    grid.on("drawcell", ondrawcell);
    grid.on("load", helper.onGridLoad);
    grid.load({ appSystemCode: 'Anycmd' });
    grid.sortBy("SortCode", "asc");

    helper.index.allInOne(
        edit,
        grid,
        bootPATH + "../Ac/EntityType/Edit",
        bootPATH + "../Ac/EntityType/Edit",
        bootPATH + "../Ac/EntityType/Delete",
        self);
    helper.index.tabInOne(grid, tabs1, tabConfigs, self);

    function search() {
        var data = {
            appSystemCode: 'Anycmd'
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

    function onKeyEnter(e) {
        search();
    }

    function ondrawcell(e) {
        var field = e.field;
        var value = e.value;
        var record = e.record;
        if (field) {
            switch (field) {
                case "IsCatalogued":
                    if (value == "正常" || value == "1" || value == "是" || value == "true" || value == true) {
                        e.cellHtml = "<span class='icon-enabled width16px'></span>";
                    } else if (value == "禁用" || value == "0" || value == "否" || value == "false" || value == false) {
                        e.cellHtml = "<span class='icon-disabled width16px'></span>";
                    } break;
            }
        }
        helperDrawcell(e);
    }

    helper.edit.allInOne(
        self,
        win,
        bootPATH + "../Ac/EntityType/Create",
        bootPATH + "../Ac/EntityType/Update",
        bootPATH + "../Ac/EntityType/Get",
        form, edit);
})(window);