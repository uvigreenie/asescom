var stGrupo = Ext.create('Ext.data.Store', {
    fields: [
         { name: 'Grupo', type: 'int' },
         { name: 'DGrupo', type: 'string' }
     ],
    proxy: {
        type: 'ajax',
        url: '../../Seguridad/Grupo/Obtener',
        reader: {
            type: 'json'
        }
    },
    sorters: [{
        property: 'Grupo',
        direction: 'ASC'
    }],
    autoLoad: false
});

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
    filters: [{
        property: 'Activo',
        value: "True"
    }],
    autoLoad: false
});

var stTree = Ext.create('Ext.data.TreeStore', {
    fields: [
            { name: 'menu', type: 'int' },
            { name: 'text', type: 'string' },
            { name: 'acceso', type: 'bool' },
            { name: 'expanded', type: 'bool', defaultValue: true }
    ],
    proxy: {
        type: 'ajax',
        url: '../../Seguridad/Autenticacion/ObtenerAccesoTreeMenu',
        reader: {
            type: 'json'
        },
        extraParams: {
            grupo: 0,
            usuario: 0
        }
    }
});

Ext.define('SegApp.Usuario.FrmMenuUsuario', {
    extend: 'Ext.panel.Panel',
    itemId: 'FrmMenuUsuario',
    title: 'Accesos Usuario',
    closable: true,
    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
            {
                title: 'Accesos Usuarios',
                border: false,
                bodyStyle: 'padding: 15px;',
                items: [
                {
                    xtype: 'combo',
                    itemId: 'cbxGrupo',
                    width: 300,
                    lastQuery: '',
                    fieldLabel: 'Grupo',
                    emptyText: '< Seleccione >',
                    store: stGrupo,
                    displayField: 'DGrupo',
                    valueField: 'Grupo',
                    allowBlank: false,
                    forceSelection: true,
                    queryMode: 'local'
                },
                {
                    xtype: 'combo',
                    itemId: 'cbxUsuario',
                    width: 300,
                    lastQuery: '',
                    fieldLabel: 'Usuario',
                    emptyText: '< Seleccione >',
                    store: stUsuario,
                    displayField: 'Login',
                    valueField: 'Usuario',
                    allowBlank: false,
                    forceSelection: true,
                    queryMode: 'local'
                }
                ]
            },
            {
                xtype: 'treepanel',
                title: 'Listado de Accesos',
                rootVisible: false,
                autoScroll: true,
                border: false,
                useArrows: true,
                store: stTree,
                columns: [
                {
                    xtype: 'treecolumn',
                    dataIndex: 'text',
                    text: 'Nombre',
                    width: 300,
                    hideable: false
                },
                {
                    xtype: 'checkcolumn',
                    dataIndex: 'acceso',
                    text: 'Permitir',
                    width: 100,
                    hideable: false
                },
                {
                    flex: 1,
                    menuDisabled: true,
                    hideable: false
                }]
            }],
            buttons: [
            {
                itemId: 'btnBuscar',
                iconCls: 'icon-buscar',
                text: 'Buscar',
                handler: me.onBtnBuscarClick,
                scope: me
            },
            {
                xtype: 'button',
                itemId: 'btnGuardar',
                text: 'Guardar',
                iconCls: 'icon-save',
                handler: me.onBtnGuardarClick,
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
                itemId: 'btnSalir',
                iconCls: 'icon-exit',
                text: 'Salir',
                handler: me.onBtnSalirClick,
                scope: me
            }]
        });
        me.callParent(arguments);
    },
    listeners: {
        afterrender: {
            fn: function (component, options) {
                stGrupo.load();
                stUsuario.load();
                stTree.load();
                this.fnEstadoForm('busqueda');
                Ext.MessageBox.hide();
            }
        }
    },

    onBtnBuscarClick: function (button, e, options) {
        if (this.fnEsValidoForm()) {
            stTree.load({
                params: {
                    grupo: this.getComponent(0).getComponent('cbxGrupo').getValue(),
                    usuario: this.getComponent(0).getComponent('cbxUsuario').getValue()
                }
            });
            this.fnEstadoForm('edicion');
        }
    },

    onBtnGuardarClick: function (button, e, options) {
        if (this.fnEsValidoForm()) {
            var records = stTree.getModifiedRecords();
            var i = 0, length = records.length, record;
            var data = new Array();

            for (; i < length; ++i) {
                record = records[i];
                if (record.data['leaf']) {
                    var dtMenu = {};
                    dtMenu['Menu'] = record.data['menu'];
                    dtMenu['Acceso'] = record.data['acceso'];
                    data.push(dtMenu);
                }
            }

            this.setLoading("Grabando...");
            Ext.Ajax.request({
                url: "../../Seguridad/Menu/GuardarMenuUsuario",
                success: function (response) {
                    this.setLoading(false);
                    var respuesta = Ext.decode(response.responseText);
                    if (respuesta['success'] == "true") {
                        if (respuesta['data'] == "true") {
                            Ext.example.msg('Información', 'Datos se guardaron con exitó');
                            this.onBtnBuscarClick(null, null, null);
                        }
                        else {
                            Ext.example.msg('Información', 'No ha realizado ningun cambio.');
                        }
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
                    datos: Ext.encode(data),
                    usuario: this.getComponent(0).getComponent('cbxUsuario').getValue()
                },
                scope: this
            });
        }
    },

    onBtnCancelarClick: function (button, e, options) {
        this.fnLimpiarControles();
        stTree.load();
        this.fnEstadoForm('busqueda');
    },

    onBtnSalirClick: function (button, e, options) {
        this.close();
    },

    fnEstadoForm: function (cadena) {
        this.fnBloquearControles(true);
        this.getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnBuscar').setDisabled(true);
        this.getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnGuardar').setDisabled(true);
        this.getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnCancelar').setDisabled(true);

        switch (cadena) {
            case 'busqueda':
                this.fnBloquearControles(false);
                this.getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnBuscar').setDisabled(false);
                break;
            case 'edicion':
                this.getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnGuardar').setDisabled(false);
                this.getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnCancelar').setDisabled(false);
                break;
            default:
        }
    },

    fnBloquearControles: function (estado) {
        this.getComponent(0).getComponent('cbxGrupo').setDisabled(estado);
        this.getComponent(0).getComponent('cbxUsuario').setDisabled(estado);
    },

    fnLimpiarControles: function () {
        this.getComponent(0).getComponent('cbxGrupo').clearValue();
        this.getComponent(0).getComponent('cbxUsuario').clearValue();
    },

    fnEsValidoForm: function () {
        if (!this.getComponent(0).getComponent('cbxGrupo').isValid()) {
            return false;
        }
        if (!this.getComponent(0).getComponent('cbxUsuario').isValid()) {
            return false;
        }
        return true;
    }
});