var stGestionMoroso = Ext.create('Ext.data.JsonStore', {
    fields: [
         { name: 'Cluster', type: 'string' },
         { name: 'Tramo', type: 'string' },
         { name: 'CodCartera', type: 'string' },
         { name: 'Cuenta', type: 'string' },
         { name: 'NumeroDocumento', type: 'string' },
         { name: 'RazonSocial', type: 'string' },
         { name: 'Deuda', type: 'float' },
         { name: 'Moroso', type: 'int' },
         { name: 'DetalleCartera', type: 'int' }
     ],
    proxy: {
        type: 'ajax',
        url: '../../Cobranza/GestionCliente/ObtenerGestionMoroso',
        reader: {
            type: 'json'
        },
        params: {
            gestionCliente: 0,
            tramo: '',
            cluster: '',
            departamento: '',
            provincia: '',
            distrito: '',
            tipoEstado: 0
        }
    }
});

var stDetalleMoroso = Ext.create('Ext.data.JsonStore', {
    fields: [
         { name: 'DetalleMoroso', type: 'int' },
         { name: 'Descripcion', type: 'string' },
         { name: 'DTipoDetalle', type: 'string' },
         { name: 'DTipoEstado', type: 'string' }
     ],
    proxy: {
        type: 'ajax',
        url: '../../Cobranza/Cartera/ListarDetalleMoroso',
        reader: {
            type: 'json'
        },
        params: {
            moroso: 0
        }
    }
});

var stServicio = Ext.create('Ext.data.JsonStore', {
    fields: [
         { name: 'IDServicio', type: 'int' },
         { name: 'TipoDocumento', type: 'string' },
         { name: 'NumeroDocumento', type: 'string' },
         { name: 'MontoDeuda', type: 'float' },
         { name: 'FechaEmision', type: 'string' },
         { name: 'FechaVencimiento', type: 'string' }
     ],
    proxy: {
        type: 'ajax',
        url: '../../Cobranza/Cartera/ListarServicio',
        reader: {
            type: 'json'
        },
        params: {
            gestionCliente: 0,
            tramo: '',
            cluster: '',
            departamento: '',
            provincia: '',
            distrito: '',
            tipoEstado: 0
        }
    }
});

var stCliente = Ext.create('Ext.data.Store', {
    autoLoad: false,
    fields: [
         { name: 'Ruc', type: 'string' },
         { name: 'RazonSocial', type: 'string' }
     ],
    proxy: {
        type: 'ajax',
        url: '../../Cobranza/Cliente/ListarClientes',
        reader: {
            type: 'json'
        }
    }
});

var stGestionCliente = Ext.create('Ext.data.Store', {
    autoLoad: false,
    fields: [
         { name: 'GestionCliente', type: 'int' },
         { name: 'Descripcion', type: 'string' }
     ],
    proxy: {
        type: 'ajax',
        url: '../../Cobranza/GestionCliente/ListarGestionCliente',
        reader: {
            type: 'json'
        },
        extraParams: {
            ruc: ''
        }
    }
});

var stTramo = Ext.create('Ext.data.Store', {
    autoLoad: false,
    fields: [
         { name: 'Tramo', type: 'string' },
         { name: 'DTramo', type: 'string' }
     ],
    proxy: {
        type: 'ajax',
        url: '../../Cobranza/Cartera/ListarTramo',
        reader: {
            type: 'json'
        },
        extraParams: {
            gestionCliente: 0
        }
    }
});

var stCluster = Ext.create('Ext.data.Store', {
    autoLoad: false,
    fields: [
         { name: 'Cluster', type: 'string' },
         { name: 'DCluster', type: 'string' }
     ],
    proxy: {
        type: 'ajax',
        url: '../../Cobranza/Cartera/ListarCluster',
        reader: {
            type: 'json'
        },
        extraParams: {
            gestionCliente: 0
        }
    }
});

var stDepartamento = Ext.create('Ext.data.Store', {
    autoLoad: false,
    fields: [
         { name: 'Departamento', type: 'string' },
         { name: 'DDepartamento', type: 'string' }
     ],
    proxy: {
        type: 'ajax',
        url: '../../Cobranza/Cartera/ListarDepartamento',
        reader: {
            type: 'json'
        },
        extraParams: {
            gestionCliente: 0
        }
    }
});

var stProvincia = Ext.create('Ext.data.Store', {
    autoLoad: false,
    fields: [
         { name: 'Provincia', type: 'string' },
         { name: 'DProvincia', type: 'string' }
     ],
    proxy: {
        type: 'ajax',
        url: '../../Cobranza/Cartera/ListarProvincia',
        reader: {
            type: 'json'
        },
        extraParams: {
            departamento: ''
        }
    }
});

var stDistrito = Ext.create('Ext.data.Store', {
    autoLoad: false,
    fields: [
         { name: 'Distrito', type: 'string' },
         { name: 'DDistrito', type: 'string' }
     ],
    proxy: {
        type: 'ajax',
        url: '../../Cobranza/Cartera/ListarDistrito',
        reader: {
            type: 'json'
        },
        extraParams: {
            provincia: ''
        }
    }
});

var stTipoEstado = Ext.create('Ext.data.Store', {
    autoLoad: false,
    fields: [
         { name: 'TipoEstado', type: 'int' },
         { name: 'DTipoEstado', type: 'string' }
     ],
    proxy: {
        type: 'ajax',
        url: '../../Cobranza/Cartera/ListarTipoEstado',
        reader: {
            type: 'json'
        }
    }
});

Ext.define('CobApp.GestionCliente.FrmGestionCliente', {
    extend: 'Ext.panel.Panel',
    itemId: 'gestioncliente',
    id: 'gestioncliente',
    layout: 'border',
    closable: true,
    title: 'Gestión de Clientes',
    border: false,
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    itemId: 'pnlFiltro',
                    title: 'Filtros',
                    region: 'east',
                    border: false,
                    split: true,
                    collapsed: true,
                    collapsible: true,
                    width: 350,
                    bodyStyle: 'padding:20px;',
                    layout: 'form',
                    items: [
                    {
                        xtype: 'combo',
                        itemId: 'cbxCliente',
                        fieldLabel: 'Cliente',
                        emptyText: 'Seleccione Cliente...',
                        store: stCliente,
                        displayField: 'RazonSocial',
                        valueField: 'Ruc',
                        allowBlank: false,
                        forceSelection: true,
                        typeAhead: true,
                        minChars: 1,
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
                        fieldLabel: 'Gestion Cliente',
                        emptyText: 'Seleccione Gestion...',
                        store: stGestionCliente,
                        displayField: 'Descripcion',
                        valueField: 'GestionCliente',
                        allowBlank: false,
                        forceSelection: true,
                        typeAhead: true,
                        lastQuery: '',
                        minChars: 1,
                        listeners: {
                            select: {
                                fn: me.oncbxGestionClienteSelect,
                                scope: me
                            }
                        }
                    },
                    {
                        xtype: 'combo',
                        itemId: 'cbxTramo',
                        lastQuery: '',
                        fieldLabel: 'Tramo',
                        emptyText: '< Seleccione >',
                        store: stTramo,
                        displayField: 'DTramo',
                        valueField: 'Tramo',
                        allowBlank: false,
                        forceSelection: true
                    },
                    {
                        xtype: 'combo',
                        itemId: 'cbxCluster',
                        lastQuery: '',
                        fieldLabel: 'Cluster',
                        emptyText: '< Seleccione >',
                        store: stCluster,
                        displayField: 'DCluster',
                        valueField: 'Cluster',
                        allowBlank: false,
                        forceSelection: true
                    },
                    {
                        xtype: 'combo',
                        itemId: 'cbxDepartamento',
                        lastQuery: '',
                        fieldLabel: 'Departamento',
                        emptyText: '< Seleccione >',
                        store: stDepartamento,
                        displayField: 'DDepartamento',
                        valueField: 'Departamento',
                        allowBlank: false,
                        forceSelection: true,
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
                        lastQuery: '',
                        fieldLabel: 'Provincia',
                        emptyText: '< Seleccione >',
                        store: stProvincia,
                        displayField: 'DProvincia',
                        valueField: 'Provincia',
                        allowBlank: false,
                        forceSelection: true,
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
                        lastQuery: '',
                        fieldLabel: 'Distrito',
                        emptyText: '< Seleccione >',
                        store: stDistrito,
                        displayField: 'DDistrito',
                        valueField: 'Distrito',
                        allowBlank: false,
                        forceSelection: true
                    },
                    {
                        xtype: 'combo',
                        itemId: 'cbxEstado',
                        fieldLabel: 'Estado',
                        emptyText: '< Seleccione >',
                        store: stTipoEstado,
                        displayField: 'DTipoEstado',
                        valueField: 'TipoEstado',
                        allowBlank: false,
                        forceSelection: true
                    }
                    ],
                    buttons: [
                    {
                        xtype: 'button',
                        itemId: 'btnBuscar',
                        text: 'Buscar',
                        textAlign: 'right',
                        iconCls: 'icon-buscar',
                        listeners: {
                            click: {
                                fn: me.onBtnBuscarClick,
                                scope: me
                            }
                        }
                    }
                    ]
                },
                {
                    border: false,
                    header: false,
                    itemId: 'pnlGestion',
                    region: 'center',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                        border: false,
                        header: false,
                        itemId: 'pnlBusqueda',
                        region: 'center',
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        items: [
                            {
                                xtype: 'gridpanel',
                                itemId: 'gpMorosos',
                                store: stGestionMoroso,
                                frameHeader: false,
                                header: true,
                                title: 'Listar Gestion Clientes',
                                selModel: {
                                    singleSelect: true
                                },
                                columns: [
                                    {
                                        xtype: 'rownumberer'
                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        dataIndex: 'Cluster',
                                        text: 'Cluster',
                                        hideable: false
                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        dataIndex: 'Tramo',
                                        text: 'Tramo',
                                        hideable: false
                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        dataIndex: 'CodCartera',
                                        text: 'CodCartera',
                                        hideable: false
                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        dataIndex: 'Cuenta',
                                        text: 'Cuenta',
                                        hideable: false
                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        dataIndex: 'NumeroDocumento',
                                        text: 'NumeroDocumento',
                                        hideable: false
                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        dataIndex: 'RazonSocial',
                                        text: 'Cliente',
                                        hideable: false
                                    },
                                    {
                                        xtype: 'numbercolumn',
                                        dataIndex: 'Deuda',
                                        text: 'Deuda Total',
                                        format: '0.00',
                                        hideable: false
                                    },
                                    {
                                        xtype: 'numbercolumn',
                                        dataIndex: 'Moroso',
                                        text: 'Moroso',
                                        hidden: true,
                                        hideable: false
                                    },
                                    {
                                        xtype: 'numbercolumn',
                                        dataIndex: 'DetalleCartera',
                                        text: 'DetalleCartera',
                                        hidden: true,
                                        hideable: false
                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        flex: 1,
                                        menuDisabled: true,
                                        hideable: false
                                    },
                                ],
                                listeners: {
                                    selectionchange: {
                                        fn: me.onGridPedidosSelectionChange,
                                        scope: me
                                    }
                                },
                                flex: 1
                            },
                            {
                                xtype: 'gridpanel',
                                itemId: 'gpServicio',
                                store: stServicio,
                                frameHeader: false,
                                header: true,
                                title: 'Detalle de Deuda',
                                selModel: {
                                    singleSelect: true
                                },
                                columns: [
                                    {
                                        xtype: 'rownumberer'
                                    },
                                    {
                                        xtype: 'numbercolumn',
                                        dataIndex: 'IDServicio',
                                        text: 'IDServicio',
                                        hidden: true,
                                        hideable: false
                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        dataIndex: 'TipoDocumento',
                                        text: 'Tipo Documento',
                                        hideable: false
                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        dataIndex: 'NumeroDocumento',
                                        text: 'Numero Documento',
                                        hideable: false
                                    },
                                    {
                                        xtype: 'numbercolumn',
                                        dataIndex: 'MontoDeuda',
                                        text: 'Deuda',
                                        format: '0.00',
                                        hideable: false
                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        dataIndex: 'NumeroDocumento',
                                        text: 'NumeroDocumento',
                                        hideable: false
                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        dataIndex: 'RazonSocial',
                                        text: 'Cliente',
                                        hideable: false
                                    },
                                    {
                                        xtype: 'numbercolumn',
                                        dataIndex: 'Deuda',
                                        text: 'Deuda Total',
                                        format: '0.00',
                                        hideable: false
                                    },
                                    {
                                        xtype: 'datecolumn',
                                        dataIndex: 'FechaEmision',
                                        renderer: function(v){
                                            return Ext.util.Format.date(Ext.Date.parse(v, 'MS'), 'd/m/Y');
                                        },
                                        text: 'Fecha Emision',
                                        hideable: false
                                    },
                                    {
                                        xtype: 'datecolumn',
                                        dataIndex: 'FechaVencimiento',
                                        renderer: function(v){
                                            return Ext.util.Format.date(Ext.Date.parse(v, 'MS'), 'd/m/Y');
                                        },
                                        text: 'Fecha Vencimiento',
                                        hideable: false
                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        flex: 1,
                                        menuDisabled: true,
                                        hideable: false
                                    },
                                ],
                                flex: 1
                            },
                            {
                                xtype: 'gridpanel',
                                itemId: 'gpDetalleMoroso',
                                store: stDetalleMoroso,
                                frameHeader: false,
                                header: true,
                                title: 'Listado de Referencias',
                                selModel: {
                                    singleSelect: true
                                },
                                columns: [
                                    {
                                        xtype: 'rownumberer'
                                    },
                                    {
                                        xtype: 'numbercolumn',
                                        dataIndex: 'DetalleMoroso',
                                        text: 'DetalleMoroso',
                                        hidden: true,
                                        hideable: false
                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        dataIndex: 'Descripcion',
                                        text: 'Descripcion',
                                        hideable: false
                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        dataIndex: 'DTipoDetalle',
                                        text: 'Tipo Detalle',
                                        hideable: false
                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        dataIndex: 'DTipoEstado',
                                        text: 'Tipo Estado',
                                        hideable: false
                                    },
                                    {
                                        xtype: 'gridcolumn',
                                        flex: 1,
                                        menuDisabled: true,
                                        hideable: false
                                    },
                                ],
                                flex: 1
                            }
                        ],//1
                        flex: 1
                    },
                    {
                        title: 'Gestion',
                        bodyStyle: 'padding:20px;',
                        layout: {
                            type: 'absolute'
                            }
                    }
                ]}],
            buttons: [
                {
                    xtype: 'button',
                    itemId: 'btnGuardar',
                    text: 'Guardar',
                    textAlign: 'right',
                    disabled: true,
                    iconCls: 'icon-save',
                    listeners: {
                        click: {
                            fn: me.onBtnGuardarClick,
                            scope: me
                        }
                    }

                },
                {
                    xtype: 'button',
                    itemId: 'btnCancelar',
                    text: 'Cancelar',
                    textAlign: 'right',
                    disabled: true,
                    iconCls: 'icon-cancelar',
                    listeners: {
                        click: {
                            fn: me.onBtnCancelarClick,
                            scope: me
                        }
                    }
                },
                {
                    xtype: 'button',
                    itemId: 'btnSalir',
                    text: 'Salir',
                    textAlign: 'right',
                    iconCls: 'icon-exit',
                    listeners: {
                        click: {
                            fn: me.onBtnSalirClick,
                            scope: me
                        }
                    }
                }
            ]
        });
        me.callParent(arguments);
    },
    //*************************FUNCIONES**************************************//

    oncbxClienteSelect: function (combo, records, eOpts) {
        stGestionCliente.load({
            params: {
                ruc: parseInt(this.getComponent('pnlFiltro').getComponent('cbxCliente').getValue())
            }
        });
    },

    oncbxGestionClienteSelect: function (combo, records, eOpts) {
        stTramo.load({
            params: {
                gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue())
            }
        });
        stCluster.load({
            params: {
                gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue())
            }
        });
        stDepartamento.load({
            params: {
                gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue())
            }
        });
    },

    oncbxDepartamentoSelect: function (combo, records, eOpts) {
        stProvincia.load({
            params: {
                departamento: this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getValue().toString()
            }
        });
    },

    oncbxProvinciaSelect: function (combo, records, eOpts) {
        stDistrito.load({
            params: {
                provincia: this.getComponent('pnlFiltro').getComponent('cbxProvincia').getValue().toString()
            }
        });
    },

    onBtnNuevoBuscarClick: function (button, e, options) {
        this.getComponent(0).getComponent(0).getComponent('cbxVendedor').setValue(0);
        stPedido.load({
            params: {
                vendedor: 0,
                fechaIni: new Date(),
                fechaFin: new Date()
            }
        });
        this.getComponent(1).getComponent(0).getComponent('txtCodigo').setValue(0);
        this.getComponent(1).getComponent(0).getComponent('cbxCliente').setValue(0);
        this.getComponent(1).getComponent(0).getComponent('dtpFechaPedido').setValue(new Date());
        this.getComponent(1).getComponent(0).getComponent('cbxTurno').setValue(0);
        stDetallePedido.load({
            params: {
                pedidoVenta: 0
            }
        });
        this.fnEstadoForm('busqueda');
    },

    onBtnBuscarClick: function (button, e, options) {
        stGestionMoroso.load({
            params: {
                gestionCliente: this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue(),
                tramo: this.getComponent('pnlFiltro').getComponent('cbxTramo').getValue(),
                cluster: this.getComponent('pnlFiltro').getComponent('cbxCluster').getValue(),
                departamento: this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getValue(),
                provincia: this.getComponent('pnlFiltro').getComponent('cbxProvincia').getValue(),
                distrito: this.getComponent('pnlFiltro').getComponent('cbxDistrito').getValue(),
                tipoEstado: this.getComponent('pnlFiltro').getComponent('cbxEstado').getValue()
            }
        });
    },

    onGridPedidosSelectionChange: function (tablepanel, selections, options) {
        if (selections.length > 0) {
            stDetalleMoroso.load({
                params: {
                    moroso: parseInt(selections[0].get('Moroso'))
                }
            });
            stServicio.load({
                params: {
                    detalleCartera: parseInt(selections[0].get('DetalleCartera'))
                }
            });
        }
    },

    onBtnNuevoClick: function (button, e, options) {
        this.getComponent(1).getComponent(0).getComponent('txtCodigo').setValue(0);
        this.getComponent(1).getComponent(0).getComponent('cbxCliente').setValue(0);
        this.getComponent(1).getComponent(0).getComponent('dtpFechaPedido').setValue(new Date());
        this.getComponent(1).getComponent(0).getComponent('cbxTurno').setValue(0);
        stDetallePedido.load({
            params: {
                pedidoVenta: 0
            }
        });
        this.fnEstadoForm('registro');
    },

    onBtnEditarClick: function (button, e, options) {
        if (this.getComponent(1).getComponent(0).getComponent('txtCodigo').getValue() != '0') {
            this.fnEstadoForm('registro');
        }
        else {
            Ext.example.msg('Error', 'No ha seleccionado ningun pedido.');
        }
    },

    getRandomMinMax: function (min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }

});