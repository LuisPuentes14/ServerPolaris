
// archivo de seguirdad p[ara otorgar permisos segun lo que tenga otorgados el usuario
var elemento = document.getElementsByClassName('btn-agregar');
for (var i = 0; i < elemento.length; i++) {
    elemento[i].style.display = 'none';
}



let modulosAsolocitarPermisos = [];
let botonesTabla = '';
let resposeSecurity = false;

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
        return response.ok ? response.json() : Promise.reject(response);
    })
    .then(responseJson => {
        if (responseJson.estado) {
            console.log(responseJson)
            setPermisos(responseJson);
        }
        resposeSecurity = true;
    })


function setPermisos(response) {
    setPermisosModuloNotButton(getModuloNotButton(response));
}

function getModuloButton(response) {

    let mod = Modulos.filter(function (objeto) {
        return objeto.isButton === true;
    });


    let obj = response.listaObjeto.find(function (objeto) {
        return objeto.nombreModulo == mod.NameModulo;
    });

    return obj;
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

function setPermisosModuloNotButton(ModuloNotButton) {

    console.log(ModuloNotButton)

    if (ModuloNotButton.perActualizar) {
        botonesTabla += '<button class="btn btn-primary btn-editar btn-sm mr-2"><i class="fas fa-pencil-alt"></i></button>';
    }

    if (ModuloNotButton.perInsertar) {
        var elemento = document.getElementsByClassName('btn-agregar');
        for (var i = 0; i < elemento.length; i++) {
            elemento[i].style.display = 'block';
        }
    }

    if (ModuloNotButton.perEliminar) {
        botonesTabla += '<button class="btn btn-danger btn-eliminar btn-sm"><i class="fas fa-trash-alt"></i></button>';
    }

}