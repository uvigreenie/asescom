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

var stPadre = Ext.create('Ext.data.Store', {
    fields: [
         { name: 'Menu', type: 'int' },
         { name: 'Ruta', type: 'string' },
         { name: 'Leaf', type: 'bool' }
     ],
    proxy: {
        type: 'ajax',
        url: '../../Seguridad/Menu/Obtener',
        reader: {
            type: 'json'
        },
        extraParams: {
            grupo: -1
        }
    },
    sorters: [{
        property: 'Menu',
        direction: 'ASC'
    }],
    filters: [{
        property: 'Leaf',
        value: "False"
    }],
    autoLoad: false
});

var stMenu = Ext.create('Ext.data.Store', {
    fields: [
         { name: 'Menu', type: 'int' },
         { name: 'Grupo', type: 'int' },
         { name: 'DGrupo', type: 'string' },
         { name: 'Padre', type: 'int' },
         { name: 'DPadre', type: 'string' },
         { name: 'DMenu', type: 'string' },
         { name: 'Tooltip', type: 'string' },
         { name: 'Clase', type: 'string' },
         { name: 'Ruta', type: 'string' },
         { name: 'Icono', type: 'string' },
         { name: 'Leaf', type: 'bool' }
     ],
    proxy: {
        type: 'ajax',
        url: '../../Seguridad/Menu/Obtener',
        reader: {
            type: 'json'
        },
        extraParams: {
            grupo: 0
        }
    },
    autoLoad: false
});

Ext.define('SegApp.Menu.FrmRegistrarMenu', {
    extend: 'Ext.panel.Panel',
    itemId: 'FrmRegistrarMenu',
    layout: 'border',
    title: 'Maestro de Menús',
    closable: true,
    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [{
                xtype: 'gridpanel',
                itemId: 'grdMenu',
                region: 'center',
                title: 'Lista de Menús',
                store: stMenu,
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
                    dataIndex: 'Menu',
                    text: 'Menu',
                    hidden: true,
                    hideable: false
                },
                {
                    dataIndex: 'DMenu',
                    text: 'Nombre',
                    locked: true,
                    width: 200,
                    hideable: false
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'Grupo',
                    text: 'Grupo',
                    hidden: true,
                    hideable: false
                },
                {
                    dataIndex: 'DGrupo',
                    text: 'Grupo',
                    lockable: false,
                    width: 200,
                    hideable: false
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'Padre',
                    text: 'Padre',
                    hidden: true,
                    hideable: false
                },
                {
                    dataIndex: 'DPadre',
                    text: 'Ruta',
                    lockable: false,
                    width: 230,
                    hideable: false
                },
                {
                    dataIndex: 'Tooltip',
                    text: 'Tooltip',
                    lockable: false,
                    width: 200,
                    hideable: false
                },
                {
                    dataIndex: 'Clase',
                    text: 'Clase',
                    lockable: false,
                    width: 200,
                    hideable: false
                },
                {
                    dataIndex: 'Icono',
                    text: 'Icono',
                    lockable: false,
                    width: 120,
                    hideable: false
                },
                {
                    xtype: 'checkcolumn',
                    dataIndex: 'Leaf',
                    text: 'Formulario',
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
                    itemId: 'txtMenu',
                    fieldLabel: 'Menu',
                    value: 0,
                    hidden: true
                },
                {
                    xtype: 'combo',
                    itemId: 'cbxGrupo',
                    lastQuery: '',
                    fieldLabel: 'Grupo',
                    emptyText: '< Seleccione >',
                    store: stGrupo,
                    displayField: 'DGrupo',
                    valueField: 'Grupo',
                    allowBlank: false,
                    forceSelection: true,
                    listeners: {
                        select: {
                            fn: me.oncbxGrupoSelect,
                            scope: me
                        }
                    }
                },
                {
                    xtype: 'combo',
                    itemId: 'cbxPadre',
                    lastQuery: '',
                    fieldLabel: 'Anidar en',
                    emptyText: '< Seleccione >',
                    store: stPadre,
                    displayField: 'Ruta',
                    valueField: 'Menu'
                },
                {
                    xtype: 'textfield',
                    itemId: 'txtNombre',
                    fieldLabel: 'Nombre',
                    allowBlank: false,
                    blankText: 'Este campo es obligatorio.'
                },
                {
                    xtype: 'textfield',
                    itemId: 'txtTooltip',
                    fieldLabel: 'Tooltip'
                },
                {
                    xtype: 'textfield',
                    itemId: 'txtClase',
                    fieldLabel: 'Clase'
                },
                {
                    xtype: 'textfield',
                    itemId: 'txtIcono',
                    fieldLabel: 'Icono'
                },
                {
                    xtype: 'checkboxfield',
                    itemId: 'chkLeaf',
                    hideLabel: true,
                    boxLabel: 'Formulario'
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
                stGrupo.load();
                stMenu.load();
                this.fnEstadoForm('visualizacion');
                Ext.MessageBox.hide();
            }
        }
    },

    oncbxGrupoSelect: function (combo, records, eOpts) {
        stPadre.load({
            params: {
                grupo: parseInt(this.getComponent('pnlRegistro').getComponent('cbxGrupo').getValue())
            }
        });
        this.getComponent('pnlRegistro').getComponent('cbxPadre').clearValue();
    },

    onBtnAgregarClick: function (button, e, options) {
        this.fnLimpiarControles();
        this.getComponent('pnlRegistro').expand();
        this.fnEstadoForm('registro');
    },

    onBtnEditarClick: function (button, e, options) {
        var filas = this.getComponent('grdMenu').getSelectionModel().getSelection();

        if (filas.length > 0) {
            this.getComponent('pnlRegistro').getComponent('txtMenu').setValue(filas[0].get('Menu'));
            this.getComponent('pnlRegistro').getComponent('cbxGrupo').select(filas[0].get('Grupo'), true);
            this.oncbxGrupoSelect(null, null, null);
            this.getComponent('pnlRegistro').getComponent('cbxPadre').setValue(filas[0].get('Padre'));
            this.getComponent('pnlRegistro').getComponent('txtNombre').setValue(filas[0].get('DMenu'));
            this.getComponent('pnlRegistro').getComponent('txtTooltip').setValue(filas[0].get('Tooltip'));
            this.getComponent('pnlRegistro').getComponent('txtClase').setValue(filas[0].get('Clase'));
            this.getComponent('pnlRegistro').getComponent('txtIcono').setValue(filas[0].get('Icono'));
            this.getComponent('pnlRegistro').getComponent('chkLeaf').setValue(filas[0].get('Leaf'));
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
        var filas = me.getComponent('grdMenu').getSelectionModel().getSelection();

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
        stMenu.load({
            params: {
                grupo: 0
            }
        });
    },

    onBtnImprimirClick: function (button, e, options) {
        Ext.ux.grid.Printer.printAutomatically = false;
        Ext.ux.grid.Printer.print(this.getComponent('grdMenu'));
    },

    onBtnGuardarClick: function (button, e, options) {
        if (this.fnEsValidoGuardar()) {
            var codigo = parseInt(this.getComponent('pnlRegistro').getComponent('txtMenu').getValue())
            var dtMenu = {};

            dtMenu['Menu'] = codigo;
            dtMenu['Grupo'] = this.getComponent('pnlRegistro').getComponent('cbxGrupo').getValue();
            dtMenu['Padre'] = this.getComponent('pnlRegistro').getComponent('cbxPadre').getValue();
            dtMenu['Nombre'] = this.getComponent('pnlRegistro').getComponent('txtNombre').getValue();
            dtMenu['Tooltip'] = this.getComponent('pnlRegistro').getComponent('txtTooltip').getValue();
            dtMenu['Clase'] = this.getComponent('pnlRegistro').getComponent('txtClase').getValue();
            dtMenu['Icono'] = this.getComponent('pnlRegistro').getComponent('txtIcono').getValue();
            dtMenu['Leaf'] = this.getComponent('pnlRegistro').getComponent('chkLeaf').getValue();

            Ext.Ajax.request({
                url: "../../Seguridad/Menu/InsUpdMenus",
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
                    datos: '[' + Ext.encode(dtMenu) + ']'
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
            var filas = this.getComponent('grdMenu').getSelectionModel().getSelection();
            Ext.Ajax.request({
                url: "../../Seguridad/Menu/Anular",
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
                    menu: parseInt(filas[0].get('Menu'))
                },
                scope: this
            });
        }
    },

    fnEstadoForm: function (cadena) {
        this.fnBloquearControles(true);
        this.getComponent('pnlRegistro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnGuardar').setDisabled(true);
        this.getComponent('pnlRegistro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnCancelar').setDisabled(true);
        this.getComponent('grdMenu').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnAgregar').setDisabled(true);
        this.getComponent('grdMenu').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnEditar').setDisabled(true);
        this.getComponent('grdMenu').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnAnular').setDisabled(true);
        this.getComponent('grdMenu').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnActualizar').setDisabled(true);
        this.getComponent('grdMenu').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnExportar').setDisabled(true);
        this.getComponent('grdMenu').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnImprimir').setDisabled(true);
        this.getComponent('grdMenu').setDisabled(true);

        switch (cadena) {
            case 'visualizacion':
                this.getComponent('grdMenu').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnAgregar').setDisabled(false);
                this.getComponent('grdMenu').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnEditar').setDisabled(false);
                this.getComponent('grdMenu').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnAnular').setDisabled(false);
                this.getComponent('grdMenu').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnActualizar').setDisabled(false);
                this.getComponent('grdMenu').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnExportar').setDisabled(false);
                this.getComponent('grdMenu').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnImprimir').setDisabled(false);
                this.getComponent('grdMenu').setDisabled(false);
                break;
            case 'edicion':
                this.fnBloquearControles(false);
                this.getComponent('pnlRegistro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnGuardar').setDisabled(false);
                this.getComponent('pnlRegistro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnCancelar').setDisabled(false);
                break;
            case 'registro':
                this.fnBloquearControles(false);
                this.getComponent('pnlRegistro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnGuardar').setDisabled(false);
                this.getComponent('pnlRegistro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnCancelar').setDisabled(false);
                this.getComponent('grdMenu').setDisabled(false);
                break;
            default:
        }
    },

    fnBloquearControles: function (estado) {
        this.getComponent('pnlRegistro').getComponent('cbxGrupo').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('cbxPadre').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('txtNombre').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('txtTooltip').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('txtClase').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('txtIcono').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('chkLeaf').setDisabled(estado);
    },

    fnLimpiarControles: function () {
        this.getComponent('pnlRegistro').getComponent('txtMenu').setValue(0);
        this.getComponent('pnlRegistro').getComponent('cbxGrupo').clearValue();
        this.getComponent('pnlRegistro').getComponent('cbxPadre').clearValue();
        this.getComponent('pnlRegistro').getComponent('txtNombre').setValue('');
        this.getComponent('pnlRegistro').getComponent('txtTooltip').setValue('');
        this.getComponent('pnlRegistro').getComponent('txtIcono').setValue('');
        this.getComponent('pnlRegistro').getComponent('txtClase').setValue('');
    },

    fnEsValidoGuardar: function () {
        if (!this.getComponent('pnlRegistro').getComponent('cbxGrupo').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlRegistro').getComponent('txtNombre').isValid()) {
            return false;
        }
        return true;
    }

});