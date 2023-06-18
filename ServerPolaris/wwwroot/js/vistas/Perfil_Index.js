
const MODELO_BASE = {
    PerfilId: 0,
    Descripcion: ""
}

let tablaData;

$(document).ready(function () {


    var intervalo = setInterval(function () {

        if (resposeSecurity) {
            clearInterval(intervalo);

            tablaData = $('#tbdata').DataTable({
                responsive: true,
                "ajax": {
                    "url": '/Perfil/Lista',
                    "type": "GET",
                    "datatype": "json"
                },
                "columns": [
                    //searchable permite al datable a realizar la busqueda
                    { "data": "perfilId" },
                    { "data": "descripcion" },
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
    $("#txtId").val(modelo.PerfilId)
    $("#txtNombre").val(modelo.Descripcion)

    $("#modalData").modal("show")
}



$("#button_add").on("click", ".btn-add", function () {
    mostrarModal()
})



$("#btnGuardar").click(function () {

    if ($("#txtNombre").val().trim() == "") {
        toastr.warning("", "Debe completar el campo Nombre")
        $("#txtNombre").focus()
        return;
    }

    const modelo = structuredClone(MODELO_BASE)
    modelo["PerfilId"] = parseInt($("#txtId").val())
    modelo["Descripcion"] = $("#txtNombre").val()

    console.log(JSON.stringify(modelo))

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    if (modelo.PerfilId == 0) {

        fetch("/Perfil/Crear", {
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
                    swal("Listo!", "El perfil fue creado", "success")
                } else {
                    swal("Lo sentimos", responseJson.mensaje, "error")
                }
            })
    } else {

        fetch("/Perfil/Editar", {
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
                    swal("Listo!", "El perfil ha sido Editado", "success")
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

    const modelo = structuredClone(MODELO_BASE)
    modelo["PerfilId"] = data.perfilId
    modelo["Descripcion"] = data.descripcion

    mostrarModal(modelo);
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
        text: `Eliminar el perfil "${data.descripcion}" `,
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


                fetch(`/Perfil/Eliminar?idPerfil=${data.perfilId}`, {
                    method: "DELETE",
                })
                    .then(response => {
                        $(".showSweetAlert").LoadingOverlay("hide");
                        return response.ok ? response.json() : Promise.reject(response);
                    })
                    .then(responseJson => {

                        if (responseJson.estado) {

                            tablaData.row(fila).remove().draw()
                            swal("Listo!", "El perfil Eliminada", "success")
                        } else {
                            swal("Lo sentimos", responseJson.mensaje, "error")
                        }
                    })
            }

        }
    )

})

