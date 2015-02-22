Ext.define('CobApp.Cartera.FrmCargarSectores', {
    extend: 'Ext.window.Window',
    itemId: 'FrmCargarSectores',
    frame: true,
    modal: true,
    border: false,
    plain: false,
    closable: true,
    resizable: false,
    height: 150,
    width: 400,
    //    layout: {
    //        type: 'vbox',
    //        align: 'stretch'
    //    },
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items:
            [
                {
                    border: false,
                    header: false,
//                    layout: {
//                        type: 'vbox',
//                        align: 'stretch'
//                    },
//                    flex: 1,
                    items:
                    [
                        {
                            xtype: 'form',
                            itemId: 'pnlCargaSectores',
//                            title: 'Carga de Sectores',
                            bodyStyle: 'padding: 10px;',
                            border: false,
                            fileUpload: true,
                            items:
                            [
                                {
                                    xtype: 'fileuploadfield',
                                    itemId: 'fileSectores',
                                    emptyText: 'Seleccione Archivo',
                                    fieldLabel: 'Archivo',
                                    name: 'file',
                                    width: 365,
                                    allowBlank: false,
                                    buttonConfig: {
                                        iconCls: 'icon-upload'
                                    },
                                    labelWidth: 50
//                                    anchor: '50%'
                                }
                            ]
                        }
                    ]
                }
            ],
            buttons: [
                {
                    itemId: 'btnGuardar',
                    text: 'Guardar',
                    iconCls: 'icon-save',
                    handler: me.onBtnGuardarClick,
                    scope: me
                }//,
            //                {
            //                    itemId: 'btnCancelar',
            //                    text: 'Cancelar',
            //                    iconCls: 'icon-cancelar',
            //                    handler: me.onBtnCancelarClick,
            //                    scope: me
            //                }
            ]
        });
        me.callParent(arguments);
    },

    onBtnGuardarClick: function (button, e, options) {
//        if (this.fnEsValidoGuardar()) {
        var fp = this.getComponent(0).getComponent('pnlCargaSectores');
        if (fp.getForm().isValid()) {
        fp.getForm().submit({
            url: '../../Cobranza/Cartera/CargarSectores',
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
    }

    //    onBtnCancelarClick: function (button, e, options) {
    //        this.close();
    //    },

//    fnEsValidoGuardar: function () {
//        if (!this.getComponent(0).getComponent('pnlRegistro').getComponent('fileSector').isValid()) {
//            return false;
//        }
//        //        if (!this.getComponent(0).getComponent('pnlRegistro').getComponent('txtHoraContacto').isValid()) {
//        //            return false;
//        //        }
//        return true;
//    }
});