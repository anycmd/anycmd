/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window, $) {
    mini.namespace("Ac.RDatabase.TableColumns");
    var self = Ac.RDatabase.TableColumns;
    self.prifix = "Ac_RDatabase_TableColumns_";
    self.loadData = loadData;
    self.search = search;
    self.gridReload = function () {
        grid.reload();
    };
    mini.namespace("Ac.RDatabase.TableColumns.TableColumnEdit");
    var edit = Ac.RDatabase.TableColumns.TableColumnEdit;
    edit.SetData = SetData;
    edit.prifix = "Ac_RDatabase_TableColumns_TableColumnEdit_"

    self.filters = {
        Description: {
            type: 'string',
            comparison: 'like'
        },
        Name: {
            type: 'string',
            comparison: 'like'
        },
        TypeName: {
            type: 'string',
            comparison: 'like'
        },
        IsNullable: {
            type: 'boolean',
            comparison: 'eq'
        },
        IsIdentity: {
            type: 'boolean',
            comparison: 'eq'
        },
        IsStoreGenerated: {
            type: 'boolean',
            comparison: 'eq'
        },
        IsPrimaryKey: {
            type: 'boolean',
            comparison: 'eq'
        }
    };

    mini.parse();

    var win = mini.get(edit.prifix + "win1");
    var form;
    if (win) {
        form = new mini.Form(edit.prifix + "form1");
    }

    var grid = mini.get(self.prifix + "datagrid1");
    grid.on("drawcell", ondrawcell);
    var helperDrawcell = helper.ondrawcell(self, "Ac.RDatabase.TableColumns");
    grid.on("load", helper.onGridLoad);

    helper.index.allInOne(
        edit,
        grid,
        null,
        bootPATH + "../Ac/RDatabase/Edit",
        null,
        self);

    function loadData() {
        search();
    }

    function getParams() {
        var data = {};
        if (self.params && self.params.databaseId) {
            data.databaseId = self.params.databaseId;
            data.schemaName = self.params.schemaName;
            data.tableName = self.params.tableName;
            data.tableId = self.params.tableId;
        }
        else {
            var fragment = $.deparam.fragment();
            var querystring = $.deparam.querystring();
            data.databaseId = fragment.databaseId || querystring.databaseId;
            data.schemaName = fragment.schemaName || querystring.schemaName;
            data.tableName = fragment.tableName || querystring.tableName;
            data.tableId = fragment.tableId || querystring.tableId;
        }
        return data;
    }

    function search() {
        var data = getParams();
        var filterArray = [];
        for (var k in self.filters) {
            var filter = self.filters[k];
            if (filter.value) {
                filterArray.push({ field: k, type: filter.type, comparison: filter.comparison, value: filter.value });
            }
        }
        data.filters = JSON.stringify(filterArray);
        if (!grid.sortField) {
            grid.sortBy("Ordinal", "asc");
        }
        grid.load(data);
    }

    function ondrawcell(e) {
        var field = e.field;
        var value = e.value;
        var record = e.record;
        if (field) {
            switch (field) {
                case "IsNullable":
                case "IsIdentity":
                case "IsStoreGenerated":
                case "IsPrimaryKey":
                    if (value == "正常" || value == "1" || value == "是" || value == "true") {
                        e.cellHtml = "<span class='icon-enabled width16px'></span>";
                    } else if (value == "禁用" || value == "0" || value == "否" || value == "false") {
                        e.cellHtml = "<span class='icon-disabled width16px'></span>";
                    } break;
            }
        }
        helperDrawcell(e);
    }
    function SetData(data) {
        //跨页面传递的数据对象，克隆后才可以安全使用
        data = mini.clone(data);
        if (data.action == "edit") {
            $.ajax({
                url: bootPATH + "../Ac/RDatabase/GetTableColumn",
                data: { id: data.id, databaseId: getParams().databaseId },
                cache: false,
                success: function (result) {
                    helper.response(result, function () {
                        form.setData(result);
                        form.validate();
                    });
                }
            });
        }
        else if (data.action == "new") {
            form.setData(data);
        }
    }
    helper.edit.allInOne(
        self,
        win,
        bootPATH + "../Ac/RDatabase/UpdateTableColumn",
        bootPATH + "../Ac/RDatabase/UpdateTableColumn",
        bootPATH + "../Ac/RDatabase/GetTableColumn",
        form, edit);
})(window, jQuery);