import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { VariableCondicional } from 'src/app/model/Variables/variablecondicionada.model';
import { VariableResponseModel } from 'src/app/model/Variables/variableresponse.model';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class VariableDetailsService {

  variableCondicionadaList: VariableCondicional[] = [];
  valorConstante: string = '';
  constructor(private http: HttpClient) { }


  setVariablesCondicional(input: VariableCondicional[])
  {
    this.variableCondicionadaList = input;
  }

  getVariablesCondicional()
  {
    return this.variableCondicionadaList;
  }

  setValorConstante(input: string)
  {
    this.valorConstante = input;
  }

  getValorConstante()
  {
    return this.valorConstante;
  }



  GetVariablesFiltrado(obj:any): Observable<any>{   
    return this.http.post<any>(environment.url + environment.ModuloVariables + environment.GetVariablesFiltrado, obj)
  };

  createVarible(obj:any): Observable<any>{   
    return this.http.post<any>(environment.url + environment.ModuloVariables + environment.CrearVariables, obj)
  };

  updateVariable(obj:VariableResponseModel): Observable<any>{   
    return this.http.post<any>(environment.url + environment.ModuloVariables + environment.ActualizarVariable, obj)
  };

  updateVariables(obj:any): Observable<any>{   
    return this.http.post<any>(environment.url + environment.ModuloVariables + environment.ActualizarVariablesLider, obj)
  };

  //ToDo: recibir id catalog
  getCatalogo(): Observable<any>{   
    return this.http.get<any>(environment.url + environment.GetCalalogWhitItems + environment.GetItemById + 4)
  };

  getItemsByCatalogId(catalgoId:number): Observable<any>{   
    return this.http.get<any>(environment.url + environment.GetCalalogWhitItems + environment.GetItemById + catalgoId)
  };

  getCondicion(): Observable<any>{   
    return this.http.get<any>(environment.url + environment.GetCalalogWhitItems + environment.GetItemById + 12)
  };

  GetDefaultValue(): Observable<any>{   
    return this.http.get<any>(environment.url + environment.GetCalalogWhitItems + environment.GetItemById + 6)
  };

  getErrorOf(): Observable<any>{   
    return this.http.get<any>(environment.url + environment.GetCalalogWhitItems + environment.GetItemById + 13)
  };

  GetGrupo(): Observable<any>{   
    return this.http.get<any>(environment.url + environment.GetCalalogWhitItems + environment.GetItemById + 4)
  };

  GetTipo(): Observable<any>{   
    return this.http.get<any>(environment.url + environment.GetCalalogWhitItems + environment.GetItemById + 7)
  };

  GetCalificaciones(): Observable<any>{   
    return this.http.get<any>(environment.url + environment.GetCalalogWhitItems + environment.GetItemById + 3)
  };
  
  deleteVariables(obj:any): Observable<any>{   
    console.log(obj)
    return this.http.get<any>(environment.url + environment.ModuloVariables + "/Delete?id="+obj) //+ environment.EliminarVariables 
  };

  

  consultaVariablesCondicionadas(variableId: number, medicionId: number): Observable<any>{   
    return this.http.get<any>(environment.url + '/Variables/ConsultaVariablesCondicionadas/' + variableId + '/' + medicionId);
  };
  
}
