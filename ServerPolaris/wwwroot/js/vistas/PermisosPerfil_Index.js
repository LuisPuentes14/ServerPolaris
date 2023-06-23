

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
                language: {
                    url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
                },
            });
        }
    }, 0);
})





let filaSeleccionada;
$("#tbdata tbody").on("click", ".btn-editar", function () {

    if ($(this).closest("tr").hasClass("child")) {
        filaSeleccionada = $(this).closest("tr").prev();
    } else {
        filaSeleccionada = $(this).closest("tr");
    }

    const data = tablaData.row(filaSeleccionada).data();

    window.location.href = "/PermisosPerfilModulo/Index?id=" + data.perfilId

})




