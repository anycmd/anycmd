mini.ux.Portal = function () {
    this.columns = [];
    this.panels = [];
    mini.ux.Portal.superclass.constructor.call(this);
}
mini.extend(mini.ux.Portal, mini.Control, {
    columns: [],
    panels: [],

    allowDrag: true,

    width: 500,
    height: 300,
    uiCls: "mini-portal",
    _initEvents: function () {
        mini.on(this.el, "mousedown", this.__OnMouseDown, this);
    },
    destroy: function (removeEl) {

        if (this.panels) {
            var cs = this.panels.clone();
            for (var i = 0, l = cs.length; i < l; i++) {
                var p = cs[i];
                p.destroy(removeEl);
            }
            this.panels.length = 0;
            this.panels = null;
            delete this.panels;
        }
        mini.ux.Portal.superclass.destroy.call(this, removeEl);
    },
    doLayout: function () {
        if (!this.canLayout()) return;

        mini.layout(this.el.firstChild);
    },

    setColumns: function (columns) {
        //[100, "50%", 200, "100%"]
        if (!mini.isArray(columns)) columns = [];

        this.columns = columns;

        var sb = '<table class="mini-portal-table"><tr>';
        for (var i = 0, l = columns.length; i < l; i++) {
            var c = columns[i];
            if (mini.isNumber(c)) c += "px";
            sb += '<td id="' + i + '" class="mini-portal-column" style="width:' + c + '"></td>';
        }
        sb += '</tr></table>';

        this.el.innerHTML = sb;
    },
    getColumnEl: function (index) {
        return this.el.firstChild.rows[0].cells[index];
    },
    getColumnsBox: function () {
        var columns = [];
        for (var i = 0, l = this.columns.length; i < l; i++) {
            var el = this.getColumnEl(i);
            var box = mini.getBox(el);
            columns.push(box);

            box.height = 3000;
            box.bottom = box.top + box.height;
        }
        return columns;
    },

    /*
    column, id
    title, iconCls, 
    showCloseButton, showMaxButton, showMinButton
    width, height
    allowDrag
    */
    createDefaultPanel: function () {
        return {
            column: 0,
            type: "panel",
            allowDrag: true,
            showCloseButton: true,
            showCollapseButton: true,
            width: "100%",
            height: "100px"
        };
    },
    getPanelBodyEl: function (panel) {
        panel = this.getPanel(panel);
        if (!panel) return;
        return panel.getBodyEl();
    },
    getPanels: function () {
        return this.panels;
    },
    getPanel: function (id) {
        return typeof id == "string" ? mini.get(id) : id;
    },
    setPanels: function (panels) {
        for (var i = 0, l = panels.length; i < l; i++) {
            this.addPanel(panels[i]);
        }
    },
    removePanel: function (panel) {
        panel = this.getPanel(panel);
        if (!panel) return;
        this.panels.remove(panel);

        var el = panel.el;
        el.parentNode.removeChild(el);
    },
    addPanel: function (panel) {
        if (!panel) return;
        if (mini.isNumber(panel.column) == false) panel.column = 0;

        if (mini.isControl(panel) == false) {
            panel = mini.copyTo(this.createDefaultPanel(), panel);
        }

        panel = mini.getAndCreate(panel);

        panel.setWidth("100%");
        panel.addCls("mini-portal-panel");

        var column = this.getColumnEl(panel.column);
        panel.render(column);

        this.panels.push(panel);

        this.doLayout();
    },
    ///////////////////////////////////////////
    getColumnIndexByXY: function (x, y) {
        var elbox = this.getBox();
        elbox.height = 3000;
        elbox.bottom = elbox.top + elbox.height;
        var columnsBox = this.getColumnsBox();
        var index = -1;
        for (var i = 0, l = columnsBox.length; i < l; i++) {
            var box = columnsBox[i];
            if (elbox.x <= x && x <= elbox.right
                && elbox.y <= y && y <= elbox.bottom
                ) {
                if (box.x <= x && x <= box.right) {
                    return i;
                }
            }
        }
        return index;
    },
    _getPanelByY: function (y, column, noPanel) {
        for (var i = 0, l = this.panels.length; i < l; i++) {
            var panel = this.panels[i];
            if (panel.column != column || panel == noPanel) continue;
            var box = panel.getBox();
            box.height += 10;
            box.bottom += 10;

            if (box.y <= y && y <= box.bottom) {
                panel.__moveAction = "after";
                if (y < box.y + box.height / 2) panel.__moveAction = "before";
                return panel;
            }
        }
        return null;
    },
    __OnMouseDown: function (e) {

        var t = mini.findParent(e.target, 'mini-portal-panel');
        if (t) {


            var panel = mini.get(t.id);
            var sf = this;

            if (this.allowDrag && panel.allowDrag && mini.isAncestor(panel.getHeaderEl(), e.target) && !mini.findParent(e.target, "mini-tools")) {
                var box = panel.getBox();
                var drag = new mini.Drag({
                    capture: false,
                    onStart: function () {
                        mini.setOpacity(panel.el, .7);

                        panel.setWidth(box.width);
                        panel.el.style.position = "absolute";

                        jQuery(panel.el).before('<div class="mini-portal-proxy"></div>')[0];
                        sf._dragProxy = panel.el.previousSibling;
                        //mini.setHeight(sf._dragProxy, box.height);
                        sf._dragProxy.style.height = box.height + "px";

                        panel.el.style.zIndex = mini.getMaxZIndex();


                    },
                    onMove: function (drag) {
                        //document.title = "move" + new Date().getTime();
                        var x = drag.now[0] - drag.init[0], y = drag.now[1] - drag.init[1];

                        x = box.x + x;
                        y = box.y + y;

                        mini.setXY(panel.el, x, y);



                        sf._targetColumn = sf._targetPanel = null;

                        var dragBox = mini.getBox(sf._dragProxy);
                        dragBox.height += 10;
                        dragBox.bottom += 10;
                        if (dragBox.x <= x && x <= dragBox.right
                        && dragBox.y <= y && y <= dragBox.bottom
                        ) {
                            return;
                        }

                        //column
                        var column = sf.getColumnIndexByXY(x, y);
                        if (column != -1) {
                            //var y2 = y + box.height / 2;
                            var tp = sf._getPanelByY(y, column, panel);
                            //                            if (tp) document.title = tp.title + ":" + tp.__moveAction + ":" + column + ":" + new Date().getTime();
                            //                            else {
                            //                                tp = null;
                            //                            }
                            sf._targetColumn = column;
                            sf._targetPanel = tp;
                        }
                        //document.title = column;


                        if (mini.isNumber(sf._targetColumn)) {
                            if (sf._targetPanel) {
                                var el = sf._targetPanel.el;
                                if (sf._targetPanel.__moveAction == "before") {
                                    jQuery(el).before(sf._dragProxy);
                                } else {
                                    jQuery(el).after(sf._dragProxy);
                                }
                            } else {
                                var el = sf.getColumnEl(sf._targetColumn);
                                mini.append(el, sf._dragProxy);
                            }
                        }
                    },
                    onStop: function () {
                        //从_dragProxy，找column和index
                        var td = sf._dragProxy.parentNode;
                        var column = parseInt(td.id);

                        jQuery(sf._dragProxy).before(panel.el);

                        sf.panels.remove(panel);

                        var next = sf._dragProxy.nextSibling;
                        if (!next) {
                            sf.panels.push(panel);
                        } else {
                            var targetPanel = mini.get(next);

                            var index = sf.panels.indexOf(targetPanel);
                            sf.panels.insert(index, panel);
                        }

                        jQuery(sf._dragProxy).remove();
                        sf._maskProxy = null;

                        panel.el.style.position = "static";
                        panel.setWidth("100%");
                        mini.setOpacity(panel.el, 1);


                        sf._targetColumn = sf._targetPanel = null;
                    }
                });
                drag.start(e);
            }
        }
    }


});
mini.regClass(mini.ux.Portal, "portal");