Ext.Loader.setConfig({ enabled: true, disableCaching: false });
Ext.Loader.setPath('Ext.ux', '/Scripts/extjs/ux');
Ext.require([
    'Ext.ux.grid.FiltersFeature', 'Ext.ux.RowExpander'
]);

Ext.serviceBase = '/DataService/';


Ext.decode = function (json, safe) {
    json = json.replace(/\"\\\/Date\((-?\d+)\)\\\/\"/gi, "new Date($1)");
    return Ext.JSON.decode(json, safe);
};

Ext.define('ContractModel', {
    extend:'Ext.data.Model',
    fields: ['Id', 'Name', 'StartDate', 'EndDate', 'Trader', 'BillingCurrency', 'TimeZone', {
        name: 'TimeZone', convert: function (v, r) { return (r.data.TimeZone != null ? r.data.TimeZone.Id : '00') + ':00 PST'; }
    }, {
        name: 'Account', convert: function (v, r) { return r.data.Trader.Account; }
    }, {
        name: 'TraderName', convert: function (v, r) { return r.data.Trader.Name; }
    }, {
        name: 'CurrencyId', convert: function (v, r) { return r.data.BillingCurrency.Id; }
    }, {
        name: 'Currency', convert: function (v, r) { return r.data.BillingCurrency.Name; }
    }]
});

Ext.define('ItemModel', {
    extend: 'Ext.data.Model',
    fields: [
        'ItemId',
        'Category',
        'SubCategory',
        'Item',
        'Unit',
        'Units', 
        'Instructions', 
        { name: 'PricePerUnit', type: 'float' },
        'CurrencyId',
        { name: 'Amount', type: 'float' }
    ]
});
Ext.define('OrderModel', {
    extend: 'Ext.data.Model',
    associations: [{
        type: 'hasOne', model: 'ContractModel', name:'Contract'
    }, {
        type: 'hasMany', model: 'ItemModel', name: 'LineItems', storeConfig: { sorters: [{ property: 'Item', direction: 'ASC' }] }
    }],
    fields: [
        'Id',
        'OrderDate',
        'QuotationDate',
        'ApprovalDate',
        'FulfilmentDate',
        'Status',
        { name: 'Amount', type: 'float' },
        { name: 'AdminFee', type: 'float' },
        'TakeoffAirport',
        'LandingAirport',
        'Aircraft',
        'TakeoffAirportId',
        'LandingAirportId',
        'AircraftId',
        'Operator',
        'FlightNumber',
        'EstimatedTakeoffTime',
        'EstimatedLandingTime',
        'BuyerId',
        'Buyer',
        'SellerId',
        'Seller',
        'IsSeller',
        'IsViewed',
        'IsQuoted',
        'IsApproved',
        'IsFulfilled',
        'Contract',
        'LineItems',
        { name: 'TraderType', convert: function (v, r) { return r.data.IsSeller ? 'As Seller' : 'As Buyer'; } },
        { name: 'ItemCount', convert: function (v, r) { return r.data.LineItems.length; } },
        { name: 'WeekName', convert: function (v, r) { return Ext.Date.getWeekOfYear(r.data.OrderDate); } }
    ]
});

Ext.showInstructions = function (n, i) {
    if (i == '') i = 'No instructions available';
    Ext.Msg.alert('AviTrade', '<b>Instructions for ' + n+ '</b><br/>' + i);
}

Ext.define('AviTrader.OrdersList', {
    grid: null,
    store: null,
    filterBar: null,

    constructor: function () {
        var me = this, monthButtons = [], baseDate = new Date(2012,7,4);

        for (var i = 0; i < 12; i++) {
            monthButtons.push({
                xtype: 'button',
                text: Ext.util.Format.date(Ext.Date.add(baseDate, Ext.Date.MONTH, i * -1), 'M Y'),
                filterDate: Ext.Date.add(baseDate, Ext.Date.MONTH, i * -1),
                pressed: i == 0,
                enableToggle: true,
                toggleGroup: 'months',
                handler: function () {
                    if (!this.pressed) {
                        this.toggle(true);
                        return;
                    }
                    me.store.proxy.extraParams.filterByOrderDate = this.filterDate;
                    me.store.currentDate = this.text;
                    me.store.load();
                }
            });
        }
        monthButtons.push('->', '-', {
            xtype: 'button', text: 'Create new order', iconCls: 'icon-new-order', handler: function () {
                new AviTrader.CreateOrder({reload:function(){me.reloadGrid();}});
            }
        });

        this.store = new Ext.data.Store({
            model: 'OrderModel',
            groupField: 'Status',
            currentDate: Ext.util.Format.date(baseDate, 'M Y'),
            proxy: {
                type: 'ajax',
                url: Ext.serviceBase + 'RetrieveOrders',
                extraParams: { traderId: currentTraderId, filterByOrderDate: baseDate },
                reader: {
                    type:'json',
                    root: 'Items',
                    totalProperty: 'Count'
                }
            },
            listeners: {
                beforeload: function () {
                    me.grid.setLoading('Loading orders for "' + this.currentDate + '"....');
                },
                load: function () {
                    me.grid.filters.clearFilters();
                    me.grid.setLoading(false);
                    me.grid.setTitle('Currently viewing orders for "' + this.currentDate + '"');
                }
            }
        });

        this.filterBar = new Ext.widget({
            xtype:'toolbar',
            dock: 'bottom',
            enableOverflow: true,
            items: [{
                xtype: 'button', text: 'Clear Filters', handler: function () { me.grid.filters.clearFilters(); }
            }, '-'],
            defaults: {
                margin: '1 2'
            }
        });

        this.grid = Ext.widget({
            xtype: 'grid',
            frame: true,
            padding: 0,
            border: false,
            store: this.store,
            title: 'Currently viewing orders for "' + Ext.util.Format.date(baseDate, 'M Y') + '"',
            features: [new Ext.ux.grid.FiltersFeature({
                local: true
            }),
            new Ext.grid.feature.Grouping({
                groupHeaderTpl: '{columnName}: {name} ({rows.length} Item{[values.rows.length > 1 ? "s" : ""]})',
                hideGroupedHeader: true
            })],
            scroll: false,
            viewConfig: {
                style: { overflow: 'auto', overflowX: 'hidden' },
                listeners: {
                    scope:this,
                    expandbody: this.viewOrder
                }
            },
            plugins: [{
                ptype: 'rowexpander',
                expandOnDblClick: false,
                rowBodyTpl: [
                    '<table width="920px"><tr><td>',
                    '<table class="order-info">',
                    '<tr><td><label>Buyer:</label></td><td>{Buyer}</td><td><label>Seller:</label></td><td>{Seller}</td></tr>',
                    '<tr><td colspan="4"><hr/></td></tr>',
                    '<tr><td><label>Flight Number:</label></td><td>{FlightNumber}</td><td><label>Aircraft:</label></td><td>{Aircraft}</td></tr>',
                    '<tr><td><label>Takeoff:</label></td><td colspan="3">{TakeoffAirport} - {EstimatedTakeoffTime:date("D, M d, Y g:i:s A")}</td></tr>',
                    '<tr><td><label>Landing:</label></td><td colspan="3">{LandingAirport} - {EstimatedLandingTime:date("D, M d, Y g:i:s A")}</td></tr>',
                    '<tr><td colspan="4"><hr/></td></tr>',
                    '<tr><td><label>Order Date:</label></td><td>{OrderDate:date("d M Y")}</td><td><label>Quotation Date:</label></td><td>{QuotationDate:date("d M Y")}</td></tr>',
                    '<tr><td><label>Approval Date:</label></td><td>{ApprovalDate:date("d M Y")}</td><td><label>Fulfillment Date:</label></td><td>{FulfilmentDate:date("d M Y")}</td></tr>',
                    '<tr><td colspan="4"><hr/></td></tr>',
                    '<tr><td colspan="4">',
                    '<table class="order-info" style="margin-left:20px">',
                    '<tr><th>Item</th><th>Quantity</th><th>Unit</th><th width="22px"/><th class="right">Price</th>',
                    '<tpl for="LineItems">',
                    '<tr><td>{Item}</td><td align="center">{Units}</td><td>{Unit}</td>',
                    '<td><button class="icon-info" style="height:22px;border:none;cursor:pointer;" title="Click to view instructions" onclick="Ext.showInstructions(\'{Item}\',\'{Instructions}\')"></button></td>',
                    '<td align="right">{Amount:currency(" ")} {CurrencyId}</td></tr>',
                    '</tpl>',
                    '</table>',
                    '</td></tr></table>',
                    '</td><td valign="top"><div id="orderActions_{Id}" align="right"></div></td></tr></table>'
                ]
            }],
            columns: [{
                id: 'colIcon',
                width: 30,
                resizable: false,
                hideable: false,
                dataIndex: 'IsViewed',
                filter: {
                    type: 'boolean',
                    defaultValue: null,
                    yesText: 'Read',
                    noText: 'Unread'
                },
                renderer: function (v, m, r) {
                    m.tdCls = v ? 'icon-read' : 'icon-unread';
                }
                /*}, {
                    id: 'colWeek',
                    dataIndex:'WeekName'*/
            }, {
                id: 'colType',
                width: 80,
                resizable: false,
                text: 'Trader Type',
                dataIndex: 'TraderType',
                align: 'center',
                filter: {
                    type: 'list',
                    store: new Ext.data.Store({
                        fields: ['id', 'text'],
                        data: [{
                            id: 'As Seller', text: 'As Seller'
                        }, {
                            id: 'As Buyer', text: 'As Buyer'
                        }]
                    })
                }
            }, {
                xtype: 'datecolumn',
                width: 80,
                resizable: false,
                groupable: false,
                renderer: Ext.util.Format.dateRenderer('d M Y'),
                text: 'Order Date',
                align: 'center',
                filter: {
                    type: 'date'
                },
                dataIndex: 'OrderDate'
            }, {
                id: 'colBuyer',
                flex: 1,
                width: 60,
                text: 'Buyer',
                filter: true,
                dataIndex: 'Buyer'
            }, {
                id: 'colSeller',
                flex: 1,
                width: 60,
                text: 'Seller',
                filter: true,
                dataIndex: 'Seller'
            }, {
                id: 'colStatus',
                width: 80,
                resizable: false,
                text: 'Status',
                align: 'center',
                dataIndex: 'Status',
                filter: {
                    type: 'list',
                    store: new Ext.data.Store({
                        fields: ['id', 'text'],
                        data: [{
                            id: 'Submitted', text: 'Submitted'
                        }, {
                            id: 'Quoted', text: 'Quoted'
                        }, {
                            id: 'Approved', text: 'Approved'
                        }, {
                            id: 'Ignored', text: 'Ignored'
                        }, {
                            id: 'Fulfilled', text: 'Fulfilled'
                        }]
                    })
                }
            }, {
                align: 'center',
                width: 60,
                groupable: false,
                resizable: false,
                text: 'Items',
                id: 'colItems',
                dataIndex: 'ItemCount'
            }, {
                id: 'colPrice',
                align: 'right',
                groupable: false,
                resizable: false,
                width: 140,
                xtype: 'numbercolumn',
                format: '0,000.00',
                text: 'Amount',
                filter: {
                    type: 'numeric'
                },
                dataIndex: 'Amount'
            }, {
                id: 'colSpacer', width: 20, dataIndex: 'none', resizable: false, hideable: false, menuDisabled: true, sortable: false, groupable: false
            }],
            dockedItems: [{
                xtype: 'toolbar',
                dock: 'top',
                items: monthButtons,
                defaults: {
                    margin: '1 3'
                }
            }, this.filterBar],

            listeners: {
                scope: this,
                filterupdate: this.buildFiltersList,
                itemdblclick: this.openOrderDialog
            },

            width: 970,
            height: 450,
            renderTo: $('.order-listing')[0]
        });

        this.store.load();
    },

    reloadGrid:function() {
        this.store.load();
    },

    openOrderDialog: function (grid, rec) {
        if (!rec.get('IsQuoted') && !rec.get('IsSeller')) new AviTrader.CreateOrder({ record: rec });
        else if (!rec.get('IsQuoted') && rec.get('IsSeller')) new AviTrader.CreateOrder({ record: rec, quote: true });
        else new AviTrader.ViewOrder({ record: rec });
    },

    buildFiltersList: function () {
        var me = this;

        this.filterBar.removeAll();
        var items = [{
            xtype: 'button', text: 'Clear Filters', handler: function () { me.grid.filters.clearFilters(); }
        }, '-'];

        Ext.Array.each(this.grid.filters.getFilterData(), function (i, idx) {
            var lbl = '';

            switch (i.field) {
                case 'IsViewed':
                    lbl = i.data.value ? 'Read' : 'Unread';
                    break;
                default:
                    lbl = i.field + ' ';
            }
            if (i.data.type == 'date') {
                if (i.data.comparison == 'lt') lbl += 'before ' + i.data.value;
                if (i.data.comparison == 'gt') lbl += 'after ' + i.data.value;
                if (i.data.comparison == 'eq') lbl += 'on ' + i.data.value;
            }
            else if (i.data.type == 'numeric') {
                if (i.data.comparison == 'lt') lbl += '< ' + i.data.value;
                if (i.data.comparison == 'gt') lbl += '> ' + i.data.value;
                if (i.data.comparison == 'eq') lbl += '= ' + i.data.value;
            }
            else if (i.data.type == 'string') lbl += 'like \'' + i.data.value + '\'';
            else if (i.data.type != 'boolean') lbl += i.data.value;

            items.push({
                xtype: 'button', iconCls: 'icon-remove', text: lbl, filterIndex: i.field, handler: function () { me.grid.filters.getFilter(this.filterIndex).setActive(false); }
            });
        });
        this.filterBar.add(items);
    },

    viewOrder: function (row, rec, body) {
        rec.set('IsViewed', true);

        var me =this, items = [];
        if (!rec.get('IsQuoted')) {
            if (!rec.get('IsSeller')) items.push({
                xtype: 'button', text: 'Edit Order', order: rec, handler: function () {
                    me.editOrder(this.order);
                }
            });
            if (rec.get('IsSeller')) items.push({
                xtype: 'button', text: 'Quote Order', order: rec, handler: function () {
                    me.quoteOrder(this.order);
                }
            });
        }
        if (rec.get('IsSeller') && rec.get('IsQuoted') && !rec.get('IsApproved')) {
            items.push({
                xtype: 'button', text: 'Approve Order', orderId: rec.get('Id'), handler: function () {
                    me.approveOrder(this.orderId);
                }
            });
            items.push({
                xtype: 'button', text: 'Ignore Order', orderId: rec.get('Id'), handler: function () {
                    me.ignoreOrder(this.orderId);
                }
            });
        }
        if (rec.get('IsApproved')) {
            items.push({
                xtype: 'button', text: 'View Invoice', orderId: rec.get('Id'), handler: function () {
                    me.viewInvoice(this.orderId);
                }
            });
        }

        if (items.length > 0) {
            $('#orderActions_' + rec.get('Id')).children().remove();
            Ext.widget({
                xtype: 'container',
                layout: { type: 'hbox', pack: 'end' },
                defaults: { margin: '5' },
                items: items,
                width: 300,
                renderTo: $('#orderActions_' + rec.get('Id'))[0]
            });
        }
    },

    ajaxError: function () {
        this.grid.setLoading(false);
        Ext.Msg.alert('AviTrade', 'Ajax error occured.');
    },

    editOrder: function (order) {
        var me = this;
        new AviTrader.CreateOrder({ record: order,reload:function(){me.reloadGrid();} });
    },

    quoteOrder: function (order) {
        var me = this;
        new AviTrader.CreateOrder({ record: order, quote: true, reload: function () { me.reloadGrid(); } });
    },

    approveOrder: function (orderId) {
        var me = this;
        this.grid.setLoading('Approving order....');
        $.ajax(Ext.serviceBase + 'ApproveOrder', {
            type: 'POST', traditional: false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ traderId: currentTraderId, orderId: orderId, approveDate: Ext.Date.format(new Date(), 'Y-m-d') }),
            success: function (data, status, resp) {
                me.grid.setLoading(false);
                me.reloadGrid();
            }
        }).error(function () { me.ajaxError(); });
    },

    ignoreOrder: function (orderId) {
    },

    viewInvoice: function (orderId) {
        var me = this;
        me.grid.setLoading('Retrieving invoice details...');
        $.getJSON(Ext.serviceBase + 'RetrieveOrderInvoice', { orderId: orderId }, function (data) {
            var html = '';

            html = '<table class="order-info">' +
                '<tr><td><label>Invoice Date:</label></td><td>' + Ext.Date.format(eval(data.CreateDate.replace(/\/Date\((-?\d+)\)\//gi, "new Date($1)")), 'd M Y') + '</td>' +
                '<td><label>Invoice No.:</label></td><td>' + data.InvoiceNumber + '</td></tr>' +
                '</tr><td><label>Buyer:</label></td><td>' + data.Buyer + '</td><td><label>Seller:</label></td><td>' + data.Seller + '</td></tr>' +
                '<tr><td colspan="4"><hr/></td></tr>' +
                '<tr><td colspan="4" class="right"><table align="right">'+
                '<tr><td><label>Amount:</label></td><td class="right">' + Ext.util.Format.number(data.Amount, '0,000.00') + '</td></tr>' +
                '<tr><td><label>Tax:</label></td><td class="right">' + Ext.util.Format.number(data.Tax, '0,000.00') + '</td></tr>' +
                '<tr><td><label>Admin Fee:</label></td><td class="right">' + Ext.util.Format.number(data.AdminFee, '0,000.00') + '</td></tr>' +
                '<tr><td colspan="2"><hr/></td></tr>' +
                '<tr><td><label>Total:</label></td><td class="right">' + Ext.util.Format.number(data.Total, '0,000.00') + '</td></tr>' +
                '</table></td></tr></table>';

            Ext.widget({
                xtype: 'window',
                title: 'Order Invoice',
                resizable: false,
                modal: true,
                items: [{
                    xtype: 'container', html: html
                }]
            }).show();
            me.grid.setLoading(false);
        }).error(function () {
            me.grid.setLoading(false);
            Ext.Msg.alert('AviTrade', 'Ajax error occured.');
        });
    }
});

Ext.define('AviTrader.CreateOrder', {
    wnd: null,
    btnPrev: null,
    btnNext: null,
    btnPreset: null,
    btnCreate: null,

    contracts: [],
    itemCategories: null,

    currencies: [],
    currencyStore: null,

    contractStore: null,
    selectedContract: null,

    orderSetting:null,

    airports: [],
    aircarfts: [],

    record: null,
    quote: false,

    itemsAdded:[],

    itemTree:null,
    formContract: null,
    formItems: null,
    formFlight: null,
    panelSummary: null,

    showDetailBar:false,

    reloadGrid:null,

    constructor: function (cfg) {
        var me = this;

        this.itemsAdded = [];
        this.selectedContract = null;

        this.contracts = [];
        this.currencies = [];

        this.record = cfg.record;
        this.quote = cfg.quote == true;

        this.reloadGrid=cfg.reload;

        this.showDetailBar = this.quote || cfg.orderId != null;

        this.wnd = new Ext.Window({
            modal: true,
            resizable: false,
            layout: 'card',
            bodyBorder: false,
            title: me.quote ? 'Quote Order' : (cfg.orderId ? 'Edit Order':'Create Order'),
            width: 800, height: 600,
            dockedItems: [{
                xtype: 'container',
                dock: 'bottom',
                defaults: { margin: '2 5' },
                layout: { type: 'hbox', pack: 'end' },
                items: [this.btnPrev = Ext.widget({
                    xtype: 'button', text: 'Previous', hidden: true, handler: function () { me.navigate('prev'); }
                }), this.btnNext = Ext.widget({
                    xtype: 'button', text: 'Next', hidden: me.quote, handler: function () { me.navigate('next'); }
                }), this.btnPreset = Ext.widget({
                    xtype: 'button', text: 'Save as Preset', hidden: (me.quote || me.record == null), handler: function () { me.savePreset(); }
                }), this.btnCreate = Ext.widget({
                    xtype: 'button', text: 'Create Order', hidden: true, handler: function () { me.createOrder(); }
                }), Ext.widget({
                    xtype: 'button', text: 'Save Quote', hidden: !me.quote, handler: function () { me.saveQuote(); }
                }), Ext.widget({
                    xtype: 'button', text: 'Close', handler: function () { me.wnd.destroy(); }
                })]
            }]
        });
        this.wnd.show();
        this.wnd.setLoading('Retrieving order items...');

        if (cfg.orderId) {
            $.getJSON(Ext.serviceBase + 'RetrieveOrder', { traderId: currentTraderId, orderId: cfg.orderId }, function (data, status, resp) {
                me.record = new OrderModel(Ext.decode(resp.responseText));
                me.preload();
            }).error(function () { me.ajaxError(); });
        }
        else {
            this.preload();
        }
    },

    ajaxError: function () {
        this.wnd.setLoading(false);
        Ext.Msg.alert('AviTrade', 'Ajax error occured.');
    },

    preload: function () {
        var me = this;

        if (this.record) {
            this.selectedContract = new ContractModel(this.record.get('Contract'));
        }
        if (!this.quote && this.record) this.btnCreate.setText('Update Order');

        if (this.quote) {
            var html = '';
            html = '<table width="100%" class="order-info">' +
                    '<tr><td width="120px"><label>Account:</label></td><td>' + me.selectedContract.get('Account') + '</td><td width="120px"><label>Buyer:</label></td><td>' + me.selectedContract.get('TraderName') + '</td></tr>' +
                    '<tr><td width="120px"><label>Contract Start:</label></td><td>' + Ext.Date.format(me.selectedContract.get('StartDate'), 'd M Y') + '</td><td width="120px"><label>Contract End:</label></td><td>' + Ext.Date.format(me.selectedContract.get('EndDate'), 'd M Y') + '</td></tr>' +
                    '<tr><td width="120px"><label>Billing Currency:</label></td><td>' + me.selectedContract.get('Currency') + '</td><td width="120px"><label>TimeZone:</label></td><td>' + me.selectedContract.get('TimeZone') + '</td></tr>' +
                    '</table><hr/>' +
                    '<table width="" class="order-info">' +
                    '<tr><td width="120px"><label>Flight Number:</label></td><td>' + me.record.get('FlightNumber') + '</td><td width="120px"><label>Aircraft:</label></td><td>' + me.record.get('Aircraft') + '</td></tr>' +
                    '<tr><td width="120px"><label>Takeoff:</label></td><td colspan="3">' + me.record.get('TakeoffAirport') + ' - ' + Ext.Date.format(me.record.get('EstimatedTakeoffTime'), 'D, M d, Y g:i:s A') + '</td></tr>' +
                    '<tr><td width="120px"><label>Landing:</label></td><td colspan="3">' + me.record.get('LandingAirport') + ' - ' + Ext.Date.format(me.record.get('EstimatedLandingTime'), 'D, M d, Y g:i:s A') + '</td></tr>' +
                    '</table>';
            this.wnd.addDocked({
                xtype: 'container', html: html, dock: 'top'
            });
        }
        else if (this.showDetailBar) {
            var html = '';
            html = '<table width="100%" class="order-info">' +
                    '<tr><td width="120px"><label>Account:</label></td><td>' + me.selectedContract.get('Account') + '</td><td width="120px"><label>Buyer:</label></td><td>' + me.selectedContract.get('TraderName') + '</td></tr>' +
                    '<tr><td width="120px"><label>Contract Start:</label></td><td>' + Ext.Date.format(me.selectedContract.get('StartDate'), 'd M Y') + '</td><td width="120px"><label>Contract End:</label></td><td>' + Ext.Date.format(me.selectedContract.get('EndDate'), 'd M Y') + '</td></tr>' +
                    '<tr><td width="120px"><label>Billing Currency:</label></td><td>' + me.selectedContract.get('Currency') + '</td><td width="120px"><label>TimeZone:</label></td><td>' + me.selectedContract.get('TimeZone') + '</td></tr>' +
                    '</table>';
            this.wnd.addDocked({
                xtype: 'container', html: html, dock: 'top'
            });
        }

        if (!this.quote) {
            $.getJSON(Ext.serviceBase + 'RetrieveContracts', { filterByTraderId: currentTraderId, filterByEndDate: Ext.Date.format(new Date(), 'Y-m-d') }, function (data, status, resp) {
                me.contracts = Ext.decode(resp.responseText);

                $.getJSON(Ext.serviceBase + 'RetrieveItemCategories', function (data) {
                    me.itemCategories = data;

                    $.getJSON(Ext.serviceBase + 'RetrieveCurrencies', function (data) {
                        me.currencies = data;

                        $.getJSON(Ext.serviceBase + 'RetrieveAirports', function (data) {
                            me.airports = data;

                            $.getJSON(Ext.serviceBase + 'RetrieveAircrafts', function (data) {
                                me.aircarfts = data;
                                me.buildWindow();
                                me.wnd.setLoading(false);
                            }).error(function () { me.ajaxError(); });

                        }).error(function () { me.ajaxError(); });

                    }).error(function () { me.ajaxError(); });

                }).error(function () { me.ajaxError(); });

            }).error(function () { me.ajaxError(); });
        }
        else {
            me.buildWindow();
        }
    },

    buildWindow: function () {
        var me = this;

        if (!this.quote && this.record) this.btnNext.setText('Next - Flight Info');
        else this.btnNext.setText('Next - Items');

        this.contractStore = new Ext.data.Store({
            model: 'ContractModel',
            data: me.contracts,
            proxy: 'memory'
        });

        me.currencyStore = new Ext.data.Store({
            fields: ['Id', 'Name'],
            data: me.currencies
        });

        me.formContract = Ext.widget({
            xtype: 'container',
            layout: { type: 'vbox', align: 'stretch' },
            defaults: { margin: '10' },
            items: [{
                itemId:'cboContract',
                xtype: 'combobox',
                labelWidth: 100,
                margin:'10 10 0',
                labelAlign: 'right',
                forceSelection: true,
                typeAhead:true,
                store: me.contractStore,
                displayField: 'Name',
                valueField: 'Id',
                emptyText:'Please select a trader contract',
                fieldLabel: 'Trader Contract',
                listConfig: {
                    itemTpl: '<p>{Name}<br/><small><b>Trader</b>: {TraderName}. <b>End Date</b>: {EndDate:date("d M Y")}</small></p>'
                },
                listeners: {
                    change: function (cbo, val) {
                        me.selectedContract = this.store.findRecord('Id', val);
                        var html = '<p style="padding-left:105px"><b>Trader</b>: ' + me.selectedContract.get('TraderName') + '. <b>End Date</b>: ' + Ext.Date.format(me.selectedContract.get('EndDate'), 'd M Y') + '</p>';
                        this.ownerCt.getComponent('divDetails').getEl().setHTML(html);
                    }
                }
            }, { xtype: 'container', itemId:'divDetails', html: '' }, { xtype: 'container', html: '<hr/>' }, {
                xtype: 'radio',
                checked:true,
                name: 'itemType',
                itemId: 'chkBlank',
                boxLabel: 'Create Blank Order',
                handler: function (chk, checked) {
                    if (!checked) return;
                    var grid = chk.ownerCt.getComponent('templateList');
                    grid.getSelectionModel().deselectAll();
                    grid.disable();
                }
            }, {
                xtype: 'radio',
                name: 'itemType',
                itemId:'chkTemplate',
                boxLabel: 'Select Template',
                handler: function (chk, checked) {
                    if (!checked) return;
                    chk.ownerCt.getComponent('templateList').enable();
                }
            }, {
                xtype: 'gridpanel',
                itemId:'templateList',
                flex: 1,
                disabled:true,
                store: new Ext.data.Store({
                    fields: ['TemplateName', 'Items', {
                        name: 'ItemNames', mapping: 'Items', convert: function (v, r) {
                            var names = [];
                            Ext.Array.each(v, function (i) {
                                names.push(i.Name);
                            });
                            return names.toString();
                        }
                    }],
                    data: [
                        { TemplateName: 'UAE Basic', Items: [{ Id: 5, Name: 'UAE Fly Zone', Unit: 'Permit' }, { Id: 2, Name: 'BP Octane 98', Unit: 'Liter' }] },
                        { TemplateName: 'Qatar Basic', Items: [{ Id: 6, Name: 'Qatar Fly Zone', Unit: 'Permit' }, { Id: 1, Name: 'BP Octane 102', Unit: 'Liter' }] }
                    ]
                }),
                columns: [{
                    dataIndex: 'TemplateName',
                    width: 120,
                    text: 'Name',
                    resizable: false,
                    menuDisabled: true
                }, {
                    dataIndex: 'ItemNames',
                    width: 100,
                    flex:1,
                    text: 'Items',
                    resizable: false,
                    menuDisabled: true
                }]
            }]
        });


        var categories = ['<b>Categories</b>']
        if (!this.quote) {
            var initialCat = me.itemCategories[0];
            Ext.Array.each(me.itemCategories, function (i, idx) {
                categories.push({
                    xtype: 'button',
                    text: i,
                    pressed: idx == 0,
                    enableToggle: true,
                    toggleGroup: 'itemCats',
                    handler: function (btn) {
                        me.changeTree(btn.getText());
                    }
                });
            });
        }

        var itemsStore = new Ext.data.TreeStore({
            fields: ['id', 'text', 'expanded', 'leaf', 'items', 'unit', 'desc', 'instruction']
        });

        // Cache the data until further call
        me.itemTree = Ext.widget({
            xtype: 'treepanel',
            region: 'west',
            cls: 'x-tree',
            lines: false,
            collapsible: true,
            title:'Items',
            minWidth: 200,
            maxWidth: 400,
            width: 180,
            split: true,
            rootVisible: false,
            store: itemsStore,
            hidden: me.quote,
            listeners: {
                viewready: function () {
                    $(this.el.dom).find('table').width('100%').find('th').width('100%');
                    this.getView().on('refresh', function () {
                        $(this.el.dom).find('table').width('100%').find('th').width('100%');
                    }, this);
                },
                beforeselect: function (rm, rec) {
                    return rec.get('leaf');
                },
                itemdblclick: function (vw, rec) {
                    me.addLineItem(rec.get('id'), rec.get('text'), rec.get('unit'), me.selectedContract.get('CurrencyId'), rec.get('instruction'));
                }
            },
            lbar: Ext.widget({
                xtype: 'toolbar',
                vertical: true,
                layout: {
                    overflowHandler: 'Menu'
                },
                items: categories
            })
        });

        var headers = [];

        if (this.quote) {
            headers = [{
                xtype: 'container', html: '<p align="center"><b>Item</b></p>', width: 100
            }, {
                xtype: 'container', html: '<b>Qty</b>', width: 50
            }, {
                xtype: 'container', html: '<b>Unit</b>', width: 50
            }, {
                xtype: 'container', html: '', width: 22
            }, {
                xtype: 'container', html: '<b>Price/Unit</b>', width: 120
            }, {
                xtype: 'container', html: '<b>Currency</b>', width: 60
            }, {
                xtype: 'container', html: '<b>Amount</b>', width: 80
            }];
        }
        else {
            headers = [{
                xtype: 'container', html: '<p align="center"><b>Item</b></p>', width: 220
            }, {
                xtype: 'container', html: '<b>Unit</b>', width: 50
            }, {
                xtype: 'container', html: '', width: 22
            }, {
                xtype: 'container', html: '<b>Currency</b>', width: 100
            }, {
                xtype: 'container', html: '<b> </b>', width: 80
            }];
        }

        me.formItems = Ext.widget({
            xtype: 'form',
            border:false,
            layout: 'border',
            items: [me.itemTree, {
                itemId:'itemForm',
                xtype: 'form',
                autoScroll: true,
                border:true,
                region: 'center',
                style: { background: 'white' },
                layout: { type: 'table', columns: 1, tableAttrs: { align:'center', style: { 'border-collapse': 'separate', 'border-spacing': '10px 0px' } }, tdAttrs: { style: { 'vertical-align': 'top' } } },
                items: [{
                    xtype: 'container',
                    defaults: { margin: '5' },
                    layout: { type: 'table', columns: headers.length, tdAttrs: { style: { 'vertical-align': 'top' } } },
                    items: headers
                }, {
                    xtype: 'container', html: '<hr/>'
                }]
            }]
        });

        var airportStore = new Ext.data.Store({
            fields: ['Id', 'Name', 'City', { name: 'DisplayText', mapping:'Name', convert: function (v, r) { return r.data.Name + ' (' + r.data.City + ')' } }],
            data:me.airports
        });

        this.formFlight = Ext.widget({
            xtype: 'form',
            padding: '5',
            border:false,
            style: { background: 'white' },
            layout: { type: 'table', columns: 2, tableAttrs: { align:'center', style: { 'border-collapse': 'separate', 'border-spacing': '10px 0px' } }, tdAttrs: { style: { 'vertical-align': 'top' } } },
            defaults: { labelWidth: 120, fieldWidth:100, labelAlign:'right' },
            items: [{
                xtype: 'textfield',
                itemId:'flightNo',
                fieldLabel: 'Flight Number',
                value: this.record ? this.record.get('FlightNumber') : null
            }, {
                xtype: 'combobox',
                store: new Ext.data.Store({
                    fields: ['Id', 'Manufacturer', 'Model', { name: 'DisplayText', mapping: 'Model', convert: function (v, r) { return r.data.Manufacturer + ' (' + r.data.Model + ')' } }],
                    data:me.aircarfts
                }),
                width: 350,
                itemId:'aircraft',
                typeAhead: true,
                valueField:'Id',
                displayField: 'DisplayText',
                fieldLabel: 'Aircraft',
                value: this.record ? this.record.get('AircraftId') : null
            }, {
                xtype: 'combobox',
                store: airportStore,
                width: 350,
                typeAhead: true,
                itemId:'takeoffAirport',
                valueField: 'Id',
                displayField: 'DisplayText',
                fieldLabel: 'Takeoff Airport',
                value: this.record ? this.record.get('TakeoffAirportId') : null
            }, {
                xtype: 'combobox',
                store: airportStore,
                width: 350,
                typeAhead: true,
                itemId:'landingAirport',
                valueField: 'Id',
                displayField: 'DisplayText',
                fieldLabel: 'Landing Airport',
                value: this.record ? this.record.get('LandingAirportId') : null
            }, {
                xtype: 'datefield',
                format: 'd M Y',
                editable:false,
                minValue: new Date(),
                itemId:'takeoffDate',
                fieldLabel: 'Takeoff Date',
                listeners: {
                    select: function (fld, dt) {
                    }
                },
                value: this.record ? this.record.get('EstimatedTakeoffTime') : null
            }, {
                xtype: 'datefield',
                format:'d M Y',
                editable: false,
                minValue: new Date(),
                itemId:'landingDate',
                fieldLabel: 'Landing Date',
                listeners: {
                    select: function (fld, dt) {
                    }
                },
                value: this.record ? this.record.get('EstimatedLandingTime') : null
            }, {
                xtype: 'container',
                colspan: 2,
                itemId: 'conItems'
            }]
        });

        this.panelSummary = Ext.widget({
            xtype:'container'
        });

        var items = [];
        if (this.record == null) items.push(this.formContract);
        items.push(this.formItems);
        if(!this.quote) items.push(this.formFlight);

        if (this.record) {
            Ext.Array.each(this.record.get('LineItems'), function (i) {
                me.addLineItem(i.ItemId, i.Item, i.Unit, i.CurrencyId, i.Instructions, i.Units, i.PricePerUnit);
            });
        }
        this.wnd.add(items);
        if (!this.quote) this.changeTree(initialCat);

        me.wnd.setLoading(false);
    },

    changeTree: function (cat) {
        var me = this;
        $.getJSON(Ext.serviceBase + 'RetrieveItems?filterByCatagory=' + cat, function (data) {
            var cats = [], cat = null, subCat;

            Ext.Array.each(data, function (i) {
                if (subCat != i.SubCategory) {
                    if (cat != null) cats.push(cat);
                    subCat = i.SubCategory;
                    cat = {
                        text: subCat, expanded: true, leaf: false,
                        children: []
                    };
                }
                cat.children.push({
                    id: i.Id, text: i.Name, unit: i.Unit, desc: i.Description, instruction:'No instructions available', leaf: true
                });
            });
            if (cat != null) cats.push(cat);

            me.itemTree.getStore().getRootNode().removeAll();
            me.itemTree.getStore().getRootNode().appendChild(cats);
        });
    },

    addLineItem: function (id, name, unit, curr, info, qty, price) {
        if (Ext.Array.contains(this.itemsAdded, id)) return;
        this.itemsAdded.push(id);

        var form = this.formItems.getComponent('itemForm');
        var c, me = this;
        if (this.quote) {
            c = Ext.widget({
                xtype: 'container',
                cls: 'lineItem',
                myItem: { id: id, name: name, unit: unit, units: qty, currency: curr },
                defaults: { margin: '5' },
                layout: { type: 'table', columns: 7, tdAttrs: { style: { 'vertical-align': 'top' } } },
                items: [{
                    xtype: 'container', html: name, width: 100
                }, {
                    xtype: 'container', html: qty+'', width: 50
                }, {
                    xtype: 'container', html: unit, width: 50
                }, {
                    xtype: 'button', iconCls: 'icon-info', width: 22, tooltip: 'Click to view instructions', tooltipType:'title', readOnly: true, instructions: info, handler: this.editInstructions, itemName: name
                }, {
                    xtype: 'numberfield', qty: qty, itemId: 'price', allowBlank: false, msgTarget: 'side', minValue: 1, width: 120, blankText: 'Price required', value: price, 
                    listeners: {
                        change: function () {
                            this.nextSibling().nextSibling().getEl().setHTML('<p align="right">' + Ext.util.Format.number(this.getValue() * this.qty, '0,000.00') + '</p>');
                        }
                    }
                }, {
                    xtype: 'container', html: curr, width: 60
                }, {
                    xtype: 'container', html: '', width: 80, cls:'totalPrice'
                }]
            });
        }
        else {
            c = Ext.widget({
                xtype: 'container',
                cls: 'lineItem',
                myItem: { id: id, name: name, unit: unit },
                defaults: { margin: '5' },
                layout: { type: 'table', columns: 5, tdAttrs: { style: { 'vertical-align': 'top' } } },
                items: [{
                    xtype: 'numberfield', itemId: 'qty', allowBlank: false, msgTarget: 'side', minValue: 1, width: 220, labelWidth: 100, labelAlign: 'right', fieldLabel: name, blankText: 'Enter a quantity', value: qty
                }, {
                    xtype: 'container', html: unit, width: 50
                }, {
                    xtype: 'button', itemId:'inst', iconCls: 'icon-info', width: 22, tooltip: 'Click to edit instructions', tooltipType: 'title', readOnly: false, instructions: info, handler: this.editInstructions, itemName: name
                }, {
                    xtype: 'combobox', itemId: 'currency', allowBlank: false, msgTarget: 'side', fieldWidth: 100, hideLabel: true, valueField: 'Id', displayField: 'Name', store: this.currencyStore, blankText: 'Select a currency', value: curr
                }, {
                    xtype: 'button', iconCls: 'icon-remove', width: 70, text: 'Remove', handler: function () {
                        me.itemsAdded = Ext.Array.remove(me.itemsAdded, this.ownerCt.myItem.id);
                        form.remove(this.ownerCt);
                    }
                }]
            });
        }
        form.add(c);
    },

    editInstructions: function (b) {
        var i = b.instructions != '' ? b.instructions : 'No instructions available';
        if (b.readOnly) {
            Ext.showInstructions(b.itemName, i);
        }
        else {
            Ext.Msg.prompt('AviTrade', 'Instructions for ' + b.itemName, function (btn, text) {
                if (btn == 'ok') {
                    b.instructions = text;
                }
            }, this, true, i);
        }
    },

    saveQuote: function () {
        if (!this.formItems.getForm().isValid()) return;

        this.wnd.setLoading('Saving order quote....');

        var me = this, lineItems= [];
        var items = this.formItems.getComponent('itemForm').query('[cls="lineItem"]');
        Ext.Array.each(items, function (i, idx) {
            lineItems.push({
                ItemId: i.myItem.id,
                CurrencyId:i.myItem.currency,
                PricePerUnit: i.getComponent('price').getValue(),
                Amount: i.getComponent('price').getValue() * i.myItem.units
            });
        });

        $.ajax(Ext.serviceBase + 'QuoteOrder', {
            type: 'POST', traditional: false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ traderId: currentTraderId, orderId: this.record.get('Id'), quoteDate: Ext.Date.format(new Date(), 'Y-m-d'), lineItems: lineItems }),
        }, function () {
            me.wnd.destroy();
            if (me.reloadGrid) me.reloadGrid();
        }).error(function () { me.ajaxError(); });
    },

    savePreset: function () {
        this.formItems.getForm().isValid();
    },

    createOrder: function () {
        var me = this;

        this.wnd.setLoading('Creating order....');
        me.orderSetting.TakeOffAirportId = me.formFlight.getComponent('takeoffAirport').getValue();
        me.orderSetting.LandingAirportId = me.formFlight.getComponent('landingAirport').getValue();
        me.orderSetting.AircraftId = me.formFlight.getComponent('aircraft').getValue();
        me.orderSetting.FlightNumber = me.formFlight.getComponent('flightNo').getValue();
        me.orderSetting.EstimateTakeOff = me.formFlight.getComponent('takeoffDate').getValue();
        me.orderSetting.EstimateLanding = me.formFlight.getComponent('landingDate').getValue();


        $.ajax(Ext.serviceBase + 'CreateOrder', {
            type: 'POST', traditional: false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ traderId: currentTraderId, settings: this.orderSetting }),
            success: function () {
                me.wnd.destroy();
                if (me.reloadGrid) me.reloadGrid();
            }
        }).error(function () { me.ajaxError(); });
    },

    navigate: function (dir) {
        var me = this, layout = me.wnd.getLayout();

        if (layout.getActiveItem() == me.formContract) {
            var cboContract = me.formContract.getComponent('cboContract'),
                chkBlank = me.formContract.getComponent('chkBlank'),
                grid = me.formContract.getComponent('templateList');
            if (cboContract.getValue() == null) {
                cboContract.focus(); return;
            }
            if (!chkBlank.getValue() && grid.getSelectionModel().getCount() == 0) {
                grid.focus();
                Ext.Msg.alert('AviTrade', 'Select a template or create a blank order');
                return;
            }

            var form = me.formItems.getComponent('itemForm');
            if (form.getForm().getFields().getCount()==0 && !chkBlank.getValue()) {
                var model = grid.getSelectionModel().getLastSelected();
                Ext.Array.each(model.get('Items'), function (i) {
                    me.addLineItem(i.Id, i.Name, i.Unit, me.selectedContract.get('CurrencyId'), i.Instructions);
                });
            }

            var dk = this.wnd.getDockedComponent('contractDetails');
            if (dk != null) this.wnd.removeDocked(dk);

            var html = '';
            html = '<table width="100%" class="order-info">' +
                    '<tr><td width="120px"><label>Account:</label></td><td>' + me.selectedContract.get('Account') + '</td><td width="120px"><label>Buyer:</label></td><td>' + me.selectedContract.get('TraderName') + '</td></tr>' +
                    '<tr><td width="120px"><label>Contract Start:</label></td><td>' + Ext.Date.format(me.selectedContract.get('StartDate'), 'd M Y') + '</td><td width="120px"><label>Contract End:</label></td><td>' + Ext.Date.format(me.selectedContract.get('EndDate'), 'd M Y') + '</td></tr>' +
                    '<tr><td width="120px"><label>Billing Currency:</label></td><td>' + me.selectedContract.get('Currency') + '</td><td width="120px"><label>TimeZone:</label></td><td>' + me.selectedContract.get('TimeZone') + '</td></tr>' +
                    '</table>';
            this.wnd.addDocked({
                xtype: 'container', html: html, dock: 'top', itemId:'contractDetails'
            });

            this.btnNext.setText('Next - Items');
        }
        if (dir == 'next' && layout.getActiveItem() == me.formItems) {
            if (!me.formItems.getForm().isValid()) return;
        }
        if (dir == 'next' && layout.getActiveItem() == me.formItems && me.formItems.getComponent('itemForm').query('[cls="lineItem"]').length==0) {
            Ext.Msg.alert('AviTrade', 'Add one line item before proceeding.');
            return;
        }
        if (layout.getNext() == me.formFlight) {
            me.orderSetting = {
                OrderDate: new Date(),
                ContractId: me.selectedContract.get('Id'),
                TakeOffAirportId: me.formFlight.getComponent('takeoffAirport').getValue(),
                LandingAirportId: me.formFlight.getComponent('landingAirport').getValue(),
                AircraftId: me.formFlight.getComponent('aircraft').getValue(),
                Operateur: '',
                FlightNumber: me.formFlight.getComponent('flightNo').getValue(),
                EstimateTakeOff: me.formFlight.getComponent('takeoffDate').getValue(),
                EstimateLanding: me.formFlight.getComponent('landingDate').getValue(),
                LineItems: []
            };

            var html = '';
            /*html = '<table width="100%" class="order-info">' +
                    '<tr><td width="120px"><label>Account:</label></td><td>' + me.selectedContract.get('Account') + '</td><td width="120px"><label>Trader:</label></td><td>' + me.selectedContract.get('TraderName') + '</td></tr>' +
                    '<tr><td width="120px"><label>Contract Start:</label></td><td>' + Ext.Date.format(me.selectedContract.get('StartDate'), 'd M Y') + '</td><td width="120px"><label>Contract End:</label></td><td>' + Ext.Date.format(me.selectedContract.get('EndDate'), 'd M Y') + '</td></tr>' +
                    '<tr><td width="120px"><label>Billing Currency:</label></td><td>' + me.selectedContract.get('Currency') + '</td><td width="120px"><label>TimeZone:</label></td><td>' + me.selectedContract.get('TimeZone') + '</td></tr>' +
                    '</table><hr/><br/>';
            me.formFlight.getComponent('conContract').getEl().setHTML(html);*/

            var items = me.formItems.getComponent('itemForm').query('[cls="lineItem"]');
            html = '<br/><hr/><table class="order-info" style="margin:0px auto;">' +
                    '<tr><th>Item</th><th>Quantity</th><th>Unit</th><th width="22px"/><th>Currency</th></tr>';
            Ext.Array.each(items, function (i, idx) {
                var c = i.getComponent('currency');
                var cur = c.getStore().findRecord('Id', c.getValue());
                var item = {
                    ItemId: i.myItem.id,
                    Instructions: i.getComponent('inst').instructions,
                    Units: i.getComponent('qty').getValue(),
                    CurrencyId: cur.get('Id')
                }
                me.orderSetting.LineItems.push(item);
                html += '<tr><td>' + i.myItem.name + '</td><td align="center">' + item.Units + '</td><td>' + i.myItem.unit + '</td>'+
                    '<td><button class="icon-info" style="height:22px;border:none;cursor:pointer;" title="Click to view instructions" onclick="Ext.showInstructions(\'' + i.myItem.name + '\',\'' + item.Instructions + '\')"></button></td>' +
                    '<td>' + item.CurrencyId + '<td></tr>';
            });
            html+= '</table>';
            me.formFlight.getComponent('conItems').getEl().setHTML(html);
        }

        layout[dir]();
        me.btnPrev.setVisible(layout.getPrev() != false);
        me.btnNext.setVisible(layout.getNext() != false);
        me.btnCreate.setVisible(layout.getNext() == false);
        me.btnPreset.setVisible(layout.getActiveItem() == me.formItems);

        if (layout.getActiveItem() == me.formContract) this.btnNext.setText('Next - Items');
        if (layout.getActiveItem() == me.formItems) this.btnNext.setText('Next - Flight Info');
        if (layout.getActiveItem() == me.formItems) this.btnPrev.setText('Previous - Contract');
        if (layout.getActiveItem() == me.formFlight) this.btnPrev.setText('Previous - Items');

        if (layout.getActiveItem() == me.formContract) {
            var dk = this.wnd.getDockedComponent('contractDetails');
            if (dk != null) this.wnd.removeDocked(dk);
        }
    }
});

Ext.define('AviTrader.ViewOrder', {
    wnd: null,
    record: null,

    btnApprove: null,
    btnIgnore: null,
    btnInvoice: null,

    reloadGrid: null,

    constructor: function (cfg) {
        var me = this;

        this.itemsAdded = [];
        this.selectedContract = null;

        this.contracts = [];
        this.currencies = [];

        this.record = cfg.record;

        this.reloadGrid = cfg.reload;

        this.wnd = new Ext.Window({
            modal: true,
            resizable: false,
            layout: 'card',
            bodyBorder: false,
            title: 'View Order',
            width: 600, height: 400,
            dockedItems: [{
                xtype: 'container',
                dock: 'bottom',
                defaults: { margin: '2 5' },
                layout: { type: 'hbox', pack: 'end' },
                items: [this.btnApprove = Ext.widget({
                    xtype: 'button', text: 'Approve', hidden: true, handler: function () { me.approveOrder(); }
                }), this.btnIgnore = Ext.widget({
                    xtype: 'button', text: 'Ignore', hidden: true, handler: function () { me.ignoreOrder(); }
                }), this.btnInvoice = Ext.widget({
                    xtype: 'button', text: 'View Invoice', hidden: true, handler: function () { me.viewInvoice(); }
                }), {
                    xtype: 'button', text: 'Close', handler: function () { me.wnd.destroy(); }
                }]
            }]
        });
        this.wnd.setLoading('Retrieving order items...');
        this.wnd.show();

        if (cfg.orderId) {
            $.getJSON(Ext.serviceBase + 'RetrieveOrder', { traderId: currentTraderId, orderId: cfg.orderId }, function (data, status, resp) {
                me.record = new OrderModel(Ext.decode(resp.responseText));
                me.preload();
            }).error(function () { me.ajaxError(); });
        }
        else {
            this.preload();
        }
    },

    ajaxError: function () {
        this.wnd.setLoading(false);
        Ext.Msg.alert('AviTrade', 'Ajax error occured.');
    },

    preload: function () {
        this.btnApprove.setVisible(this.record.get('BuyerId')==currentTraderId && !this.record.get('IsFulfilled'));
        this.btnIgnore.setVisible(this.record.get('BuyerId') == currentTraderId && !this.record.get('IsFulfilled'));
        this.btnInvoice.setVisible(this.record.get('IsFulfilled'));

        var tpl = new Ext.XTemplate('<table align="center"><tr><td>',
                    '<table class="order-info">',
                    '<tr><td><label>Buyer:</label></td><td>{Buyer}</td><td><label>Seller:</label></td><td>{Seller}</td></tr>',
                    '<tr><td colspan="4"><hr/></td></tr>',
                    '<tr><td><label>Flight Number:</label></td><td>{FlightNumber}</td><td><label>Aircraft:</label></td><td>{Aircraft}</td></tr>',
                    '<tr><td><label>Takeoff:</label></td><td colspan="3">{TakeoffAirport} - {EstimatedTakeoffTime:date("D, M d, Y g:i:s A")}</td></tr>',
                    '<tr><td><label>Landing:</label></td><td colspan="3">{LandingAirport} - {EstimatedLandingTime:date("D, M d, Y g:i:s A")}</td></tr>',
                    '<tr><td colspan="4"><hr/></td></tr>',
                    '<tr><td><label>Order Date:</label></td><td>{OrderDate:date("d M Y")}</td><td><label>Quotation Date:</label></td><td>{QuotationDate:date("d M Y")}</td></tr>',
                    '<tr><td><label>Approval Date:</label></td><td>{ApprovalDate:date("d M Y")}</td><td><label>Fulfillment Date:</label></td><td>{FulfilmentDate:date("d M Y")}</td></tr>',
                    '<tr><td colspan="4"><hr/></td></tr>',
                    '<tr><td colspan="4">',
                    '<table class="order-info" style="margin-left:20px">',
                    '<tr><th>Item</th><th>Quantity</th><th>Unit</th><th width="22px"/><th class="right">Price</th>',
                    '<tpl for="LineItems">',
                    '<tr><td>{Item}</td><td align="center">{Units}</td><td>{Unit}</td>',
                    '<td><button class="icon-info" style="height:22px;border:none;cursor:pointer;" title="Click to view instructions" onclick="Ext.showInstructions(\'{Item}\',\'{Instructions}\')"></button></td>',
                    '<td align="right">{Amount:currency(" ")} {CurrencyId}</td></tr>',
                    '</tpl>',
                    '</table>',
                    '</td></tr></table>',
                    '</td><td valign="top"><div id="orderActions_{Id}" align="right"></div></td></tr></table>');
        tpl.overwrite(this.wnd.body, this.record.data);
        this.wnd.setLoading(false);
    },

    approveOrder: function () {
        var me = this;
        me.wnd.setLoading('Approving order...');
        $.ajax(Ext.serviceBase + 'ApproveOrder', {
            type: 'POST', traditional: false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ traderId: currentTraderId, orderId: this.record.get('Id'), approveDate: Ext.Date.format(new Date(), 'Y-m-d') }),
            success: function (data, status, resp) {
                me.wnd.destroy();
                if (me.reloadGrid) me.reloadGrid();

                me.record = new OrderModel(Ext.decode(resp.responseText));
                me.preload();
            }
        }).error(function () { me.ajaxError(); });
    },

    ignoreOrder: function () {
        var me = this;
        /*$.ajax(Ext.serviceBase + 'IgnoreOrder', {
            type: 'POST', traditional: false,
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ traderId: currentTraderId, orderId: this.record.get('Id'), approveDate: Ext.Date.format(new Date(), 'Y-m-d') }),
            success: function (data) {
                me.wnd.destroy();
                if (me.reloadGrid) me.reloadGrid();

                me.record = new OrderModel(Ext.decode(resp.responseText));
                me.preload();
            }
        }).error(function () { me.ajaxError(); });*/
    },

    viewInvoice: function () {
        var me = this;
        me.wnd.setLoading('Retrieving invoice details...');
        $.getJSON(Ext.serviceBase + 'RetrieveOrderInvoice', { orderId: this.record.get('Id') }, function (data) {
            var html = '';

            html = '<table class="order-info">' +
                '<tr><td><label>Invoice Date:</label></td><td>' + Ext.Date.format(eval(data.CreateDate.replace(/\/Date\((-?\d+)\)\//gi, "new Date($1)")), 'd M Y') + '</td>' +
                '<td><label>Invoice No.:</label></td><td>' + data.InvoiceNumber + '</td></tr>' +
                '</tr><td><label>Buyer:</label></td><td>' + data.Buyer + '</td><td><label>Seller:</label></td><td>' + data.Seller + '</td></tr>' +
                '<tr><td colspan="4"><hr/></td></tr>' +
                '<tr><td colspan="4" class="right"><table align="right">' +
                '<tr><td><label>Amount:</label></td><td class="right">' + Ext.util.Format.number(data.Amount, '0,000.00') + '</td></tr>' +
                '<tr><td><label>Tax:</label></td><td class="right">' + Ext.util.Format.number(data.Tax, '0,000.00') + '</td></tr>' +
                '<tr><td><label>Admin Fee:</label></td><td class="right">' + Ext.util.Format.number(data.AdminFee, '0,000.00') + '</td></tr>' +
                '<tr><td colspan="2"><hr/></td></tr>' +
                '<tr><td><label>Total:</label></td><td class="right">' + Ext.util.Format.number(data.Total, '0,000.00') + '</td></tr>' +
                '</table></td></tr></table>';

            Ext.widget({
                xtype: 'window',
                title: 'Order Invoice',
                resizable: false,
                modal: true,
                items: [{
                    xtype: 'container', html: html
                }]
            }).show();
            me.wnd.setLoading(false);
        }).error(function () {
            me.wnd.setLoading(false);
            Ext.Msg.alert('AviTrade', 'Ajax error occured.');
        });
    }
});