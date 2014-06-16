Ext.define('CobApp.Moroso.FrmRegistrarDatosMoroso', {
    extend: 'Ext.window.Window',
    itemId: 'FrmRegistrarDatosMoroso',
    frame: true,
    modal: true,
    plain: false,
    closable: false,
    resizable: false,
    height: 350,
    width: 500,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    initComponent: function () {
        var me = this;

        var stRubroEmpleo = Ext.create('Ext.data.Store', {
            autoLoad: true,
            fields: [
         { name: 'RubroEmpleo', type: 'int' },
         { name: 'DRubroEmpleo', type: 'string' }
     ],
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Moroso/ListarRubroEmpleo',
                reader: {
                    type: 'json'
                }
            },
            sorters: [{
                property: 'DRubroEmpleo',
                direction: 'ASC'
            }]
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
                            itemId: 'pnlRegistro',
                            bodyStyle: 'padding: 15px;',
                            header: false,
                            flex: 1,
                            layout: {
                                type: 'table',
                                columns: 1,
                                tableAttrs: {
                                    style: {
                                        width: '100%',
                                        height: '100%'
                                    }
                                }
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    itemId: 'txtMoroso',
                                    hidden: true
                                },
                                {
                                    xtype: 'textfield',
                                    itemId: 'txtNumeroDocumento',
                                    fieldLabel: 'N° Documento',
                                    readOnly: true
                                },
                                {
                                    xtype: 'textfield',
                                    itemId: 'txtDMoroso',
                                    fieldLabel: 'Nombres',
                                    readOnly: true,
                                    anchor: '100%',
                                    width: 450,
                                    colspan: 2
                                },
                                {
                                    xtype: 'checkboxfield',
                                    itemId: 'chkTrabaja',
                                    hideLabel: true,
                                    boxLabel: 'Trabaja'
                                },
                                {
                                    xtype: 'combo',
                                    itemId: 'cbxRubroEmpleo',
                                    lastQuery: '',
                                    store: stRubroEmpleo,
                                    fieldLabel: 'Rubro Empleo',
                                    emptyText: '< Seleccione >',
                                    displayField: 'DRubroEmpleo',
                                    valueField: 'RubroEmpleo',
                                    allowBlank: true,
                                    forceSelection: true,
                                    queryMode: 'local'
                                },
                                {
                                    xtype: 'timefield',
                                    itemId: 'txtHoraContacto',
                                    fieldLabel: 'Hora Contacto',
                                    minValue: '6:00 AM',
                                    maxValue: '11:00 PM',
                                    increment: 30,
                                    colspan: 2
                                },
                                {
                                    xtype: 'label',
                                    name: 'message',
                                    text: 'Observaciones Adicionales:',
                                    colspan: 2
                                },
                                {
                                    xtype: 'textareafield',
                                    itemId: 'txtObservacion',
                                    grow: true,
                                    anchor: '100%',
                                    width: '100%',
                                    colspan: 2
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
                },
                {
                    itemId: 'btnCancelar',
                    text: 'Cancelar',
                    iconCls: 'icon-cancelar',
                    handler: me.onBtnCancelarClick,
                    scope: me
                }
            ]
        });
        me.callParent(arguments);
    },

    onBtnGuardarClick: function (button, e, options) {
        if (this.fnEsValidoGuardar()) {
            var codigo = parseInt(this.getComponent(0).getComponent('pnlRegistro').getComponent('txtMoroso').getValue())
            var dtMoroso = {};

            dtMoroso['Moroso'] = codigo;
            dtMoroso['Empleado'] = this.getComponent(0).getComponent('pnlRegistro').getComponent('chkTrabaja').getValue();
            dtMoroso['RubroEmpleo'] = this.getComponent(0).getComponent('pnlRegistro').getComponent('cbxRubroEmpleo').getValue();
            dtMoroso['HoraContacto'] = this.getComponent(0).getComponent('pnlRegistro').getComponent('txtHoraContacto').getValue();
            dtMoroso['Observacion'] = this.getComponent(0).getComponent('pnlRegistro').getComponent('txtObservacion').getValue();

            Ext.Ajax.request({
                url: "../../Moroso/UpdMoroso",
                success: function (response) {
                    var respuesta = Ext.decode(response.responseText);
                    if (respuesta['success'] == "true") {
                        Ext.example.msg('Información', 'Actualización realizada con exitó');
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
                    Ext.MessageBox.show({
                        title: 'Sistema RJ Abogados',
                        msg: 'Se Produjo un error en la conexión.',
                        buttons: Ext.MessageBox.OK,
                        animateTarget: button,
                        icon: Ext.Msg.ERROR
                    });
                },
                params: {
                    datos: '[' + Ext.encode(dtMoroso) + ']'
                },
                scope: this
            });
        }
    },

    onBtnCancelarClick: function (button, e, options) {
        this.close();
    },

    fnEsValidoGuardar: function () {
        if (!this.getComponent(0).getComponent('pnlRegistro').getComponent('cbxRubroEmpleo').isValid()) {
            return false;
        }
        if (!this.getComponent(0).getComponent('pnlRegistro').getComponent('txtHoraContacto').isValid()) {
            return false;
        }
        return true;
    }
});