import { Injectable } from '@angular/core';
import { PermisoModel } from '../model/permiso/permiso-model.interface';
import { enumPermisos } from '../model/Util/enumerations.enum';

@Injectable({
  providedIn: 'root'
})
export class GetPermisosServices {
       
    _enumPermisos : any = enumPermisos;		

    // habilitado , visible
    Gestor_de_Mediciones	=	{ habilitado: false , visible : false };
    Gestor_de_Usuarios	=	{ habilitado: false , visible : false };
    Cronograma	=	{ habilitado: false , visible : false };
    Configuracion	=	{ habilitado: false , visible : false };
    Cargar_Poblacion	=	{ habilitado: false , visible : false };
    Mover_Registros	=	{ habilitado: false , visible : false };
    Asignacion	=	{ habilitado: false , visible : false };
    Editar_Medicion	=	{ habilitado: false , visible : false };
    Duplicar_Medicion	=	{ habilitado: false , visible : false };
    Ver_Variables	=	{ habilitado: false , visible : false };
    Eliminar_Medicion	=	{ habilitado: false , visible : false };
    Nueva_medicion	=	{ habilitado: false , visible : false };
    Crear_Variable	=	{ habilitado: false , visible : false };
    Editar_Variable	=	{ habilitado: false , visible : false };
    Nuevo_usuario	=	{ habilitado: false , visible : false };
    Editar_Usuario	=	{ habilitado: false , visible : false };
    Inactivar_Usuario	=	{ habilitado: false , visible : false };
    Eliminar_Usuario	=	{ habilitado: false , visible : false };
    Log_Usuario	=	{ habilitado: false , visible : false };
    Parametrizacion_calificacion_IPS	=	{ habilitado: false , visible : false };
    Agregar_nuevo_Item_IPS	=	{ habilitado: false , visible : false };
    Grupo__de_Variables	=	{ habilitado: false , visible : false };
    Agregar_nuevo_item_Grupos_de_Variables	=	{ habilitado: false , visible : false };
    Administracion_de_Catalogos	=	{ habilitado: false , visible : false };
    Nuevo_Catalogo	=	{ habilitado: false , visible : false };
    Items	=	{ habilitado: false , visible : false };
    Eliminar_Catalogo	=	{ habilitado: false , visible : false };
    Editar_Catalogo	=	{ habilitado: false , visible : false };
    Crear_Item_Cobertura	=	{ habilitado: false , visible : false };
    Eliminar_Item_Cobertura	=	{ habilitado: false , visible : false };
    Editar_Item_Cobertura	=	{ habilitado: false , visible : false };
    Codigos_administrativos	=	{ habilitado: false , visible : false };
    Crear_Codigo_administrativo	=	{ habilitado: false , visible : false };
    Editar_Codigo_administrativo	=	{ habilitado: false , visible : false };
    Eliminar_Codigo_administrativo	=	{ habilitado: false , visible : false };
    Cargar_plantilla_codigos_administrativos	=	{ habilitado: false , visible : false };
    Auditoria	=	{ habilitado: false , visible : false };
    Errores	=	{ habilitado: false , visible : false };
    Soportes	=	{ habilitado: false , visible : false };
    Detalles_del_registro_en_auditoria	=	{ habilitado: false , visible : false };
    CalificacionIPS	=	{ habilitado: false , visible : false };
    Banco_de_Informacion	=	{ habilitado: false , visible : false };
    Comite_adminsitrativo	=	{ habilitado: false , visible : false };
    Comite_Experto	=	{ habilitado: false , visible : false };
    Mantener_calificacion	=	{ habilitado: false , visible : false };
    Levantar_Glosa	=	{ habilitado: false , visible : false };
    Editar	=	{ habilitado: false , visible : false };
    Guardar	=	{ habilitado: false , visible : false };
    Administracion_rol  =	{ habilitado: false , visible : false };
    Tipo_codigos_administrativo_rol  =	{ habilitado: false , visible : false };
    Eliminar_Variable =	{ habilitado: false , visible : false };
    Eliminar_Registro_Auditoria =	{ habilitado: false , visible : false };       
    Consulta_Asignacion_lider_por_entidad =	{ habilitado: false , visible : false };       
    Asignar_auditor_lider =	{ habilitado: false , visible : false };  
    Calificacion_Masiva = { habilitado: false , visible : false };    
    Permisos_Accesos = { habilitado: false , visible : false };    
    Modificar_Fecha_Final_Medicion = { habilitado: false , visible : false }; 
    Parametrizar_Actas = { habilitado: false , visible : false };   
    Generar_Actas = { habilitado: false , visible : false };
    
listaPermisos!: PermisoModel[];

  constructor() {
      this.contruir();        

   }

   

   getPermiso(id: number):any{
        const temp=this.listaPermisos.find(e => e.id === id);
        if(temp){
            return { habilitado: temp?.habilitado , visible : temp?.visible };
        }else{
            return { habilitado: false , visible : false };
        }
   }

   contruir(){
    this.listaPermisos=JSON.parse(atob(sessionStorage.getItem("permisoRol") || '{}')); 
    this.Gestor_de_Mediciones= this.getPermiso(this._enumPermisos.Gestor_de_Mediciones);
    this.Gestor_de_Usuarios= this.getPermiso(this._enumPermisos.Gestor_de_Usuarios);
    this.Cronograma= this.getPermiso(this._enumPermisos.Cronograma);
    this.Configuracion= this.getPermiso(this._enumPermisos.Configuracion);
    this.Cargar_Poblacion= this.getPermiso(this._enumPermisos.Cargar_Poblacion);
    this.Mover_Registros= this.getPermiso(this._enumPermisos.Mover_Registros);
    this.Asignacion= this.getPermiso(this._enumPermisos.Asignacion);
    this.Editar_Medicion= this.getPermiso(this._enumPermisos.Editar_Medicion);
    this.Duplicar_Medicion= this.getPermiso(this._enumPermisos.Duplicar_Medicion);
    this.Ver_Variables= this.getPermiso(this._enumPermisos.Ver_Variables);
    this.Eliminar_Medicion= this.getPermiso(this._enumPermisos.Eliminar_Medicion);
    this.Nueva_medicion= this.getPermiso(this._enumPermisos.Nueva_medicion);
    this.Crear_Variable= this.getPermiso(this._enumPermisos.Crear_Variable);
    this.Editar_Variable= this.getPermiso(this._enumPermisos.Editar_Variable);
    this.Nuevo_usuario= this.getPermiso(this._enumPermisos.Nuevo_usuario);
    this.Editar_Usuario= this.getPermiso(this._enumPermisos.Editar_Usuario);
    this.Inactivar_Usuario= this.getPermiso(this._enumPermisos.Inactivar_Usuario);
    this.Eliminar_Usuario= this.getPermiso(this._enumPermisos.Eliminar_Usuario);
    this.Log_Usuario= this.getPermiso(this._enumPermisos.Log_Usuario);
    this.Parametrizacion_calificacion_IPS= this.getPermiso(this._enumPermisos.Parametrizacion_calificacion_IPS);
    this.Agregar_nuevo_Item_IPS= this.getPermiso(this._enumPermisos.Agregar_nuevo_Item_IPS);
    this.Grupo__de_Variables= this.getPermiso(this._enumPermisos.Grupo__de_Variables);
    this.Agregar_nuevo_item_Grupos_de_Variables= this.getPermiso(this._enumPermisos.Agregar_nuevo_item_Grupos_de_Variables);
    this.Administracion_de_Catalogos= this.getPermiso(this._enumPermisos.Administracion_de_Catalogos);
    this.Nuevo_Catalogo= this.getPermiso(this._enumPermisos.Nuevo_Catalogo);
    this.Items= this.getPermiso(this._enumPermisos.Items);
    this.Eliminar_Catalogo= this.getPermiso(this._enumPermisos.Eliminar_Catalogo);
    this.Editar_Catalogo= this.getPermiso(this._enumPermisos.Editar_Catalogo);
    this.Crear_Item_Cobertura= this.getPermiso(this._enumPermisos.Crear_Item_Cobertura);
    this.Eliminar_Item_Cobertura= this.getPermiso(this._enumPermisos.Eliminar_Item_Cobertura);
    this.Editar_Item_Cobertura= this.getPermiso(this._enumPermisos.Editar_Item_Cobertura);
    this.Codigos_administrativos= this.getPermiso(this._enumPermisos.Codigos_administrativos);
    this.Crear_Codigo_administrativo= this.getPermiso(this._enumPermisos.Crear_Codigo_administrativo);
    this.Editar_Codigo_administrativo= this.getPermiso(this._enumPermisos.Editar_Codigo_administrativo);
    this.Eliminar_Codigo_administrativo= this.getPermiso(this._enumPermisos.Eliminar_Codigo_administrativo);
    this.Cargar_plantilla_codigos_administrativos= this.getPermiso(this._enumPermisos.Cargar_plantilla_codigos_administrativos);
    this.Auditoria= this.getPermiso(this._enumPermisos.Auditoria);
    this.Errores= this.getPermiso(this._enumPermisos.Errores);
    this.Soportes= this.getPermiso(this._enumPermisos.Soportes);
    this.Detalles_del_registro_en_auditoria= this.getPermiso(this._enumPermisos.Detalles_del_registro_en_auditoria);
    this.CalificacionIPS= this.getPermiso(this._enumPermisos.CalificacionIPS);
    this.Banco_de_Informacion= this.getPermiso(this._enumPermisos.Banco_de_Informacion);
    this.Comite_adminsitrativo= this.getPermiso(this._enumPermisos.Comite_adminsitrativo);
    this.Comite_Experto= this.getPermiso(this._enumPermisos.Comite_Experto);
    this.Mantener_calificacion= this.getPermiso(this._enumPermisos.Mantener_calificacion);
    this.Levantar_Glosa= this.getPermiso(this._enumPermisos.Levantar_Glosa);
    this.Editar= this.getPermiso(this._enumPermisos.Editar);
    this.Guardar= this.getPermiso(this._enumPermisos.Guardar);
    this.Administracion_rol  =	this.getPermiso(this._enumPermisos.Administracion_rol);
    this.Tipo_codigos_administrativo_rol  =	this.getPermiso(this._enumPermisos.Tipo_codigos_administrativo_rol);
    this.Eliminar_Variable = this.getPermiso(this._enumPermisos.Eliminar_Variable);
    this.Eliminar_Registro_Auditoria = this.getPermiso(this._enumPermisos.Eliminar_Registro_Auditoria);
    this.Consulta_Asignacion_lider_por_entidad= this.getPermiso(this._enumPermisos.Consulta_Asignacion_lider_entidad);
    this.Asignar_auditor_lider   = this.getPermiso(this._enumPermisos.Asignar_auditor_lider);
    this.Calificacion_Masiva   = this.getPermiso(this._enumPermisos.Calificacion_Masiva);
    this.Permisos_Accesos   = this.getPermiso(this._enumPermisos.Permisos_Accesos);
    this.Modificar_Fecha_Final_Medicion   = this.getPermiso(this._enumPermisos.Modificar_Fecha_Final_Medicion);
    this.Parametrizar_Actas = this.getPermiso(this._enumPermisos.Parametrizar_Actas);
    this.Generar_Actas = this.getPermiso(this._enumPermisos.Generar_Actas);

   }
}
