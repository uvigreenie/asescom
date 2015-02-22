Ext.define('CobApp.Cartera.FrmListarPagosXProducto', {
    extend: 'Ext.window.Window',
    itemId: 'FrmListarPagosXProducto',
    frame: true,
    modal: true,
//    plain: true,
    closable: true,
    collapsible: true,
    resizable: true,
//    height: 250,
    width: 225,
    minWidth: 225,
    layout: 'fit'/*{
        type: 'vbox',
        align: 'stretch'
    }*/,
    initComponent: function () {
        var me = this;

        var stPagos = Ext.create('Ext.data.Store', {
            autoLoad: false,
            model: Ext.define('Pagos', { extend: 'Ext.data.Model' }),
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cartera/ListarPagoXProducto',
                reader: { type: 'json', root: 'data' }
            },
            listeners: {
                'metachange': function (store, meta) {
                    this.getComponent(0).getComponent('pnlConsulta').getComponent('grdPagos').reconfigure(store, meta.columns);
//                    this.getComponent(0).getComponent('pnlConsulta').getComponent('grdPagos').setVisible(true);
                    console.log(this.getComponent(0).getComponent('pnlConsulta').getComponent('grdPagos'));
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
//                            bodyStyle: 'padding: 15px;',
                            header: false,
//                            flex: 1,
//                            hidden: true,
//                            layout: {
//                                type: 'table',
//                                columns: 1,
//                                tableAttrs: {
//                                    style: {
//                                        width: '100%',
//                                        height: '100%'
//                                    }
//                                }
//                            },
                            items: [

                                {
                                    xtype: 'gridpanel',
                                    itemId: 'grdPagos',
                                    //                                    title: 'Lista de Pagos',
                                    header: false,
                                    store: stPagos,
                                    emptyText: 'No se encontraron datos.',
                                    width: '100%',
                                    columnLines: true,
                                    columns: []
                                }
                            ]
                        }
                    ]
                }
            ]//,
//            buttons: [
//            //                {
//            //                    itemId: 'btnGuardar',
//            //                    text: 'Guardar',
//            //                    iconCls: 'icon-save',
//            //                    handler: me.onBtnGuardarClick,
//            //                    scope: me
//            //                },
//                {
//                itemId: 'btnSalir',
//                text: 'Salir',
//                iconCls: 'icon-exit',
//                handler: me.onBtnSalirClick,
//                scope: me
//            }
//            ]
        });
        me.callParent(arguments);
    }//,

    //    onBtnGuardarClick: function (button, e, options) {
    //        if (this.fnEsValidoGuardar()) {
    //            var codigo = parseInt(this.getComponent(0).getComponent('pnlRegistro').getComponent('txtMoroso').getValue())
    //            var dtMoroso = {};

    //            dtMoroso['Moroso'] = codigo;
    //            dtMoroso['Empleado'] = this.getComponent(0).getComponent('pnlRegistro').getComponent('chkTrabaja').getValue();
    //            dtMoroso['RubroEmpleo'] = this.getComponent(0).getComponent('pnlRegistro').getComponent('cbxRubroEmpleo').getValue();
    //            dtMoroso['HoraContacto'] = this.getComponent(0).getComponent('pnlRegistro').getComponent('txtHoraContacto').getValue();
    //            dtMoroso['Observacion'] = this.getComponent(0).getComponent('pnlRegistro').getComponent('txtObservacion').getValue();

    //            Ext.Ajax.request({
    //                url: "../../Moroso/UpdMoroso",
    //                success: function (response) {
    //                    var respuesta = Ext.decode(response.responseText);
    //                    if (respuesta['success'] == "true") {
    //                        Ext.example.msg('Información', 'Actualización realizada con exitó');
    //                    }
    //                    else {
    //                        Ext.MessageBox.show({
    //                            title: 'Sistema RJ Abogados',
    //                            msg: respuesta['data'],
    //                            buttons: Ext.MessageBox.OK,
    //                            animateTarget: button,
    //                            icon: Ext.Msg.ERROR
    //                        });
    //                    }
    //                },
    //                failure: function (response) {
    //                    Ext.MessageBox.show({
    //                        title: 'Sistema RJ Abogados',
    //                        msg: 'Se Produjo un error en la conexión.',
    //                        buttons: Ext.MessageBox.OK,
    //                        animateTarget: button,
    //                        icon: Ext.Msg.ERROR
    //                    });
    //                },
    //                params: {
    //                    datos: '[' + Ext.encode(dtMoroso) + ']'
    //                },
    //                scope: this
    //            });
    //        }
    //    },

//    onBtnSalirClick: function (button, e, options) {
//        this.close();
//    } //,

    //    fnEsValidoGuardar: function () {
    //        if (!this.getComponent(0).getComponent('pnlRegistro').getComponent('cbxRubroEmpleo').isValid()) {
    //            return false;
    //        }
    //        if (!this.getComponent(0).getComponent('pnlRegistro').getComponent('txtHoraContacto').isValid()) {
    //            return false;
    //        }
    //        return true;
    //    }
});