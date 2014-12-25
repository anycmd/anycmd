/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
(function (window) {
    mini.namespace("Edi.Element.Index");
    var self = Edi.Element.Index;
    self.prifix = "Edi_Element_Index_";
    self.search = search;
    self.sortUrl = bootPATH + "../Edi/Element/UpdateSortCode";
    self.help = { appSystemCode: "Anycmd", areaCode: "Edi", resourceCode: "Element", functionCode: "Index" };
    helper.helperSplitterInOne(self);
    self.gridReload = function () {
        grid.reload();
    };
    mini.namespace("Element.Edit");
    var edit = Element.Edit;
    edit.prifix = "Edi_Element_Index_Edit_";
    edit.SetData = SetData;

    var ontologyId;
    var faceInitialized = false;

    var currentOntologyRecord;
    var currentInfoGroupRecord;
    var tabConfigs = {
        infoTab: {
            url: bootPATH + "../Edi/Element/Details",
            params: [{ "pName": 'id', "pValue": "Id" }],
            namespace: "Element.Details"
        },
        InfoRuleTab: {
            url: bootPATH + "../Edi/InfoRule/ElementInfoRules",
            params: [{ "pName": 'elementId', "pValue": "Id" }],
            namespace: "InfoRule.ElementInfoRules"
        },
        checkTab: {
            url: bootPATH + "../Edi/Element/Checks",
            params: [{ "pName": 'elementId', "pValue": "Id" }],
            namespace: "Element.Checks"
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
        },
        IsInfoIdItem: {
            type: 'boolean',
            comparison: 'eq'
        },
        FieldCode: {
            type: 'string',
            comparison: 'like'
        },
        OType: {
            type: 'string',
            comparison: 'eq'
        },
        Nullable: {
            type: 'boolean',
            comparison: 'eq'
        },
        IsDetailsShow: {
            type: 'boolean',
            comparison: 'eq'
        },
        IsGridColumn: {
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
    var comboGroupCode = mini.get("comboGroupCode");

    var splitter = mini.get(self.prifix + "splitter");
    var tabs1 = mini.get(self.prifix + "tabs1");
    var grid = mini.get(self.prifix + "dgElement");
    var dgInfoGroup = mini.get(self.prifix + "dgInfoGroup");
    dgInfoGroup.on("load", function (e) {
        if (e.data.length != 0) {
            e.sender.addRow({ Name: '全部', SortCode: '-1', Id: '' }, 0);
            e.sender.addRow({ Name: '未分组', SortCode: '0', Id: '00000000-0000-0000-0000-000000000000' }, 1);
            splitter.show();
        }
        else {
            helper.onGridLoad(e);
            splitter.hide();
        }
    });
    dgInfoGroup.on("selectionchanged", function (e) {
        currentInfoGroupRecord = dgInfoGroup.getSelected();
        search();
    });
    var dgOntology = mini.get(self.prifix + "dgOntology");
    dgOntology.on("load", helper.onGridLoad);
    dgOntology.on("selectionchanged", function (e) {
        currentOntologyRecord = dgOntology.getSelected();
        dgInfoGroup.load({ ontologyId: currentOntologyRecord.Id });
        if (!dgInfoGroup.sortField) {
            dgInfoGroup.sortBy("SortCode", "asc");
        }
    });
    dgOntology.load();
    dgOntology.sortBy("SortCode", "asc");
    dgOntology.on("drawcell", onOntologyDrawcell);
    grid.on("drawcell", ondrawcell);
    var helperDrawcell = helper.ondrawcell(self, "Edi.Element.Index");
    grid.on("load", function (e) {
        var records = grid.getSelecteds();
        if (records.length == 1) {
            var record = records[0];
            if (record) {
                dgAction.load({ elementId: record.Id });
            }
        }
        else {
            dgAction.clearRows();
        }
        helper.onGridLoad(e);
    });
    var btnSaveAction = mini.get(self.prifix + "btnSaveAction");
    btnSaveAction.on("click", saveElementAction);
    var dgAction = mini.get(self.prifix + "dgAction");
    dgAction.on("drawcell", ondrawcell);
    grid.on("selectionchanged", function (e) {
        var records = grid.getSelecteds();
        if (records.length == 1) {
            var record = records[0];
            if (record) {
                dgAction.load({ elementId: record.Id });
            }
        }
        else {
            dgAction.clearRows();
        }
    });
    helper.index.allInOne(
        edit,
        grid,
        bootPATH + "../Edi/Element/Edit",
        bootPATH + "../Edi/Element/Edit",
        bootPATH + "../Edi/Element/Delete",
        self);
    helper.index.tabInOne(grid, tabs1, tabConfigs, self);

    function search() {
        var data = { };
        if (currentOntologyRecord && currentOntologyRecord.Id) {
            data.ontologyId = currentOntologyRecord.Id;
        }
        if (currentInfoGroupRecord) {
            data.groupId = currentInfoGroupRecord.Id;
        }
        var filterArray = [];
        for (var k in self.filters) {
            var filter = self.filters[k];
            if (filter.value) {
                filterArray.push({ field: k, type: filter.type, comparison: filter.comparison, value: filter.value });
            }
        }
        data.filters = JSON.stringify(filterArray);
        if (!grid.sortField) {
            grid.sortBy("SortCode", "asc");
        }
        grid.load(data);
    }

    function saveElementAction() {
        var data = dgAction.getChanges();
        var json = mini.encode(data);

        dgAction.loading("保存中，请稍后......");
        $.ajax({
            url: bootPATH + "../Edi/Element/AddOrUpdateElementActions",
            data: { data: json },
            type: "post",
            success: function (result) {
                dgAction.reload();
            },
            error: function (jqXHR, textStatus, errorThrown) {
                mini.alert(jqXHR.responseText);
            }
        });
    }

    function onOntologyDrawcell(e) {
        var field = e.field;
        var value = e.value;
        var record = e.record;
        if (field) {
            switch (field) {
                case "IsEnabled":
                case "IsConfigValid":
                    if (value == "正常" || value == "1" || value == "是" || value == "true") {
                        e.cellHtml = "<span class='icon-enabled width16px'></span>";
                    } else if (value == "禁用" || value == "0" || value == "否" || value == "false") {
                        e.cellHtml = "<span class='icon-disabled width16px'></span>";
                    } break;
            }
        }
    }

    function ondrawcell(e) {
        var field = e.field;
        var value = e.value;
        var record = e.record;
        if (field) {
            switch (field) {
                case "IsDetailsShow":
                case "ActionIsAllow":
                case "IsGridColumn":
                case "DbIsPrimaryKey":
                case "Nullable":
                case "DbIsNullable":
                case "IsConfigValid":
                case "IsInfoIdItem":
                    if (value == "正常" || value == "1" || value == "是" || value == "true" || value == true) {
                        e.cellHtml = "<span class='icon-enabled width16px'></span>";
                    } else if (value == "禁用" || value == "0" || value == "否" || value == "false" || value == false) {
                        e.cellHtml = "<span class='icon-disabled width16px'></span>";
                    } break;
                case "Icon":
                    if (value) {
                        e.cellHtml = "<img src='" + bootPATH + "../Content/icons/16x16/" + value + "'></img>";
                    }
                    break;
                case "InfoDicName":
                    if (value) {
                        var url = bootPATH + "../Edi/InfoDic/Details?isTooltip=true&id=" + record.InfoDicId
                        e.cellHtml = "<a href='" + url + "' onclick='helper.cellTooltip(this);return false;' rel='" + url + "'>" + value + "</a>";
                    }
                    break;
            }
        }
        helperDrawcell(e);
    }

    function SetData(data) {
        var currentOntologyId = currentOntologyRecord.Id;
        if (!ontologyId || ontologyId != currentOntologyId) {
            ontologyId = currentOntologyId;
            comboGroupCode.load(bootPATH + "../Edi/Ontology/GetInfoGroups?ontologyId=" + ontologyId);
        }
        //跨页面传递的数据对象，克隆后才可以安全使用
        data = mini.clone(data);
        if (data.action == "edit") {
            $.ajax({
                url: bootPATH + "../Edi/Element/Get?id=" + data.id,
                cache: false,
                success: function (result) {
                    helper.response(result, function () {
                        form.setData(result);
                        form.validate();
                        if (result.Icon) {
                            $("#msg").html("<img style='margin-top:3px;' src='" + bootPATH + "../Content/icons/16x16/" + result.Icon + "' alt='图标' />");
                        } else {
                            $("#msg").html("");
                        }
                    });
                }
            });
        }
        else if (data.action == "new") {
            data["InfoDicId"] = "";
            data["FilterType"] = "";
            data["OntologyId"] = currentOntologyRecord.Id;
            form.setData(data);
        }

        if (!faceInitialized) {
            $.getJSON(bootPATH + "../Home/GetIcons", null, function (result) {
                $("#message_face").jqfaceedit({ txtAreaObj: $("#msg"), containerObj: $(mini.get('faceWindow').getBodyEl()), emotions: result, top: 25, left: -27, width: 658, height: 420 });
            });
            faceInitialized = true;
        }
    }
    helper.edit.allInOne(
        self,
        win,
        bootPATH + "../Edi/Element/Create",
        bootPATH + "../Edi/Element/Update",
        bootPATH + "../Edi/Element/Get",
        form, edit);
})(window);