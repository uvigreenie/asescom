Ext.define('CobApp.Pagos.FrmListarPagos', {
    extend: 'Ext.panel.Panel',
    itemId: 'FrmListarPagos',
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
        var stListadoPagos = Ext.create('Ext.data.Store', {
            autoLoad: false,
            model: Ext.define('ListadoPagos', { extend: 'Ext.data.Model' }),
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cartera/ListarPagos',
                reader: { type: 'json', root: 'data' }
            },
            listeners: {
                'metachange': function (store, meta) {
                    this.getComponent('grdListadoPagos').reconfigure(store, meta.columns);
                },
                scope: this
            }
        });

        Ext.applyIf(me, {
            items:
            [{
                xtype: 'gridpanel',
                itemId: 'grdListadoPagos',
                region: 'center',
                title: 'Lista de Pagos',
                store: stListadoPagos,
                filterable: true,
                columnLines: true,
                columns: [],
                emptyText: 'No se encontraron datos.',
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
                    itemId: 'chkAcumulado',
                    fieldLabel: 'Acumulado'
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
                    href: '../../Cobranza/Cartera/ExportarPagos'
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
                this.getComponent('pnlFiltro').getComponent('dtpFechaFin').setDisabled(true);
                this.getComponent('pnlFiltro').getComponent('cbxCliente').getStore().load();
                this.getComponent('pnlFiltro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnExportar').setDisabled(true);
                Ext.MessageBox.hide();
            }
        }
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
    },
    oncbxGestionClienteSelect: function (combo, records, eOpts) {
        this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getStore().load({
            params: {
                gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue())
            }
        });
    },
    oncbxFechaFinSelect: function (combo, records, eOpts) {
        this.getComponent('pnlFiltro').getComponent('cbxZonal').getStore().load({
            params: {
                gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue()),
                fechaFin: this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getValue()
            }
        });
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
    },
    onBtnCancelarClick: function (button, e, options) {
        this.getComponent('pnlFiltro').collapse();
    },
    onBtnBuscarClick: function (button, e, options) {
        if (this.fnEsValidoBuscar()) {
            var dtZonal = [];
            var dtDepartamento = [];
            if (this.getComponent('pnlFiltro').getComponent('cbxZonal').getValue() != null) {
                dtZonal = this.getComponent('pnlFiltro').getComponent('cbxZonal').getValue();
            }

            if (this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getValue() != null) {
                dtDepartamento = this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getValue();
            }
            this.getComponent('grdListadoPagos').getStore().load({
                params: {
                    cliente: this.getComponent('pnlFiltro').getComponent('cbxCliente').getValue().toString(),
                    gestionCliente: this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue().toString(),
                    fechaFin: this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getValue(),
                    zonal: dtZonal,
                    departamento: dtDepartamento,
                    fechaDesde: this.getComponent('pnlFiltro').getComponent('dtpFechaInicio').getValue(),
                    fechaHasta: this.getComponent('pnlFiltro').getComponent('dtpFechaFin').getValue(),
                    acumulado: this.getComponent('pnlFiltro').getComponent('chkAcumulado').getValue()
                }
            });
            this.getComponent('pnlFiltro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnExportar').setParams({
                cliente: this.getComponent('pnlFiltro').getComponent('cbxCliente').getValue().toString(),
                gestionCliente: this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue().toString(),
                fechaFin: this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getValue(),
                zonal: dtZonal,
                departamento: dtDepartamento,
                fechaDesde: this.getComponent('pnlFiltro').getComponent('dtpFechaInicio').getValue(),
                fechaHasta: this.getComponent('pnlFiltro').getComponent('dtpFechaFin').getValue(),
                acumulado: this.getComponent('pnlFiltro').getComponent('chkAcumulado').getValue()
            });
            this.getComponent('pnlFiltro').getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnExportar').setDisabled(false);
        }
    },
    fnEsValidoBuscar: function () {
        if (!this.getComponent('pnlFiltro').getComponent('cbxCliente').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').isValid()) {
            return false;
        }
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
});