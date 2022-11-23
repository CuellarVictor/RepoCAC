USE [AuditCAC_Development]
GO

/****** Object:  Table [dbo].[RegistroAuditoriaDetalleLog]    Script Date: 10/08/2022 9:57:40 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RegistroAuditoriaDetalleLog](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[RegistroAuditoriaDetalleId] [int] NOT NULL,
	[CalificacionAnterior] [int] NOT NULL,
	[CalificacionNueva] [int] NOT NULL,
	[DatoReportado] [nvarchar](255) NULL,
	[MotivoAnterior] [nvarchar](255) NULL,
	[MotivoNuevo] [nvarchar](255) NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateBy] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_RegistroAuditoriaDetalleLog] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


