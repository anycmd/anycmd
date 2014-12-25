/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window, $) {
    mini.namespace("Ac.DsdSet.Index");
    var self = Ac.DsdSet.Index;
    self.prifix = "Ac_DsdSet_Index_";
    self.search = search;
    self.gridReload = function () {
        grid.reload();
    };
    self.help = { appSystemCode: "Anycmd", areaCode: "Ac", resourceCode: "DsdSet", functionCode: "Index" };
    helper.helperSplitterInOne(self);
    mini.namespace("DsdSet.Edit");
    var edit = DsdSet.Edit;
    edit.prifix = "Ac_DsdSet_Index_Edit_";
    var faceInitialized = false;

    var tabConfigs = {
        infoTab: {
            url: bootPATH + "../Ac/DsdSet/Details",
            params: [{ "pName": 'id', "pValue": "Id" }],
            namespace: "DsdSet.Details"
        },
        roleTab: {
            url: bootPATH + "../Ac/DsdSet/Roles",
            params: [{ "pName": 'dsdSetId', "pValue": "Id" }],
            namespace: "Ac.DsdSet.Roles"
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
        IsEnabled: {
            type: 'numeric',
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
    grid.on("drawcell", helper.ondrawcell(self, "Ac.DsdSet.Index"));
    grid.on("load", helper.onGridLoad);
    grid.sortBy("CreateOn", "asc");

    function search() {
        var data = {};
        var filterArray = [];
        for (var k in self.filters) {
            var filter = self.filters[k];
            if (filter.value) {
                filterArray.push({ field: k, type: filter.type, comparison: filter.comparison, value: filter.value });
            }
        }
        data.filters = JSON.stringify(filterArray);
        grid.load(data);
    }

    helper.index.allInOne(
        edit,
        grid,
        bootPATH + "../Ac/DsdSet/Edit",
        bootPATH + "../Ac/DsdSet/Edit",
        bootPATH + "../Ac/DsdSet/Delete",
        self);
    helper.index.tabInOne(grid, tabs1, tabConfigs, self);

    helper.edit.allInOne(
        self,
        win,
        bootPATH + "../Ac/DsdSet/Create",
        bootPATH + "../Ac/DsdSet/Update",
        bootPATH + "../Ac/DsdSet/Get",
        form, edit);
})(window, jQuery);