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

    var btnSearchOrganization = mini.get(self.prifix + "btnSearchOrganization");
    btnSearchOrganization.on("click", searchOrganization);
    var keyOrganization = mini.get(self.prifix + "keyOrganization");
    keyOrganization.on("enter", searchOrganization);
    var treeOrganization = mini.get(self.prifix + "treeOrganization");
    treeOrganization.on("nodeselect", onOrganizationNodeSelect);
    treeOrganization.on("beforeload", onOrganizationTreeBeforeload);
    var tabs1 = mini.get(self.prifix + "tabs1");
    tabs1.on("activechanged", onactivechanged);
    tabs1.on("tabload", ontabload);

    currentNode = treeOrganization.getSelectedNode();
    if (!currentNode) {
        currentNode = treeOrganization.getRootNode();
        if (!currentNode.Id) {
            currentNode = treeOrganization.getChildNodes(currentNode)[0];
        }
        if (currentNode.Id) {
            treeOrganization.selectNode(currentNode);
        }
    }

    function searchOrganization() {
        var k = keyOrganization.getValue().trim();
        if (k == "") {
            treeOrganization.clearFilter();
            $("#" + self.prifix + "msg").hide()
        } else {
            k = k.toLowerCase();
            var anyIsTrue = false;
            treeOrganization.filter(function (node) {
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

    function onOrganizationNodeSelect(e) {
        var tree = e.sender;
        var node = e.node;
        var isLeaf = e.isLeaf;
        currentNode = node;
        tabs1.show();
        var tab = tabs1.getTab(tabs1.activeIndex);
        loadTabData("refresh", tab);
    }

    function onOrganizationTreeBeforeload(e) {
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
                        data.organizationId = currentNode.Id;
                        data.organizationCode = currentNode.Code;
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
                        data.organizationId = currentNode.Id;
                        data.organizationCode = currentNode.Code;
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