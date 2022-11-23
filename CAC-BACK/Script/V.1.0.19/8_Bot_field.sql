/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.RegistrosAuditoriaDetalle ADD
	Bot bit NULL
GO
ALTER TABLE dbo.RegistrosAuditoriaDetalle ADD CONSTRAINT
	DF_RegistrosAuditoriaDetalle_Bot DEFAULT 0 FOR Bot
GO
ALTER TABLE dbo.RegistrosAuditoriaDetalle SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.RegistrosAuditoriaDetalle', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.RegistrosAuditoriaDetalle', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.RegistrosAuditoriaDetalle', 'Object', 'CONTROL') as Contr_Per 