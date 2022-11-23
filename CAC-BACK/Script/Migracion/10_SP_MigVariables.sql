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
IF OBJECT_ID('[dbo].[SP_MigVariables]') IS NOT NULL
    EXEC('DROP PROCEDURE [dbo].[SP_MigVariables]')
GO
CREATE PROCEDURE [dbo].[SP_MigVariables]
	-- Add the parameters for the stored procedure here
	@CacTable MigCACVariables READONLY
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE  @MessageResult NVARCHAR(MAX) = 'OK',
			 @Usuario VARCHAR(50)

    -- Insert statements for procedure here

	BEGIN TRY 
		BEGIN TRAN

		SET @Usuario = (SELECT TOP 1 U.Id 
		FROM AspNetUsers U INNER JOIN AspNetUserRoles US 
		ON (U.Id = US.UserId) WHERE US.RoleId = 1)
		
			UPDATE [dbo].[Variables]
				SET [idCobertura] = vcac.idCobertura
					,[nombre] = vcac.nombre
					,[nemonico] = vcac.nemonico
					,[descripcion] = vcac.descripcion
					,[idTipoVariable] = vcac.idTipoVariable
					,[longitud] = vcac.longitud
					,[decimales] = vcac.decimales
					,[formato] = vcac.formato
					,[tablaReferencial] = vcac.tablaReferencial
					,[campoReferencial] = vcac.campoReferencial		
					,[ModifyDate] = GETDATE()				    				
				FROM @CacTable vcac
					LEFT JOIN [dbo].[Variables] vau 
					ON vcac.idVariable = vau.idVariable
					WHERE vau.idVariable is not null	

			INSERT INTO [dbo].[Variables]
				SELECT [Activa] = 1
					,[Orden] = 1
					,[idVariable]=vcac.[idVariable]
					,[idCobertura]=vcac.[idCobertura]
					,[nombre]=vcac.[nombre]
					,[nemonico]=vcac.[nemonico]
					,[descripcion]=vcac.[descripcion]
					,[idTipoVariable]=vcac.[idTipoVariable]
				    ,[longitud]=vcac.[longitud]
					,[decimales]=vcac.[decimales]
					,[formato]=vcac.[formato]
					,[tablaReferencial]=vcac.[tablaReferencial]
					,[campoReferencial]=vcac.[campoReferencial]
					,[CreatedBy] = @Usuario
					,[CreatedDate] = GETDATE()
					,[ModifyBy] = null
					,[ModifyDate] = null
					,[MotivoVariable] = null
					,[Bot] = 0
					,[TipoVariableItem] = 35
				    ,[EstructuraVariable] = 39
					,[Alerta] = 0
					,[AlertaDescripcion] = null
					,[Status] = 1
					FROM @CacTable vcac
					LEFT JOIN [dbo].[Variables] vau 
					ON vcac.idVariable = vau.idVariable
					WHERE vau.idVariable is null			

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
