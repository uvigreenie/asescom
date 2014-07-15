Ext.define('SegApp.Trabajador.FrmRegistrarTrabajador', {
    extend: 'Ext.panel.Panel',
    itemId: 'FrmRegistrarTrabajador',
    layout: 'border',
    closable: true,
    initComponent: function () {
        var me = this;

        var stSexo = Ext.create('Ext.data.Store', {
            autoLoad: false,
            fields: [ { name: 'Sexo', type: 'string' }, { name: 'DSexo', type: 'string' } ],
            data: { 'items': [ { 'Sexo': 'M', "DSexo": "Masculino" }, { 'Sexo': 'F', "DSexo": "Femenino" } ] },
            proxy: {
                type: 'memory',
                reader: { type: 'json', root: 'items' }
            }
        });

        var stTipoDocumento = Ext.create('Ext.data.Store', {
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: '../../Seguridad/Trabajador/ListarTipoDocumento',
                reader: { type: 'json', root: 'data' }
            }
        });

        var stDepartamento = Ext.create('Ext.data.Store', {
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: '../../Seguridad/Trabajador/ListarDepartamento',
                reader: { type: 'json', root: 'data' }
            }
        });

        var stProvincia = Ext.create('Ext.data.Store', {
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: '../../Seguridad/Trabajador/ListarProvincia',
                reader: { type: 'json', root: 'data' }
            }
        });

        var stDistrito = Ext.create('Ext.data.Store', {
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: '../../Seguridad/Trabajador/ListarDistrito',
                reader: { type: 'json', root: 'data' }
            }
        });

        var stZonal = Ext.create('Ext.data.Store', {
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: '../../Seguridad/Trabajador/ListarZonal',
                reader: { type: 'json', root: 'data' }
            }
        });

        var stPuesto = Ext.create('Ext.data.Store', {
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: '../../Seguridad/Trabajador/ListarPuesto',
                reader: { type: 'json', root: 'data' }
            }
        });

        var stTrabajador = Ext.create('Ext.data.Store', {
            autoLoad: false,
            model: Ext.define('Trabajador', { extend: 'Ext.data.Model' }),
            proxy: {
                type: 'ajax',
                url: '../../Seguridad/Trabajador/Listar',
                reader: { type: 'json', root: 'data' }
            },
            listeners: {
                'metachange': function (store, meta) {
                    this.getComponent('grdTrabajador').reconfigure(store, meta.columns);
                },
                scope: this
            }
        });

        Ext.applyIf(me, {
            items: [{
                xtype: 'gridpanel',
                itemId: 'grdTrabajador',
                region: 'center',
                title: 'Lista de Trabajadores',
                store: stTrabajador,
                filterable: true,
                columnLines: true,
                features: [{
                    ftype: 'filters',
//                    autoReload: false,
                    local: true,
                    filters: 
                    [
                        { type: 'string', dataIndex: 'ApellidoPaterno' },
                        { type: 'string', dataIndex: 'ApellidoMaterno' },
                        { type: 'list', dataIndex: 'DZonal', options: ['OF. AREQUIPA', 'OF. AYACUCHO', 'OF. CAJAMARCA','OF. CHICLAYO', 'OF. CHIMBOTE', 'OF. HUARAZ', 'OF. IQUITOS', 'OF. PUCALLPA','OF. PIURA', 'OF. TACNA', 'OF. TRUJILLO'] },
                        { type: 'list', dataIndex: 'TipoPuesto', options: ['ADMINISTRATIVO', 'GESTION DE CAMPO', 'GESTION DE CALL','RESPONSABLE DE ZONAL'] },
                        { type: 'list', dataIndex: 'Sexo', options: ['F', 'M'] },
                        { type: 'string', dataIndex: 'NumeroDocumento' },
                        { type: 'string', dataIndex: 'Nombre' },
                        { type: 'string', dataIndex: 'Zonal' }
                    ]
                }],
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
                    itemId: 'btnSalir',
                    iconCls: 'icon-exit',
                    tooltip: 'Salir',
                    handler: me.onBtnSalirClick,
                    scope: me
                }],
                columns: []
            },
            {
                itemId: 'pnlRegistro',
                title: 'Crear / Editar',
                region: 'east',
                border: false,
                autoScroll: true,
                split: true,
                collapsed: true,
                collapsible: true,
                width: 400,
                bodyStyle: 'padding:20px;',
                layout: 'form',
                items: [{
                    xtype: 'textfield',
                    itemId: 'txtTrabajador',
                    value: 0,
                    hidden: true
                },
                {
                    xtype: 'combo',
                    itemId: 'cbxTipoDocumento',
                    width: 300,
                    lastQuery: '',
                    fieldLabel: 'Tipo Documento',
                    emptyText: '< Seleccione >',
                    store: stTipoDocumento,
                    displayField: 'DTipoDocumento',
                    valueField: 'TipoDocumento',
                    allowBlank: false,
                    forceSelection: true,
                    queryMode: 'local'
                },
                {
                    xtype: 'textfield',
                    itemId: 'txtNumeroDocumento',
                    fieldLabel: 'N° Documento',
                    allowBlank: false,
                    blankText: 'Este campo es obligatorio.'
                },
                {
                    xtype: 'textfield',
                    itemId: 'txtApellidoPaterno',
                    fieldLabel: 'Apellido Paterno',
                    allowBlank: false,
                    blankText: 'Este campo es obligatorio.'
                },
                {
                    xtype: 'textfield',
                    itemId: 'txtApellidoMaterno',
                    fieldLabel: 'Apellido Materno',
                    allowBlank: false,
                    blankText: 'Este campo es obligatorio.'
                },
                {
                    xtype: 'textfield',
                    itemId: 'txtNombre',
                    fieldLabel: 'Nombres',
                    allowBlank: false,
                    blankText: 'Este campo es obligatorio.'
                },
                {
                    xtype: 'datefield',
                    itemId: 'dtpFechaNacimiento',
                    format: 'd/m/Y',
                    fieldLabel: 'Fecha Nacimiento',
                    allowBlank: false,
                    value: new Date()
                },
                {
                    xtype: 'combo',
                    itemId: 'cbxSexo',
                    width: 300,
                    lastQuery: '',
                    fieldLabel: 'Sexo',
                    emptyText: '< Seleccione >',
                    store: stSexo,
                    displayField: 'Sexo',
                    valueField: 'Sexo',
                    allowBlank: false,
                    forceSelection: true,
                    queryMode: 'local'
                },
                {
                    xtype: 'textfield',
                    itemId: 'txtTelefonoFijo',
                    fieldLabel: 'Teléfono Fijo'
                },
                {
                    xtype: 'textfield',
                    itemId: 'txtCelular',
                    fieldLabel: 'Celular'
                },
                {
                    xtype: 'textfield',
                    itemId: 'txtCorreo',
                    fieldLabel: 'Correo',
                    vtype: 'email'
                },
                {
                    xtype: 'textfield',
                    itemId: 'txtCorreo',
                    fieldLabel: 'Correo',
                    vtype: 'email'
                },
                {
                    xtype: 'textfield',
                    itemId: 'txtCorreoAlternativo',
                    fieldLabel: 'Correo Alterno',
                    vtype: 'email'
                },
                {
                    xtype: 'textfield',
                    itemId: 'txtDireccion',
                    fieldLabel: 'Dirección'
                },
                {
                    xtype: 'textfield',
                    itemId: 'txtDireccionAlterna',
                    fieldLabel: 'Dirección Alterna'
                },
                {
                    xtype: 'combo',
                    itemId: 'cbxDepartamento',
                    width: 300,
                    lastQuery: '',
                    fieldLabel: 'Departamento',
                    emptyText: '< Seleccione >',
                    store: stDepartamento,
                    displayField: 'DDepartamento',
                    valueField: 'Departamento',
                    allowBlank: false,
                    forceSelection: true,
                    queryMode: 'local',
                    listeners: {
                        select: {
                            fn: me.oncbxDepartamentoSelect,
                            scope: me
                        }
                    }
                },
                {
                    xtype: 'combo',
                    itemId: 'cbxProvincia',
                    width: 300,
                    lastQuery: '',
                    fieldLabel: 'Provincia',
                    emptyText: '< Seleccione >',
                    store: stProvincia,
                    displayField: 'DProvincia',
                    valueField: 'Provincia',
                    allowBlank: false,
                    forceSelection: true,
                    queryMode: 'local',
                    listeners: {
                        select: {
                            fn: me.oncbxProvinciaSelect,
                            scope: me
                        }
                    }
                },
                {
                    xtype: 'combo',
                    itemId: 'cbxDistrito',
                    width: 300,
                    lastQuery: '',
                    fieldLabel: 'Distrito',
                    emptyText: '< Seleccione >',
                    store: stDistrito,
                    displayField: 'DDistrito',
                    valueField: 'Distrito',
                    allowBlank: false,
                    forceSelection: true,
                    queryMode: 'local'
                },
                {
                    xtype: 'combo',
                    itemId: 'cbxZonal',
                    width: 300,
                    lastQuery: '',
                    fieldLabel: 'Zonal',
                    emptyText: '< Seleccione >',
                    store: stZonal,
                    displayField: 'DZonal',
                    valueField: 'Zonal',
                    allowBlank: false,
                    forceSelection: true,
                    queryMode: 'local'
                },
                {
                    xtype: 'combo',
                    itemId: 'cbxPuesto',
                    width: 300,
                    lastQuery: '',
                    fieldLabel: 'Puesto',
                    emptyText: '< Seleccione >',
                    store: stPuesto,
                    displayField: 'DPuesto',
                    valueField: 'Puesto',
                    allowBlank: false,
                    forceSelection: true,
                    queryMode: 'local'
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
                component.getComponent('pnlRegistro').getComponent('cbxTipoDocumento').getStore().load();
                component.getComponent('pnlRegistro').getComponent('cbxSexo').getStore().load();
                component.getComponent('pnlRegistro').getComponent('cbxDepartamento').getStore().load();
                component.getComponent('pnlRegistro').getComponent('cbxZonal').getStore().load();
                component.getComponent('pnlRegistro').getComponent('cbxPuesto').getStore().load();
                component.getComponent('grdTrabajador').getStore().load();
                component.fnEstadoForm('visualizacion');
                Ext.MessageBox.hide();
            }
        }
    },

    oncbxDepartamentoSelect: function (combo, records, eOpts) {
        this.getComponent('pnlRegistro').getComponent('cbxProvincia').getStore().load({
            params: {
                Departamento: this.getComponent('pnlRegistro').getComponent('cbxDepartamento').getValue().toString()
            }
        });
    },

    oncbxProvinciaSelect: function (combo, records, eOpts) {
        this.getComponent('pnlRegistro').getComponent('cbxDistrito').getStore().load({
            params: {
                Departamento: this.getComponent('pnlRegistro').getComponent('cbxDepartamento').getValue().toString(),
                Provincia: this.getComponent('pnlRegistro').getComponent('cbxProvincia').getValue().toString()
            }
        });
    },

    onBtnAgregarClick: function (button, e, options) {
        this.fnLimpiarControles();
        this.getComponent('pnlRegistro').expand();
        this.fnEstadoForm('registro');
    },

    onBtnEditarClick: function (button, e, options) {
        var filas = this.getComponent('grdTrabajador').getSelectionModel().getSelection();

        if (filas.length > 0) {
            this.getComponent('pnlRegistro').getComponent('txtTrabajador').setValue(filas[0].get('Trabajador'));
            this.getComponent('pnlRegistro').getComponent('cbxTipoDocumento').setValue(filas[0].get('TipoDocumento'));
            this.getComponent('pnlRegistro').getComponent('txtNumeroDocumento').setValue(filas[0].get('NumeroDocumento'));
            this.getComponent('pnlRegistro').getComponent('txtApellidoPaterno').setValue(filas[0].get('ApellidoPaterno'));
            this.getComponent('pnlRegistro').getComponent('txtApellidoMaterno').setValue(filas[0].get('ApellidoMaterno'));
            this.getComponent('pnlRegistro').getComponent('txtNombre').setValue(filas[0].get('Nombre'));
            this.getComponent('pnlRegistro').getComponent('dtpFechaNacimiento').setValue(filas[0].get('FechaNacimiento'));
            this.getComponent('pnlRegistro').getComponent('cbxSexo').setValue(filas[0].get('Sexo'));
            this.getComponent('pnlRegistro').getComponent('txtTelefonoFijo').setValue(filas[0].get('TelefonoFijo'));
            this.getComponent('pnlRegistro').getComponent('txtCelular').setValue(filas[0].get('Celular'));
            this.getComponent('pnlRegistro').getComponent('txtCorreo').setValue(filas[0].get('Correo'));
            this.getComponent('pnlRegistro').getComponent('txtCorreoAlternativo').setValue(filas[0].get('CorreoAlternativo'));
            this.getComponent('pnlRegistro').getComponent('txtDireccion').setValue(filas[0].get('Direccion'));
            this.getComponent('pnlRegistro').getComponent('txtDireccionAlterna').setValue(filas[0].get('DireccionAlterna'));
            this.getComponent('pnlRegistro').getComponent('chkActivo').setValue(filas[0].get('Activo'));
            this.getComponent('pnlRegistro').getComponent('cbxDepartamento').setValue(filas[0].get('Departamento'));
            this.getComponent('pnlRegistro').getComponent('cbxProvincia').getStore().load({
                params: {
                    Departamento: filas[0].get('Departamento')
                }
            });
            this.getComponent('pnlRegistro').getComponent('cbxProvincia').setValue(filas[0].get('Provincia'));
            this.getComponent('pnlRegistro').getComponent('cbxDistrito').getStore().load({
                params: {
                    Departamento: filas[0].get('Departamento'),
                    Provincia: filas[0].get('Provincia')
                }
            });
            this.getComponent('pnlRegistro').getComponent('cbxDistrito').setValue(filas[0].get('Distrito'));
            this.getComponent('pnlRegistro').getComponent('cbxZonal').setValue(filas[0].get('Zonal'));
            this.getComponent('pnlRegistro').getComponent('cbxPuesto').setValue(filas[0].get('Puesto'));
            this.getComponent('pnlRegistro').expand();
            this.fnEstadoForm('edicion');
        }
        else {
            Ext.MessageBox.show({
                title: 'Sistema RJ Abogados',
                msg: 'No ha seleccionado ningun registro!',
                buttons: Ext.MessageBox.OK,
                animateTarget: button,
                icon: Ext.MessageBox.WARNING
            });
        }
    },

    onBtnAnularClick: function (button, e, options) {
        var me = this;
        var filas = me.getComponent('grdTrabajador').getSelectionModel().getSelection();

        if (filas.length > 0) {
            Ext.MessageBox.show({
                title: 'Sistema RJ Abogados',
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
                title: 'Sistema RJ Abogados',
                msg: '¡No ha seleccionado ningun registro!',
                buttons: Ext.MessageBox.OK,
                animateTarget: button,
                icon: Ext.MessageBox.WARNING
            });
        }
    },

    onBtnActualizarClick: function (button, e, options) {
        this.getComponent('grdTrabajador').getStore().load();
    },

    onBtnGuardarClick: function (button, e, options) {
        if ( this.fnEsValidoGuardar() ) {
            var codigo = parseInt(this.getComponent('pnlRegistro').getComponent('txtTrabajador').getValue())
            var dtTrabajador = {};

            dtTrabajador['Trabajador'] = codigo;
            dtTrabajador['TipoDocumento'] = this.getComponent('pnlRegistro').getComponent('cbxTipoDocumento').getValue();
            dtTrabajador['NumeroDocumento'] = this.getComponent('pnlRegistro').getComponent('txtNumeroDocumento').getValue();
            dtTrabajador['ApellidoPaterno'] = this.getComponent('pnlRegistro').getComponent('txtApellidoPaterno').getValue();
            dtTrabajador['ApellidoMaterno'] = this.getComponent('pnlRegistro').getComponent('txtApellidoMaterno').getValue();
            dtTrabajador['Nombre'] = this.getComponent('pnlRegistro').getComponent('txtNombre').getValue();
            dtTrabajador['FechaNacimiento'] = this.getComponent('pnlRegistro').getComponent('dtpFechaNacimiento').getValue();
            dtTrabajador['Sexo'] = this.getComponent('pnlRegistro').getComponent('cbxSexo').getValue();
            dtTrabajador['TelefonoFijo'] = this.getComponent('pnlRegistro').getComponent('txtTelefonoFijo').getValue();
            dtTrabajador['Celular'] = this.getComponent('pnlRegistro').getComponent('txtCelular').getValue();
            dtTrabajador['Correo'] = this.getComponent('pnlRegistro').getComponent('txtCorreo').getValue();
            dtTrabajador['CorreoAlternativo'] = this.getComponent('pnlRegistro').getComponent('txtCorreoAlternativo').getValue();
            dtTrabajador['Direccion'] = this.getComponent('pnlRegistro').getComponent('txtDireccion').getValue();
            dtTrabajador['DireccionAlterna'] = this.getComponent('pnlRegistro').getComponent('txtDireccionAlterna').getValue();
            dtTrabajador['Distrito'] = this.getComponent('pnlRegistro').getComponent('cbxDistrito').getValue();
            dtTrabajador['Zonal'] = this.getComponent('pnlRegistro').getComponent('cbxZonal').getValue();
            dtTrabajador['Puesto'] = this.getComponent('pnlRegistro').getComponent('cbxPuesto').getValue();
            dtTrabajador['Activo'] = this.getComponent('pnlRegistro').getComponent('chkActivo').getValue();

            Ext.Ajax.request({
                url: "../../Seguridad/Trabajador/InsUpdTrabajador",
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
                    datos: '[' + Ext.encode(dtTrabajador) + ']'
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
            var filas = this.getComponent('grdTrabajador').getSelectionModel().getSelection();
            Ext.Ajax.request({
                url: "../../Seguridad/Trabajador/Anular",
                success: function (response) {
                    this.setLoading(false);
                    var respuesta = Ext.decode(response.responseText);
                    if (respuesta['success'] == "true") {
                        Ext.example.msg('Información', 'Se anulo el registro con exitó');
                        this.onBtnActualizarClick(null, null, null);
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
                    this.setLoading(false);
                    Ext.MessageBox.show({
                        title: 'Sistema RJ Abogados',
                        msg: 'Se Produjo un error en la conexión.',
                        buttons: Ext.MessageBox.OK,
                        animateTarget: button,
                        icon: Ext.Msg.ERROR
                    });
                },
                params: {
                    trabajador: parseInt(filas[0].get('Trabajador'))
                },
                scope: this
            });
        }
    },

    fnEstadoForm: function (cadena) {
        this.fnBloquearControles(true);
        this.getComponent('pnlRegistro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnGuardar').setDisabled(true);
        this.getComponent('pnlRegistro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnCancelar').setDisabled(true);
        this.getComponent('grdTrabajador').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnAgregar').setDisabled(true);
        this.getComponent('grdTrabajador').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnEditar').setDisabled(true);
        this.getComponent('grdTrabajador').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnAnular').setDisabled(true);
        this.getComponent('grdTrabajador').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnActualizar').setDisabled(true);
        this.getComponent('grdTrabajador').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnExportar').setDisabled(true);
        this.getComponent('grdTrabajador').setDisabled(true);

        switch (cadena) {
            case 'visualizacion':
                this.getComponent('grdTrabajador').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnAgregar').setDisabled(false);
                this.getComponent('grdTrabajador').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnEditar').setDisabled(false);
                this.getComponent('grdTrabajador').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnAnular').setDisabled(false);
                this.getComponent('grdTrabajador').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnActualizar').setDisabled(false);
                this.getComponent('grdTrabajador').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnExportar').setDisabled(false);
                this.getComponent('grdTrabajador').setDisabled(false);
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
                this.getComponent('grdTrabajador').setDisabled(false);
                break;
            default:
        }
    },

    fnBloquearControles: function (estado) {
        this.getComponent('pnlRegistro').getComponent('cbxTipoDocumento').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('txtNumeroDocumento').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('txtApellidoPaterno').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('txtApellidoMaterno').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('txtNombre').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('dtpFechaNacimiento').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('cbxSexo').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('txtTelefonoFijo').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('txtCelular').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('txtCorreo').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('txtCorreoAlternativo').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('txtDireccion').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('txtDireccionAlterna').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('cbxDepartamento').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('cbxProvincia').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('cbxDistrito').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('cbxZonal').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('cbxPuesto').setDisabled(estado);
        this.getComponent('pnlRegistro').getComponent('chkActivo').setDisabled(estado);
    },

    fnLimpiarControles: function () {
        this.getComponent('pnlRegistro').getComponent('txtTrabajador').setValue(0);
        this.getComponent('pnlRegistro').getComponent('cbxTipoDocumento').clearValue();
        this.getComponent('pnlRegistro').getComponent('txtNumeroDocumento').setValue('');
        this.getComponent('pnlRegistro').getComponent('txtApellidoPaterno').setValue('');
        this.getComponent('pnlRegistro').getComponent('txtApellidoMaterno').setValue('');
        this.getComponent('pnlRegistro').getComponent('txtNombre').setValue('');
        this.getComponent('pnlRegistro').getComponent('cbxSexo').clearValue();
        this.getComponent('pnlRegistro').getComponent('txtTelefonoFijo').setValue('');
        this.getComponent('pnlRegistro').getComponent('txtCelular').setValue('');
        this.getComponent('pnlRegistro').getComponent('txtCorreo').setValue('');
        this.getComponent('pnlRegistro').getComponent('txtCorreoAlternativo').setValue('');
        this.getComponent('pnlRegistro').getComponent('txtDireccion').setValue('');
        this.getComponent('pnlRegistro').getComponent('txtDireccionAlterna').setValue('');
        this.getComponent('pnlRegistro').getComponent('cbxDepartamento').clearValue();
        this.getComponent('pnlRegistro').getComponent('cbxProvincia').clearValue();
        this.getComponent('pnlRegistro').getComponent('cbxDistrito').clearValue();
        this.getComponent('pnlRegistro').getComponent('cbxZonal').clearValue();
        this.getComponent('pnlRegistro').getComponent('cbxPuesto').clearValue();
    },

    fnEsValidoGuardar: function () {
        if (!this.getComponent('pnlRegistro').getComponent('cbxTipoDocumento').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlRegistro').getComponent('txtNumeroDocumento').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlRegistro').getComponent('cbxSexo').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlRegistro').getComponent('txtApellidoPaterno').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlRegistro').getComponent('txtApellidoMaterno').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlRegistro').getComponent('txtNombre').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlRegistro').getComponent('dtpFechaNacimiento').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlRegistro').getComponent('cbxDepartamento').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlRegistro').getComponent('cbxProvincia').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlRegistro').getComponent('cbxDistrito').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlRegistro').getComponent('cbxZonal').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlRegistro').getComponent('cbxPuesto').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlRegistro').getComponent('dtpFechaNacimiento').isValid()) {
            return false;
        }
        return true;
    }
});