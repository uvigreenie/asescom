﻿Ext.define('CobApp.Moroso.FrmGestionMoroso', {
    extend: 'Ext.panel.Panel',
    closable: true,
    layout: 'border',
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

        var stTrabajadorUsuario = Ext.create('Ext.data.Store', {
            autoLoad: false,
            fields: [
                { name: 'Trabajador', type: 'int' },
                { name: 'DTrabajador', type: 'string' }
            ],
            proxy: {
                type: 'ajax',
                url: '../../Seguridad/Trabajador/ListarTrabajadoresxUsuarioLog',
                reader: { type: 'json' }
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

        var stTramo = Ext.create('Ext.data.Store', {
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cartera/ListarTramoxDepartamento',
                reader: { type: 'json', root: 'data' }
            }
        });

        var stProducto = Ext.create('Ext.data.Store', {
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cartera/ListarProductoxDepartamento',
                reader: { type: 'json', root: 'data' }
            }
        });

        var stCluster = Ext.create('Ext.data.Store', {
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cartera/ListarClusterxTramo',
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

        var stTipoDetalle = Ext.create('Ext.data.Store', {
            autoLoad: true,
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/TipoDetalle/Listar',
                reader: { type: 'json', root: 'data' }
            }
        });

        var stTipoEstado = Ext.create('Ext.data.Store', {
            autoLoad: true,
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/TipoEstado/Listar',
                reader: { type: 'json', root: 'data' }
            }
        });

        var stMoroso = Ext.create('Ext.data.Store', {
            autoLoad: false,
            model: Ext.define('Moroso', { extend: 'Ext.data.Model' }),
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cartera/ListarMorososEnCarteraV2',
                reader: { type: 'json', root: 'data' }
            },
            listeners: {
                'metachange': function (store, meta) {
                    this.getComponent('pnlBusqueda').getComponent('grdMoroso').reconfigure(store, meta.columns);
                    console.log(this.getComponent('pnlBusqueda').getComponent('grdMoroso'));
                },
                scope: this
            }
        });

        var stServicio = Ext.create('Ext.data.Store', {
            autoLoad: false,
            model: Ext.define('Servicio', { extend: 'Ext.data.Model' }),
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cartera/ListarServicioV2',
                reader: { type: 'json', root: 'data' }
            },
            listeners: {
                'metachange': function (store, meta) {
                    this.getComponent('pnlBusqueda').getComponent('grdServicio').reconfigure(store, meta.columns);
                    console.log(this.getComponent('pnlBusqueda').getComponent('grdServicio'));
                },
                scope: this
            }
        });

        var stDetalleMoroso = Ext.create('Ext.data.Store', {
            fields: [
                { name: 'DetalleMoroso', type: 'int' },
                { name: 'TipoDetalle', type: 'int' },
                { name: 'DTipoDetalle', type: 'string' },
                { name: 'Descripcion', type: 'string' },
                { name: 'DescripcionEstado', type: 'string' },
                { name: 'TipoEstado', type: 'int' },
                { name: 'DTipoEstado', type: 'string' },
                { name: 'Editable', type: 'bool' }
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
            },
            listeners: {
                load: function () {
                    this.getComponent('pnlBusqueda').getComponent('grdDetalleMoroso').getSelectionModel().select(0); ;
                },
                scope: this
            },
            autoLoad: false
        });

        var stTipoGestion = Ext.create('Ext.data.Store', {
            fields: [
                { name: 'TipoGestion', type: 'int' },
                { name: 'DTipoGestion', type: 'string' }
            ],
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/GestionCliente/ListarTipoGestion',
                reader: {
                    type: 'json'
                },
                params: {
                    tipoDetalle: 0
                }
            },
            sorters: [{
                property: 'DTipoGestion',
                direction: 'ASC'
            }],
            autoLoad: false
        });

        var stClaseGestion = Ext.create('Ext.data.Store', {
            fields: [
                { name: 'ClaseGestion', type: 'int' },
                { name: 'Codigo', type: 'string' },
                { name: 'DClaseGestion', type: 'string' },
                { name: 'AplicaPromesa', type: 'bool' }
            ],
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/GestionCliente/ListarClaseGestion',
                reader: {
                    type: 'json'
                }
            },
            sorters: [{
                property: 'ClaseGestion',
                direction: 'ASC'
            }],
            autoLoad: false
        });

        var stDClaseGestion = Ext.create('Ext.data.Store', {
            fields: [
                { name: 'DClaseGestion', type: 'int' },
                { name: 'Descripcion', type: 'string' }
            ],
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/GestionCliente/ListarDClaseGestion',
                reader: {
                    type: 'json'
                }
            },
            autoLoad: false
        });

        var stGestionMoroso = Ext.create('Ext.data.Store', {
            fields: [
                { name: 'GestionMoroso', type: 'int' },
                { name: 'DetalleMoroso', type: 'int' },
                { name: 'TipoGestion', type: 'int' },
                { name: 'DTipoGestion', type: 'string' },
                { name: 'ClaseGestion', type: 'int' },
                { name: 'Codigo', type: 'string' },
                { name: 'DescClaseGestion', type: 'string' },
                { name: 'DClaseGestion', type: 'int' },
                { name: 'DescDClaseGestion', type: 'string' },
                { name: 'FechaGestion', type: 'string' },
                { name: 'FechaPromesa', type: 'string' },
                { name: 'Monto', type: 'Monto' },
                { name: 'Trabajador', type: 'int' },
                { name: 'DTrabajador', type: 'string' },
                { name: 'Observacion', type: 'Observacion' }
            ],
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cartera/ListarGestionMoroso',
                reader: {
                    type: 'json'
                },
                params: {
                    detalleMoroso: 0
                }
            },
            autoLoad: false
        });

        Ext.applyIf(me, {
            items: [
            {
                itemId: 'pnlBusqueda',
                header: false,
                //                title: 'Lista de Morosos',
                region: 'west',
                border: false,
                split: true,
                width: 700,
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [
                {
                    xtype: 'gridpanel',
                    itemId: 'grdMoroso',
                    title: 'Lista de Morosos',
                    //                    header: false,
                    store: stMoroso,
                    columnLines: true,
                    emptyText: 'No se encontraron datos.',
                    flex: 1.5,
                    columns: [],
                    plugins: {
                        ptype: 'bufferedrenderer',
                        trailingBufferZone: 20,
                        leadingBufferZone: 30
                    },
                    features: [{
                        ftype: 'filters',
                        autoReload: false,
                        local: true,
                        filters: [{ type: 'string', dataIndex: 'Cluster' },
                            { type: 'string', dataIndex: 'Departamento' },
                            { type: 'string', dataIndex: 'NumeroDocumento' },
                            { type: 'string', dataIndex: 'CodCliente' },
                            { type: 'string', dataIndex: 'Cuenta' },
                            { type: 'string', dataIndex: 'Servicio' },
                            { type: 'string', dataIndex: 'DMoroso' },
                            { type: 'numeric', dataIndex: 'DeudaTotal' },
                            { type: 'numeric', dataIndex: 'PagoTotal' },
                            { type: 'numeric', dataIndex: 'Saldo' },
                            { type: 'boolean', dataIndex: 'GestionadoCall', defaultValue: null, yesText: 'Si', noText: 'No' },
                            { type: 'boolean', dataIndex: 'ContactadoCall', defaultValue: null, yesText: 'Si', noText: 'No' },
                            { type: 'date', dataIndex: 'PromesaPago', beforeText: 'Antes del', afterText: 'Después del', onText: 'En el' },
                            { type: 'string', dataIndex: 'Zonal' }
                        ]
                    }],
                    listeners: {
                        selectionchange: {
                            fn: me.onGridMorosoSelectionChange,
                            scope: me
                        }
                    }
                },
                {
                    xtype: 'gridpanel',
                    itemId: 'grdServicio',
                    title: 'Lista de Servicios',
                    store: stServicio,
                    columnLines: true,
                    emptyText: 'No se encontraron datos.',
                    flex: 1,
                    columns: []
                },
                {
                    xtype: 'gridpanel',
                    itemId: 'grdDetalleMoroso',
                    title: 'Detalles Morosos',
                    store: stDetalleMoroso,
                    columnLines: true,
                    emptyText: 'No se encontraron datos.',
                    flex: 1,
                    rbar: [
                        {
                            itemId: 'btnAgregar',
                            iconCls: 'icon-add',
                            tooltip: 'Agregar',
                            handler: me.onBtnAgregarClick,
                            scope: me
                        }/*,
                        {
                            itemId: 'btnQuitar',
                            iconCls: 'icon-cancel',
                            tooltip: 'Quitar',
                            handler: me.onBtnQuitarClick,
                            scope: me
                        }*/
                    ],
                    columns: [
                        {
                            xtype: 'rownumberer'
                        },
                        {
                            xtype: 'numbercolumn',
                            dataIndex: 'DetalleMoroso',
                            hidden: true,
                            hideable: false
                        },
                        {
                            dataIndex: 'TipoDetalle',
                            text: 'Tipo Detalle',
                            width: 100,
                            hideable: false,
                            renderer: function (v, metaData, record) {
                                var data;
                                if (record.get('Editable').toString() == 'true') {
                                    data = '<span style="color:green;">' + record.get('DTipoDetalle') + '</span>';
                                }
                                else {
                                    data = record.get('DTipoDetalle');
                                }
                                return data;
                            },
                            editor: {
                                xtype: 'combo',
                                itemId: 'cbxTipoDetalle',
                                store: stTipoDetalle,
                                displayField: 'DTipoDetalle',
                                valueField: 'TipoDetalle',
                                allowBlank: false,
                                forceSelection: true,
                                listeners: {
                                    focus: {
                                        fn: function (combo, The, eOpts) {
                                            var editable = this.getComponent('pnlBusqueda').getComponent('grdDetalleMoroso').getSelectionModel().getSelection()[0].get('Editable');
                                            console.log(editable);
                                            if (editable.toString() == 'true') {
                                                combo.setReadOnly(false);
                                            }
                                            else {
                                                combo.setReadOnly(true);
                                            }
                                        },
                                        scope: me
                                    }
                                }
                            }
                        },
                        {
                            dataIndex: 'Descripcion',
                            text: 'Descripción',
                            width: 250,
                            hideable: false,
                            renderer: function (v, metaData, record) {
                                var data;
                                if (record.get('Editable').toString() == 'true') {
                                    data = '<span style="color:green;">' + record.get('Descripcion') + '</span>';
                                }
                                else {
                                    data = record.get('Descripcion');
                                }
                                return data;
                            },
                            editor: {
                                allowBlank: true,
                                listeners: {
                                    focus: {
                                        fn: function (combo, The, eOpts) {
                                            var editable = this.getComponent('pnlBusqueda').getComponent('grdDetalleMoroso').getSelectionModel().getSelection()[0].get('Editable');
                                            console.log(editable);
                                            if (editable.toString() == 'true') {
                                                combo.setReadOnly(false);
                                            }
                                            else {
                                                combo.setReadOnly(true);
                                            }
                                        },
                                        scope: me
                                    }
                                }
                            }
                        },
                        {
                            dataIndex: 'DescripcionEstado',
                            text: 'DescripcionEstado',
                            width: 100,
                            hideable: false,
                            renderer: function (v, metaData, record) {
                                var data;
                                if (record.get('Editable').toString() == 'true') {
                                    data = '<span style="color:green;">' + record.get('DescripcionEstado') + '</span>';
                                }
                                else {
                                    data = record.get('DescripcionEstado');
                                }
                                return data;
                            },
                            editor: {
                                allowBlank: true
                            }
                        },
                        {
                            dataIndex: 'TipoEstado',
                            text: 'Tipo Estado',
                            width: 150,
                            hideable: false,
                            renderer: function (v, metaData, record) {
                                var data;
                                if (record.get('Editable').toString() == 'true') {
                                    data = '<span style="color:green;">' + record.get('DTipoEstado') + '</span>';
                                }
                                else {
                                    data = record.get('DTipoEstado');
                                }
                                return data;
                            },
                            editor: {
                                xtype: 'combo',
                                itemId: 'cbxEstado',
                                store: stTipoEstado,
                                displayField: 'DTipoEstado',
                                valueField: 'TipoEstado',
                                allowBlank: false,
                                forceSelection: true
                            },
                            flex: 1
                        },
                    //                        {
                    //                            flex: 1,
                    //                            menuDisabled: true,
                    //                            hideable: false
                    //                        },
                        {
                        xtype: 'checkcolumn',
                        dataIndex: 'Editable',
                        text: 'Editable',
                        lockable: false,
                        processEvent: function () { return false; },
                        hidden: true,
                        hideable: false
                    }
                    ],
                    plugins: [{
                        ptype: 'rowediting',
                        clicksToMoveEditor: 1,
                        autoCancel: false,
                        pluginId: 'editplugin',
                        listeners: {
                            edit: {
                                fn: me.onDetalleMorosoAfterEdit,
                                scope: me
                            },
                            beforeedit: {
                                fn: me.onDetalleMorosoBeforeEdit,
                                scope: me
                            }
                        }
                    }
                    ],
                    listeners: {
                        selectionchange: {
                            fn: me.onGridDetalleMorosoSelectionChange,
                            scope: me
                        }
                    }
                }
                ]
            },
            {
                itemId: 'pnlContenido',
                header: false,
                region: 'center',
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [
                    {
                        xtype: 'gridpanel',
                        itemId: 'grdGestion',
                        title: 'Lista de Gestiones',
                        store: stGestionMoroso,
                        columnLines: true,
                        emptyText: 'No se encontraron datos.',
                        flex: 1,
                        columns: [{
                            xtype: 'rownumberer'
                        },
                        {
                            xtype: 'numbercolumn',
                            dataIndex: 'GestionMoroso',
                            text: 'N° Gestión',
                            format: '0',
                            hidden: true,
                            hideable: false
                        },
                        {
                            xtype: 'numbercolumn',
                            dataIndex: 'DetalleMoroso',
                            text: 'DetalleMoroso',
                            hidden: true,
                            hideable: false
                        },
                        {
                            xtype: 'numbercolumn',
                            dataIndex: 'TipoGestion',
                            text: 'TipoGestion',
                            hidden: true,
                            hideable: false
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'DTipoGestion',
                            text: 'Tipo Gestión',
                            hideable: false
                        },
                        {
                            xtype: 'numbercolumn',
                            dataIndex: 'ClaseGestion',
                            text: 'ClaseGestion',
                            hidden: true,
                            hideable: false
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Codigo',
                            text: 'Código',
                            hideable: false
                        },
                        {
                            xtype: 'numbercolumn',
                            dataIndex: 'DClaseGestion',
                            text: 'DClaseGestion',
                            hidden: true,
                            hideable: false
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'DescDClaseGestion',
                            text: 'SubCódigo',
                            hideable: false
                        },
                        {
                            xtype: 'datecolumn',
                            dataIndex: 'FechaGestion',
                            text: 'Fecha Gestión',
                            renderer: function (v) {
                                var dateObject = Ext.Date.parse(v, 'MS');
                                var ymdString = Ext.util.Format.date(dateObject, 'd/m/Y');
                                return ymdString;
                            },
                            width: 115,
                            hideable: false
                        },
                        {
                            xtype: 'datecolumn',
                            dataIndex: 'FechaPromesa',
                            text: 'Fecha Promesa',
                            renderer: function (v) {
                                var dateObject = Ext.Date.parse(v, 'MS');
                                var ymdString = Ext.util.Format.date(dateObject, 'd/m/Y');
                                return ymdString;
                            },
                            width: 115,
                            hideable: false
                        },
                        {
                            xtype: 'numbercolumn',
                            dataIndex: 'Monto',
                            text: 'Monto',
                            width: 90,
                            hideable: false
                        },
                        {
                            xtype: 'numbercolumn',
                            dataIndex: 'Trabajador',
                            text: 'Trabajador',
                            hidden: true,
                            hideable: false
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'DTrabajador',
                            text: 'Gestor',
                            width: 150,
                            hideable: false
                        }
                        ],
                        listeners: {
                            selectionchange: {
                                fn: me.onGridGestionSelectionChange,
                                scope: me
                            }
                        }
                    },
                    {
                        itemId: 'pnlRegistro',
                        title: 'Registro de Gestión',
                        autoScroll: true,
                        bodyStyle: 'padding:10px;',
                        layout: {
                            type: 'table',
                            columns: 2,
                            tableAttrs: {
                                style: {
                                    width: '100%'
                                }
                            }
                        },
                        items: [
                        {
                            xtype: 'textfield',
                            itemId: 'txtGestionMoroso',
                            hidden: true,
                            fieldLabel: 'N° Gestión'
                        },
                        {
                            xtype: 'textfield',
                            itemId: 'txtDetalleMoroso',
                            fieldLabel: 'DetalleMoroso',
                            hidden: true
                        },
                        {
                            xtype: 'textfield',
                            itemId: 'txtTrabajador',
                            fieldLabel: 'Trabajador',
                            hidden: true
                        },
                        {
                            xtype: 'textfield',
                            itemId: 'txtGestionCliente',
                            fieldLabel: 'txtGestionCliente',
                            hidden: true
                        },
                        {
                            xtype: 'combo',
                            itemId: 'cbxTipoGestion',
                            lastQuery: '',
                            fieldLabel: 'Tipo Gestión',
                            emptyText: '< Seleccione >',
                            store: stTipoGestion,
                            displayField: 'DTipoGestion',
                            valueField: 'TipoGestion',
                            allowBlank: false,
                            colspan: 2,
                            forceSelection: true,
                            queryMode: 'local'
                        },
                        {
                            xtype: 'combo',
                            itemId: 'cbxTrabajador',
                            lastQuery: '',
                            fieldLabel: 'Gestor',
                            emptyText: '< Seleccione >',
                            store: stTrabajadorUsuario,
                            displayField: 'DTrabajador',
                            valueField: 'Trabajador',
                            allowBlank: false,
                            forceSelection: true,
                            queryMode: 'local',
                            colspan: 2,
                            //anchor: '100%',
                            width: 450
                        },
                        {
                            xtype: 'combo',
                            itemId: 'cbxClaseGestion',
                            lastQuery: '',
                            fieldLabel: 'Código',
                            emptyText: '< Seleccione >',
                            store: stClaseGestion,
                            displayField: 'Codigo',
                            valueField: 'ClaseGestion',
                            colspan: 2,
                            //anchor: '100%',
                            width: 450,
                            allowBlank: false,
                            forceSelection: true,
                            queryMode: 'local',
                            listeners: {
                                select: {
                                    fn: me.oncbxClaseGestionSelect,
                                    scope: me
                                }
                            }
                        },
                        {
                            xtype: 'combo',
                            itemId: 'cbxDClaseGestion',
                            lastQuery: '',
                            fieldLabel: 'SubCódigo',
                            emptyText: '< Seleccione >',
                            store: stDClaseGestion,
                            displayField: 'Descripcion',
                            valueField: 'DClaseGestion',
                            allowBlank: false,
                            forceSelection: true,
                            queryMode: 'local',
                            colspan: 2,
                            //anchor: '100%',
                            width: 450
                        },
                        {
                            xtype: 'datefield',
                            itemId: 'dtpFechaGestion',
                            format: 'd/m/Y',
                            anchor: '100%',
                            width: '90%',
                            fieldLabel: 'Fecha Gestión',
                            maxValue: new Date(),
                            value: new Date()
                        },
                        {
                            xtype: 'datefield',
                            itemId: 'dtpFechaPromesa',
                            width: '90%',
                            format: 'd/m/Y',
                            fieldLabel: 'Fecha Promesa'
                        },
                        {
                            xtype: 'numberfield',
                            itemId: 'txtMonto',
                            fieldLabel: 'Monto',
                            minValue: 0,
                            value: 0,
                            decimalSeparator: '.',
                            decimalPrecision: 2,
                            allowBlank: false,
                            blankText: 'Este campo es obligatorio.'
                        },
                        {
                            xtype: 'button',
                            itemId: 'btnDatos',
                            text: 'Datos Adicionales',
                            handler: me.onBtnDialogClick,
                            scope: me
                        },
                        {
                            xtype: 'label',
                            name: 'message',
                            text: 'Observación:',
                            colspan: 2
                        },
                        {
                            xtype: 'textareafield',
                            itemId: 'txtObservacion',
                            grow: true,
                            anchor: '100%',
                            width: '95%',
                            colspan: 2
                        }
                        ]
                    }
                ]
            },
            {
                itemId: 'pnlFiltro',
                title: 'Filtros',
                region: 'east',
                border: false,
                split: true,
                collapsed: true,
                collapsible: true,
                width: 280,
                bodyStyle: 'padding:20px;',
                layout: 'form',
                items: [
                    {
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
                        emptyText: '< Seleccione >',
                        store: stDepartamento,
                        displayField: 'Departamento',
                        valueField: 'Departamento',
                        allowBlank: false,
                        forceSelection: true,
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
                        emptyText: '< Todos >',
                        store: stDepartamentoIBK,
                        displayField: 'Departamento',
                        valueField: 'Departamento',
                        allowBlank: true,
                        //                        forceSelection: true,
                        multiSelect: true,
                        queryMode: 'local',
                        listeners: {
                            select: {
                                fn: me.oncbxDepartamentoIBKSelect,
                                scope: me
                            }
                        }
                    },
                    {
                        xtype: 'combo',
                        itemId: 'cbxTramo',
                        lastQuery: '',
                        fieldLabel: 'Tramo',
                        emptyText: '< Todos >',
                        store: stTramo,
                        displayField: 'Tramo',
                        valueField: 'Tramo',
                        multiSelect: true,
                        allowBlank: true,
                        //                        forceSelection: true,
                        queryMode: 'local',
                        listeners: {
                            select: {
                                fn: me.oncbxTramoSelect,
                                scope: me
                            }
                        }
                    },
                    {
                        xtype: 'combo',
                        itemId: 'cbxProducto',
                        lastQuery: '',
                        fieldLabel: 'Producto',
                        emptyText: '< Todos >',
                        store: stProducto,
                        displayField: 'Producto',
                        valueField: 'Producto',
                        multiSelect: true,
                        allowBlank: true,
                        hidden: true,
                        //                        forceSelection: true,
                        queryMode: 'local'
                    },
                    {
                        xtype: 'combo',
                        itemId: 'cbxCluster',
                        lastQuery: '',
                        fieldLabel: 'Cluster',
                        emptyText: '< Todos >',
                        store: stCluster,
                        displayField: 'Cluster',
                        valueField: 'Cluster',
                        multiSelect: true,
                        queryMode: 'local',
                        listeners: {
                            select: {
                                fn: me.oncbxClusterSelect,
                                scope: me
                            }
                        }
                    },
                    {
                        xtype: 'checkbox',
                        itemId: 'chkOtrosFiltros',
                        labelWidth: 100,
                        hidden: true,
                        fieldLabel: 'Más opciones',
                        listeners: {
                            change: {
                                fn: me.onchkOtrosFiltrosChange,
                                scope: me
                            }
                        }
                    },
                    {
                        xtype: 'combo',
                        itemId: 'cbxBuscarEn',
                        lastQuery: '',
                        hidden: true,
                        fieldLabel: 'Buscar en',
                        emptyText: '< Seleccione >',
                        //                        store: stCluster,
                        //                        displayField: 'Cluster',
                        //                        valueField: 'Cluster',
                        queryMode: 'local',
                        listeners: {
                            select: {
                                fn: me.oncbxBuscarEnSelect,
                                scope: me
                            }
                        }
                    },
                    {
                        xtype: 'combo',
                        itemId: 'cbxBuscarPor',
                        lastQuery: '',
                        hidden: true,
                        fieldLabel: 'Buscar por',
                        emptyText: '< Seleccione >',
                        //                        store: stCluster,
                        //                        displayField: 'Cluster',
                        //                        valueField: 'Cluster',
                        queryMode: 'local'//,
                        //                        listeners: {
                        //                            select: {
                        //                                fn: me.oncbxBuscarPorSelect,
                        //                                scope: me
                        //                            }
                        //                        }
                    }
                ],
                buttons: [
                    {
                        xtype: 'button',
                        itemId: 'btnBuscar',
                        text: 'Buscar',
                        textAlign: 'right',
                        iconCls: 'icon-buscar',
                        handler: me.onBtnBuscarClick,
                        scope: me
                    }
                ]
            }],
            buttons: [
                {
                    itemId: 'btnNuevo',
                    text: 'Nuevo',
                    iconCls: 'icon-new',
                    handler: me.onBtnNuevoClick,
                    scope: me
                },
                {
                    itemId: 'btnEditar',
                    text: 'Editar',
                    iconCls: 'icon-edit',
                    handler: me.onBtnEditarClick,
                    scope: me
                },
                {
                    itemId: 'btnGuardar',
                    text: 'Guardar',
                    iconCls: 'icon-save',
                    handler: me.onBtnGuardarClick,
                    scope: me
                },
                {
                    itemId: 'btnCancelar',
                    text: 'Cancelar',
                    iconCls: 'icon-cancel',
                    handler: me.onBtnCancelarClick,
                    scope: me
                },
                {
                    itemId: 'btnExportar',
                    iconCls: 'icon-export',
                    text: 'Exportar',
                    hidden: true,
                    href: '../../Cobranza/Cartera/ExportarMorosos'
                },
                {
                    itemId: 'btnSalir',
                    text: 'Salir',
                    iconCls: 'icon-exit',
                    handler: me.onBtnSalirClick,
                    scope: me
                }
            ]
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
                this.getComponent('pnlFiltro').getComponent('cbxTramo').setVisible(false);
                this.getComponent('pnlFiltro').getComponent('cbxProducto').setVisible(false);
                this.getComponent('pnlFiltro').getComponent('cbxCluster').setVisible(false);
                this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxTrabajador').getStore().load();
                this.fnEstadoForm('visualizacion');
                this.getComponent('pnlFiltro').getComponent('cbxBuscarEn').setDisabled(true);
                this.getComponent('pnlFiltro').getComponent('cbxBuscarPor').setDisabled(true);
                Ext.MessageBox.hide();
            }
        }
    },

    onDetalleMorosoBeforeEdit: function (editor, e) {
        //console.log(this.getComponent('pnlBusqueda').getComponent('grdDetalleMoroso').columns[2].getEditor());
    },

    onDetalleMorosoAfterEdit: function (editor, e) {
        var moroso = this.getComponent('pnlBusqueda').getComponent('grdMoroso').getSelectionModel().getSelection()[0].get('Moroso');

        var dtGestion = {};

        dtGestion['DetalleMoroso'] = e.record.get('DetalleMoroso');
        dtGestion['Moroso'] = moroso;
        dtGestion['TipoDetalle'] = e.record.get('TipoDetalle');
        dtGestion['Descripcion'] = e.record.get('Descripcion');
        dtGestion['DescripcionEstado'] = e.record.get('DescripcionEstado');
        dtGestion['TipoEstado'] = e.record.get('TipoEstado');
        dtGestion['Editable'] = e.record.get('Editable');

        Ext.Ajax.request({
            url: "../../Cartera/InsUpdDetalleMoroso",
            success: function (response) {
                var respuesta = Ext.decode(response.responseText);
                if (respuesta['success'] == "true") {
                    if (moroso == 0) {
                        Ext.example.msg('Información', 'Se registro con exitó');
                    }
                    else {
                        Ext.example.msg('Información', 'Actualización realizada con exitó');
                    }
                    this.getComponent('pnlBusqueda').getComponent('grdDetalleMoroso').getStore().load({
                        params: {
                            moroso: moroso
                        }
                    });
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
                datos: '[' + Ext.encode(dtGestion) + ']'
            },
            scope: this
        });
    },

    oncbxClienteSelect: function (combo, records, eOpts) {
        this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getStore().load({
            params: {
                cliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxCliente').getValue())
            }
        });
        this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxFechaFin').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxZonal').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxDepartamento').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxDepartamentoIBK').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxTramo').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxProducto').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxCluster').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').setVisible(false);
        this.getComponent('pnlFiltro').getComponent('cbxFechaFin').setVisible(false);
        this.getComponent('pnlFiltro').getComponent('cbxZonal').setVisible(false);
        this.getComponent('pnlFiltro').getComponent('cbxDepartamento').setVisible(false);
        this.getComponent('pnlFiltro').getComponent('cbxDepartamentoIBK').setVisible(false);
        this.getComponent('pnlFiltro').getComponent('cbxTramo').setVisible(false);
        this.getComponent('pnlFiltro').getComponent('cbxProducto').setVisible(false);
        this.getComponent('pnlFiltro').getComponent('cbxCluster').setVisible(false);

    },

    onchkOtrosFiltrosChange: function (cb, checked) {
        //        this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').setDisabled(!checked);
        this.getComponent('pnlFiltro').getComponent('cbxFechaFin').setDisabled(checked);
        this.getComponent('pnlFiltro').getComponent('cbxZonal').setDisabled(checked);
        this.getComponent('pnlFiltro').getComponent('cbxDepartamento').setDisabled(checked);
        this.getComponent('pnlFiltro').getComponent('cbxTramo').setDisabled(checked);
        this.getComponent('pnlFiltro').getComponent('cbxCluster').setDisabled(checked);
        this.getComponent('pnlFiltro').getComponent('cbxBuscarEn').setDisabled(!checked);
        this.getComponent('pnlFiltro').getComponent('cbxBuscarPor').setDisabled(!checked);
    },

    oncbxGestionClienteSelect: function (combo, records, eOpts) {
        if (this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 1) {
            this.getComponent('pnlFiltro').getComponent('cbxFechaFin').setVisible(true);
            this.getComponent('pnlFiltro').getComponent('cbxZonal').setVisible(true);
            this.getComponent('pnlFiltro').getComponent('cbxDepartamento').setVisible(true);
            this.getComponent('pnlFiltro').getComponent('cbxTramo').setVisible(true);
            this.getComponent('pnlFiltro').getComponent('cbxCluster').setVisible(true);
            this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').setVisible(false);
            this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getStore().load({
                params: {
                    gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue())
                }
            });
            this.getComponent('pnlFiltro').getComponent('cbxFechaFin').clearValue();
            this.getComponent('pnlFiltro').getComponent('cbxZonal').clearValue();
            this.getComponent('pnlFiltro').getComponent('cbxDepartamento').clearValue();
            this.getComponent('pnlFiltro').getComponent('cbxTramo').clearValue();
            this.getComponent('pnlFiltro').getComponent('cbxProducto').clearValue();
            this.getComponent('pnlFiltro').getComponent('cbxCluster').clearValue();
        } else if (this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 2) {
            this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').setVisible(true);
            this.getComponent('pnlFiltro').getComponent('cbxDepartamentoIBK').setVisible(true);
            this.getComponent('pnlFiltro').getComponent('cbxTramo').setVisible(true);
            //            this.getComponent('pnlFiltro').getComponent('cbxProducto').setVisible(true);
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
            this.getComponent('pnlFiltro').getComponent('cbxTramo').clearValue();
            this.getComponent('pnlFiltro').getComponent('cbxCluster').clearValue();
        }
    },

    oncbxFechaInicioSelect: function (combo, records, eOpts) {
        this.getComponent('pnlFiltro').getComponent('cbxDepartamentoIBK').getStore().load({
            params: {
                gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue()),
                fechaInicio: this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').getValue()
            }
        });
        this.getComponent('pnlFiltro').getComponent('cbxTramo').getStore().load({
            params: {
                gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue()),
                fechaInicio: this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').getValue()
            }
        });
        this.getComponent('pnlFiltro').getComponent('cbxZonal').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxDepartamento').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxDepartamentoIBK').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxTramo').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxCluster').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxProducto').clearValue();
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
        this.getComponent('pnlFiltro').getComponent('cbxTramo').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxCluster').clearValue();
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
        this.getComponent('pnlFiltro').getComponent('cbxTramo').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxCluster').clearValue();
    },

    oncbxDepartamentoSelect: function (combo, records, eOpts) {
        var zon = [];
        var dpto = [];
        if (this.getComponent('pnlFiltro').getComponent('cbxZonal').getValue() != null) {
            zon = this.getComponent('pnlFiltro').getComponent('cbxZonal').getValue();
        }
        if (this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getValue() != null) {
            dpto = this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getValue();
        }
        this.getComponent('pnlFiltro').getComponent('cbxTramo').getStore().load({
            params: {
                gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue()),
                fechaFin: this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getValue(),
                zonales: zon,
                departamento: dpto
            }
        });
        this.getComponent('pnlFiltro').getComponent('cbxTramo').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxCluster').clearValue();
        //this.getComponent('pnlSector').setDisabled(true);
    },

    oncbxDepartamentoIBKSelect: function (combo, records, eOpts) {
        var dpto = [];
        if (this.getComponent('pnlFiltro').getComponent('cbxDepartamentoIBK').getValue() != null) {
            dpto = this.getComponent('pnlFiltro').getComponent('cbxDepartamentoIBK').getValue();
        }
        this.getComponent('pnlFiltro').getComponent('cbxTramo').getStore().load({
            params: {
                gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue()),
                fechaInicio: this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').getValue(),
                departamento: dpto
            }
        });
        this.getComponent('pnlFiltro').getComponent('cbxTramo').clearValue();
        this.getComponent('pnlFiltro').getComponent('cbxProducto').clearValue();
    },

    oncbxTramoSelect: function (combo, records, eOpts) {
        var zon = [];
        var dpto = [];
        var tram = [];
        if (this.getComponent('pnlFiltro').getComponent('cbxZonal').getValue() != null) {
            zon = this.getComponent('pnlFiltro').getComponent('cbxZonal').getValue();
        }
        if (this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getValue() != null) {
            dpto = this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getValue();
        }
        if (this.getComponent('pnlFiltro').getComponent('cbxTramo').getValue() != null) {
            tram = this.getComponent('pnlFiltro').getComponent('cbxTramo').getValue();
        }
        this.getComponent('pnlFiltro').getComponent('cbxCluster').getStore().load({
            params: {
                gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue()),
                fechaFin: this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getValue(),
                zonales: zon,
                departamento: dpto,
                tramo: tram
            }
        });
        this.getComponent('pnlFiltro').getComponent('cbxCluster').clearValue();
    },

    oncbxClusterSelect: function (combo, records, eOpts) {

    },

    oncbxBuscarEnSelect: function (combo, records, eOpts) {

    },

    oncbxClaseGestionSelect: function (combo, records, eOpts) {
        if (records[0].get('AplicaPromesa').toString() == 'true') {
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('dtpFechaPromesa').setDisabled(false);
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtMonto').setDisabled(false);
        }
        else {
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('dtpFechaPromesa').setDisabled(true);
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtMonto').setDisabled(true);
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('dtpFechaPromesa').setValue(null);
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtMonto').setValue('0');
        }
        this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxDClaseGestion').getStore().load({
            params: {
                claseGestion: parseInt(this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxClaseGestion').getValue())
            }
        });
    },

    onGridMorosoSelectionChange: function (tablepanel, selections, options) {
        if (this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtGestionCliente').getValue().toString() == '1') {
            if (selections.length > 0) {
                this.getComponent('pnlBusqueda').getComponent('grdServicio').getStore().load({
                    params: {
                        detalleCartera: parseInt(selections[0].get('DetalleCartera')),
                        gestionCliente: this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtGestionCliente').getValue(),
                        moroso: parseInt(selections[0].get('Moroso')),
                        cartera: parseInt(selections[0].get('Cartera'))
                    }
                });
                this.getComponent('pnlBusqueda').getComponent('grdDetalleMoroso').getStore().load({
                    params: {
                        moroso: parseInt(selections[0].get('Moroso'))
                    }
                });
            }
            console.log(Ext.encode(this.onBtnBuscarClick));
        } else if (this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtGestionCliente').getValue() == '2') {
            if (selections.length > 0) {
                this.getComponent('pnlBusqueda').getComponent('grdServicio').getStore().load({
                    params: {
                        gestionCliente: this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtGestionCliente').getValue(),
                        moroso: parseInt(selections[0].get('Moroso')),
                        cartera: parseInt(selections[0].get('Cartera')),
                        detalleCartera: 0
                    }
                });
                this.getComponent('pnlBusqueda').getComponent('grdDetalleMoroso').getStore().load({
                    params: {
                        moroso: parseInt(selections[0].get('Moroso'))
                    }
                });
            }
        }
    },

    onGridDetalleMorosoSelectionChange: function (tablepanel, selections, options) {
        if (selections.length > 0) {
            this.getComponent('pnlContenido').getComponent('grdGestion').getStore().load({
                params: {
                    detalleMoroso: parseInt(selections[0].get('DetalleMoroso'))
                }
            });
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtDetalleMoroso').setValue(selections[0].get('DetalleMoroso'));
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxTipoGestion').getStore().load({
                params: {
                    tipoDetalle: parseInt(selections[0].get('TipoDetalle'))
                }
            });
            this.fnLimpiarControles();

        }
    },

    onGridGestionSelectionChange: function (tablepanel, selections, options) {
        if (selections.length > 0) {
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtGestionMoroso').setValue(selections[0].get('GestionMoroso'));
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtDetalleMoroso').setValue(selections[0].get('DetalleMoroso'));
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxTipoGestion').setValue(selections[0].get('TipoGestion'));
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxClaseGestion').setValue(selections[0].get('ClaseGestion'));
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxTrabajador').setValue(selections[0].get('Trabajador'));
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxDClaseGestion').getStore().load({
                params: {
                    claseGestion: parseInt(this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxClaseGestion').getValue())
                }
            });
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxDClaseGestion').setValue(selections[0].get('DClaseGestion'));
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('dtpFechaGestion').setValue(Ext.Date.parse(selections[0].get('FechaGestion'), 'MS'));
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('dtpFechaPromesa').setValue(Ext.Date.parse(selections[0].get('FechaPromesa'), 'MS'));
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtMonto').setValue(selections[0].get('Monto'));
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtObservacion').setValue(selections[0].get('Observacion'));
        }
    },

    onBtnAgregarClick: function (button, e, options) {
        this.getComponent('pnlBusqueda').getComponent('grdDetalleMoroso').getPlugin('editplugin').cancelEdit();
        var r = { 'DetalleMoroso': 0, 'Descripcion': '', 'DescripcionEstado': '', 'DTipoEstado': null, 'Editable': true };
        this.getComponent('pnlBusqueda').getComponent('grdDetalleMoroso').getStore().insert(0, r);
        this.getComponent('pnlBusqueda').getComponent('grdDetalleMoroso').getPlugin('editplugin').startEdit(0, 0);
    },

    onBtnQuitarClick: function (button, e, options) {

    },

    onBtnDialogClick: function (button, e, options) {
        if (this.getComponent('pnlBusqueda').getComponent('grdMoroso').getSelectionModel().getSelection().length > 0) {
            var dialog = Ext.create('CobApp.Moroso.FrmRegistrarDatosMoroso');
            dialog.setTitle('Ingreso de datos de Adicionales');
            //dialog.getComponent(0).getComponent('pnlRegistro').getComponent('cbxRubroEmpleo').bindStore(stRubroEmpleo);

            var moroso = this.getComponent('pnlBusqueda').getComponent('grdMoroso').getSelectionModel().getSelection()[0].get('Moroso');

            Ext.Ajax.request({
                url: "../../Moroso/ObtenerDatosMoroso",
                success: function (response) {
                    var respuesta = Ext.decode(response.responseText)[0];
                    dialog.getComponent(0).getComponent('pnlRegistro').getComponent('txtMoroso').setValue(respuesta['Moroso']);
                    dialog.getComponent(0).getComponent('pnlRegistro').getComponent('txtNumeroDocumento').setValue(respuesta['NumeroDocumento']);
                    dialog.getComponent(0).getComponent('pnlRegistro').getComponent('txtDMoroso').setValue(respuesta['DMoroso']);
                    dialog.getComponent(0).getComponent('pnlRegistro').getComponent('chkTrabaja').setValue(respuesta['Empleado']);
                    dialog.getComponent(0).getComponent('pnlRegistro').getComponent('cbxRubroEmpleo').setValue(respuesta['RubroEmpleo']);
                    dialog.getComponent(0).getComponent('pnlRegistro').getComponent('txtHoraContacto').setValue(respuesta['HoraContacto']);
                    dialog.getComponent(0).getComponent('pnlRegistro').getComponent('txtObservacion').setValue(respuesta['Observacion']);
                    dialog.show();
                },
                failure: function (response) {
                    Ext.MessageBox.show({
                        title: 'Sistema RJ Abogados',
                        msg: 'Se Produjo un error en la conexión.',
                        buttons: Ext.MessageBox.OK,
                        animateTarget: combo,
                        icon: Ext.Msg.ERROR
                    });
                },
                params: {
                    moroso: moroso
                },
                scope: this
            });
        }
        else {
            Ext.MessageBox.show({
                title: 'Sistema RJ Abogados',
                msg: 'Seleccione un Moroso.',
                buttons: Ext.MessageBox.OK,
                animateTarget: button,
                icon: Ext.Msg.INFO
            });
        }
    },

    onBtnBuscarClick: function (button, e, options) {
        if (this.fnEsValidoComprobar()) {
            if (this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 1) {
                if (this.fnEsValidoBuscar()) {
                    this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtGestionCliente').setValue(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue());
                    var dtCluster = [];
                    var dtDepartamento = [];
                    var dtTramo = [];
                    var dtZonal = [];
                    if (this.getComponent('pnlFiltro').getComponent('cbxCluster').getValue() != null) {
                        dtCluster = this.getComponent('pnlFiltro').getComponent('cbxCluster').getValue();
                    }

                    if (this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getValue() != null) {
                        dtDepartamento = this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getValue();
                    }

                    if (this.getComponent('pnlFiltro').getComponent('cbxTramo').getValue() != null) {
                        dtTramo = this.getComponent('pnlFiltro').getComponent('cbxTramo').getValue();
                    }

                    if (this.getComponent('pnlFiltro').getComponent('cbxZonal').getValue() != null) {
                        dtZonal = this.getComponent('pnlFiltro').getComponent('cbxZonal').getValue();
                    }

                    this.getComponent('pnlBusqueda').getComponent('grdMoroso').getStore().load({
                        params: {
                            cliente: this.getComponent('pnlFiltro').getComponent('cbxCliente').getValue().toString(),
                            gestionCliente: this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue().toString(),
                            fechaFin: this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getValue(),
                            zonal: dtZonal,
                            departamento: dtDepartamento,
                            tramo: dtTramo,
                            cluster: dtCluster
                        }
                    });
                    this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxClaseGestion').getStore().load({
                        params: {
                            gestionCliente: this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue().toString()
                        }
                    });
                }
            } else if (this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue() == 2) {
                if (this.fnEsValidoBuscarIBK()) {
                    this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtGestionCliente').setValue(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue());
                    var dtDepartamento = [];
                    var dtTramo = [];
                    if (this.getComponent('pnlFiltro').getComponent('cbxDepartamentoIBK').getValue() != null) {
                        dtDepartamento = this.getComponent('pnlFiltro').getComponent('cbxDepartamentoIBK').getValue();
                    }
                    if (this.getComponent('pnlFiltro').getComponent('cbxTramo').getValue() != null) {
                        dtTramo = this.getComponent('pnlFiltro').getComponent('cbxTramo').getValue();
                    }
                    this.getComponent('pnlBusqueda').getComponent('grdMoroso').getStore().load({
                        params: {
                            cliente: this.getComponent('pnlFiltro').getComponent('cbxCliente').getValue().toString(),
                            gestionCliente: this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue().toString(),
                            fechaInicio: this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').getValue(),
                            departamento: dtDepartamento,
                            tramo: dtTramo
                        }
                    });
                    this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxClaseGestion').getStore().load({
                        params: {
                            gestionCliente: this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue().toString()
                        }
                    });
                }
            }
        }
    },

    onBtnNuevoClick: function (button, e, options) {
        if (this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtDetalleMoroso').getValue().toString() == '') {
            Ext.MessageBox.show({
                title: 'Sistema RJ Abogados',
                msg: 'Seleccione un detalle moroso.',
                buttons: Ext.MessageBox.OK,
                animateTarget: button,
                icon: Ext.Msg.WARNING
            });
        }
        else {
            this.fnLimpiarControles();
            if (this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxTrabajador').getStore().getCount() ==1) {
                this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxTrabajador').setValue(this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxTrabajador').store.getAt(0).get('Trabajador'));
            } 
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxTipoGestion').setValue(this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxTipoGestion').store.getAt(0).get('TipoGestion'));
            this.fnEstadoForm('registro');
            if (this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxTipoGestion').getValue() == 1) {
                this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('dtpFechaGestion').setValue(new Date());
                this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('dtpFechaGestion').setDisabled(true);
            } else {
                this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('dtpFechaGestion').setDisabled(false);
            }
        }
    },

    onBtnEditarClick: function (button, e, options) {
        if (this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtGestionMoroso').getValue().toString() == '') {
            Ext.MessageBox.show({
                title: 'Sistema RJ Abogados',
                msg: 'No hay datos para editar.',
                buttons: Ext.MessageBox.OK,
                animateTarget: button,
                icon: Ext.Msg.WARNING
            });
        }
        else {
            this.fnEstadoForm('registro');
        }
    },

    onBtnCancelarClick: function (button, e, options) {
        this.fnLimpiarControles();
        this.fnEstadoForm('visualizacion');
    },

    onBtnSalirClick: function (button, e, options) {
        this.close();
    },

    onBtnGuardarClick: function (button, e, options) {
        if (this.fnEsValidoGuardar()) {
            var codigo = parseInt(this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtGestionMoroso').getValue())
            var dtGestion = {};

            dtGestion['GestionMoroso'] = codigo;
            dtGestion['Cartera'] = this.getComponent('pnlBusqueda').getComponent('grdMoroso').getSelectionModel().getSelection()[0].get('Cartera');
            dtGestion['DetalleMoroso'] = this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtDetalleMoroso').getValue();
            dtGestion['TipoGestion'] = this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxTipoGestion').getValue();
            dtGestion['ClaseGestion'] = this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxClaseGestion').getValue();
            dtGestion['DClaseGestion'] = this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxDClaseGestion').getValue();
            dtGestion['FechaGestion'] = this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('dtpFechaGestion').getValue();
            dtGestion['FechaPromesa'] = this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('dtpFechaPromesa').getValue();
            dtGestion['Monto'] = this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtMonto').getValue();
            dtGestion['Observacion'] = this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtObservacion').getValue();
            dtGestion['Trabajador'] = this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxTrabajador').getValue();

            Ext.Ajax.request({
                url: "../../Cartera/InsUpdGestionMoroso",
                success: function (response) {
                    var respuesta = Ext.decode(response.responseText);
                    if (respuesta['success'] == "true") {
                        if (codigo == 0) {
                            Ext.example.msg('Información', 'Se registró con éxito');
                        }
                        else {
                            Ext.example.msg('Información', 'Actualización realizada con éxito');
                        }
                        this.fnLimpiarControles();
                        var selections = this.getComponent('pnlBusqueda').getComponent('grdDetalleMoroso').getSelectionModel().getSelection();
                        if (selections.length > 0) {
                            this.getComponent('pnlContenido').getComponent('grdGestion').getStore().load({
                                params: {
                                    detalleMoroso: parseInt(selections[0].get('DetalleMoroso'))
                                }
                            });
                            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtDetalleMoroso').setValue(selections[0].get('DetalleMoroso'));
                        }
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
                    datos: '[' + Ext.encode(dtGestion) + ']'
                },
                scope: this
            });
        }
    },

    fnColorearPago: function (val) {
        if (val > 0) {
            return '<span style="color:red;">' + val + '%</span>';
        } else if (val < 0) {
            return '<span style="color:green;">' + val + '%</span>';
        }
        return val;
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
        if (!this.getComponent('pnlFiltro').getComponent('cbxCluster').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlFiltro').getComponent('cbxDepartamento').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlFiltro').getComponent('cbxZonal').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlFiltro').getComponent('cbxFechaFin').isValid()) {
            return false;
        }
        return true;
    },

    fnEsValidoBuscarIBK: function () {
        if (!this.getComponent('pnlFiltro').getComponent('cbxFechaInicio').isValid()) {
            return false;
        }
        return true;
    },

    fnEsValidoGuardar: function () {
        if (!this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxTipoGestion').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxTrabajador').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxClaseGestion').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxDClaseGestion').isValid()) {
            return false;
        }
        return true;
    },

    fnBloquearControles: function (estado) {
        this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtGestionMoroso').setDisabled(estado);
        this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxTipoGestion').setDisabled(estado);
        this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxClaseGestion').setDisabled(estado);
        this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxDClaseGestion').setDisabled(estado);
        this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('dtpFechaGestion').setDisabled(estado);
        this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('dtpFechaPromesa').setDisabled(estado);
        this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('dtpFechaPromesa').setDisabled(estado);
        this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxTrabajador').setDisabled(estado);
        this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtObservacion').setDisabled(estado);
        this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('btnDatos').setDisabled(estado);
    },

    fnLimpiarControles: function () {
        this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtGestionMoroso').setValue('');
        this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxTipoGestion').clearValue();
        this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxClaseGestion').clearValue();
        this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxDClaseGestion').clearValue();
        this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('dtpFechaPromesa').setValue(null);
        this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtMonto').setValue('0');
        this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtObservacion').setValue('');
    },

    fnEstadoForm: function (cadena) {
        //pnl2.getComponent('grdDetalle').getDockedItems('toolbar[dock="top"]')[0].getComponent('btnAgregar').setDisabled(true);
        this.getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnNuevo').setDisabled(true);
        this.getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnEditar').setDisabled(true);
        this.getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnGuardar').setDisabled(true);
        this.getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnCancelar').setDisabled(true);
        this.getComponent('pnlBusqueda').getComponent('grdMoroso').setDisabled(true);
        this.getComponent('pnlBusqueda').getComponent('grdDetalleMoroso').setDisabled(true);
        this.getComponent('pnlContenido').getComponent('grdGestion').setDisabled(true);
        this.fnBloquearControles(true);

        switch (cadena) {
            case 'visualizacion':
                this.getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnNuevo').setDisabled(false);
                this.getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnEditar').setDisabled(false);
                this.getComponent('pnlBusqueda').getComponent('grdMoroso').setDisabled(false);
                this.getComponent('pnlBusqueda').getComponent('grdDetalleMoroso').setDisabled(false);
                this.getComponent('pnlContenido').getComponent('grdGestion').setDisabled(false);
                this.fnBloquearControles(true);
                break;
            case 'registro':
                this.getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnGuardar').setDisabled(false);
                this.getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnCancelar').setDisabled(false);
                this.fnBloquearControles(false);
                break;
            default:
        }
    }
});