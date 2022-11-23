USE AuditCAC_QAInterno
GO

/****** Object:  Table [dbo].[cacperiodo]    Script Date: 25/04/2022 3:54:56 p. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Periodo](
	[idPeriodo] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](100) NOT NULL,
	[fechaCorte] [date] NOT NULL,
	[fechaFinalReporte] [date] NOT NULL,
	[fechaMaximaCorrecciones] [date] NOT NULL,
	[fechaMaximaSoportes] [date] NOT NULL,
	[fechaMaximaConciliaciones] [date] NOT NULL,
	[idCobertura] [int] NOT NULL,
	[idPeriodoAnt] [int] NULL,
	[FechaMinConsultaAudit] [date] NULL,
	[FechaMaxConsultaAudit] [date] NULL
) ON [PRIMARY]
GO

