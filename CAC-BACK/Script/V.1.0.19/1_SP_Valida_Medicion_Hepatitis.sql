USE AuditCAC_QAInterno
GO
/****** Object:  StoredProcedure [dbo].[SP_Valida_Medicion_Hepatitis]    Script Date: 10/08/2022 12:20:04 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar
-- Create date: 2022-008-09
-- Description: Validacion para medicion o registro que sea tipo hepatitis
-- =============================================
-- Execute
-- DECLARE	@return_value int; EXEC	@return_value = [dbo].[SP_Valida_Medicion_Hepatitis] @MedicionId = 21, @RegistroAuditoriaId = 0; SELECT	'Return Value' = @return_value
-- =============================================



CREATE OR ALTER   PROCEDURE  [dbo].[SP_Valida_Medicion_Hepatitis]

	@MedicionId INT,
	@RegistroAuditoriaId INT
AS
BEGIN
	
	DECLARE @IdHEPATITIS VARCHAR(10) = (SELECT Valor FROM ParametrosGenerales WHERE Id = 37) -- Parametro que contiene las coberturas de hepatitis	  
	DECLARE @IsHeapititis INT = 0

	IF @RegistroAuditoriaId <> 0 AND (SELECT COUNT(*) FROM RegistrosAuditoria WHERE Id =@RegistroAuditoriaId) > 0
	BEGIN		
		SET @MedicionId = (SELECT IdMedicion FROM RegistrosAuditoria WHERE Id = @RegistroAuditoriaId)
	END

	SET @IsHeapititis = (SELECT COUNT(Id) FROM Medicion ME WHERE ME.Id = @MedicionId AND ME.IdCobertura IN (SELECT CAST(value As VARCHAR(MAX)) FROM STRING_SPLIT_CUSTOM(@IdHEPATITIS,',')));
	
	IF @IsHeapititis > 0 
		BEGIN
			RETURN 1
		END
	ELSE
		BEGIN 
			RETURN 0
		END

END
