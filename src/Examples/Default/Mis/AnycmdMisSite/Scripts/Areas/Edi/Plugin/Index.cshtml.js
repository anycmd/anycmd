/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
(function (window) {
    mini.namespace("Edi.Plugin.Index");
    var self = Edi.Plugin.Index;
    self.prifix = "Edi_Plugin_Index_";
    self.help = { appSystemCode: "Anycmd", areaCode: "Edi", resourceCode: "Plugin", functionCode: "Index" };
    helper.helperSplitterInOne(self);
    self.gridReload = function () {
        grid.reload();
    };
    self.search = search;
    mini.namespace("Plugin.Edit");
    var edit = Plugin.Edit;
    edit.prifix = "Edi_Plugin_Index_Edit_";

    var tabConfigs = {
        infoTab: {
            url: bootPATH + "../Edi/Plugin/Details",
            params: [{ "pName": 'id', "pValue": "Id" }],
            namespace: "Plugin.Details"
        },
        operationLogTab: {
            url: bootPATH + "../Ac/OperationLog/Index",
            params: [{ "pName": 'targetId', "pValue": "Id" }],
            namespace: "Ac.OperationLog.Index"
        }
    };
    self.filters = {
        Title: {
            type: 'string',
            comparison: 'like'
        },
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
    var helperDrawcell = helper.ondrawcell(self, "Edi.Plugin.Index");
    grid.on("drawcell", ondrawcell);
    grid.on("load", helper.onGridLoad);
    grid.load();
    grid.sortBy("Name", "asc");

    helper.index.allInOne(
        edit,
        grid,
        bootPATH + "../Edi/Plugin/Edit",
        bootPATH + "../Edi/Plugin/Edit",
        bootPATH + "../Edi/Plugin/DeletePlugin",
        self);
    helper.index.tabInOne(grid, tabs1, tabConfigs, self);

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
            }
        }
        helperDrawcell(e);
    }

    helper.edit.allInOne(
        self,
        win,
        bootPATH + "../Edi/Plugin/Create",
        bootPATH + "../Edi/Plugin/Update",
        bootPATH + "../Edi/Plugin/Get",
        form, edit);
})(window);