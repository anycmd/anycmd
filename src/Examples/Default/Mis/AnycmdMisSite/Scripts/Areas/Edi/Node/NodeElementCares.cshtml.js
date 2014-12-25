/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    mini.namespace("Node.ElementCares");
    var self = Node.ElementCares;
    self.prifix = "Edi_Node_NodeElementCares_";
    self.loadData = loadData;
    var nodeId = $.deparam.fragment().nodeId || $.deparam.querystring().nodeId || '';

    var elementFilters = {
        IsAssigned: {
            type: 'boolean',
            comparison: 'eq'
        },
        IsInfoIdItem: {
            type: 'boolean',
            comparison: 'eq'
        },
        ElementIsInfoIdItem: {
            type: 'boolean',
            comparison: 'eq'
        },
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

    for (var k in elementFilters) {
        var id = k + "Filter";
        if (self.prifix) {
            id = self.prifix + id;
        }
        var filterBox = mini.get(id);
        elementFilters[k].filterBox = filterBox;
        filterBox.on("valuechanged", helper.index.onFilterChanged(elementFilters, search));
    }
    var btnSearchClear = mini.get(self.prifix + "btnSearchClear");
    btnSearchClear.on("click", function () {
        helper.index.clearSearch(elementFilters, search);
    });

    var btnOntologySave = mini.get(self.prifix + "btnOntologySave");
    if (btnOntologySave) {
        btnOntologySave.on("click", saveOntologyData);
    }

    var btnElementSave = mini.get(self.prifix + "btnElementSave");
    btnElementSave.on("click", saveData);
    var grid = mini.get(self.prifix + "datagrid1");
    var dgOntology = mini.get(self.prifix + "dgOntology");
    dgOntology.on("drawcell", ondrawcell);
    dgOntology.on("load", helper.onGridLoad);
    dgOntology.on("selectionchanged", function (e) {
        var record = dgOntology.getSelected();
        search();
        $("#" + self.prifix + "ontologyName").text(record.Name);
    });
    grid.on("drawcell", ondrawcell);
    grid.on("load", helper.onGridLoad);

    function loadOntologyData() {
        var data = {
            nodeId: getParams().nodeId
        };
        if (!dgOntology.sortField) {
            dgOntology.sortBy("SortCode", "asc");
        }
        dgOntology.load(data);
    }

    function loadData() {
        loadOntologyData();
    }

    function getParams() {
        var data = {};
        if (self.params && self.params.nodeId) {
            data.nodeId = self.params.nodeId;
        }
        else {
            var fragment = $.deparam.fragment();
            var querystring = $.deparam.querystring();
            data.nodeId = fragment.nodeId || querystring.nodeId;
        }
        return data;
    }

    function onCategoryValuechanged(e) {
        search();
    }

    function search() {
        var ontologyRecord = dgOntology.getSelected();
        if (ontologyRecord) {
            var data = {
                nodeId: getParams().nodeId,
                ontologyId: ontologyRecord.OntologyId
            };
            var filterArray = [];
            for (var k in elementFilters) {
                var filter = elementFilters[k];
                var field = k;
                if (filter.field) {
                    field = filter.field;
                }
                if (filter.value) {
                    filterArray.push({ field: field, type: filter.type, comparison: filter.comparison, value: filter.value });
                }
            }
            data.filters = JSON.stringify(filterArray);
            if (!grid.sortField) {
                grid.sortBy("SortCode", "asc");
            }
            grid.load(data);
        }
    }

    function saveOntologyData() {
        var data = dgOntology.getChanges();
        var json = mini.encode(data);

        dgOntology.loading("保存中，请稍后......");
        $.ajax({
            url: bootPATH + "../Edi/Node/AddOrRemoveOntologies?nodeId=" + nodeId,
            data: { data: json },
            type: "post",
            success: function (result) {
                helper.response(result, function () {
                    dgOntology.reload();
                });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                mini.alert(jqXHR.responseText);
            }
        });
    }

    function saveData() {
        var data = grid.getChanges();
        var json = mini.encode(data);

        grid.loading("保存中，请稍后......");
        $.ajax({
            url: bootPATH + "../Edi/Node/AddOrRemoveElementCares?nodeId=" + nodeId,
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
        var grid = e.sender;
        var field = e.field;
        var value = e.value;
        var record = e.record;
        if (field) {
            switch (field) {
                case "IsEnabled":
                case "ElementIsInfoIdItem":
                    if (value == true || value == "正常" || value == "1") {
                        e.cellHtml = "<span class='icon-enabled width16px'></span>";
                    } else if (value == false || value == "禁用" || value == "0") {
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
})(window);