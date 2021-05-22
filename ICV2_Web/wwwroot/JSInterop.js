window.JSInteropExt = {};
window.JSInteropExt.saveAsFile = (filename, type, bytesBase64) => {
    if (navigator.msSaveBlob) {
        var data = window.atob(bytesBase64);
        var bytes = new Uint8Array(data.length);
        for (var i = 0; i < data.length; i++) {
            bytes[i] = data.charCodeAt(i);
        }
        var blob = new Blob([bytes.buffer], { type: type });
        navigator.msSaveBlob(blob, filename);
    }
    else {
        var link = document.createElement('a');
        link.download = filename;
        link.href = "data:" + type + ";base64," + bytesBase64;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }
}


//var wrapp;

//var apiURL = "http://localhost:43119";
//export function CreateWrapper(wrapper) {
//	wrapp = wrapper;
//}

////var $form = $("#MyForm");

////function getFormData($form) {
////    var unindexed_array = $form.serializeArray();
////    var indexed_array = {};

////    $.map(unindexed_array, function (n, i) {
////        indexed_array[n['name']] = n['value'];
////    });

////    return indexed_array;
////}

//export function TestFunctionCalledFromComponent(dataForm){
//    //var dataObj = JSON.stringify(dataForm);
//    //console.log(dataObj);

//    //console.log(dataObj.stockGroup);
//    //console.log(dataForm.includeZeroBalance);
//    console.log(dataForm);
//    //console.log(dataForm.companyCode);
   
//    $("#example").DataTable({
//        "processing": true, // for show progress bar  
//        "serverSide": true, // for process server side  
//        "filter": true, // this is for disable filter (search box)  
//        "orderMulti": false, // for disable multiple column at once  
//        "pageLength": 5,
//        "ajax": {
//            "type": "POST",
//            "url": "http://localhost:43119/api/Report/GenerateBatchNoBalDetails",
//            "contentType": "application/json; charset=utf-8",
//            "data": function (d) {
//                let additionalValues = [];
//                additionalValues[0] = dataForm.companyCode;
//                additionalValues[1] = dataForm.stockGroup;
//                additionalValues[2] = dataForm.location;
//                additionalValues[3] = dataForm.includeZeroBalance;
//                d.AdditionalValues = additionalValues;  
//                return JSON.stringify(d);
//            }
//        },
//        "columns": [
//            { "data": "stockCode" },
//            { "data": "batchno" },
//            { "data": "uOMcode" },
//        ],
//        "order": [[0,"desc"]],
//    });

//    //$.ajax({
//    //    type: "POST",
//    //    url: "http://localhost:43119/api/Report/GenerateBatchNoBalDetails",
//    //    data: dataObj,
//    //    contentType: "application/json",
//    //    success: function (dataResult) {
//    //        console.log(dataResult);
//    //    },
//    //    error: function (req, status, error) {
//    //        console.log(req);
//    //        console.log(status);
//    //        console.log(error);
//    //    }
//    //});

//    //$('#testDiv').html("WORKING!");
//    //console.log(data);



//	//$('#example').DataTable();


//}