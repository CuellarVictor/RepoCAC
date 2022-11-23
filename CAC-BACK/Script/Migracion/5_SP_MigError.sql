USE AuditCAC_QAInterno
GO

-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Eriks Palma
-- Create date: 26/04/2022
-- Description:	Insertar datos Error
-- =============================================
IF OBJECT_ID('[dbo].[SP_MigError]') IS NOT NULL
    EXEC('DROP PROCEDURE [dbo].[SP_MigError]')
GO
CREATE PROCEDURE [dbo].[SP_MigError] 
	-- Add the parameters for the stored procedure here
	@CacErrorTable MigCACError READONLY
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE  @MessageResult NVARCHAR(MAX) = 'OK'

    -- Insert statements for procedure here

	DECLARE @Insertar MigCACError
	INSERT INTO @Insertar
		SELECT ecac.idError, ecac.descripcion, ecac.idTipoError 
			FROM  @CacErrorTable ecac
			LEFT JOIN [dbo].[ErrorRegla] eau 
				ON ecac.idError = eau.idError
			WHERE eau.idError is null	

	BEGIN TRY 
		BEGIN TRAN
			INSERT INTO [dbo].[ErrorRegla]
				SELECT idError, descripcion, idTipoError FROM @Insertar

			UPDATE [dbo].[ErrorRegla] 
				SET descripcion = ecac.descripcion,
					idTipoError = ecac.idTipoError
					FROM						   					
			@CacErrorTable ecac
		LEFT JOIN [dbo].[ErrorRegla] eau 
			ON ecac.idError = eau.idError
		WHERE eau.idError is not null
		COMMIT TRAN
		 
	END TRY

	BEGIN CATCH
		-- SELECT
		--ERROR_NUMBER() AS ErrorNumber,
		--ERROR_STATE() AS ErrorState,
		--ERROR_SEVERITY() AS ErrorSeverity,
		--ERROR_PROCEDURE() AS ErrorProcedure,
		--ERROR_LINE() AS ErrorLine,
		--ERROR_MESSAGE() AS ErrorMessage;
		ROLLBACK TRAN
		SET @MessageResult = 'ERROR, ' + ERROR_MESSAGE() + '- SP LINEA: ' + CAST(ERROR_LINE() AS nvarchar(255));
				
	END CATCH

END
GO
