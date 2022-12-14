USE [AuditCAC_QAInterno] -- db segun ambiente

DROP TABLE RegistroAuditoriaDetalleError;  

GO
/****** Object:  Table [dbo].[RegistroAuditoriaDetalleError]    Script Date: 3/10/2022 4:59:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RegistroAuditoriaDetalleError](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RegistrosAuditoriaDetalleId] [int] NULL,
	[Reducido] [nvarchar](255) NULL,
	[VariableId] [int] NULL,
	[ErrorId] [nvarchar](255) NULL,
	[Descripcion] [nvarchar](max) NULL,
	[Enable] [bit] NULL,
	[CreatedBy] [nvarchar](255) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifyBy] [nvarchar](255) NULL,
	[ModifyDate] [datetime] NULL,
 CONSTRAINT [PK_RegistroAuditoriaDetalleError] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[RegistroAuditoriaDetalleError] ADD  CONSTRAINT [DF__RegistroA__Statu__32AB8735]  DEFAULT ((1)) FOR [Enable]
GO

