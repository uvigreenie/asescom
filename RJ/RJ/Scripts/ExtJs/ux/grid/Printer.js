﻿/**
* @class Ext.ux.grid.Printer
* @author Ed Spencer (edward@domine.co.uk)
* Helper class to easily print the contents of a grid. Will open a new window with a table where the first row
* contains the headings from your column model, and with a row for each item in your grid's store. When formatted
* with appropriate CSS it should look very similar to a default grid. If renderers are specified in your column
* model, they will be used in creating the table. Override headerTpl and bodyTpl to change how the markup is generated
*
* Usage:
*
* 1 - Add Ext.Require Before the Grid code
* Ext.require([
* 'Ext.ux.grid.GridPrinter',
* ]);
*
* 2 - Declare the Grid
* var grid = Ext.create('Ext.grid.Panel', {
* columns: //some column model,
* store : //some store
* });
*
* 3 - Print!
* Ext.ux.grid.Printer.mainTitle = 'Your Title here'; //optional
* Ext.ux.grid.Printer.print(grid);
*
* Original url: http://edspencer.net/2009/07/printing-grids-with-ext-js.html
*
* Modified by Loiane Groner (me@loiane.com) - September 2011 - Ported to Ext JS 4
* http://loianegroner.com (English)
* http://loiane.com (Portuguese)
*
* Modified by Bruno Sales - August 2012
*
* Modified by Paulo Goncalves - March 2012
*
* Modified by Beto Lima - March 2012
*
* Modified by Beto Lima - April 2012
*
* Modified by Paulo Goncalves - May 2012
*
* Modified by Nielsen Teixeira - 2012-05-02
*
* Modified by Joshua Bradley - 2012-06-01
*
* Modified by Loiane Groner - 2012-09-08
*
* Modified by Loiane Groner - 2012-09-24
*
* Modified by Loiane Groner - 2012-10-17
* FelipeBR contribution: Fixed: support for column name that cotains numbers
* Fixed: added support for template columns
*
* Modified by Loiane Groner - 2013-Feb-26
* Fixed: added support for row expander plugin
* Tested using Ext JS 4.1.2
*
*/
Ext.define("Ext.ux.grid.Printer", {

    requires: 'Ext.XTemplate',

    statics: {
        /**
        * Prints the passed grid. Reflects on the grid's column model to build a table, and fills it using the store
        * @param {Ext.grid.Panel} grid The grid to print
        */
        print: function (grid, featureId) {
            // We generate an XTemplate here by using 2 intermediary
            // XTemplates - one to create the header, the other
            // to create the body (see the escaped {} below)
            var columns = [];
            var isGrouped = grid.store.isGrouped();
            if (isGrouped) {
                var feature = this.getFeature(grid, featureId);
                var groupField = feature.getGroupField();
            }
            //account for grouped columns
            Ext.each(grid.columns, function (c) {
                if (c.items.length > 0) {
                    columns = columns.concat(c.items.items);
                } else {
                    columns.push(c);
                }
            });

            //build a usable array of store data for the XTemplate
            var data = [];
            grid.store.data.each(function (item, row) {
                var convertedData = {};

                //apply renderers from column model
                for (var key in item.data) {
                    var value = item.data[key];
                    var found = false;

                    Ext.each(columns, function (column, col) {

                        if (column && column.dataIndex == key) {

                            /*
                            * TODO: add the meta to template
                            */
                            var meta = { item: '', tdAttr: '', style: '' };
                            value = column.renderer ? column.renderer.call(grid, value, meta, item, row, col, grid.store, grid.view) : value;
                            var varName = Ext.String.createVarName(column.dataIndex);
                            convertedData[varName] = value;
                            found = true;

                        } else if (column && column.xtype === 'rownumberer') {

                            var varName = Ext.String.createVarName(column.id);
                            convertedData[varName] = (row + 1);
                            found = true;

                        } else if (column && column.xtype === 'templatecolumn') {

                            value = column.tpl ? column.tpl.apply(item.data) : value;

                            var varName = Ext.String.createVarName(column.id);
                            convertedData[varName] = value;
                            found = true;

                        }
                    }, this);

                    if (!found) { // model field not used on Grid Column, can be used on RowExpander
                        var varName = Ext.String.createVarName(key);
                        convertedData[varName] = value;
                    }
                }

                data.push(convertedData);
            });

            // remove columns that do not contain dataIndex
            // or dataIndex is empty.
            // for example: columns filter or columns button
            var clearColumns = [];
            Ext.each(
                columns,
                function (column) {
                    if (column) {
                        if (!Ext.isEmpty(column.dataIndex) &&
                             !column.hidden &&
                             !isGrouped) {
                            clearColumns.push(column);
                        } else if (column.xtype === 'rownumberer') {
                            column.text = 'Row';
                            clearColumns.push(column);
                        } else if (column.xtype === 'templatecolumn') {
                            clearColumns.push(column);
                        } else if (isGrouped && column.dataIndex !== groupField) {
                            clearColumns.push(column);
                        }
                    }
                }
            );
            columns = clearColumns;

            //get Styles file relative location, if not supplied
            if (this.stylesheetPath === null) {
                var scriptPath = Ext.Loader.getPath('Ext.ux.grid.Printer');
                this.stylesheetPath = scriptPath.substring(0, scriptPath.indexOf('Printer.js')) + 'gridPrinterCss/print.css';
            }

            //use the headerTpl and bodyTpl markups to create the main XTemplate below
            var headings = Ext.create('Ext.XTemplate', this.headerTpl).apply(columns);
            var body = this.generateBody(grid, columns, feature);
            var pluginsBody = '',
                pluginsBodyMarkup = [];

            //add relevant plugins
            Ext.each(grid.plugins, function (p) {
                if (p.ptype == 'rowexpander') {
                    pluginsBody += p.rowBodyTpl.html;
                }
            });

            if (pluginsBody != '') {
                pluginsBodyMarkup = [
                    '<tr class="{[xindex % 2 === 0 ? "even" : "odd"]}"><td colspan="' + columns.length + '">',
                      pluginsBody,
                    '</td></tr>'
                ];
            }

            var title = grid.title || this.defaultGridTitle;

            //Here because inline styles using CSS, the browser did not show the correct formatting of the data the first time that loaded
            var htmlMarkup = [
                '<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">',
                '<html class="' + Ext.baseCSSPrefix + 'ux-grid-printer">',
                  '<head>',
                    '<meta content="text/html; charset=UTF-8" http-equiv="Content-Type" />',
                    '<link href="' + this.stylesheetPath + '" rel="stylesheet" type="text/css" />',
                    '<title>' + title + '</title>',
                  '</head>',
                  '<body class="' + Ext.baseCSSPrefix + 'ux-grid-printer-body">',
                  '<div class="' + Ext.baseCSSPrefix + 'ux-grid-printer-noprint ' + Ext.baseCSSPrefix + 'ux-grid-printer-links">',
                      '<a class="' + Ext.baseCSSPrefix + 'ux-grid-printer-linkprint" href="javascript:void(0);" onclick="window.print();">' + this.printLinkText + '</a>',
                      '<a class="' + Ext.baseCSSPrefix + 'ux-grid-printer-linkclose" href="javascript:void(0);" onclick="window.close();">' + this.closeLinkText + '</a>',
                  '</div>',
                  '<h1>' + this.mainTitle + '</h1>',
                    '<table>',
                      '<tr>',
                        headings,
                      '</tr>',
                        '<tpl for=".">',
                           '<tr class="{[xindex % 2 === 0 ? "even" : "odd"]}">',
                              body,
                           '</tr>',
                           pluginsBodyMarkup.join(''),
                           '{% if (this.isGrouped && xindex > 0) break; %}',
                        '</tpl>',
                    '</table>',
                  '</body>',
                '</html>',
                {
                    isGrouped: isGrouped
                }
            ];

            var html = Ext.create('Ext.XTemplate', htmlMarkup).apply(data);

            //open up a new printing window, write to it, print it and close
            var win = window.open('', 'printgrid');

            //document must be open and closed
            win.document.open();
            win.document.write(html);
            win.document.close();

            if (this.printAutomatically) {
                win.print();
            }

            //Another way to set the closing of the main
            if (this.closeAutomaticallyAfterPrint) {
                if (Ext.isIE) {
                    window.close();
                } else {
                    win.close();
                }
            }
        },

        getFeature: function (grid, featureId) {
            var feature;
            var view = grid.getView();
            if (featureId) {
                feature = view.getFeature(featureId);
            }
            else {
                var features = view.features;
                if (features.length > 1) {
                    alert("More than one feature requires to pass " +
                           "featureId as parameter to 'print'.");
                    return;
                }
                else {
                    feature = features[0];
                }
            }
            return feature;
        },

        generateBody: function (grid, columns, feature) {

            var groups = grid.store.getGroups();
            var fields = grid.store.getProxy().getModel().getFields();
            var hideGroupField = true;
            var groupField;
            var body;

            if (grid.store.isGrouped()) {
                hideGroupField = feature.hideGroupedHeader; // bool
                groupField = feature.getGroupField();

                if (!feature || !fields || !groupField) {
                    return;
                }

                if (hideGroupField) {
                    var removeGroupField = function (item) {
                        return (item.name != groupField);
                    };
                    // Remove group field from fields array.
                    // This could be done later in the template,
                    // but it is easier to do it here.
                    fields = fields.filter(removeGroupField);
                }

                // Use group header template for the header.
                var html = feature.groupHeaderTpl.html || '';

                var bodyTpl = [
                    '<tpl for=".">',
                        '<tr class="group-header">',
                            '<td colspan="{[this.colSpan]}">',
                              html, // This is the group header!
                            '</td>',
                        '</tr>',
                        '<tpl for="children">',
                            '<tr>',
                                '<tpl for="this.fields">',
                                    '{% if (values.name==="id") continue; %}',
                                    '<td>',
                                      '{[ parent.get(values.name) ]}',
                                    '</td>',
                                '</tpl>',
                            '</tr>',
                        '</tpl>',
                    '</tpl>',
                    {
                        // XTemplate configuration:
                        fields: fields,
                        colSpan: fields.length - 1,
                        // XTemplate member functions:
                        childCount: function (c) {
                            return c.length;
                        }
                    }
                ];
                body = Ext.create('Ext.XTemplate', bodyTpl).apply(groups);
            }
            else {
                body = Ext.create('Ext.XTemplate', this.bodyTpl).apply(columns);
            }
            return body;
        },

        /**
        * @property stylesheetPath
        * @type String
        * The path at which the print stylesheet can be found (defaults to 'ux/grid/gridPrinterCss/print.css')
        */
        stylesheetPath: null,

        /**
        * @property printAutomatically
        * @type Boolean
        * True to open the print dialog automatically and close the window after printing. False to simply open the print version
        * of the grid (defaults to false)
        */
        printAutomatically: false,

        /**
        * @property closeAutomaticallyAfterPrint
        * @type Boolean
        * True to close the window automatically after printing.
        * (defaults to false)
        */
        closeAutomaticallyAfterPrint: false,

        /**
        * @property mainTitle
        * @type String
        * Title to be used on top of the table
        * (defaults to empty)
        */
        mainTitle: '',

        /**
        * @property defaultGridTitle
        * @type String
        * Title to be used if grid to be printed
        * has no title attribute set.
        */
        defaultGridTitle: 'Vista Impresion',

        /**
        * Text show on print link
        * @type String
        */
        printLinkText: 'Imprimir',

        /**
        * Text show on close link
        * @type String
        */
        closeLinkText: 'Cerrar',

        /**
        * @property headerTpl
        * @type {Object/Array} values
        * The markup used to create the headings row. By default this just uses <th> elements, override to provide your own
        */
        headerTpl: [
            '<tpl for=".">',
                '<th>{text}</th>',
            '</tpl>'
        ],

        /**
        * @property bodyTpl
        * @type {Object/Array} values
        * The XTemplate used to create each row. This is used inside the 'print' function to build another XTemplate, to which the data
        * are then applied (see the escaped dataIndex attribute here - this ends up as "{dataIndex}")
        */
        bodyTpl: [
            '<tpl for=".">',
                '<tpl if="values.dataIndex">',
                    '<td>\{{[Ext.String.createVarName(values.dataIndex)]}\}</td>',
                '<tpl else>',
                    '<td>\{{[Ext.String.createVarName(values.id)]}\}</td>',
                '</tpl>',
            '</tpl>'
        ]
    }
});