    $(document).ready(function () {
    //Prepare jtable plugin
        $('#transdatepicker').datepicker({
           onSelect: function(dateText, el) { 
                $('#TransactionTable').jtable('load', { Day: dateText });
           }
        });
        $('#TransactionTable').jtable({
            columnSelectable: false,
            selecting: true, //Enable selecting
            multiselect: false, //Allow multiple selecting
            selectingCheckboxes: false, //Show checkboxes on first column
            title: "Today's Transactions",
            actions: {
                listAction: '/Inventory/GetTodayTransactions'
                //,deleteAction: '/Inventory/DeleteShoppingList'
                //,updateAction: '/Inventory/UpdateShoppingList'
                //,createAction: '/DataManagement/CreateShoppingList'
            },
            fields: {
                TransId: {
                    key: true,
                    create: false,
                    edit: false,
                    list: false
                },
                InvId: {
                    create: false,
                    edit: false,
                    list: false
                },
                TimeStamp: {
                    list: true,
                    edit: true,
                    title: 'Time',
                    width: '8%',
                    type: 'date',
                    displayFormat: 'mm/dd/yy'
                },
                Description: {
                    list: true,
                    edit: false,
                    title: 'Description',
                    width: '40%'
                },
                Quantity: {
                    edit: true,
                    list: true,
                    title: 'Quantity',
                    width: '10%'
                },
                Size: {
                    edit: true,
                    list: true,
                    title: 'Size',
                    defaultValue: '0'
                },
                UnitId: {
                    list: true,
                    edit: true,
                    title: 'Units',
                    defaultValue: '',
                    options: '/Options/Units'
                },
                Cost: {
                    list: true,
                    edit: true,
                    title: 'Cost',
                    defaultValue: '0'
                },
                ActionId: {
                    list: true,
                    edit: true,
                    title: 'Action',
                    defaultValue: '',
                    options: '/Options/InventoryActions'
                }
            },
            //Register to selectionChanged event
            selectionChanged: function () {
                // Populate Transaction Editor
            }
        });

        //Load activity list from server
        $('#TransactionTable').jtable('load');
    });