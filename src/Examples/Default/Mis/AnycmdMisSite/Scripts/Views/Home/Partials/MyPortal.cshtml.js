/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../helper.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
/// <reference path="../../../miniui/ux/Portal.js" />
var portal = new mini.ux.Portal();
portal.set({
    style: "width: 800px; height: 100%; margin: 20px auto;",
    columns: [220, 550]
});
portal.render("myPortal");

//panel
portal.setPanels([
    { column: 0, id: "myRoles", title: "我的角色", showCloseButton: false, height: 200, body: "#myRoles" },
    { column: 0, id: "myGroups", title: "我的工作组", showCloseButton: false, height: 200, body: "#myGroups" }
]);
