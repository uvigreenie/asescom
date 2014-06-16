Ext.define('SegApp.Usuario.FrmChangePassword', {
    extend: 'Ext.window.Window',

    bodyStyle: 'padding: 12px;',
    border: false,
    title: 'Cambiar Contraseña',
    iconCls: 'icon-frmLogin',
    height: 210,
    width: 305,
    modal: true,
    resizable: false,
    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'textfield',
                    itemId: 'txtPasswordOld',
                    inputType: 'password',
                    width: 268,
                    fieldLabel: 'Contraseña',
                    blankText: '*Este campo es obligatorio.',
                    allowBlank: false,
                    listeners: {
                        specialkey: {
                            fn: me.ontxtPasswordOldKeyup,
                            scope: me
                        }
                    }
                },
                {
                    xtype: 'textfield',
                    itemId: 'txtPasswordNew',
                    inputType: 'password',
                    width: 268,
                    fieldLabel: 'Nueva Contraseña',
                    allowBlank: false,
                    minLength: 3,
                    maxLength: 15,
                    blankText: '*Este campo es obligatorio.',
                    listeners: {
                        specialkey: {
                            fn: me.ontxtPasswordNewKeyup,
                            scope: me
                        }
                    }
                },
                {
                    xtype: 'textfield',
                    itemId: 'txtPasswordRe',
                    inputType: 'password',
                    width: 268,
                    fieldLabel: 'Confirmar Contraseña',
                    allowBlank: false,
                    minLength: 3,
                    maxLength: 15,
                    blankText: '*Este campo es obligatorio.',
                    validator: function () {
                        if (me.getComponent('txtPasswordNew').getValue() == me.getComponent('txtPasswordRe').getValue()) {
                            return true;
                        }
                        else {
                            return 'Las contraseñas no coincide';
                        }
                    },
                    listeners: {
                        specialkey: {
                            fn: me.ontxtPasswordReKeyup,
                            scope: me
                        }
                    }
                }
             ],
            buttons: [
                {
                    itemId: 'btnAplicar',
                    text: 'Aplicar',
                    iconCls: 'icon-aceptar',
                    listeners: {
                        click: {
                            fn: me.onbtnAplicarClick,
                            scope: me
                        }
                    }
                },
                {
                    itemId: 'btnCancelar',
                    text: 'Cancelar',
                    iconCls: 'icon-cancelar',
                    listeners: {
                        click: {
                            fn: me.onbtnCancelarClick,
                            scope: me
                        }
                    }
                }
            ]
        });
        me.callParent(arguments);
    },

    ontxtPasswordOldKeyup: function (textfield, e) {
        if (e.getKey() == e.ENTER) {
            this.getComponent('txtPasswordNew').focus();
        }
    },

    ontxtPasswordNewKeyup: function (textfield, e) {
        if (e.getKey() == e.ENTER) {
            this.getComponent('txtPasswordRe').focus();
        }
    },

    ontxtPasswordReKeyup: function (textfield, e) {
        if (e.getKey() == e.ENTER) {
            this.onbtnAplicarClick(null, null, null);
        }
    },

    onbtnAplicarClick: function (button, e, options) {
        var txtPasswordOld = this.getComponent('txtPasswordOld');
        var txtPasswordNew = this.getComponent('txtPasswordNew');
        var txtPasswordRe = this.getComponent('txtPasswordRe');

        if (txtPasswordOld.isValid() && txtPasswordNew.isValid() && txtPasswordRe.isValid()) {
            Ext.Ajax.request({
                url: "../Autenticacion/ChangePassword",
                success: function (response) {
                    if (response.responseText == "true") {
                        Ext.Msg.show({
                            title: 'RJ Abogados',
                            msg: 'Se realizó el cambio de contraseña con exito.',
                            buttons: Ext.Msg.OK,
                            icon: Ext.Msg.INFO,
                            fn: this.fnOKChange,
                            scope: this
                        });
                    }
                    else {
                        Ext.Msg.show({
                            title: 'RJ Abogados',
                            msg: 'No se pudo completar la acción, intentelo nuevamente.',
                            buttons: Ext.Msg.OK,
                            icon: Ext.Msg.ERROR,
                            fn: this.fnErrorChange,
                            scope: this
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
                },
                params: {
                    passwordOld: txtPasswordOld.getValue(),
                    passwordNew: txtPasswordNew.getValue()
                },
                scope: this
            });
        }
    },

    onbtnCancelarClick: function (button, e, options) {
        this.destroy();
    },

    fnOKChange: function () {
        this.destroy();
    },

    fnErrorChange: function () {
        this.getComponent('txtPasswordOld').focus();
    }

});