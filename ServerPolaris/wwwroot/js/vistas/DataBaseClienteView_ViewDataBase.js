

let tablaData;
let tablaData1;

$(document).ready(function () {
   

    tablaData = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/DataBaseClienteView/getInfoIndex',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            //searchable permite al datable a realizar la busqueda
            { "data": "dbName" },
            { "data": "databaseShemaTable" },
            { "data": "equalityColumns" },
            { "data": "inequalityColumns" },
            { "data": "includedColums" },
            { "data": "avgUserImpact" },
            { "data": "createCmd" },

            
        ],
        order: [[0, "desc"]],
        // dom: "Bfrtip",
        //buttons: [
        //    {
        //        text: 'Exportar Excel',
        //        extend: 'excelHtml5',
        //        title: '',
        //        filename: 'Reporte Categorias',
        //        exportOptions: {
        //            columns: [1, 2]
        //        }
        //    }, 'pageLength'
        //],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });

    tablaData1 = $('#tbdata1').DataTable({
        responsive: true,
        "ajax": {
            "url": '/DataBaseClienteView/getInfoFiles',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            //searchable permite al datable a realizar la busqueda
            { "data": "dataBaseName" },
            { "data": "fileSizeMB" },
            { "data": "physicalNeme" }       
        ],
        order: [[0, "desc"]],
        
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });

})







