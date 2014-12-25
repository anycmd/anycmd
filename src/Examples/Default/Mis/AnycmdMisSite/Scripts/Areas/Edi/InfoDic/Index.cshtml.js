/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
(function (window) {
    mini.namespace("Edi.InfoDic.Index");
    var self = Edi.InfoDic.Index;
    self.prifix = "Edi_InfoDic_Index_";
    self.sortUrl = bootPATH + "../Edi/InfoDic/UpdateSortCode";
    self.help = { appSystemCode: "Anycmd", areaCode: "Edi", resourceCode: "InfoDic", functionCode: "Index" };
    helper.helperSplitterInOne(self);
    self.gridReload = function () {
        grid.reload();
    };
    self.search = search;
    mini.namespace("InfoDic.Edit");
    var edit = InfoDic.Edit;
    edit.prifix = "Edi_InfoDic_Index_Edit_";

    var tabConfigs = {
        infoTab: {
            url: bootPATH + "../Edi/InfoDic/Details",
            params: [{ "pName": 'id', "pValue": "Id" }],
            namespace: "InfoDic.Details"
        },
        dicItemTab: {
            url: bootPATH + "../Edi/InfoDicItem/Index",
            params: [{ "pName": 'infoDicId', "pValue": "Id" }],
            namespace: "Edi.InfoDicItem.Index"
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
        Code: {
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
    var helperDrawcell = helper.ondrawcell(self, "Edi.InfoDic.Index");
    grid.on("drawcell", ondrawcell);
    grid.on("load", helper.onGridLoad);
    grid.load();
    grid.sortBy("SortCode", "asc");

    helper.index.allInOne(
        edit,
        grid,
        bootPATH + "../Edi/InfoDic/Edit",
        bootPATH + "../Edi/InfoDic/Edit",
        bootPATH + "../Edi/InfoDic/Delete",
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
        bootPATH + "../Edi/InfoDic/Create",
        bootPATH + "../Edi/InfoDic/Update",
        bootPATH + "../Edi/InfoDic/Get",
        form, edit);
})(window);