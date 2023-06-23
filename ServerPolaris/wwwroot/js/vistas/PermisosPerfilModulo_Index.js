

let tablaData;

$(document).ready(function () {


    tablaData = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/PermisosPerfilModulo/Lista',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            //searchable permite al datable a realizar la busqueda
            { "data": "perId" },
            { "data": "nombrePerfil" },
            { "data": "nombreModulo" },
            { "data": "tipoModulo" },
            {
                "data": "perAcceder", render: function (data) {

                    if (!data) {
                        return '<input class="Acceder" onchange="changeAcceder(this)"  type="checkbox" value="" id="flexCheckAcceder">';
                    }
                    return '<input class="Acceder" onchange="changeAcceder(this)" type="checkbox" value="" id="flexCheckChecked" checked>';
                }
            },
            {
                "data": "perInsertar", render: function (data) {

                    if (!data) {
                        return '<input class="Insertar"  onchange="changeInsertar(this)" type="checkbox" value="" id="flexCheckInsertar">';
                    }
                    return '<input class="Insertar"  onchange="changeInsertar(this)" type="checkbox" value="" id="flexCheckChecked" checked>';
                }
            },
            {
                "data": "perActualizar", render: function (data) {

                    if (!data) {
                        return '<input class="Actualizar"  onchange="changeActualizar(this)" type="checkbox" value="" id="flexCheckActualizar">';
                    }
                    return '<input class="Actualizar"   onchange="changeActualizar(this)" type="checkbox" value="" id="flexCheckChecked" checked>';
                }
            },
            {
                "data": "perEliminar", render: function (data) {

                    if (!data) {
                        return '<input class="Eliminar" onchange="changeEliminar(this)" type="checkbox" value="" id="flexCheckEliminar">';
                    }
                    return '<input class="Eliminar"  onchange="changeEliminar(this)" type="checkbox" value="" id="flexCheckChecked" checked>';
                }
            },           
        ],
        order: [[0, "desc"]],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });

})



function changeInsertar(obj) {

    let value = obj.checked;

    const data = getElemento(obj);

    data.perInsertar = value;

    console.log(data)
    sendData(data)

}

function changeActualizar(obj) {

    let value = obj.checked;

    const data = getElemento(obj);

    data.perActualizar = value;

    console.log(data)
    sendData(data)

}

function changeAcceder(obj) {

    let value = obj.checked;

    const data = getElemento(obj);

    data.perAcceder = value;

    console.log(data)
    sendData(data)

}


function changeEliminar(obj) {
    
    let value = obj.checked;

    const data = getElemento(obj);

    data.perEliminar = value;

    console.log(data)

    sendData(data)

}



let filaSeleccionada;
function getElemento(obj) {

    if ($(obj).closest("tr").hasClass("child")) {
        filaSeleccionada = $(obj).closest("tr").prev();
    } else {
        filaSeleccionada = $(obj).closest("tr");
    }

    return tablaData.row(filaSeleccionada).data();

}

function sendData(obj) {
    fetch("/PermisosPerfilModulo/Editar", {
        method: "PUT",
        headers: { "Content-Type": "application/json;charset=utf-8" },
        body: JSON.stringify(obj)
    })
        .then(response => {
            
            return response.ok ? response.json() : toastr.error("", "Error actualizando");
        })
        .then(responseJson => {

            if (responseJson.estado) {
                toastr.success("", "Actualizado");                
            } 
        })


}

