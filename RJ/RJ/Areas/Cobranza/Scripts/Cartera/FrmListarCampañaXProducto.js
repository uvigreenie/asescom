Ext.define('CobApp.Cartera.FrmListarCampañaXProducto', {
    extend: 'Ext.window.Window',
    itemId: 'FrmListarCampañaXProducto',
    frame: true,
    modal: true,
    //    plain: true,
    closable: true,
    collapsible: true,
    resizable: true,
    //    height: 250,
    width: 425,
    minWidth: 225,
    layout: 'fit',
    initComponent: function () {
        var me = this;

        var stCampaña = Ext.create('Ext.data.Store', {
            autoLoad: false,
            model: Ext.define('Campaña', { extend: 'Ext.data.Model' }),
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cartera/ListarCampañaXProducto',
                reader: { type: 'json', root: 'data' }
            },
            listeners: {
                'metachange': function (store, meta) {
                    this.getComponent(0).getComponent('pnlConsulta').getComponent('grdCampaña').reconfigure(store, meta.columns);
                    console.log(this.getComponent(0).getComponent('pnlConsulta').getComponent('grdCampaña'));
                },
                scope: this
            }
        });

        Ext.applyIf(me, {
            items: [
                {
                    border: false,
                    header: false,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    flex: 1,
                    items: [
                        {
                            itemId: 'pnlMoroso',
                            bodyStyle: 'padding:6px;',
                            layout: {
                                type: 'table',
                                columns: 2,
                                tdAttrs: {
                                    style: {
                                        padding: '2px'
                                    }
                                },
                                tableAttrs: {
                                    style: {
                                        width: '100%'
                                    }
                                }
                            },
                            header: false,
                            items: [
                                {
                                    xtype: 'label',
                                    itemId: 'txtCodigoCliente',
                                    tpl: '<b>Código:</b> {CodigoCliente}',
                                    data: {
                                        CodigoCliente: ''
                                    }
                                },
                                {
                                    xtype: 'label',
                                    itemId: 'txtNroProducto',
                                    tpl: '<b>Nro Producto:</b> {NroProducto}',
                                    data: {
                                        NroProducto: ''
                                    }
                                },
                                {
                                    xtype: 'label',
                                    itemId: 'txtNombre',
                                    tpl: '<b>Nombre:</b> {Nombre}',
                                    data: {
                                        Nombre: ''
                                    },
                                    colspan: 2
                                },
                                {
                                    xtype: 'label',
                                    itemId: 'txtTipoProducto',
                                    tpl: '<b>Producto:</b> {Producto}',
                                    data: {
                                        Producto: ''
                                    }
                                },
                                {
                                    xtype: 'label',
                                    itemId: 'txtDeuda',
                                    tpl: '<b>Deuda:</b> {Deuda}',
                                    data: {
                                        Deuda: ''
                                    }
                                }
                            ]
                        },
                        {
                            itemId: 'pnlConsulta',
                            header: false,
                            items: [
                                {
                                    xtype: 'gridpanel',
                                    itemId: 'grdCampaña',
                                    header: false,
                                    store: stCampaña,
                                    emptyText: 'No se encontraron datos.',
                                    width: '100%',
                                    columnLines: true,
                                    columns: []
                                }
                            ]
                        }
                    ]
                }
            ]
        });
        me.callParent(arguments);
    }
});