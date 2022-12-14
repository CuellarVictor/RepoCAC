USE [AuditCAC_Development]
GO
/****** Object:  StoredProcedure [dbo].[SP_Actualiza_Estado_Registro_Auditoria]    Script Date: 10/08/2022 10:05:54 p. m. ******/
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
							  WHERE rg.RegistrosAuditoriaId = @RegistroAuditoriaId 
								AND rg.Dato_DC_NC_ND <> @IdItemDC -- DC
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

		ELSE IF (@Estado = 17 AND @CountGlosaNCND = 0 AND @CountNCND = 0) -- RP Registro pendiente y todas las variables en DC
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
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Origen_BOT]    Script Date: 10/08/2022 10:05:54 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE   [dbo].[SP_Consulta_Origen_BOT]
	-- Add the parameters for the stored procedure here
	@InputData DT_LLave_Valor READONLY

AS
BEGIN
	
	-- Data Table calificacion CAC
DECLARE @CalificacionCAC DT_LLave_Valor

INSERT INTO @CalificacionCAC VALUES (32,'1') -- DC
INSERT INTO @CalificacionCAC VALUES (33,'2') -- NC
INSERT INTO @CalificacionCAC VALUES (34,'3') -- ND

	SELECT  
		RA.Id AS IdAuditing,
		RAD.Id AS AuditingDetail,
		RA.Identificacion AS Identificacion,
		V.idVariable AS Id_Variable,
		V.nemonico AS Variable,
		RAD.DatoReportado AS Valor_Variable,
		CAST(CA.Valor AS INT) AS Calificacion,
		itemTp.Id AS TipoVariableId,
		itemTp.ItemName AS TipoVariableNombre

	FROM  RegistrosAuditoria RA
		  INNER JOIN RegistrosAuditoriaDetalle RAD ON RA.Id = RAD.RegistrosAuditoriaId
		  INNER JOIN VariableXMedicion VXM ON VXM.MedicionId = RA.IdMedicion AND VXM.VariableId = RAD.VariableId
		  INNER JOIN Variables V ON V.Id = RA.Id
		  INNER JOIN @CalificacionCAC CA ON CA.Id = RAD.Dato_DC_NC_ND
		  INNER JOIN @InputData INP ON INP.Id = RA.IdRadicado
		  INNER JOIN Item itemTp ON itemTP.Id = V.TipoVariableItem

		 WHERE
		 V.TipoVariableItem IN	(35, -- Resolución
								 37) -- Adicional

		AND VXM.EsCalificable = 1
		AND VXM.Hallazgos = 1
		AND RA.[Status] = 1
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Registra_RegistroAuditoriaDetalleLog]    Script Date: 10/08/2022 10:05:54 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar - It Sense
-- Create date: 2022-08-10
-- Description:	Registra Log califiacion de variable
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Registra_RegistroAuditoriaDetalleLog]
	-- Add the parameters for the stored procedure here
	@RegistroAuditoriaDetalleId INT,
	@Calificacion INT,
	@Motivo NVARCHAR(255),
	@User NVARCHAR (255)
AS
BEGIN
	
	BEGIN TRY
			
			IF EXISTS (SELECT * FROM RegistrosAuditoriaDetalle WHERE Id = @RegistroAuditoriaDetalleId) 
			BEGIN

				DECLARE @DatoReportado NVARCHAR(MAX)
				DECLARE @CalificacionAnterior INT
				DECLARE @MotivoAnterior NVARCHAR(MAX)


				SELECT 
					@DatoReportado = RE.DatoReportado,
					@CalificacionAnterior = RE.Dato_DC_NC_ND,
					@MotivoAnterior = RE.MotivoVariable
				FROM RegistrosAuditoriaDetalle RE WHERE Id = @RegistroAuditoriaDetalleId 


				INSERT INTO [dbo].[RegistroAuditoriaDetalleLog]
						   ([RegistroAuditoriaDetalleId]
						   ,[CalificacionAnterior]
						   ,[CalificacionNueva]
						   ,[DatoReportado]
						   ,[MotivoAnterior]
						   ,[MotivoNuevo]
						   ,[CreateDate]
						   ,[CreateBy])
					 VALUES
						   (@RegistroAuditoriaDetalleId
						   ,@CalificacionAnterior
						   ,@Calificacion
						   ,@DatoReportado
						   ,@MotivoAnterior
						   ,@Motivo
						   ,GETDATE()
						   ,@User)
			END


	END TRY

	BEGIN CATCH
		PRINT('ERROR')
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Registra_Respuesta_BOT]    Script Date: 10/08/2022 10:05:54 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar
-- Create date: 2022-08-10
-- Description:	Registra respuesta proceso BOT
-- =============================================
CREATE OR ALTER  PROCEDURE [dbo].[SP_Registra_Respuesta_BOT] 

	@InputData DT_Registrar_Respuesta_Bot READONLY
AS
BEGIN
	
	-- VARIABLES
	DECLARE @TotalRecord INT = (SELECT COUNT(*) FROM @InputData)
	DECLARE @Position INT  = (SELECT top(1) Id FROM @InputData order by Id);

	DECLARE @RegistroAuditoriaId INT  = 0
	DECLARE @RegistroAuditoriaDetalleId INT  = 0
	DECLARE @MotivoBot NVARCHAR(255)
	DECLARE @CalificacionBot INT
	DECLARE @ContextoBot NVARCHAR(MAX)
	DECLARE @FechaBot  DATETIME 
	DECLARE @UsuarioBot NVARCHAR(255)
	DECLARE @Resultado NVARCHAR(255)
	DECLARE @Message NVARCHAR(255)


	-- Data Table calificacion CAC
	DECLARE @CalificacionCAC DT_LLave_Valor

	INSERT INTO @CalificacionCAC VALUES (32,'1') -- DC
	INSERT INTO @CalificacionCAC VALUES (33,'2') -- NC
	INSERT INTO @CalificacionCAC VALUES (34,'3') -- ND

	-- Crea tabla Registro Auditoria
	DECLARE @RegistrosAuditoria TABLE 
	(
		Id INT IDENTITY(1,1) PRIMARY KEY,
		RegistroAuditoriaId INT,
		Usuario NVARCHAR(255)
	)
	
	----------------------------------------------------------------------------------------------------------------------
	-- PROCESA REGISTROS AUDITORIA DETALLE
	----------------------------------------------------------------------------------------------------------------------

	 WHILE (@Position <= @TotalRecord)
	 BEGIN

			BEGIN TRY
				
				
				SELECT 
					@RegistroAuditoriaDetalleId = IdAuditingDetail,
					@MotivoBot = ResultadoBOT_Label,
					@CalificacionBot = ResultadoBOT_comparacion,
					@ContextoBot = Contextos,
					@FechaBot = FechaModificacion,
					@UsuarioBot = Usuario
				FROM @InputData WHERE Id = @Position

				BEGIN TRAN

					
					IF EXISTS (SELECT * FROM RegistrosAuditoriaDetalle WHERE Id = @RegistroAuditoriaDetalleId)
					BEGIN
					
							-- Consulta Id Registro Cabecera
							SET @RegistroAuditoriaId = (SELECT TOP(1) RegistrosAuditoriaId FROM RegistrosAuditoriaDetalle WHERE Id = @RegistroAuditoriaDetalleId)

							-- Registra Log
							EXEC	[dbo].[SP_Registra_RegistroAuditoriaDetalleLog]
										@RegistroAuditoriaDetalleId = @RegistroAuditoriaDetalleId,
										@Calificacion = @CalificacionBot,
										@Motivo = @MotivoBot,
										@User = @UsuarioBot


							-- Actualiza Registro Auditoria Detalle
							UPDATE RegistrosAuditoriaDetalle
								SET ModifyBy = @UsuarioBot,
									ModifyDate = @FechaBot,
									MotivoVariable = @MotivoBot,
									Dato_DC_NC_ND = (SELECT TOP(1) Id FROM  @CalificacionCAC WHERE Valor =  CAST(@CalificacionBot AS NVARCHAR(255))),
									Bot = 1
							   WHERE Id = @RegistroAuditoriaDetalleId

							-- Construye listado cabeceras
							IF NOT EXISTS (SELECT * FROM @RegistrosAuditoria WHERE RegistroAuditoriaId = @RegistroAuditoriaId)
							BEGIN
								INSERT INTO @RegistrosAuditoria VALUES (@RegistroAuditoriaId, @UsuarioBot)
							END

							 SET @Message =  CAST(@RegistroAuditoriaDetalleId AS NVARCHAR(255)) +', RegistroAuditoriaDetalleId Proceso Correcto'
							 SET @Resultado =  'OK'

					END

					ELSE

						BEGIN
								SET @Message =  CAST(@RegistroAuditoriaDetalleId AS NVARCHAR(255)) +', RegistroAuditoriaDetalleId NO existe el RegistroAuditoriaDetalleId '
								SET @Resultado =  'ERROR'
						END


				COMMIT TRAN	

			

			END TRY

			BEGIN CATCH	

						 ROLLBACK TRAN	
						 IF @RegistroAuditoriaDetalleId IS NOT NULL
						 BEGIN
							set @Message = CAST(@RegistroAuditoriaDetalleId AS nvarchar(255)) + + ', RegistroAuditoriaDetalleId ' + ERROR_MESSAGE() + '- SP LINEA: ' + CAST(ERROR_LINE() AS nvarchar(255));
						 END
						 ELSE
						 BEGIN 
							set @Message = CAST(@Position AS nvarchar(255)) + + ', Posicion ' + ERROR_MESSAGE() + '- SP LINEA: ' + CAST(ERROR_LINE() AS nvarchar(255));

						 END
				 
				 SET @Resultado =  'ERROR'

				

			END CATCH
				
			 -- Registra Log Bot
			EXEC SP_Insertar_Process_Log 510, @UsuarioBot, @Resultado, @Message;	

			SET @Position = @Position + 1

	 END -- WHILE

	 

	----------------------------------------------------------------------------------------------------------------------
	-- PROCESA REGISTROS AUDITORIA 
	----------------------------------------------------------------------------------------------------------------------
	
	-- VARIABLES
	DECLARE @TotalRecordRegistroAuditoria INT = (SELECT COUNT(*) FROM @RegistrosAuditoria)
	DECLARE @PositionRegistroAuditoria INT  = (SELECT top(1) Id FROM @RegistrosAuditoria order by Id);
	DECLARE @RegistroAuditoriaActual INT = 0
	DECLARE @UsuarioRegistro NVARCHAR(255)


	 WHILE (@PositionRegistroAuditoria <= @TotalRecordRegistroAuditoria)
		 BEGIN

			BEGIN TRY

				BEGIN TRAN

				SET @RegistroAuditoriaActual = (SELECT RegistroAuditoriaId FROM @RegistrosAuditoria WHERE Id = @PositionRegistroAuditoria) 
				SET @UsuarioRegistro = (SELECT Usuario FROM @RegistrosAuditoria WHERE Id = @PositionRegistroAuditoria) 

				-- Actualiza Estado
				EXEC [dbo].[SP_Actualiza_Estado_Registro_Auditoria]
					@userId = @UsuarioRegistro,
					@RegistroAuditoriaId = @RegistroAuditoriaActual,
					@BotonAccion = 0,
					@IsBot = 1
				COMMIT TRAN	

				SET @Message =  CAST(@RegistroAuditoriaActual AS NVARCHAR(255)) +', RegistroAuditoriaId Proceso Correcto'
				SET @Resultado =  'OK'

			END TRY

			BEGIN CATCH
				
				ROLLBACK TRAN
				set @Message = CAST(@RegistroAuditoriaActual AS nvarchar(255)) + + ', RegistroAuditoriaId ' + ERROR_MESSAGE() + '- SP LINEA: ' + CAST(ERROR_LINE() AS nvarchar(255));
				SET @Resultado =  'ERROR'


			END CATCH
				
			 -- Registra Log Bot
			EXEC SP_Insertar_Process_Log 510, @UsuarioRegistro, @Resultado, @Message;	

				

		    SET @PositionRegistroAuditoria = @PositionRegistroAuditoria + 1

		 END -- END while

END
GO
