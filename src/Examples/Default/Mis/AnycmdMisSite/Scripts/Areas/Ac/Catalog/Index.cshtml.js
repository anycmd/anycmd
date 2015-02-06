/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    mini.namespace("Ac.Catalog.Index");
    var self = Ac.Catalog.Index;
    self.prifix = "Ac_Catalog_Index_";
    self.help = { appSystemCode: "Anycmd", areaCode: "Ac", resourceCode: "Catalog", functionCode: "Index" };
    helper.helperSplitterInOne(self);
    mini.namespace("Catalog.Edit");
    var edit = Catalog.Edit;
    edit.SaveData = SaveData;
    edit.prifix = "Ac_Catalog_Index_Edit_"

    window.addNewNode = addNewNode;
    window.updateCurrentNodeName = updateCurrentNodeName;
    window.updateChildNodeName = updateChildNodeName;
    window.removeChildNode = removeChildNode;

    var currentNode;
    var gridChildrenInitialized = false;
    var detailsInitialized = false;
    var managerInitialized = false;
    var childreniframe;
    var useriframe;
    var infoiframe;
    var manageriframe;

    mini.parse();

    var win = mini.get(edit.prifix + "win1");
    var form;
    if (win) {
        form = new mini.Form(edit.prifix + "form1");
    }

    var btnEdit = mini.get(self.prifix + "btnEdit");
    if (btnEdit) {
        btnEdit.on("click", editNode);
    }
    var btnAdd = mini.get(self.prifix + "btnAdd");
    if (btnAdd) {
        btnAdd.on("click", add);
    }
    var btnRemove = mini.get(self.prifix + "btnRemove");
    if (btnRemove) {
        btnRemove.on("click", remove);
    }
    var btnSearchCatalog = mini.get(self.prifix + "btnSearchCatalog");
    btnSearchCatalog.on("click", searchCatalog);

    var keyCatalog = mini.get(self.prifix + "keyCatalog");
    keyCatalog.on("enter", searchCatalog);
    var treeCatalog = mini.get(self.prifix + "treeCatalog");
    treeCatalog.on("nodeselect", onNodeSelect);
    treeCatalog.on("beforeload", onTreeBeforeload);
    treeCatalog.on("nodedblclick", function (e) {
        if (e.isLeaf) {
            editNode();
        }
    });
    currentNode = treeCatalog.getRootNode();
    var tabs1 = mini.get(self.prifix + "tabs1");
    tabs1.hide();
    tabs1.on("tabload", ontabload);
    tabs1.on("activechanged", onactivechanged);

    function searchCatalog() {
        var k = keyCatalog.getValue().trim();
        if (k == "") {
            treeCatalog.clearFilter();
            $("#" + self.prifix + "msg").hide()
        } else {
            k = k.toLowerCase();
            var anyIsTrue = false;
            treeCatalog.filter(function (node) {
                var name = node.Name ? node.Name.toLowerCase() : "";
                if (!node.expanded && !node.isLeaf && !node.IsCategory) {
                    return false;
                }
                if (name.indexOf(k) != -1) {
                    anyIsTrue = true;
                    return true;
                }
            });
            if (anyIsTrue) {
                $("#" + self.prifix + "msg").hide()
            }
            else {
                $("#" + self.prifix + "msg").show();
            }
        }
    }
    
    function onNodeSelect(e) {
        var node = e.node;
        currentNode = node;
        $("#" + self.prifix + "tabs1").show();
        loadTabData("refresh", tabs1.getTab(tabs1.activeIndex));
    }

    function onTreeBeforeload(e) {
        var node = e.node;
        var params = e.params;

        params.parentId = node.Id;
    }

    function haveSelectedNode() {
        if (!currentNode || !currentNode.Id) {
            mini.alert("请先选择一个节点");
            return false;
        }
        return true;
    }

    function add() {
        var id='';
        if (currentNode) {
            id = currentNode.Id;
        }
        var data = { action: "new", ParentId: id, CategoryId: "" };
        win.setTitle("添加");
        win.setIconCls("icon-add");
        win.show();
        edit.SetData(data);
    }

    function editNode() {
        if (haveSelectedNode()) {
            var data = { action: "edit", id: currentNode.Id };
            win.setTitle("编辑");
            win.setIconCls("icon-edit");
            win.show();
            edit.SetData(data);
        }
    }

    function remove() {
        if (!currentNode || !currentNode.Id) {
            mini.alert("请选择一个叶节点");
            return;
        }
        if (!currentNode.isLeaf) {
            mini.alert("不能删除父节点")
            return;
        }
        if (haveSelectedNode()) {
            mini.confirm("确定删除记录？", "确定？", function (action) {
                if (action == "ok") {
                    $.post(bootPATH + "../Ac/Catalog/Delete", { id: currentNode.Id }, function (result) {
                        helper.response(result, function () {
                            treeCatalog.removeNode(currentNode);
                            var nodes = treeCatalog.getChildNodes(currentNode.parent);
                            if (!nodes || nodes.length == 0) {
                                currentNode.parent.isLeaf = true;
                            }
                        });
                    }, "json");
                }
            });
        }
    }

    function addNewNode(newNode) {
        if (currentNode.isLeaf || currentNode.expanded || currentNode == treeCatalog.getRootNode()) {
            treeCatalog.addNode(newNode, 0, currentNode);
        }
    }
    function updateCurrentNodeName(newName) {
        if (currentNode) {
            treeCatalog.updateNode(currentNode, { Name: newName });
            if (tabs1.activeIndex == 0) {
                var infoTab = tabs1.getTab("infoTab");
                loadTabData("refresh", infoTab);
            }
        }
    }
    function updateChildNodeName(id, newName) {
        if (currentNode.expanded) {
            var nodes = treeCatalog.getChildNodes(currentNode);
            var node;
            if (nodes) {
                for (var i = 0; i < nodes.length; i++) {
                    if (nodes[i].Id == id) {
                        node = nodes[i];
                        break;
                    }
                }
            }
            if (node) {
                treeCatalog.updateNode(node, { Name: newName });
            }
        }
    }
    function removeChildNode(id) {
        if (!id) {
            return;
        }
        var nodes = treeCatalog.getChildNodes(currentNode);
        if (currentNode.expanded || nodes && nodes.length > 0) {
            var ids = id.split(',');
            if (ids.length > 0) {
                for (var i = 0; i < ids.length; i++) {
                    var nodeId = ids[i];
                    for (var j = 0; j < nodes.length; j++) {
                        if (nodes[j].Id.toLowerCase() == nodeId.toLowerCase()) {
                            treeCatalog.removeNode(nodes[j]);
                        }
                    }
                }
                if (treeCatalog.getChildNodes(currentNode).length == 0) {
                    treeCatalog.updateNode(currentNode, { isLeaf: true });
                }
            }
        }
    }
    /***********tabs***********/
    function onactivechanged(e) {
        loadTabData(null, e.tab);
    }

    function loadTabData(refresh, tab) {
        var tabName = tab.name;
        switch (tabName) {
            // 详细信息          
            case "infoTab":
                if (refresh || !detailsInitialized) {
                    if (currentNode.Id) {
                        var data = { id: currentNode.Id };
                        if (infoiframe) {
                            infoiframe.contentWindow.Catalog.Details.params = data;
                            infoiframe.contentWindow.Catalog.Details.loadData();
                        }
                        else {
                            var href = bootPATH + "../Ac/Catalog/Details";
                            tabs1.loadTab(href, tab, function () {
                                var contentWindow = tabs1.getTabIFrameEl(tab).contentWindow;
                                contentWindow.Catalog.Details.params = data;
                                contentWindow.Catalog.Details.loadData();
                            });
                        }
                    }

                    detailsInitialized = true;
                } break;
            case "managerTab":
                if (refresh || !managerInitialized) {
                    var data = {};
                    if (currentNode.Id) {
                        data.catalogCode = currentNode.Code;
                        data.catalogId = currentNode.Id;
                    }
                    
                    var href = $.param.fragment(bootPATH + "../Ac/Catalog/Accounts", data);
                    if (manageriframe) {
                        manageriframe.contentWindow.location.href = href;
                        manageriframe.contentWindow.Catalog.Accounts.loadData();
                    }
                    else {
                        tabs1.loadTab(href, tab);
                    }

                    managerInitialized = true;
                } break;
                // 子组织机构          
            case "childrenTab":
                if (refresh || !gridChildrenInitialized) {
                    var data = {};
                    if (currentNode.Id) {
                        data.parentId = currentNode.Id;
                        data.parentCode = currentNode.Code;
                    }
                    
                    var href = $.param.fragment(bootPATH + "../Ac/Catalog/Children", data);
                    if (childreniframe) {
                        childreniframe.contentWindow.location.href = href;
                        childreniframe.contentWindow.Catalog.Children.loadData();
                    }
                    else {
                        tabs1.loadTab(href, tab);
                    }

                    gridChildrenInitialized = true;
                } break;
            default:
                alert("意外的tab名:" + tabName);
                break;
        }

        if (refresh) {
            if (tab.name != "infoTab") {
                detailsInitialized = false;
            }
            if (tab.name != "managerTab") {
                managerInitialized = false;
            }
            if (tab.name != "childrenTab") {
                gridChildrenInitialized = false;
            }
        }
    }

    function ontabload(e) {
        var tabs = e.sender;
        var tab = e.tab;
        var tabName = tab.name;
        var iframe = tabs.getTabIFrameEl(e.tab);
        switch (tabName) {
            case "infoTab":
                infoiframe = iframe;
                break;
            case "managerTab":
                manageriframe = iframe;
                break;
            case "childrenTab":
                childreniframe = iframe;
                break;
            default:
                alert("意外的tab名:" + tabName);
                break;
        }
    }

    function SaveData() {
        var data = $("#" + edit.prifix + "form1").serialize();
        var id = $("#" + edit.prifix + "form1 input[name='Id']").val();
        var url = bootPATH + "../Ac/Catalog/Create";
        if (id) {
            url = bootPATH + "../Ac/Catalog/Update";
        }
        form.validate();
        if (form.isValid() == false) return;

        $.post(url, data, function (result) {
            helper.response(result, function () {
                var name = $("#" + edit.prifix + "form1 input[name='Name']").val();
                var categoryId = $("#" + edit.prifix + "form1 input[name='CategoryId']").val();
                var code = $("#" + edit.prifix + "form1 input[name='Code']").val();
                var parentCode = $("#" + edit.prifix + "form1 input[name='ParentCode']").val();
                var newNode = { Id: result.id, Name: name, isLeaf: true, ParentCode: parentCode, Code: code, CategoryId: categoryId };

                if (id) {
                    if (window.updateCurrentNodeName) {
                        window.updateCurrentNodeName(name);
                    }
                    if (window.parent.updateChildNodeName) {
                        window.parent.updateChildNodeName(id, name);
                    }
                }
                else {
                    if (window.addNewNode) {
                        window.addNewNode(newNode);
                    }
                    if (window.parent.addNewNode) {
                        window.parent.addNewNode(newNode);
                    }
                }
                edit.CloseWindow("save");
            });
        }, "json");
    };

    helper.edit.allInOne(
        self,
        win,
        bootPATH + "../Ac/Catalog/Create",
        bootPATH + "../Ac/Catalog/Update",
        bootPATH + "../Ac/Catalog/Get",
        form, edit);
})(window);