
let Modulos = [

    new Module(
        "LogCliente/Index", //NameModulo
        "",                 //class
        false,              //perEliminar
        false,              //perActualizar
        false,              //perInsertar
        false,              //perAcceder
        false,              //isButton   
        true,               //isTable
        true,               //belongsCRUD
        "",                  //HTML
        ''
    ), 
    new Module(
        "LogInfo/Index", //NameModulo
        "",                 //class
        false,              //perEliminar
        false,              //perActualizar
        false,              //perInsertar
        false,              //perAcceder
        true,              //isButton   
        true,               //isTable
        false,               //belongsCRUD
        '<button class="btn btn-warning btn-view btn-sm"><i class="fa fa-cog" aria-hidden="true"></i></button>',                 //HTML
        ''
    )
]
