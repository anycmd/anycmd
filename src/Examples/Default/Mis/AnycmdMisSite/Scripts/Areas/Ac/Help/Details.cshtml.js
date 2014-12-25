/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../jquery-tmpl/jquery.tmpl.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
/// <reference path="../../../miniui/miniui.js" />
(function (window) {
    mini.namespace("Help.Details");
    var self = Help.Details;

    var divId = "Help_Details";
    var templateId = "Help_DetailsTemplate";
    var $templateId = "#" + templateId;
    var $divId = "#" + divId;

    self.loadData = loadData;

    $().ready(function () {
        if (location.pathname.toLowerCase() == "/Ac/help/details") {
            var params = getParams();
            if (params && (params.id || params.code)) {
                self.loadData();
            }
        }
    });

    function loadData() {
        helper.requesting();
        $.getJSON(
            bootPATH + "../Ac/Help/GetInfo",
            getParams(),
            function (result) {
                helper.response(result, function () {
                    $($divId).empty();
                    $($templateId).tmpl(result).appendTo($divId);
                    $("#content").html(result.Content);
                });
                helper.responsed();
            });
    }

    function getParams() {
        if (self.params && self.params.id) {
            return self.params;
        }
        else {
            var fragment = $.deparam.fragment();
            var querystring = $.deparam.querystring();
            return { id: fragment.id || querystring.id };
        }
    }
})(window);