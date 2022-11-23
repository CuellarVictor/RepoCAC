USE [AuditCAC_QAInterno] -- db segun ambiente

ALTER TABLE dbo.Medicion ALTER COLUMN FechaInicioAuditoria Date;
ALTER TABLE dbo.Medicion ALTER COLUMN FechaFinAuditoria Date;
ALTER TABLE dbo.Medicion ALTER COLUMN FechaCorteAuditoria Date;