/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
(function (window) {
    mini.namespace("Edi.StateCode.Index");
    var self = Edi.StateCode.Index;
    self.prifix = "Edi_StateCode_Index_";
    self.help = { appSystemCode: "Anycmd", areaCode: "Edi", resourceCode: "StateCode", functionCode: "Index" };
    helper.helperSplitterInOne(self);
    self.gridReload = function () {
        grid.reload();
    };
    self.search = search;
    mini.namespace("StateCode.Edit");
    var edit = StateCode.Edit;
    edit.prifix = "Edi_StateCode_Index_Edit_";

    var tabConfigs = {
        infoTab: {
            url: bootPATH + "../Edi/StateCode/Details",
            params: [{ "pName": 'id', "pValue": "Id" }],
            namespace: "StateCode.Details"
        }
    };
    self.filters = {
        ReasonPhrase: {
            type: 'string',
            comparison: 'like'
        },
        Description: {
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
    grid.on("load", helper.onGridLoad);
    grid.load();
    grid.sortBy("Code", "asc");

    helper.index.allInOne(
        edit,
        grid,
        bootPATH + "../Edi/StateCode/Edit",
        bootPATH + "../Edi/StateCode/Edit",
        bootPATH + "../Edi/StateCode/DeleteStateCode",
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

    helper.edit.allInOne(
        self,
        win,
        bootPATH + "../Edi/StateCode/Create",
        bootPATH + "../Edi/StateCode/Update",
        bootPATH + "../Edi/StateCode/Get",
        form, edit);
})(window);