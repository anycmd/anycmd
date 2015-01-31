/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window, $) {
    mini.namespace("Ac.Account.Index");
    var self = Ac.Account.Index;
    self.prifix = "Ac_Account_Index_";
    self.help = { appSystemCode: "Anycmd", areaCode: "Ac", resourceCode: "Account", functionCode: "Index" };
    // 当前选中的部门节点
    var currentNode;
    // tab页初始化标记
    var dgAccountUserInitialized = false;
    var dgUserAccountInitialized = false;
    var orgInfoInitialized = false;
    // tab页中的iframe
    var accountUseriframe;
    var userAccountiframe;
    var orgInfoiframe;

    mini.parse();

    helper.helperSplitterInOne(self);

    var btnSearchCatalog = mini.get(self.prifix + "btnSearchCatalog");
    btnSearchCatalog.on("click", searchCatalog);
    var keyCatalog = mini.get(self.prifix + "keyCatalog");
    keyCatalog.on("enter", searchCatalog);
    var treeCatalog = mini.get(self.prifix + "treeCatalog");
    treeCatalog.on("nodeselect", onCatalogNodeSelect);
    treeCatalog.on("beforeload", onCatalogTreeBeforeload);
    var tabs1 = mini.get(self.prifix + "tabs1");
    tabs1.on("activechanged", onactivechanged);
    tabs1.on("tabload", ontabload);

    currentNode = treeCatalog.getSelectedNode();
    if (!currentNode) {
        currentNode = treeCatalog.getRootNode();
        if (!currentNode.Id) {
            currentNode = treeCatalog.getChildNodes(currentNode)[0];
        }
        if (currentNode.Id) {
            treeCatalog.selectNode(currentNode);
        }
    }

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

    function onCatalogNodeSelect(e) {
        var tree = e.sender;
        var node = e.node;
        var isLeaf = e.isLeaf;
        currentNode = node;
        tabs1.show();
        var tab = tabs1.getTab(tabs1.activeIndex);
        loadTabData("refresh", tab);
    }

    function onCatalogTreeBeforeload(e) {
        var tree = e.sender;
        var node = e.node;
        var params = e.params;

        params.parentId = node.Id;
    }

    function onactivechanged(e) {
        loadTabData(null, e.tab);
    }

    // 加载tab页数据，如果当前tab页是第一次激活即相应的初始化标记为false则加载tab页否则仅刷新数据
    function loadTabData(refresh, tab) {
        var tabName = tab.name;
        var tabBody = $(tabs1.getTabBodyEl(tab));
        switch (tabName) {
            case "userTab":
                if (refresh || !dgAccountUserInitialized) {
                    var data = {};
                    if (currentNode && currentNode.Id) {
                        data.catalogId = currentNode.Id;
                        data.catalogCode = currentNode.Code;
                        data.isLeaf = currentNode.isLeaf;
                    }
                    else {
                        return;
                    }
                    
                    var href = $.param.fragment(bootPATH + "../Ac/Account/Contractors", data);
                    if (accountUseriframe) {
                        accountUseriframe.contentWindow.location.href = href;
                        accountUseriframe.contentWindow.Ac.Account.Contractors.loadData();
                    }
                    else {
                        var $iframe = $('<iframe frameborder="0" style="width:100%;height:100%;" src="' + href + '"></iframe>');
                        $iframe.appendTo(tabBody);
                        accountUseriframe = $iframe.get(0);
                    }
                    dgAccountUserInitialized = true;
                } break;
            case "accountTab":
                if (refresh || !dgUserAccountInitialized) {
                    var data = {};
                    if (currentNode && currentNode.Id) {
                        data.catalogId = currentNode.Id;
                        data.catalogCode = currentNode.Code;
                        data.isLeaf = currentNode.isLeaf;
                    }
                    else {
                        return;
                    }
                    
                    var href = $.param.fragment(bootPATH + "../Ac/Account/Index", data);
                    if (userAccountiframe) {
                        userAccountiframe.contentWindow.location.href = href;
                        userAccountiframe.contentWindow.Ac.Account.Index.loadData();
                    }
                    else {
                        var $iframe = $('<iframe frameborder="0" style="width:100%;height:100%;" src="' + href + '"></iframe>');
                        $iframe.appendTo(tabBody);
                        userAccountiframe = $iframe.get(0);
                    }
                    dgUserAccountInitialized = true;
                } break;
            default:
                alert("意外的tab名:" + tabName);
                break;
        }

        if (refresh) {
            if (tab.name != "userTab") {
                dgAccountUserInitialized = false;
            }
            if (tab.name != "accountTab") {
                dgUserAccountInitialized = false;
            }
        }
    }

    function ontabload(e) {
        var tabs = e.sender;
        var tab = e.tab;
        var iframe = tabs.getTabIFrameEl(e.tab);
        if (tab.name == "userTab") {
            accountUseriframe = iframe;
        }
        if (tab.name == "accountTab") {
            userAccountiframe = iframe;
        }
    }
})(window, jQuery);