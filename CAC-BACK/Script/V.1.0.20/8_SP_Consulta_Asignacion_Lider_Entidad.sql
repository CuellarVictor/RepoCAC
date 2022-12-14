USE [AuditCAC_Development]
GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Asignacion_Lider_Entidad]    Script Date: 17/08/2022 11:04:02 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-07-25
-- Description:	Para consultar Asignacion de lider por entidad
-- =============================================
--DROP PROCEDURE SP_Consulta_Asignacion_Lider_Entidad
ALTER PROCEDURE [dbo].[SP_Consulta_Asignacion_Lider_Entidad]
(@PageNumber INT, @MaxRows INT, @IdCobertura VARCHAR(MAX), @IdPeriodo VARCHAR(MAX))
AS
BEGIN

	DECLARE @CoberturaEPS VARCHAR(MAX) = 'cacEPS'

	-- Guardamos Query inicial.  
	DECLARE @Query VARCHAR(MAX);  
	SET @Query = 'SELECT (ScriptNoRegistrosTotales) As NoRegistrosTotales, MAX(RA.IdEPS) As Data_IdEPS, (SELECT TOP(1) ItemDescripcion from CatalogoItemCobertura  WHERE ItemId = RA.IdEPS ) As Data_NombreEPS, 
	MAX(LIEPS.IdAuditorLider) As IdAuditorLider, MAX(USA.Usuario) As UsuarioAuditor, 
	MAX(ME.IdCobertura) As IdCobertura, MAX(RA.IdPeriodo) As IdPeriodo 

	FROM RegistrosAuditoria RA 
	INNER JOIN CatalogoCobertura CCO ON CCO.NombreCatalogo = ' + '''' + @CoberturaEPS + '''' + ' 
	LEFT JOIN CatalogoItemCobertura CIC ON (CCO.Id = CIC.CatalogoCoberturaId AND CIC.ItemId = RA.IdEPS) 
	INNER JOIN Medicion ME ON ME.Id = RA.IdMedicion 
	LEFT JOIN Lider_EPS LIEPS ON (RA.IdEPS = LIEPS.IdEPS AND ME.IdCobertura = LIEPS.IdCobertura AND RA.IdPeriodo = LIEPS.IdPeriodo) 
	LEFT JOIN AUTH.Usuario USA ON (USA.Id = LIEPS.IdAuditorLider) ';
	-- //

	-- Calculo de paginado.  
	DECLARE @Paginate INT = @PageNumber * @MaxRows;  
	-- //

	-- Guardamos Condiciones.  
	DECLARE @Where VARCHAR(MAX) = '';  
	IF(@IdCobertura <> '')  
	BEGIN   
		IF(@Where = '')  
		BEGIN   
			SET @Where = @Where + 'ME.IdCobertura IN (' + CAST(@IdCobertura AS NVARCHAR(MAX)) + ')'  
		END   
		ElSE  
		BEGIN   
			SET @Where = @Where + ' AND ME.IdCobertura IN (' + CAST(@IdCobertura AS NVARCHAR(MAX)) + ')'  
		END   
	END
	--
	IF(@IdPeriodo <> '')  
	BEGIN   
		IF(@Where = '')  
		BEGIN   
			SET @Where = @Where + 'RA.IdPeriodo IN (' + CAST(@IdPeriodo AS NVARCHAR(MAX)) + ')'  
		END   
		ElSE  
		BEGIN   
			SET @Where = @Where + ' AND RA.IdPeriodo IN (' + CAST(@IdPeriodo AS NVARCHAR(MAX)) + ')'  
		END   
	END	 	
	-- //


	-- Validamos estado activo.
	IF(@Where = '')  
	BEGIN   
		SET @Where = @Where + ' RA.Status = 1 ' --  AND USA.Enable = 1 AND LIEPS.Status = 1
	END   
	ElSE  
	BEGIN   
		SET @Where = @Where + ' AND RA.Status = 1 ' -- AND USA.Enable = 1 AND LIEPS.Status = 1 
	END 
	-- //

	-- Paginado  
	DECLARE @Paginado VARCHAR(MAX) = '  
	ORDER BY MAX(ME.IdCobertura), MAX(RA.IdEPS)
	OFFSET ' + CAST(@Paginate AS NVARCHAR(MAX)) + ' ROWS  
	FETCH NEXT ' + CAST(@MaxRows AS NVARCHAR(MAX)) + ' ROWS ONLY;'  
	-- //

	-- GROUP BY, ORDER BY
	DECLARE @GroupBy VARCHAR(MAX) = 'GROUP BY ME.IdCobertura, RA.IdEPS, RA.IdPeriodo  '
	-- //

	-- Concatenamos Query, Condiciones y Paginado. Luego ejecutamos.  
	IF(@Where <> '' )  
	BEGIN   
		SET @Where = ' WHERE ' + @Where + @GroupBy;                
	END    
	DECLARE @Total VARCHAR(MAX);  
	SET @Total = @Query + '' + @Where  + ' ' + @Paginado  
	-- //


	-- Para calcular total registros filtrados.  
	DECLARE @QueryCount NVARCHAR(MAX);  
	SET @QueryCount = N'SELECT NEWID() AS Idk, COUNT(*) As NoRegistrosTotalesFiltrado  
	FROM RegistrosAuditoria RA 
	INNER JOIN CatalogoCobertura CCO ON CCO.NombreCatalogo = ' + '''' + '''' +  @CoberturaEPS  +  '''' + '''' + ' 
	LEFT JOIN CatalogoItemCobertura CIC ON (CCO.Id = CIC.CatalogoCoberturaId AND CIC.ItemId = RA.IdEPS) 
	INNER JOIN Medicion ME ON ME.Id = RA.IdMedicion 
	LEFT JOIN Lider_EPS LIEPS ON (RA.IdEPS = LIEPS.IdEPS AND ME.IdCobertura = LIEPS.IdCobertura AND RA.IdPeriodo = LIEPS.IdPeriodo) 
	LEFT JOIN AUTH.Usuario USA ON (USA.Id = LIEPS.IdAuditorLider) '  
  
	DECLARE @TotalCount NVARCHAR(MAX);  
	SET @TotalCount = '''' + @QueryCount + '' + @Where + ''''
  
	-- str, old_str. new_str  
	SET @Total = REPLACE(@Total, 'ScriptNoRegistrosTotales', @TotalCount);  
	-- //


	-- Imprimimos/Ejecutamos.  
	 --PRINT(@Total); 
	exec(@Total); 

END -- END SP