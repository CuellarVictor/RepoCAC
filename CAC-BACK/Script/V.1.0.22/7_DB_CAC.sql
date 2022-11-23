USE [CAC]
GO
/****** Object:  StoredProcedure [dbo].[SP_GetDataMigVariable]    Script Date: 19/09/2022 12:50:54 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SP_GetDataMigVariable]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT cacv.[idVariable]
      ,cacv.[idCobertura]
      ,cacv.[nombre]
      ,cacv.[nemonico]
      ,cacv.[descripcion]
      ,cacv.[idTipoVariable]
      ,cacv.[longitud]
      ,cacv.[decimales]
      ,ISNULL(cacv.[formato],'') As formato
      ,cacv.[tablaReferencial]
      ,cacv.[campoReferencial]
	  ,cacv.[idErrorTipo]
	  ,(SELECT TOP(1) orden FROM cacVariablesPeriodo cacp WHERE cacp.idVariable = cacv.idVariable ORDER BY 1 DESC) AS orden 
	FROM [dbo].[Cacvariable] cacv

END


