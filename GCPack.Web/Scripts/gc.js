function isEmpty(value) {
    return (value === null || value.length === 0);
}


function SetSelectedColumn(thisObj, f) {

    var $tableThis = thisObj.closest('table');

    $tableThis.find(".sortBy").each(function () {
        var $sortByA = $(this);
        var $sortBySpan = $sortByA.find('span.glyphicon');
        $sortBySpan.removeClass('setSortBy');
        $sortBySpan.addClass('setSortByDefault');
        //alert($sortByA.attr('sortBy'));
    });

    orderBy = thisObj.attr('sortBy');

    var $sortByA = thisObj;

    var $sortBySpan = $sortByA.find('span.glyphicon');

    $sortBySpan.removeClass('setSortByDefault');
    $sortBySpan.addClass('setSortBy');

    f();
}


//function isStringNullOrEmpty (val) {
//    switch (val) {
//        case "":
//        case 0:
//        case "0":
//        case null:
//        case false:
//        case undefined:
//        case typeof this === 'undefined':
//            return true;
//        default: return false;
//    }
//}

//$('th').click(function () {
//    var table = $(this).parents('table').eq(0)
//    var rows = table.find('tr:gt(0)').toArray().sort(comparer($(this).index()))
//    this.asc = !this.asc
//    if (!this.asc) { rows = rows.reverse() }
//    for (var i = 0; i < rows.length; i++) { table.append(rows[i]) }
//})
//function comparer(index) {
//    return function (a, b) {
//        var valA = getCellValue(a, index), valB = getCellValue(b, index)
//        return $.isNumeric(valA) && $.isNumeric(valB) ? valA - valB : valA.localeCompare(valB)
//    }
//}
//function getCellValue(row, index) { return $(row).children('td').eq(index).text() }

//function createTableHeader(table, headers, alignment) {
//    if (headers.length > 0) {
//        var thead = document.createElement('thead');
//        table.appendChild(thead);
//        var tr = document.createElement('tr');
//        for (var i = 0; i <= headers.length - 1; i++) {
//            var th = document.createElement('th');
//            var text = document.createTextNode(headers[i]);
//            th.appendChild(text);
//            th.style.textAlign = alignment;
//            th.style.cursor = 'pointer';
//            th.setAttribute('title', "Sort by " + headers[i]);
//            th.onclick = function () {
//                var rows = $(table).find('tbody').find('tr').toArray().sort(comparer($(this).index()));
//                this.asc = !this.asc;
//                if (!this.asc) {
//                    rows = rows.reverse();
//                }
//                for (var i = 0; i < rows.length; i++) {
//                    $(table).append(rows[i]);
//                }
//            }
//            tr.appendChild(th);
//        }
//        thead.appendChild(tr);
//    }
//}


function GetUsersJob() {

    var timeoutHandle;
    if (timeoutHandle !== null) window.clearTimeout(timeoutHandle);

    timeoutHandle = setTimeout(
        function () {


            var AllPath = AppRoot + "/user/GetUsersJob";

            //JobPositionIDs
            //SelectedUsers

            selectedUsers = [];
            $("#SelectedUsers").children().each(function () {
                selectedUsers.push($(this).val());
            });

            selectedJobPositionIDs = [];
            $("#JobPositionIDs").children().each(function () {
                selectedJobPositionIDs.push($(this).val());
            });


            var query = {
                "selectedUserIDs": selectedUsers.join(','),
                "selectedJobPositionIDs": selectedJobPositionIDs.join(','),
                "orderBy": GetUsersJob_orderBy 

            };



            // LF 10.11.2017 pred tim nez jsme skryli jobPosition a dalsi pole $.getJSON(AppRoot +"/user/getUsers", { name: $('#filter').val(), jobPositionId: $('#jobPosition').val(), preservedUsers: selectedUsers.join(',') })
            $.getJSON(AllPath, query)
                .done(function (usersJob) {

                  

                    $('#AllUsersJob .dataRow').remove();

                    $.each(usersJob, function (index, userjob) {
                        $('#AllUsersJob').append('<tr class="dataRow">' +
                            '<td> ' + userjob.LastName + ' ' + userjob.FirstName+ '</td>' +
                            '<td> ' + userjob.JobPositionName + '</td>'
                        );

                    });
                    

                })
                .fail(function (jqxhr, textStatus, error) {
                    var err = textStatus + ", " + error;
                    console.log("Request Failed: " + err);
                });


        },
        500);
}

function GetReadConfirms() {

    var timeoutHandle;
    if (timeoutHandle !== null) window.clearTimeout(timeoutHandle);

    timeoutHandle = setTimeout(
        function () {


            var AllPath = AppRoot + "/documents/GetReadConfirms";

            //JobPositionIDs
            //SelectedUsers

            //selectedUsers = [];

            //$("#SelectedUsers").children().each(function () {
            //    selectedUsers.push($(this).val());
            //});

            //selectedJobPositionIDs = [];
            //$("#JobPositionIDs").children().each(function () {
            //    selectedJobPositionIDs.push($(this).val());
            //});


            var query = {
                //"selectedUserIDs": selectedUsers.join(','),
                //"selectedJobPositionIDs": selectedJobPositionIDs.join(',')
                "documentId": $('#ID').val(),
                "orderBy": GetReadConfirms_orderBy 
            };

            // LF 10.11.2017 pred tim nez jsme skryli jobPosition a dalsi pole $.getJSON(AppRoot +"/user/getUsers", { name: $('#filter').val(), jobPositionId: $('#jobPosition').val(), preservedUsers: selectedUsers.join(',') })
            $.getJSON(AllPath, query)
                .done(function (readConfirms) {

                    $('#AllReadConfirms .dataRow').remove();

                    $.each(readConfirms, function (index, readConfirm) {

                        dateCreated = '';
                        if (readConfirm.Created !== null) {
                            var dateC = eval(readConfirm.Created.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
                            dateCreated =
                                dateC.getDate() + "." + (dateC.getMonth() + 1) + "." + dateC.getFullYear();
                        } 

                        dateRead = '';
                        if (readConfirm.ReadDate !== null) {
                            var dateR = eval(readConfirm.ReadDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
                            dateRead =
                                dateR.getDate() + "." + (dateR.getMonth() + 1) + "." + dateR.getFullYear();
                        }

                        $('#AllReadConfirms').append('<tr class="dataRow">' +
                            '<td> ' + readConfirm.LastName + ' ' + readConfirm.FirstName + '</td>' +
                            '<td> ' + readConfirm.JobPositionName + '</td>' +
                            '<td> ' + dateCreated + '</td>' + 
                            '<td> ' + dateRead + '</td>'
                        );

                    });


                })
                .fail(function (jqxhr, textStatus, error) {
                    var err = textStatus + ", " + error;
                    console.log("Request Failed: " + err);
                });


        },
        500);
}

function GetDocuments_SetParameters(name, Number, AdministratorName, DocumentTypeID, OrderBy, ProjectID,
    DivisionID, AppSystemID, WorkplaceID, EffeciencyDateFrom, EffeciencyDateTo, NextReviewDateFrom,
    NextReviewDateTo, ReadType, StateCode, Revision, ReviewNecessaryChange, Page, ItemsPerPage) {

// nepouzito 
    name = $('#Name').val();
    name = $('#Name').val();
    Number = $('#Number').val();
    AdministratorName = $('#AdministratorName').val();
    DocumentTypeID = $('#DocumentTypeID').val();
    //OrderBy = orderBy;
    ProjectID = $('#ProjectID').val();
    DivisionID = $('#DivisionID').val();
    AppSystemID = $('#AppSystemID').val();
    WorkplaceID = $('#WorkplaceID').val();
    EffeciencyDateFrom = $('#EffeciencyDateFrom').val();
    EffeciencyDateTo = $('#EffeciencyDateTo').val();
    NextReviewDateFrom = $('#NextReviewDateFrom').val();
    NextReviewDateTo = $('#NextReviewDateTo').val();
    ReadType = $('#ReadType').val();
    //StateID: $('#StateID').val(),
    StateCode = $('#StateCode').val();
    Revision = $('#Revision').val();
    //Archiv: $('#Archiv').val(),
    ReviewNecessaryChange = $('#ReviewNecessaryChange').val();
    alert($('#page').val());
    Page = $('#page').val();
    ItemsPerPage = $('.rowsPerPage').val();

}

function GetDocuments_Edit()
{

    if ($('#MainID').val()!=="0") {

        var timeoutHandle;
        if (timeoutHandle !== null) window.clearTimeout(timeoutHandle);

        timeoutHandle = setTimeout(
            function () {

                alert(GetDocuments_Edit_orderBy);

                var query = {
                    MainID: $('#MainID').val(),
                    ItemsPerPage: 100,
                    ReadType: 'all',
                    SaveFilter: false,
                    "orderBy": GetDocuments_Edit_orderBy
                };


                var AllPath = AppRoot + "/documents/getDocuments";
                $.post(AppRoot + "/documents/getDocuments", query)

                    .done(function (documentCollection) {
                         $('#AllIssue .dataRowClick').remove();
                         $.each(documentCollection.Documents, function (index, doc) {

                             var dateStr = "";

                             var unReadCount = 0;

                             var redColor = "#EE9090";
                             var blackColor = "#000000";
                             var readColor = blackColor;
                             var archiv = "";

                             if (doc.EffeciencyDate !== null) {
                                 var date = eval(doc.EffeciencyDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
                                 dateStr =
                                     date.getDate() + "." + (date.getMonth() + 1) + "." + date.getFullYear();
                             }

                             unReadCount = doc.AllUsers - doc.UsersRead;

                             if (unReadCount > 0) {
                                 readColor = redColor
                             }

                             var IssueChnageComent = doc.IssueChangeComment;
                       
                             if (isEmpty(IssueChnageComent) === true) { IssueChnageComent = '-' };

                             $('#AllIssue').append('<tr class="dataRowClick"' + doc.ID + ' value="' + doc.ID + '">' +
                                 '<td> ' + doc.DocumentNumber + ' (V' + doc.IssueNumber + ')</td>' +
                                 '<td> ' + doc.Title + '</td>' +
                                 '<td> ' + dateStr + '</td>' +
                                 '<td> ' + IssueChnageComent + '</td>' +
                                '<td> <A href="' + AppRoot + '/Documents/Details?documentId=' + doc.ID +
                                '"><span class="glyphicon glyphicon-eye-open"></span></A></td>'
                            );
                        });

                    })
                    .fail(function (jqxhr, textStatus, error) {
                        var err = textStatus + ", " + error;
                        console.log("Request Failed: " + err);
                    });


            },
            500);
    }
}