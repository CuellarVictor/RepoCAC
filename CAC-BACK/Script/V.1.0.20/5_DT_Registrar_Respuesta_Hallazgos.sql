USE AuditCAC_QAInterno
GO

USE AuditCAC_QAInterno

/****** Object:  UserDefinedTableType [dbo].[DT_Registrar_Respuesta_Hallazgos]    Script Date: 16/08/2022 12:25:11 p. m. ******/
CREATE TYPE [dbo].[DT_Registrar_Respuesta_Hallazgos] AS TABLE(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RegistroAuditoriaDetalleId] [int] NULL,
	[Estado] [int] NULL,
	[Observacion] [nvarchar](max) NULL,
	[Usuario] [nvarchar](255) NULL,
	PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO


