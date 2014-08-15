var carga = 0;

var fnTreeStoreLoad = function () {
    if (carga >= 5) {
        Ext.MessageBox.hide();
        if (!treeStore1.getRootNode().hasChildNodes()) {
            treePanel1.setVisible(false);
        }
        if (!treeStore2.getRootNode().hasChildNodes()) {
            treePanel2.setVisible(false);
        }
        if (!treeStore3.getRootNode().hasChildNodes()) {
            treePanel3.setVisible(false);
        }
    }
    else {
        carga = carga + 1;
    }
}

var treeStore1 = Ext.create('Ext.data.TreeStore', {
    proxy: {
        type: 'ajax',
        url: '../../Seguridad/Autenticacion/ObtenerTreeMenu',
        reader: {
            type: 'json'
        },
        extraParams: {
            grupo: 0,
            usuario: 0
        }
    },
    listeners: {
        load: {
            fn: fnTreeStoreLoad
        }
    }
});

var treeStore2 = Ext.create('Ext.data.TreeStore', {
    proxy: {
        type: 'ajax',
        url: '../../Seguridad/Autenticacion/ObtenerTreeMenu',
        reader: {
            type: 'json'
        },
        extraParams: {
            grupo: 0,
            usuario: 0
        }
    },
    listeners: {
        load: {
            fn: fnTreeStoreLoad
        }
    }
});

var treeStore3 = Ext.create('Ext.data.TreeStore', {
    proxy: {
        type: 'ajax',
        url: '../../Seguridad/Autenticacion/ObtenerTreeMenu',
        reader: {
            type: 'json'
        },
        extraParams: {
            grupo: 0,
            usuario: 0
        }
    },
    listeners: {
        load: {
            fn: fnTreeStoreLoad
        }
    }
});

var fnItemClickTree = function (dataview, record, item, index, e, options) {
    try {
        if (record.get('leaf')) {
            Ext.MessageBox.show({
                title: 'RJ Abogados',
                msg: 'Cargando formulario, por favor espere...',
                width: 300,
                wait: true,
                waitConfig: { interval: 100 }
            });
            var panel = Ext.create(record.getId());
            if (contentPanel.getComponent(panel.getItemId()) == null || record.get('multiple') ) {
                panel.setTitle(record.get('text').toString());
                contentPanel.add(panel);
                contentPanel.setActiveTab(panel.getItemId());
            }
            else {
                contentPanel.setActiveTab(panel.getItemId());
                Ext.MessageBox.hide();
            }
        }
    }
    catch (ex) {
        Ext.Msg.show({
            title: 'RJ Abogados',
            msg: 'No se encontro el formulario ' + record.get('text') + '. ' + ex.toString(),
            buttons: Ext.Msg.OK,
            icon: Ext.Msg.ERROR
        });
    }
}

var contentPanel = Ext.create('Ext.tab.Panel', {
    xtype: 'plain-tabs',
    itemId: 'content-panel',
    region: 'center',
    margins: '2 5 5 0',
    border: false
});

var treePanel1 = Ext.create('Ext.tree.Panel', {
    title: 'Parametros y Configuración',
    rootVisible: false,
    autoScroll: true,
    border: false,
    useArrows: true,
    store: treeStore1,
    listeners: {
        itemclick: {
            fn: fnItemClickTree
        }
    }
});

var treePanel2 = Ext.create('Ext.tree.Panel', {
    title: 'Operaciones y Procesos',
    rootVisible: false,
    autoScroll: true,
    border: false,
    useArrows: true,
    store: treeStore2,
    listeners: {
        itemclick: {
            fn: fnItemClickTree
        }
    }

});

var treePanel3 = Ext.create('Ext.tree.Panel', {
    title: 'Consultas y Reportes',
    rootVisible: false,
    autoScroll: true,
    border: false,
    useArrows: true,
    store: treeStore3,
    listeners: {
        itemclick: {
            fn: fnItemClickTree
        }
    }
});

var toolbar = Ext.create('Ext.toolbar.Toolbar', {
    id: 'app-header',
    html: '<h1 style="Margin:0"> RJ Abogados</h1>',
    region: 'north',
    height: 40,
    defaults: {
        menu: [{
            text: 'Cambiar Contrase&ntilde;a',
            iconCls: 'icon-change',
            handler: function () {
                var frmChange = Ext.create('SegApp.Usuario.FrmChangePassword');
                frmChange.show();
                frmChange.getComponent('txtPasswordOld').focus();
            }
        }, {
            text: 'Cerrar Sesión',
            iconCls: 'icon-logout',
            handler: function () { window.location = '../../Seguridad/Autenticacion/Login'; }
        }]
    },
    items: [
        { xtype: 'tbfill' },
        { xtype: 'splitbutton', itemId: 'sbtnSession', text: 'No Conectado', iconCls: 'icon-user'}]
});

var updateSession = function () {
    Ext.Ajax.request({
        url: "../../Seguridad/Autenticacion/ObtenerDatosSession",
        timeout: 300000,
        success: function (response) {
            var data = Ext.decode(response.responseText);

            if (data.length == 0) {
                runner.stopAll();
                Ext.Msg.show({
                    title: 'RJ Abogados',
                    msg: 'Su Sesión ha expirado.',
                    buttons: Ext.Msg.OK,
                    icon: Ext.Msg.WARNING,
                    fn: function () { window.location = '../../Seguridad/Autenticacion/Login'; }
                });
            }
        },
        failure: function (response) {
            runner.stopAll();
            Ext.Msg.show({
                title: 'RJ Abogados',
                msg: 'Su Sesión ha expirado.',
                buttons: Ext.Msg.OK,
                icon: Ext.Msg.WARNING,
                fn: function () { window.location = '../../Seguridad/Autenticacion/Login'; }
            });
        }
    });
}

var task = {
    run: updateSession,
    interval: 60000
}

var runner = new Ext.util.TaskRunner();
runner.start(task);

Ext.define('SegApp.Layout.Contenedor', {
    extend: 'Ext.container.Viewport',
    itemId: 'contenedor',
    layout: 'border',
    id: 'app-header',
    items: [
        {
            layout: 'accordion',
            itemId: 'content-treepanel',
            collapsible: true,
            title: 'Explorador',
            iconCls: 'icon-treeMain',
            region: 'west',
            border: true,
            split: true,
            margins: '2 0 5 5',
            width: 250,
            items: [treePanel1, treePanel2, treePanel3]
        },
        contentPanel,
        toolbar
    ],
    listeners: {
        afterrender: {
            fn: function (component, options) {
                Ext.Ajax.request({
                    url: "../../Seguridad/Autenticacion/ObtenerDatosSession",
                    timeout: 400000,
                    success: function (response) {
                        var data = Ext.decode(response.responseText);
                        if (data.length == 0) {
                            Ext.Msg.show({
                                title: 'RJ Abogados',
                                msg: 'Su Sesión ha expirado.',
                                buttons: Ext.Msg.OK,
                                icon: Ext.Msg.WARNING,
                                fn: function () { window.location = '../../Seguridad/Autenticacion/Login'; }
                            });
                        }
                        else {
                            toolbar.getComponent('sbtnSession').setText(data[0]['Login'].toString());
                            treeStore1.load({
                                params: {
                                    grupo: 1,
                                    usuario: parseInt(data[0]['Usuario'].toString())
                                }
                            });
                            treeStore2.load({
                                params: {
                                    grupo: 2,
                                    usuario: parseInt(data[0]['Usuario'].toString())
                                }
                            });
                            treeStore3.load({
                                params: {
                                    grupo: 3,
                                    usuario: parseInt(data[0]['Usuario'].toString())
                                }
                            });
                        }
                    },
                    failure: function (response) {
                        Ext.Msg.show({
                            title: 'RJ Abogados',
                            msg: "El error ocurrio durante la carga:\n" + response.responseText,
                            buttons: Ext.Msg.OK,
                            icon: Ext.Msg.ERROR
                        });
                    }
                });
            }
        }
    },
    renderTo: Ext.getBody()
});