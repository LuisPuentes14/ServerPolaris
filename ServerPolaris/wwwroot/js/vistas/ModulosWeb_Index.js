
const MODELO_BASE = {
    usuId: 0,
    usuLogin: "",
    usuNombre: "",
    usuEmail: "",
    usuPassword: "",
    perfils: [{
        perfilId: 0,
        descripcion: ""
    }],
    estadoId: 1,
    isUpdatePassword : false
}

let tablaData;

$(document).ready(function () {


    //fetch("/Perfil/Lista")
    //    .then(response => {
    //        return response.ok ? response.json() : Promise.reject(response);
    //    })
    //    .then(responseJson => {
    //        if (responseJson.data.length > 0) {
    //            responseJson.data.forEach((item) => {
    //                $("#id_sc_field_groups").append(
    //                    $("<option>").val(item.perfilId).text(item.descripcion)
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

    let options = $("#id_ds_field_groups ").find("option");
    for (let i = 0; i < options.length; i++) {
        $("#id_ds_field_groups option[value='" + options[i].value + "']").remove();
    }

    $("#txtId").val(modelo.usuId)
    $("#txtUsuario").val(modelo.usuLogin)
    $("#txtNombre").val(modelo.usuNombre)
    $("#txtEmail").val(modelo.usuEmail)
    $("#cboestado").val(modelo.estadoId)
    $("#txtPassword").val(modelo.usuPassword)
    $('#exampleCheck1').prop('checked', false)


    for (let i = 0; i < modelo.perfils.length; i++) {

        if (modelo.perfils[i].perfilId != 0) {
            $("#id_ds_field_groups").append(
                $("<option>").val(modelo.perfils[i].perfilId).text(modelo.perfils[i].descripcion)
            )
        }
    }

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

    if ($("#txtUsuario").val().trim() == "") {

        toastr.warning("", "Debe completar el campo Usuario")
        $("#txtUsuario").focus()
        return;
    }

    if ($("#txtNombre").val().trim() == "") {
        toastr.warning("", "Debe completar el campo Nombre")
        $("#txtUsuario").focus()
        return;
    }

    if ($("#txtEmail").val().trim() == "") {
        toastr.warning("", "Debe completar el campo Correo")
        $("#txtEmail").focus()
        return;
    }

      

    if ($('#sectionPassword').css('display') === 'none') {
       
        if ($("#txtPassword").val().trim() == "") {
            toastr.warning("", "Debe agregar una contraseña")
            $("#txtPassword").focus()
            return;
        }

    }

    if ($('#exampleCheck1').prop('checked')) {
       
        if ($("#txtPassword").val().trim() == "") {
            toastr.warning("", "Debe agregar una contraseña")
            $("#txtPassword").focus()
            return;
        }


    }

    

    let perfiles = $("#id_ds_field_groups").find("option");

    if (perfiles.length == 0) {
        toastr.warning("", "Debe agregar almenos un perfil")
        $("#id_ds_field_groups").focus()
        return;
    }

    //$("#txtId").val(modelo.usuId)
    //$("#txtUsuario").val(modelo.usuLogin)
    //$("#txtNombre").val(modelo.usuNombre)
    //$("#txtEmail").val(modelo.usuEmail)
    //$("#cboestado").val(modelo.estadoId)
    //$("#txtPassword").val(modelo.usuPassword)
    //$('#exampleCheck1').prop('checked', false)

    const modelo = structuredClone(MODELO_BASE)
    modelo["usuId"] = parseInt($("#txtId").val())
    modelo["usuLogin"] = $("#txtUsuario").val()
    modelo["usuNombre"] = $("#txtNombre").val()
    modelo["usuEmail"] = $("#txtEmail").val()
    modelo["estadoId"] = $("#cboestado").val()
    modelo["usuPassword"] = $("#txtPassword").val()
    modelo["isUpdatePassword"] = $('#exampleCheck1').prop('checked')



    let roles = [];

    let options = $("#id_ds_field_groups").find("option");
    for (let i = 0; i < options.length; i++) {
        options[i].value

        roles[i] = {
            perfilId: options[i].value,
            descripcion: options[i].text
        }

    }
    modelo["perfils"] = roles

    console.log(modelo)


    console.log(JSON.stringify(modelo))

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    if (modelo.usuId == 0) {

        fetch("/Usuario/Crear", {
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

       

        fetch("/Usuario/Editar", {
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

    $("#sectionPassword").css('display', 'inline');
    $("#txtPassword").css('display', 'none');
    $("#labelPassword").css('display', 'none');

    const data = tablaData.row(filaSeleccionada).data();

    console.log(data)

    setValueSelect("groups_orig", data);



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

$("#tbdata tbody").on("click", ".btn-volver", function () {

    if ($(this).closest("tr").hasClass("child")) {
        filaSeleccionada = $(this).closest("tr").prev();
    } else {
        filaSeleccionada = $(this).closest("tr");
    }

    const data = tablaData.row(filaSeleccionada).data();

    var tabla = $('#tbdata').DataTable();
    tabla.clear().draw();

    console.log(data.modIdPadre)

    fetch("/ModuloWeb/Lista?tipoModulo=2&idPadre=" + data.modIdPadre)
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.data.length > 0) {

                /*tabla.row.add(responseJson.data).draw();*/

                console.log(responseJson.data)
                responseJson.data.forEach((item) => {
                    tabla.row.add(item).draw();
                    console.log(item)
                })
            }
        })

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

    console.log(data.modIdPadre)
    console.log(data.modIdHijo)

    var idPadre;

    if (data.modIdHijo == null) {
        idPadre = data.modIdPadre
    } else {
        idPadre = data.modIdHijo
    }

    console.log(idPadre)

    fetch("/ModuloWeb/Lista?tipoModulo=2&idPadre=" + idPadre )
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.data.length > 0) {

                /*tabla.row.add(responseJson.data).draw();*/

                console.log(responseJson.data)
                responseJson.data.forEach((item) => {
                    tabla.row.add(item).draw();
                    console.log(item)
                })
            }
        })

})


