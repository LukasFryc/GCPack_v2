function isEmpty(value) {
    return (value == null || value.length === 0);
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


function GetDocuments_SetParameters(name, Number, AdministratorName, DocumentTypeID, OrderBy, ProjectID,
    DivisionID, AppSystemID, WorkplaceID, EffeciencyDateFrom, EffeciencyDateTo, NextReviewDateFrom,
    NextReviewDateTo, ReadType, StateCode, Revision, ReviewNecessaryChange, Page, ItemsPerPage) {

// nepouzito 
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

    if ($('#MainID').val()!="0") {

        var timeoutHandle;
        if (timeoutHandle != null) window.clearTimeout(timeoutHandle);

        timeoutHandle = setTimeout(
            function () {

                //alert('GetDocuments_Edit 2');

                var AllPath = AppRoot + "/documents/getDocuments";
                $.post(AppRoot + "/documents/getDocuments", {
                    //Number: $('#DocumentNumber').val(),
                     MainID: $('#MainID').val(),
                     ItemsPerPage: 100,
                     ReadType: 'all',
                     SaveFilter: false
                })

                    .done(function (documentCollection) {
                         $('table .dataRow').remove();
                         $.each(documentCollection.Documents, function (index, doc) {

                             var dateStr = "";

                             var unReadCount = 0;

                             var redColor = "#EE9090";
                             var blackColor = "#000000";
                             var readColor = blackColor;
                             var archiv = "";

                             if (doc.EffeciencyDate != null) {
                                 var date = eval(doc.EffeciencyDate.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"));
                                 dateStr =
                                     date.getDate() + "." + (date.getMonth() + 1) + "." + date.getFullYear();
                             }

                             unReadCount = doc.AllUsers - doc.UsersRead;

                             if (unReadCount > 0) {
                                 readColor = redColor
                             }

                             var IssueChnageComent = doc.IssueChangeComment;
                       
                             if (isEmpty(IssueChnageComent) == true) { IssueChnageComent = '-' };

                             $('table').append('<tr class="dataRow">' +
                                 '<td> ' + doc.DocumentNumber + ' (V' + doc.IssueNumber + ')</td>' +
                                 '<td> ' + doc.Title + '</td>' +
                                 '<td> ' + dateStr + '</td>' +
                                 '<td> ' + IssueChnageComent + '</td>' +
                                '<td> <A class="linktest" href="' + AppRoot + '/Documents/Details?documentId=' + doc.ID +
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