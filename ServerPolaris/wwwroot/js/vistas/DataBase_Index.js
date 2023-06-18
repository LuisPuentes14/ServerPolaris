
const MODELO_BASE = {
    dataBaseId: 0,
    clienteId: 0,
    dataBaseInstance: "",
    dataBaseName: "",
    dataBaseUser: "",
    dataBasePassword: "",
    clienteName: ""
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

    var intervalo = setInterval(function () {

        if (resposeSecurity) {
            clearInterval(intervalo);

            tablaData = $('#tbdata').DataTable({
                responsive: true,
                "ajax": {
                    "url": '/DataBaseCliente/Lista',
                    "type": "GET",
                    "datatype": "json"
                },
                "columns": [
                    //searchable permite al datable a realizar la busqueda
                    { "data": "dataBaseId" },
                    { "data": "clienteName" },
                    { "data": "dataBaseInstance" },
                    { "data": "dataBaseName" },
                    { "data": "dataBaseUser" },
                    { "data": "dataBasePassword" },

                    {
                        "defaultContent": botonesTabla,
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

        }
    }, 0);

})


function mostrarModal(modelo = MODELO_BASE) {
    $("#txtId").val(modelo.dataBaseId)
    $("#cboCliente").val(modelo.clienteId == 0 ? $("#cboCliente option:first").val() : modelo.clienteId)
    $("#txtInstancia").val(modelo.dataBaseInstance)
    $("#txtNombre").val(modelo.dataBaseName)
    $("#txtUsuario").val(modelo.dataBaseUser)
    $("#txtPassword").val(modelo.dataBasePassword)
    /* $("#txtRutaLog").val(modelo.clienteName)*/

    $("#modalData").modal("show")
}



$("#button_add").on("click", ".btn-add", function () {
    mostrarModal()
})



$("#btnGuardar").click(function () {

    if ($("#txtInstancia").val().trim() == "") {
        toastr.warning("", "Debe completar el campo de instancia")
        $("#txtInstancia").focus()
        return;
    }

    if ($("#txtNombre").val().trim() == "") {
        toastr.warning("", "Debe completar el campo Nombre")
        $("#txtNombre").focus()
        return;
    }

    if ($("#txtUsuario").val().trim() == "") {
        toastr.warning("", "Debe completar el campo usuario")
        $("#txtUsuario").focus()
        return;
    }

    if ($("#txtPassword").val().trim() == "") {
        toastr.warning("", "Debe completar el campo contraseña")
        $("#txtPassword").focus()
        return;
    }

    const modelo = structuredClone(MODELO_BASE)
    modelo["dataBaseId"] = parseInt($("#txtId").val())
    modelo["clienteId"] = parseInt($("#cboCliente").val())
    modelo["dataBaseInstance"] = $("#txtInstancia").val()
    modelo["dataBaseName"] = $("#txtNombre").val()
    modelo["dataBaseUser"] = $("#txtUsuario").val()
    modelo["dataBasePassword"] = $("#txtPassword").val()

    console.log(JSON.stringify(modelo))

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    if (modelo.dataBaseId == 0) {

        fetch("/DataBaseCliente/Crear", {
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

        fetch("/DataBaseCliente/Editar", {
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
        text: `Eliminar la base de datos "${data.dataBaseName}" del cliente "${data.clienteName}" `,
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


                fetch(`/DataBaseCliente/Eliminar?idDb=${data.dataBaseId}`, {
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


$("#tbdata tbody").on("click", ".btn-view", function () {

    if ($(this).closest("tr").hasClass("child")) {
        filaSeleccionada = $(this).closest("tr").prev();
    } else {
        filaSeleccionada = $(this).closest("tr");

    }

    const data = tablaData.row(filaSeleccionada).data();

    window.location.href = '/DataBaseClienteView/ViewDataBase?idDb=' + data.dataBaseId

})
