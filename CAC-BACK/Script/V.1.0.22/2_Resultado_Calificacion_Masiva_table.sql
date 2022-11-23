USE [AuditCAC_Development]
GO

/****** Object:  Table [dbo].[Resultado_Calificacion_Masiva]    Script Date: 19/09/2022 12:34:37 a. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Resultado_Calificacion_Masiva](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CurrentProcessId] [int] NULL,
	[RegistroAuditoriaDetalleId] [int] NULL,
	[IdRadicado] [int] NULL,
	[VariableId] [int] NULL,
	[NemonicoVariable] [varchar](1024) NULL,
	[Tipo] [varchar](255) NULL,
	[Result] [varchar](max) NULL,
	[RegistroEstadoAnterior] [varchar](max) NULL,
	[RegistroEstadoNuevo] [varchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Resultado_Calificacion_Masiva] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

