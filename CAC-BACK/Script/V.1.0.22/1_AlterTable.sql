USE AuditCAC_QAInterno

ALTER TABLE dbo.VariableXMedicion ADD
	Calculadora bit NULL
GO
ALTER TABLE dbo.VariableXMedicion SET (LOCK_ESCALATION = TABLE)
GO


ALTER TABLE dbo.VariableXMedicion ADD
	TipoCalculadora int NULL
GO
