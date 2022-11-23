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
-- Description:	Migrar datos de la tabla regla
-- =============================================
IF OBJECT_ID('[dbo].[SP_MigRegla]') IS NOT NULL
    EXEC('DROP PROCEDURE [dbo].[SP_MigRegla]')
GO
CREATE PROCEDURE [dbo].[SP_MigRegla] 
	-- Add the parameters for the stored procedure here
	@CacReglaTable MigCACRegla READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE  @MessageResult NVARCHAR(MAX) = 'OK'

    -- Insert statements for procedure here		

	BEGIN TRY 
		BEGIN TRAN
			INSERT INTO [dbo].[Regla]
				SELECT rcac.[idCobertura]
					,rcac.[nombre]
					,rcac.[idTipoRegla]
					,rcac.[idTiempoAplicacion]
					,rcac.[habilitado]
					,rcac.[idError]
					,rcac.[idVariable]
					,rcac.[idTipoEnvioLimbo]
					FROM @CacReglaTable rcac
					LEFT JOIN [dbo].[Regla] rau 
					ON rcac.idRegla = rau.idRegla
					WHERE rau.idRegla is null;	

			UPDATE [dbo].[Regla]
				SET idCobertura = rcac.idCobertura,
					nombre = rcac.nombre
					,[idTipoRegla] = rcac.idTipoRegla
					,[idTiempoAplicacion] = rcac.idTiempoAplicacion
					,[habilitado] = rcac.habilitado
					,[idError] = rcac.idError
					,[idVariable] = rcac.idVariable
					,[idTipoEnvioLimbo] = rcac.idTipoEnvioLimbo
					FROM						   					
					@CacReglaTable rcac
					LEFT JOIN [dbo].[Regla] rau 
						ON rcac.idRegla = rau.idRegla
					WHERE rau.idRegla is not null			
			
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
