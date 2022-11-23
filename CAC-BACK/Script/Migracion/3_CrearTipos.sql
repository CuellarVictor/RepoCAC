-- ================================
-- Create User-defined Table Type
-- ================================
USE AuditCAC_QAInterno
GO

IF TYPE_ID('[dbo].[MigCACError]') IS NOT NULL
    EXEC('DROP TYPE [dbo].[MigCACError]')
GO
-- Migracion Error
CREATE TYPE [dbo].[MigCACError] AS TABLE 
(
	idError CHAR(5) NOT NULL, 
	descripcion VARCHAR(300) NULL, 
	idTipoError CHAR(10) NULL
)
GO

IF TYPE_ID('[dbo].[MigCACCobertura]') IS NOT NULL
    EXEC('DROP TYPE [dbo].[MigCACCobertura]')
GO
-- Enfermedad (pendiente ALTER para añadir campos)
CREATE TYPE [dbo].[MigCACCobertura] AS TABLE 
(
	idCobertura INT NOT NULL, 
	nombre VARCHAR(250) NOT NULL, 
	nemonico VARCHAR(50) NOT NULL
    ,legislacion VARCHAR(300) NOT NULL
    ,definicion VARCHAR(500) NOT NULL
    ,tiempoEstimado INT NOT NULL
    ,ExcluirNovedades BIT NULL
    ,Novedades  VARCHAR(200) NOT NULL
    ,idResolutionSiame  INT NULL
    ,NovedadesCompartidosUnicos VARCHAR(50) NOT NULL
    ,CantidadVariables INT NULL
    ,idCoberturaPadre INT NULL
    ,idCoberturaAdicionales INT NULL
)
GO

IF TYPE_ID('[dbo].[MigCACVariables]') IS NOT NULL
    EXEC('DROP TYPE [dbo].[MigCACVariables]')
GO
-- Variables 
CREATE TYPE [dbo].[MigCACVariables] AS TABLE 
(
	idVariable INT NOT NULL,
	idCobertura INT NOT NULL, 
	nombre VARCHAR(100) NOT NULL,
	nemonico VARCHAR(50) NOT NULL,
	descripcion VARCHAR(500) NOT NULL,
	idTipoVariable VARCHAR(10) NOT NULL,
	longitud INT NULL,
    decimales INT NULL,
	formato VARCHAR(300) NOT NULL,
	tablaReferencial VARCHAR(128) NULL,
    campoReferencial VARCHAR(128) NULL,
	status BIT NULL
)
GO

IF TYPE_ID('[dbo].[MigCACRegla]') IS NOT NULL
    EXEC('DROP TYPE [dbo].[MigCACRegla]')
GO
--Regla	
CREATE TYPE [dbo].[MigCACRegla] AS TABLE 
(
	idRegla INT NOT NULL, 
	idCobertura INT NOT NULL, 
	nombre VARCHAR(200) NOT NULL,
	idTipoRegla TINYINT NOT NULL,
	idTiempoAplicacion TINYINT NOT NULL,
    habilitado BIT NOT NULL,
    idError CHAR(5) NOT NULL,
    idVariable INT NULL,
    idTipoEnvioLimbo TINYINT NULL
)
GO

IF TYPE_ID('[dbo].[MigCACRestriccionesConsistencia]') IS NOT NULL
    EXEC('DROP TYPE [dbo].[MigCACRestriccionesConsistencia]')
GO
--Restriccionesconsistencia
CREATE TYPE [dbo].[MigCACRestriccionesConsistencia] AS TABLE 
(
	idRegla INT NOT NULL,
	idRestriccionConsistencia INT NOT NULL,
	idSignoComparacion CHAR (10) NOT NULL,
	idCompararCon TINYINT NOT NULL,
	idVariableComparacion INT NULL,
    idTipoValorComparacion TINYINT NULL,
    valorEspecifico SQL_VARIANT NULL,
    idVariableAsociada INT NULL,
    idSignoComparacionAsociada CHAR(10) NULL,
    idCompararConAsociada TINYINT NULL,
    idVariableComparacionAsociada INT NULL,
    idTipoValorComparacionAsociada TINYINT NULL,
    valorEspecificoAsociada SQL_VARIANT NULL
)
GO

IF TYPE_ID('[dbo].[MigCACPeriodo]') IS NOT NULL
    EXEC('DROP TYPE [dbo].[MigCACPeriodo]')
GO
-- Periodo
CREATE TYPE [dbo].[MigCACPeriodo] AS TABLE 
(
	idPeriodo INT NOT NULL,
    nombre VARCHAR(100) NOT NULL,
    fechaCorte DATE NOT NULL,
    fechaFinalReporte DATE NOT NULL,
    fechaMaximaCorrecciones DATE NOT NULL,
    fechaMaximaSoportes DATE NOT NULL,
    fechaMaximaConciliaciones DATE NOT NULL,
    idCobertura INT NOT NULL,
    idPeriodoAnt INT NULL,
    FechaMinConsultaAudit DATE NOT NULL,
    FechaMaxConsultaAudit DATE NOT NULL
)
GO