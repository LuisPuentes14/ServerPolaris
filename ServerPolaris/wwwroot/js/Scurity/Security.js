
// archivo de seguirdad p[ara otorgar permisos segun lo que tenga otorgados el usuario


////*Bloquea los botones por clase cuando carga la pagina
////__________________________________________________________________
//var elemento = document.getElementsByClassName('btn-agregar');
//for (var i = 0; i < elemento.length; i++) {
//    elemento[i].style.display = 'none';
//}
////__________________________________________________________________




let modulosAsolocitarPermisos = [];
let botonesTabla = '';
let resposeSecurity = false;


//*Agrega la url's que van a pedir permisos
//__________________________________________________________________
Modulos.forEach(e => {
    modulosAsolocitarPermisos.push(e.NameModulo)
})


const request = {
    Modulos: modulosAsolocitarPermisos
}
//__________________________________________________________________


fetch("/Security/Index", {
    method: "POST",
    headers: { "Content-Type": "application/json;charset=utf-8" },
    body: JSON.stringify(request)
})
    .then(response => {
        return response.ok ? response.json() : Promise.reject(response);
    })
    .then(responseJson => {
        if (responseJson.estado) {          
            setPermisos(responseJson);
        }
        resposeSecurity = true;
    })


function setPermisos(response) {

    console.log(response)

    Modulos.forEach(e => {

        let mod = response.listaObjeto.find(function (objeto) {
            return objeto.nombreModulo === e.NameModulo;
        }); 

        e.perEliminar = mod.perEliminar
        e.perActualizar = mod.perActualizar
        e.perInsertar = mod.perInsertar
        e.perAcceder = mod.perAcceder       


        if (e.isTable && e.belongsCRUD) {
            setPermissionsDRUD(e);
        }

        if (e.isTable && e.isButton) {
            setPermissionsButtonTable(e);
        }

        if (!e.isTable && e.isButton) {
            setPermissionsButton(e);
        }

    })  

}

function setPermissionsDRUD(e) {   

    if (e.perActualizar) {
        botonesTabla += '<button class="btn btn-primary btn-editar btn-sm mr-2"><i class="fas fa-pencil-alt"></i></button>';
    }

    if (e.perInsertar) {
        //var elemento = document.getElementsByClassName('btn-agregar');
        //for (var i = 0; i < elemento.length; i++) {
        //    elemento[i].style.display = 'block';
        //}

        const div = document.getElementById('button_add');

        // Crea el elemento <div> con la clase "col-sm-3"
        const newDiv = document.createElement('div');
        newDiv.className = 'col-sm-3';

        // Crea el elemento <button> con las clases y el identificador
        const newBoton = document.createElement('button');
        newBoton.className = 'btn btn-success btn-add';
        newBoton.id = 'btnNuevo';
        newBoton.textContent = 'Agregar';

        // Agrega el botón al div
        newDiv.appendChild(newBoton);

        // Agrega el div al div principal
        div.appendChild(newDiv);
    }

    if (e.perEliminar) {
        botonesTabla += '<button class="btn btn-danger btn-eliminar btn-sm"><i class="fas fa-trash-alt"></i></button>';
    }

}

function setPermissionsButtonTable(e) {    

    if (e.perAcceder) {
        botonesTabla += e.HTML;
    }  

}


function setPermissionsButton(e) {

    if (e.perAcceder) {
        var elemento = document.getElementsByClassName(e.class);
        for (var i = 0; i < elemento.length; i++) {
            elemento[i].style.display = 'block';
        }
    }

}