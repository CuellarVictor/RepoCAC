USE AuditCAC_QAInterno
GO
/****** Object:  StoredProcedure [dbo].[GetDataMedicioneslider]    Script Date: 16/08/2022 12:16:56 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-03-23
-- Description:	Para consultar mediciones asociadas a un lider.
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[GetDataMedicioneslider]
(@PageNumber INT, @MaxRows INT, @IdLider VARCHAR(MAX), @IdCobertura VARCHAR(MAX), @IdEstado VARCHAR(MAX))
AS  
BEGIN  
SET NOCOUNT ON  

--Declaramos variable para Query    
DECLARE @Query VARCHAR(MAX);    
    
-- Validamos si es EsConSubGrupo, es decir hepatitis.
DECLARE @IdHEPATITIS VARCHAR(10) = (SELECT Valor FROM ParametrosGenerales WHERE Id = 37) -- 37: Parametro con Ids usados.


--Guardamos Query inicial.      
SET @Query = '    
SELECT   
   '' '' As QueryNoRegistrosTotales, 
   me.Id AS IdMedicion,    
   me.Nombre AS Medicion,      
   (Select isnull(TR.TotalRegistros, 0)) as TotalRegistros,       
   (Select isnull(TR.TotalRegistros, 0)) AS TotalAsignados,    
   e.idCobertura AS IdEnfMadre,    
   e.nombre  EnfMadre,    
   (Select isnull(RA.TotalAuditados, 0)) AS TotalAuditados,    
   MAX(est.Id) AS IdEstadoAuditoria,    
   MAX(est.ItemName) AS EstadoAuditoria,    
   MAX(me.Resolucion) AS Resolucion,    
   MAX(me.CreatedDate) AS FechaCreacion,    
   MAX(me.FechaInicioAuditoria) AS FechaInicio,  
   MAX(me.FechaFinAuditoria) AS FechaFin,        	
   MAX(me.ModifyDate) AS UltimaModificacion,     
    (SELECT ISNULL((SELECT TOP 1 CAST(Progress AS NVARCHAR(255)) AS Progreso from Current_Process WHERE Result LIKE ''%Medicion' + '''' +' + CAST(me.Id AS nvarchar(255))' + '+' + '''' +'%''), 0)) AS Progreso,
  CAST(CASE ISNULL(MAX(EF.idCobertura), 0) WHEN 0 THEN 0 ELSE 1 END AS BIT) As EsConSubGrupo ' + ' 
FROM Medicion me     
 JOIN Enfermedad e on e.idCobertura = me.IdCobertura ' + 
 'LEFT JOIN Enfermedad EF ON (EF.idCobertura = me.IdCobertura AND me.IdCobertura IN (' + @IdHEPATITIS + ')) ' +
 'JOIN Item est on est.Id = me.Estado     
 LEFT JOIN (select med.id, COUNT(reg.Id) as TotalAuditados from RegistrosAuditoria reg     
  INNER JOIN Medicion med on med.id = reg.IdMedicion    
  where reg.Estado not in (1,17) AND reg.Status = 1  
  group by med.id) as RA on RA.Id = me.Id    
 LEFT JOIN (select med.id, COUNT(reg.Id) as TotalRegistros from RegistrosAuditoria reg     
   INNER JOIN Medicion med on med.id = reg.IdMedicion    
   where reg.Status = 1
   group by med.id) as TR on TR.Id = me.Id 
   JOIN UsuarioXEnfermedad US on e.idCobertura = me.IdCobertura
   JOIN [AUTH].[Usuario] U on US.IdUsuario = U.Id ';    

   
--Calculo de paginado.  
--SET @PageNumber = @PageNumber - 1;  
DECLARE @Paginate INT = @PageNumber * @MaxRows;  


--Guardamos Condiciones.      
DECLARE @Where VARCHAR(MAX) = '';      
--      
IF(@IdLider <> '') -- or @IdLider <> '0'
BEGIN       

	--Obtenemos UserName    
	DECLARE @UserName VARCHAR(50) = ''    
	SELECT @UserName = Usuario 
	FROM [AUTH].[Usuario] 
	WHERE Id = @IdLider  

	--Obtenemos el roll
	DECLARE @RolId INT = 0
	SELECT @RolId = RolId FROM [AUTH].[Usuario] WHERE Id = @IdLider

	IF(@RolId != 1)
	BEGIN		
		 IF(@Where = '')      
		 BEGIN       
		  SET @Where = @Where + ' US.IdUsuario = ''' + CAST(@IdLider AS NVARCHAR(MAX)) +''''    
		  SET @Where = @Where + ' and me.IdCobertura IN (SELECT IdCobertura FROM UsuarioXEnfermedad WHERE IdUsuario = ''' + @IdLider + ''')'      
		 END
		 ElSE      
		 BEGIN       
		  SET @Where = @Where + ' and US.IdUsuario = (''' + CAST(@IdLider AS NVARCHAR(MAX)) + ''')' + ')'      
		  SET @Where = @Where + ' and me.IdCobertura IN (SELECT IdCobertura FROM UsuarioXEnfermedad WHERE IdUsuario = ''' + @IdLider + ''')' 
		 END 
	END
END
--      
IF(@IdCobertura <> '')      
BEGIN       
 IF(@Where = '')      
 BEGIN       
SET @Where = @Where + 'e.IdCobertura IN (' + CAST(@IdCobertura AS NVARCHAR(MAX)) + ')'      
 END       
    ElSE      
 BEGIN       
  SET @Where = @Where + 'and e.IdCobertura IN (' + CAST(@IdCobertura AS NVARCHAR(MAX)) + ')'      
 END       
END   --     
IF(@IdEstado <> '')      
BEGIN       
 IF(@Where = '')      
 BEGIN       
  SET @Where = @Where + 'est.Id IN (' + CAST(@IdEstado AS NVARCHAR(MAX)) + ')'      
 END       
    ElSE      
 BEGIN       
  SET @Where = @Where + ' AND est.Id IN (' + CAST(@IdEstado AS NVARCHAR(MAX)) + ')'      
 END       
END       
--      

IF(@Where = '')      
 BEGIN       
SET @Where = @Where + 'me.Status = 1 '      
 END       
    ElSE      
 BEGIN       
  SET @Where = @Where + 'and me.Status = 1 '      
 END       


    
--Paginado   
DECLARE @Paginado VARCHAR(MAX) = '  
ORDER BY me.Id desc
OFFSET ' + CAST(@Paginate AS NVARCHAR(MAX)) + ' ROWS  
FETCH NEXT ' + CAST(@MaxRows AS NVARCHAR(MAX)) + ' ROWS ONLY;'  


--Concatenamos Query, Condiciones y Paginado. Luego ejecutamos.  
IF(@Where <> '' )  
BEGIN   
 SET @Where = ' WHERE ' + @Where;                
END    
DECLARE @Total VARCHAR(MAX);  
DECLARE @GroupBy VARCHAR(MAX) = ' GROUP BY me.IdCobertura, me.Id, me.Nombre, TR.TotalRegistros, e.idCobertura, e.nombre, RA.TotalAuditados' 
--SET @Total = @Query + '' + @Where  + @GroupBy +  + ' order by me.Id desc '
SET @Total = @Query + '' + @Where + ' ' + @GroupBy + ' ' + @Paginado
--SET @Total = @Query  + @GroupBy


--Para calcular total registros filtrados.  
DECLARE @Query2 NVARCHAR(MAX);  
SET @Query2 = N'SELECT NEWID() As Idk, COUNT(me.Id) As NoRegistrosTotalesFiltrado     
FROM Medicion me        
JOIN Enfermedad e on e.idCobertura = me.IdCobertura       
JOIN Item est on est.Id = me.Estado 
WHERE me.Status = 1 '  
--
IF(@IdLider <> '') -- or @IdLider <> '0'
BEGIN    
SET @Query2 = @Query2 + ' and me.IdCobertura IN (SELECT IdCobertura FROM UsuarioXEnfermedad WHERE IdUsuario = ''''' + @IdLider + ''''') '
END
-- //


DECLARE @Total2 NVARCHAR(MAX);  
SET @Total2 = @Query2 --+ '' + REPLACE(@Where, '''', '''''')  

--str, old_str. new_str  
SET @Total = REPLACE(@Total, ''' '' As QueryNoRegistrosTotales', '''' + CAST(@Total2 AS NVARCHAR(MAX)) + ''' As QueryNoRegistrosTotales');  

--Imprimimos/Ejecutamos.	
EXEC(@Total);      
--PRINT(@Total);      
--SELECT @Total;      


END --END PROCEDURE
GO
/****** Object:  StoredProcedure [dbo].[SP_Actualiza_Estado_Registro_Auditoria]    Script Date: 16/08/2022 12:16:56 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER PROCEDURE [dbo].[SP_Actualiza_Estado_Registro_Auditoria]
	-- Add the parameters for the stored procedure here
	@userId NVARCHAR(255),
	@RegistroAuditoriaId INT,
	@BotonAccion INT,
	@IsBot BIT = 0
AS
BEGIN

		-- Variables
		DECLARE @Estado INT = (SELECT Estado FROM RegistrosAuditoria WHERE Id = @RegistroAuditoriaId);
		DECLARE @NombreEstado NVARCHAR(255);
		DECLARE @NuevoEstado INT = @Estado;
		DECLARE @NombreNuevoEstado NVARCHAR(255);
		DECLARE @Observacion NVARCHAR(1024) = '';
		DECLARE @Observacion2 NVARCHAR(1024) = '';
		DECLARE @IdItemGlosa INT = (SELECT Id FROM Item WHERE ItemName = 'Glosas' AND CatalogId = 4);
		DECLARE @IdItemDC INT = (SELECT Id FROM Item WHERE ItemName = 'DC' AND CatalogId = 6);
		DECLARE @IdItemNC INT = (SELECT Id FROM Item WHERE ItemName = 'NC' AND CatalogId = 6);
		DECLARE @CountGlosaNCND INT = 0;
		DECLARE @TipoObservacion INT = (SELECT Id FROM Item WHERE ItemName = 'General' AND CatalogId = 1)
		DECLARE @LevantarGlosa INT = (SELECT LevantarGlosa FROM RegistrosAuditoria WHERE Id = @RegistroAuditoriaId);
		DECLARE @MantenerCalifiacion INT = (SELECT MantenerCalificacion FROM RegistrosAuditoria WHERE Id = @RegistroAuditoriaId);
		DECLARE @ErroresReportados BIT = 0;
		DECLARE @CountNCND INT = 0;

		-- Count para saber si hay glosas NC o ND
		SET @CountGlosaNCND = (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rg
								INNER JOIN RegistrosAuditoria ra ON rg.RegistrosAuditoriaId = ra.Id
								INNER JOIN VariableXMedicion vm ON vm.VariableId = rg.VariableId AND ra.IdMedicion = vm.MedicionId
							  WHERE rg.RegistrosAuditoriaId = @RegistroAuditoriaId 
								AND vm.SubGrupoId = @IdItemGlosa -- Glosa
								AND rg.Dato_DC_NC_ND <> @IdItemDC -- DC
								AND vm.EsCalificable = 1
								)

		SET @CountNCND = (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rg
								INNER JOIN RegistrosAuditoria ra ON rg.RegistrosAuditoriaId = ra.Id
								INNER JOIN VariableXMedicion vm ON vm.VariableId = rg.VariableId AND ra.IdMedicion = vm.MedicionId
								INNER JOIN Variables v ON rg.VariableId = v.Id
							  WHERE rg.RegistrosAuditoriaId = @RegistroAuditoriaId 
								AND rg.Dato_DC_NC_ND <> @IdItemDC -- DC
								AND v.TipoVariableItem = 35	-- Resolución
								AND vm.EsCalificable = 1
								)


		-------------------- Validacion Errores -------------------------

		-- Count errores reportados
		IF (SELECT COUNT(*) FROM RegistrosAuditoriaDetalleSeguimiento
						WHERE RegistroAuditoriaId = @RegistroAuditoriaId 
						AND TipoObservacion = (SELECT Id FROM Item WHERE ItemName = 'Error lógica' and CatalogId = 1) -- Glosa
						) > 0

						BEGIN
							SET @ErroresReportados = 1

						END
						ELSE 
						BEGIN
							SET @ErroresReportados = 0

						END

		-- Count errores de logica
		DECLARE @CountErrores INT = (SELECT COUNT(*)
								  FROM RegistrosAuditoriaDetalle rd
										INNER JOIN  RegistroAuditoriaDetalleError re ON rd.Id = RegistrosAuditoriaDetalleId
										INNER JOIN Regla rg ON rg.idRegla = re.IdRegla
								  WHERE rd.RegistrosAuditoriaId = @RegistroAuditoriaId 
										AND Enable = 1)

	    -- Validacion para no permitir avance de cambio de estado si: Hay errores y no hay observacion de no corregbiles
		IF (@ErroresReportados = 0 AND @CountErrores > 0  AND @Estado = 17) --RP)
		BEGIN
			EXEC('DROP TABLE Error_Existen_Errores_de_logica_corregibles') -- Comando para generar error
		END

		

		-------------------- Fin Validacion Errores -------------------------
								
		IF (@Estado = 1 AND @IsBot = 0) --RN Registro nuevo
			BEGIN
				SET @NuevoEstado = 17 -- RP Registro pendiente
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
			END


		ELSE IF (@Estado = 17 AND @BotonAccion = 6) -- RP Registro pendiente y  accion errores encontrados
			BEGIN
				SET @NuevoEstado = 6	-- ELL Error lógica marcación auditor
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
			END

		ELSE IF (@Estado = 17 AND @CountGlosaNCND > 0) -- RP Registro pendiente
			BEGIN
				SET @NuevoEstado = 2 -- GRE
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
			END

		ELSE IF (@Estado = 17 AND @CountGlosaNCND = 0 AND @CountNCND = 0) -- RP Registro pendiente y todas las variables calificables en DC
			BEGIN
				SET @NuevoEstado = 16 -- RC registro Cerrado
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
			END

		ELSE IF (@Estado = 17 AND @CountGlosaNCND = 0 AND @CountNCND <> 0) -- RP Registro pendiente
			BEGIN
				SET @NuevoEstado = 8 -- E Enviado a entidad
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
			END

		ELSE IF (@Estado = 3 AND @BotonAccion = 3) -- GO1 Glosa objetada 1 y accion mantener calificacion
			BEGIN
				SET @NuevoEstado = 4 -- GORE Glosa objetada en revisión por la entidad
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
				SET @Observacion2 = 'El usuario mantiene calificación'
				SET @MantenerCalifiacion = @MantenerCalifiacion + 1
			END

	   ELSE IF (@Estado = 3 and @CountGlosaNCND > 0) -- GO1 Glosa objetada 1  y glosas NC o ND
			BEGIN
				SET @NuevoEstado = 4 -- GORE Glosa objetada en revisión por la entidad
				SET @Observacion2 = 'El usuario editó la calificación'
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
			END

		ELSE IF (@Estado = 3 AND @CountGlosaNCND = 0 AND @CountNCND = 0) -- GO1 Glosa objetada 1  y todas las variables en DC
			BEGIN
				SET @NuevoEstado = 16 -- RC registro Cerrado
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
			END

		ELSE IF (@Estado = 3) -- GO1 Glosa objetada 1 
			BEGIN
				SET @NuevoEstado = 8 -- E Enviado a entidad
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
			END

		ELSE IF (@Estado = 5 AND @BotonAccion = 2) -- GO2 Glosa objetada 2 Y Accion Levantar glosa
			BEGIN
				SET @NuevoEstado = @Estado -- GO Glosa objetada 2
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
				SET @LevantarGlosa = @LevantarGlosa + 1
				SET @Observacion2 = 'El usuario levanta glosa'

				-- Actualiza registro auditoria detalle
				UPDATE rad
					SET rad.Dato_DC_NC_ND = @IdItemDC,
					rad.MotivoVariable = ''
					FROM RegistrosAuditoriaDetalle rad
					INNER JOIN VariableXMedicion vm
					ON rad.VariableId = vm.VariableId
					AND vm.SubGrupoId = @IdItemGlosa
					AND rad.RegistrosAuditoriaId = @RegistroAuditoriaId
					INNER JOIN RegistrosAuditoria ra
					ON rad.RegistrosAuditoriaId = ra.Id
					AND ra.IdMedicion = vm.MedicionId;
			END

		ELSE IF (@Estado = 5 AND @BotonAccion = 3) -- GO2 Glosa objetada 2 y accion mantener calificacion
			BEGIN
				SET @NuevoEstado = 16	-- RC Registro cerrado
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
				SET @MantenerCalifiacion = @MantenerCalifiacion + 1
				SET @Observacion2 = 'El usuario mantiene calificación'

			END

		ELSE IF (@Estado = 5 AND @BotonAccion = 4) -- GO2 Glosa objetada 2 y accion comite administrativo
			BEGIN
				SET @NuevoEstado = 9	-- CA	Comité administrativo
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)

			END

		ELSE IF (@Estado = 5 AND @BotonAccion = 5) -- GO2 Glosa objetada 2 y accion comite experto
			BEGIN
				SET @NuevoEstado = 10 --	CE	Comité expertos
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)

			END
		
		ELSE IF (@Estado = 5 AND @CountGlosaNCND = 0 AND @CountNCND = 0) -- GO2 Glosa objetada 2  y todas las variables en DC
			BEGIN
				SET @NuevoEstado = 16 -- RC registro Cerrado
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
			END

		ELSE IF (@Estado = 5 AND @BotonAccion = 0) -- GO2 Glosa objetada 2 y guardar
			BEGIN
				SET @NuevoEstado = 8 -- E Enviado a entidad
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)

			END

		ELSE IF (@Estado = 9 AND @BotonAccion = 5) -- CA Comite Administrativo y accion comite experto
			BEGIN
				SET @NuevoEstado = 10 -- CE Comite experto
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)

			END

		ELSE IF (@Estado = 9 AND @BotonAccion = 2) -- CA Comite Administrativo y levantar glosa
			BEGIN
				SET @NuevoEstado = 8 -- E Enviado a la entidad
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
				SET @LevantarGlosa = @LevantarGlosa + 1
				SET @Observacion2 = 'El usuario levanta glosa'


			END
		ELSE IF (@Estado = 9 AND @BotonAccion = 3) -- CA Comite Administrativo y accion mantener calificacion
			BEGIN
				SET @NuevoEstado = 16	-- RC Registro cerrado
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
				SET @MantenerCalifiacion = @MantenerCalifiacion + 1
				SET @Observacion2 = 'El usuario mantiene calificación'

			END

		ELSE IF (@Estado = 10 AND @BotonAccion = 2) -- CE Comite Comite experto y levantar glosa
			BEGIN
				SET @NuevoEstado = 8 -- E Enviado a la entidad
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
				SET @LevantarGlosa = @LevantarGlosa + 1
				SET @Observacion2 = 'El usuario levanta glosa'

			END
		ELSE IF (@Estado = 10 AND @BotonAccion = 3) -- CE Comite Comite experto y accion mantener calificacion
			BEGIN
				SET @NuevoEstado = 16	-- RC Registro cerrado
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
				SET @MantenerCalifiacion = @MantenerCalifiacion + 1
				SET @Observacion2 = 'El usuario mantiene calificación'

			END

		ELSE IF (@Estado = 6 AND @CountGlosaNCND > 0) -- ELA	Error lógica marcación auditor y glosas no conformes o no disponibles
			BEGIN
				SET @NuevoEstado = 2 -- GRE
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
				SET @MantenerCalifiacion = @MantenerCalifiacion + 1

			END

		ELSE IF (@Estado = 6) -- ELA	Error lógica marcación auditor
			BEGIN
				SET @NuevoEstado = 7 --	ELL	Error lógica marcación líder
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
				SET @MantenerCalifiacion = @MantenerCalifiacion + 1
			END

		ELSE IF (@Estado = 7 AND @CountGlosaNCND > 0) -- ELL Error lógica marcación lider y glosas no conformes o no disponibles
			BEGIN
				SET @NuevoEstado = 2 -- GRE
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
				SET @MantenerCalifiacion = @MantenerCalifiacion + 1

			END
		ELSE IF (@Estado = 7 AND @CountNCND = 0) -- ELL Error lógica marcación lider y todas las variables calificables en DC
			BEGIN
				SET @NuevoEstado = 16 -- RC registro Cerrado
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
				SET @MantenerCalifiacion = @MantenerCalifiacion + 1

			END
		ELSE IF (@Estado = 7) -- ELL Error lógica marcación lider
			BEGIN
				SET @NuevoEstado = 8 -- E Enviado a la entidad
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
				SET @LevantarGlosa = @LevantarGlosa + 1

			END
		ELSE IF (@Estado = 12 AND @BotonAccion = 3) -- H1 Hallazgos1 y accion mantener calificacion
			BEGIN
				SET @NuevoEstado = 13	--H1E Hallazgo 1 enviado a entidad
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
				SET @MantenerCalifiacion = @MantenerCalifiacion + 1
				SET @Observacion2 = 'El usuario mantiene calificación'
			END

		ELSE IF (@Estado = 12) -- H1 Hallazgos1
			BEGIN
				SET @NuevoEstado = 16 -- RC registro Cerrado
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
			END
		ELSE IF (@Estado = 14 AND @BotonAccion = 3) --	H2L	Hallazgo 2 líder y accion mantener calificacion
			BEGIN
				SET @NuevoEstado = 16 -- RC registro Cerrado
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
				SET @MantenerCalifiacion = @MantenerCalifiacion + 1
				SET @Observacion2 = 'El usuario mantiene calificación'
			END

		ELSE IF (@Estado = 14) -- H2L	Hallazgo 2 líder
			BEGIN
				SET @NuevoEstado = 15 -- H2A	Hallazgo 2 auditor
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
			END
		ELSE IF (@Estado = 15) -- 	H2A	Hallazgo 2 auditor
		    BEGIN
				SET @NuevoEstado = 16 -- RC registro Cerrado
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
			END
		ELSE IF (@Estado = 16) -- 	-- RC registro Cerrado
		    BEGIN
				--
				-- AQUI TODO LO QUE DICE ALEJA.
				--
				SET @NuevoEstado = 17 -- RP Registro pendiente
				SET @TipoObservacion  = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)
			END
				

		--Consulta nombres de estado
		 SET @NombreEstado = ( SELECT Nombre FROM EstadosRegistroAuditoria WHERE Id = @Estado)
		 SET @NombreNuevoEstado = ( SELECT Nombre FROM EstadosRegistroAuditoria WHERE Id = @NuevoEstado)
		 SET @Observacion = 'Cambio de estado ' + @NombreEstado + ' a ' + @NombreNuevoEstado

		

		-- Observacion adicional
		IF @Observacion2 <> ''
		BEGIN

		
			-- Registra seguimiento
			INSERT INTO [dbo].[RegistrosAuditoriaDetalleSeguimiento]([RegistroAuditoriaId],[TipoObservacion],[Observacion],[Soporte],[EstadoActual]
					   ,[EstadoNuevo],[CreatedBy],[CreatedDate],[ModifyBy],[ModifyDate],[Status])
				 VALUES(@RegistroAuditoriaId,@TipoObservacion,@Observacion2,0,@Estado,@Estado,@userId,GETDATE(),@userId,GETDATE(),1)
		END

		-- Actualiza RegistrosAuditoria
		UPDATE RegistrosAuditoria SET Estado = @NuevoEstado, ModifyBy = @userId, ModifyDate = GETDATE(), LevantarGlosa = @LevantarGlosa, MantenerCalificacion = @MantenerCalifiacion WHERE Id = @RegistroAuditoriaId

		-- Registra seguimiento
		INSERT INTO [dbo].[RegistrosAuditoriaDetalleSeguimiento]([RegistroAuditoriaId],[TipoObservacion],[Observacion],[Soporte],[EstadoActual]
				   ,[EstadoNuevo],[CreatedBy],[CreatedDate],[ModifyBy],[ModifyDate],[Status])
			 VALUES(@RegistroAuditoriaId,@TipoObservacion,@Observacion,0,@Estado,@NuevoEstado,@userId,GETDATE(),@userId,GETDATE(),1)

		-- Registra Log Auditoria (RegistroAuditoriaLog)
		INSERT INTO [dbo].[RegistroAuditoriaLog] ([RegistroAuditoriaId], [Proceso], [Observacion], [EstadoAnterioId], [EstadoActual], [AsignadoA], [AsingadoPor], [CreatedBy], [CreatedDate], [ModifyBy], [ModificationDate], [Status])
		VALUES(@RegistroAuditoriaId, 'Cambio de estado.', @Observacion, @Estado, @NuevoEstado, @userId, @userId, @userId, GETDATE(), @userId, GETDATE(), 1)
		

		-- Insertamos Log de operacion. 
		DECLARE @IdRadicado INT = (SELECT IdRadicado FROM RegistrosAuditoria WHERE Id = @RegistroAuditoriaId);
		DECLARE @LogObservation VARCHAR(255) = 'Cronograma: Auditar (Cambio de estado). Radicado: ' + CAST(@IdRadicado AS nvarchar(255)) + ': ' + @Observacion;
	
		EXEC SP_Insertar_Process_Log 19, @userId, 'OK', @LogObservation;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_AsignaVariablesMedicion]    Script Date: 16/08/2022 12:16:56 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[SP_AsignaVariablesMedicion] 
	@IdMedicion INT, 
	@UserId varchar(50) 

AS
BEGIN

	SET NOCOUNT ON; 

	DECLARE @IdCobertura INT 
	SELECT @IdCobertura = IdCobertura FROM Medicion WHERE Id = @IdMedicion 

	DECLARE @TableVariables as table ( 
		Id int not null primary key identity(1,1),  
		IdVariable int not null, 
		Orden int not null, 
		tablareferencial NVARCHAR(MAX) 
	)
	
	DECLARE @Count int = 0, @Register int = 1 

	INSERT @TableVariables SELECT  Id, Orden, tablaReferencial FROM Variables WHERE idCobertura = @IdCobertura 

	SELECT @Count = COUNT(*) FROM @TableVariables 

	DECLARE @CalificacionXDefecto INT = 0 
	DECLARE @Subgrupo INT = 0 

	SELECT @CalificacionXDefecto = Item.Id	 
	FROM [Item]  
	inner join  [Catalog] on Item.CatalogId = Catalog.Id  
	Where CatalogName = 'Calificacion Defecto' AND Item.ItemName = 'DC' 
		
	SELECT @Subgrupo = Item.Id	 
	FROM [Item]  
	inner join  [Catalog] on Item.CatalogId = Catalog.Id  
	Where CatalogName = 'Sub-Grupo de variables' AND Item.ItemName = 'Sin grupo' 

	DECLARE @Orden int  = 0 
	DECLARE @VariableId int = 0 
	DECLARE @tablareferencial NVARCHAR(MAX) 
	DECLARE @Lista BIT 
	DECLARE @tipoVariable NVARCHAR(255) =  (SELECT top(1) idTipoVariable FROM Variables)
	DECLARE @tipoVariableItem INT




	WHILE(@Register <= @Count) 
	BEGIN 
		
		SELECT @VariableId = IdVariable, @Orden = Orden , @tablareferencial = tablareferencial 
		FROM @TableVariables WHERE Id = @Register 

			  SET @tipoVariable  =  (SELECT top(1) idTipoVariable FROM Variables where Id = @VariableId)

			  IF(@tipoVariable = 'int')
			  BEGIN
				SET @tipoVariableItem = 89 -- Decimal
			  END
			  ELSE IF(@tipoVariable = 'numeric')
			  BEGIN
				SET @tipoVariableItem = 86 -- Numerico
			  END
			  ELSE IF(@tipoVariable = 'datetime')
			  BEGIN
				SET @tipoVariableItem = 88 -- date
			  END
			  ELSE
			  BEGIN
				SET @tipoVariableItem = 87 -- alfanumerico
			  END


			IF @tablareferencial IS NULL OR @tablareferencial = '' 
			BEGIN 
				SET @Lista = 0 
			END 
			ELSE 
			BEGIN 
				SET @Lista = 1 
			END 

		INSERT VariableXMedicion (
			VariableId, MedicionId, Orden,EsGlosa, EsVisible, EsCalificable, Activo, EnableDC, EnableNC, EnableND, 
			Encuesta,SubGrupoId,CalificacionXDefecto,CreatedBy,CreationDate,ModifyBy,ModificationDate,[Status],
			TipoCampo,Promedio,ValidarEntreRangos,Desde,Hasta,Condicionada,ValorConstante, Lista
	   ) 
	   VALUES  
	   ( 
			@VariableId, 
			@IdMedicion,
			@Orden,
			0,
			1,
			1,
			1,			
			0,
			0,
			0,
			0,
			@Subgrupo,
			@CalificacionXDefecto,
			@UserId,
			GETDATE(),
			@UserId,
			GETDATE(),
			1,
			@tipoVariableItem, 
			0, -- Promedio
			1, -- ValidarEntreRango
			null, -- Desde
			null, -- Hasta
			0, -- Condicionado
			null, --Valor Constante,
			@Lista -- Lista

	   )		

		SET @Register = @Register + 1 
	END 

END 
GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Asignacion_Lider_Entidad]    Script Date: 16/08/2022 12:16:56 p. m. ******/
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
CREATE OR ALTER PROCEDURE [dbo].[SP_Consulta_Asignacion_Lider_Entidad]
(@PageNumber INT, @MaxRows INT, @IdCobertura VARCHAR(MAX), @IdPeriodo VARCHAR(MAX))
AS
BEGIN

	DECLARE @CoberturaEPS VARCHAR(MAX) = 'cacEPS'

	-- Guardamos Query inicial.  
	DECLARE @Query VARCHAR(MAX);  
	SET @Query = 'SELECT (ScriptNoRegistrosTotales) As NoRegistrosTotales, MAX(RA.IdEPS) As Data_IdEPS, ISNULL(MAX(CIC.ItemDescripcion), '''') As Data_NombreEPS, 
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
	EXEC(@Total); 

END -- END SP
GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Auditores_Asignacion_Lider_Entidad]    Script Date: 16/08/2022 12:16:56 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar
-- Create date: 2022-08-15
-- Description:	Consulta auditores por Cobertura, EPS, Idperiodo, para asignar el lider
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Consulta_Auditores_Asignacion_Lider_Entidad]
	
	@IdPeriodo INT,
	@IdCobertura INT,
	@IdEPS NVARCHAR(255)
AS
BEGIN
	
	SELECT  
		CAST(NEWID() AS NVARCHAR(255)) AS Id,
		RA.IdAuditor,
		MAX(US.Usuario) AS Usuario
	FROM Registrosauditoria RA
	INNER JOIN Medicion ME ON ME.Id = RA.IdMedicion 
	INNER JOIN AUTH.Usuario US ON US.Id = RA.IdAuditor
	WHERE RA.IdPeriodo = @IdPeriodo AND ME.IdCobertura = @IdCobertura AND RA.IdEPS = @IdEPS AND RA.[Status] = 1
	GROUP BY IdAuditor

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Hallazgos_Generados]    Script Date: 16/08/2022 12:16:56 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar - IT SENSE
-- Create date: 2022-08-12
-- Description:	Consulta de registros con hallazgos pendientes para enviar a la entidad por Id de Medicion (Suministro para implementacion de Modulo de hallazgos)
-- =============================================
CREATE OR ALTER   PROCEDURE [dbo].[SP_Consulta_Hallazgos_Generados]
	@MedicionId INT,
	@FechaInicial DATE,
	@FechaFinal DATE

AS
BEGIN
	
		-- Consulta registros primera instancia, tiene presente el rango de fecha, estado E, GRE
		SELECT
			RA.Id AS RegistroAuditoriaId,
			RA.IdRadicado AS RadicadoId,
			ES.Codigo AS CodigoEstado,
			ES.Descripción AS DescripcioneEstado,
			RAD.Id AS RegistroAuditoriaDetalleId,
			VA.idVariable AS IdVariableSISCAC,
			VA.nemonico AS Nemonico,
			itemTp.Id AS TipoVariableId,
			itemTp.ItemName AS TipoVariableNombre,
			CAST(RAD.ModifyDate AS DATE) AS FechaModificacion,
			RA.IdMedicion,
			(select TOP(3) ' OBSERVACION: ' + Observacion from RegistrosAuditoriaDetalleSeguimiento WHERE RegistroAuditoriaId = RA.Id  ORDER BY Id desc FOR XML PATH ('')) AS Observacion
		
		FROM RegistrosAuditoria RA

			INNER JOIN RegistrosAuditoriaDetalle RAD ON RAD.Id = RAD.RegistrosAuditoriaId
			INNER JOIN Variables VA ON VA.Id = RAD.VariableId 
			INNER JOIN VariableXMedicion VXM ON RAD.VariableId = VXM.VariableId AND VXM.MedicionId = RA.IdMedicion
			INNER JOIN EstadosRegistroAuditoria ES ON  ES.Id = RA.Estado
			INNER JOIN Item itemTp ON itemTP.Id = VA.TipoVariableItem
		WHERE
			RA.Estado IN (  8, -- E Enviado a entidad 
							2) -- GRE Glosa en revisión por la entidad


			AND VXM.Hallazgos = 1 -- Parametrizacion de variable (Aplica para hallazgos)

			AND CAST(RA.ModifyDate AS date) BETWEEN @FechaInicial AND @FechaFinal -- Rango de Modificacion del registro a auditar

			AND MedicionId = @MedicionId
			AND RA.[Status] = 1

		UNION 

		-- Consulta registros segunda instancia, NO tiene presente el rango de fecha, estado GORE, H1E
		SELECT
			RA.Id AS RegistroAuditoriaId,
			RA.IdRadicado AS RadicadoId,
			ES.Codigo AS CodigoEstado,
			ES.Descripción AS DescripcioneEstado,
			RAD.Id AS RegistroAuditoriaDetalleId,
			VA.idVariable AS IdVariableSISCAC,
			VA.nemonico AS Nemonico,
			itemTp.Id AS TipoVariableId,
			itemTp.ItemName AS TipoVariableNombre,
			CAST(RAD.ModifyDate AS DATE) AS FechaModificacion,
			RA.IdMedicion,
			(select TOP(3) ' OBSERVACION: ' + Observacion from RegistrosAuditoriaDetalleSeguimiento WHERE RegistroAuditoriaId = RA.Id  ORDER BY Id desc FOR XML PATH ('')) AS Observacion
		
		FROM RegistrosAuditoria RA

			INNER JOIN RegistrosAuditoriaDetalle RAD ON RAD.Id = RAD.RegistrosAuditoriaId
			INNER JOIN Variables VA ON VA.Id = RAD.VariableId 
			INNER JOIN VariableXMedicion VXM ON RAD.VariableId = VXM.VariableId AND VXM.MedicionId = RA.IdMedicion
			INNER JOIN EstadosRegistroAuditoria ES ON  ES.Id = RA.Estado
			INNER JOIN Item itemTp ON itemTP.Id = VA.TipoVariableItem

		WHERE
			RA.Estado IN (  4, -- GORE Glosa objetada en revisión por la entidad
							13) -- H1E Hallazgo 1 enviado a entidad
						
			AND VXM.Hallazgos = 1 -- Parametrizacion de variable (Aplica para hallazgos)

			AND MedicionId = @MedicionId
			AND RA.[Status] = 1
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Log_Accion]    Script Date: 16/08/2022 12:16:56 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar - IT SENSE
-- Create date: 2022-02-22
-- Description:	consulta el log de accion filtrando por fecha o nombre, observacion, codigo auditor
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Consulta_Log_Accion]

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
				,RA.[Proceso]
				,RA.[Observacion]
				,RA.[EstadoAnterioId]
				,RA.[EstadoActual]
				,CAST(AU.Codigo As NVARCHAR(250)) As Codigo
				,AU.Nombres
				,AU.Apellidos
				,AU.[Usuario] as NombreUsuario
				,RA.[CreatedDate]
				,RA.[ModifyBy]
				,RA.[ModificationDate]
				,RA.Status
				,CAST(CAST(RA.[CreatedDate] AS DATE) AS NVARCHAR(255)) AS Fecha
				,CAST(CONVERT(VARCHAR(5),RA.[CreatedDate],108) AS NVARCHAR(255)) AS Hora
				,ME.Id AS MedicionId
				,ME.Nombre AS NombreMedicion

		FROM [RegistroAuditoriaLog] RA
		INNER JOIN [AUTH].[Usuario] AU on RA.AsignadoA = AU.Id	
		INNER JOIN RegistrosAuditoria rsa ON RA.RegistroAuditoriaId = rsa.Id
		INNER JOIN Medicion ME ON rsa.IdMedicion = ME.Id

			WHERE RA.CreatedBy IN(''' + @IdAuditores + ''') ' 

		SET @QueryBaseCount = 
		'SELECT 
				COUNT(*)
		FROM [RegistroAuditoriaLog] RA
		INNER JOIN [AUTH].[Usuario] AU on RA.AsignadoA = AU.Id
		INNER JOIN RegistrosAuditoria rsa ON RA.RegistroAuditoriaId = rsa.Id
		INNER JOIN Medicion ME ON rsa.IdMedicion = ME.Id
			WHERE RA.CreatedBy IN(''' + @IdAuditores + ''') ' 

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

			exec(@QueryTotal)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Observacion_Temporal]    Script Date: 16/08/2022 12:16:56 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar
-- Create date: 2022-08-11
-- Description:	Consulta observacion temporal
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Consulta_Observacion_Temporal]
	-- Add the parameters for the stored procedure here
	@RegistroAuditoriaId INT
AS
BEGIN
	
	SELECT TOP(1) 
		Id AS Id, 
		ObservacionTemporal AS Valor 
	FROM RegistrosAuditoria 

	WHERE Id = @RegistroAuditoriaId AND Status = 1
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Periodos_Cobertura]    Script Date: 16/08/2022 12:16:56 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar
-- Create date: 2022-08-15
-- Description:	Consulta periodos con data en cronograma por Id de cobertura
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Consulta_Periodos_Cobertura]
	
	@IdCobertura INT

AS
BEGIN
	
	SELECT  
		RA.IdPeriodo AS Id,
		'' AS Valor
	FROM Registrosauditoria RA
	INNER JOIN Medicion ME ON ME.Id = RA.IdMedicion 
	WHERE ME.IdCobertura = @IdCobertura AND RA.[Status] = 1
	GROUP BY RA.IdPeriodo

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Registra_Respuesta_Hallazgos]    Script Date: 16/08/2022 12:16:56 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar - IT SENSE
-- Create date: 2022-08-12
-- Description:	Registra respuestas de las entidades sobre los hallazgos
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Registra_Respuesta_Hallazgos]
	-- Add the parameters for the stored procedure here
	@InputData DT_Registrar_Respuesta_Hallazgos READONLY
AS
BEGIN
	


	-- Declara tabla temporal para tomar Header Id
	DECLARE @HallazgosTemp TABLE 
	(
		Id INT IDENTITY(1,1) PRIMARY KEY,
		RegistroAuditoriaId INT,
		RegistrosAuditoriaDetalleId int NULL,
		Observacion varchar(500) NULL,
		Estado int NULL,
		CreatedBy varchar(450) NULL,
		CreatedDate datetime NULL,
		ModifyBy varchar(450) NULL,
		ModifyDate datetime NULL,
		[Status] bit NULL,
		Dato_DC_NC_ND int NULL
	)



	-- Inserta Hallazgo Temp
	INSERT INTO @HallazgosTemp
	SELECT 
			RA.Id,
			INPUT.RegistroAuditoriaDetalleId,
			INPUT.Observacion,
			INPUT.Estado,
			INPUT.Usuario,
			GETDATE(),
			INPUT.Usuario,
			GETDATE(),
			1,
			RAD.Dato_DC_NC_ND
	
	FROM @InputData INPUT
		INNER JOIN RegistrosAuditoriaDetalle RAD ON RAD.Id = INPUT.RegistroAuditoriaDetalleId
		INNER JOIN RegistrosAuditoria RA ON RAD.RegistrosAuditoriaId = RA.Id
	WHERE RA.[Status] = 1


	-- Inserta Hallazgos DB
	INSERT INTO Hallazgos
	SELECT 
		   RegistrosAuditoriaDetalleId
		  ,Observacion
		  ,Estado
		  ,CreatedBy
		  ,CreatedDate
		  ,ModifyBy
		  ,ModifyDate
		  ,Status
		  ,Dato_DC_NC_ND
	FROM  @HallazgosTemp

	-- Declara Tabla para agrupar Header Id
	DECLARE @HeaderGrouped TABLE 
	(
		Id INT IDENTITY(1,1) PRIMARY KEY,
		RegistroAuditoriaId INT
	)

	-- Inserta info data agrupada
	INSERT INTO @HeaderGrouped
	SELECT  RegistroAuditoriaId FROM @HallazgosTemp GROUP BY RegistroAuditoriaId ORDER BY RegistroAuditoriaId


	----------------------------------------------------------------------------------------------------------------------
	-- RECORRE AGRUPACION PARA CAMBIAR EL ESTADO DE REGISTRO AUDITORIA
	----------------------------------------------------------------------------------------------------------------------

	-- VARIABLES
	DECLARE @TotalRecord INT = (SELECT COUNT(*) FROM @HeaderGrouped)
	DECLARE @Position INT  = (SELECT top(1) Id FROM @HeaderGrouped order by Id);
	DECLARE @UsuarioRecord NVARCHAR(255)
	DECLARE @Resultado NVARCHAR(255)
	DECLARE @Message NVARCHAR(255)

	DECLARE @RegistroAuditoriaRecord NVARCHAR(255)
	DECLARE @CountPendienteRespuesta INT
	DECLARE @CountAcepta INT
	DECLARE @CountNoAcepta INT
	DECLARE @CountSilencioAdministrativo INT
	DECLARE @EstadoActual INT
	DECLARE @EstadoNuevo INT
	DECLARE @NombreEstado NVARCHAR(255);
	DECLARE @NombreNuevoEstado NVARCHAR(255);
	DECLARE @Observacion NVARCHAR(1024) = '';
	DECLARE @TipoObservacion  INT = (SELECT Id FROM  Item where ItemName = 'Cambio de estado' AND CatalogId = 1)

	WHILE (@Position <= @TotalRecord)
	 BEGIN
		
			BEGIN TRY

				BEGIN TRAN

					
					-- INICIA PROCESO REGISTRO ------------------------------------------------------------------------------------

					SET @RegistroAuditoriaRecord = (SELECT TOP(1) RegistroAuditoriaId FROM @HeaderGrouped WHERE Id = @Position)
					SET @EstadoActual = (SELECT TOP(1) Estado FROM RegistrosAuditoria WHERE Id = @Position)
					SET @UsuarioRecord = (SELECT TOP(1) CreatedBy FROM @HallazgosTemp WHERE RegistroAuditoriaId = @RegistroAuditoriaRecord ) 

					-- COUNT Respuestas
					SET @CountAcepta = (SELECT COUNT(*) FROM @HallazgosTemp WHERE RegistroAuditoriaId = @RegistroAuditoriaRecord AND Estado = 96 ) -- Acepta
					SET @CountNoAcepta = (SELECT COUNT(*) FROM @HallazgosTemp WHERE RegistroAuditoriaId = @RegistroAuditoriaRecord AND Estado = 97 ) -- No Acepta
					SET @CountSilencioAdministrativo = (SELECT COUNT(*) FROM @HallazgosTemp WHERE RegistroAuditoriaId = @RegistroAuditoriaRecord AND Estado = 98 ) -- Silencio Administrativo


					-- Validaciones para estado Nuevo (SELECT * FROM EstadosRegistroAuditoria)

					-- E => H1 Cuando algun de los los hallazgos tiene estado No Acepta
					IF @EstadoActual = 8 AND @CountNoAcepta > 0
					BEGIN
						SET @EstadoNuevo = 12 -- H1
					END

					-- E= > RC Cuando todos los hallazgos los acepta la entidad o si algun hallazgo tiene silencio administrativo
					ELSE IF @EstadoActual = 8 AND (@CountNoAcepta = 0 OR @CountSilencioAdministrativo > 0)
					BEGIN
						SET @EstadoNuevo = 16 -- RC
					END

					-- GRE => GO1, Cuando alguno de los estados de hallazgo es no acepta
					ELSE IF @EstadoActual = 2 AND @CountNoAcepta > 0
					BEGIN
						SET @EstadoNuevo = 3 -- GO1
					END

					-- GRE => RC Cuando todos los hallazgos los acepta la entidad o si algun hallazgo tiene silencio administrativo
					ELSE IF @EstadoActual = 2 AND (@CountNoAcepta = 0 OR @CountSilencioAdministrativo > 0)
					BEGIN
						SET @EstadoNuevo = 16 -- RC
					END

					-- GORE => GO2, Cuando alguno de los estados de hallazgo es no acepta
					ELSE IF @EstadoActual = 4 AND @CountNoAcepta > 0
					BEGIN
						SET @EstadoNuevo = 5 -- GO2
					END

					-- GORE => RC Cuando todos los hallazgos los acepta la entidad o si algun hallazgo tiene silencio administrativo
					ELSE IF @EstadoActual = 4 AND (@CountNoAcepta = 0 OR @CountSilencioAdministrativo > 0)
					BEGIN
						SET @EstadoNuevo = 16 -- RC
					END

					--H1E => H2L , Cuando no hay ninguno con silencio administrativo
					ELSE IF @EstadoActual = 13 AND (@CountSilencioAdministrativo = 0)
					BEGIN
						SET @EstadoNuevo = 14 -- H2L
					END

					-- H1E => RC Cuando hay silencio administrativo
					ELSE IF @EstadoActual = 13 AND (@CountSilencioAdministrativo = 0)
					BEGIN
						SET @EstadoNuevo = 16 -- RC
					END


					--Consulta nombres de estado
				 SET @NombreEstado = ( SELECT Nombre FROM EstadosRegistroAuditoria WHERE Id = @EstadoActual)
				 SET @NombreNuevoEstado = ( SELECT Nombre FROM EstadosRegistroAuditoria WHERE Id = @EstadoNuevo)
				 SET @Observacion = 'Cambio de estado ' + @NombreEstado + ' a ' + @NombreNuevoEstado

				 -- Actualiza RegistrosAuditoria
				UPDATE RegistrosAuditoria SET Estado = @EstadoNuevo, ModifyBy = @UsuarioRecord, ModifyDate = GETDATE() WHERE Id = @RegistroAuditoriaRecord

				-- Registra seguimiento
				INSERT INTO [dbo].[RegistrosAuditoriaDetalleSeguimiento]([RegistroAuditoriaId],[TipoObservacion],[Observacion],[Soporte],[EstadoActual]
						   ,[EstadoNuevo],[CreatedBy],[CreatedDate],[ModifyBy],[ModifyDate],[Status])
					 VALUES(@RegistroAuditoriaRecord,@TipoObservacion,@Observacion,0,@EstadoActual,@EstadoNuevo,@UsuarioRecord,GETDATE(),@UsuarioRecord,GETDATE(),1)

				-- Registra Log Auditoria (RegistroAuditoriaLog)
				INSERT INTO [dbo].[RegistroAuditoriaLog] ([RegistroAuditoriaId], [Proceso], [Observacion], [EstadoAnterioId], [EstadoActual], [AsignadoA], [AsingadoPor], [CreatedBy], [CreatedDate], [ModifyBy], [ModificationDate], [Status])
				VALUES(@RegistroAuditoriaRecord, 'Cambio de estado.', @Observacion, @EstadoActual, @EstadoNuevo, @UsuarioRecord, @UsuarioRecord, @UsuarioRecord, GETDATE(), @UsuarioRecord, GETDATE(), 1)
		

				-- Insertamos Log de operacion. 
				DECLARE @IdRadicado INT = (SELECT IdRadicado FROM RegistrosAuditoria WHERE Id = @RegistroAuditoriaRecord);
				DECLARE @LogObservation VARCHAR(255) = 'Cronograma: Auditar (Cambio de estado). Radicado: ' + CAST(@IdRadicado AS nvarchar(255)) + ': ' + @Observacion;
	
				SET @Message =  CAST(@RegistroAuditoriaRecord AS NVARCHAR(255)) +', RegistroAuditoriaId Proceso Correcto'
				SET @Resultado =  'OK'

					-- FINALIZA PROCESO REGISTRO ----------------------------------------------------------------------------------
				COMMIT TRAN	

			END TRY

			BEGIN CATCH
				ROLLBACK TRAN	

				set @Message = CAST(@RegistroAuditoriaRecord AS nvarchar(255)) + + ', RegistroAuditoriaId ' + ERROR_MESSAGE() + '- SP LINEA: ' + CAST(ERROR_LINE() AS nvarchar(255));
				SET @Resultado =  'ERROR'

			END CATCH
				
			 -- Registra Log 
			EXEC SP_Insertar_Process_Log 511, @UsuarioRecord, @Resultado, @Message;


			SET @Position = @Position + 1
		
	 END -- END WHILE


END
GO
/****** Object:  StoredProcedure [dbo].[SP_Upsert_Lider_EPS]    Script Date: 16/08/2022 12:16:56 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-08-01
-- Description:	Crea o Actualiza lideres de una EPS.
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Upsert_Lider_EPS]
(@IdEPS VARCHAR(10), @IdAuditorLider VARCHAR(450), @IdCobertura INT, @IdPeriodo INT, @Usuario VARCHAR(450))
AS
BEGIN
BEGIN TRANSACTION [Tran1]

	BEGIN TRY		
		DECLARE @ResponseId INT;
		DECLARE @Mensaje VARCHAR(MAX) = '';

		-- Validamos si vamos a Insertar o Actualizar
		IF NOT EXISTS (SELECT * FROM Lider_EPS WHERE IdPeriodo = @IdPeriodo AND IdEPS = @IdEPS AND IdCobertura = @IdCobertura)
		BEGIN
			-- Insertamos datos.
			INSERT INTO Lider_EPS(IdEPS, IdAuditorLider, IdCobertura, IdPeriodo, Status, CreatedBy, CreateDate, ModifyBy, ModifyDate)
			VALUES(@IdEPS, @IdAuditorLider, @IdCobertura, @IdPeriodo, 1, @Usuario, GETDATE(), @Usuario, GETDATE())
			-- //

			SET @ResponseId = (SELECT SCOPE_IDENTITY());
			SET @Mensaje = 'Exito';
			SELECT @ResponseId AS Id, @Mensaje AS Valor

			-- // --
		
			-- Insertamos Log de operacion. 
			-- EXEC SP_Insertar_Process_Log 14, @ModifyBy, 'OK', 'Gestor Variables: Editar';
			-- // 
		END
		ELSE
		BEGIN
			-- Actualizamos.
			UPDATE Lider_EPS
			SET
				IdEPS = @IdEPS, 
				IdAuditorLider = @IdAuditorLider, 
				IdCobertura = @IdCobertura, 
				IdPeriodo = @IdPeriodo,
				ModifyBy = @Usuario, 
				ModifyDate = GETDATE()
			WHERE 
					IdEPS = @IdEPS
					AND IdCobertura = @IdCobertura
					AND IdPeriodo = @IdPeriodo
			-- //

			SET @ResponseId = 1;
			SET @Mensaje = 'Exito';
			SELECT @ResponseId AS Id, @Mensaje AS Valor

			-- // --
		
			-- Insertamos Log de operacion. 
			-- EXEC SP_Insertar_Process_Log 14, @ModifyBy, 'OK', 'Gestor Variables: Editar';
			-- // 

		END -- END IF
		-- //

		COMMIT TRANSACTION [Tran1]

	END TRY

	BEGIN CATCH

		ROLLBACK TRANSACTION [Tran1]
		
		SET @Mensaje = 'Error: ' + ERROR_MESSAGE();
		SELECT 1 AS Id, @Mensaje AS Valor

		INSERT INTO TBL_TX_LOG(Date, Thread, Level, Logger, Message, Exception)
		VALUES(GETDATE(), '1', 'ERROR CATCH', 'Fallo EN SP SP_Upsert_Lider_EPS', ERROR_MESSAGE(), CONCAT('ErrorNumber: ',ERROR_NUMBER(),' - ', 'ERROR_STATE: ',ERROR_STATE(),' - ', 'ERROR_SEVERITY: ',ERROR_SEVERITY(),' - ', 'ERROR_PROCEDURE: ',ERROR_PROCEDURE(),' - ', 'ERROR_LINE: ',ERROR_LINE(),' - ', 'ERROR_MESSAGE: ',ERROR_MESSAGE()) );
	END CATCH 
END -- END SP
GO
/****** Object:  StoredProcedure [dbo].[SP_Upsert_Observacion_Temporal]    Script Date: 16/08/2022 12:16:56 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar
-- Create date: 2022-08-11
-- Description:	Actualiza observacion temporal
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Upsert_Observacion_Temporal]
	-- Add the parameters for the stored procedure here
	@RegistroAuditoriaId INT,
	@Observacion NVARCHAR(600)
AS
BEGIN
	
	UPDATE RegistrosAuditoria SET ObservacionTemporal = @Observacion WHERE Id = @RegistroAuditoriaId AND Status = 1
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Valida_Errores]    Script Date: 16/08/2022 12:16:56 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE - Victor Cuellar
-- Create date: 2022-04-19
-- Description:	Valida los errores por Id de Regla y Id Registro auditoria detalle
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Valida_Errores] 
	-- Add the parameters for the stored procedure here
	 @RegistroAuditoria INT,
     @User NVARCHAR(MAX)
AS

	
BEGIN

	DECLARE @ValidaErrorTipoA NVARCHAR(255) =   (SELECT Valor FROM ParametrosGenerales WHERE Nombre = 'ValidarErroesTipoA')
	DECLARE @ValidaErrorTipoB NVARCHAR(255) =   (SELECT Valor FROM ParametrosGenerales WHERE Nombre = 'ValidarErroesTipoB')
	DECLARE @Medicion INT = (SELECT IdMedicion FROM RegistrosAuditoria WHERE Id = @RegistroAuditoria)
	DECLARE @Cobertura INT = (SELECT IdCobertura FROM Medicion WHERE Id = @Medicion)
	

	-- Crea tabla resultado
	DECLARE @ResultData TABLE 
	(
		Id INT IDENTITY(1,1) PRIMARY KEY,
		IdRegistroAuditoriaDetalle INT,
		IdT INT, 
		Result BIT, 
		Obligatoria BIT,
		Sentencia NVARCHAR(MAX), 
		VariableId INT, 
		IdRegla INT, 
		IdRestriccion INT, 
		IdError NVARCHAR(255),
		DescripcionError NVARCHAR(MAX),
		ResultGroup BIT
	)

	-- Condicion para validar errroes tipo B
	IF (@ValidaErrorTipoB = 'true')
	BEGIN

			-- Crea tabla temporal
			DECLARE @ResultDataTemp TABLE 
				(
					Result BIT
				)


			-- Crea tabla temporal para consulta
			DECLARE @TableData TABLE (
				Id INT IDENTITY(1,1) PRIMARY KEY,
				IdRegistroAuditoriaDetalle INT,
				IdRegla INT, 
				idRestriccion INT,
				idError NVARCHAR(255),
				IdVariable INT,
				descripcion NVARCHAR(1024),
				TipoVariable1 NVARCHAR(255),
				Variable1  NVARCHAR(1024),
				idSignoComparacion NVARCHAR(255),
				idCompararCon INT, 
				Variable2  NVARCHAR(1024) NULL,
				valorEspecifico sql_variant NULL,
				TipoVariable3 NVARCHAR(255) NULL,
				Variable3  NVARCHAR(1024) NULL,
				idSignoComparacionAsociada NVARCHAR(255) NULL,
				idCompararConAsociada INT NULL,
				Variable4  NVARCHAR(1024) NULL,
				valorEspecificoAsociada sql_variant NULL
				)

			-- Inserta data de parametrizacion de las reglas a la tabla temporal
			INSERT INTO @TableData

			SELECT 
		
				rad.Id,
				rc.idRegla,
				rc.idRestriccionConsistencia,
				rg.idError,
				rad.VariableId,
				er.descripcion AS DescripcionError,
				v.idTipoVariable AS TipoVariable1,
				CASE WHEN rad.MotivoVariable IS NULL OR rad.MotivoVariable = '' THEN rad.DatoReportado ELSE rad.MotivoVariable END AS Variable1,
				rc.idSignoComparacion,
				rc.idCompararCon,
				CASE WHEN rad2.MotivoVariable IS NULL OR rad2.MotivoVariable = '' THEN rad2.DatoReportado ELSE rad2.MotivoVariable END AS Variable2,
				rc.valorEspecifico,
				v3.idTipoVariable AS TipoVariable3,
				CASE WHEN rad3.MotivoVariable IS NULL OR rad3.MotivoVariable = '' THEN rad3.DatoReportado ELSE rad3.MotivoVariable END AS Variable3,
				rc.idSignoComparacionAsociada,
				rc.idCompararConAsociada,
				CASE WHEN rad4.MotivoVariable IS NULL OR rad4.MotivoVariable = '' THEN rad4.DatoReportado ELSE rad4.MotivoVariable END AS Variable4,
				rc.valorEspecificoAsociada

				FROM RegistrosAuditoriaDetalle rad

					-- Variable 1 (Sentencia 1)
					INNER JOIN RegistrosAuditoria ra ON ra.Id = rad.RegistrosAuditoriaId
					INNER JOIN Medicion m ON m.Id = ra.IdMedicion
					INNER JOIN Variables v ON v.id = rad.VariableId
					INNER JOIN Regla rg ON rg.idVariable = v.idVariable
					INNER JOIN CoberturaError ce ON rg.idCobertura = ce.idCobertura 
													AND rg.idError = ce.idError
													AND m.IdCobertura = ce.idCobertura
					INNER JOIN ErrorRegla er ON er.idError = rg.IdError
					INNER JOIN RestriccionesConsistencia rc ON rc.idRegla = rg.idRegla
					INNER JOIN VariableXMedicion vmx ON vmx.VariableId = v.Id AND vmx.MedicionId = ra.IdMedicion

					-- Varaible  2 (Sentencia 1) 
					LEFT JOIN Variables v2 ON v2.idVariable = rc.idVariableComparacion
					LEFT JOIN RegistrosAuditoriaDetalle rad2 ON rad2.RegistrosAuditoriaId = rad.RegistrosAuditoriaId AND rad2.VariableId = v2.Id

					-- Varaible  3 (Sentencia 2) 
					LEFT JOIN Variables v3 ON v3.idVariable = rc.idVariableAsociada
					LEFT JOIN RegistrosAuditoriaDetalle rad3 ON rad3.RegistrosAuditoriaId = rad.RegistrosAuditoriaId AND rad3.VariableId = v3.Id

					-- Varaible  4 (Sentencia 2) 
					LEFT JOIN Variables v4 ON v4.idVariable = rc.idVariableComparacionAsociada
					LEFT JOIN RegistrosAuditoriaDetalle rad4 ON rad4.RegistrosAuditoriaId = rad.RegistrosAuditoriaId AND rad4.VariableId = v4.Id

				WHERE 
	
					rad.RegistrosAuditoriaId = @RegistroAuditoria
					AND rg.habilitado = 1
					AND vmx.EsCalificable = 1 
					AND vmx.MedicionId = @Medicion
					AND rad.Dato_DC_NC_ND <> 34 -- Para que no tenga encuenta errores cuando la calificacion es ND
				--	AND v.Id = 2389   --2239 AND rg.idRegla in ( 18479,18480)  -- Todo test
				ORDER BY rad.VariableId, idRegla

				-- Recorre sentencias consultadas
				DECLARE @position INT = 1;
				DECLARE @total INT = (SELECT TOP(1) Id FROM @TableData ORDER BY Id DESC);


				WHILE(@position <= @total)

					BEGIN

						DELETE FROM @ResultDataTemp  -- Limpia tabla temporal

						-- Sentencia 1 de la restriccion ------------------------------------------------------------------------
						DECLARE @ScriptComparacion1 NVARCHAR(MAX) = 'IF (Sentencia) BEGIN SELECT 1 END  ELSE BEGIN SELECT 0 END'
						DECLARE @Comparacion1 NVARCHAR(MAX)

				

						-- Ejecuta Validacion y la guarda en tabla temporal
							BEGIN TRY  
							
									-- Construye script de sentencia a ejecutar


									IF (
										((SELECT TipoVariable1 FROM @TableData WHERE Id = @position) = 'int') OR
										((SELECT TipoVariable1 FROM @TableData WHERE Id = @position) = 'intEDT') OR
										((SELECT TipoVariable1 FROM @TableData WHERE Id = @position) = 'numeric') 
										)

										BEGIN
										 --DECIMAL(15,2)
											SET @Comparacion1
												= (SELECT 
													'CAST(' +   '''' +  LTRIM(RTRIM(Variable1)) + '''' + 'AS DECIMAL(15,2)) ' 
													+ ' ' + LTRIM(RTRIM(idSignoComparacion))  + ' ' + 

													CASE WHEN idCompararCon = 2 
														THEN 'CAST(' +  '''' + LTRIM(RTRIM(CAST(valorEspecifico AS NVARCHAR(MAX))))  + ''''  + ' AS DECIMAL(15,2)) ' 
														ELSE 'CAST(' + '''' + LTRIM(RTRIM(CAST(Variable2 AS NVARCHAR(MAX))))  + '''' + ' AS DECIMAL(15,2)) ' 
													END AS Variable2 
												FROM @TableData WHERE Id = @position)
						
										END
									ELSE IF
									 ((SELECT TipoVariable1 FROM @TableData WHERE Id = @position) = 'datetime')

										BEGIN

											-- Consulta Variable 1 para generar el formato correcto de fecha
											DECLARE @Variable1 NVARCHAR(255) = (SELECT  Variable1 FROM @TableData WHERE Id = @position)

											IF CHARINDEX('/',@Variable1) > 0
											BEGIN
												EXEC [dbo].SP_Replace_Fecha_ErroresLogica @Input = @Variable1, @OutputDate = @Variable1 OUTPUT;  
											END
											-- Consulta Variable 2 para generar el formato correcto de fecha
											DECLARE @Variable2 NVARCHAR(255) = (SELECT  Variable2 FROM @TableData WHERE Id = @position)

											IF CHARINDEX('/',@Variable2) > 0
											BEGIN
												EXEC [dbo].SP_Replace_Fecha_ErroresLogica @Input = @Variable2, @OutputDate = @Variable2 OUTPUT;  
											END

											-- Consulta Valor especifico  para generar el formato correcto de fecha
											DECLARE @valorEspecifico NVARCHAR(255) = (SELECT  CAST(valorEspecifico AS NVARCHAR(MAX)) FROM @TableData WHERE Id = @position)

											IF CHARINDEX('/',@valorEspecifico) > 0
											BEGIN
												EXEC [dbo].SP_Replace_Fecha_ErroresLogica @Input = @valorEspecifico, @OutputDate = @valorEspecifico OUTPUT;
											END


											-- Consturye sentencia para comparar
											SET @Comparacion1
												= (SELECT 
													'CAST(' +   '''' +  LTRIM(RTRIM(@Variable1)) + '''' + 'AS DATE) ' 
													+ ' ' + LTRIM(RTRIM(idSignoComparacion))  + ' ' + 

													CASE WHEN idCompararCon = 2 
														THEN 'CAST(' +  '''' + LTRIM(RTRIM(CAST(@valorEspecifico AS NVARCHAR(MAX))))  + ''''  + ' AS DATE) ' 
														ELSE 'CAST(' + '''' + LTRIM(RTRIM(CAST(@Variable2 AS NVARCHAR(MAX))))  + '''' + ' AS DATE) ' 
													END AS Variable2 
												FROM @TableData WHERE Id = @position)
						
										END

										ELSE
											BEGIN
												SET @Comparacion1	
													= (SELECT 
														'''' +  LTRIM(RTRIM(Variable1)) + '''' + 
														+ ' ' + LTRIM(RTRIM(idSignoComparacion))  + ' ' +

														CASE WHEN idCompararCon = 2 
															THEN '''' + LTRIM(RTRIM(CAST(valorEspecifico AS NVARCHAR(MAX))))  + '''' 
															ELSE '''' + LTRIM(RTRIM(CAST(Variable2 AS NVARCHAR(MAX))))  + '''' 
														END AS Variable2 
													FROM @TableData WHERE Id = @position)
											END

				
			
									-- Replace script completo de validacion
									DECLARE @ScriptComparacionCompleto NVARCHAR(MAX) = REPLACE(@ScriptComparacion1, 'Sentencia', @Comparacion1) 	

									INSERT @ResultDataTemp  EXEC (@ScriptComparacionCompleto)


							END TRY 
		
							BEGIN CATCH  

									SET @Comparacion1	
										= (SELECT 
											'''' +  LTRIM(RTRIM(Variable1)) + '''' + 
											+ ' ' + LTRIM(RTRIM(idSignoComparacion))  + ' ' +

											CASE WHEN idCompararCon = 2 
												THEN '''' + LTRIM(RTRIM(CAST(valorEspecifico AS NVARCHAR(MAX))))  + '''' 
												ELSE '''' + LTRIM(RTRIM(CAST(Variable2 AS NVARCHAR(MAX))))  + '''' 
											END AS Variable2 
										FROM @TableData WHERE Id = @position)

									SET @ScriptComparacionCompleto = REPLACE(@ScriptComparacion1, 'Sentencia', @Comparacion1) 

									INSERT @ResultDataTemp  EXEC (@ScriptComparacionCompleto)			
							END CATCH

						-- Guarda en tabla resultado	
						DECLARE  @RowSentencia NVARCHAR(MAX) = REPLACE(REPLACE(REPLACE(@Comparacion1,'CAST(',''),'AS DECIMAL(15,2))',''),'AS DATE)','')


						IF @RowSentencia IS NOT NULL
						BEGIN 
					
							INSERT INTO @ResultData
							VALUES
								(
								(SELECT IdRegistroAuditoriaDetalle FROM @TableData WHERE Id = @position),
								@position,
								(SELECT TOP(1) Result FROM @ResultDataTemp),
								CASE WHEN (SELECT idSignoComparacionAsociada FROM @TableData WHERE Id = @position ) IS NULL THEN 1 ELSE 0 END,
								@RowSentencia,
								(SELECT IdVariable FROM @TableData WHERE Id = @position),
								(SELECT IdRegla FROM @TableData WHERE Id = @position),
								(SELECT idRestriccion FROM @TableData WHERE Id = @position),
								(SELECT idError FROM @TableData WHERE Id = @position),
								(SELECT descripcion FROM @TableData WHERE Id = @position),
								0
							)
					
						END
			
						-- Sentencia 2 de la restriccion ------------------------------------------------------------------------
						IF (
						(SELECT idSignoComparacionAsociada FROM @TableData WHERE Id = @position ) IS NOT NULL
						AND 
						(SELECT Result FROM @ResultDataTemp) = 1)

						BEGIN
							DELETE FROM @ResultDataTemp -- Limpia tabla temporal
							DECLARE @ScriptComparacion2 NVARCHAR(MAX) = 'IF (Sentencia) BEGIN SELECT 1 END  ELSE BEGIN SELECT 0 END'
							DECLARE @Comparacion2 NVARCHAR(MAX)

							-- DELETE FROM @ResultDataTemp

					

							---- Ejecuta Validacion y la guarda en tabla temporal
							BEGIN TRY  

									-- Construye script de sentencia a ejecutar
									IF (
										((SELECT TipoVariable3 FROM @TableData WHERE Id = @position) = 'int') OR
										((SELECT TipoVariable3 FROM @TableData WHERE Id = @position) = 'intEDT') OR
										((SELECT TipoVariable3 FROM @TableData WHERE Id = @position) = 'numeric') 
									)
									BEGIN
											SET @Comparacion2
												= (SELECT 
													'CAST(' +   '''' +  LTRIM(RTRIM(Variable3)) + '''' + 'AS DECIMAL(15,2)) ' 
													+ ' ' + LTRIM(RTRIM(idSignoComparacionAsociada))  + ' ' + 

													CASE WHEN idCompararConAsociada = 2 
														THEN 'CAST(' +  '''' + LTRIM(RTRIM(CAST(valorEspecificoAsociada AS NVARCHAR(MAX))))  + ''''  + ' AS DECIMAL(15,2)) ' 
														ELSE 'CAST(' + '''' + LTRIM(RTRIM(CAST(Variable4 AS NVARCHAR(MAX))))  + '''' + ' AS DECIMAL(15,2)) ' 
													END AS Variable2 
												FROM @TableData WHERE Id = @position)
									END
									ELSE IF
								 ((SELECT TipoVariable3 FROM @TableData WHERE Id = @position) = 'datetime')

									BEGIN
										-- Consulta Variable 1 para generar el formato correcto de fecha
										DECLARE @Variable3 NVARCHAR(255) = (SELECT  Variable3 FROM @TableData WHERE Id = @position)

										IF CHARINDEX('/',@Variable3) > 0
										BEGIN
											EXEC [dbo].SP_Replace_Fecha_ErroresLogica @Input = @Variable3, @OutputDate = @Variable3 OUTPUT;
										END

										-- Consulta Variable 2 para generar el formato correcto de fecha
										DECLARE @Variable4 NVARCHAR(255) = (SELECT  Variable4 FROM @TableData WHERE Id = @position)

										IF CHARINDEX('/',@Variable4) > 0
										BEGIN
											EXEC [dbo].SP_Replace_Fecha_ErroresLogica @Input = @Variable4, @OutputDate = @Variable4 OUTPUT;
										END

										-- Consulta Valor especifico  para generar el formato correcto de fecha
										DECLARE @valorEspecificoAsociada NVARCHAR(255) = (SELECT  CAST(valorEspecificoAsociada AS NVARCHAR(MAX))  FROM @TableData WHERE Id = @position)

										IF CHARINDEX('/',@valorEspecificoAsociada) > 0
										BEGIN
											EXEC [dbo].SP_Replace_Fecha_ErroresLogica @Input = @valorEspecificoAsociada, @OutputDate = @valorEspecificoAsociada OUTPUT;
										END


										-- Consturye sentencia para comparar
										SET @Comparacion2
											= (SELECT 
												'CAST(' +   '''' +  LTRIM(RTRIM(@Variable3)) + '''' + 'AS DATE) ' 
												+ ' ' + LTRIM(RTRIM(idSignoComparacionAsociada))  + ' ' + 

												CASE WHEN idCompararConAsociada = 2 
													THEN 'CAST(' +  '''' + LTRIM(RTRIM(CAST(@valorEspecificoAsociada AS NVARCHAR(MAX))))  + ''''  + ' AS DATE) ' 
													ELSE 'CAST(' + '''' + LTRIM(RTRIM(CAST(@Variable4 AS NVARCHAR(MAX))))  + '''' + ' AS DATE) ' 
												END AS Variable2 
											FROM @TableData WHERE Id = @position)
						
									END
									ELSE
										BEGIN
											SET @Comparacion2	
												= (SELECT 
													'''' +  LTRIM(RTRIM(Variable3)) + '''' + 
													+ ' ' + LTRIM(RTRIM(idSignoComparacionAsociada))  + ' ' +

													CASE WHEN idCompararConAsociada = 2 
														THEN '''' + LTRIM(RTRIM(CAST(valorEspecificoAsociada AS NVARCHAR(MAX))))  + '''' 
														ELSE '''' + LTRIM(RTRIM(CAST(Variable4 AS NVARCHAR(MAX))))  + '''' 
													END AS Variable2 
												FROM @TableData WHERE Id = @position)
										END

					
			
									-- Replace script completo de validacion
									DECLARE @ScriptComparacionCompleto2 NVARCHAR(MAX) = REPLACE(@ScriptComparacion2, 'Sentencia', @Comparacion2) 
									INSERT @ResultDataTemp  EXEC (@ScriptComparacionCompleto2)


							END TRY 
		
							BEGIN CATCH  

									SET @Comparacion2	
										= (SELECT 
											'''' +  LTRIM(RTRIM(Variable3)) + '''' + 
											+ ' ' + LTRIM(RTRIM(idSignoComparacionAsociada))  + ' ' +

											CASE WHEN idCompararConAsociada = 2 
												THEN '''' + LTRIM(RTRIM(CAST(valorEspecificoAsociada AS NVARCHAR(MAX))))  + '''' 
												ELSE '''' + LTRIM(RTRIM(CAST(Variable4 AS NVARCHAR(MAX))))  + '''' 
											END AS Variable2 
										FROM @TableData WHERE Id = @position)
									SET @ScriptComparacionCompleto2 = REPLACE(@ScriptComparacion2, 'Sentencia', @Comparacion2) 
							
									INSERT @ResultDataTemp  EXEC (@ScriptComparacionCompleto2)

							END CATCH
					
							-- Actualiza resultado si ya existe, esto lo hace con el fin de dejar solo una validacion por restriccion 
							UPDATE @ResultData

								SET Result = (SELECT TOP(1) Result FROM @ResultDataTemp),
								Obligatoria = CASE WHEN (SELECT idSignoComparacionAsociada FROM @TableData WHERE Id = @position ) IS NULL THEN 0 ELSE 1 END,
								Sentencia =  Sentencia +  ' Entonces ' + REPLACE(REPLACE(REPLACE(@Comparacion2,'CAST(',''),'AS DECIMAL(15,2))',''),'AS DATE)',''),
								VariableId = (SELECT IdVariable FROM @TableData WHERE Id = @position),
								IdRegla = (SELECT IdRegla FROM @TableData WHERE Id = @position),
								idRestriccion = (SELECT idRestriccion FROM @TableData WHERE Id = @position),
								IdError = (SELECT idError FROM @TableData WHERE Id = @position),
								DescripcionError = (SELECT descripcion FROM @TableData WHERE Id = @position)
							WHERE IdT = @position
		

						END

						SET @position = @position + 1

					END


		

				  -- Inicia recorrido tabla resultado, para verificar por agrupacion de reglas si los errores aplican

				  DECLARE  @totalResultData INT = (SELECT COUNT(*) FROM @ResultData)
				  DECLARE  @positionResultData INT = (SELECT TOP(1) ID FROM @ResultData)
				  DECLARE  @ReglaSeleccionada INT = 0
				  DECLARE  @ConteoRegistroPorRegla INT = 0
				  DECLARE  @VariableSeleccionada INT = 0
				  DECLARE  @RegostroAuditoriaDetalle INT = 0

				  -- Inicia recorrido tabla resultado
				  WHILE (@positionResultData <=  @totalResultData)
					  BEGIN
								-- Selecciona la regla del registro
								SET  @ReglaSeleccionada = (SELECT IdRegla FROM @ResultData WHERE Id = @positionResultData)
								SET  @VariableSeleccionada = (SELECT VariableId FROM @ResultData WHERE Id = @positionResultData)
								SET  @RegostroAuditoriaDetalle = (SELECT IdRegistroAuditoriaDetalle FROM @ResultData WHERE Id = @positionResultData)

								-- Realiza conteo para identificar si una de las restricciones de la regla no se cumple
								SET  @ConteoRegistroPorRegla = (SELECT COUNT(*) FROM @ResultData WHERE Id = @positionResultData 
																											AND IdRegla = @ReglaSeleccionada
																											AND VariableId = @VariableSeleccionada
																											AND IdRegistroAuditoriaDetalle = @RegostroAuditoriaDetalle
																											AND Obligatoria = 1
																											AND Result = 0 -- No cumple con la validacion
																											)
						
							   -- Si alguna de las restricciones no se cumple, setea el valor de Resultado en 1
							   IF (@ConteoRegistroPorRegla > 0)

							   BEGIN
									UPDATE @ResultData SET ResultGroup = 1 WHERE IdRegla = @ReglaSeleccionada AND VariableId = @VariableSeleccionada AND IdRegistroAuditoriaDetalle = @RegostroAuditoriaDetalle
							   END


							   SET @positionResultData = @positionResultData + 1
					
					  END

				-- Crea tabla resultado agrupado
				DECLARE @ResultDataGroup TABLE 
				(
					Id INT IDENTITY(1,1) PRIMARY KEY,
					Result BIT, 
					VariableId INT, 
					IdRegla INT, 
					Obligatoria BIT,
					ResultGroup BIT,
					IdError NVARCHAR(255)
				)

				-- Inserta datos agrupado para validar si la regla en total cumple con la validacion
				INSERT INTO @ResultDataGroup
				SELECT  MAX(CAST(Result AS INT)) AS Result, MAX(VariableId) AS VariableId, MAX(IdRegla) AS IdRegla, MAX(CAST(Obligatoria AS INT)),  MAX(CAST(ResultGroup AS INT))  AS ResultGroup, MAX(CAST(IdError AS nvarchar(255)))  
						FROM @ResultData
							GROUP BY VariableId, IdRegla
							ORDER BY VariableId, ResultGroup, IdRegla


				 -- Declara variables para data agrupada
				 DECLARE  @totalResultDataGroup INT = (SELECT COUNT(*) FROM @ResultDataGroup)
				 DECLARE  @positionResultDataGroup INT = (SELECT TOP(1) Id FROM @ResultDataGroup)
				 DECLARE  @ReglaSeleccionadaGroup INT = 0
				 DECLARE  @ConteoRegistroPorReglaGroup INT = 0
				 DECLARE  @VariableSeleccionadaGroup INT = 0

				  -- Inicia recorrido tabla resultado agrupado por variable y regla (PARA CONDICIONES AND)
				 --WHILE (@positionResultDataGroup <=  @totalResultDataGroup)
				 --BEGIN

					--	-- Selecciona la regla del registro
					--			SET  @ReglaSeleccionadaGroup = (SELECT IdRegla FROM @ResultDataGroup WHERE Id = @positionResultDataGroup)
					--			SET  @VariableSeleccionadaGroup = (SELECT VariableId FROM @ResultDataGroup WHERE Id = @positionResultDataGroup)

					--			-- Realiza conteo para identificar si una de las restricciones de la regla no se cumple
					--			SET  @ConteoRegistroPorReglaGroup = (SELECT COUNT(*) FROM @ResultDataGroup WHERE  
					--																							IdRegla = @ReglaSeleccionadaGroup
					--																						AND VariableId = @VariableSeleccionadaGroup
					--																						AND Obligatoria = 1
					--																						AND Result = 0) -- Indica que no cumple con la sentencia

					--		  -- Si encuentra que ninguna de las las restricciones por regla de la variable se cumple setea resultado grupal en 1
					--		   IF (@ConteoRegistroPorReglaGroup > 0)

					--		   BEGIN
						
					--				UPDATE @ResultData SET ResultGroup = 1 WHERE VariableId =  @VariableSeleccionadaGroup AND IdRegla = @ReglaSeleccionadaGroup
					--				UPDATE @ResultDataGroup SET ResultGroup = 1 WHERE VariableId =  @VariableSeleccionadaGroup AND IdRegla = @ReglaSeleccionadaGroup

					--		   END

					--		SET @positionResultDataGroup = @positionResultDataGroup + 1
				 --END



				  -- Inicia recorrido tabla resultado agrupado por variable y regla (PARA CONDICIONES OR)
				 DECLARE  @ErrorSeleccionadoGroup NVARCHAR(255)
				 SET @positionResultDataGroup = (SELECT TOP(1) Id FROM @ResultDataGroup)
				 WHILE (@positionResultDataGroup <=  @totalResultDataGroup)
				 BEGIN

						-- Selecciona la regla del registro
								SET  @ReglaSeleccionadaGroup = (SELECT IdRegla FROM @ResultDataGroup WHERE Id = @positionResultDataGroup)
								SET  @VariableSeleccionadaGroup = (SELECT VariableId FROM @ResultDataGroup WHERE Id = @positionResultDataGroup)
								SET  @ErrorSeleccionadoGroup = (SELECT IdError FROM @ResultDataGroup WHERE Id = @positionResultDataGroup)

								-- Realiza conteo para identificar si una de las restricciones de la regla no se cumple
								SET  @ConteoRegistroPorReglaGroup = (SELECT COUNT(*) FROM @ResultDataGroup WHERE 
																											VariableId = @VariableSeleccionadaGroup
																											AND IdError = @ErrorSeleccionadoGroup
																											AND IdRegla = @ReglaSeleccionadaGroup
																											AND Obligatoria = 1
																											AND ResultGroup = 0) -- Indica que cumple con la sentencia

							  -- Si encuentra que alguna de las reglas de la variable se cumple setea resultado en 0
							   IF (@ConteoRegistroPorReglaGroup > 0)

							   BEGIN
								   UPDATE @ResultData SET ResultGroup = 0 WHERE VariableId =  @VariableSeleccionadaGroup AND IdError = @ErrorSeleccionadoGroup
							   END
							   

							SET @positionResultDataGroup = @positionResultDataGroup + 1
				 END


	END

	
	-- Condicion para validar errroes tipo A
	IF (@ValidaErrorTipoA = 'true')
	BEGIN

		-- Crea tabla consulta data
		DECLARE @GetDataTipoA TABLE 
		(
			Id INT IDENTITY(1,1) PRIMARY KEY,
			IdRegistroAuditoriaDetalle INT,
			VariableId INT, 
			idTipoVariable NVARCHAR(MAX), 
			idErrorTipo NVARCHAR(MAX), 
			ValidacionTipoA INT, 
			DescripcionErrorTipo NVARCHAR(MAX), 
			Dato NVARCHAR(MAX), 
			tablaReferencial NVARCHAR(MAX)
		)


		

		INSERT INTO @GetDataTipoA
		SELECT 

				rd.Id,
				v.Id AS VariableId,
				v.idTipoVariable,
				ISNULL(ert.idError, 'TIPOA') AS idErrorTipo,

				--Validacion Tipo A

				CASE WHEN v.tablaReferencial <> '' AND v.tablaReferencial IS NOT NULL AND ci.ItemId IS NULL
					THEN
						0
				ELSE
					[dbo].[ValidarTipoDato](CASE WHEN rd.MotivoVariable IS NULL OR rd.MotivoVariable = '' THEN rd.DatoReportado ELSE rd.MotivoVariable END, v.idTipoVariable)

				END AS ValidacionTipoDato,
				CASE WHEN v.tablaReferencial <> '' AND v.tablaReferencial IS NOT NULL AND ci.ItemId IS NULL
					THEN
						'Dato no cumple con Tabla referencial ' + v.tablaReferencial
				ELSE
					 ISNULL(erT.descripcion, 'No cumple con el tipo de dato' + v.idTipoVariable) 
				END AS DescripcionErrorTipo,
				CASE WHEN rd.MotivoVariable IS NULL OR rd.MotivoVariable = '' THEN rd.DatoReportado ELSE rd.MotivoVariable END AS Dato,
				CASE WHEN v.tablaReferencial IS NULL OR v.tablaReferencial = '' THEN NULL ELSE v.tablaReferencial END AS tablaReferencial
				--,ci.ItemId AS ItemTablaReferencial
				FROM

					RegistrosAuditoriaDetalle rd
						INNER JOIN Variables v ON rd.VariableId = v.Id
						INNER JOIN RegistrosAuditoria ra ON ra.Id = rd.RegistrosAuditoriaId
						INNER JOIN VariableXMedicion vxm ON vxm.VariableId = v.Id AND vxm.MedicionId = ra.IdMedicion

						-- Error Tipo
						LEFT JOIN ErrorRegla erT ON erT.idError = v.idErrorTipo
						LEFT JOIN CoberturaError ceT ON ceT.idCobertura = 23
															AND erT.idError = ceT.idError

						-- Tabla Referencial		
						LEFT  JOIN  CatalogoCobertura cc ON cc.NombreCatalogo = v.tablaReferencial  AND v.tablaReferencial <> '' AND v.tablaReferencial IS NOT NULL
						LEFT JOIN CatalogoItemCobertura ci ON cc.Id = ci.CatalogoCoberturaId 
															AND 
															(CASE WHEN rd.MotivoVariable IS NULL OR rd.MotivoVariable = '' THEN rd.DatoReportado ELSE rd.MotivoVariable END)
															= ci.ItemId
				WHERE rd.RegistrosAuditoriaId = @RegistroAuditoria
							AND vxm.EsCalificable = 1 
							AND vxm.MedicionId = @Medicion
							AND rd.Dato_DC_NC_ND <> 34 
		
				

				
				-- Inserta resultado Tipo A
				INSERT INTO @ResultData
						SELECT 
								 IdRegistroAuditoriaDetalle,
								 0,
								 CASE WHEN ValidacionTipoA = 1 THEN 0 ELSE 1 END,
								 1,
								 DescripcionErrorTipo,
								 VariableId,
								 0,
								 0,
								 idErrorTipo,
								 DescripcionErrorTipo,
								 CASE WHEN ValidacionTipoA = 1 THEN 0 ELSE 1 END

							FROM  @GetDataTipoA


		
	END
	
		  
		   --Actualiza errores previamente registrados 
				UPDATE re
					SET 
					re.[Enable] = rd.ResultGroup,
					re.ModifyBy = @User,
					re.Descripcion = (CASE WHEN rd.DescripcionError IS NULL THEN rd.Sentencia ELSE rd.DescripcionError END),
					re.ModifyDate = GETDATE(),
					re.Sentencia = rd.Sentencia,
					re.Reducido = CASE WHEN rd.IdRegla  <> 0 THEN CONCAT('Regla: ',CAST (rd.IdRegla AS NVARCHAR(255)), ' - ', CAST (rd.IdRestriccion AS NVARCHAR(255))) ELSE 'Tipo A' END
					FROM RegistroAuditoriaDetalleError re -- Tabla a actualizar
						INNER JOIN @ResultData rd -- Tabla con la que valida info
						ON (rd.IdError IS NULL OR rd.IdError = re.ErrorId)
						AND rd.VariableId = re.Variableid
						AND rd.IdRegistroAuditoriaDetalle = re.RegistrosAuditoriaDetalleId
						AND rd.IdRestriccion = re.IdRestriccion


		-- Inserta los registros de errores que no existan
			INSERT INTO [dbo].[RegistroAuditoriaDetalleError]([RegistrosAuditoriaDetalleId],[IdRegla],[IdRestriccion],[Reducido],[VariableId],[ErrorId],[Descripcion],[Sentencia],[NoCorregible],[Enable],[CreatedBy],[CreatedDate],[ModifyBy],[ModifyDate])
			SELECT 
				rd.IdRegistroAuditoriaDetalle,
				rd.IdRegla, 
				rd.IdRestriccion,
				CASE WHEN rd.IdRegla  <> 0 THEN CONCAT('Regla: ',CAST (rd.IdRegla AS NVARCHAR(255)), ' - ', CAST (rd.IdRestriccion AS NVARCHAR(255))) ELSE 'Tipo A' END,
				rd.VariableId, 
				rd.IdError,
				CASE WHEN rd.DescripcionError IS NULL THEN rd.Sentencia ELSE rd.DescripcionError END AS DescripcionError,
				rd.Sentencia,
				0,
				rd.ResultGroup,
				@User,
				GETDATE(),
				@User,
				GETDATE()
			FROM  @ResultData rd
				LEFT JOIN RegistroAuditoriaDetalleError re 
						ON (rd.IdError IS NULL OR rd.IdError = re.ErrorId)
						AND rd.VariableId = re.Variableid
						AND rd.IdRegistroAuditoriaDetalle = re.RegistrosAuditoriaDetalleId 
						AND rd.IdRestriccion = re.IdRestriccion 
			WHERE re.Id IS NULL


			--Deshabilita Errores de variables que pasaron a ND
				UPDATE re
					SET 
					re.[Enable] = 0,
					re.ModifyBy = @User
					FROM RegistroAuditoriaDetalleError re -- Tabla a actualizar
						INNER JOIN RegistrosAuditoriaDetalle rd -- Tabla con la que valida info
						ON rd.Id = re.RegistrosAuditoriaDetalleId
						AND rd.Dato_DC_NC_ND = 34  


			EXEC SP_Consulta_ErrorRegistroAuditoria @RegistrosAuditoriaId = @RegistroAuditoria



END


GO
