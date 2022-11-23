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
-- Description:	Migrar datos de la tabla cacperiodo
-- =============================================
IF OBJECT_ID('[dbo].[SP_MigCobertura]') IS NOT NULL
    EXEC('DROP PROCEDURE [dbo].[SP_MigCobertura]')
GO
CREATE PROCEDURE [dbo].[SP_MigCobertura] 
	-- Add the parameters for the stored procedure here
	@CacTable MigCACCobertura READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE  @MessageResult NVARCHAR(MAX) = 'OK'

    -- Insert statements for procedure here

	BEGIN TRY 
		BEGIN TRAN
		
			UPDATE [dbo].[Enfermedad]
				SET [idCobertura] = ccac.idCobertura
				,[nombre] = ccac.nombre
				,[Status] = 1
				,[nemonico] = ccac.nemonico
				,[legislacion] = ccac.legislacion
				,[definicion] = ccac.definicion
				,[tiempoEstimado] = ccac.tiempoEstimado
				,[ExcluirNovedades] = ccac.ExcluirNovedades
				,[Novedades] = ISNULL(ccac.[Novedades],'')
				,[idResolutionSiame] = ccac.idResolutionSiame
				,[CantidadVariables] = ccac.CantidadVariables
				,[idCoberturaPadre] = ccac.idCoberturaPadre
				,[idCoberturaAdicionales] = ccac.idCoberturaAdicionales
					FROM @CacTable ccac
						LEFT JOIN [dbo].[Enfermedad] cau 
							ON ccac.idCobertura = cau.idCobertura
					WHERE cau.idCobertura is not null	

			INSERT INTO [dbo].[Enfermedad]
				SELECT idCobertura = ccac.idCobertura
					,nombre = ccac.nombre
					,Status = 1
					,nemonico = ccac.nemonico
					,legislacion = ccac.legislacion
					,[definicion] = ccac.[definicion]
					, [tiempoEstimado] = ccac.[tiempoEstimado]
					,[ExcluirNovedades] = ccac.[ExcluirNovedades]
					, [Novedades] = ISNULL(ccac.[Novedades], '')
					,[idResolutionSiame] = ccac.[idResolutionSiame]
					,[CantidadVariables] = ccac.[CantidadVariables]
					,[idCoberturaPadre] = ccac.[idCoberturaPadre]
					,[idCoberturaAdicionales] = ccac.[idCoberturaAdicionales]
					FROM @CacTable ccac
						LEFT JOIN [dbo].[Enfermedad] cau 
							ON ccac.idCobertura = cau.idCobertura
					WHERE cau.idCobertura is null				

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
