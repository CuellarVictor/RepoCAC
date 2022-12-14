USE [AuditCAC_Development]
GO
/****** Object:  StoredProcedure [dbo].[CrearVariables]    Script Date: 19/09/2022 12:45:31 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-01-11
-- Description:	Para Crear una variable con todos sus datos vinculados.
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[CrearVariables]
(@Variable VARCHAR(MAX), @Orden INT, @idCobertura INT, @nombre VARCHAR(100), @descripcion VARCHAR(500), @idTipoVariable VARCHAR(500), @longitud INT, @decimales INT, @formato VARCHAR(300), @tablaReferencial VARCHAR(128), @CreatedBy VARCHAR(MAX), @ModifyBy VARCHAR(50), @TipoVariableItem INT, 
 @EstructuraVariable INT, @Alerta BIT, @AlertaDescripcion VARCHAR(MAX), @MedicionId VARCHAR(MAX), @EsVisible BIT, @EsCalificable BIT, @Hallazgos BIT, @EnableDC BIT, @EnableNC BIT, @EnableND BIT, @CalificacionXDefecto INT, @SubGrupoId INT, @Encuesta BIT, @CalificacionIPSItem VARCHAR(MAX),
	@TipoCampo int,
	@Promedio bit,
	@ValidarEntreRangos bit,
	@Desde nvarchar(255),
	@Hasta nvarchar(255),
	@Condicionada bit,
	@ValorConstante nvarchar(255),
	@Lista BIT,
	@VariablesCondicional DT_LLave_Valor READONLY,
	@Calculadora BIT,
	@TipoCalculadora INT)

AS
BEGIN
SET NOCOUNT ON

--Declaramos variables con valor default.
DECLARE @Activa BIT = 1;
DECLARE @idVariable INT = 1;
DECLARE @nemonico VARCHAR(50) = @nombre; -- Nemo Test SP
--DECLARE @longitud INT = 10;
--DECLARE @decimales INT = 1;
--DECLARE @formato VARCHAR(300) = '' ; -- Format TEST
DECLARE @campoReferencial VARCHAR(128) = '';
DECLARE @MotivoVariable NVARCHAR(MAX) = '';
DECLARE @Bot BIT = 1;
DECLARE @EsGlosa BIT = 1;
DECLARE @Activo BIT = 1;
DECLARE @VxM_Orden INT = @Orden;

BEGIN TRANSACTION [Tran1]

	BEGIN TRY


		IF (@TipoCampo = 86) -- Numerico
			BEGIN
				SET @idTipoVariable	 = 'int'
			END

		ELSE IF (@TipoCampo = 87) -- Alfanumerico
			BEGIN
				SET @idTipoVariable	 = 'varchar'
			END

		ELSE IF (@TipoCampo = 88) -- Fecha
			BEGIN
				SET @idTipoVariable	 = 'datetime'
			END
		ELSE IF (@TipoCampo = 89) -- Decimal
			BEGIN
				SET @idTipoVariable	 = 'numeric'
			END
		ELSE 
			BEGIN
				SET @idTipoVariable	 = 'varchar'
			END

		--Declaramos variables
		DECLARE @OutputTbl TABLE (Id INT)
		DECLARE @ItemValueVxM INT;
		DECLARE @ItemValueVxI INT;
		DECLARE @VariableId INT;

		--Insertamos en Variables.
		SET @EstructuraVariable = 1;
		INSERT INTO Variables(Activa, Orden, idVariable, idCobertura, nombre, nemonico, descripcion, idTipoVariable, longitud, decimales, formato, tablaReferencial, campoReferencial, CreatedBy, CreatedDate, ModifyBy, ModifyDate, MotivoVariable, Bot, TipoVariableItem, EstructuraVariable, Alerta, AlertaDescripcion) OUTPUT INSERTED.ID INTO @OutputTbl(Id)
		VALUES (@Activa, @Orden, @idVariable, @idCobertura, @nombre, @nemonico, @descripcion, @idTipoVariable, @longitud, @decimales, @formato, @tablaReferencial, @campoReferencial, @CreatedBy, GETDATE(), @ModifyBy, GETDATE(), @MotivoVariable, @Bot, @TipoVariableItem, @EstructuraVariable, @Alerta, @AlertaDescripcion);

		--Capturamos Id Insertado.
		SET @VariableId = (SELECT Id FROM @OutputTbl);
		
		UPDATE Variables SET idVariable = @VariableId WHERE Id = @VariableId;
		-- // --

		--Insertamos en VariableXMedicion 

		--Verificamos si debemos eliminar tabla temporal.
		BEGIN TRY
		DROP TABLE #TablaTemporalVxM
		END TRY
		BEGIN CATCH  END CATCH

		--Creamos Tabla Temporal.
		CREATE TABLE #TablaTemporalVxM (Id INT IDENTITY(1, 1) PRIMARY KEY, campo VARCHAR(128));
		INSERT INTO #TablaTemporalVxM
		--SELECT value FROM STRING_SPLIT(@MedicionId,',')
		SELECT CAST(value As VARCHAR(MAX)) FROM STRING_SPLIT_CUSTOM(@MedicionId,',');

		--Recorremos datos de tabla temporal.
		DECLARE @CountVxM INT = 1;
		DECLARE @MaxCountVxM INT = (SELECT COUNT(*) FROM #TablaTemporalVxM);

		WHILE @CountVxM <= @MaxCountVxM --Numero de ciclos Maximos
		BEGIN
			--Capturaramos valor de @@CalificacionIPSItem.
			SET @ItemValueVxM = (SELECT campo FROM #TablaTemporalVxM WHERE Id = @CountVxM)
	
			--Insertamos. VariableXMedicion
			INSERT INTO dbo.VariableXMedicion(VariableId, MedicionId, EsGlosa, EsVisible, EsCalificable, Hallazgos, Activo, CreatedBy, CreationDate, ModifyBy, ModificationDate, EnableDC, EnableNC, EnableND, CalificacionXDefecto, SubGrupoId, Encuesta, Orden,
						TipoCampo,Promedio,ValidarEntreRangos,Desde,Hasta,Condicionada,ValorConstante, Lista, status, Calculadora, TipoCalculadora)
			VALUES(@VariableId, @ItemValueVxM, @EsGlosa, @EsVisible, @EsCalificable, @Hallazgos, @Activo, @CreatedBy, GETDATE(), @ModifyBy, GETDATE(), @EnableDC, @EnableNC, @EnableND, @CalificacionXDefecto, @SubGrupoId, @Encuesta, @VxM_Orden,
					@TipoCampo,@Promedio,@ValidarEntreRangos,@Desde,@Hasta,@Condicionada,@ValorConstante, @Lista, 1, @Calculadora, @TipoCalculadora)
			-- // --


			-- Insertar variables que no existen
			INSERT INTO VariableCondicional
					SELECT 
								CAST(@ItemValueVxM AS int),
								CAST(@VariableId AS INT),
								vcinput.Id,
								GETDATE(),
								@ModifyBy,
								GETDATE(),
								@ModifyBy,
								1
					FROM @VariablesCondicional vcinput
			-- // --


			--Insertamos en VariablesXItems
			IF(@CalificacionIPSItem <> '')
			BEGIN
				--Verificamos si debemos eliminar tabla temporal.
				BEGIN TRY
				DROP TABLE #TablaTemporalVxI
				END TRY
				BEGIN CATCH  END CATCH

				--Creamos Tabla Temporal.
				CREATE TABLE #TablaTemporalVxI (Id INT IDENTITY(1, 1) PRIMARY KEY, campo VARCHAR(128));
				INSERT INTO #TablaTemporalVxI
				--SELECT value FROM STRING_SPLIT(@CalificacionIPSItem,',')
				SELECT CAST(value As VARCHAR(MAX)) FROM STRING_SPLIT_CUSTOM(@CalificacionIPSItem,',');

				--Recorremos datos de tabla temporal.
				DECLARE @CountVxI INT = 1;
				DECLARE @MaxCountVxI INT = (SELECT COUNT(*) FROM #TablaTemporalVxI);

				WHILE @CountVxI <= @MaxCountVxI --Numero de ciclos Maximos
				BEGIN
					--Capturaramos valor de @@CalificacionIPSItem.
					SET @ItemValueVxI = (SELECT campo FROM #TablaTemporalVxI WHERE Id = @CountVxI)
	
					--Insertamos.
					INSERT INTO VariablesXItems(VariablesId, ItemId, CreatedBy, CreatedDate, ModifyBy, ModifyDate, Status, IdMedicion)
					VALUES(@VariableId, @ItemValueVxI, @CreatedBy, GETDATE(), @ModifyBy, GETDATE(), 1, @ItemValueVxM)

					--Sumamos al contador del ciclo.
					SET @CountVxI = @CountVxI + 1
				END -- END WHILE Tabla temporal.		
			END
			-- // --


			--Sumamos al contador del ciclo.
			SET @CountVxM = @CountVxM + 1
		END -- END WHILE Tabla temporal.		

		-- // --
			
			
		
		--Actualizamos campos Orden.
		-- CODE
		-- // --

		-- Insertamos Log de operacion. 
		EXEC SP_Insertar_Process_Log 13, @CreatedBy, 'OK', 'Gestor Variables: Nueva'; 
		-- //

		COMMIT TRANSACTION [Tran1]

		--SELECT * FROM  @VariablesCondicional

	END TRY

	BEGIN CATCH

		ROLLBACK TRANSACTION [Tran1]
		--PRINT('FAIL');
		--SELECT ERROR_NUMBER() AS ErrorNumber, ERROR_STATE() AS ErrorState, ERROR_SEVERITY() AS ErrorSeverity, ERROR_PROCEDURE() AS ErrorProcedure, ERROR_LINE() AS ErrorLine, ERROR_MESSAGE() AS ErrorMessage;
		INSERT INTO TBL_TX_LOG(Date, Thread, Level, Logger, Message, Exception)
		VALUES(GETDATE(), '1', 'ERROR CATCH', 'Fallo EN SP CrearVariables', ERROR_MESSAGE(), CONCAT('ErrorNumber: ',ERROR_NUMBER(),' - ', 'ERROR_STATE: ',ERROR_STATE(),' - ', 'ERROR_SEVERITY: ',ERROR_SEVERITY(),' - ', 'ERROR_PROCEDURE: ',ERROR_PROCEDURE(),' - ', 'ERROR_LINE: ',ERROR_LINE(),' - ', 'ERROR_MESSAGE: ',ERROR_MESSAGE()) );
	END CATCH 

END --END PROCEDURE
GO
/****** Object:  StoredProcedure [dbo].[EditarVariables]    Script Date: 19/09/2022 12:45:31 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-01-11
-- Description:	Para Editar una variable con todos sus datos vinculados.
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[EditarVariables]
(@Variable VARCHAR(MAX), @Orden INT, @idCobertura INT, @nombre VARCHAR(100), @descripcion VARCHAR(500), @idTipoVariable VARCHAR(500), @longitud INT, @decimales INT, @formato VARCHAR(300), @tablaReferencial VARCHAR(128), @CreatedBy VARCHAR(MAX), @ModifyBy VARCHAR(50), @TipoVariableItem INT, 
 @EstructuraVariable INT, @Alerta BIT, @AlertaDescripcion VARCHAR(MAX), @MedicionId VARCHAR(MAX), @EsVisible BIT, @EsCalificable BIT, @Hallazgos BIT, @EnableDC BIT, @EnableNC BIT, @EnableND BIT, @CalificacionXDefecto INT, @SubGrupoId INT, @Encuesta BIT, @CalificacionIPSItem VARCHAR(MAX),
	@TipoCampo int,
	@Promedio bit,
	@ValidarEntreRangos bit,
	@Desde nvarchar(255),
	@Hasta nvarchar(255),
	@Condicionada bit,
	@ValorConstante nvarchar(255),
	@Lista bit,
	@VariablesCondicional DT_LLave_Valor READONLY,
	@Calculadora BIT,
	@TipoCalculadora INT)
AS
BEGIN
SET NOCOUNT ON

----Declaramos variables con valor default.
--DECLARE @Activa BIT = 1;
--DECLARE @idVariable INT = 1;
DECLARE @nemonico VARCHAR(50) = @nombre;
--DECLARE @longitud INT = 10;
--DECLARE @decimales INT = 1;
--DECLARE @formato VARCHAR(300) = '' ; -- Format TEST
--DECLARE @campoReferencial VARCHAR(128) = '';
--DECLARE @MotivoVariable NVARCHAR(MAX) = '';
--DECLARE @Bot BIT = 1;
DECLARE @EsGlosa BIT = 1;
DECLARE @Activo BIT = 1;
DECLARE @VxM_Orden INT = @Orden;

BEGIN TRANSACTION [Tran1]

	BEGIN TRY

		
		IF (@TipoCampo = 86) -- Numerico
			BEGIN
				SET @idTipoVariable	 = 'int'
			END

		ELSE IF (@TipoCampo = 87) -- Alfanumerico
			BEGIN
				SET @idTipoVariable	 = 'varchar'
			END

		ELSE IF (@TipoCampo = 88) -- Fecha
			BEGIN
				SET @idTipoVariable	 = 'datetime'
			END
		ELSE IF (@TipoCampo = 89) -- Decimal
			BEGIN
				SET @idTipoVariable	 = 'numeric'
			END
		ELSE 
			BEGIN
				SET @idTipoVariable	 = 'varchar'
			END

		--Declaramos variables		
		DECLARE @ItemValueVxM INT;
		DECLARE @ItemValueVxI INT;
		DECLARE @Valid INT;

		IF(@Variable <> '') --IF(@Variable IS NOT NULL)
		BEGIN
		
			--Actualizamos en Variables.
			--(Activa, idVariable, campoReferencial, , MotivoVariable, Bot)			
			UPDATE Variables SET Orden = @Orden, idCobertura = @idCobertura, nombre = @nombre, nemonico = @nemonico, descripcion = @descripcion, idTipoVariable = @idTipoVariable, longitud = @longitud, decimales = @decimales, formato = @formato, 
			tablaReferencial = @tablaReferencial, ModifyBy = @ModifyBy, ModifyDate = GETDATE(), TipoVariableItem = @TipoVariableItem, EstructuraVariable = @EstructuraVariable, Alerta = @Alerta, AlertaDescripcion = @AlertaDescripcion
			
			WHERE Id = @Variable;

			-- // --


			--Actualizamos en VariableXMedicion 

			--Verificamos si debemos eliminar tabla temporal.
			BEGIN TRY
			DROP TABLE #TablaTemporalVxM
			END TRY
			BEGIN CATCH  END CATCH

			--Creamos Tabla Temporal.
			CREATE TABLE #TablaTemporalVxM (Id INT IDENTITY(1, 1) PRIMARY KEY, campo VARCHAR(128));
			INSERT INTO #TablaTemporalVxM
			--SELECT value FROM STRING_SPLIT(@MedicionId,',')
			SELECT CAST(value As VARCHAR(MAX)) FROM STRING_SPLIT_CUSTOM(@MedicionId,',');

			--Recorremos datos de tabla temporal.
			DECLARE @CountVxM INT = 1;
			DECLARE @MaxCountVxM INT = (SELECT COUNT(*) FROM #TablaTemporalVxM);

			WHILE @CountVxM <= @MaxCountVxM --Numero de ciclos Maximos
			BEGIN
				--Capturaramos valor de @@CalificacionIPSItem.
				SET @ItemValueVxM = (SELECT campo FROM #TablaTemporalVxM WHERE Id = @CountVxM)
	
				--Validamos si el registro ya existe. En ese caso Actualizamos. De lo contrario Insertamos.
				SET @Valid = (SELECT VariableId FROM VariableXMedicion WHERE VariableId = @Variable AND MedicionId = @ItemValueVxM)

				--Inactivamos todos los registros.
				UPDATE VariableXMedicion SET Status = 1
				WHERE VariableId = @Variable AND MedicionId = @ItemValueVxM;

				IF(@Valid IS NOT NULL)
				BEGIN
					--Actualizamos.
					UPDATE VariableXMedicion SET VariableId = @Variable, MedicionId = @ItemValueVxM, EsVisible = @EsVisible, EsCalificable = @EsCalificable, Hallazgos = @Hallazgos, ModifyBy = @ModifyBy, ModificationDate = GETDATE(), EnableDC = @EnableDC, EnableNC = @EnableNC, EnableND = @EnableND, 
					CalificacionXDefecto = @CalificacionXDefecto, SubGrupoId = @SubGrupoId, Encuesta = @Encuesta, Orden = @Orden, Status = 1,
					TipoCampo = @TipoCampo,Promedio = @Promedio, ValidarEntreRangos = @ValidarEntreRangos,Desde = @Desde,Hasta = @Hasta,Condicionada = @Condicionada, ValorConstante = @ValorConstante, Lista = @Lista,
					Calculadora = @Calculadora,
					TipoCalculadora = @TipoCalculadora
					--VALUES(@VariableId, @MedicionId, @EsGlosa, @EsVisible, @EsCalificable, @Activo, @CreatedBy, GETDATE(), @ModifyBy, GETDATE(), @EnableDC, @EnableNC, @EnableND, @CalificacionXDefecto, @SubGrupoId, @Encuesta, @VxM_Orden)
					WHERE VariableId = @Variable AND MedicionId = @ItemValueVxM;
				END --END IF
				ELSE
				BEGIN
					--Insertamos.
					INSERT INTO dbo.VariableXMedicion(VariableId, MedicionId, EsGlosa, EsVisible, EsCalificable, Hallazgos, Activo, CreatedBy, CreationDate, ModifyBy, ModificationDate, EnableDC, EnableNC, EnableND, CalificacionXDefecto, SubGrupoId, Encuesta, Orden, TipoCampo,Promedio,ValidarEntreRangos,Desde,Hasta,Condicionada,ValorConstante, Lista, Calculadora, TipoCalculadora)
					VALUES(@Variable, @ItemValueVxM, @EsGlosa, @EsVisible, @EsCalificable, @Hallazgos, @Activo, @CreatedBy, GETDATE(), @ModifyBy, GETDATE(), @EnableDC, @EnableNC, @EnableND, @CalificacionXDefecto, @SubGrupoId, @Encuesta, @VxM_Orden, @TipoCampo,@Promedio,@ValidarEntreRangos,@Desde,@Hasta,@Condicionada,@ValorConstante, @Lista, @Calculadora, @TipoCalculadora);				
				END --END ELSE IF
				-- // --


				--Actualiza variables condicionadas
						UPDATE vc
							SET 
							vc.[Enable] = 0,
							vc.ModifyBy = @ModifyBy,
							vc.ModifyDate = GETDATE()
							FROM VariableCondicional vc -- Tabla a actualizar
										WHERE vc.VariablePadreId = @Variable
										AND vc.MedicionId = CAST(@ItemValueVxM AS int)

						--Deshabilita variables
						UPDATE vc
							SET 
							vc.[Enable] = 1,
							vc.ModifyBy = @ModifyBy,
							vc.ModifyDate = GETDATE()
							FROM VariableCondicional vc -- Tabla a actualizar						
								INNER JOIN @VariablesCondicional vcinput -- Tabla con la que valida info
									ON vc.VariableHijaId = vcinput.Id
									AND vc.VariablePadreId = CAST(@Variable AS INT)
									AND vc.MedicionId = CAST(@ItemValueVxM AS int)
						

						-- Insertar variables que no existen
						INSERT INTO VariableCondicional
							SELECT 
								CAST(@ItemValueVxM AS int),
								CAST(@Variable AS INT),
								vcinput.Id,
								GETDATE(),
								@CreatedBy,
								GETDATE(),
								@ModifyBy,
								1
							FROM @VariablesCondicional vcinput
						
								LEFT JOIN VariableCondicional vc
									ON vcinput.Id = vc.VariableHijaId 
										AND vc.VariablePadreId = CAST(@Variable AS INT)
										AND vc.MedicionId = CAST(@ItemValueVxM AS int)

							WHERE vc.Id IS NULL
				-- // --


				--Actualizamos en VariablesXItems
				IF(@CalificacionIPSItem != '')
				BEGIN

					--Inactivamos por defecto todos los Items calificables asociados a la variable actual.
					UPDATE VariablesXItems SET Status = 0 WHERE VariablesId = @Variable AND IdMedicion = @ItemValueVxM

					--Verificamos si debemos eliminar tabla temporal.
					BEGIN TRY
					DROP TABLE #TablaTemporalVxIT
					END TRY
					BEGIN CATCH  END CATCH

					--Creamos Tabla Temporal.
					CREATE TABLE #TablaTemporalVxIT (Id INT IDENTITY(1, 1) PRIMARY KEY, campo VARCHAR(128));
					INSERT INTO #TablaTemporalVxIT
					--SELECT value FROM STRING_SPLIT(@CalificacionIPSItem,',')
					SELECT CAST(value As VARCHAR(MAX)) FROM STRING_SPLIT_CUSTOM(@CalificacionIPSItem,',');

					--Recorremos datos de tabla temporal.
					DECLARE @CountVxIT INT = 1;
					DECLARE @MaxCountVxIT INT = (SELECT COUNT(*) FROM #TablaTemporalVxIT);

					IF(@MaxCountVxIT > 0)
					BEGIN
						WHILE @CountVxIT <= @MaxCountVxIT --Numero de ciclos Maximos
						BEGIN

							--Capturaramos valor de @@CalificacionIPSItem.
							SET @ItemValueVxI = (SELECT campo FROM #TablaTemporalVxIT WHERE Id = @CountVxIT)				

							-- Consultamos si el item ya existe.
							DECLARE @CreaItem INT = 0;
							SET @CreaItem = (SELECT COUNT(VAI.ItemId) FROM Variables VA 
								INNER JOIN VariableXMedicion VAME ON (VA.Id = VAME.VariableId)
								LEFT JOIN VariablesXItems VAI ON (VA.Id = VAI.VariablesId)
								WHERE VA.Id = @Variable AND VAI.ItemId = @ItemValueVxI AND VAI.IdMedicion = @ItemValueVxM); --19, 20, 21 | @ItemValueVxI | @Variable

							-- Validamos si debemos actualizar o crear el item. 
							IF( @CreaItem = 0) 
							BEGIN
								-- Insertamos.															
								INSERT INTO VariablesXItems(VariablesId, ItemId, CreatedBy, CreatedDate, ModifyBy, ModifyDate, Status, IdMedicion)
								VALUES(@Variable, @ItemValueVxI, @CreatedBy, GETDATE(), @ModifyBy, GETDATE(), 1, @ItemValueVxM)
							END
							ELSE
							BEGIN
								-- Actualizamos.
								UPDATE VariablesXItems SET Status = 1, ModifyBy = @ModifyBy, ModifyDate = GETDATE()
								WHERE ItemId = @ItemValueVxI AND VariablesId = @Variable AND IdMedicion = @ItemValueVxM;
							END

							--Sumamos al contador del ciclo.
							SET @CountVxIT = @CountVxIT + 1
						END -- END WHILE Tabla temporal.
					END --END IF validacion de ejecucion.
					-- // --
			
					--Insertamos en Reglas de Variables, en ReglasVariable.
					--UPDATE ReglasVariable SET IdRegla = @IdRegla, IdVariable = @Variable, Concepto = @Concepto, ModifyDate = GETDATE(), ModifyBy = @ModifyBy
					--WHERE IdVariable = @Variable;
				END -- END IF: IF(@CalificacionIPSItem != '')
				-- // --

				--Sumamos al contador del ciclo.
				SET @CountVxM = @CountVxM + 1
			END -- END WHILE Tabla temporal.			

			-- // --
		


		-- // --
			
		END --END IF(@Variable <> '')
		--ELSE	
		--BEGIN
		--PRINT('SIN VARIABLE');
		--END --END ELSE IF(@Variable <> NULL)

		-- Insertamos Log de operacion. 
		EXEC SP_Insertar_Process_Log 14, @ModifyBy, 'OK', 'Gestor Variables: Editar';
		-- // 

		COMMIT TRANSACTION [Tran1]

		SELECT * FROM  @VariablesCondicional
	END TRY

	BEGIN CATCH

		ROLLBACK TRANSACTION [Tran1]
		--PRINT('FAIL');
		--SELECT ERROR_NUMBER() AS ErrorNumber, ERROR_STATE() AS ErrorState, ERROR_SEVERITY() AS ErrorSeverity, ERROR_PROCEDURE() AS ErrorProcedure, ERROR_LINE() AS ErrorLine, ERROR_MESSAGE() AS ErrorMessage;
		INSERT INTO TBL_TX_LOG(Date, Thread, Level, Logger, Message, Exception)
		VALUES(GETDATE(), '1', 'ERROR CATCH', 'Fallo EN SP EditarVariables', ERROR_MESSAGE(), CONCAT('ErrorNumber: ',ERROR_NUMBER(),' - ', 'ERROR_STATE: ',ERROR_STATE(),' - ', 'ERROR_SEVERITY: ',ERROR_SEVERITY(),' - ', 'ERROR_PROCEDURE: ',ERROR_PROCEDURE(),' - ', 'ERROR_LINE: ',ERROR_LINE(),' - ', 'ERROR_MESSAGE: ',ERROR_MESSAGE()) );
	END CATCH 

END --END PROCEDURE
GO
/****** Object:  StoredProcedure [dbo].[GetVariablesFiltrado]    Script Date: 19/09/2022 12:45:31 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[GetVariablesFiltrado](@PageNumber INT, @MaxRows INT, @Id VARCHAR(MAX), @Variable VARCHAR(MAX), @Activa VARCHAR(MAX), @Orden VARCHAR(MAX), @idVariable VARCHAR(MAX), @idCobertura VARCHAR(MAX), @nombre VARCHAR(MAX), @nemonico VARCHAR(MAX), @descripcion VARCHAR(MAX), @idTipoVariable VARCHAR(MAX), @longitud VARCHAR(MAX), @decimales VARCHAR(MAX), @formato VARCHAR(MAX), @tablaReferencial VARCHAR(MAX), @campoReferencial VARCHAR(MAX), @CreatedBy VARCHAR(MAX), @CreatedDate VARCHAR(MAX), @ModifyBy VARCHAR(MAX), @ModifyDate VARCHAR(MAX), @MotivoVariable VARCHAR(MAX), @Bot VARCHAR(MAX), @TipoVariableItem VARCHAR(MAX), @EstructuraVariable VARCHAR(MAX), @MedicionId VARCHAR(MAX), @EsGlosa VARCHAR(MAX), @EsVisible VARCHAR(MAX), @EsCalificable VARCHAR(MAX), @Activo VARCHAR(MAX), @EnableDC VARCHAR(MAX), @EnableNC VARCHAR(MAX), @EnableND VARCHAR(MAX), @CalificacionXDefecto VARCHAR(MAX), @SubGrupoId VARCHAR(MAX), @Encuesta VARCHAR(MAX), @VxM_Orden VARCHAR(MAX), @Alerta VARCHAR(MAX), @AlertaDescripcion VARCHAR(MAX), @calificacionIPSItem VARCHAR(MAX), @IdRegla VARCHAR(MAX), @Concepto VARCHAR(MAX)) AS BEGIN
SET NOCOUNT ON DECLARE @Query VARCHAR(MAX);
SET @Query='SELECT '' '' As QueryNoRegistrosTotales, NEWID() As Idk, CAST(VA.Id As NVARCHAR(MAX)) As Id, CAST(VA.Id AS NVARCHAR(12)) AS Variable, VA.Activa As Activa, CAST(VA.Orden As NVARCHAR(MAX)) As Orden, CAST(VA.idVariable As NVARCHAR(MAX)) As idVariable, CAST(VA.idCobertura As NVARCHAR(MAX)) As idCobertura, CAST(VA.nombre As NVARCHAR(MAX)) As nombre, CAST(VA.nemonico As NVARCHAR(MAX)) As nemonico, CAST(VA.descripcion As NVARCHAR(MAX)) As descripcion, CAST(VA.idTipoVariable As NVARCHAR(MAX)) As idTipoVariable, CAST(VA.longitud As NVARCHAR(MAX)) As longitud, CAST(VA.decimales As NVARCHAR(MAX)) As decimales, CAST(VA.formato As NVARCHAR(MAX)) As formato, CAST(VA.tablaReferencial As NVARCHAR(MAX)) As tablaReferencial, CAST(VA.campoReferencial As NVARCHAR(MAX)) As campoReferencial, CAST(VA.CreatedBy As NVARCHAR(MAX)) As CreatedBy, CAST(VA.CreatedDate As NVARCHAR(MAX)) As CreatedDate, CAST(VA.ModifyBy As NVARCHAR(MAX)) As ModifyBy, CAST(VA.ModifyDate As NVARCHAR(MAX)) As ModifyDate, CAST(VA.MotivoVariable As NVARCHAR(MAX)) As MotivoVariable, VA.Bot As Bot,VA.TipoVariableItem TipoVariableItem, VA.EstructuraVariable AS EstructuraVariable, CAST(VAXM.VariableId As NVARCHAR(MAX)) As VariableId, CAST(VAXM.MedicionId As NVARCHAR(MAX)) As MedicionId, VAXM.EsGlosa As EsGlosa, VAXM.EsVisible As EsVisible, VAXM.EsCalificable As EsCalificable, VAXM.Hallazgos As Hallazgos, VAXM.Activo As Activo, VAXM.EnableDC As EnableDC, VAXM.EnableNC As EnableNC, VAXM.EnableND As EnableND, VAXM.CalificacionXDefecto As CalificacionXDefecto, VAXM.SubGrupoId As SubGrupoId, CAST(IT.ItemName As NVARCHAR(MAX)) As SubGrupoNombre, VAXM.Encuesta As Encuesta, CAST(VAXM.Orden As NVARCHAR(MAX)) As VxM_Orden, VA.Alerta As Alerta, CAST(VA.AlertaDescripcion As NVARCHAR(MAX)) As AlertaDescripcion, VAI.ItemId As calificacionIPSItem, CAST(IT2.ItemName As NVARCHAR(MAX)) As calificacionIPSItemNombre, CAST(RVA.IdRegla As NVARCHAR(MAX)) As IdRegla, CAST(IT3.ItemName As NVARCHAR(MAX)) As NombreRegla, CAST(RVA.Concepto As NVARCHAR(MAX)) As Concepto, ITTipo.ItemName as TipoVariableDesc, VAXM.TipoCampo, VAXM.Promedio, VAXM.ValidarEntreRangos, VAXM.Desde, VAXM.Hasta, VAXM.Condicionada, VAXM.ValorConstante, VAXM.Lista, VAXM.Calculadora, VAXM.TipoCalculadora FROM Variables VA INNER JOIN VariableXMedicion VAXM ON (VA.Id=VAXM.VariableId) INNER JOIN Item IT ON (VAXM.SubGrupoId=IT.Id) INNER JOIN Item ITTipo ON (VA.TipoVariableItem=ITTipo.Id) LEFT JOIN VariablesXItems VAI ON (VA.Id=VAI.VariablesId AND VAI.Status=1 AND VAI.IdMedicion=VAXM.MedicionId) LEFT JOIN ReglasVariable RVA ON (VA.Id=RVA.IdVariable) LEFT JOIN Item IT2 ON (VAI.ItemId=IT2.Id) LEFT JOIN Item IT3 ON (RVA.IdRegla=IT3.Id) ' DECLARE @Paginate INT=@PageNumber * @MaxRows; DECLARE @Where VARCHAR(MAX)=''; IF(@Id <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.Id IN (' + CAST(@Id AS NVARCHAR(MAX)) + ')' END ELSE BEGIN
SET @Where=@Where + ' AND VA.Id IN (' + CAST(@Id AS NVARCHAR(MAX)) + ')' END END IF(@Variable <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + '(''VAR'' + CAST(VA.Id AS NVARCHAR(MAX)) LIKE ''%' + CAST(@Variable AS NVARCHAR(MAX)) + '%''' + ' OR CAST(VA.nombre AS NVARCHAR(MAX)) LIKE ''%' + CAST(@Variable AS NVARCHAR(MAX)) + '%'')' END ELSE BEGIN
SET @Where=@Where + ' OR ' + '''VAR'' + CAST(VA.Id AS NVARCHAR(MAX)) LIKE ''%' + CAST(@Variable AS NVARCHAR(MAX)) + '%''' END END IF(@Activa='true') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.Activa=' + CAST(1 AS NVARCHAR(MAX)) END ELSE BEGIN
SET @Where=@Where + ' AND VA.Activa=' + CAST(1 AS NVARCHAR(MAX)) END END ELSE IF(@Activa='false') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.Activa=' + CAST(0 AS NVARCHAR(MAX)) END ELSE BEGIN
SET @Where=@Where + ' AND VA.Activa=' + CAST(0 AS NVARCHAR(MAX)) END END IF(@Orden <> ''
                                                                            AND ISNUMERIC(@Orden) <> 0) BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.Orden IN (' + CAST(@Orden AS NVARCHAR(MAX)) + ')' END ELSE BEGIN
SET @Where=@Where + ' OR VA.Orden IN (' + CAST(@Orden AS NVARCHAR(MAX)) + ')' END END IF(@idVariable <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.idVariable IN (' + CAST(@idVariable AS NVARCHAR(MAX)) + ')' END ELSE BEGIN
SET @Where=@Where + ' AND VA.idVariable IN (' + CAST(@idVariable AS NVARCHAR(MAX)) + ')' END END IF(@idCobertura <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.idCobertura IN (' + CAST(@idCobertura AS NVARCHAR(MAX)) + ')' END ELSE BEGIN
SET @Where=@Where + ' AND VA.idCobertura IN (' + CAST(@idCobertura AS NVARCHAR(MAX)) + ')' END END IF(@nemonico <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.nemonico LIKE ''%' + CAST(@nemonico AS NVARCHAR(MAX)) + '%''' END ELSE BEGIN
SET @Where=@Where + ' AND VA.nemonico LIKE ''%' + CAST(@nemonico AS NVARCHAR(MAX)) + '%''' END END IF(@idTipoVariable <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.idTipoVariable LIKE ''%' + CAST(@idTipoVariable AS NVARCHAR(MAX)) + '%''' END ELSE BEGIN
SET @Where=@Where + ' AND VA.idTipoVariable LIKE ''%' + CAST(@idTipoVariable AS NVARCHAR(MAX)) + '%''' END END IF(@longitud <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.longitud IN (' + CAST(@longitud AS NVARCHAR(MAX)) + ')' END ELSE BEGIN
SET @Where=@Where + ' AND VA.longitud IN (' + CAST(@longitud AS NVARCHAR(MAX)) + ')' END END IF(@decimales <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.decimales IN (' + CAST(@decimales AS NVARCHAR(MAX)) + ')' END ELSE BEGIN
SET @Where=@Where + ' AND VA.decimales IN (' + CAST(@decimales AS NVARCHAR(MAX)) + ')' END END IF(@formato <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.formato LIKE ''%' + CAST(@formato AS NVARCHAR(MAX)) + '%''' END ELSE BEGIN
SET @Where=@Where + ' AND VA.formato LIKE ''%' + CAST(@formato AS NVARCHAR(MAX)) + '%''' END END IF(@tablaReferencial <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.tablaReferencial LIKE ''%' + CAST(@tablaReferencial AS NVARCHAR(MAX)) + '%''' END ELSE BEGIN
SET @Where=@Where + ' AND VA.tablaReferencial LIKE ''%' + CAST(@tablaReferencial AS NVARCHAR(MAX)) + '%''' END END IF(@campoReferencial <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.campoReferencial LIKE ''%' + CAST(@campoReferencial AS NVARCHAR(MAX)) + '%''' END ELSE BEGIN
SET @Where=@Where + ' AND VA.campoReferencial LIKE ''%' + CAST(@campoReferencial AS NVARCHAR(MAX)) + '%''' END END IF(@CreatedBy <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.CreatedBy IN (' + CAST(@CreatedBy AS NVARCHAR(MAX)) + ')' END ELSE BEGIN
SET @Where=@Where + ' AND VA.CreatedBy IN (' + CAST(@CreatedBy AS NVARCHAR(MAX)) + ')' END END IF(@CreatedDate <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.CreatedDate LIKE ''%' + CAST(@CreatedDate AS NVARCHAR(MAX)) + '%''' END ELSE BEGIN
SET @Where=@Where + ' AND VA.CreatedDate LIKE ''%' + CAST(@CreatedDate AS NVARCHAR(MAX)) + '%''' END END IF(@ModifyBy <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.ModifyBy IN (' + CAST(@ModifyBy AS NVARCHAR(MAX)) + ')' END ELSE BEGIN
SET @Where=@Where + ' AND VA.ModifyBy IN (' + CAST(@ModifyBy AS NVARCHAR(MAX)) + ')' END END IF(@ModifyDate <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.ModifyDate LIKE ''%' + CAST(@ModifyDate AS NVARCHAR(MAX)) + '%''' END ELSE BEGIN
SET @Where=@Where + ' AND VA.ModifyDate LIKE ''%' + CAST(@ModifyDate AS NVARCHAR(MAX)) + '%''' END END IF(@MotivoVariable <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.MotivoVariable LIKE ''%' + CAST(@MotivoVariable AS NVARCHAR(MAX)) + '%''' END ELSE BEGIN
SET @Where=@Where + ' AND VA.MotivoVariable LIKE ''%' + CAST(@MotivoVariable AS NVARCHAR(MAX)) + '%''' END END IF(@Bot='true') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.Bot=' + CAST(1 AS NVARCHAR(MAX)) END ELSE BEGIN
SET @Where=@Where + ' AND VA.Bot=' + CAST(1 AS NVARCHAR(MAX)) END END ELSE IF(@Bot='false') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.Bot=' + CAST(0 AS NVARCHAR(MAX)) END ELSE BEGIN
SET @Where=@Where + ' AND VA.Bot=' + CAST(0 AS NVARCHAR(MAX)) END END IF(@TipoVariableItem <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.TipoVariableItem IN (' + CAST(@TipoVariableItem AS NVARCHAR(MAX)) + ')' END ELSE BEGIN
SET @Where=@Where + ' AND VA.TipoVariableItem IN (' + CAST(@TipoVariableItem AS NVARCHAR(MAX)) + ')' END END IF(@EstructuraVariable <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.EstructuraVariable IN (' + CAST(@EstructuraVariable AS NVARCHAR(MAX)) + ')' END ELSE BEGIN
SET @Where=@Where + ' AND VA.EstructuraVariable IN (' + CAST(@EstructuraVariable AS NVARCHAR(MAX)) + ')' END END IF(@Id <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VAXM.VariableId IN (' + CAST(@Id AS NVARCHAR(MAX)) + ')' END ELSE BEGIN
SET @Where=@Where + ' AND VAXM.VariableId IN (' + CAST(@Id AS NVARCHAR(MAX)) + ')' END END IF(@MedicionId <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VAXM.MedicionId IN (' + CAST(@MedicionId AS NVARCHAR(MAX)) + ')' END ELSE BEGIN
SET @Where = @Where + ' AND VAXM.MedicionId IN (' + CAST(@MedicionId AS NVARCHAR(MAX)) + ')' END END IF(@EsGlosa='true') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VAXM.EsGlosa=' + CAST(1 AS NVARCHAR(MAX)) END ELSE BEGIN
SET @Where=@Where + ' AND VAXM.EsGlosa=' + CAST(1 AS NVARCHAR(MAX)) END END ELSE IF(@EsGlosa='false') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VAXM.EsGlosa=' + CAST(0 AS NVARCHAR(MAX)) END ELSE BEGIN
SET @Where=@Where + ' AND VAXM.EsGlosa=' + CAST(0 AS NVARCHAR(MAX)) END END IF(@EsVisible='true') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VAXM.EsVisible=' + CAST(1 AS NVARCHAR(MAX)) END ELSE BEGIN
SET @Where=@Where + ' AND VAXM.EsVisible=' + CAST(1 AS NVARCHAR(MAX)) END END ELSE IF(@EsVisible='false') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VAXM.EsVisible=' + CAST(0 AS NVARCHAR(MAX)) END ELSE BEGIN
SET @Where=@Where + ' AND VAXM.EsVisible=' + CAST(0 AS NVARCHAR(MAX)) END END IF(@EsCalificable='true') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VAXM.EsCalificable=' + CAST(1 AS NVARCHAR(MAX)) END ELSE BEGIN
SET @Where=@Where + ' AND VAXM.EsCalificable=' + CAST(1 AS NVARCHAR(MAX)) END END ELSE IF(@EsCalificable='false') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VAXM.EsCalificable=' + CAST(0 AS NVARCHAR(MAX)) END ELSE BEGIN
SET @Where=@Where + ' AND VAXM.EsCalificable=' + CAST(0 AS NVARCHAR(MAX)) END END IF(@Activo='true') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VAXM.Activo=' + CAST(1 AS NVARCHAR(MAX)) END ELSE BEGIN
SET @Where=@Where + ' AND VAXM.Activo=' + CAST(1 AS NVARCHAR(MAX)) END END ELSE IF(@Activo='false') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VAXM.Activo=' + CAST(0 AS NVARCHAR(MAX)) END ELSE BEGIN
SET @Where=@Where + ' AND VAXM.Activo=' + CAST(0 AS NVARCHAR(MAX)) END END IF(@EnableDC='true') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VAXM.EnableDC=' + CAST(1 AS NVARCHAR(MAX)) END ELSE BEGIN
SET @Where=@Where + ' AND VAXM.EnableDC=' + CAST(1 AS NVARCHAR(MAX)) END END ELSE IF(@EnableDC='false') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VAXM.EnableDC=' + CAST(0 AS NVARCHAR(MAX)) END ELSE BEGIN
SET @Where=@Where + ' AND VAXM.EnableDC=' + CAST(0 AS NVARCHAR(MAX)) END END IF(@EnableNC='true') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VAXM.EnableNC=' + CAST(1 AS NVARCHAR(MAX)) END ELSE BEGIN
SET @Where=@Where + ' AND VAXM.EnableNC=' + CAST(1 AS NVARCHAR(MAX)) END END ELSE IF(@EnableNC='false') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VAXM.EnableNC=' + CAST(0 AS NVARCHAR(MAX)) END ELSE BEGIN
SET @Where=@Where + ' AND VAXM.EnableNC=' + CAST(0 AS NVARCHAR(MAX)) END END IF(@EnableND='true') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VAXM.EnableND=' + CAST(1 AS NVARCHAR(MAX)) END ELSE BEGIN
SET @Where=@Where + ' AND VAXM.EnableND=' + CAST(1 AS NVARCHAR(MAX)) END END ELSE IF(@EnableND='false') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VAXM.EnableND=' + CAST(0 AS NVARCHAR(MAX)) END ELSE BEGIN
SET @Where=@Where + ' AND VAXM.EnableND=' + CAST(0 AS NVARCHAR(MAX)) END END IF(@CalificacionXDefecto <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VAXM.CalificacionXDefecto IN (' + CAST(@CalificacionXDefecto AS NVARCHAR(MAX)) + ')' END ELSE BEGIN
SET @Where=@Where + ' AND VAXM.CalificacionXDefecto IN (' + CAST(@CalificacionXDefecto AS NVARCHAR(MAX)) + ')' END END IF(@SubGrupoId <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VAXM.SubGrupoId IN (' + CAST(@SubGrupoId AS NVARCHAR(MAX)) + ')' END ELSE BEGIN
SET @Where=@Where + ' AND VAXM.SubGrupoId IN (' + CAST(@SubGrupoId AS NVARCHAR(MAX)) + ')' END END IF(@Encuesta='true') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VAXM.Encuesta=' + CAST(1 AS NVARCHAR(MAX)) END ELSE BEGIN
SET @Where=@Where + ' AND VAXM.Encuesta=' + CAST(1 AS NVARCHAR(MAX)) END END ELSE IF(@Encuesta='false') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VAXM.Encuesta=' + CAST(0 AS NVARCHAR(MAX)) END ELSE BEGIN
SET @Where=@Where + ' AND VAXM.Encuesta=' + CAST(0 AS NVARCHAR(MAX)) END END IF(@Orden <> ''
                                                                                AND ISNUMERIC(@Orden) <> 0) BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VAXM.Orden IN (' + CAST(@Orden AS NVARCHAR(MAX)) + ')' END ELSE BEGIN
SET @Where=@Where + ' OR VAXM.Orden IN (' + CAST(@Orden AS NVARCHAR(MAX)) + ')' END END IF(@Alerta='true') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.Alerta=' + CAST(1 AS NVARCHAR(MAX)) END ELSE BEGIN
SET @Where=@Where + ' AND VA.Alerta=' + CAST(1 AS NVARCHAR(MAX)) END END ELSE IF(@Alerta='false') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.Alerta=' + CAST(0 AS NVARCHAR(MAX)) END ELSE BEGIN
SET @Where=@Where + ' AND VA.Alerta=' + CAST(0 AS NVARCHAR(MAX)) END END IF(@AlertaDescripcion <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VA.AlertaDescripcion LIKE ''%' + CAST(@AlertaDescripcion AS NVARCHAR(MAX)) + '%''' END ELSE BEGIN
SET @Where=@Where + ' OR VA.AlertaDescripcion LIKE ''%' + CAST(@AlertaDescripcion AS NVARCHAR(MAX)) + '%''' END END IF(@calificacionIPSItem <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'VAI.ItemId IN (' + CAST(@calificacionIPSItem AS NVARCHAR(MAX)) + ')' END ELSE BEGIN
SET @Where=@Where + ' AND VAI.ItemId IN (' + CAST(@calificacionIPSItem AS NVARCHAR(MAX)) + ')' END END IF(@IdRegla <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'RVA.IdRegla IN (' + CAST(@IdRegla AS NVARCHAR(MAX)) + ')' END ELSE BEGIN
SET @Where=@Where + ' AND RVA.IdRegla IN (' + CAST(@IdRegla AS NVARCHAR(MAX)) + ')' END END IF(@Concepto <> '') BEGIN IF(@Where='') BEGIN
SET @Where=@Where + 'RVA.Concepto LIKE ''%' + CAST(@Concepto AS NVARCHAR(MAX)) + '%''' END ELSE BEGIN
SET @Where=@Where + ' AND RVA.Concepto LIKE ''%' + CAST(@Concepto AS NVARCHAR(MAX)) + '%''' END END IF(@Where='') BEGIN
SET @Where=@Where + 'VA.Status=1 AND VAXM.Status=1 AND IT.Status=1 ' END ELSE BEGIN
SET @Where=@Where + ' AND VA.Status=1 AND VAXM.Status=1 AND IT.Status=1 ' END DECLARE @Paginado VARCHAR(MAX)='ORDER BY VAXM.Orden ASC OFFSET ' + CAST(@Paginate AS NVARCHAR(MAX)) + ' ROWS FETCH NEXT ' + CAST(@MaxRows AS NVARCHAR(MAX)) + ' ROWS ONLY;' IF(@Where <> '') BEGIN
SET @Where=' WHERE ' + @Where; END DECLARE @Total VARCHAR(MAX);
SET @Total=@Query + '' + @Where + ' ' + @Paginado DECLARE @Query2 NVARCHAR(MAX);
SET @Query2=N'SELECT NEWID() As Idk, COUNT(*) As NoRegistrosTotalesFiltrado FROM Variables VA INNER JOIN VariableXMedicion VAXM ON (VA.Id=VAXM.VariableId) INNER JOIN Item IT ON (VAXM.SubGrupoId=IT.Id) INNER JOIN Item ITTipo ON (VA.TipoVariableItem=ITTipo.Id) LEFT JOIN ReglasVariable RVA ON (VA.Id=RVA.IdVariable) LEFT JOIN Item IT3 ON (RVA.IdRegla=IT3.Id)' DECLARE @Total2 NVARCHAR(MAX);
SET @Total2=@Query2 + '' + REPLACE(@Where, '''', '''''')
SET @Total=REPLACE(@Total, ''' '' As QueryNoRegistrosTotales', '''' + CAST(@Total2 AS NVARCHAR(MAX)) + ''' As QueryNoRegistrosTotales'); EXEC(@Total); END
GO
/****** Object:  StoredProcedure [dbo].[SP_Cambiar_Estados_BolsasMedicion]    Script Date: 19/09/2022 12:45:31 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[SP_Cambiar_Estados_BolsasMedicion]

(@IdLider VARCHAR(MAX), @IdCobertura VARCHAR(MAX), @IdEstado VARCHAR(MAX)) AS BEGIN

SET NOCOUNT ON BEGIN TRANSACTION [Tran1] 


	BEGIN TRY 

		-- Set Estados
		DECLARE @EnCurso INT=28;
		DECLARE @Asignada INT=29;
		DECLARE @Finalizada INT=30;
		DECLARE @Creada INT=31;
		DECLARE @FechaActual Datetime = GETDATE();
		DECLARE @FechaLastYear Datetime=DATEADD(YEAR, -1, @FechaActual);
		DECLARE @FechaNextYear Datetime=DATEADD(YEAR,+1, @FechaActual);

		SET @FechaNextYear=DATEADD(DAY,+1, @FechaNextYear);


			-- Actualiza mediciones a en curso
			UPDATE ME
				SET Estado= @EnCurso
			FROM Medicion ME
					--INNER JOIN RegistrosAuditoria RA ON (ME.Id = RA.IdMedicion)
			WHERE (CAST(@FechaActual AS DATE) >= ME.FechaInicioAuditoria
				   AND CAST(@FechaActual AS DATE) <=ME.FechaFinAuditoria)
					-- AND (ME.Estado != 30)
					--AND (CAST(@FechaLastYear AS DATE) <= ME.FechaInicioAuditoria
				 --  AND CAST(@FechaNextYear AS DATE) >= ME.FechaFinAuditoria);

			

			UPDATE ME
			SET Estado=@Asignada
			FROM Medicion ME
			INNER JOIN RegistrosAuditoria RA ON (ME.Id=RA.IdMedicion)
			WHERE ((CAST(@FechaActual AS DATE) < ME.FechaInicioAuditoria
					AND CAST(@FechaActual AS DATE) < ME.FechaFinAuditoria))
			  AND (ME.Estado != 30)
			  --AND (CAST(@FechaLastYear AS DATE) <=ME.FechaInicioAuditoria
				 --  AND CAST(@FechaNextYear AS DATE) >=ME.FechaFinAuditoria);

			UPDATE ME
			SET Estado=@Finalizada
			FROM Medicion ME
			WHERE CAST(@FechaActual AS DATE) > ME.FechaFinAuditoria
			  --AND (ME.Estado != 30)
			  ----AND (CAST(@FechaLastYear AS DATE) <=ME.FechaInicioAuditoria
				 ----  AND CAST(@FechaNextYear AS DATE) >=ME.FechaFinAuditoria);

			COMMIT TRANSACTION [Tran1]

		END TRY 

		BEGIN CATCH

			ROLLBACK TRANSACTION [Tran1]
			INSERT INTO TBL_TX_LOG(Date, THREAD, LEVEL, Logger, Message,
								   EXCEPTION)
			VALUES(GETDATE(), '1', 'ERROR CATCH', 'Fallo EN SP SP_Cambiar_Estados_BolsasMedicion', ERROR_MESSAGE(), CONCAT('ErrorNumber: ',ERROR_NUMBER(),' - ', 'ERROR_STATE: ',ERROR_STATE(),' - ', 'ERROR_SEVERITY: ',ERROR_SEVERITY(),' - ', 'ERROR_PROCEDURE: ',ERROR_PROCEDURE(),' - ', 'ERROR_LINE: ',ERROR_LINE(),' - ', 'ERROR_MESSAGE: ',ERROR_MESSAGE()));

	END CATCH 
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Cantidad_Campos_Calificacion_Masiva]    Script Date: 19/09/2022 12:45:31 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar - IT SENSE
-- Create date: 2022-09-01
-- Description:	Consulta la cantidad de campos que debe tener el archivo de cargue
-- =============================================
CREATE   PROCEDURE  [dbo].[SP_Consulta_Cantidad_Campos_Calificacion_Masiva]

AS
BEGIN

	SELECT 
	5 AS Id,
	'Total campos cargue' AS Valor

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Cantidad_ErrorRegistroAuditoria]    Script Date: 19/09/2022 12:45:31 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar - IT SENSE
-- Create date: 2022-16-09
-- Description: Consulta Ccantidad  RegistroAuditoriaDetalleError
-- =============================================
CREATE OR ALTER  PROCEDURE [dbo].[SP_Consulta_Cantidad_ErrorRegistroAuditoria]	
	@RegistroAuditoriaId INT

AS 
BEGIN
			
			DECLARE @Total INT =
					(SELECT COUNT(*)
					  FROM RegistrosAuditoriaDetalle rd WITH (NOLOCK)
							INNER JOIN  RegistroAuditoriaDetalleError re ON rd.Id = RegistrosAuditoriaDetalleId
							LEFT JOIN Regla rg ON rg.idRegla = re.IdRegla
					  WHERE rd.RegistrosAuditoriaId = @RegistroAuditoriaId 
							AND Enable = 1 AND NoCorregible = 0)

		RETURN @Total


END

GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Detalle_Registros_Auditoria]    Script Date: 19/09/2022 12:45:31 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
-- SP_Consulta_Detalle_Registros_Auditoria 7673
--DROP PROCEDURE SP_Consulta_Detalle_Registros_Auditoria
CREATE OR ALTER  PROCEDURE [dbo].[SP_Consulta_Detalle_Registros_Auditoria]
-- Add the parameters for the stored procedure here
@IdRegistroAuditoria INT
AS
BEGIN

	DECLARE @IdItem INT Select @IdItem = Id from Item where ItemName = 'Variables ocultas'

	SELECT
	rad.Id,
	rad.RegistrosAuditoriaId,
	CAST(vr.Id AS NVARCHAR(12)) AS CodigoVariable,
	rad.VariableId AS VariableId,
	vr.nombre AS NombreVariable,
	--vr.descripcion AS NombreVariable,
	rad.DatoReportado AS DatoReportado,
	rad.EstadoVariableId AS EstadoVariableId,
	ev.Descripcion AS NombreEstadoVariable,
	CASE WHEN rad.Visible = 0 THEN @IdItem ELSE vaxm.SubGrupoId END SubGrupoId,
	CASE WHEN rad.Visible = 0 THEN 'Variables ocultas' ELSE it.ItemName END SubGrupoDescripcion,
	CASE WHEN rad.Visible = 0 THEN 0 ELSE CAST(it.Concept As int) END SubGrupoOrden,
	vr.MotivoVariable AS MotivoVariable,
	rad.MotivoVariable AS Motivo,
	--rad.Dato_DC_NC_ND As Dato_DC_NC_ND,
	CASE WHEN rad.Dato_DC_NC_ND IS NULL THEN vaxm.CalificacionXDefecto ELSE rad.Dato_DC_NC_ND END As Dato_DC_NC_ND,
	rad.Visible As Visible,
	vr.Bot As Bot,
	vaxm.Encuesta As VariableEncuesta,
	ra.Encuesta As RegistrosAuditoriaEncuesta,
	--1 As IpsId,
	'' As Nombre,
	tablaReferencial As TablaReferencial,
	campoReferencial As CampoReferencial,
	CASE WHEN vr.idTipoVariable IS NULL OR vr.idTipoVariable = '' OR vr.idTipoVariable = 'bit' THEN 'varchar' ELSE vr.idTipoVariable END As IdTipoVariable, -- ToDo: Pendiente definicion bit
	vr.longitud As Longitud,
	vr.decimales As Decimales,
	vr.formato As Formato,
	cic.ItemDescripcion AS ValorDatoReportado,
	vaxm.Orden,
	vr.TipoVariableItem AS TipoVariableId,
	SUBSTRING(vTipo.ItemName, 1, 1) AS TipoVariableNombre,
	vaxm.EnableDC AS EnableDC,
	vaxm.EnableNC AS EnableNC,
	vaxm.EnableND AS EnableND,
	vaxm.Condicionada AS Condicionada,
	vaxm.ValorConstante AS ValorConstante,
	vr.descripcion AS DescripcionVariable,
	vr.Alerta AS Alerta,
	vr.AlertaDescripcion AS AlertaDescripcion,
	vaxm.MedicionId AS MedicionId,
	vaxm.EsVisible AS EsVisible,
	vaxm.EsCalificable AS EsCalificable,
	(SELECT COUNT(*) FROM Hallazgos WHERE RegistrosAuditoriaDetalleId = rad.Id) AS CountHallazgos,
	RAD.Contexto AS Contexto,
	vaxm.Calculadora,
	vaxm.TipoCalculadora
	FROM



	registrosauditoriadetalle rad
	INNER JOIN RegistrosAuditoria reg on reg.Id = rad.RegistrosAuditoriaId
	INNER JOIN Variables vr ON rad.VariableId = vr.Id
	INNER JOIN VariableXMedicion vaxm ON (vr.id = vaxm.VariableId and reg.IdMedicion = vaxm.MedicionId)
	INNER JOIN EstadoVariable ev ON rad.EstadoVariableId = ev.Id
	INNER JOIN Item it ON vaxm.SubGrupoId = it.Id
	INNER JOIN RegistrosAuditoria ra ON (rad.RegistrosAuditoriaId = ra.Id)
	LEFT JOIN CatalogoCobertura cc ON cc.NombreCatalogo = tablaReferencial
	LEFT JOIN CatalogoItemCobertura cic ON cc.Id = cic.CatalogoCoberturaId AND cic.ItemId = rad.DatoReportado
	INNER JOIN Item vTipo ON vr.TipoVariableItem = vTipo.Id
	--LEFT JOIN VariableSubgrupo vsb ON vr.SubGrupoId = vsb.Id
	WHERE RAD.RegistrosAuditoriaId = @IdRegistroAuditoria AND vaxm.EsVisible = 1
	ORDER BY vaxm.SubGrupoId, vr.Id, vaxm.Orden

END --END PROCEDURE
GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Resultado_Calificacion_Masiva]    Script Date: 19/09/2022 12:45:31 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[SP_Consulta_Resultado_Calificacion_Masiva]
	
	@CurrentProcessId INT
AS
BEGIN
	
	
	SELECT Id
		  ,CurrentProcessId
		  ,RegistroAuditoriaDetalleId
		  ,IdRadicado
		  ,VariableId
		  ,NemonicoVariable
		  ,Tipo
		  ,Result
		  ,RegistroEstadoAnterior
		  ,RegistroEstadoNuevo
		  ,CreateDate
	  FROM dbo.Resultado_Calificacion_Masiva WHERE CurrentProcessId = @CurrentProcessId

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Crea_Plantilla_Calificacion_Masiva]    Script Date: 19/09/2022 12:45:31 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar
-- Create date: 2022-01-09
-- Description:	Genera plantilla para cargue de califiacion masiva
-- =============================================
CREATE OR ALTER  PROCEDURE [dbo].[SP_Crea_Plantilla_Calificacion_Masiva]
		@MedicionId INT
AS
BEGIN
	
	SELECT 
		 1 AS Id,
		'IdRadicado' AS IdRadicado, 
		'NemonicoVariable' AS NemonicoVariable,
		'Calificacion' AS Calificacion, 
		'Motivo' AS Motivo,
		'Observacion' AS Observacion 
	UNION 

	SELECT 
		 vr.Id,
		'1234', 
		 vr.nemonico,
		'NC',
		'50',
		'Observacion'

	FROM Medicion med
		INNER JOIN VariableXMedicion vxm ON vxm.MedicionId = med.Id
		INNER JOIN Variables vr ON vr.Id =  vxm.VariableId
	WHERE
		 med.Id = @MedicionId


END
GO
/****** Object:  StoredProcedure [dbo].[SP_Eliminar_RegistrosAuditoria]    Script Date: 19/09/2022 12:45:31 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
 CREATE OR ALTER  PROCEDURE [dbo].[SP_Eliminar_RegistrosAuditoria]
 (@header NVARCHAR(MAX), @line NVARCHAR(MAX), @MedicionId NVARCHAR(255), @Observacion NVARCHAR(255), @IdUsuario VARCHAR(450))
 AS
 BEGIN
 
 	SET NOCOUNT ON;
 
 	DECLARE  @MessageResult NVARCHAR(MAX) = 'OK';
 	DECLARE  @idRadiacdo INT;
 	DECLARE  @Separador NVARCHAR(255) = ',';
 	DECLARE @codigoAuditor NVARCHAR(255) = '';
 	DECLARE @perteneceBolsa INT = 0;
 	DECLARE @idRadiacdoExiste INT = 0;	
 	DECLARE @RegistroAuditoriaId INT;
 	DECLARE @EstadoRA INT;
 
 	IF CHARINDEX(';',@header) > 0 
 	BEGIN
 		SET @Separador = ';' 
 	END
 
 	DECLARE @HeaderTable TABLE (Id int IDENTITY(1,1) PRIMARY KEY, Header NVARCHAR(MAX))
 	INSERT @HeaderTable SELECT CAST(value As VARCHAR(MAX)) FROM STRING_SPLIT_CUSTOM(@header, @Separador)
 
 	DECLARE @LineTable TABLE (Id int IDENTITY(1,1) PRIMARY KEY, Valor NVARCHAR(MAX))
 	INSERT @LineTable SELECT CAST(value As VARCHAR(MAX)) FROM STRING_SPLIT_CUSTOM(@line, @Separador)
 	
 	DECLARE @UnionTable TABLE (Id INT, header NVARCHAR(MAX), Valor NVARCHAR(MAX))
 	INSERT INTO  @UnionTable SELECT H.Id, H.Header, L.Valor FROM @HeaderTable H INNER JOIN @LineTable L ON (H.Id = L.Id);
 
 	BEGIN TRY  
 		BEGIN TRAN	
 
 			SET @idRadiacdo = (SELECT CAST(Valor AS INT) FROM @UnionTable WHERE Id = 1);
 			SET @RegistroAuditoriaId = (SELECT TOP(1) Id FROM RegistrosAuditoria WHERE IdRadicado = @idRadiacdo AND [Status] = 1);
 			SET @EstadoRA = (SELECT TOP(1) Estado FROM RegistrosAuditoria WHERE IdRadicado = @idRadiacdo AND [Status] = 1);
 
 			SET @perteneceBolsa = (SELECT TOP(1) COUNT(Id) FROM RegistrosAuditoria WHERE IdRadicado = (@idRadiacdo) AND IdMedicion IN (@MedicionId)  AND [Status] = 1);			
 
 			SET @idRadiacdoExiste = (SELECT TOP(1)  COUNT(Id) FROM RegistrosAuditoria WHERE IdRadicado = (@idRadiacdo)  AND [Status] = 1);
 
 			SET @codigoAuditor = (SELECT Codigo FROM AUTH.Usuario WHERE Id = @IdUsuario);
 
 			IF ( @IdUsuario = '' OR @IdUsuario IS NULL)
 				BEGIN
 					COMMIT TRAN	
 					SET @MessageResult = 'ERROR, Radicado ' + CAST(@idRadiacdo AS nvarchar(255)) + ', No existe el Auditor Codigo ' + CAST(@codigoAuditor AS nvarchar(255))
 					SELECT @idRadiacdo AS id, @MessageResult AS Result
 				END
 			ELSE IF ( @perteneceBolsa = 0)
 				BEGIN
 					COMMIT TRAN
 					SET @MessageResult = 'ERROR, Radicado ' + CAST(@idRadiacdo AS nvarchar(255)) + ', El registro no pertenece a la bolsa con Id ' + CAST(@MedicionId AS nvarchar(255))
 					SELECT @idRadiacdo AS id, @MessageResult AS Result
 				END 
 			ELSE IF ( @idRadiacdoExiste = 0)
 				BEGIN
 					COMMIT TRAN
 					SET @MessageResult = 'ERROR, Radicado ' + CAST(@idRadiacdo AS nvarchar(255)) + ', El registro con IdRadicado ' + CAST(@idRadiacdo AS nvarchar(255)) + ' no existe '
 					SELECT @idRadiacdo AS id, @MessageResult AS Result
 				END 			
 			ELSE
 				BEGIN
 						    
 					UPDATE RegistrosAuditoria SET
 					Status = 0, ModifyDate = GETDATE()
 					WHERE IdMedicion = @MedicionId AND IdRadicado = @idRadiacdo  AND [Status] = 1
 
 					INSERT INTO [dbo].[RegistroAuditoriaLog] ([RegistroAuditoriaId], [Proceso], [Observacion], [EstadoAnterioId], [EstadoActual], [AsignadoA], [AsingadoPor], [CreatedBy], [CreatedDate], [ModifyBy], [ModificationDate], [Status])
 					VALUES(@RegistroAuditoriaId, 'Eliminar registro.', @Observacion, @EstadoRA, @EstadoRA, @IdUsuario, @IdUsuario, @IdUsuario, GETDATE(), @IdUsuario, GETDATE(), 1)
 
 					set @MessageResult = 'OK, ' + CAST(@idRadiacdo AS nvarchar(255)) + ', Registro eliminado correctamente '
 					
 					EXEC SP_Insertar_Process_Log 35, @IdUsuario, 'OK', 'Cronograma: Auditar (Eliminar)'; 
 
 					COMMIT TRAN
 					SELECT @idRadiacdo AS id, @MessageResult AS Result														  
 				END
 
 	END TRY 
 		
 	BEGIN CATCH  
 
 		ROLLBACK TRAN
 		SET @MessageResult = 'ERROR, ' + CAST(@idRadiacdo AS nvarchar(255)) + ', ' + ERROR_MESSAGE() + '- SP LINEA: ' + CAST(ERROR_LINE() AS nvarchar(255));
 		SELECT (@idRadiacdo) AS id, @MessageResult AS Result
 	END CATCH
 END 
GO
/****** Object:  StoredProcedure [dbo].[SP_MigVariables]    Script Date: 19/09/2022 12:45:31 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[SP_MigVariables]
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

		SET @Usuario = (SELECT TOP 1 Id from AUTH.Usuario)
		
			UPDATE [dbo].[Variables]
				SET [idCobertura] = dbo.LimpiarCaracteres(vcac.idCobertura)
					,[nombre] = dbo.LimpiarCaracteres(vcac.nombre)
					,[nemonico] = dbo.LimpiarCaracteres(vcac.nemonico)
					,[descripcion] = dbo.LimpiarCaracteres(vcac.descripcion)
					,[idTipoVariable] = vcac.idTipoVariable
					,[longitud] = vcac.longitud
					,[decimales] = vcac.decimales
					,[formato] = vcac.formato
					,[tablaReferencial] = vcac.tablaReferencial
					,[campoReferencial] = vcac.campoReferencial		
					,idErrorTipo = vcac.idErrorTipo
					,[ModifyDate] = GETDATE()
					,Orden = vcac.orden
				FROM @CacTable vcac
					LEFT JOIN [dbo].[Variables] vau 
					ON vcac.idVariable = vau.idVariable
					WHERE vau.idVariable is not null	

			INSERT INTO [dbo].[Variables](
				   [Activa]
				  ,[Orden]
				  ,[idVariable]
				  ,[idCobertura]
				  ,[nombre]
				  ,[nemonico]
				  ,[descripcion]
				  ,[idTipoVariable]
				  ,[longitud]
				  ,[decimales]
				  ,[formato]
				  ,[tablaReferencial]
				  ,[campoReferencial]
				  ,[CreatedBy]
				  ,[CreatedDate]
				  ,[ModifyBy]
				  ,[ModifyDate]
				  ,[MotivoVariable]
				  ,[Bot]
				  ,[TipoVariableItem]
				  ,[EstructuraVariable]
				  ,[Alerta]
				  ,[AlertaDescripcion]
				  ,[Status]
				  ,idErrorTipo
							)
							SELECT 1
								, vcac.orden
								,vcac.[idVariable]
								,vcac.[idCobertura]
								,dbo.LimpiarCaracteres(vcac.[nombre])
								,dbo.LimpiarCaracteres(vcac.[nemonico])
								,dbo.LimpiarCaracteres(vcac.[descripcion])
								,vcac.[idTipoVariable]
								,vcac.[longitud]
								,vcac.[decimales]
								,vcac.[formato]
								,vcac.[tablaReferencial]
								,vcac.[campoReferencial]
								, @Usuario
								, GETDATE()
								, null
								, null
								, null
								, 0
								, 35
								, 39
								, 0
								, null
								, 1
								, vcac.idErrorTipo
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
/****** Object:  StoredProcedure [dbo].[SP_Realiza_Calificacion_Masiva]    Script Date: 19/09/2022 12:45:31 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar
-- Create date: 2022-09-17
-- Description:	Realiza calificacion masiva
-- =============================================
CREATE OR ALTER  PROCEDURE [dbo].[SP_Realiza_Calificacion_Masiva]
	
	@InputData DT_Input_Calificacion_Masiva READONLY,
	@IdMedicion INT,
	@User NVARCHAR(MAX),
	@CurrentProcessId INT,
    @ResultCurrentProcess NVARCHAR(255),
	@Progreso INT

AS
BEGIN

		-- Tabla para procesar registros
		DECLARE @RegistrosAcalificar TABLE 
		(
			Id INT IDENTITY(1,1) PRIMARY KEY,
			IdRadicado INT,
			VariableId INT,
			NemonicoVariable NVARCHAR(MAX),
			RegistroAuditoriaId INT,
			RegistroAuditoriaDetalleId INT,
			Estado INT,
			Calificacion INT,
			Motivo NVARCHAR(MAX), 
			Observacion NVARCHAR(MAX),
			Resultado NVARCHAR(MAX) 
		)

	
		-- Construye consulta para cruzar con registros auditoria y completar los datos que se necesitan para procesar la información
		INSERT INTO @RegistrosAcalificar
		SELECT 
			inData.IdRadicado,
			vr.Id,
			inData.NemonicoVariable,
			ra.Id AS RegistroAuditoriaId,
			rad.Id  AS RegistroAuditoriaDetalleId,
			ra.Estado AS Estado,
			itm.Id AS Calificacion,
			inData.Motivo AS Motivo,
			inData.Observacion AS Observacion,
			NULL
		FROM @InputData inData

			LEFT JOIN Medicion med ON med.Id = @IdMedicion
			LEFT JOIN Enfermedad enf ON enf.idCobertura = med.IdCobertura
			LEFT JOIN Variables vr ON vr.nemonico = inData.NemonicoVariable AND vr.idCobertura = enf.idCobertura
			LEFT JOIN RegistrosAuditoria ra ON ra.IdRadicado = inData.IdRadicado AND ra.Status = 1
			LEFT JOIN RegistrosAuditoriaDetalle rad ON ra.Id = rad.RegistrosAuditoriaId AND rad.VariableId = vr.Id
			LEFT JOIN Item itm ON itm.ItemName = inData.Calificacion AND itm.CatalogId = 6

		ORDER BY ra.Id, rad.Id


		-- Variables
		DECLARE @TotalRegistros INT = (SELECT COUNT(*) FROM @RegistrosAcalificar) -- Total De Registros
		DECLARE @Posicion INT  = (SELECT top(1) Id FROM @RegistrosAcalificar order by Id); -- Variable para tomar la posicion del registro a procesar
		DECLARE @MessageResult NVARCHAR(MAX) = 'OK' -- Variable para identificar mensaje de respuesta del registro que esta procesando
		DECLARE @countCabecera INT -- Variable para indentificar que ya valido todos los detalles de la cabecera
		DECLARE @posicionCabecera INT = 0; -- Variable para indentificar que ya valido todos los detalles de la cabecera
		DECLARE @DataAnterior nvarchar(max) = ''
		DECLARE @DataActualizada nvarchar(max) = ''
		DECLARE @CountGlosaNCND INT = 0;
		DECLARE @CountVariablesNoEncontradas INT = 0;
		DECLARE @CountErrors INT = 0
		DECLARE @VariableNOEncontradas NVARCHAR(MAX)

		-- Variables del Registro
		DECLARE @IdRadicado INT
		DECLARE @NemonicoVariable NVARCHAR(MAX)
		DECLARE @VariableId INT
		DECLARE @RegistrosAuditoriaId INT
		DECLARE @RegistroAuditoriaDetalleId INT
		DECLARE @Estado INT
		DECLARE @Calificacion INT
		DECLARE @Motivo NVARCHAR(MAX) 
		DECLARE @Observacion NVARCHAR(MAX)
		DECLARE @Resultado NVARCHAR(MAX)
		DECLARE @ObservacionSeguimiento NVARCHAR(MAX)
		DECLARE @TipoObservacion INT = (SELECT Id FROM  Item where ItemName = 'General' AND CatalogId = 1)

		 -- Progreso
		DECLARE @ProgresoCargue NVARCHAR(255) = 'Medicion' + CAST(@IdMedicion AS nvarchar(255)) + ',Progreso,' + CAST(@TotalRegistros AS nvarchar(255)) 
	


		-- Inicia recorrido de registros
		WHILE (@Posicion <= @TotalRegistros)

			  BEGIN
			
		  		BEGIN TRY  
					--BEGIN TRAN
						

							-- Limpia data del registro
							SET @IdRadicado = NULL
							SET @NemonicoVariable = NULL
							SET @RegistrosAuditoriaId = NULL
							SET @RegistroAuditoriaDetalleId = NULL
							SET @Calificacion = NULL
							SET @Motivo = NULL 
							SET @Observacion = NULL
							SET @Resultado = NULL
							SET @CountErrors = 0
							SET @CountGlosaNCND = 0
							SET @CountVariablesNoEncontradas = 0
							SET @VariableNOEncontradas = ''
							set @ObservacionSeguimiento = ''

							-- Captura información del registro a procesar
							SELECT 

								@IdRadicado = rac.IdRadicado,
								@NemonicoVariable = rac.NemonicoVariable,
								@VariableId = rac.VariableId,
								@RegistrosAuditoriaId = rac.RegistroAuditoriaId,
								@RegistroAuditoriaDetalleId = rac.RegistroAuditoriaDetalleId,
								@Calificacion = rac.Calificacion,
								@Motivo = rac.Motivo,
								@Observacion = rac.Observacion,
								@Estado = rac.Estado
							FROM @RegistrosAcalificar rac
								WHERE Id = @Posicion

							-- Aumento posicion cabecera
							SET  @posicionCabecera = @posicionCabecera + 1

							-- Cantidad de Registros por Radicado
							SET @countCabecera = (SELECT COUNT(*) FROM  @RegistrosAcalificar WHERE RegistroAuditoriaId = @RegistrosAuditoriaId);

							-- Conteo de variables no encontradas por el radicado
							SET @CountVariablesNoEncontradas = (SELECT COUNT(*) FROM @RegistrosAcalificar WHERE IdRadicado = @IdRadicado AND VariableId IS NULL)										   



							-- Validaciones
							IF @RegistrosAuditoriaId IS NULL
								BEGIN
										-- Resultado 
										INSERT INTO [dbo].[Resultado_Calificacion_Masiva]([CurrentProcessId], [RegistroAuditoriaDetalleId],[IdRadicado],[VariableId],[NemonicoVariable],[Tipo]
												   ,[Result],[RegistroEstadoAnterior],[RegistroEstadoNuevo],[CreateDate])
											 VALUES(@CurrentProcessId, @RegistroAuditoriaDetalleId, @IdRadicado,@VariableId,@NemonicoVariable,'ERROR','No se encontro información para el Id de radicado',NULL,NULL,GETDATE())
							
								END
							IF @RegistroAuditoriaDetalleId IS NULL
								BEGIN
										-- Resultado 
										INSERT INTO [dbo].[Resultado_Calificacion_Masiva]([CurrentProcessId], [RegistroAuditoriaDetalleId],[IdRadicado],[VariableId],[NemonicoVariable],[Tipo]
												   ,[Result],[RegistroEstadoAnterior],[RegistroEstadoNuevo],[CreateDate])
											 VALUES(@CurrentProcessId, @RegistroAuditoriaDetalleId, @IdRadicado,@VariableId,@NemonicoVariable,'ERROR','No se encontro la variable para el Id de radicado',NULL,NULL,GETDATE())
							
								END
							ELSE IF @Estado IN (2,4,8,13)
								BEGIN

										--2	GRE	Glosa en revisión por la entidad
										--4	GORE	Glosa objetada en revisión por la entidad
										--8	E	Enviado a entidad
										--13	H1E	Hallazgo 1 enviado a entidad

										-- Resultado estado no valido
										INSERT INTO [dbo].[Resultado_Calificacion_Masiva]([CurrentProcessId], [RegistroAuditoriaDetalleId],[IdRadicado],[VariableId],[NemonicoVariable],[Tipo]
												   ,[Result],[RegistroEstadoAnterior],[RegistroEstadoNuevo],[CreateDate])
											 VALUES(@CurrentProcessId, @RegistroAuditoriaDetalleId, @IdRadicado,@VariableId,@NemonicoVariable,'ERROR','Estado del registro no valido para calificación masiva',NULL,NULL,GETDATE())

								END
							ELSE IF (@CountVariablesNoEncontradas > 0)
								BEGIN
								
										SET @VariableNOEncontradas = (SELECT ', Variable: ' + NemonicoVariable FROM @RegistrosAcalificar WHERE IdRadicado = @IdRadicado  ORDER BY Id desc FOR XML PATH (''))

										INSERT INTO [dbo].[Resultado_Calificacion_Masiva]([CurrentProcessId], [RegistroAuditoriaDetalleId],[IdRadicado],[VariableId],[NemonicoVariable],[Tipo]
												   ,[Result],[RegistroEstadoAnterior],[RegistroEstadoNuevo],[CreateDate])
											 VALUES(@CurrentProcessId, @RegistroAuditoriaDetalleId, @IdRadicado,@VariableId,@NemonicoVariable,'ERROR','Variables no encontradas en la medición: ' + @VariableNOEncontradas ,NULL,NULL,GETDATE())

								END 
							ELSE
								BEGIN 

										-- Consulta data antes de modificacion
										SET @DataAnterior = (select Id, VariableId, ModifyBy, ModifyDate, MotivoVariable, DatO_DC_NC_ND from RegistrosAuditoriaDetalle WHERE Id = @RegistroAuditoriaDetalleId  FOR XML PATH)


										-- Registra resultado registro
										INSERT INTO [dbo].[Resultado_Calificacion_Masiva]([CurrentProcessId], [RegistroAuditoriaDetalleId],[IdRadicado],[VariableId],[NemonicoVariable],[Tipo]
												   ,[Result],[RegistroEstadoAnterior],[RegistroEstadoNuevo],[CreateDate])
											 VALUES(@CurrentProcessId, @RegistroAuditoriaDetalleId, @IdRadicado,@VariableId,@NemonicoVariable,'OK','',@DataAnterior,NULL,GETDATE())

	

										-- Actualizar calificacion
										EXEC [dbo].[ActualizarCampo_Dato_DC_NC_ND] 
													@RegistrosAuditoriaDetalle = @RegistroAuditoriaDetalleId, 
													@MotivoVariable = @Motivo, 
													@Dato_DC_NC_ND = @Calificacion, 
													@UserId = @User
								

										-- Consulta data modificada
										SET @DataActualizada = (select Id, VariableId, ModifyBy, ModifyDate, MotivoVariable, DatO_DC_NC_ND from RegistrosAuditoriaDetalle WHERE Id = @RegistroAuditoriaDetalleId  FOR XML PATH)
						
										-- Actualiza registro con data actualizada
										UPDATE Resultado_Calificacion_Masiva
											SET Result = 'Calificacion Realizada',
												RegistroEstadoNuevo = @DataActualizada
											WHERE CurrentProcessId = @CurrentProcessId AND
												  RegistroAuditoriaDetalleId = @RegistroAuditoriaDetalleId
								  
								
										-- Valida si se calificaron todas las variables del registro

										IF @posicionCabecera =  @countCabecera
											BEGIN

												-- Reinicia conteo
												SET  @posicionCabecera = 0;

												-- SP Valida Glosas
												EXEC @CountGlosaNCND = [dbo].[SP_Validacion_Conteo_Glosas] @RegistroAuditoriaId = @RegistrosAuditoriaId


												IF @CountGlosaNCND = 0
												BEGIN
														-- Valida errores
													--	EXEC   [dbo].[SP_Valida_Errores] @RegistroAuditoria = @RegistrosAuditoriaId, @User = @User, @ShowInfo = 0								
														EXEC @CountErrors = [dbo].[SP_Consulta_Cantidad_ErrorRegistroAuditoria] @RegistroAuditoriaId = @RegistrosAuditoriaId

												END						


												-- Valida cantidad de errores
												IF @CountErrors > 0
													BEGIN
										
														-- Actualiza resultado registro con error
														UPDATE Resultado_Calificacion_Masiva
															SET Result =  Result + ', El registro presenta errores de logica',
																Tipo = 'ERROR',
																RegistroEstadoNuevo = @DataActualizada
															WHERE CurrentProcessId = @CurrentProcessId AND
																  IdRadicado = @IdRadicado AND
																  RegistroAuditoriaDetalleId IS NOT NULL

													END

												ELSE
													BEGIN


														-- Registra Observacion
														IF (SELECT COUNT(*) FROM @RegistrosAcalificar WHERE IdRadicado = @IdRadicado AND Observacion IS  NOT NULL AND Observacion <> '') > 0
															BEGIN
																SET @ObservacionSeguimiento = 	(SELECT TOP(1) Observacion FROM @RegistrosAcalificar WHERE IdRadicado = @IdRadicado AND Observacion IS  NOT NULL AND Observacion <> '')

																	-- Registra seguimiento
																	INSERT INTO [dbo].[RegistrosAuditoriaDetalleSeguimiento]([RegistroAuditoriaId],[TipoObservacion],[Observacion],[Soporte],[EstadoActual]
																			   ,[EstadoNuevo],[CreatedBy],[CreatedDate],[ModifyBy],[ModifyDate],[Status])
																		 VALUES(@RegistrosAuditoriaId,@TipoObservacion,@Observacion,0,@Estado,@Estado,@User,GETDATE(),@User,GETDATE(),1)

															END


														-- Actualiza estado
														EXEC [dbo].[SP_Actualiza_Estado_Registro_Auditoria] @userId = @User, @RegistroAuditoriaId = @RegistrosAuditoriaId, @BotonAccion = 0


														IF @Estado = 1 --Registro Nuevo (Para cambiar de nuevo el estado y no quede en RP
															BEGIN
																EXEC [dbo].[SP_Actualiza_Estado_Registro_Auditoria] @userId = @User, @RegistroAuditoriaId = @RegistrosAuditoriaId, @BotonAccion = 0
															END

														

														IF @CountGlosaNCND = 0
															BEGIN
																	SET @MessageResult = ', Validacion de errores exitosa, Cambio de estado realizado'
															END
														ELSE
															BEGIN
																	SET @MessageResult = ', Registro con glosas (No se validan errores)' 
															END

														-- Actualiza resultado registro con error
														UPDATE Resultado_Calificacion_Masiva
															SET Result =  Result + @MessageResult,
																Tipo = 'OK',
																RegistroEstadoNuevo = @DataActualizada
															WHERE CurrentProcessId = @CurrentProcessId AND
																  IdRadicado = @IdRadicado

													END
								


								
											END						
						
								END --END ELSE			
						
							--Actualiza porceso actual
							UPDATE Current_Process SET Result = REPLACE(@ProgresoCargue, 'Progreso', CAST(@TotalRegistros AS nvarchar(255))) WHERE Id = @CurrentProcessId 

							-- Actualiza contador posiciones
							SET @Posicion = @Posicion + 1 

							END TRY 
		
							BEGIN CATCH  

								-- SELECT
								--ERROR_NUMBER() AS ErrorNumber,
								--ERROR_STATE() AS ErrorState,
								--ERROR_SEVERITY() AS ErrorSeverity,
								--ERROR_PROCEDURE() AS ErrorProcedure,
								--ERROR_LINE() AS ErrorLine,
								--ERROR_MESSAGE() AS ErrorMessage;
								--ROLLBACK TRAN
								set @MessageResult = CAST(@IdRadicado AS nvarchar(255)) + + ', ' + ERROR_MESSAGE() + '- SP LINEA: ' + CAST(ERROR_LINE() AS nvarchar(255)) + '- ' + CAST(XACT_STATE() AS nvarchar(255));


								IF ((SELECT COUNT(*) FROM Resultado_Calificacion_Masiva WHERE  CurrentProcessId = @CurrentProcessId AND RegistroAuditoriaDetalleId = @RegistroAuditoriaDetalleId) > 0 )
									BEGIN

										-- Actualiza resultado registro con error
										UPDATE Resultado_Calificacion_Masiva
											SET Result =  Result + ', ERROR ' + @MessageResult,
												Tipo = 'ERROR',
												RegistroEstadoNuevo = @DataActualizada
											WHERE CurrentProcessId = @CurrentProcessId AND
												  RegistroAuditoriaDetalleId = @RegistroAuditoriaDetalleId
									END
								ELSE
									BEGIN
									
										-- Inserta resultado registro
										INSERT INTO [dbo].[Resultado_Calificacion_Masiva]([CurrentProcessId], [RegistroAuditoriaDetalleId],[IdRadicado],[VariableId],[NemonicoVariable],[Tipo]
													,[Result],[RegistroEstadoAnterior],[RegistroEstadoNuevo],[CreateDate])
										VALUES(@CurrentProcessId, @RegistroAuditoriaDetalleId, @IdRadicado, @VariableId,@NemonicoVariable,'OK',@MessageResult,NULL,NULL,GETDATE())
									END
						
								-- Actualiza contador posiciones
								SET @Posicion = @Posicion + 1 

							END CATCH

				

			  END -- END WHILE
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Valida_Errores]    Script Date: 19/09/2022 12:45:31 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER  PROCEDURE [dbo].[SP_Valida_Errores] 
	-- Add the parameters for the stored procedure here
	 @RegistroAuditoria INT,
     @User NVARCHAR(MAX),
	 @ShowInfo BIT = 1
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
					LEFT JOIN RegistrosAuditoriaDetalle rad2 ON rad2.RegistrosAuditoriaId = rad.RegistrosAuditoriaId AND rad2.VariableId = v2.Id  AND rad.IdRadicado = rad2.IdRadicado

					-- Varaible  3 (Sentencia 2) 
					LEFT JOIN Variables v3 ON v3.idVariable = rc.idVariableAsociada
					LEFT JOIN RegistrosAuditoriaDetalle rad3 ON rad3.RegistrosAuditoriaId = rad.RegistrosAuditoriaId AND rad3.VariableId = v3.Id  AND rad3.IdRadicado = rad2.IdRadicado

					-- Varaible  4 (Sentencia 2) 
					LEFT JOIN Variables v4 ON v4.idVariable = rc.idVariableComparacionAsociada
					LEFT JOIN RegistrosAuditoriaDetalle rad4 ON rad4.RegistrosAuditoriaId = rad.RegistrosAuditoriaId AND rad4.VariableId = v4.Id  AND rad3.IdRadicado = rad4.IdRadicado

				WHERE 
	
					rad.RegistrosAuditoriaId = @RegistroAuditoria
					AND rg.habilitado = 1
					AND vmx.EsCalificable = 1 
					AND vmx.EsVisible = 1
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
									BEGIN TRY
											INSERT @ResultDataTemp  EXEC (@ScriptComparacionCompleto)	
									END TRY

									BEGIN CATCH
											PRINT CAST(ERROR_MESSAGE() AS nvarchar(255))  + '- SP LINEA: ' + CAST(ERROR_LINE() AS nvarchar(255)) + '- ' + CAST(XACT_STATE() AS nvarchar(255));
									END CATCH

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
						(SELECT TOP(1) Result FROM @ResultDataTemp) = 1)

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

			IF @ShowInfo = 1
			BEGIN 
				EXEC SP_Consulta_ErrorRegistroAuditoria @RegistrosAuditoriaId = @RegistroAuditoria
			END



END


GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Conteo_Glosas]    Script Date: 19/09/2022 12:45:31 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor cuellar
-- Create date: 2022-09-16
-- Description:	Valida si existen glosas
-- =============================================
CREATE OR ALTER  PROCEDURE [dbo].[SP_Validacion_Conteo_Glosas]
	
	@RegistroAuditoriaId INT
AS
BEGIN

	DECLARE @IdItemGlosa INT = (SELECT Id FROM Item WHERE ItemName = 'Glosas' AND CatalogId = 4);
	DECLARE @IdItemDC INT = (SELECT Id FROM Item WHERE ItemName = 'DC' AND CatalogId = 6);

	DECLARE @Conteo INT  = (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rg
								INNER JOIN RegistrosAuditoria ra ON rg.RegistrosAuditoriaId = ra.Id
								INNER JOIN VariableXMedicion vm ON vm.VariableId = rg.VariableId AND ra.IdMedicion = vm.MedicionId
							  WHERE rg.RegistrosAuditoriaId = @RegistroAuditoriaId 
								AND vm.SubGrupoId = @IdItemGlosa -- Glosa
								AND rg.Dato_DC_NC_ND <> @IdItemDC -- DC
								AND vm.EsCalificable = 1) 
END
GO
