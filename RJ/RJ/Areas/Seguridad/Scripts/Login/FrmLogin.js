Ext.define('SegApp.Login.FrmLogin', {
    extend: 'Ext.window.Window',

    bodyStyle: 'padding: 12px;',
    border: false,
    closable: false,
    title: 'Autenticación',
    iconCls: 'icon-frmLogin',
    height: 160,
    width: 300,
    modal: true,
    resizable: false,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'textfield',
                    itemId: 'txtUsuario',
                    fieldLabel: 'Usuario',
                    blankText: '*Este campo es obligatorio.',
                    emptyText: 'Usuario',
                    allowBlank: false,
                    listeners: {
                        specialkey: {
                            fn: me.ontxtUsuarioKeyup,
                            scope: me
                        }
                    }
                },
                {
                    xtype: 'textfield',
                    itemId: 'txtPassword',
                    inputType: 'password',
                    fieldLabel: 'Contraseña',
                    blankText: '*Este campo es obligatorio.',
                    emptyText: 'Contraseña',
                    allowBlank: false,
                    listeners: {
                        specialkey: {
                            fn: me.ontxtPasswordKeyup,
                            scope: me
                        }
                    }
                }
             ],
            buttons: [
                {
                    itemId: 'btnLogin',
                    text: 'Login',
                    iconCls: 'icon-aceptar',
                    listeners: {
                        click: {
                            fn: me.onButtonClick,
                            scope: me
                        }
                    }
                }
            ]
        });
        me.callParent(arguments);
    },

    ontxtUsuarioKeyup: function (textfield, e) {
        if (e.getKey() == e.ENTER) {
            this.getComponent('txtPassword').focus();
        }
    },

    ontxtPasswordKeyup: function (textfield, e) {
        if (e.getKey() == e.ENTER) {
            this.onButtonClick(null, null, null);
        }
    },

    onButtonClick: function (button, e, options) {
        var txtUsuario = this.getComponent('txtUsuario');
        var txtPassword = this.getComponent('txtPassword');

        if (txtUsuario.isValid() && txtPassword.isValid()) {
            this.setLoading("Verificando...");
            Ext.Ajax.request({
                url: "../Autenticacion/Verificar",
                success: function (response) {
                    this.setLoading(false);
                    var data = Ext.decode(response.responseText);
                    if (data.length == 0) {
                        Ext.Msg.show({
                            title: 'RJ Abogados',
                            msg: 'El usuario o contraseña son incorrectos',
                            buttons: Ext.Msg.OK,
                            icon: Ext.Msg.ERROR,
                            fn: this.fnErrorLogin,
                            scope: this
                        });
                    }
                    else {
                        if (data[0]['Activo'].toString() == 'true') {
                            Ext.Msg.show({
                                title: 'RJ Abogados',
                                msg: 'Bienvenido ' + data[0]['Nombres'].toString() + '.',
                                buttons: Ext.Msg.OK,
                                icon: Ext.Msg.INFO,
                                fn: this.fnOKLogin,
                                scope: this
                            });
                        }
                        else {
                            Ext.Msg.show({
                                title: 'RJ Abogados',
                                msg: 'El usuario no se encuentra activo.',
                                buttons: Ext.Msg.OK,
                                icon: Ext.Msg.ERROR,
                                fn: this.fnErrorLogin,
                                scope: this
                            });
                        }
                    }
                },
                failure: function (response) {
                    this.setLoading(false);
                    Ext.Msg.show({
                        title: 'RJ Abogados',
                        msg: "El error ocurrio durante la carga:\n" + response.responseText,
                        buttons: Ext.Msg.OK,
                        icon: Ext.Msg.ERROR
                    });
                },
                params: {
                    usuario: txtUsuario.getValue(),
                    password: txtPassword.getValue(),
                    empresa: 2
                },
                scope: this
            });
        }
    },

    fnOKLogin: function () {
        this.setLoading("Redirigiendo...");
        window.location = '../../Seguridad/Index/Index';
    },

    fnErrorLogin: function () {
        this.getComponent('txtPassword').focus();
        this.getComponent('txtPassword').setValue('');
    }

});