Ext.define('CobApp.Moroso.FrmCargarDetalleMoroso', {
    extend: 'Ext.panel.Panel',
    itemId: 'FrmCargarDetalleMoroso',
    closable: true,
    layout: 'fit',
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items:
            [
                {
                    xtype: 'form',
                    itemId: 'pnlCarga',
                    title: 'Carga de Referencias',
                    bodyStyle: 'padding: 20px;',
                    fileUpload: true,
                     tbar: [
                    {
                        xtype: 'label',
                        name: 'message',
                        html: '<b><u>Instrucciones</u></b>',
                        colspan: 2
                    }],
                    items:
                    [

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
                Ext.MessageBox.hide();
            }
        }
    },

    onBtnCargarClick: function (button, e, options) {
        var fp = this.getComponent('pnlCarga');
        if (fp.getForm().isValid()) {
            fp.getForm().submit({
                url: '../../Cobranza/Moroso/CargarDetalleMoroso',
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