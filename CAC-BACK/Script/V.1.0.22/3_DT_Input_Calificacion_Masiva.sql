USE [AuditCAC_Development]
GO

/****** Object:  UserDefinedTableType [dbo].[DT_Input_Calificacion_Masiva]    Script Date: 19/09/2022 12:36:56 a. m. ******/
CREATE TYPE [dbo].[DT_Input_Calificacion_Masiva] AS TABLE(
	[Id] [int] NULL,
	[IdRadicado] [int] NULL,
	[NemonicoVariable] [nvarchar](max) NULL,
	[Calificacion] [nvarchar](max) NULL,
	[Motivo] [nvarchar](max) NULL,
	[Observacion] [nvarchar](max) NULL
)
GO


