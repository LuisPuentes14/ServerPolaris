
const MODELO_BASE = {
    logId: 0,
    clienteId: 0,
    clienteName: "",
    logIdTipoLog: 0,
    tipoLogDescripcion: "",
    logPathFile: ""
}

let tablaData;

$(document).ready(function () {


    fetch("/Clientes/Lista")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.data.length > 0) {
                responseJson.data.forEach((item) => {
                    $("#cboCliente").append(
                        $("<option>").val(item.clienteId).text(item.clienteName)
                    )
                })
            }
        })

    fetch("/TipoLog/Lista")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.data.length > 0) {
                responseJson.data.forEach((item) => {
                    $("#cboTipoLog").append(
                        $("<option>").val(item.tipoLogId).text(item.tipoLogDescripcion)
                    )
                })
            }
        })





    tablaData = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/LogCliente/Lista',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            //searchable permite al datable a realizar la busqueda
            { "data": "logId" },
            { "data": "clienteName" },
            { "data": "tipoLogDescripcion" },
            { "data": "logPathFile" },

            {
                "defaultContent": '<button class="btn btn-primary btn-editar btn-sm mr-2"><i class="fas fa-pencil-alt"></i></button>' +
                    '<button class="btn btn-danger btn-eliminar btn-sm"><i class="fas fa-trash-alt"></i></button>',
                "orderable": false,
                "searchable": false,
                "width": "80px"
            }
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

})


function mostrarModal(modelo = MODELO_BASE) {
    $("#txtId").val(modelo.logId)
    $("#cboCliente").val(modelo.clienteId == 0 ? $("#cboCliente option:first").val() : modelo.clienteId)
    $("#cboTipoLog").val(modelo.logIdTipoLog == 0 ? $("#cboTipoLog option:first").val() : modelo.logIdTipoLog)
    $("#txtRutaLog").val(modelo.logPathFile)

    $("#modalData").modal("show")
}



$("#btnNuevo").click(function () {
    mostrarModal()
})



$("#btnGuardar").click(function () {

    if ($("#txtRutaLog").val().trim() == "") {

        toastr.warning("", "Debe completar el campo Ruta log")
        $("#txtRutaLog").focus()
        return;
    }



    const modelo = structuredClone(MODELO_BASE)
    modelo["logId"] = parseInt($("#txtId").val())
    modelo["clienteId"] = parseInt($("#cboCliente").val())
    //modelo["clienteName"] = $("#txtNombre").val()
    modelo["logIdTipoLog"] = $("#cboTipoLog").val()
    //modelo["tipoLogDescripcion"] = $("#txtNombre").val()
    modelo["logPathFile"] = $("#txtRutaLog").val()

    console.log(JSON.stringify(modelo))

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    if (modelo.logId == 0) {

        fetch("/LogCliente/Crear", {
            method: "POST",
            headers: { "Content-Type": "application/json;charset=utf-8" },
            body: JSON.stringify(modelo)
        })
            .then(response => {
                $("#modalData").find("div.modal-content").LoadingOverlay("hide");
                return response.ok ? response.json() : Promise.reject(response);
            })
            .then(responseJson => {

                if (responseJson.estado) {

                    tablaData.row.add(responseJson.objeto).draw(false)
                    $("#modalData").modal("hide")
                    swal("Listo!", "El log fue creado", "success")
                } else {
                    swal("Lo sentimos", responseJson.mensaje, "error")
                }
            })
    } else {

        fetch("/LogCliente/Editar", {
            method: "PUT",
            headers: { "Content-Type": "application/json;charset=utf-8" },
            body: JSON.stringify(modelo)
        })
            .then(response => {
                $("#modalData").find("div.modal-content").LoadingOverlay("hide");
                return response.ok ? response.json() : Promise.reject(response);
            })
            .then(responseJson => {

                if (responseJson.estado) {

                    tablaData.row(filaSeleccionada).data(responseJson.objeto).draw(false);
                    filaSeleccionada = null;
                    $("#modalData").modal("hide")
                    swal("Listo!", "El log ha sido Editado", "success")
                } else {
                    swal("Lo sentimos", responseJson.mensaje, "error")
                }
            })

    }

})


let filaSeleccionada;
$("#tbdata tbody").on("click", ".btn-editar", function () {

    if ($(this).closest("tr").hasClass("child")) {
        filaSeleccionada = $(this).closest("tr").prev();
    } else {
        filaSeleccionada = $(this).closest("tr");

    }

    const data = tablaData.row(filaSeleccionada).data();



    mostrarModal(data);
})


$("#tbdata tbody").on("click", ".btn-eliminar", function () {

    let fila;

    if ($(this).closest("tr").hasClass("child")) {
        fila = $(this).closest("tr").prev();
    } else {
        fila = $(this).closest("tr");

    }
    console.log(fila)
    const data = tablaData.row(fila).data();
    console.log(data)



    swal({
        title: "¿Estas seguro?",
        text: `Eliminar el log del cliente "${data.clienteName}" con la ruta "${data.logPathFile}"`,
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Si, eliminar",
        cancelButtonText: "No, cancelar",
        closeOnConfirm: false,
        closeOnCancel: true
    },

        function (respuesta) {

            if (respuesta) {

                $(".showSweetAlert").LoadingOverlay("show");


                fetch(`/LogCliente/Eliminar?idLog=${data.logId}`, {
                    method: "DELETE",
                })
                    .then(response => {
                        $(".showSweetAlert").LoadingOverlay("hide");
                        return response.ok ? response.json() : Promise.reject(response);
                    })
                    .then(responseJson => {

                        if (responseJson.estado) {

                            tablaData.row(fila).remove().draw()
                            swal("Listo!", "El log fue Eliminada", "success")
                        } else {
                            swal("Lo sentimos", responseJson.mensaje, "error")
                        }
                    })
            }

        }
    )

})