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
IF OBJECT_ID('[dbo].[SP_MigPeriodo]') IS NOT NULL
    EXEC('DROP PROCEDURE [dbo].[SP_MigPeriodo]')
GO
CREATE PROCEDURE [dbo].[SP_MigPeriodo] 
	-- Add the parameters for the stored procedure here
	@CacTable MigCACPeriodo READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE  @MessageResult NVARCHAR(MAX) = 'OK'

    -- Insert statements for procedure here	
	BEGIN TRY 
		BEGIN TRAN

			UPDATE [dbo].[Periodo]
				SET [nombre] = pcac.nombre 
					,[fechaCorte] = pcac.fechaCorte
				    ,[fechaFinalReporte] = pcac.fechaFinalReporte
					,[fechaMaximaCorrecciones] = pcac.fechaMaximaCorrecciones
			        ,[fechaMaximaSoportes] = pcac.fechaMaximaSoportes
				    ,[fechaMaximaConciliaciones] = pcac.fechaMaximaConciliaciones
			        ,[idCobertura] = pcac.idCobertura
					,[idPeriodoAnt] = pcac.idPeriodoAnt
					,[FechaMinConsultaAudit] = pcac.FechaMinConsultaAudit
					,[FechaMaxConsultaAudit] = pcac.FechaMaxConsultaAudit
					FROM @CacTable pcac
						LEFT JOIN [dbo].[Periodo] pau 
							ON pcac.idPeriodo = pau.idPeriodo
					WHERE pau.idPeriodo is not null	

			INSERT INTO [dbo].[Periodo]
				SELECT pcac.[nombre]
					,pcac.[fechaCorte]
					,pcac.[fechaFinalReporte]
					,pcac.[fechaMaximaCorrecciones]
					,pcac.[fechaMaximaSoportes]
					,pcac.[fechaMaximaConciliaciones]
					,pcac.[idCobertura]
					,pcac.[idPeriodoAnt]
					,pcac.[FechaMinConsultaAudit]
				    ,pcac.[FechaMaxConsultaAudit]
					FROM @CacTable pcac
						LEFT JOIN [dbo].[Periodo] pau 
							ON pcac.idPeriodo = pau.idPeriodo
					WHERE pau.idPeriodo is null;					
			
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
