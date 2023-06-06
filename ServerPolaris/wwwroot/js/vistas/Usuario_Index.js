
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
    estadoId: 1
}

let tablaData;

$(document).ready(function () {


    fetch("/Perfil/Lista")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.data.length > 0) {
                responseJson.data.forEach((item) => {
                    $("#id_sc_field_groups").append(
                        $("<option>").val(item.perfilId).text(item.descripcion)
                    )
                })
            }
        })



    tablaData = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/Usuario/Lista',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            //searchable permite al datable a realizar la busqueda
            { "data": "usuId" },
            { "data": "usuLogin" },
            { "data": "usuNombre" },
            { "data": "usuEmail" },
            {
                "data": "perfils", render: function (data) {

                    let roles = "";
                    for (let i = 0; i < data.length; i++) {
                        roles += "*" + data[i].descripcion + " "
                    }

                    return roles;
                }
            },
            {
                "data": "estadoId", render: function (data) {
                    if (data == 1)
                        return '<span class="badge badge-info">Activo</span>';
                    else
                        return '<span class="badge badge-danger">Desactivo</span>';
                }
            },

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

    let options = $("#id_ds_field_groups ").find("option");
    for (let i = 0; i < options.length; i++) {       
        $("#id_ds_field_groups option[value='" + options[i].value + "']").remove();
    }

    $("#txtId").val(modelo.usuId)
    $("#txtUsuario").val(modelo.usuLogin)
    $("#txtNombre").val(modelo.usuNombre)
    $("#txtEmail").val(modelo.usuEmail)
    $("#cboestado").val(modelo.estadoId)

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
    mostrarModal()
})



$("#btnGuardar").click(function () {

    //if ($("#txtRutaLog").val().trim() == "") {

    //    toastr.warning("", "Debe completar el campo Ruta log")
    //    $("#txtRutaLog").focus()
    //    return;
    //}

    const modelo = structuredClone(MODELO_BASE)
    modelo["usuId"] = parseInt($("#txtId").val())
    modelo["usuLogin"] = $("#txtUsuario").val()
    modelo["usuNombre"] = $("#txtNombre").val()
    modelo["usuEmail"] = $("#txtEmail").val()
    modelo["estadoId"] = $("#cboestado").val()
    modelo["usuPassword"] = $("#txtPassword").val()



    let roles = [];

    let options = $("#id_ds_field_groups ").find("option");
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





//PERFILES USUSARIO

// disabled un elemento
//----------------------

function setValueSelect(sOrig, modelo) {

    oOrig = document.F1.elements[sOrig];

    for (i = 0; i < oOrig.length; i++) {

        for (let j = 0; j < modelo.perfils.length; j++) {

            let id = parseInt(oOrig.options[i].value)

            if (modelo.perfils[j].perfilId == id) {
                oOrig.options[i].disabled = true;
            }

        }
    }

}




// Adiciona um elemento
//----------------------
function nm_add_sel(sOrig, sDest, fCBack, sRow) {
    //scMarkFormAsChanged();
    // Recupera objetos
    oOrig = document.F1.elements[sOrig];
    oDest = document.F1.elements[sDest];
    // Varre itens da origem
    for (i = 0; i < oOrig.length; i++) {
        // Item na origem selecionado e valido
        if (oOrig.options[i].selected && !oOrig.options[i].disabled) {
            // Recupera valores da origem
            sText = oOrig.options[i].text;
            sValue = oOrig.options[i].value;
            // Cria item no destino
            oDest.options[oDest.length] = new Option(sText, sValue);
            // Desabilita item na origem
            oOrig.options[i].style.color = "#A0A0A0";
            oOrig.options[i].disabled = true;
            oOrig.options[i].selected = false;
        }
    }
    // Reset combos
    oOrig.selectedIndex = -1;
    oDest.selectedIndex = -1;
    if (fCBack) {
        fCBack(sRow);
    }
}
// Adiciona todos os elementos
//-----------------------------
function nm_add_all(sOrig, sDest, fCBack, sRow) {
    //scMarkFormAsChanged();
    // Recupera objetos
    oOrig = document.F1.elements[sOrig];
    oDest = document.F1.elements[sDest];
    // Varre itens da origem
    for (i = 0; i < oOrig.length; i++) {
        // Item na origem valido
        if (!oOrig.options[i].disabled) {
            // Recupera valores da origem
            sText = oOrig.options[i].text;
            sValue = oOrig.options[i].value;
            // Cria item no destino
            oDest.options[oDest.length] = new Option(sText, sValue);
            // Desabilita item na origem
            oOrig.options[i].style.color = "#A0A0A0";
            oOrig.options[i].disabled = true;
            oOrig.options[i].selected = false;
        }
    }
    // Reset combos
    oOrig.selectedIndex = -1;
    oDest.selectedIndex = -1;
    if (fCBack) {
        fCBack(sRow);
    }
}
// Remove um elemento
//--------------------
function nm_del_sel(sOrig, sDest, fCBack, sRow) {
    //scMarkFormAsChanged();
    // Recupera objetos
    oOrig = document.F1.elements[sOrig];
    oDest = document.F1.elements[sDest];
    aSel = new Array();
    atxt = new Array();
    solt = new Array();
    j = 0;
    z = 0;
    // Remove itens selecionados na origem
    for (i = oOrig.length - 1; i >= 0; i--) {
        // Item na origem selecionado
        if (oOrig.options[i].selected) {
            aSel[j] = oOrig.options[i].value;
            atxt[j] = oOrig.options[i].text;
            j++;
            oOrig.options[i] = null;
        }
    }
    // Habilita itens no destino
    for (i = 0; i < oDest.length; i++) {
        if (oDest.options[i].disabled && in_array(aSel, oDest.options[i].value)) {
            oDest.options[i].disabled = false;
            oDest.options[i].style.color = "";
            solt[z] = oDest.options[i].value;
            z++;
        }
    }
    for (i = 0; i < aSel.length; i++) {
        if (!in_array(solt, aSel[i])) {
            oDest.options[oDest.length] = new Option(atxt[i], aSel[i]);
        }
    }
    // Reset combos
    oOrig.selectedIndex = -1;
    oDest.selectedIndex = -1;
    if (fCBack) {
        fCBack(sRow);
    }
}
// Remove todos os elementos
//---------------------------
function nm_del_all(sOrig, sDest, fCBack, sRow) {
    // scMarkFormAsChanged();
    // Recupera objetos
    oOrig = document.F1.elements[sOrig];
    oDest = document.F1.elements[sDest];
    aSel = new Array();
    atxt = new Array();
    solt = new Array();
    j = 0;
    z = 0;
    // Remove todos os itens na origem
    while (0 < oOrig.length) {
        i = oOrig.length - 1;
        aSel[j] = oOrig.options[i].value;
        atxt[j] = oOrig.options[i].text;
        j++;
        oOrig.options[i] = null;
    }
    // Habilita itens no destino
    for (i = 0; i < oDest.length; i++) {
        if (oDest.options[i].disabled && in_array(aSel, oDest.options[i].value)) {
            oDest.options[i].disabled = false;
            oDest.options[i].style.color = "";
            solt[z] = oDest.options[i].value;
            z++;
        }
    }
    for (i = 0; i < aSel.length; i++) {
        if (!in_array(solt, aSel[i])) {
            oDest.options[oDest.length] = new Option(atxt[i], aSel[i]);
        }
    }
    // Reset combos
    oOrig.selectedIndex = -1;
    oDest.selectedIndex = -1;
    if (fCBack) {
        fCBack(sRow);
    }
}
function nm_sincroniza(sOrig, sDest) {
    // Recupera objetos
    oOrig = document.F1.elements[sOrig];
    oDest = document.F1.elements[sDest];
    // Varre itens do destino
    for (i = 0; i < oDest.length; i++) {
        dValue = oDest.options[i].value;
        bFound = false;
        for (x = 0; x < oOrig.length && !bFound; x++) {
            oValue = oOrig.options[x].value;
            if (dValue == oValue) {
                // Desabilita item na origem
                oOrig.options[x].style.color = "#A0A0A0";
                oOrig.options[x].disabled = true;
                oOrig.options[x].selected = false;
                bFound = true;
            }
        }
    }
}
var nm_quant_pack;
function nm_pack(sOrig, sDest) {
    if (!document.F1.elements[sOrig] || !document.F1.elements[sDest]) return;
    obj_sel = document.F1.elements[sOrig];
    str_val = "";
    nm_quant_pack = 0;
    for (i = 0; i < obj_sel.length; i++) {
        if ("" != str_val) {
            str_val += "@?@";
            nm_quant_pack++;
        }
        str_val += obj_sel.options[i].value;
    }
    document.F1.elements[sDest].value = str_val;
}
function nm_pack_sel(sOrig, sDest) {
    if (!document.F1.elements[sOrig] || !document.F1.elements[sDest]) return;
    obj_sel = document.F1.elements[sOrig];
    str_val = "";
    nm_quant_pack = 0;
    for (i = 0; i < obj_sel.length; i++) {
        if (obj_sel.options[i].selected) {
            if ("" != str_val) {
                str_val += "@?@";
                nm_quant_pack++;
            }
            str_val += obj_sel.options[i].value;
        }
    }
    document.F1.elements[sDest].value = str_val;
}
function nm_del_combo(sOcombo) {
    // Recupera objetos
    oOrig = document.F1.elements[sOcombo];
    aSel = new Array();
    j = 0;
    // Remove todos os itens na origem
    while (0 < oOrig.length) {
        i = oOrig.length - 1;
        aSel[j] = oOrig.options[i].value;
        j++;
        oOrig.options[i] = null;
    }
}
function nm_add_item(sDest, sText, sValue, sSelected) {
    oDest = document.F1.elements[sDest];
    oDest.options[oDest.length] = new Option(sText, sValue);
    if (sSelected == 'selected') {
        oDest.options[oDest.length - 1].selected = true;
    }
}
function in_array(aArray, sElem) {
    for (iCount = 0; iCount < aArray.length; iCount++) {
        if (sElem == aArray[iCount]) {
            return true;
        }
    }
    return false;
}
var scInsertFieldWithErrors = new Array();
