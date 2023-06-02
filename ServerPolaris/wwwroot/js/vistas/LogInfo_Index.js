// logs tiempo real
var play = true;
var numLine = 0;
let list = document.getElementById("list");
var res = 0;
var ruta = document.getElementById("rutaLog").value;
// Descarga de logs

var lengthTotal = 0;
var lengthProgress = 0;
var offset = 0;
var next = 0;
var fileByte = "";
var fileByte_1 = "";
var paht;
var extension;
var date;
var countclass = 0;
var count920500 = 0;
var idterminal;
var count = 0;
var cc



function remove() {
    numLine = 0;

    var container = document.getElementById("list");
    let cupcakes = Array.prototype.slice.call(document.getElementsByClassName("white"), 0);

    for (element of cupcakes) {
        element.remove();
    }
}

function on() {
    play = true;
    recuestLogs();
}

function off() {
    play = false;
}


function recuestLogs() {
    if (play) {

        //    let json = '{"numLine":"' + numLine + '", "path": "C:\\Users\\WPOSS\\Downloads\\Servicio\\Servicio\\logTransacciones\\PolarisCloud.log"}'
        //     var request = new XMLHttpRequest();
        //     request.open("POST", "http://127.0.0.1:9091/");
        //     request.send('{"numLine":"' + numLine + '", "path": "C:\\Users\\WPOSS\\Downloads\\Servicio\\Servicio\\logTransacciones\\PolarisCloud.log"}');
        //     var ruta = "http://127.0.0.1:9091/"
        //   request.open("POST", ruta, true );
        //     request.setRequestHeader('Content-type', 'application/json; charset=utf-8');
        // request.send(json);

        var request = new XMLHttpRequest();
        request.open("POST", 'http://127.0.0.1:9091/');
        request.send(JSON.stringify({
            numLine: numLine, path: ruta
        }));

        request.onreadystatechange = function () {


            if (this.readyState === 4 && this.status === 200) {

                var data = JSON.parse(this.response);
                console.log(data)
                var flag = false;
                var json = data;

                for (var clave in json) {
                    // Controlando que json realmente tenga esa propiedad
                    if (!json.hasOwnProperty(clave)) {
                        continue;
                    }

                    if (clave == 'num') {
                        continue;
                    }

                    if (json[clave] != "Sin cambios" && json[clave] != "inicio") {
                        var patron = /[0-9]{8}/;
                        let id;
                        id = patron.exec(json[clave]);
                        if (id === null || id.length === 0) { id = "" }
                        else { id = id[0]; }
                        const p = document.createElement("p");
                        if (clave.substr(-1) % 2 === 0) {
                            p.innerHTML = `<p class="white green c${count}">${json[clave]}</p>`;
                        }
                        else {
                            p.innerHTML = `<p class="white  c${count}">${json[clave]}</p>`;
                        }
                        document.querySelector("#divu").appendChild(p);

                        var objDiv = document.getElementById("divu");
                        objDiv.scrollTop = objDiv.scrollHeight;
                    } else {
                        flag = true
                    }
                    var elementos = document.getElementsByClassName("white");
                    if (elementos.length > 100) {
                        var conutLi = 0;
                        let cupcakes = Array.prototype.slice.call(document.getElementsByClassName("white"), 0);

                        for (element of cupcakes) {
                            if (conutLi <= 50) {
                                console.log("supero el limite de lineas");
                                element.remove();
                            }
                            conutLi = conutLi + 1;
                        }
                    }
                    clase = numLine;
                    count++;
                }
                var parrafoFinal = document.querySelector(".c" + cc);

                if (!flag) {
                    numLine = parseInt(data.num) + 1;
                } else {
                    numLine = parseInt(data.num);
                    if (parrafoFinal !== null) {
                        parrafoFinal.lastElementChild.classList.remove("mark1");
                        parrafoFinal.lastElementChild.classList.add("mark2");
                    }
                }
                recuestLogs();
            }
        }
    };
}
window.onload = function () {
    recuestLogs();
};