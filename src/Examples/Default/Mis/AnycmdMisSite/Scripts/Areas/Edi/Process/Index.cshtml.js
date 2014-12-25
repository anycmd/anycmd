/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
(function (window) {
    mini.namespace("Edi.Process.Index");
    var self = Edi.Process.Index;
    self.prifix = "Edi_Process_Index_";
    self.help = { appSystemCode: "Anycmd", areaCode: "Edi", resourceCode: "Process", functionCode: "Index" };
    helper.helperSplitterInOne(self);
    self.gridReload = function () {
        grid.reload();
    };
    self.search = search;
    mini.namespace("Process.Edit");
    var edit = Process.Edit;
    edit.prifix = "Edi_Process_Index_Edit_";

    var tabConfigs = {
        infoTab: {
            url: bootPATH + "../Edi/Process/Details",
            params: [{ "pName": 'id', "pValue": "Id" }],
            namespace: "Process.Details"
        }
    };
    self.filters = {
        Name: {
            type: 'string',
            comparison: 'like'
        },
        Type: {
            type: 'string',
            comparison: 'like'
        },
        IsEnabled: {
            type: 'numeric',
            comparison: 'eq'
        },
        OntologyCode: {
            type: 'string',
            comparison: 'like'
        },
        OntologyName: {
            type: 'string',
            comparison: 'like'
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
    var helperDrawcell = helper.ondrawcell(self, "Edi.Process.Index");
    grid.on("drawcell", ondrawcell);
    grid.on("load", helper.onGridLoad);
    grid.load();
    grid.sortBy("Name", "asc");

    helper.index.allInOne(
        edit,
        grid,
        bootPATH + "../Edi/Process/Edit",
        bootPATH + "../Edi/Process/Edit",
        bootPATH + "../Edi/Process/DeleteProcess",
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
        var columnName = e.column.name;
        helperDrawcell(e);
    };

    helper.edit.allInOne(
        self,
        win,
        bootPATH + "../Edi/Process/Create",
        bootPATH + "../Edi/Process/Update",
        bootPATH + "../Edi/Process/Get",
        form, edit);
})(window);