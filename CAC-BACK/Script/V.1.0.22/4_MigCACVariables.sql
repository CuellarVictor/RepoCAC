USE [AuditCAC_Development]

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].[SP_MigVariables]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE SP_MigVariables
GO


DROP TYPE [dbo].[MigCACVariables]

CREATE TYPE [dbo].[MigCACVariables] AS TABLE(
	[idVariable] [int] NOT NULL,
	[idCobertura] [int] NOT NULL,
	[nombre] [varchar](100) NOT NULL,
	[nemonico] [varchar](50) NOT NULL,
	[descripcion] [varchar](500) NOT NULL,
	[idTipoVariable] [varchar](10) NOT NULL,
	[longitud] [int] NULL,
	[decimales] [int] NULL,
	[formato] [varchar](300) NOT NULL,
	[tablaReferencial] [varchar](128) NULL,
	[campoReferencial] [varchar](128) NULL,
	[idErrorTipo] [nvarchar](255) NULL,
	[orden] [int] NULL
)
GO