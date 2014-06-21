Ext.define('SegApp.Trabajador.FrmUsuarioTrabajador', {
    extend: 'Ext.panel.Panel',
    itemId: 'FrmUsuarioTrabajador',
    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    initComponent: function () {

        var stUsuario = Ext.create('Ext.data.Store', {
            fields: [
                { name: 'Usuario', type: 'int' },
                { name: 'Login', type: 'string' },
                { name: 'Nombres', type: 'string' },
                { name: 'Activo', type: 'bool' }
            ],
            proxy: {
                type: 'ajax',
                url: '../../Seguridad/Autenticacion/ListarUsuarios',
                reader: {
                    type: 'json'
                }
            },
            sorters: [{
                property: 'Login',
                direction: 'ASC'
            }],
            filters: [{
                property: 'Activo',
                value: "True"
            }],
            autoLoad: false
        });

        var stUsuarioTrabajador = Ext.create('Ext.data.Store', {
            fields: [
                { name: 'Trabajador', type: 'int' },
                { name: 'DTrabajador', type: 'string' }
        ],
            proxy: {
                type: 'ajax',
                url: '../../Seguridad/Trabajador/ListarTrabajadoresxUsuario',
                reader: {
                    type: 'json'
                },
                params: {
                    usuario: 0
                }
            },
            sorters: [{
                property: 'DTrabajador',
                direction: 'ASC'
            }],
            autoLoad: false
        });

        var stTrabajador = Ext.create('Ext.data.Store', {
            fields: [
                { name: 'Trabajador', type: 'int' },
                { name: 'DTrabajador', type: 'string' }
            ],
            proxy: {
                type: 'ajax',
                url: '../../Seguridad/Trabajador/ListarTrabajadoresxAsignar',
                reader: {
                    type: 'json'
                },
                params: {
                    usuario: 0
                }
            },
            sorters: [{
                property: 'DTrabajador',
                direction: 'ASC'
            }],
            autoLoad: false
        });


        var me = this;
        Ext.applyIf(me, {
            items: [
            {
                xtype: 'gridpanel',
                itemId: 'grdUsuario',
                region: 'center',
                title: 'Lista de Usuarios',
                store: stUsuario,
                columnLines: true,
                emptyText: 'No se encontraron datos.',
                flex: 1,
                columns: [{
                    xtype: 'rownumberer'
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'Usuario',
                    text: 'Usuario',
                    hidden: true,
                    hideable: false
                },
                {
                    dataIndex: 'Login',
                    text: 'Login',
                    width: 110,
                    hideable: false
                },
                {
                    dataIndex: 'Nombres',
                    text: 'Nombres',
                    width: 160,
                    hideable: false
                },
                {
                    flex: 1,
                    menuDisabled: true,
                    hideable: false
                }],
                listeners: {
                    selectionchange: {
                        fn: me.onGridUsuarioSelectionChange,
                        scope: me
                    }
                }
            },
            {
                border: false,
                layout: {
                    type: 'hbox',
                    align: 'stretch'
                },
                flex: 2,
                items: [
                {
                    xtype: 'gridpanel',
                    itemId: 'grdUsuarioTrabajador',
                    title: 'Trabajadores Asignados',
                    store: stUsuarioTrabajador,
                    multiSelect: true,
                    columnLines: true,
                    emptyText: 'No se encontraron datos.',
                    flex: 1,
                    viewConfig: {
                        plugins: {
                            ptype: 'gridviewdragdrop'
                        }
                    },
                    columns: [{
                        xtype: 'rownumberer'
                    },
                    {
                        xtype: 'numbercolumn',
                        dataIndex: 'Trabajador',
                        text: 'Trabajador',
                        hidden: true,
                        hideable: false
                    },
                    {
                        dataIndex: 'DTrabajador',
                        text: 'Trabajador',
                        width: 250,
                        hideable: false
                    },
                    {
                        flex: 1,
                        menuDisabled: true,
                        hideable: false
                    }]
                },
                {
                    xtype: 'gridpanel',
                    itemId: 'grdTrabajador',
                    title: 'Lista de Trabajadores',
                    store: stTrabajador,
                    multiSelect: true,
                    columnLines: true,
                    emptyText: 'No se encontraron datos.',
                    flex: 1,
                    viewConfig: {
                        plugins: {
                            ptype: 'gridviewdragdrop'
                        }
                    },
                    columns: [{
                        xtype: 'rownumberer'
                    },
                    {
                        xtype: 'numbercolumn',
                        dataIndex: 'Trabajador',
                        text: 'Trabajador',
                        hidden: true,
                        hideable: false
                    },
                    {
                        dataIndex: 'DTrabajador',
                        text: 'Trabajador',
                        width: 250,
                        hideable: false
                    },
                    {
                        flex: 1,
                        menuDisabled: true,
                        hideable: false
                    }
                    ]
                }]
            }],
            buttons: [
            {
                itemId: 'btnGuardar',
                iconCls: 'icon-save',
                text: 'Guardar',
                handler: me.onBtnGuardarClick,
                scope: me
            },
            {
                itemId: 'btnSalir',
                iconCls: 'icon-exit',
                text: 'Salir',
                handler: me.onBtnSalirClick,
                scope: me
            }]
        });
        me.callParent(arguments);
    },

    listeners: {
        afterrender: {
            fn: function (component, options) {
                component.getComponent('grdUsuario').getStore().load();
                component.getComponent(1).getComponent('grdUsuarioTrabajador').getStore().on('load', component.onStUsuarioTrabajadorLoad, component);
                component.fnEstadoForm('visualizacion');
                Ext.MessageBox.hide();
            }
        }
    },

    onStUsuarioTrabajadorLoad: function (store, records, successful, eOpts) {
        if (successful) {
            this.fnEstadoForm('edicion');
        }
    },

    onGridUsuarioSelectionChange: function (tablepanel, selections, options) {
        if (selections.length > 0) {
            this.fnEstadoForm('visualizacion');
            this.getComponent(1).getComponent('grdUsuarioTrabajador').getStore().load({
                params: {
                    usuario: parseInt(selections[0].get('Usuario'))
                }
            });
            this.getComponent(1).getComponent('grdTrabajador').getStore().load({
                params: {
                    usuario: parseInt(selections[0].get('Usuario'))
                }
            });
        }
    },

    onBtnGuardarClick: function (button, e, options) {
        var me = this;
        var filas = me.getComponent('grdUsuario').getSelectionModel().getSelection();

        if (filas.length > 0) {
            var records = me.getComponent(1).getComponent('grdUsuarioTrabajador').getStore().getRange();
            var i = 0, length = records.length, record, data = new Array();
            var usuario = parseInt(filas[0].get('Usuario'));

            for (; i < length; ++i) {
                record = records[i];
                var dt = {};
                dt['Trabajador'] = record.data['Trabajador'];
                data.push(dt);
            }

            this.setLoading("Grabando...");
            Ext.Ajax.request({
                url: "../../Seguridad/Trabajador/InsUpdUsuarioTrabajador",
                success: function (response) {
                    this.setLoading(false);
                    var respuesta = Ext.decode(response.responseText);
                    if (respuesta['success'] == "true") {
                        Ext.example.msg('Información', 'Datos se guardaron con exitó');
                    }
                    else {
                        Ext.MessageBox.show({
                            title: 'Sistema RJ Abogados',
                            msg: respuesta['data'],
                            buttons: Ext.MessageBox.OK,
                            animateTarget: button,
                            icon: Ext.Msg.ERROR
                        });
                    }
                },
                failure: function (response) {
                    this.setLoading(false);
                    Ext.MessageBox.show({
                        title: 'Sistema RJ Abogados',
                        msg: 'Se Produjo un error en la conexión.',
                        buttons: Ext.MessageBox.OK,
                        animateTarget: button,
                        icon: Ext.Msg.ERROR
                    });
                },
                params: {
                    usuario: usuario,
                    datos: Ext.encode(data)
                },
                scope: this
            });
        }
        else {
            Ext.MessageBox.show({
                title: 'Sistema RJ Abogados',
                msg: '¡No ha seleccionado ningun usuario!',
                buttons: Ext.MessageBox.OK,
                animateTarget: button,
                icon: Ext.MessageBox.WARNING
            });
        }
    },

    onBtnSalirClick: function (button, e, options) {
        this.close();
    },

    fnEstadoForm: function (cadena) {
        this.getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnGuardar').setDisabled(true);

        switch (cadena) {
            case 'edicion':
                this.getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnGuardar').setDisabled(false);
                break;
            case 'visualizacion':
                break;
            default:
        }
    }
});