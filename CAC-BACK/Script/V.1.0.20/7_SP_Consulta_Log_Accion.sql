USE [AuditCAC_Development]
GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Log_Accion]    Script Date: 17/08/2022 10:43:07 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar - IT SENSE
-- Create date: 2022-02-22
-- Description:	consulta el log de accion filtrando por fecha o nombre, observacion, codigo auditor
-- =============================================
ALTER PROCEDURE [dbo].[SP_Consulta_Log_Accion]

	@ParametroBusqueda NVARCHAR(255),
	@FechaInicial NVARCHAR(255),
	@FechaFinal NVARCHAR(255),
	@IdAuditores NVARCHAR(255),
	@Paginate INT,
	@MaxRows INT,
	@MedicionId INT
AS
BEGIN
	
		--- Query Variables
		DECLARE @QueryTotal VARCHAR(MAX) = '';
		DECLARE @QueryTotalCount VARCHAR(MAX) = '';
		DECLARE @QueryBase VARCHAR(MAX) = '';
		DECLARE @QueryBaseCount VARCHAR(MAX) = '';
		DECLARE @QueryWhere VARCHAR(MAX) = '';
		DECLARE @Paginador VARCHAR(MAX) =  '  ORDER BY RA.Id OFFSET (' + CAST(@Paginate AS NVARCHAR(255)) +  '- 1) * ' + CAST(@MaxRows AS NVARCHAR(255)) + ' ROWS FETCH NEXT ' + CAST(@MaxRows AS NVARCHAR(255)) + ' ROWS ONLY;'

		-- Eliminar caracteres extra
		--str, old_str. new_str  
		SET @IdAuditores = REPLACE(@IdAuditores, ''''',''''', ''',''');  

		-- Query 

		SET @QueryBase = 
		'SELECT 
				(ScriptQueryCount) AS CountQuery,
				0 AS Paginas,
				ra.Id AS Id,
				 rsa.IdRadicado AS IdRadicado
				,RA.[RegistroAuditoriaId]
				,itemP.ItemName AS [Proceso]
				,RA.[Observacion]
				,RA.EstadoActual AS EstadoAnterioId
				,RA.EstadoNuevo AS EstadoActual
				,CAST(AU.Codigo As NVARCHAR(250)) As Codigo
				,AU.Nombres
				,AU.Apellidos
				,AU.[Usuario] as NombreUsuario
				,RA.[CreatedDate]
				,RA.[ModifyBy]
				,RA.ModifyDate AS ModificationDate
				,RA.Status
				,CAST(CAST(RA.[CreatedDate] AS DATE) AS NVARCHAR(255)) AS Fecha
				,CAST(CONVERT(VARCHAR(5),RA.[CreatedDate],108) AS NVARCHAR(255)) AS Hora
				,ME.Id AS MedicionId
				,ME.Nombre AS NombreMedicion

		FROM RegistrosAuditoriaDetalleSeguimiento RA
		INNER JOIN RegistrosAuditoria rsa ON RA.RegistroAuditoriaId = rsa.Id
		INNER JOIN [AUTH].[Usuario] AU on rsa.IdAuditor = AU.Id	
		INNER JOIN Medicion ME ON rsa.IdMedicion = ME.Id
		INNER JOIN Item itemP on itemP.Id = RA.TipoObservacion

			WHERE rsa.IdAuditor IN(''' + @IdAuditores + ''') ' 

		SET @QueryBaseCount = 
		'SELECT 
				COUNT(*)
		FROM RegistrosAuditoriaDetalleSeguimiento RA
		INNER JOIN RegistrosAuditoria rsa ON RA.RegistroAuditoriaId = rsa.Id
		INNER JOIN [AUTH].[Usuario] AU on rsa.IdAuditor = AU.Id	
		INNER JOIN Medicion ME ON rsa.IdMedicion = ME.Id
		INNER JOIN Item itemP on itemP.Id = RA.TipoObservacion
			WHERE rsa.IdAuditor IN(''' + @IdAuditores + ''') ' 

		SET @QueryTotal = @QueryBase + @QueryWhere;
		SET @QueryTotalCount = @QueryBaseCount + @QueryWhere;


		IF @ParametroBusqueda <> ''

			BEGIN
		
			  SET @QueryWhere = @QueryWhere +

			  ' AND
				(
					RA.Observacion LIKE ''%' + @ParametroBusqueda + '%''' +
					' OR ' +
					'CAST(rsa.IdRadicado AS NVARCHAR(255)) LIKE ''%' + @ParametroBusqueda + '%''' +
					' OR ' +
					'AU.Nombres LIKE ''%' + @ParametroBusqueda + '%''' +
					' OR ' +
					'AU.Apellidos LIKE ''%' + @ParametroBusqueda + '%''' +
					' OR ' +
					'CAST(AU.Codigo AS NVARCHAR(255)) LIKE ''%' + @ParametroBusqueda + '%''' +
				')'

			   SET @QueryTotal = @QueryBase + @QueryWhere;
			   SET @QueryTotalCount = @QueryBaseCount + @QueryWhere;

			END

			IF @FechaFinal <> ''
			BEGIN 
				SET @QueryWhere = @QueryWhere +
				' AND
				CAST(RA.CreatedDate AS DATE) BETWEEN' + '''' + @FechaInicial + ''' AND ' + '''' + @FechaFinal + '''';

				 SET @QueryTotal = @QueryBase + @QueryWhere;
				 SET @QueryTotalCount = @QueryBaseCount + @QueryWhere;

			END

			IF @MedicionId <> 0
			BEGIN 
				SET @QueryWhere = @QueryWhere +
				' AND rsa.IdMedicion = ' + CAST(@MedicionId AS nvarchar(255)) + ' ';

				 SET @QueryTotal = @QueryBase + @QueryWhere;
				 SET @QueryTotalCount = @QueryBaseCount + @QueryWhere;

			END

			SET @QueryTotal = REPLACE(@QueryTotal, 'ScriptQueryCount', @QueryTotalCount) + @Paginador

			EXEC(@QueryTotal)
END
