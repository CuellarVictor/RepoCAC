USE AuditCAC_QAInterno

IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'SP_Cambiar_Estado_Entidad')
BEGIN
	DROP PROCEDURE dbo.SP_Cambiar_Estado_Entidad
END

IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'SP_Insertar_Hallazgos_Masivo')
BEGIN
	DROP PROCEDURE dbo.SP_Insertar_Hallazgos_Masivo
END

IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'SP_Consultar_Estados_Entidad')
BEGIN
	DROP PROCEDURE dbo.SP_Consultar_Estados_Entidad
END

IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'SP_Consultar_Variables_Para_Hallazgos')
BEGIN
	DROP PROCEDURE dbo.SP_Consultar_Variables_Para_Hallazgos
END

IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'SP_Consulta_Auditores_EPS_Cobertura_Periodo')
BEGIN
	DROP PROCEDURE dbo.SP_Consulta_Auditores_EPS_Cobertura_Periodo
END
