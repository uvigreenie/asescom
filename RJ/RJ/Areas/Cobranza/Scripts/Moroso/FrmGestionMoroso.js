Ext.define('CobApp.Moroso.FrmGestionMoroso', {
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
        /**/
        /**/
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

        var stTramo = Ext.create('Ext.data.Store', {
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cartera/ListarTramo',
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
                url: '../../Cobranza/Cartera/ListarDepartamento',
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
            model: Ext.define('Moroso', {extend: 'Ext.data.Model'}),
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cartera/ListarMorososEnCartera',
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
            fields: [
                { name: 'IDServicio', type: 'int' },
                { name: 'TipoDocumento', type: 'string' },
                { name: 'NumeroDocumento', type: 'string' },
                { name: 'FechaEmision', type: 'string' },
                { name: 'FechaVencimiento', type: 'string' },
                { name: 'Moneda', type: 'string' },
                { name: 'MontoDeuda', type: 'float' },
                { name: 'MontoPagado', type: 'float' },
                { name: 'FechaPago', type: 'string' }
            ],
            proxy: {
                type: 'ajax',
                url: '../../Cobranza/Cartera/ListarServicio',
                reader: {
                    type: 'json'
                },
                params: {
                    detalleCartera: 0
                }
            },
            autoLoad: false
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
                title: 'Lista de Morosos',
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
                    header: false,
                    store: stMoroso,
                    columnLines: true,
                    emptyText: 'No se encontraron datos.',
                    flex: 1,
                    columns: [],
                    features: [{
                        ftype: 'filters',
                        autoReload: false,
                        local: true,
                        filters: [ { type: 'string', dataIndex: 'Cluster'},
                            { type: 'string', dataIndex: 'Departamento'},
                            { type: 'string', dataIndex: 'NumeroDocumento' },
                            { type: 'string', dataIndex: 'CodCliente' },
                            { type: 'string', dataIndex: 'Cuenta' },
                            { type: 'string', dataIndex: 'Servicio' },
                            { type: 'string', dataIndex: 'DMoroso' },
                            { type: 'numeric', dataIndex: 'DeudaTotal' },
                            { type: 'numeric', dataIndex: 'PagoTotal' },
                            { type: 'numeric', dataIndex: 'Saldo' },
                            { type: 'boolean', dataIndex: 'Gestionado', defaultValue: null, yesText: 'Si', noText:'No'},
                            { type: 'boolean', dataIndex: 'Contactado', defaultValue: null, yesText: 'Si', noText:'No'},
                            { type: 'boolean', dataIndex: 'PromesaPago', defaultValue: null, yesText: 'Si', noText:'No'},
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
                    columns: [
                        {
                            xtype: 'rownumberer'
                        },
                        {
                            xtype: 'numbercolumn',
                            dataIndex: 'IDServicio',
                            hidden: true,
                            hideable: false
                        },
                        {
                            dataIndex: 'TipoDocumento',
                            text: 'T. Documento',
                            width: 100,
                            hideable: false
                        },
                        {
                            dataIndex: 'NumeroDocumento',
                            text: 'N° Documento',
                            width: 100,
                            hideable: false
                        },
                        {
                            xtype: 'datecolumn',
                            dataIndex: 'FechaEmision',
                            text: 'Emisión',
                            width: 90,
                            renderer: function (v) {
                                var dateObject = Ext.Date.parse(v, 'MS');
                                var ymdString = Ext.util.Format.date(dateObject, 'd/m/Y');
                                return ymdString;
                            },
                            hideable: false
                        },
                        {
                            xtype: 'datecolumn',
                            dataIndex: 'FechaVencimiento',
                            text: 'Vencimiento',
                            width: 90,
                            renderer: function (v) {
                                var dateObject = Ext.Date.parse(v, 'MS');
                                var ymdString = Ext.util.Format.date(dateObject, 'd/m/Y');
                                return ymdString;
                            },
                            hideable: false
                        },
                        {
                            dataIndex: 'Moneda',
                            text: 'Moneda',
                            width: 80,
                            hideable: false
                        },
                        {
                            xtype: 'numbercolumn',
                            dataIndex: 'MontoDeuda',
                            text: 'Deuda',
                            format: '0,000.##',
                            width: 80,
                            hideable: false
                        },
                        {
                            xtype: 'numbercolumn',
                            dataIndex: 'MontoPagado',
                            text: 'Monto Pagado',
                            format: '0,000.##',
                            width: 80,
                            hideable: false
                        },
                        {
                            xtype: 'datecolumn',
                            dataIndex: 'FechaPago',
                            text: 'Ult.Fecha Pago',
                            width: 90,
                            renderer: function (v) {
                                var dateObject = Ext.Date.parse(v, 'MS');
                                var ymdString = Ext.util.Format.date(dateObject, 'd/m/Y');
                                return ymdString;
                            },
                            hideable: false
                        },
                        {
                            flex: 1,
                            menuDisabled: true,
                            hideable: false
                        }
                    ]
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
                            width: 150,
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
                            width: 150,
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
                            width: 150,
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
                            width: 100,
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
                        bodyStyle: 'padding:15px;',
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
                            fieldLabel: 'N° Gestión'
                        },
                        {
                            xtype: 'textfield',
                            itemId: 'txtDetalleMoroso',
                            fieldLabel: 'DetalleMoroso',
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
                            width: 520
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
                            width: 520,
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
                            width: 520
                        },
                        {
                            xtype: 'datefield',
                            itemId: 'dtpFechaGestion',
                            format: 'd/m/Y',
                            fieldLabel: 'Fecha Gestión',
                            value: new Date()
                        },
                        {
                            xtype: 'datefield',
                            itemId: 'dtpFechaPromesa',
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
                            width: '100%',
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
                width: 350,
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
                        itemId: 'cbxTramo',
                        lastQuery: '',
                        fieldLabel: 'Tramo',
                        emptyText: '< Seleccione >',
                        store: stTramo,
                        displayField: 'Tramo',
                        valueField: 'Tramo',
                        allowBlank: false,
                        forceSelection: true,
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
                this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxTrabajador').getStore().load();
                this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxClaseGestion').getStore().load();
                this.fnEstadoForm('visualizacion');
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
    },

    oncbxGestionClienteSelect: function (combo, records, eOpts) {
        this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getStore().load({
            params: {
                gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue())
            }
        });
    },

    oncbxFechaFinSelect: function (combo, records, eOpts) {
        this.getComponent('pnlFiltro').getComponent('cbxTramo').getStore().load({
            params: {
                gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue()),
                fechaFin: this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getValue()
            }
        });
    },

    oncbxTramoSelect: function (combo, records, eOpts) {
        this.getComponent('pnlFiltro').getComponent('cbxCluster').getStore().load({
            params: {
                gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue()),
                fechaFin: this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getValue(),
                tramo: this.getComponent('pnlFiltro').getComponent('cbxTramo').getValue()
            }
        });
    },

    oncbxClusterSelect: function (combo, records, eOpts) {
        var datos = [];
        if (this.getComponent('pnlFiltro').getComponent('cbxCluster').getValue() != null) {
            datos = this.getComponent('pnlFiltro').getComponent('cbxCluster').getValue();
        }
        this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getStore().load({
            params: {
                gestionCliente: parseInt(this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue()),
                fechaFin: this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getValue(),
                tramo: this.getComponent('pnlFiltro').getComponent('cbxTramo').getValue(),
                clusters: datos
            }
        });
    },

    oncbxClaseGestionSelect: function (combo, records, eOpts) {
        if ( records[0].get('AplicaPromesa').toString() == 'true' ) {
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('dtpFechaPromesa').setDisabled(false);
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtMonto').setDisabled(false);
        }
        else{
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
        if (selections.length > 0) {
            this.getComponent('pnlBusqueda').getComponent('grdServicio').getStore().load({
                params: {
                    detalleCartera: parseInt(selections[0].get('DetalleCartera'))
                }
            });
            this.getComponent('pnlBusqueda').getComponent('grdDetalleMoroso').getStore().load({
                params: {
                    moroso: parseInt(selections[0].get('Moroso'))
                }
            });
        }
            console.log(Ext.encode(this.onBtnBuscarClick));
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
        if (this.fnEsValidoBuscar()) {
            var dtCluster = [];
            var dtDepartamento = [];

            if (this.getComponent('pnlFiltro').getComponent('cbxCluster').getValue() != null) {
                dtCluster = this.getComponent('pnlFiltro').getComponent('cbxCluster').getValue();
            }

            if (this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getValue() != null) {
                dtDepartamento = this.getComponent('pnlFiltro').getComponent('cbxDepartamento').getValue();
            }

            this.getComponent('pnlBusqueda').getComponent('grdMoroso').getStore().load({
                params: {
                    cliente: this.getComponent('pnlFiltro').getComponent('cbxCliente').getValue().toString(),
                    gestionCliente: this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue().toString(),
                    fechaFin: this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getValue(),
                    tramo: this.getComponent('pnlFiltro').getComponent('cbxTramo').getValue(),
                    cluster: dtCluster,
                    departamento: dtDepartamento
                }
            });

            this.getDockedItems('toolbar[dock="bottom"]')[0].getComponent('btnExportar').setParams({
                    cliente: this.getComponent('pnlFiltro').getComponent('cbxCliente').getValue().toString(),
                    gestionCliente: this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').getValue().toString(),
                    fechaFin: this.getComponent('pnlFiltro').getComponent('cbxFechaFin').getValue(),
                    tramo: this.getComponent('pnlFiltro').getComponent('cbxTramo').getValue(),
                    cluster: dtCluster,
                    departamento: dtDepartamento});
            }
    },

    onBtnNuevoClick: function (button, e, options) {    
        if ( this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtDetalleMoroso').getValue().toString() == '' ){
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
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxTrabajador').setValue(this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxTrabajador').store.getAt(0).get('Trabajador'));
            this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxTipoGestion').setValue(this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('cbxTipoGestion').store.getAt(0).get('TipoGestion'));
            this.fnEstadoForm('registro');
        }
    },

    onBtnEditarClick: function (button, e, options) {
        if ( this.getComponent('pnlContenido').getComponent('pnlRegistro').getComponent('txtGestionMoroso').getValue().toString() == '' ) {
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
                            Ext.example.msg('Información', 'Se registro con exitó');
                        }
                        else {
                            Ext.example.msg('Información', 'Actualización realizada con exitó');
                        }
                        //this.onBtnActualizarClick(null, null, null);
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

    fnEsValidoBuscar: function () {
        if (!this.getComponent('pnlFiltro').getComponent('cbxCliente').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlFiltro').getComponent('cbxGestionCliente').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlFiltro').getComponent('cbxCluster').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlFiltro').getComponent('cbxTramo').isValid()) {
            return false;
        }
        if (!this.getComponent('pnlFiltro').getComponent('cbxDepartamento').isValid()) {
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
    },

});