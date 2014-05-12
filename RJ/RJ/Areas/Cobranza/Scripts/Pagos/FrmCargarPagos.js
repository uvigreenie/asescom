Ext.define('CobApp.Pagos.FrmCargarPagos', {
    extend: 'Ext.panel.Panel',
    itemId: 'FrmCargarPagos',
    closable: true,
    layout: 'fit',
    initComponent: function () {
        var me = this;

        var stTipo = Ext.create('Ext.data.Store', {
            fields: [
                 { name: 'Tipo', type: 'int' },
                 { name: 'Descripcion', type: 'string' }
             ],
            data: { 'items': [
                    { 'Tipo': '1', "Descripcion": "Pagos" },
                    { 'Tipo': '2', "Descripcion": "Depuraciones" }
                    ]
            },
            proxy: {
                type: 'memory',
                reader: {
                    type: 'json',
                    root: 'items'
                }
            },
            autoLoad: false
        });

        var stCliente = Ext.create('Ext.data.Store', {
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cliente/Listar',
                reader: { type: 'json', root: 'data' }
            }
        });

        var stGestionCliente = Ext.create('Ext.data.Store', {
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/GestionCliente/Listar',
                reader: { type: 'json', root: 'data' }
            }
        });

        Ext.applyIf(me, {
            items:
            [
                {
                    xtype: 'form',
                    itemId: 'pnlCarga',
                    title: 'Carga de Pagos',
                    bodyStyle: 'padding: 20px;',
                    fileUpload: true,
                    items:
                    [
                        {
                            xtype: 'combo',
                            itemId: 'cbxCliente',
                            lastQuery: '',
                            fieldLabel: 'Cliente',
                            name: 'cliente',
                            emptyText: '< Seleccione >',
                            store: stCliente,
                            displayField: 'DCliente',
                            valueField: 'Cliente',
                            allowBlank: false,
                            forceSelection: true,
                            queryMode: 'local',
                            labelWidth: 120,
                            anchor: '50%',
                            listeners: {
                                select: {
                                    fn: this.oncbxClienteSelect,
                                    scope: me
                                }
                            }
                        },
                        {
                            xtype: 'combo',
                            itemId: 'cbxGestionCliente',
                            lastQuery: '',
                            fieldLabel: 'Gestion Cliente',
                            name: 'gestionCliente',
                            emptyText: '< Seleccione >',
                            store: stGestionCliente,
                            displayField: 'DGestionCliente',
                            valueField: 'GestionCliente',
                            allowBlank: false,
                            forceSelection: true,
                            queryMode: 'local',
                            labelWidth: 120,
                            anchor: '50%'
                        },
                        {
                            xtype: 'combo',
                            itemId: 'cbxTipo',
                            lastQuery: '',
                            fieldLabel: 'Tipo Archivo',
                            name: 'tipo',
                            emptyText: 'Seleccione Tipo de Archivo...',
                            store: stTipo,
                            displayField: 'Descripcion',
                            valueField: 'Tipo',
                            allowBlank: false,
                            forceSelection: true,
                            queryMode: 'local',
                            labelWidth: 120,
                            anchor: '50%'
                        },
                        {
                            xtype: 'fileuploadfield',
                            itemId: 'form-file',
                            emptyText: 'Seleccione Archivo',
                            fieldLabel: 'Archivo',
                            name: 'file',
                            allowBlank: false,
                            buttonConfig: {
                                iconCls: 'icon-upload'
                            },
                            labelWidth: 120,
                            anchor: '50%'
                        }
                    ],
                    buttons:
                    [
                        {
                            xtype: 'button',
                            itemId: 'btnCargar',
                            text: 'Cargar',
                            iconCls: 'icon-save',
                            handler: me.onBtnCargarClick,
                            scope: me
                        },
                        {
                            xtype: 'button',
                            itemId: 'btnCancelar',
                            text: 'Cancelar',
                            iconCls: 'icon-cancelar',
                            handler: me.onBtnCancelarClick,
                            scope: me
                        },
                        {
                            xtype: 'button',
                            itemId: 'btnSalir',
                            text: 'Salir',
                            iconCls: 'icon-exit',
                            handler: me.onBtnSalirClick,
                            scope: me
                        }
                    ]
                }
            ]
        });
        me.callParent(arguments);
    },

    listeners: {
        afterrender: {
            fn: function (component, options) {
                this.getComponent('pnlCarga').getComponent('cbxCliente').getStore().load();
                Ext.MessageBox.hide();
            }
        }
    },

    oncbxClienteSelect: function (combo, records, eOpts) {
        this.getComponent('pnlCarga').getComponent('cbxGestionCliente').getStore().load({
            params: {
                cliente: parseInt(this.getComponent('pnlCarga').getComponent('cbxCliente').getValue())
            }
        });
    },

    onBtnCargarClick: function (button, e, options) {
        var fp = this.getComponent('pnlCarga');
        if (fp.getForm().isValid()) {
            fp.getForm().submit({
                url: '../../Cobranza/Pagos/CargarPagos',
                waitMsg: 'Subiendo Archivo...',
                waitConfig: { interval: 100 },
                timeout: 1,
                reader: {
                    type: 'json'
                },
                success: function (fp, o) {
                    if (o.result.success == "true") {
                        Ext.Msg.show({
                        title: 'RJ Abogados',
                        msg: "Se realizo la carga con exito.",
                        buttons: Ext.Msg.OK,
                        icon: Ext.Msg.INFO
                    });
                    }
                    else {
                        Ext.MessageBox.show({
                            title: 'RJ Abogados',
                            msg: o.result.data,
                            buttons: Ext.MessageBox.OK,
                            animateTarget: button,
                            icon: Ext.Msg.ERROR
                        });
                    }
                },
                failure: function (fp, o) {
                    Ext.Msg.show({
                        title: 'RJ Abogados',
                        msg: "Ha ocurrido un error en la conexión.",
                        buttons: Ext.Msg.OK,
                        icon: Ext.Msg.ERROR
                    });
                },
            });
        }
    },

    onBtnCancelarClick: function (button, e, options) {
        this.getComponent('pnlCarga').getForm().reset();
    },

    onBtnSalirClick: function (button, e, options) {
        this.close();
    }
});