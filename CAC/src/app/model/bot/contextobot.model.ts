export interface Archivo{

    type: number,
    text: string
}

export interface Pagina{

    type: string,
    text: string
}

export interface Texto{

    type: string,
    text: string
}

export interface PalabraClave{

    type: string,
    text: string
}

export interface Fecha{

    type: string,
    text: string
}

export interface Parrafo{

    texto: Texto,
    palabraClave: PalabraClave,
    fecha: Fecha,
    type: string,
    text: string,
}

export interface Parrafos{

    parrafo: Parrafo,
    type: string,
    text: string,
}

export interface Contexto{

    pagina: Pagina,
    parrafos: Parrafos,
    type: string,
    text: string,
}

export interface Contextos{

    contexto: Contexto[]
    type: string,
    text: string,
}

export interface Documento{

    archivo: Archivo
    contextos: Contextos,
    type: string,
    text: string,
}

export interface Documentos{

    documento: Documento[]
    type: string,
    text: string,
}

export interface ContextoModel{

    documentos: Documentos

}