/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window, $) {
    mini.namespace("Ac.RDatabase.ViewDefinition");
    var self = Ac.RDatabase.ViewDefinition;
    self.prifix = "Ac_RDatabase_ViewDefinition_";
    self.loadData = loadData;

    function loadData() {
        var data = getParams();
        $("#" + self.prifix + "content").load(bootPATH + "../Ac/RDatabase/GetViewDefinition?databaseId=" + data.databaseId + "&viewId=" + data.viewId);
    }

    function getParams() {
        var data = {};
        if (self.params && self.params.databaseId) {
            data.databaseId = self.params.databaseId;
            data.viewId = self.params.viewId;
        }
        else {
            data.databaseId = $.deparam.fragment().databaseId || $.deparam.querystring().databaseId;
            data.viewId = $.deparam.fragment().viewId || $.deparam.querystring().viewId;
        }
        return data;
    }
})(window, jQuery);