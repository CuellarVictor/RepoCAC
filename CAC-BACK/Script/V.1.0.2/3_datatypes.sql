USE [AuditCAC_QAInterno] -- db segun ambiente

DROP PROCEDURE ActualizarBanderasRegistrosAuditoria;
DROP PROCEDURE ActualizarCampo_Dato_DC_NC_ND;
DROP PROCEDURE ActualizarVariablesLider;
DROP PROCEDURE ActualizarVariablesLiderMasivo;
DROP PROCEDURE AtarListadoVariablesMedicion;
DROP PROCEDURE CalificarRegistroAuditoria;
DROP PROCEDURE CambiarEstadoRegistroAuditoria;
DROP PROCEDURE CambiarEstadoRegistroAuditoriaMasivo;
DROP PROCEDURE consultaPascientes;
DROP PROCEDURE ConsultarOrderVariables;
DROP PROCEDURE CrearVariables;
DROP PROCEDURE Delete_User;
DROP PROCEDURE DuplicarMedicion;
DROP PROCEDURE DuplicarvariablesMedicion;
DROP PROCEDURE EditarVariables;
DROP PROCEDURE EliminarVariable;
DROP PROCEDURE GetAlertasRegistrosAuditoria;
DROP PROCEDURE GetBancoInformacionByPalabraClave;
DROP PROCEDURE GetCalificacionEsCompletas;
DROP PROCEDURE GetCalificacionesRegistroAuditoriaByRegistrosAuditoriaId;
DROP PROCEDURE GetCalificacionesRegistroAuditoriaByVariableId;
DROP PROCEDURE GetCountLeaderIssues;
DROP PROCEDURE GetCountPaginator;
DROP PROCEDURE GetDataMedicioneslider;
DROP PROCEDURE GetDataMedicioneslider_Prueba;
DROP PROCEDURE getEnfermedades;
DROP PROCEDURE GETENFERMEDADESMADREXUSUARIO;
DROP PROCEDURE GetExportToExcle;
DROP PROCEDURE GetFiltrosBolsaMedicion;
DROP PROCEDURE GetMedicionCriterio;
DROP PROCEDURE GetObservacionesByRegistroAuditoriaId;
DROP PROCEDURE GetPercentageProcess;
DROP PROCEDURE GetRegistrosAuditoria;
DROP PROCEDURE GetRegistrosAuditoria2;
DROP PROCEDURE GetRegistrosAuditoriaPrueba;
DROP PROCEDURE GetRegistrosXMedicion;
DROP PROCEDURE GetSoportesEntidad;
DROP PROCEDURE getUserCode;
DROP PROCEDURE GetUsuariosByRoleCoberturaId;
DROP PROCEDURE GetUsuariosByRoleId;
DROP PROCEDURE GetUsuarioXEnfermedad;
DROP PROCEDURE GetVariablesFiltrado;
DROP PROCEDURE Inactive_User;
DROP PROCEDURE InsertarPascientes;
DROP PROCEDURE MoverRegistrosAuditoria;
DROP PROCEDURE RegistroAuditoriaLogSP;
DROP PROCEDURE RegistrosAuditoriaAsignadoAuditorEstado;
DROP PROCEDURE RegistrosAuditoriaDetallesAsignacion;
DROP PROCEDURE RegistrosAuditoriaFiltrado;
DROP PROCEDURE RegistrosAuditoriaFiltros;
DROP PROCEDURE RegistrosAuditoriaFiltrosTest;
DROP PROCEDURE RegistrosAuditoriaProgresoDiario;
DROP PROCEDURE showUserCode;
DROP PROCEDURE SP_Actualiza_Estado_Registro_Auditoria;
DROP PROCEDURE SP_AsignaVariablesMedicion;
DROP PROCEDURE SP_BolsasMedicion_XEnfermedadMadre;
DROP PROCEDURE SP_Consulta_Detalle_Registros_Auditoria;
DROP PROCEDURE SP_Consulta_Detalle_Registros_Auditoria2;
DROP PROCEDURE SP_Consulta_ErrorRegistroAuditoria;
DROP PROCEDURE SP_Consulta_Log_Accion;
DROP PROCEDURE SP_Consulta_Perfil_Accion;
DROP PROCEDURE SP_Consulta_Registros_Auditoria_Filtrados;
DROP PROCEDURE SP_Consulta_RegistrosAuditoria_Info;
DROP PROCEDURE SP_Consulta_Usuarios_Lider;
DROP PROCEDURE SP_MoverAlgunos_RegistrosAuditoria_BolsaMedicion;
DROP PROCEDURE SP_MoverTodos_RegistrosAuditoria_BolsaMedicion;
DROP PROCEDURE SP_MoverTodos_RegistrosAuditoria_BolsaMedicion_Plantilla;
DROP PROCEDURE SP_Realiza_Cargue_Poblacion;
DROP PROCEDURE SP_Reasignaciones_Bolsa;
DROP PROCEDURE SP_Reasignaciones_Bolsa_Detallada;
DROP PROCEDURE SP_RegistrosAuditoria_XBolsaMedicion;
DROP PROCEDURE SP_RegistrosAuditoria_XBolsaMedicion_Filtros;
DROP PROCEDURE SP_Test_Proceso;
DROP PROCEDURE SP_Upsert_ErrorRegistroAuditoria;
DROP PROCEDURE SP_Usuarios_BolsaMedicion;
DROP PROCEDURE SP_Usuarios_BolsaMedicion_Filtro;
DROP PROCEDURE SP_Usuarios_By_Rol;
DROP PROCEDURE SP_Validacion_Estado_BolsasMedicion;
DROP PROCEDURE SP_Validacion_Estado_CA;
DROP PROCEDURE SP_Validacion_Estado_CE;
DROP PROCEDURE SP_Validacion_Estado_ELA;
DROP PROCEDURE SP_Validacion_Estado_ELL;
DROP PROCEDURE SP_Validacion_Estado_GO1;
DROP PROCEDURE SP_Validacion_Estado_GO2;
DROP PROCEDURE SP_Validacion_Estado_GRE;
DROP PROCEDURE SP_Validacion_Estado_RN;
DROP PROCEDURE SP_Validacion_Observacion_Registrada;
DROP PROCEDURE SP_Validacion_Registrar_Motivo;
DROP PROCEDURE Update_User;


-- db segun ambiente
GO
/****** Object:  UserDefinedTableType [dbo].[DT_Variables]    Script Date: 3/10/2022 5:08:40 PM ******/
DROP TYPE [dbo].[DT_Variables]
GO
/****** Object:  UserDefinedTableType [dbo].[DT_Reasignaciones_Bolsa]    Script Date: 3/10/2022 5:08:40 PM ******/
DROP TYPE [dbo].[DT_Reasignaciones_Bolsa]
GO
/****** Object:  UserDefinedTableType [dbo].[DT_Pascientes]    Script Date: 3/10/2022 5:08:40 PM ******/
DROP TYPE [dbo].[DT_Pascientes]
GO
/****** Object:  UserDefinedTableType [dbo].[DT_Medicion]    Script Date: 3/10/2022 5:08:40 PM ******/
DROP TYPE [dbo].[DT_Medicion]
GO



/****** Object:  UserDefinedTableType [dbo].[DT_Medicion]    Script Date: 3/10/2022 5:08:40 PM ******/
CREATE TYPE [dbo].[DT_Medicion] AS TABLE(
	[Id] [int] NOT NULL,
	[IdCobertura] [int] NOT NULL,
	[IdPeriodo] [int] NOT NULL,
	[Descripcion] [varchar](200) NOT NULL,
	[Activo] [bit] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifyBy] [int] NULL,
	[ModifyDate] [datetime] NULL,
	PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  UserDefinedTableType [dbo].[DT_Pascientes]    Script Date: 3/10/2022 5:08:40 PM ******/
CREATE TYPE [dbo].[DT_Pascientes] AS TABLE(
	[Id] [int] NOT NULL,
	[IdRadicado] [int] NOT NULL,
	[IdMedicion] [int] NULL,
	[IdPeriodo] [int] NULL,
	[PrimerNombre] [varchar](50) NOT NULL,
	[SegundoNombre] [varchar](50) NULL,
	[PrimerApellido] [varchar](50) NOT NULL,
	[SegundoApellido] [varchar](50) NOT NULL,
	[TipoIdentificacion] [varchar](2) NULL,
	[Identificacion] [int] NOT NULL,
	[FechaNacimiento] [datetime] NULL,
	[FechaCreacion] [datetime] NULL,
	[FechaAuditoria] [datetime] NOT NULL,
	[Activo] [bit] NOT NULL,
	[Conclusion] [varchar](200) NOT NULL,
	[UrlSoportes] [varchar](200) NOT NULL,
	[Reverse] [bit] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[Ara] [bit] NOT NULL,
	[Eps] [bit] NOT NULL,
	[FechaReverso] [datetime] NOT NULL,
	[AraAtendido] [bit] NOT NULL,
	[EpsAtendido] [bit] NOT NULL,
	[Revisar] [bit] NOT NULL,
	[Estado] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifyBy] [int] NULL,
	[ModifyDate] [datetime] NULL,
	PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO
/****** Object:  UserDefinedTableType [dbo].[DT_Reasignaciones_Bolsa]    Script Date: 3/10/2022 5:08:40 PM ******/
CREATE TYPE [dbo].[DT_Reasignaciones_Bolsa] AS TABLE(
	[AuditorId] [varchar](450) NOT NULL,
	[IdRadicado] [int] NOT NULL,
	[FechaAsignacion] [date] NOT NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[DT_Variables]    Script Date: 3/10/2022 5:08:40 PM ******/
CREATE TYPE [dbo].[DT_Variables] AS TABLE(
	[Activa] [bit] NOT NULL,
	[Orden] [int] NOT NULL,
	[idVariable] [int] NOT NULL,
	[idCobertura] [int] NOT NULL,
	[nombre] [varchar](100) NOT NULL,
	[nemonico] [varchar](50) NOT NULL,
	[descripcion] [varchar](500) NOT NULL,
	[idTipoVariable] [varchar](10) NOT NULL,
	[longitud] [int] NULL,
	[decimales] [int] NULL,
	[formato] [varchar](300) NULL,
	[tablaReferencial] [varchar](128) NULL,
	[campoReferencial] [varchar](128) NULL,
	[CreatedBy] [varchar](255) NOT NULL,
	[ModifyBy] [varchar](255) NULL,
	[MotivoVariable] [nvarchar](max) NULL,
	[Bot] [bit] NULL,
	[TipoVariableItem] [int] NULL,
	[EstructuraVariable] [int] NULL,
	[MedicionId] [int] NOT NULL,
	[EsGlosa] [bit] NULL,
	[EsVisible] [bit] NULL,
	[EsCalificable] [bit] NULL,
	[Activo] [bit] NULL,
	[EnableDC] [bit] NULL,
	[EnableNC] [bit] NULL,
	[EnableND] [bit] NULL,
	[CalificacionXDefecto] [bit] NULL,
	[SubGrupoId] [int] NOT NULL,
	[Encuesta] [bit] NULL,
	[VxM_Orden] [int] NOT NULL
)
GO
