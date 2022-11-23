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
IF OBJECT_ID('[dbo].[SP_MigRestriccionesConsistencia]') IS NOT NULL
    EXEC('DROP PROCEDURE [dbo].[SP_MigRestriccionesConsistencia]')
GO
CREATE PROCEDURE [dbo].[SP_MigRestriccionesConsistencia]  
	-- Add the parameters for the stored procedure here
	@CacTable MigCACRestriccionesConsistencia READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE  @MessageResult NVARCHAR(MAX) = 'OK'

    -- Insert statements for procedure here

	BEGIN TRY 
		BEGIN TRAN
		
		UPDATE [dbo].[RestriccionesConsistencia]
				SET [idRegla] = rccac.idRegla					
					,[idSignoComparacion] = rccac.[idSignoComparacion]
					,[idCompararCon] = rccac.[idCompararCon]
					,[idVariableComparacion] = rccac.[idVariableComparacion]
					,[idTipoValorComparacion] = rccac.[idTipoValorComparacion]
					,[valorEspecifico] = rccac.[valorEspecifico]
					,[idVariableAsociada] = rccac.[idVariableAsociada]
					,[idSignoComparacionAsociada] = rccac.[idSignoComparacionAsociada]
					,[idCompararConAsociada] = rccac.[idCompararConAsociada]
					,[idVariableComparacionAsociada] = rccac.[idVariableComparacionAsociada]
					,[idTipoValorComparacionAsociada] = rccac.[idTipoValorComparacionAsociada]
					,[valorEspecificoAsociada] = rccac.[valorEspecificoAsociada]
					FROM @CacTable rccac
						LEFT JOIN [dbo].[RestriccionesConsistencia] rcau 
							ON rccac.idRestriccionConsistencia = rcau.idRestriccionConsistencia
					WHERE rcau.idRestriccionConsistencia is not null	

			INSERT INTO [dbo].[RestriccionesConsistencia]
				SELECT rccac.[idRegla]
					,rccac.[idSignoComparacion]
					,rccac.[idCompararCon]
					,rccac.[idVariableComparacion]
					,rccac.[idTipoValorComparacion]
					,rccac.[valorEspecifico]
					,rccac.[idVariableAsociada]
					,rccac.[idSignoComparacionAsociada]
					,rccac.[idCompararConAsociada]
					,rccac.[idVariableComparacionAsociada]
					,rccac.[idTipoValorComparacionAsociada]
					,rccac.[valorEspecificoAsociada]
					FROM @CacTable rccac
						LEFT JOIN [dbo].[RestriccionesConsistencia] rcau 
							ON rccac.idRestriccionConsistencia = rcau.idRestriccionConsistencia
					WHERE rcau.idRestriccionConsistencia is null;

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
