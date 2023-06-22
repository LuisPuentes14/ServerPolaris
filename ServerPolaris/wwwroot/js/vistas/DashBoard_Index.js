

$(document).ready(function () {

    // ESTADO DE CPU DEL SERVIDOR

    let utilizationCpu = [];
    let time = [];
    
    fetch("/DashBoard/GetInfoCPU")
        .then(response => {

            return response.ok ? response.json() : Promise.reject(response);
        }).then(responseJson => {

            console.log("respuesta")
            let count = 0;
            // console.log(responseJson.listaObjeto)
            for (let i = responseJson.listaObjeto.length - 1; i > 0; i--) {

                utilizationCpu[count] = responseJson.listaObjeto[i].otherCPUUtilization

                time[count] = responseJson.listaObjeto[i].dateTime.toString()

                count++;
            }
            count = 0;

            // Area Chart Example
            var ctx = document.getElementById("myAreaChart");
            var myLineChart = new Chart(ctx, {
                type: 'line',
                data: {
                    labels: time,
                    datasets: [{
                        label: "%",
                        lineTension: 0.3,
                        backgroundColor: "rgba(78, 115, 223, 0.05)",
                        borderColor: "rgba(49,177,49)",
                        pointRadius: 3,
                        pointBackgroundColor: "rgba(49,177,49)",
                        pointBorderColor: "rgba(49,177,49)",
                        pointHoverRadius: 3,
                        pointHoverBackgroundColor: "rgba(49,177,49)",
                        pointHoverBorderColor: "rgba(49,177,49)",
                        pointHitRadius: 10,
                        pointBorderWidth: 2,
                        data: utilizationCpu,
                    }],
                },
                options: {
                    maintainAspectRatio: false,
                    layout: {
                        padding: {
                            left: 10,
                            right: 25,
                            top: 25,
                            bottom: 0
                        }
                    },
                    scales: {
                        xAxes: [{
                            time: {
                                unit: 'date'
                            },
                            gridLines: {
                                display: false,
                                drawBorder: false
                            },
                            ticks: {
                                maxTicksLimit: 7
                            }
                        }],
                        yAxes: [{
                            ticks: {
                                maxTicksLimit: 5,
                                padding: 10,
                                // Include a dollar sign in the ticks
                                callback: function (value, index, values) {
                                    return '%' + number_format(value);
                                }
                            },
                            gridLines: {
                                color: "rgb(234, 236, 244)",
                                zeroLineColor: "rgb(234, 236, 244)",
                                drawBorder: false,
                                borderDash: [2],
                                zeroLineBorderDash: [2]
                            }
                        }],
                    },
                    legend: {
                        display: false
                    },
                    tooltips: {
                        backgroundColor: "rgb(255,255,255)",
                        bodyFontColor: "#858796",
                        titleMarginBottom: 10,
                        titleFontColor: '#6e707e',
                        titleFontSize: 14,
                        borderColor: '#dddfeb',
                        borderWidth: 1,
                        xPadding: 15,
                        yPadding: 15,
                        displayColors: false,
                        intersect: false,
                        mode: 'index',
                        caretPadding: 10,
                        callbacks: {
                            label: function (tooltipItem, chart) {
                                var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                                return datasetLabel + ' ' + number_format(tooltipItem.yLabel);
                            }
                        }
                    }
                }
            });



        });


    //ESTADO DE DISCOS DUROS DEL SERVIDOR

    fetch("/DashBoard/GetInfoHardDisk")
        .then(response => {

            return response.ok ? response.json() : Promise.reject(response);
        }).then(responseJson => {                    
                   

            for (let i =0; i <= responseJson.listaObjeto.length -1; i++) {

                let obj = {
                    dbGrowthMo: responseJson.listaObjeto[i].dbGrowthMo,
                    dbSizeMo: responseJson.listaObjeto[i].dbSizeMo,
                    drive: responseJson.listaObjeto[i].drive,
                    freeMB: responseJson.listaObjeto[i].freeMB,
                    free_pct: responseJson.listaObjeto[i].free_pct,
                    leftAfterGrowthMo: responseJson.listaObjeto[i].leftAfterGrowthMo,
                    totalMB: responseJson.listaObjeto[i].totalMB
                }

               
                var miDiv = document.getElementById("serverGraphics");

                var nuevoBr = document.createElement("br");
                miDiv.appendChild(nuevoBr);

                var nuevoDiv = document.createElement("div");

                if (i % 2 == 0) {
                    nuevoDiv.classList.add("col-xl-4", "col-lg-5", "row");                   
                } else {
                    nuevoDiv.classList.add("col-xl-4", "col-lg-5");                   
                }         

                miDiv.appendChild(nuevoDiv);

                var cardDiv = document.createElement("div");
                cardDiv.classList.add("card", "shadow", "mb-4");
                nuevoDiv.appendChild(cardDiv);

                var cardHeaderDiv = document.createElement("div");
                cardHeaderDiv.classList.add("card-header", "py-3");
                cardDiv.appendChild(cardHeaderDiv);

                var headerH6 = document.createElement("h6");
                headerH6.classList.add("m-0", "font-weight-bold", "text-black");
                headerH6.textContent = "Unidad de almacenamiento "+obj.drive;
                cardHeaderDiv.appendChild(headerH6);

                var cardBodyDiv = document.createElement("div");
                cardBodyDiv.classList.add("card-body");
                cardDiv.appendChild(cardBodyDiv);

                var chartPieDiv = document.createElement("div");
                chartPieDiv.classList.add("chart-pie", "pt-4");
                cardBodyDiv.appendChild(chartPieDiv);

                var canvasElement = document.createElement("canvas");
                canvasElement.id = "myPieChart22"+i;
                chartPieDiv.appendChild(canvasElement);

                var hrElement = document.createElement("hr");
                cardBodyDiv.appendChild(hrElement);

                var textNode = document.createTextNode("Estado de unidad de almacenamiento " + obj.drive);
                cardBodyDiv.appendChild(textNode);
               

                let espacioOcupado = obj.totalMB - obj.freeMB;
                                
                var ctx = document.getElementById("myPieChart22" + i);
                
                
                var myPieChart = new Chart(ctx, {
                    type: 'doughnut',
                    data: {
                        labels: ["Espacio ocupado", "Espacio libre"],
                        datasets: [{
                            data: [espacioOcupado, obj.freeMB],
                            backgroundColor: ['#ea1e1e', '#31B131'],
                            hoverBackgroundColor: ['#ea1e1e', '#31B131'],
                            hoverBorderColor: "rgba(234, 236, 244, 1)",
                        }],
                    },
                    options: {
                        maintainAspectRatio: false,
                        tooltips: {
                            backgroundColor: "rgb(255,255,255)",
                            bodyFontColor: "#858796",
                            borderColor: '#dddfeb',
                            borderWidth: 1,
                            xPadding: 15,
                            yPadding: 15,
                            displayColors: false,
                            caretPadding: 10,
                        },
                        legend: {
                            display: false
                        },
                        cutoutPercentage: 80,
                    },
                });

            }      

        })

    
    // INFORMACION DEL SERVIDOR
    fetch("/DashBoard/GetInfoServer")
        .then(response => {

            return response.ok ? response.json() : Promise.reject(response);
        }).then(responseJson => {

           // console.log(responseJson.listaObjeto)

            for (let i = 0; i <= responseJson.listaObjeto.length - 1; i++) { 

                var miDiv = document.getElementById("ServerInfo");

                var cardBodyDiv = document.createElement("div");
                cardBodyDiv.classList.add("card-body", "py-3");
                miDiv.appendChild(cardBodyDiv);

                var h6Element = document.createElement("h6");
                h6Element.classList.add("m-0", "font-weight-bold", "text-black");
                h6Element.textContent = "Nombre del servidor: " + responseJson.listaObjeto[i].machineName;
                cardBodyDiv.appendChild(h6Element);

                var tableDiv = document.createElement("div");
                tableDiv.classList.add("table-responsive");
                miDiv.appendChild(tableDiv);

                var tableElement = document.createElement("table");
                tableElement.classList.add("table", "table-striped", "responsibe");
                tableDiv.appendChild(tableElement);

                var theadElement = document.createElement("thead");
                tableElement.appendChild(theadElement);

                var trElement = document.createElement("tr");
                theadElement.appendChild(trElement);

                var thElements = ["Edicion", "Verion Producto", "Coleccion", "Version"];
                thElements.forEach(function (text) {
                    var thElement = document.createElement("th");
                    thElement.textContent = text;
                    trElement.appendChild(thElement);
                });

                var tbodyElement = document.createElement("tbody");
                tableElement.appendChild(tbodyElement);

                var tdData = [
                    responseJson.listaObjeto[i].edition,
                    responseJson.listaObjeto[i].productVersion,
                    responseJson.listaObjeto[i].collation,
                    responseJson.listaObjeto[i].version
                ];
                var trData = document.createElement("tr");
                tbodyElement.appendChild(trData);

                tdData.forEach(function (text) {
                    var tdElement = document.createElement("td");
                    tdElement.textContent = text;
                    trData.appendChild(tdElement);
                });



            }

        })

    // EESTADO DE LA MEMORIA RAM
    fetch("/DashBoard/GetInfoRam")
        .then(response => {

            return response.ok ? response.json() : Promise.reject(response);
        }).then(responseJson => {

             console.log(responseJson.listaObjeto)

            for (let i = 0; i <= responseJson.listaObjeto.length - 1; i++) {

                var miDiv = document.getElementById("serverGraphics");

                var nuevoBr = document.createElement("br");
                miDiv.appendChild(nuevoBr);

                var nuevoDiv = document.createElement("div");

                /*if (i % 2 == 0) {*/
                    nuevoDiv.classList.add("col-xl-4", "col-lg-5");
                //} else {
                //    nuevoDiv.classList.add("col-xl-4", "col-lg-5");
                //}

                miDiv.appendChild(nuevoDiv);

                var cardDiv = document.createElement("div");
                cardDiv.classList.add("card", "shadow", "mb-4");
                nuevoDiv.appendChild(cardDiv);

                var cardHeaderDiv = document.createElement("div");
                cardHeaderDiv.classList.add("card-header", "py-3");
                cardDiv.appendChild(cardHeaderDiv);

                var headerH6 = document.createElement("h6");
                headerH6.classList.add("m-0", "font-weight-bold", "text-black");
                headerH6.textContent = "Memoria Ram";
                cardHeaderDiv.appendChild(headerH6);

                var cardBodyDiv = document.createElement("div");
                cardBodyDiv.classList.add("card-body");
                cardDiv.appendChild(cardBodyDiv);

                var chartPieDiv = document.createElement("div");
                chartPieDiv.classList.add("chart-pie", "pt-4");
                cardBodyDiv.appendChild(chartPieDiv);

                var canvasElement = document.createElement("canvas");
                canvasElement.id = "myPieChart21" + i;
                chartPieDiv.appendChild(canvasElement);

                var hrElement = document.createElement("hr");
                cardBodyDiv.appendChild(hrElement);

                var textNode = document.createTextNode("Estado de memoria ram");
                cardBodyDiv.appendChild(textNode);

                //var codeElement = document.createElement("code");
                //codeElement.textContent = "/js/demo/chart-pie-demo.js";
                //cardBodyDiv.appendChild(codeElement);

                let espacioOcupado = responseJson.listaObjeto[i].physicalMemoryMB - responseJson.listaObjeto[i].availableMemoryMB; 

                var ctx = document.getElementById("myPieChart21" + i);


                var myPieChart = new Chart(ctx, {
                    type: 'doughnut',
                    data: {
                        labels: ["Espacio ocupado", "Espacio libre"],
                        datasets: [{
                            data: [espacioOcupado, responseJson.listaObjeto[i].availableMemoryMB],
                            backgroundColor: ['#ea1e1e', '#31B131'],
                            hoverBackgroundColor: ['#ea1e1e', '#31B131'],
                            hoverBorderColor: "rgba(234, 236, 244, 1)",
                        }],
                    },
                    options: {
                        maintainAspectRatio: false,
                        tooltips: {
                            backgroundColor: "rgb(255,255,255)",
                            bodyFontColor: "#858796",
                            borderColor: '#dddfeb',
                            borderWidth: 1,
                            xPadding: 15,
                            yPadding: 15,
                            displayColors: false,
                            caretPadding: 10,
                        },
                        legend: {
                            display: false
                        },
                        cutoutPercentage: 80,
                    },
                });



            }

        })



})