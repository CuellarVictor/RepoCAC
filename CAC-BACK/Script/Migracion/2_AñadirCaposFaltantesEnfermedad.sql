USE AuditCAC_QAInterno
GO
SELECT * FROM Enfermedad
ALTER TABLE [dbo].[Enfermedad]
	ADD nemonico VARCHAR(50) NOT NULL DEFAULT ('') WITH VALUES

ALTER TABLE [dbo].[Enfermedad]
	ADD legislacion VARCHAR(300) NOT NULL DEFAULT ('') WITH VALUES
		    
ALTER TABLE [dbo].[Enfermedad]
	ADD definicion VARCHAR(500) NOT NULL DEFAULT ('') WITH VALUES
	
ALTER TABLE [dbo].[Enfermedad]
	ADD tiempoEstimado INT NOT NULL DEFAULT (0) WITH VALUES
    
ALTER TABLE [dbo].[Enfermedad]
	ADD ExcluirNovedades BIT NULL

ALTER TABLE [dbo].[Enfermedad]
	ADD Novedades  VARCHAR(200) NOT NULL DEFAULT ('') WITH VALUES

ALTER TABLE [dbo].[Enfermedad]
	ADD idResolutionSiame  INT NULL

ALTER TABLE [dbo].[Enfermedad]
	ADD NovedadesCompartidosUnicos VARCHAR(50) NOT NULL DEFAULT ('') WITH VALUES

ALTER TABLE [dbo].[Enfermedad]
	ADD CantidadVariables INT NULL
	,idCoberturaPadre INT NULL
    ,idCoberturaAdicionales INT NULL
;
