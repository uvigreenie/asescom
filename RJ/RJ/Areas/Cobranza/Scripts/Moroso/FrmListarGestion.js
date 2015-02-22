Ext.define('CobApp.Moroso.FrmListarGestion', {
    extend: 'Ext.panel.Panel',
    itemId: 'FrmListarGestion',
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
        var stListadoGestion = Ext.create('Ext.data.Store', {
            autoLoad: false,
            model: Ext.define('ListadoGestion', { extend: 'Ext.data.Model' }),
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cartera/ListarGestiones',
                reader: { type: 'json', root: 'data' }
            },
            listeners: {
                'metachange': function (store, meta) {
                    this.getComponent('grdListadoGestion').reconfigure(store, meta.columns);
                },
                scope: this
            }
        });

        Ext.applyIf(me, {
            items:
            [{
                xtype: 'gridpanel',
                itemId: 'grdListadoGestion',
                region: 'center',
                title: 'Lista de Gestiones',
                store: stListadoGestion,
                filterable: true,
                columnLines: true,
                columns: [],
                plugins: {
                    ptype: 'bufferedrenderer',
                    trailingBufferZone: 20,
                    leadingBufferZone: 30
                },
                emptyText: 'No se encontraron datos.',
                tbar: [
                    {
                        xtype: 'label',
                        name: 'message',
                        text: 'Sólo se muestran las primeras 3500 filas. Para ver la totalidad de filas y columnas, exportar.',
                        colspan: 2
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
                columns: []
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
                    queryMode: 'local'
                },
                {
                    xtype: 'datefield',
                    itemId: 'dtpFechaInicio',
                    format: 'd/m/Y',
                    fieldLabel: 'Desde',
                    maxValue: new Date(),
                    value: new Date(),
                    allowBlank: false,
                    listeners: {
                        select: {
                            fn: me.ondtpFechaInicioSelect,
                            scope: me
                        }
                    }
                },
                {
                    xtype: 'datefield',
                    itemId: 'dtpFechaFin',
                    format: 'd/m/Y',
                    fieldLabel: 'Hasta',
                    maxValue: new Date(),
                    value: new Date(),
                    allowBlank: false,
                    listeners: {
                        select: {
                            fn: me.ondtpFechaFinSelect,
                            scope: me
                        }
                    }
                },
                {
                    xtype: 'checkbox',
                    itemId: 'chkMejorGestion',
                    fieldLabel: 'Mejor gestión'
                },
                {
                    xtype: 'checkbox',
                    itemId: 'chkPromesas',
                    fieldLabel: 'Sólo Promesas',
                    listeners: {
                        change: {
                            fn: me.onchkPromesasChange,
                            scope: me
                        }
                    }
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
                    itemId: 'btnExportar',
                    text: 'Exportar',
                    textAlign: 'right',
                    iconCls: 'icon-export',
                    href: '../../Cobranza/Cartera/ExportarGestion'
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
                //                component.fnEstadoForm('visualizacion');
                this.getComponent('pnlFiltro').getComponent('dtpFechaFin').setDisabled(true);
                this.getComponent('pnlFiltro').getComponent('cbxCliente').getStore().load();
                this.getComponent('pnlFiltro').getComponent('cbxFechaFin').setVisible(false);
                this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').setVisible(false);
                this.getComponent('pnlFiltro').getComponent('cbxZonal').setVisible(false);
                this.getComponent('pnlFiltro').getComponent('cbxDepartamento').setVisible(false);
                this.getComponent('pnlFiltro').getComponent('chkMejorGestion').setVisible(false);
                this.getComponent('pnlFiltro').getComponent('chkPromesas').setVisible(false);
                this.getComponent('pnlFiltro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnExportar').setDisabled(true);
                //                component.getComponent('grdListadoGestion').getStore().load();
                Ext.MessageBox.hide();
            }
        }
    },

    onBtnActualizarClick: function (button, e, options) {
        this.onBtnBuscarClick(null, null, null);
    },

    onchkPromesasChange: function (cb, checked) {
        //        this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').setDisabled(!checked);
        //        this.getComponent('pnlFiltro').getComponent('cbxFechaFin').setDisabled(checked);
        //        this.getComponent('pnlFiltro').getComponent('cbxZonal').setDisabled(checked);
        //        this.getComponent('pnlFiltro').getComponent('cbxDepartamento').setDisabled(checked);
        //        this.getComponent('pnlFiltro').getComponent('cbxTramo').setDisabled(checked);
        //        this.getComponent('pnlFiltro').getComponent('cbxCluster').setDisabled(checked);
        this.getComponent('pnlFiltro').getComponent('chkMejorGestion').setDisabled(checked);
        this.getComponent('pnlFiltro').getComponent('chkMejorGestion').setValue(false);
        //        this.getComponent('pnlFiltro').getComponent('cbxBuscarPor').setDisabled(!checked);
    },

    oncbxClienteSelect: function (combo, records, eOpts) {
        this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getStore().load({
            params: {
                cliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxCliente').getValue())
            }
        });
        this.getComponent('pnlFiltro').getComponent('cbxFechaFin').setVisible(false);
        this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').setVisible(false);
        this.getComponent('pnlFiltro').getComponent('cbxZonal').setVisible(false);
        this.getComponent('pnlFiltro').getComponent('cbxDepartamento').setVisible(false);
        this.getComponent('pnlFiltro').getComponent('cbxFechaFin').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxZonal').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxDepartamento').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').clearValue();
        this.getComponent('pnlFiltro').getComponent('chkMejorGestion').setVisible(false);
        this.getComponent('pnlFiltro').getComponent('chkPromesas').setVisible(false);
    },

    oncbxGestionClienteSelect: function (combo, records, eOpts) {
        if (this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 1 || this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 4 || this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 6) {
            this.getComponent('pnlFiltro').getComponent('cbxFechaFin').setVisible(true);
            this.getComponent('pnlFiltro').getComponent('cbxZonal').setVisible(true);
            this.getComponent('pnlFiltro').getComponent('cbxDepartamento').setVisible(true);
            this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').setVisible(false);
            this.getComponent('pnlFiltro').getComponent('chkMejorGestion').setVisible(true);
            this.getComponent('pnlFiltro').getComponent('chkPromesas').setVisible(false);
            this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getStore().load({
                params: {
                    gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue())
                }
            });
            this.getComponent('pnlFiltro').getComponent('cbxFechaFin').clearValue();
            this.getComponent('pnlFiltro').getComponent('cbxZonal').clearValue();
            this.getComponent('pnlFiltro').getComponent('cbxDepartamento').clearValue();
        } else if (this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 2 || this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 5) {
            this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').setVisible(true);
            this.getComponent('pnlFiltro').getComponent('cbxDepartamento').setVisible(true);
            this.getComponent('pnlFiltro').getComponent('cbxFechaFin').setVisible(false);
            this.getComponent('pnlFiltro').getComponent('cbxZonal').setVisible(false);
            this.getComponent('pnlFiltro').getComponent('chkMejorGestion').setVisible(true);
            this.getComponent('pnlFiltro').getComponent('chkPromesas').setVisible(true);
            this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').getStore().load({
                params: {
                    gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue())
                }
            });
            this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').clearValue();
            this.getComponent('pnlFiltro').getComponent('cbxZonal').clearValue();
            this.getComponent('pnlFiltro').getComponent('cbxDepartamento').clearValue();
        } else if (this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 7 || this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 8) {
            this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').setVisible(false);
            this.getComponent('pnlFiltro').getComponent('cbxDepartamento').setVisible(false);
            this.getComponent('pnlFiltro').getComponent('cbxFechaFin').setVisible(false);
            this.getComponent('pnlFiltro').getComponent('cbxZonal').setVisible(false);
            this.getComponent('pnlFiltro').getComponent('chkMejorGestion').setVisible(true);
            this.getComponent('pnlFiltro').getComponent('chkPromesas').setVisible(true);
            this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').clearValue();
            this.getComponent('pnlFiltro').getComponent('cbxZonal').clearValue();
            this.getComponent('pnlFiltro').getComponent('cbxDepartamento').clearValue();
            this.getComponent('pnlFiltro').getComponent('cbxFechaFin').clearValue();
        }
    },

    oncbxFechaFinSelect: function (combo, records, eOpts) {
        this.getComponent('pnlFiltro').getComponent('cbxZonal').getStore().load({
            params: {
                gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue()),
                fechaFin: this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getValue()
            }
        });
        this.getComponent('pnlFiltro').getComponent('cbxZonal').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxDepartamento').clearValue();
    },

    oncbxFechaInicioSelect: function (combo, records, eOpts) {
        this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getStore().load({
            params: {
                gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue()),
                fechaInicio: this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').getValue()
            }
        });
        this.getComponent('pnlFiltro').getComponent('cbxZonal').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxDepartamento').clearValue();
    },

    ondtpFechaInicioSelect: function (field, value, eOpts) {
        this.getComponent('pnlFiltro').getComponent('dtpFechaFin').setDisabled(false);
        this.getComponent('pnlFiltro').getComponent('dtpFechaFin').setMinValue(this.getComponent('pnlFiltro').getComponent('dtpFechaInicio').getValue())
    },

    ondtpFechaFinSelect: function (field, value, eOpts) {
        this.getComponent('pnlFiltro').getComponent('dtpFechaInicio').setMaxValue(this.getComponent('pnlFiltro').getComponent('dtpFechaFin').getValue())
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
        this.getComponent('pnlFiltro').getComponent('cbxDepartamento').clearValue();
    },

    onBtnCancelarClick: function (button, e, options) {
        this.getComponent('pnlFiltro').collapse();
    },

    onBtnBuscarClick: function (button, e, options) {
        if (this.fnEsValidoComprobar()) {
            if (this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 1 || this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 4 || this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 6) {
                if (this.fnEsValidoBuscar()) {
                    var dtZonal = [];
                    var dtDepartamento = [];
                    if (this.getComponent('pnlFiltro').getComponent('cbxZonal').getValue() != null) {
                        dtZonal = this.getComponent('pnlFiltro').getComponent('cbxZonal').getValue();
                    }

                    if (this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getValue() != null) {
                        dtDepartamento = this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getValue();
                    }
                    if (this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 1) {
                        this.getComponent('grdListadoGestion').getStore().load({
                            params: {
                                cliente: this.getComponent('pnlFiltro').getComponent('cbxCliente').getValue().toString(),
                                gestionCliente: this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue().toString(),
                                fechaFin: this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getValue(),
                                zonal: dtZonal,
                                departamento: dtDepartamento,
                                fechaDesde: this.getComponent('pnlFiltro').getComponent('dtpFechaInicio').getValue(),
                                fechaHasta: this.getComponent('pnlFiltro').getComponent('dtpFechaFin').getValue(),
                                mejorGestion: this.getComponent('pnlFiltro').getComponent('chkMejorGestion').getValue(),
                                promesa: this.getComponent('pnlFiltro').getComponent('chkPromesas').getValue()
                            }
                        });
                    } else { Ext.example.msg('Información', 'Haga click en exportar para descargar el listado de gestiones.'); }

                    this.getComponent('pnlFiltro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnExportar').setParams({
                        cliente: this.getComponent('pnlFiltro').getComponent('cbxCliente').getValue().toString(),
                        gestionCliente: this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue().toString(),
                        fechaFin: this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getValue(),
                        zonal: dtZonal,
                        departamento: dtDepartamento,
                        fechaDesde: this.getComponent('pnlFiltro').getComponent('dtpFechaInicio').getValue(),
                        fechaHasta: this.getComponent('pnlFiltro').getComponent('dtpFechaFin').getValue(),
                        mejorGestion: this.getComponent('pnlFiltro').getComponent('chkMejorGestion').getValue(),
                        promesa: this.getComponent('pnlFiltro').getComponent('chkPromesas').getValue()
                    });
                    this.getComponent('pnlFiltro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnExportar').setDisabled(false);
                }
            } else if (this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 2 || this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 5) {
                if (this.fnEsValidoBuscarIBK()) {
                    var dtDepartamento = [];
                    if (this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getValue() != null) {
                        dtDepartamento = this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getValue();
                    }
//                    this.getComponent('grdListadoGestion').getStore().load({
//                        params: {
//                            cliente: this.getComponent('pnlFiltro').getComponent('cbxCliente').getValue().toString(),
//                            gestionCliente: this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue().toString(),
//                            fechaInicio: this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').getValue(),
//                            departamento: dtDepartamento,
//                            fechaDesde: this.getComponent('pnlFiltro').getComponent('dtpFechaInicio').getValue(),
//                            fechaHasta: this.getComponent('pnlFiltro').getComponent('dtpFechaFin').getValue(),
//                            mejorGestion: this.getComponent('pnlFiltro').getComponent('chkMejorGestion').getValue(),
//                            promesa: this.getComponent('pnlFiltro').getComponent('chkPromesas').getValue()
//                        }
                    //                    });
                    Ext.example.msg('Información', 'Haga click en Exportar para descargar el listado de gestiones.');
                    this.getComponent('pnlFiltro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnExportar').setParams({
                        cliente: this.getComponent('pnlFiltro').getComponent('cbxCliente').getValue().toString(),
                        gestionCliente: this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue().toString(),
                        fechaInicio: this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').getValue(),
                        departamento: dtDepartamento,
                        fechaDesde: this.getComponent('pnlFiltro').getComponent('dtpFechaInicio').getValue(),
                        fechaHasta: this.getComponent('pnlFiltro').getComponent('dtpFechaFin').getValue(),
                        mejorGestion: this.getComponent('pnlFiltro').getComponent('chkMejorGestion').getValue(),
                        promesa: this.getComponent('pnlFiltro').getComponent('chkPromesas').getValue()
                    });
                    this.getComponent('pnlFiltro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnExportar').setDisabled(false);
                }
            } else if (this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 7 || this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 8)
            {
                Ext.example.msg('Información', 'Haga click en Exportar para descargar el listado de gestiones.');
                this.getComponent('pnlFiltro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnExportar').setParams({
                    cliente: this.getComponent('pnlFiltro').getComponent('cbxCliente').getValue().toString(),
                    gestionCliente: this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue().toString(),
                    fechaInicio: this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').getValue(),
//                    departamento: dtDepartamento,
                    fechaDesde: this.getComponent('pnlFiltro').getComponent('dtpFechaInicio').getValue(),
                    fechaHasta: this.getComponent('pnlFiltro').getComponent('dtpFechaFin').getValue(),
                    mejorGestion: this.getComponent('pnlFiltro').getComponent('chkMejorGestion').getValue(),
                    promesa: this.getComponent('pnlFiltro').getComponent('chkPromesas').getValue()
                });
                this.getComponent('pnlFiltro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnExportar').setDisabled(false);
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
        if (!this.getComponent('pnlFiltro').getComponent('dtpFechaInicio').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlFiltro').getComponent('dtpFechaFin').isValid()) {
            return false;
        }
        return true;
    },
    fnEsValidoBuscarIBK: function () {
        if (!this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlFiltro').getComponent('dtpFechaInicio').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlFiltro').getComponent('dtpFechaFin').isValid()) {
            return false;
        }
        return true;
    }
});