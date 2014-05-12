var stUsuario = Ext.create('Ext.data.Store', {
    fields: [
         { name: 'Usuario', type: 'int' },
         { name: 'Login', type: 'string' },
         { name: 'Nombres', type: 'string' },
         { name: 'Correo', type: 'string' },
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
    autoLoad: false
});

Ext.define('SegApp.Usuario.FrmRegistrarUsuario', {
    extend: 'Ext.panel.Panel',
    itemId: 'FrmRegistrarUsuario',
    layout: 'border',
    title: 'Registrar Usuarios',
    closable: true,
    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [{
                xtype: 'gridpanel',
                itemId: 'grdUsuario',
                region: 'center',
                title: 'Lista de Usuarios',
                store: stUsuario,
                columnLines: true,
                emptyText: 'No se encontraron datos.',
                tbar: [{
                    itemId: 'btnAgregar',
                    iconCls: 'icon-add',
                    text: 'Agregar',
                    handler: me.onBtnAgregarClick,
                    scope: me
                },
                {
                    itemId: 'btnEditar',
                    iconCls: 'icon-edit',
                    text: 'Editar',
                    handler: me.onBtnEditarClick,
                    scope: me
                },
                {
                    itemId: 'btnAnular',
                    iconCls: 'icon-cancel',
                    text: 'Eliminar',
                    handler: me.onBtnAnularClick,
                    scope: me
                },
                {
                    xtype: 'tbfill'
                },
                {
                    itemId: 'btnActualizar',
                    iconCls: 'icon-refresh',
                    tooltip: 'Actualizar',
                    handler: me.onBtnActualizarClick,
                    scope: me
                },
                {
                    itemId: 'btnExportar',
                    iconCls: 'icon-export',
                    tooltip: 'Exportar'
                },
                {
                    itemId: 'btnImprimir',
                    iconCls: 'icon-print',
                    tooltip: 'Imprimir',
                    handler: me.onBtnImprimirClick,
                    scope: me
                },
                {
                    itemId: 'btnSalir',
                    iconCls: 'icon-exit',
                    tooltip: 'Salir',
                    handler: me.onBtnSalirClick,
                    scope: me
                }],
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
                    text: 'Usuario',
                    width: 150,
                    hideable: false
                },
                {
                    dataIndex: 'Nombres',
                    text: 'Nombres',
                    width: 220,
                    hideable: false
                },
                {
                    dataIndex: 'Correo',
                    text: 'Correo',
                    width: 200,
                    hideable: false
                },
                {
                    xtype: 'checkcolumn',
                    dataIndex: 'Activo',
                    text: 'Activo',
                    lockable: false,
                    processEvent: function () { return false; },
                    width: 80,
                    hideable: false
                },
                {
                    flex: 1,
                    menuDisabled: true,
                    hideable: false
                }]
            },
            {
                itemId: 'pnlRegistro',
                title: 'Crear / Editar',
                region: 'east',
                border: false,
                split: true,
                collapsed: true,
                collapsible: true,
                width: 400,
                bodyStyle: 'padding:20px;',
                layout: 'form',
                items: [{
                    xtype: 'textfield',
                    itemId: 'txtUsuario',
                    fieldLabel: 'Usuario',
                    value: 0,
                    hidden: true
                },
                {
                    xtype: 'textfield',
                    itemId: 'txtLogin',
                    fieldLabel: 'Usuario',
                    allowBlank: false,
                    blankText: 'Este campo es obligatorio.'
                },
                {
                    xtype: 'textfield',
                    itemId: 'txtNombres',
                    fieldLabel: 'Nombres',
                    allowBlank: false,
                    blankText: 'Este campo es obligatorio.'
                },
                {
                    xtype: 'textfield',
                    itemId: 'txtCorreo',
                    fieldLabel: 'Correo',
                    vtype: 'email'
                },
                {
                    xtype: 'checkboxfield',
                    itemId: 'chkActivo',
                    hideLabel: true,
                    boxLabel: 'Activo'
                }],
                buttons: [{
                    xtype: 'button',
                    itemId: 'btnGuardar',
                    text: 'Guardar',
                    textAlign: 'right',
                    iconCls: 'icon-save',
                    handler: me.onBtnGuardarClick,
                    scope: me
                },
                {
                    xtype: 'button',
                    itemId: 'btnCancelar',
                    text: 'Cancelar',
                    textAlign: 'right',
                    iconCls: 'icon-cancelar',
                    handler: me.onBtnCancelarClick,
                    scope: me
                }]
            }]
        });
        me.callParent(arguments);
    },

    listeners: {
        afterrender: {
            fn: function (component, options) {
                stUsuario.load();
                this.fnEstadoForm('visualizacion');
                Ext.MessageBox.hide();
            }
        }
    },

    onBtnAgregarClick: function (button, e, options) {
        this.fnLimpiarControles();
        this.getComponent('pnlRegistro').expand();
        this.fnEstadoForm('registro');
    },

    onBtnEditarClick: function (button, e, options) {
        var filas = this.getComponent('grdUsuario').getSelectionModel().getSelection();

        if (filas.length > 0) {
            this.getComponent('pnlRegistro').getComponent('txtUsuario').setValue(filas[0].get('Usuario'));
            this.getComponent('pnlRegistro').getComponent('txtLogin').setValue(filas[0].get('Login'));
            this.getComponent('pnlRegistro').getComponent('txtNombres').setValue(filas[0].get('Nombres'));
            this.getComponent('pnlRegistro').getComponent('txtCorreo').setValue(filas[0].get('Correo'));
            this.getComponent('pnlRegistro').getComponent('chkActivo').setValue(filas[0].get('Activo'));
            this.getComponent('pnlRegistro').expand();
            this.fnEstadoForm('edicion');
        }
        else {
            Ext.MessageBox.show({
                title: 'RJ Abogados',
                msg: 'No ha seleccionado ningun registro!',
                buttons: Ext.MessageBox.OK,
                animateTarget: button,
                icon: Ext.MessageBox.WARNING
            });
        }
    },

    onBtnAnularClick: function (button, e, options) {
        var me = this;
        var filas = me.getComponent('grdUsuario').getSelectionModel().getSelection();

        if (filas.length > 0) {
            Ext.MessageBox.show({
                title: 'RJ Abogados',
                msg: 'Se eliminara el registro permanentemente. ¿Desea Continuar?',
                buttons: Ext.MessageBox.YESNO,
                animateTarget: button,
                icon: Ext.MessageBox.QUESTION,
                fn: me.fnAnularRegistro,
                scope: me
            });
        }
        else {
            Ext.MessageBox.show({
                title: 'RJ Abogados',
                msg: '¡No ha seleccionado ningun registro!',
                buttons: Ext.MessageBox.OK,
                animateTarget: button,
                icon: Ext.MessageBox.WARNING
            });
        }
    },

    onBtnActualizarClick: function (button, e, options) {
        stUsuario.load();
    },

    onBtnImprimirClick: function (button, e, options) {
        Ext.ux.grid.Printer.printAutomatically = false;
        Ext.ux.grid.Printer.print(this.getComponent('grdUsuario'));
    },

    onBtnGuardarClick: function (button, e, options) {
        if (this.fnEsValidoGuardar()) {
            var codigo = parseInt(this.getComponent('pnlRegistro').getComponent('txtUsuario').getValue())
            var dtUsuario = {};

            dtUsuario['Usuario'] = codigo;
            dtUsuario['Login'] = this.getComponent('pnlRegistro').getComponent('txtLogin').getValue();
            dtUsuario['Nombres'] = this.getComponent('pnlRegistro').getComponent('txtNombres').getValue();
            dtUsuario['Correo'] = this.getComponent('pnlRegistro').getComponent('txtCorreo').getValue();
            dtUsuario['Activo'] = this.getComponent('pnlRegistro').getComponent('chkActivo').getValue();

            Ext.Ajax.request({
                url: "../../Seguridad/Autenticacion/InsUpdUsuarios",
                success: function (response) {
                    var respuesta = Ext.decode(response.responseText);
                    if (respuesta['success'] == "true") {
                        if (codigo == 0) {
                            Ext.example.msg('Información', 'Se registro con exitó');
                        }
                        else {
                            Ext.example.msg('Información', 'Actualización realizada con exitó');
                        }
                        this.onBtnActualizarClick(null, null, null);
                        this.fnLimpiarControles();
                        this.getComponent('pnlRegistro').collapse();
                        this.fnEstadoForm('visualizacion');
                    }
                    else {
                        Ext.MessageBox.show({
                            title: 'RJ Abogados',
                            msg: respuesta['data'],
                            buttons: Ext.MessageBox.OK,
                            animateTarget: button,
                            icon: Ext.Msg.ERROR
                        });
                    }
                },
                failure: function (response) {
                    Ext.MessageBox.show({
                        title: 'RJ Abogados',
                        msg: 'Se Produjo un error en la conexión.',
                        buttons: Ext.MessageBox.OK,
                        animateTarget: button,
                        icon: Ext.Msg.ERROR
                    });
                },
                params: {
                    datos: '[' + Ext.encode(dtUsuario) + ']'
                },
                scope: this
            });
        }
    },

    onBtnCancelarClick: function (button, e, options) {
        this.fnLimpiarControles();
        this.getComponent('pnlRegistro').collapse();
        this.fnEstadoForm('visualizacion');
    },

    onBtnSalirClick: function (button, e, options) {
        this.close();
    },

    fnAnularRegistro: function (btn) {
        if (btn == 'yes') {
            this.setLoading("Anulando...");
            var filas = this.getComponent('grdUsuario').getSelectionModel().getSelection();
            Ext.Ajax.request({
                url: "../../Seguridad/Autenticacion/AnularUsuario",
                success: function (response) {
                    this.setLoading(false);
                    var respuesta = Ext.decode(response.responseText);
                    if (respuesta['success'] == "true") {
                        Ext.example.msg('Información', 'Se anulo el registro con exitó');
                        this.onBtnActualizarClick(null, null, null);
                    }
                    else {
                        Ext.MessageBox.show({
                            title: 'RJ Abogados',
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
                        title: 'RJ Abogados',
                        msg: 'Se Produjo un error en la conexión.',
                        buttons: Ext.MessageBox.OK,
                        animateTarget: button,
                        icon: Ext.Msg.ERROR
                    });
                },
                params: {
                    usuario: parseInt(filas[0].get('Usuario'))
                },
                scope: this
            });
        }
    },

    fnEstadoForm: function (cadena) {
        this.fnBloquearControles(true);
        this.getComponent('pnlRegistro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnGuardar').setDisabled(true);
        this.getComponent('pnlRegistro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnCancelar').setDisabled(true);
        this.getComponent('grdUsuario').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnAgregar').setDisabled(true);
        this.getComponent('grdUsuario').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnEditar').setDisabled(true);
        this.getComponent('grdUsuario').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnAnular').setDisabled(true);
        this.getComponent('grdUsuario').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnActualizar').setDisabled(true);
        this.getComponent('grdUsuario').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnExportar').setDisabled(true);
        this.getComponent('grdUsuario').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnImprimir').setDisabled(true);
        this.getComponent('pnlRegistro').getComponent('txtLogin').setDisabled(true);
        this.getComponent('grdUsuario').setDisabled(true);

        switch (cadena) {
            case 'visualizacion':
                this.getComponent('grdUsuario').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnAgregar').setDisabled(false);
                this.getComponent('grdUsuario').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnEditar').setDisabled(false);
                this.getComponent('grdUsuario').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnAnular').setDisabled(false);
                this.getComponent('grdUsuario').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnActualizar').setDisabled(false);
                this.getComponent('grdUsuario').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnExportar').setDisabled(false);
                this.getComponent('grdUsuario').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnImprimir').setDisabled(false);
                this.getComponent('grdUsuario').setDisabled(false);
                break;
            case 'edicion':
                this.fnBloquearControles(false);
                this.getComponent('pnlRegistro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnGuardar').setDisabled(false);
                this.getComponent('pnlRegistro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnCancelar').setDisabled(false);
                break;
            case 'registro':
                this.fnBloquearControles(false);
                this.getComponent('pnlRegistro').getComponent('txtLogin').setDisabled(false);
                this.getComponent('pnlRegistro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnGuardar').setDisabled(false);
                this.getComponent('pnlRegistro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnCancelar').setDisabled(false);
                this.getComponent('grdUsuario').setDisabled(false);
                break;
            default:
        }
    },

    fnBloquearControles: function (estado) {
        this.getComponent('pnlRegistro').getComponent('txtNombres').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('txtCorreo').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('chkActivo').setDisabled(estado);
    },

    fnLimpiarControles: function () {
        this.getComponent('pnlRegistro').getComponent('txtUsuario').setValue(0);
        this.getComponent('pnlRegistro').getComponent('txtLogin').setValue('');
        this.getComponent('pnlRegistro').getComponent('txtNombres').setValue('');
        this.getComponent('pnlRegistro').getComponent('txtCorreo').setValue('');
    },

    fnEsValidoGuardar: function () {
        if (!this.getComponent('pnlRegistro').getComponent('txtLogin').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlRegistro').getComponent('txtNombres').isValid()) {
            return false;
        }
        return true;
    }

});