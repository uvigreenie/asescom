Ext.define('CobApp.Cartera.FrmSectorizarCartera', {
    extend: 'Ext.panel.Panel',
    itemId: 'FrmSectorizarCartera',
    layout: 'border',
    closable: true,
    initComponent: function () {
        var me = this;
        var stCliente = Ext.create('Ext.data.Store', {
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cliente/Listar',
                reader: { type: 'json', root: 'data' }
            }
        });
        var stGestionCliente = Ext.create('Ext.data.Store', {
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/GestionCliente/Listar',
                reader: { type: 'json', root: 'data' }
            }
        });
        var stFechaFin = Ext.create('Ext.data.Store', {
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cartera/ListarFechaFinCartera',
                reader: { type: 'json', root: 'data' }
            }
        });
        var stFechaInicio = Ext.create('Ext.data.Store', {
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cartera/ListarFechaInicioCartera',
                reader: { type: 'json', root: 'data' }
            }
        });
        var stZonal = Ext.create('Ext.data.Store', {
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cartera/ListarZonal',
                reader: { type: 'json', root: 'data' }
            }
        });
        var stCluster = Ext.create('Ext.data.Store', {
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cartera/ListarCluster',
                reader: { type: 'json', root: 'data' }
            }
        });
        var stDepartamento = Ext.create('Ext.data.Store', {
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cartera/ListarDepartamentoxZonal',
                reader: { type: 'json', root: 'data' }
            }
        });
        var stDepartamentoIBK = Ext.create('Ext.data.Store', {
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cartera/ListarDepartamentoIBK',
                reader: { type: 'json', root: 'data' }
            }
        });
        var stProvincia = Ext.create('Ext.data.Store', {
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cartera/ListarProvinciaxDpto',
                reader: { type: 'json', root: 'data' }
            }
        });
        //        var stProvinciaIBK = Ext.create('Ext.data.Store', {
        //            autoLoad: false,
        //            proxy: {
        //                type: 'ajax',
        //                url: '../../Cobranza/Cartera/ListarProvinciaxDptoIBK',
        //                reader: { type: 'json', root: 'data' }
        //            }
        //        });
        var stDistrito = Ext.create('Ext.data.Store', {
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cartera/ListarDistritoxProv',
                reader: { type: 'json', root: 'data' }
            }
        });
        //        var stDistrito = Ext.create('Ext.data.Store', {
        //            autoLoad: false,
        //            proxy: {
        //                type: 'ajax',
        //                url: '../../Cobranza/Cartera/ListarDistritoxProvIBK',
        //                reader: { type: 'json', root: 'data' }
        //            }
        //        });
        var stDirecciones = Ext.create('Ext.data.Store', {
            autoLoad: false,
            //            model: Ext.define('Direcciones', { extend: 'Ext.data.Model' }),
            fields: [
                { name: 'DetalleMoroso', type: 'int' },
            //                { name: 'NumeroDocumento', type: 'string' },
                {name: 'RazonSocial', type: 'string' },
                { name: 'Departamento', type: 'string' },
                { name: 'Provincia', type: 'string' },
                { name: 'Distrito', type: 'string' },
                { name: 'Direccion', type: 'string' },
                { name: 'Sector', type: 'string' }
            ],
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cartera/ListarDirecciones',
                reader: {
                    type: 'json'
                }
            }
        });

        Ext.applyIf(me, {
            items: [{
                itemId: 'pnlSector',
                title: 'Sectorizar Provincia o Distrito',
                bodyStyle: 'padding:15px;',
                region: 'south',
                layout: {
                    type: 'table',
                    columns: 4
                },
                items: [
                    {
                        xtype: 'combo',
                        itemId: 'cbxProvincia',
                        lastQuery: '',
                        fieldLabel: 'Provincia',
                        emptyText: '< Seleccione >',
                        labelWidth: 70,
                        width: 270,
                        margin: 10,
                        store: stProvincia,
                        displayField: 'Provincia',
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
                //                    {
                //                        xtype: 'combo',
                //                        itemId: 'cbxProvinciaIBK',
                //                        lastQuery: '',
                //                        fieldLabel: 'Provincia',
                //                        emptyText: '< Seleccione >',
                //                        labelWidth: 70,
                //                        width: 270,
                //                        margin: 10,
                //                        store: stProvinciaIBK,
                //                        displayField: 'Provincia',
                //                        valueField: 'Provincia',
                //                        allowBlank: false,
                //                        forceSelection: true,
                //                        queryMode: 'local',
                //                        listeners: {
                //                            select: {
                //                                fn: me.oncbxProvinciaIBKSelect,
                //                                scope: me
                //                            }
                //                        }
                //                    },
                    {
                    xtype: 'combo',
                    itemId: 'cbxDistrito',
                    lastQuery: '',
                    labelWidth: 60,
                    width: 270,
                    margin: 10,
                    fieldLabel: 'Distrito',
                    multiSelect: true,
                    emptyText: '< Todos >',
                    store: stDistrito,
                    displayField: 'Distrito',
                    valueField: 'Distrito',
                    allowBlank: true,
                    queryMode: 'local'
                },
                //                    {
                //                        xtype: 'combo',
                //                        itemId: 'cbxDistritoIBK',
                //                        lastQuery: '',
                //                        labelWidth: 60,
                //                        width: 270,
                //                        margin: 10,
                //                        fieldLabel: 'Distrito',
                //                        emptyText: '< Todos >',
                //                        store: stDistritoIBK,
                //                        displayField: 'Distrito',
                //                        valueField: 'Distrito',
                //                        allowBlank: true,
                //                        queryMode: 'local'
                //                    },
                    {
                    xtype: 'textfield',
                    itemId: 'txtSector',
                    labelWidth: 50,
                    allowBlank: false,
                    maxLength: 2,
                    width: 90,
                    margin: 10,
                    enforceMaxLength: true,
                    maskRe: /[0-9]/,
                    blankText: 'Campo obligatorio.',
                    fieldLabel: 'Sector'
                },
                    {
                        xtype: 'button',
                        itemId: 'btnGuardarSector',
                        text: 'Guardar',
                        margin: 10,
                        textAlign: 'right',
                        iconCls: 'icon-save',
                        handler: me.onBtnGuardarSectorClick,
                        scope: me
                    }
                ]
            },
            {
                xtype: 'gridpanel',
                itemId: 'grdDirecciones',
                region: 'center',
                title: 'Lista de Direcciones',
                autoScroll: true,
                store: stDirecciones,
                flex: 1,
                columnLines: true,
                columns: [
                    {
                        xtype: 'rownumberer',
                        resizable: true,
                        width: 60
                    },
                    {
                        xtype: 'numbercolumn',
                        dataIndex: 'DetalleMoroso',
                        hidden: true,
                        hideable: false
                    },
                    {
                        dataIndex: 'RazonSocial',
                        text: 'Moroso',
                        width: 200,
                        filterable: true
                    },
                    {
                        dataIndex: 'Departamento',
                        text: 'Dpto.',
                        width: 150
                    },
                    {
                        dataIndex: 'Provincia',
                        itemId: 'provincias',
                        text: 'Prov.',
                        width: 150,
                        hideable: false
                    },
                    {
                        dataIndex: 'Distrito',
                        text: 'Dist.',
                        width: 150,
                        hideable: false
                    },
                    {
                        dataIndex: 'Direccion',
                        text: 'Dirección',
                        width: 300,
                        hideable: false
                    },
                    {
                        dataIndex: 'Sector',
                        text: 'Sector',
                        width: 70,
                        hideable: false,
                        editor: {
                            xtype: 'textfield',
                            maxLength: 2,
                            enforceMaxLength: true,
                            maskRe: /[0-9]/,
                            allowBlank: true
                        }
                    },
                    {
                        flex: 1,
                        menuDisabled: true,
                        hideable: false
                    }
                ],
                emptyText: 'No se encontraron datos.',
                features: [{
                    ftype: 'filters',
                    //                    autoReload: false,
                    local: true,
                    filters:
                    [
                        { type: 'string', dataIndex: 'RazonSocial' },
                        { type: 'string', dataIndex: 'Departamento' },
                        { type: 'string', dataIndex: 'Provincia' },
                        { type: 'string', dataIndex: 'Distrito' }
                    ]
                }],
                tbar: [
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
                        hidden: true,
                        tooltip: 'Exportar'
                    },
                    {
                        itemId: 'btnSalir',
                        iconCls: 'icon-exit',
                        tooltip: 'Salir',
                        handler: me.onBtnSalirClick,
                        scope: me
                    }],
                plugins: [{
                    ptype: 'rowediting',
                    clicksToEdit: 1,
                    clicksToMoveEditor: 1,
//                    autoCancel: false,
                    pluginId: 'editplugin',
                    listeners: {
                        edit: {
                            fn: me.onDireccionesAfterEdit,
                            scope: me
                        },
                        beforeedit: {
                            fn: me.onDireccionesBeforeEdit,
                            scope: me
                        }
                    }
                }
                ]
            },
            {
                itemId: 'pnlFiltro',
                title: 'Filtros',
                region: 'east',
                border: false,
                autoScroll: true,
                split: true,
                collapsed: true,
                collapsible: true,
                width: 300,
                bodyStyle: 'padding:20px;',
                layout: 'form',
                items: [{
                    xtype: 'combo',
                    itemId: 'cbxCliente',
                    lastQuery: '',
                    fieldLabel: 'Cliente',
                    emptyText: '< Seleccione >',
                    store: stCliente,
                    displayField: 'DCliente',
                    valueField: 'Cliente',
                    allowBlank: false,
                    forceSelection: true,
                    queryMode: 'local',
                    listeners: {
                        select: {
                            fn: me.oncbxClienteSelect,
                            scope: me
                        }
                    }
                },
            {
                xtype: 'combo',
                itemId: 'cbxGestionCliente',
                lastQuery: '',
                fieldLabel: 'Gestion Cliente',
                emptyText: '< Seleccione >',
                store: stGestionCliente,
                displayField: 'DGestionCliente',
                valueField: 'GestionCliente',
                allowBlank: false,
                forceSelection: true,
                queryMode: 'local',
                listeners: {
                    select: {
                        fn: me.oncbxGestionClienteSelect,
                        scope: me
                    }
                }
            },
            {
                xtype: 'combo',
                itemId: 'cbxFechaFin',
                lastQuery: '',
                fieldLabel: 'Fin Cartera',
                emptyText: '< Seleccione >',
                store: stFechaFin,
                displayField: 'DFechaFin',
                valueField: 'FechaFin',
                allowBlank: false,
                forceSelection: true,
                queryMode: 'local',
                listeners: {
                    select: {
                        fn: me.oncbxFechaFinSelect,
                        scope: me
                    }
                }
            },
            {
                xtype: 'combo',
                itemId: 'cbxFechaInicio',
                lastQuery: '',
                fieldLabel: 'Inicio Cartera',
                emptyText: '< Seleccione >',
                store: stFechaInicio,
                displayField: 'DFechaInicio',
                valueField: 'FechaInicio',
                allowBlank: false,
                forceSelection: true,
                queryMode: 'local',
                listeners: {
                    select: {
                        fn: me.oncbxFechaInicioSelect,
                        scope: me
                    }
                }
            },
            {
                xtype: 'combo',
                itemId: 'cbxZonal',
                lastQuery: '',
                fieldLabel: 'Zonal',
                emptyText: '< Seleccione >',
                store: stZonal,
                displayField: 'Zonal',
                valueField: 'Zonal',
                allowBlank: false,
                multiSelect: true,
                forceSelection: true,
                queryMode: 'local',
                listeners: {
                    select: {
                        fn: me.oncbxZonalSelect,
                        scope: me
                    }
                }
            },
            {
                xtype: 'combo',
                itemId: 'cbxDepartamento',
                lastQuery: '',
                fieldLabel: 'Departamento',
                emptyText: '< Todos >',
                store: stDepartamento,
                displayField: 'Departamento',
                valueField: 'Departamento',
                multiSelect: true,
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
                itemId: 'cbxDepartamentoIBK',
                lastQuery: '',
                fieldLabel: 'Departamento',
                emptyText: '< Seleccionar >',
                store: stDepartamentoIBK,
                displayField: 'Departamento',
                valueField: 'Departamento',
                forceSelection: true,
                //                multiSelect: true,
                queryMode: 'local'
            }
            ],
                buttons: [{
                    xtype: 'button',
                    itemId: 'btnBuscar',
                    text: 'Buscar',
                    textAlign: 'right',
                    iconCls: 'icon-save',
                    handler: me.onBtnBuscarClick,
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
                this.getComponent('pnlFiltro').getComponent('cbxCliente').getStore().load();
                this.getComponent('pnlFiltro').getComponent('cbxFechaFin').setVisible(false);
                this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').setVisible(false);
                this.getComponent('pnlFiltro').getComponent('cbxZonal').setVisible(false);
                this.getComponent('pnlFiltro').getComponent('cbxDepartamento').setVisible(false);
                this.getComponent('pnlFiltro').getComponent('cbxDepartamentoIBK').setVisible(false);
                this.getComponent('pnlSector').setDisabled(true);
                Ext.MessageBox.hide();
            }
        }
    },

    onDireccionesBeforeEdit: function (editor, e) {
    },

    onDireccionesAfterEdit: function (editor, e) {
        var detalleMoroso = this.getComponent('grdDirecciones').getSelectionModel().getSelection()[0].get('DetalleMoroso');
        var dtDirecciones = {};
        dtDirecciones["DetalleMoroso"] = detalleMoroso;
        dtDirecciones["Sector"] = e.record.get('Sector');

        Ext.Ajax.request({
            url: "../../Cartera/InsUpdSector",
            success: function (response) {
                var respuesta = Ext.decode(response.responseText);
                var context = editor.context;
                if (respuesta['success'] == "true") {
                    if (detalleMoroso == 0) {
                        Ext.example.msg('Información', 'Se registró con éxito');
                    }
                    else {
                        Ext.example.msg('Información', 'Actualización realizada con éxito');
                    }
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
                datos: '[' + Ext.encode(dtDirecciones) + ']'
            },
            scope: this
        })
    },

    onBtnActualizarClick: function (button, e, options) {
        this.onBtnBuscarClick(null, null, null);
    },

    oncbxClienteSelect: function (combo, records, eOpts) {
        this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getStore().load({
            params: {
                cliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxCliente').getValue())
            }
        });
        this.getComponent('pnlSector').setDisabled(true);
        this.getComponent('pnlSector').setDisabled(true);
        this.getComponent('pnlFiltro').getComponent('cbxZonal').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxDepartamento').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxDepartamentoIBK').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxFechaFin').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxFechaFin').setVisible(false);
        this.getComponent('pnlFiltro').getComponent('cbxZonal').setVisible(false);
        this.getComponent('pnlFiltro').getComponent('cbxDepartamento').setVisible(false);
        this.getComponent('pnlFiltro').getComponent('cbxDepartamentoIBK').setVisible(false);
        this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').setVisible(false);
    },

    oncbxGestionClienteSelect: function (combo, records, eOpts) {
        if (this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 1) {
            this.getComponent('pnlFiltro').getComponent('cbxFechaFin').setVisible(true);
            this.getComponent('pnlFiltro').getComponent('cbxZonal').setVisible(true);
            this.getComponent('pnlFiltro').getComponent('cbxDepartamento').setVisible(true);
            this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').setVisible(false);
            this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getStore().load({
                params: {
                    gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue())
                }
            });
            this.getComponent('pnlFiltro').getComponent('cbxFechaFin').clearValue();
            this.getComponent('pnlFiltro').getComponent('cbxZonal').clearValue();
            this.getComponent('pnlFiltro').getComponent('cbxDepartamento').clearValue();
        } else if (this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 2) {
            this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').setVisible(true);
            this.getComponent('pnlFiltro').getComponent('cbxDepartamentoIBK').setVisible(true);
            this.getComponent('pnlFiltro').getComponent('cbxFechaFin').setVisible(false);
            this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').getStore().load({
                params: {
                    gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue())
                }
            });
            this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').clearValue();
            this.getComponent('pnlFiltro').getComponent('cbxZonal').clearValue();
            this.getComponent('pnlFiltro').getComponent('cbxDepartamento').clearValue();
            this.getComponent('pnlFiltro').getComponent('cbxDepartamentoIBk').clearValue();
        }
    },

    oncbxFechaFinSelect: function (combo, records, eOpts) {
        this.getComponent('pnlFiltro').getComponent('cbxZonal').getStore().load({
            params: {
                gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue()),
                fechaFin: this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getValue()
            }
        });
        this.getComponent('pnlSector').setDisabled(true);
        this.getComponent('pnlFiltro').getComponent('cbxZonal').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxDepartamento').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxDepartamentoIBK').clearValue();
    },

    oncbxFechaInicioSelect: function (combo, records, eOpts) {
        this.getComponent('pnlFiltro').getComponent('cbxDepartamentoIBK').getStore().load({
            params: {
                gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue()),
                fechaInicio: this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').getValue()
            }
        });
        this.getComponent('pnlFiltro').getComponent('cbxZonal').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxDepartamento').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxDepartamentoIBK').clearValue();
    },

    oncbxZonalSelect: function (combo, records, eOpts) {
        var datos = [];
        if (this.getComponent('pnlFiltro').getComponent('cbxZonal').getValue() != null) {
            datos = this.getComponent('pnlFiltro').getComponent('cbxZonal').getValue();
        }
        this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getStore().load({
            params: {
                gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue()),
                fechaFin: this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getValue(),
                zonales: datos
            }
        });
        this.getComponent('pnlSector').setDisabled(true);
    },

    oncbxDepartamentoSelect: function (combo, records, eOpts) {
        this.getComponent('pnlSector').setDisabled(true);
    },

    oncbxProvinciaSelect: function (combo, records, eOpts) {
        this.getComponent('pnlSector').getComponent('cbxDistrito').clearValue();
        var datosZonal = [];
        var datosDepartamento = [];
        if (this.getComponent('pnlFiltro').getComponent('cbxZonal').getValue() != null) {
            datosZonal = this.getComponent('pnlFiltro').getComponent('cbxZonal').getValue();
        }
        if (this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getValue() != null) {
            datosDepartamento = this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getValue();
        }
        this.getComponent('pnlSector').getComponent('cbxDistrito').getStore().load({
            params: {
                gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue()),
                fechaFin: this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getValue(),
                fechaInicio: this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').getValue(),
                zonal: datosZonal,
                departamento: datosDepartamento,
                dpto: this.getComponent('pnlFiltro').getComponent('cbxDepartamentoIBK').getValue(),
                provincia: this.getComponent('pnlSector').getComponent('cbxProvincia').getValue()
            }
        });
    },

    onBtnCancelarClick: function (button, e, options) {
        this.getComponent('pnlFiltro').collapse();
    },

    onBtnGuardarSectorClick: function (button, e, options) {
        if (this.fnEsValidoComprobar()) {
            if (this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 1) {
                if (this.fnEsValidoGuardar()) {
                    var dtZonal = [];
                    var dtDpto = [];
                    var dtDistrito = [];

                    if (this.getComponent('pnlFiltro').getComponent('cbxZonal').getValue() != null) {
                        dtZonal = this.getComponent('pnlFiltro').getComponent('cbxZonal').getValue();
                    }
                    if (this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getValue() != null) {
                        dtDpto = this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getValue();
                    }
                    if (this.getComponent('pnlSector').getComponent('cbxDistrito').getValue() != null) {
                        dtDistrito = this.getComponent('pnlSector').getComponent('cbxDistrito').getValue();
                    }

                    var xmlZonal = "<root>";
                    var xmlDpto = "<root>";
                    var xmlDistrito = "<root>";
                    if (dtZonal != null) {
                        for (var i = 0; i < dtZonal.length; i++) {
                            xmlZonal += "<zonal Zonal = '" + dtZonal[i].toString() + "' />";
                        }
                    }
                    xmlZonal += "</root>";
                    if (dtDpto != null) {
                        for (var i = 0; i < dtDpto.length; i++) {
                            xmlDpto += "<departamento Departamento = '" + dtDpto[i].toString() + "' />";
                        }
                    }
                    xmlDpto += "</root>";
                    if (dtDistrito != null) {
                        for (var i = 0; i < dtDistrito.length; i++) {
                            xmlDistrito += "<distrito Distrito = '" + dtDistrito[i].toString() + "' />";
                        }
                    }
                    xmlDistrito += "</root>";

                    var dtSector = {};
                    dtSector["Cliente"] = this.getComponent('pnlFiltro').getComponent('cbxCliente').getValue();
                    dtSector["GestionCliente"] = this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue();
                    dtSector["FechaFin"] = this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getValue();
                    dtSector["Zonal"] = xmlZonal;
                    dtSector["Departamento"] = xmlDpto;
                    dtSector["Provincia"] = this.getComponent('pnlSector').getComponent('cbxProvincia').getValue();
                    dtSector["Distrito"] = xmlDistrito;
                    dtSector["Sector"] = this.getComponent('pnlSector').getComponent('txtSector').getValue();

                    Ext.Ajax.request({
                        url: "../../Cartera/InsUpdSectores",
                        success: function (response) {
                            var respuesta = Ext.decode(response.responseText);
                            if (respuesta['success'] == "true") {
                                Ext.example.msg('Información', 'Actualización realizada con éxito');
                                this.onBtnBuscarClick(null, null, null);
                                this.getComponent('pnlSector').getComponent('cbxProvincia').clearValue();
                                this.getComponent('pnlSector').getComponent('cbxDistrito').clearValue();
                                this.getComponent('pnlSector').getComponent('txtSector').setValue('');
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
                            datos: '[' + Ext.encode(dtSector) + ']'
                        },
                        scope: this
                    })
                }
            } else if (this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 2) {
                if (this.fnEsValidoGuardarIBK()) {

                    var dtDistrito = [];

                    if (this.getComponent('pnlSector').getComponent('cbxDistrito').getValue() != null) {
                        dtDistrito = this.getComponent('pnlSector').getComponent('cbxDistrito').getValue();
                    }

                    var xmlDistrito = "<root>";
                    if (dtDistrito != null) {
                        for (var i = 0; i < dtDistrito.length; i++) {
                            xmlDistrito += "<distrito Distrito = '" + dtDistrito[i].toString() + "' />";
                        }
                    }
                    xmlDistrito += "</root>";

                    var dtSector = {};
                    dtSector["Cliente"] = this.getComponent('pnlFiltro').getComponent('cbxCliente').getValue();
                    dtSector["GestionCliente"] = this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue();
                    dtSector["FechaInicio"] = this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').getValue();
                    dtSector["Departamento"] = this.getComponent('pnlFiltro').getComponent('cbxDepartamentoIBK').getValue(); ;
                    dtSector["Provincia"] = this.getComponent('pnlSector').getComponent('cbxProvincia').getValue();
                    dtSector["Distrito"] = xmlDistrito;
                    dtSector["Sector"] = this.getComponent('pnlSector').getComponent('txtSector').getValue();

                    Ext.Ajax.request({
                        url: "../../Cartera/InsUpdSectores",
                        success: function (response) {
                            var respuesta = Ext.decode(response.responseText);
                            if (respuesta['success'] == "true") {
                                Ext.example.msg('Información', 'Actualización realizada con éxito');
                                this.onBtnBuscarClick(null, null, null);
                                this.getComponent('pnlSector').getComponent('cbxProvincia').clearValue();
                                this.getComponent('pnlSector').getComponent('cbxDistrito').clearValue();
                                this.getComponent('pnlSector').getComponent('txtSector').setValue('');
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
                            datos: '[' + Ext.encode(dtSector) + ']'
                        },
                        scope: this
                    })
                }
            }
        }
    },

    onBtnBuscarClick: function (button, e, options) {
        if (this.fnEsValidoComprobar()) {
            if (this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 1) {
                if (this.fnEsValidoBuscar()) {
                    var dtZonal = [];
                    var dtDepartamento = [];
                    if (this.getComponent('pnlFiltro').getComponent('cbxZonal').getValue() != null) {
                        dtZonal = this.getComponent('pnlFiltro').getComponent('cbxZonal').getValue();
                    }
                    if (this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getValue() != null) {
                        dtDepartamento = this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getValue();
                    }
                    this.getComponent('grdDirecciones').getStore().load({
                        params: {
                            cliente: this.getComponent('pnlFiltro').getComponent('cbxCliente').getValue().toString(),
                            gestionCliente: this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue().toString(),
                            fechaFin: this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getValue(),
                            zonal: dtZonal,
                            departamento: dtDepartamento
                        }
                    });
                    this.getComponent('pnlSector').setDisabled(false);
                    //        this.getComponent('grdDirecciones').getView().getHeaderCt().child('#provincias').setDisabled(true);
                    this.getComponent('pnlSector').getComponent('cbxProvincia').getStore().load({
                        params: {
                            cliente: this.getComponent('pnlFiltro').getComponent('cbxCliente').getValue().toString(),
                            gestionCliente: this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue().toString(),
                            fechaFin: this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getValue(),
                            zonal: dtZonal,
                            departamento: dtDepartamento
                        }
                    });
                    this.getComponent('grdDirecciones').getView().getHeaderCt().child('#provincias').initialConfig.filter.options = this.getComponent('pnlSector').getComponent('cbxProvincia').getStore().collect('Provincia');
                }
            } else if (this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 2) {
                if (this.fnEsValidoBuscarIBK()) {
                    this.getComponent('grdDirecciones').getStore().load({
                        params: {
                            cliente: this.getComponent('pnlFiltro').getComponent('cbxCliente').getValue().toString(),
                            gestionCliente: this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue().toString(),
                            fechaInicio: this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').getValue(),
                            dpto: this.getComponent('pnlFiltro').getComponent('cbxDepartamentoIBK').getValue()
                        }
                    });
                    this.getComponent('pnlSector').getComponent('cbxProvincia').getStore().load({
                        params: {
                            cliente: this.getComponent('pnlFiltro').getComponent('cbxCliente').getValue().toString(),
                            gestionCliente: this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue().toString(),
                            fechaInicio: this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').getValue(),
                            dpto: this.getComponent('pnlFiltro').getComponent('cbxDepartamentoIBK').getValue()
                        }
                    });
                    this.getComponent('pnlSector').setDisabled(false);
                }
            }
        }
    },

    fnEsValidoComprobar: function () {
        if (!this.getComponent('pnlFiltro').getComponent('cbxCliente').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').isValid()) {
            return false;
        }
        return true;
    },

    fnEsValidoBuscar: function () {
        if (!this.getComponent('pnlFiltro').getComponent('cbxFechaFin').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlFiltro').getComponent('cbxZonal').isValid()) {
            return false;
        }
        return true;
    },

    fnEsValidoBuscarIBK: function () {
        if (!this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlFiltro').getComponent('cbxDepartamentoIBK').isValid()) {
            return false;
        }
        return true;
    },

    fnEsValidoGuardar: function () {
        if (!this.getComponent('pnlFiltro').getComponent('cbxFechaFin').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlFiltro').getComponent('cbxZonal').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlSector').getComponent('cbxProvincia').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlSector').getComponent('txtSector').isValid()) {
            return false;
        }
        return true;
    },

    fnEsValidoGuardarIBK: function () {
        if (!this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlFiltro').getComponent('cbxDepartamentoIBK').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlSector').getComponent('cbxProvincia').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlSector').getComponent('txtSector').isValid()) {
            return false;
        }
        return true;
    }
});