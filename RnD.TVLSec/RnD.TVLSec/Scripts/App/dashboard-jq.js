//-------------------------------------------------------
//start of kendo grid

function name(id, readUrl) {

    var _id = id;
    var _readUrl = readUrl;

    $(_id).kendoGrid({
        dataSource: {
            transport: {
                read: _readUrl
            },
            schema: {
                data: function (data) {
                    return data.Data;
                },
                model: {
                    fields: {
                        AccountId: { type: "number" },
                        AccountName: { type: "string" },
                        ActionLink: { type: "string" }
                    }
                }, //end model
                total: function (data) {
                    return data.Total;
                }
            },
            //pageSize: 20,
            serverPaging: true,
            serverFiltering: false,
            serverSorting: true
        },
        height: 250,
        filterable: true,
        groupable: true,
        sortable: true,
        resizable: true,
        pageable: {
            refresh: true,
            pageSizes: true,
            pageSizes: [20, 40, 60, 80, 100]
        },
        columns: [{ field: "AccountId", title: "AccountID", hidden: true, filterable: false, sortable: false },
                  { field: "AccountName", title: "Account Name", width: "80%" },
                  { field: "ActionLink", title: "Actions", width: "12%", filterable: false, sortable: false, template: "#= ActionLink #" }
        ]
    });

}

//end of kendo grid
//-------------------------------------------------------
