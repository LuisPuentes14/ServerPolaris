class Module {

    constructor(NameModulo, Class, perEliminar, perActualizar, perInsertar, perAcceder, isButton, isTable, belongsCRUD, HTML ) {
        this.NameModulo = NameModulo
        this.class = Class
        this.perEliminar = perEliminar
        this.perActualizar = perActualizar
        this.perInsertar = perInsertar
        this.perAcceder = perAcceder
        this.isButton = isButton
        this.isTable = isTable
        this.belongsCRUD = belongsCRUD
        this.HTML = HTML
    }

}