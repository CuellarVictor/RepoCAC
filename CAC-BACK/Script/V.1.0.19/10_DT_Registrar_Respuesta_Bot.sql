USE [AuditCAC_Development]
GO

/****** Object:  UserDefinedTableType [dbo].[DT_Registrar_Respuesta_Bot]    Script Date: 10/08/2022 9:58:20 p. m. ******/
CREATE TYPE [dbo].[DT_Registrar_Respuesta_Bot] AS TABLE(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[IdAuditingDetail] [int] NULL,
	[ResultadoBOT_Label] [nvarchar](255) NULL,
	[Contextos] [nvarchar](max) NULL,
	[ResultadoBOT_comparacion] [int] NULL,
	[FechaModificacion] [datetime] NULL,
	[Usuario] [nvarchar](255) NULL,
	PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (IGNORE_DUP_KEY = OFF)
)
GO


