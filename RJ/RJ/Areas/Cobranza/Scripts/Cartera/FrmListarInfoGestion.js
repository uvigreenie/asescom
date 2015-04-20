Ext.define('CobApp.Cartera.FrmListarInfoGestion', {
    extend: 'Ext.window.Window',
    itemId: 'FrmListarInfoGestion',
    frame: true,
    modal: true,
    //    plain: true,
    closable: true,
    collapsible: true,
    resizable: true,
    //    height: 250,
    width: 200,
    minWidth: 225,
    layout: 'fit',
    initComponent: function () {
        var me = this;

        var stControlGestionXTrabajador = Ext.create('Ext.data.Store', {
            autoLoad: false,
            model: Ext.define('ControlGestion', { extend: 'Ext.data.Model' }),
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cartera/ListarControlGestionXTrabajador',
                reader: { type: 'json', root: 'data' }
            },
            listeners: {
                'metachange': function (store, meta) {
                    this.getComponent(0).getComponent('pnlConsulta').getComponent('grdControlGestion').reconfigure(store, meta.columns);
                    console.log(this.getComponent(0).getComponent('pnlConsulta').getComponent('grdControlGestion'));
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
                            itemId: 'pnlConsulta',
                            header: false,
                            items: [
                                {
                                    xtype: 'gridpanel',
                                    itemId: 'grdControlGestion',
                                    header: false,
                                    store: stControlGestionXTrabajador,
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