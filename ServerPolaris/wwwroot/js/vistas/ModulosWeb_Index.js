
const MODELO_BASE = {
    modId: 0,
    modIdPadre: 0,
    modIdHijo: null,
    modNombre: "",
    modUrl: "",
    modDescripcion:"",
    modIcono:"",
    idTipoModulo: 0,
    descripcionTipoModulo: ""

}

let oldIdPadre;
let oldIdModulo;
let listObjOld = [];
let countTranslation = 0;
let tipoModulo = 0

let tablaData;

$(document).ready(function () {


    $("#txtTipoModulo").val("1")
    $("#txtidPadre").val("0")

    //fetch("/TipoModulo/Lista")
    //    .then(response => {
    //        return response.ok ? response.json() : Promise.reject(response);
    //    })
    //    .then(responseJson => {
    //        if (responseJson.data.length > 0) {
    //            responseJson.data.forEach((item) => {
    //                $("#cbotipomodulo").append(
    //                    $("<option>").val(item.idTipoModulo).text(item.descripcion)
    //                )
    //            })
    //        }
    //    })





    tablaData = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/ModuloWeb/Lista?tipoModulo=1',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            //searchable permite al datable a realizar la busqueda
            { "data": "modId" },
            { "data": "modIdPadre" },
            { "data": "modNombre" },
            { "data": "modUrl" },
            { "data": "modDescripcion" },
            { "data": "modIcono" },
            { "data": "descripcionTipoModulo" },

            {
                "defaultContent": '<button class="btn btn-primary btn-editar btn-sm mr-2"><i class="fas fa-pencil-alt"></i></button>' +
                    '<button class="btn btn-danger btn-eliminar btn-sm mr-2"><i class="fas fa-trash-alt"></i></button>' +
                    '<button class="btn btn-secondary btn-botones btn-sm mr-2"><i class="fa fa-cubes" aria-hidden="true"></i></button>' +
                    '<button class="btn btn-warning btn-submodulos btn-sm"><i class="fa fa-clone" aria-hidden="true"></i></button>',
                "orderable": false,
                "searchable": false,
                "width": "80px"
            }
        ],
        order: [[0, "desc"]],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });

})



function mostrarModal(modelo = MODELO_BASE) {




    //let options = $("#id_ds_field_groups ").find("option");
    //for (let i = 0; i < options.length; i++) {
    //    $("#id_ds_field_groups option[value='" + options[i].value + "']").remove();
    //}

    $("#txtId").val(modelo.modId)
    $("#txtNombre").val(modelo.modNombre)
    $("#txtUrl").val(modelo.modUrl)
    $("#txtDescripcion").val(modelo.modDescripcion)
    $("#txtIcono").val(modelo.modIcono)
    //$("#txtUsuario").val(modelo.usuLogin)
    //$("#txtNombre").val(modelo.usuNombre)
    //$("#txtEmail").val(modelo.usuEmail)
    //$("#cboestado").val(modelo.estadoId)
    //$("#txtPassword").val(modelo.usuPassword)
    //$('#exampleCheck1').prop('checked', false)


    //for (let i = 0; i < modelo.perfils.length; i++) {

    //    if (modelo.perfils[i].perfilId != 0) {
    //        $("#id_ds_field_groups").append(
    //            $("<option>").val(modelo.perfils[i].perfilId).text(modelo.perfils[i].descripcion)
    //        )
    //    }
    //}

    $("#modalData").modal("show")
}



$("#btnNuevo").click(function () {

    console.log($("#id_sc_field_groups").find("option"))
    let options = $("#id_sc_field_groups").find("option")

    for (let i = 0; i < options.length; i++) {
        console.log(options[i])
        options[i].style.color = ""
        options[i].disabled = false;
    }

    $("#txtPassword").css('display', 'inline');
    $("#labelPassword").css('display', 'inline');

    $("#sectionPassword").css('display', 'none');

    mostrarModal()
})



$("#btnGuardar").click(function () {

    //if ($("#txtUsuario").val().trim() == "") {

    //    toastr.warning("", "Debe completar el campo Usuario")
    //    $("#txtUsuario").focus()
    //    return;
    //}

    //if ($("#txtNombre").val().trim() == "") {
    //    toastr.warning("", "Debe completar el campo Nombre")
    //    $("#txtUsuario").focus()
    //    return;
    //}

    
    //}

    const modelo = structuredClone(MODELO_BASE)
    modelo["modId"] = parseInt($("#txtId").val())
    modelo["modNombre"] = $("#txtNombre").val()
    modelo["modUrl"] = $("#txtUrl").val()
    modelo["modIdPadre"] = parseInt($("#txtidPadre").val())
    modelo["idTipoModulo"] = parseInt($("#txtTipoModulo").val())
    modelo["modDescripcion"] = $("#txtDescripcion").val()
    modelo["modIcono"] = $("#txtIcono").val()



    //modelo["usuEmail"] = $("#txtEmail").val()
    //modelo["estadoId"] = $("#cboestado").val()
    //modelo["usuPassword"] = $("#txtPassword").val()
    //modelo["isUpdatePassword"] = $('#exampleCheck1').prop('checked')

   

    console.log(modelo)


    console.log(JSON.stringify(modelo))

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    if (modelo.modId == 0) {

        fetch("/ModuloWeb/Crear", {
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
                    swal("Listo!", "El usuario fue creado", "success")
                } else {
                    swal("Lo sentimos", responseJson.mensaje, "error")
                }
            })
    } else {

        fetch("/ModuloWeb/Editar", {
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
                    swal("Listo!", "El usuario ha sido Editado", "success")
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

    console.log(data)    

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
        text: `Eliminar el usuario "${data.usuNombre}"`,
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


                fetch(`/Usuario/Eliminar?idUsuario=${data.usuId}`, {
                    method: "DELETE",
                })
                    .then(response => {
                        $(".showSweetAlert").LoadingOverlay("hide");
                        return response.ok ? response.json() : Promise.reject(response);
                    })
                    .then(responseJson => {

                        if (responseJson.estado) {

                            tablaData.row(fila).remove().draw()
                            swal("Listo!", "El usuario fue Eliminada", "success")
                        } else {
                            swal("Lo sentimos", responseJson.mensaje, "error")
                        }
                    })
            }

        }
    )

})

$(".btn-volver").on("click", function () {

    var tabla = $('#tbdata').DataTable();
    tabla.clear().draw();

    listObjOld.sort(function (a, b) {
        return b.id - a.id;
    });

    console.log(listObjOld)

    countTranslation = listObjOld[0].id

    $("#txtTipoModulo").val(listObjOld[0].oldIdModulo)
    $("#txtidPadre").val(listObjOld[0].oldIdPadre)


    fetch("/ModuloWeb/Lista?tipoModulo=" + listObjOld[0].oldIdModulo + "&idPadre=" + listObjOld[0].oldIdPadre)
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.data.length > 0) {

                getOldValues(responseJson.data)

                console.log(responseJson.data)
                responseJson.data.forEach((item) => {
                    tabla.row.add(item).draw();
                    console.log(item)
                })

                listObjOld = listObjOld.filter(function (obj) {
                    return obj.id !== listObjOld[0].id;
                });

                if (listObjOld.length == 0) {
                    $('#volver').css('display', 'none');
                }

                console.log(listObjOld.length)

                if (listObjOld.length != 0) {
                    var elementos = document.querySelectorAll('.btn-submodulos');
                    elementos.forEach(e => console.log(e.style.display = 'none'));

                    const btn = document.querySelectorAll('.sorting_1');
                    btn.forEach(e => {
                        e.addEventListener('click', () => {
                            setTimeout(() => {
                                document.querySelectorAll('.dtr-data .btn-submodulos').forEach(e => e.style.display = "none");
                            }, 0)
                        })
                    })

                }




            }
        })

    //listObjOld = listObjOld.filter(function (obj) {
    //    return obj.id !== listObjOld[0].id;
    //});



    //if (listObjOld.length == 0) {
    //    $('#volver').css('display', 'none');
    //}



})

$("#tbdata tbody").on("click", ".btn-submodulos", function () {



    if ($(this).closest("tr").hasClass("child")) {
        filaSeleccionada = $(this).closest("tr").prev();
    } else {
        filaSeleccionada = $(this).closest("tr");
    }

    const data = tablaData.row(filaSeleccionada).data();

    var tabla = $('#tbdata').DataTable();
    tabla.clear().draw();


    var idPadre;

    if (data.modIdHijo == null) {
        idPadre = data.modIdPadre
    } else {
        idPadre = data.modIdHijo;
    }

    $("#txtTipoModulo").val("2")
    $("#txtidPadre").val(idPadre)

    getOldValues(data)

    fetch("/ModuloWeb/Lista?tipoModulo=2&idPadre=" + idPadre)
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.data.length > 0) {
                responseJson.data.forEach((item) => {
                    tabla.row.add(item).draw();
                })


                var elementos = document.querySelectorAll('.btn-submodulos');
                elementos.forEach(e => console.log(e.style.display = 'none'));

                const btn = document.querySelectorAll('.sorting_1');
                btn.forEach(e => {
                    e.addEventListener('click', () => {
                        setTimeout(() => {
                            document.querySelectorAll('.dtr-data .btn-submodulos').forEach(e => e.style.display = "none");
                        }, 0)
                    })
                })

            }
        })

})


$("#tbdata tbody").on("click", ".btn-botones", function () {



    if ($(this).closest("tr").hasClass("child")) {
        filaSeleccionada = $(this).closest("tr").prev();
    } else {
        filaSeleccionada = $(this).closest("tr");
    }

    const data = tablaData.row(filaSeleccionada).data();

    var tabla = $('#tbdata').DataTable();
    tabla.clear().draw();


    var idPadre;

    if (data.modIdHijo == null) {
        idPadre = data.modIdPadre
    } else {
        idPadre = data.modIdHijo;
    }

    $("#txtTipoModulo").val("3")
    $("#txtidPadre").val(idPadre)

    getOldValues(data)

    fetch("/ModuloWeb/Lista?tipoModulo=3&idPadre=" + idPadre)
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.data.length > 0) {
                responseJson.data.forEach((item) => {
                    tabla.row.add(item).draw();
                })


                var elementos = document.querySelectorAll('.btn-submodulos');
                elementos.forEach(e => console.log(e.style.display = 'none'));

                const btn = document.querySelectorAll('.sorting_1');
                btn.forEach(e => {
                    e.addEventListener('click', () => {
                        setTimeout(() => {
                            document.querySelectorAll('.dtr-data .btn-submodulos').forEach(e => e.style.display = "none");
                        }, 0)
                    })
                })
            }
        })

})


function getOldValues(fila) {

    let tipo = fila.idTipoModulo
    let id = fila.modIdPadre


    fetch("/ModuloWeb/Lista?tipoModulo=" + tipo + "&idPadre=" + id)
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.data.length > 0) {

                oldIdModulo = responseJson.data[0].idTipoModulo

                if (responseJson.data[0].modIdHijo == null) {
                    oldIdPadre = 0
                } else {
                    oldIdPadre = responseJson.data[0].modIdPadre
                }

                listObjOld[countTranslation] = {
                    id: countTranslation,
                    oldIdPadre: oldIdPadre,
                    oldIdModulo: oldIdModulo
                }

                countTranslation++;

                $('#volver').css('display', 'inline');

                console.log(listObjOld)
            }
        })

}


