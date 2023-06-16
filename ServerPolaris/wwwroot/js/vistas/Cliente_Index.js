
const MODELO_BASE = {
    clienteId: 0,
    clienteName: ""
}


let tablaData;

$(document).ready(function () {

    tablaData = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/Clientes/Lista',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            //searchable permite al datable a realizar la busqueda
            { "data": "clienteId" },
            { "data": "clienteName" },

            {
                "defaultContent": '<button class="btn btn-primary btn-editar btn-sm mr-2" style="display:none;"><i class="fas fa-pencil-alt"></i></button>' +
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
    $("#txtId").val(modelo.clienteId)
    $("#txtNombre").val(modelo.clienteName)


    $("#modalData").modal("show")
}



$("#btnNuevo").click(function () {
    mostrarModal()
})



$("#btnGuardar").click(function () {

    if ($("#txtNombre").val().trim() == "") {

        toastr.warning("", "Debe completar el campo nombre")
        $("#txtNombre").focus()
        return;
    }

    const modelo = structuredClone(MODELO_BASE)
    modelo["clienteId"] = parseInt($("#txtId").val())
    modelo["clienteName"] = $("#txtNombre").val()

    console.log(JSON.stringify(modelo))




    //$("#modalData").find("div.modal-content").LoadingOverlay("show");

    if (modelo.clienteId == 0) {

        fetch("/Clientes/Crear", {
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
                    swal("Listo!", "El cliente fue creado", "success")
                } else {
                    swal("Lo sentimos", responseJson.mensaje, "error")
                }
            })
    } else {

        fetch("/Clientes/Editar", {
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
                    swal("Listo!", "El cliente ha sido Editado", "success")
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
        text: `Eliminar el cliente "${data.clienteName}"`,
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


                fetch(`/Clientes/Eliminar?idCliente=${data.clienteId}`, {
                    method: "DELETE",
                })
                    .then(response => {
                        $(".showSweetAlert").LoadingOverlay("hide");
                        return response.ok ? response.json() : Promise.reject(response);
                    })
                    .then(responseJson => {

                        if (responseJson.estado) {

                            tablaData.row(fila).remove().draw()
                            swal("Listo!", "El cliente fue Eliminada", "success")
                        } else {
                            swal("Lo sentimos", responseJson.mensaje, "error")
                        }
                    })
            }

        }
    )

})



let modulosAsolocitarPermisos = [];


let Modulos = [
    {
        NameModulo: "Clientes/Index",
        clase: "",
        isButton: false
    },

]

Modulos.forEach(e => {
    modulosAsolocitarPermisos.push(e.NameModulo)    
})

const request = {
    Modulos: modulosAsolocitarPermisos
}


fetch("/Security/Index", {
    method: "POST",
    headers: { "Content-Type": "application/json;charset=utf-8" },
    body: JSON.stringify(request)
})
    .then(response => {
        //$("#modalData").find("div.modal-content").LoadingOverlay("hide");
        return response.ok ? response.json() : Promise.reject(response);
    })
    .then(responseJson => {

        if (responseJson.estado) {  
            
            setPermisos(responseJson);     

        } else {

        }
    })


function setPermisos(response) {
    var intervalo = setInterval(function () {
        if (tablaData != undefined) {

           
            console.log(response.listaObjeto)          

            let ModuloNotButton = getModuloNotButton(response)      


           

            clearInterval(intervalo);
            console.log("ssss", tablaData)
        }
    }, 0);

}


function getModuloNotButton(response) {

    let mod = Modulos.find(function (objeto) {
        return objeto.isButton === false;
    });
    

    let obj = response.listaObjeto.find(function (objeto) {
        return objeto.nombreModulo == mod.NameModulo;
    }); 

    return obj;
}




