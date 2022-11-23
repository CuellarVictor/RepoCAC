import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CatalogoRequest } from 'src/app/model/catalogo/catalogorequest.model';
import { ItemConf } from 'src/app/model/catalogo/itemConf.model';
import { ItemFiltroRequest } from 'src/app/model/catalogo/itemFiltroRequest.model';
import { ItemReponse } from 'src/app/model/catalogo/itemResponse.model';
import { NewCatalogoRequest } from 'src/app/model/catalogo/newCatalogoRequest.model';
import { NewCodigo } from 'src/app/model/codigo/newCodigo.model';
import { LiderEntidadRequest } from 'src/app/model/lider_entidad/lider_entidad_request.model';
import { RequestRol } from 'src/app/model/roles/requestRol.model';
import { environment } from 'src/environments/environment';


@Injectable({
  providedIn: 'root'
})
export class ConfigService {

  constructor(private http: HttpClient) { }

  getUsers(): Observable<any>{   
    return this.http.get<any>(environment.url + environment.GestionUsuariosControllercs + environment.GetUsers)
  };

  getCatalog(id:any): Observable<any>{
    return this.http.get<any>(environment.url + environment.GetCalalogWhitItems + environment.GetItemById+id)
  };

  getItemWhitCatalog(id:number){
    return this.http.get<any>(environment.url + environment.GetCalalogWhitItems + environment.GetCatalogoWhitItemsById +id)
  }

  addItemWhitCatalog(obj:any){
    return this.http.post<any>(environment.url + environment.GetCalalogWhitItems + environment.AddCatalogoWhitItemsById, obj)
  }

  updateItemWhitCatalog(obj:any){
    return this.http.post<any>(environment.url + environment.GetCalalogWhitItems + environment.UpdateCatalogoWhitItemsById, obj)
  }

  deleteItemWhitCatalog(id:number){
    return this.http.get<any>(environment.url + environment.GetCalalogWhitItems + environment.DeleteCatalogoWhitItemsById +id)
  }

  deleteItem(id:number){
    return this.http.get<any>(environment.url + environment.GetCalalogWhitItems + environment.DeleteItem +id)
  }

  getItemById(id:number){
    return this.http.get<any>(environment.url + environment.GetCalalogWhitItems + environment.GetItemById +id)
  }

  getFiltroCatalogo(obj: CatalogoRequest){
    return this.http.post<any>(environment.url + '/CatalogoCobertura/GetCatalogoCoberturaFiltrado',obj)
  }

  postCatalogoCobertura(obj: NewCatalogoRequest){
    return this.http.post<any>(environment.url + '/CatalogoCobertura/Post',obj)
  }
  updateCatalogoCobertura(id:string,obj: NewCatalogoRequest){
    return this.http.post<any>(environment.url + '/CatalogoCobertura/Update/'+id,obj)
  }

  deleteCatalogoCobertura(id:string){
    return this.http.get<any>(environment.url + '/CatalogoCobertura/Delete/'+id)   
  }

  getFiltroItem(obj: ItemFiltroRequest){
    return this.http.post<any>(environment.url + '/ItemCobertura/GetCatalogoItemCoberturaFiltrado/',obj)
  }

  postItemCobertura(obj: ItemReponse){
    return this.http.post<any>(environment.url + '/ItemCobertura/Create',obj)
  }
  updateItemCobertura(id:string,obj: ItemReponse){
    return this.http.post<any>(environment.url + '/ItemCobertura/Update?id='+id,obj)
  }

  deleteItemCobertura(id:string){
    return this.http.get<any>(environment.url + '/ItemCobertura/Delete?id='+id)   
  }

  crearItem( obj: ItemConf , iduser : string){
    return this.http.post<any>(environment.url + '/Item/Create?UsuarioId='+iduser,obj) 
  }

  actualizarItem( obj: ItemConf, iduser : string){
    return this.http.post<any>(environment.url + '/Item/Update?UsuarioId='+iduser,obj)
  }

  eliminarItem(id:string,iduser: string){
    return this.http.get<any>(environment.url + '/Item/Delete?id='+id+'&UsuarioId='+iduser)   
  }
  

  getFiltroCodigo(obj: CatalogoRequest){
    return this.http.post<any>(environment.url + '/BancoInformacion/GetBancoInformacionFiltrado',obj)
  }

  postCodigo(obj: NewCodigo){
    return this.http.post<any>(environment.url + '/BancoInformacion',obj)
  }
  updateCodigo(id:string,obj: NewCodigo){
    return this.http.post<any>(environment.url + '/BancoInformacion/'+id,obj)
  }

  deleteCodigo(id:string){
    return this.http.get<any>(environment.url + '/BancoInformacion/Delete?id='+id)   
  }

  descargarPlantillaBancoCodigo(){
    return this.http.post<any>(environment.url + '/BancoInformacion/CargarBancoInformacionPlantilla',null)
  }
  cargarArchivoBancoCodigo(obj : any){
    return this.http.post<any>(environment.url + '/BancoInformacion/CargarBancoInformacion',obj)
  }

  getRoles(){
    return this.http.get<any>(environment.url + '/permisos/ConsultaRoles'); 
  }

  consultarPermisosRol(idRol: string){
    return this.http.get<any>(environment.url + '/permisos/ConsultaPermisosRol/'+idRol); 
  }

  actualizarPermisos(obj: any){
    return this.http.post<any>(environment.url + '/permisos/UpsertPermisoRol',obj); 
  }


  getAdminRoles(){
    return this.http.get<any>(environment.url + '/Roles/GetAll'); 
  }

  crearRol(obj : RequestRol){
    return this.http.post<any>(environment.url + '/Roles/Create',obj)
  }

  updateRol(id:string,obj : RequestRol){
    return this.http.post<any>(environment.url + '/Roles/Update?id='+id,obj)
  }

  deleteRol(id: string){
    return this.http.get<any>(environment.url + '/Roles/Delete?id='+id); 
  }

  consultaAsignacionLiderEntidad(data: LiderEntidadRequest): Observable<any>{
    return this.http.post<any>(environment.url + '/GestionUsuariosControllercs/ConsultaAsignacionLiderEntidad', data);
  } 

  consultaAuditoresAsignacionLiderEntidad(data: any): Observable<any>{
    return this.http.post<any>(environment.url + '/GestionUsuariosControllercs/ConsultaAuditoresAsignacionLiderEntidad', data);
  } 

  setLiderAuditorEps(data: any): Observable<any>{
    return this.http.post<any>(environment.url+ '/GestionUsuariosControllercs/UpsertLiderEPS', data);
  }

  consultaPeriodosCobertura(idCobertura: number): Observable<any>{
    return this.http.get<any>(environment.url+ '/GestionUsuariosControllercs/ConsultaPeriodosCobertura/' + idCobertura);
  }  
  

  //Actas ----------

  consultaListadoEPS(idCobertura: number): Observable<any>
  {
    let path = environment.url + '/Medicion/ObtenerListadoDeEPS?idCobertura='
      return this.http.get<any>(path + idCobertura);
  }
  consultaParametrosTemplate(): Observable<any>
  {
    let path = environment.url + '/Acta/ConsultaParametrosTemplate'
      return this.http.get<any>(path);
  }

  upsertParametroTemplate(data: any): Observable<any>{
    return this.http.post<any>(environment.url+ '/Acta/UpsertParametroTemplate', data);
  }
  
  generarActa(data: any): Observable<any>{
    return this.http.post<any>(environment.url+ '/Acta/GenerarActa', data);
  }




}
