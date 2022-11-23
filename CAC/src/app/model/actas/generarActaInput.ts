
export class GenerarActaInput
{
    idTemplate : number = 0;
    idCobertura : number = 0;
    idEPS : string = "";
    listaParametros : Parametro[]  = [];

}


export class Parametro
{
    parametroTemplateKey : string = "";
    value : string  = "";

}