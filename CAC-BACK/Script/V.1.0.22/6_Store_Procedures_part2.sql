USE AuditCAC_QAInterno
GO
/****** Object:  StoredProcedure [dbo].[SP_Actualiza_Estado_Registro_Auditoria]    Script Date: 19/09/2022 12:49:34 a. m. ******/
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
		-- SP Valida Glosas
		EXEC @CountGlosaNCND = [dbo].[SP_Validacion_Conteo_Glosas] @RegistroAuditoriaId = @RegistroAuditoriaId

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
										LEFT JOIN Regla rg ON rg.idRegla = re.IdRegla
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
/****** Object:  StoredProcedure [dbo].[SP_Calculadora_KRU]    Script Date: 19/09/2022 12:49:34 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:	Victor Cuellar - IT SENSE
-- Create date: 2022-09-08
-- Description:	Calculadora TFG
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Calculadora_KRU]
	-- Add the parameters for the stored procedure here
	@Hemodialisis BIT,
	@NitrogenoUrinario DECIMAL(10,2),
	@VolumenUrinario DECIMAL(10,2) ,
	@BrunPre DECIMAL(10,2),
	@BrunPost DECIMAL(10,2)

AS
BEGIN


	DECLARE @Minutos INT = 0;
	DECLARE @dividendo DECIMAL(10,2);
	DECLARE @divisor DECIMAL(10,2);
	DECLARE @Brun DECIMAL(10,2);
	DECLARE @Resultado DECIMAL(10,2);


	-- Define tiempo para hemodialisis y dialisis
	IF @Hemodialisis = 1
		BEGIN
			SET @Minutos = 2640
			SET @Brun = (@BrunPre + @BrunPost)/2
		END
	ELSE
		BEGIN
			SET @Minutos = 1440
			SET @Brun = @BrunPre
		END



	SET @dividendo = @NitrogenoUrinario * @VolumenUrinario
	SET @divisor = @Brun * @Minutos

	SET @Resultado = @dividendo / @divisor


	SELECT 1 AS Id, CAST(@Resultado AS NVARCHAR(255)) AS Valor;

END

/****** Object:  StoredProcedure [dbo].[SP_Calculadora_TFG]    Script Date: 19/09/2022 12:49:34 a. m. ******/

-- =============================================
-- Author:	Victor Cuellar - IT SENSE
-- Create date: 2022-09-08
-- Description:	Calculadora TFG
-- =============================================
-- EXEC	[dbo].[SP_Calculadora_TFG]@edad = 39,@hombre = 1,@creatinina = 0.76,@peso = 84,@estatura = 0
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[SP_Calculadora_TFG]
	
	@edad DECIMAL(10,2),
	@hombre BIT,
	@creatinina DECIMAL(10,2),

	-- Mayor de edad
	@peso DECIMAL(10,2),
	-- Menor de edad
	@estatura DECIMAL(10,2)

AS
BEGIN
	
	-- Variable 
	DECLARE @mayorEdad BIT = 0;
	DECLARE @constante DECIMAL(10,2);
	DECLARE @dividendo DECIMAL(10,2);
	DECLARE @divisor DECIMAL(10,2);
	DECLARE @Resultado DECIMAL(10,2);
	DECLARE @ValorBase  DECIMAL(10,2) = 140.00

	IF @edad > 18 
	BEGIN
		SET @mayorEdad = 1
	END


	-- Define cosntante de acuerdo al genero
	IF @hombre = 1
		BEGIN	
			 SET @constante = @peso;

			 IF @edad BETWEEN 0 AND 13
				 BEGIN
					SET @constante = 0.55 
				 END
			 ELSE IF @edad BETWEEN 14 AND 18
				 BEGIN
					SET @constante = 0.7 
				 END
		END
	ELSE
		BEGIN	
			 SET @constante = @peso  * 0.84;

			 IF @mayorEdad = 0
				 BEGIN
					SET @constante = 0.55
				 END
		END


	-- Calcula formula de acuerdo a la edad
	IF @mayorEdad = 1
			BEGIN
					SET @dividendo = CAST(((@ValorBase - @edad)  * @constante) AS DECIMAL (10,2));
					SET @divisor = 72 * @creatinina

			END
		ELSE
			BEGIN
			
					SET @dividendo = @estatura * @constante;
					SET @divisor = @creatinina
			END

		SET @Resultado = @dividendo / @divisor;

		SELECT 1 AS Id, CAST(@Resultado AS NVARCHAR(255)) AS Valor;

END
GO
