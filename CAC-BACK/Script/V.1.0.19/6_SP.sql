USE [AuditCAC_Development]
GO
/****** Object:  StoredProcedure [dbo].[EditarVariables]    Script Date: 10/08/2022 12:04:16 p. m. ******/
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
	@VariablesCondicional DT_LLave_Valor READONLY)
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
					TipoCampo = @TipoCampo,Promedio = @Promedio, ValidarEntreRangos = @ValidarEntreRangos,Desde = @Desde,Hasta = @Hasta,Condicionada = @Condicionada, ValorConstante = @ValorConstante, Lista = @Lista
					--VALUES(@VariableId, @MedicionId, @EsGlosa, @EsVisible, @EsCalificable, @Activo, @CreatedBy, GETDATE(), @ModifyBy, GETDATE(), @EnableDC, @EnableNC, @EnableND, @CalificacionXDefecto, @SubGrupoId, @Encuesta, @VxM_Orden)
					WHERE VariableId = @Variable AND MedicionId = @ItemValueVxM;
				END --END IF
				ELSE
				BEGIN
					--Insertamos.
					INSERT INTO dbo.VariableXMedicion(VariableId, MedicionId, EsGlosa, EsVisible, EsCalificable, Hallazgos, Activo, CreatedBy, CreationDate, ModifyBy, ModificationDate, EnableDC, EnableNC, EnableND, CalificacionXDefecto, SubGrupoId, Encuesta, Orden, TipoCampo,Promedio,ValidarEntreRangos,Desde,Hasta,Condicionada,ValorConstante, Lista)
					VALUES(@Variable, @ItemValueVxM, @EsGlosa, @EsVisible, @EsCalificable, @Hallazgos, @Activo, @CreatedBy, GETDATE(), @ModifyBy, GETDATE(), @EnableDC, @EnableNC, @EnableND, @CalificacionXDefecto, @SubGrupoId, @Encuesta, @VxM_Orden, @TipoCampo,@Promedio,@ValidarEntreRangos,@Desde,@Hasta,@Condicionada,@ValorConstante, @Lista);				
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
/****** Object:  StoredProcedure [dbo].[GetCalificacionesRegistroAuditoriaByVariableId]    Script Date: 10/08/2022 12:04:16 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--ALTER PROCEDURE [dbo].[GetCalificacionesRegistroAuditoriaByVariableId](@VariableId VARCHAR(255), @RegistrosAuditoriaId VARCHAR(255)) AS BEGIN SET NOCOUNT ON DECLARE @IdMedicion VARCHAR(MAX) = ''; SET @IdMedicion = (SELECT CAST(IdMedicion AS NVARCHAR(MAX)) FROM RegistrosAuditoria WHERE Id = @RegistrosAuditoriaId); DECLARE @Query VARCHAR(MAX); SET @Query = 'SELECT  NEWID() As Idk, ISNULL(RAC.Id,0) As Id, ' + @RegistrosAuditoriaId  + ' As RegistrosAuditoriaId,  ISNULL(RD.Id,0) As RegistrosAuditoriaDetalleId, ISNULL(VA.Id,0) As VariableId, ISNULL(RAC.IpsId,0) As IpsId, ISNULL(IT.Id,0) As ItemId, ISNULL(IT.ItemName,'''') As NombreItem, ISNULL(RAC.Calificacion,0) As Calificacion, ISNULL(RAC.Observacion,'''') As Observacion, ISNULL(RAC.CreatedBy,'''') As CreatedBy, RAC.CreatedDate, ISNULL(RAC.ModifyBy,'''') As ModifyBy, RAC.ModifyDate FROM Variables VA INNER JOIN VariablesXItems VAIT ON (VA.Id = VAIT.VariablesId) INNER JOIN Item IT ON (VAIT.ItemId = IT.Id) INNER JOIN RegistrosAuditoriaDetalle RD ON RD.RegistrosAuditoriaId =  ' + @RegistrosAuditoriaId  + '  AND RD.VariableId = VA.Id ' + 'LEFT JOIN RegistroAuditoriaCalificaciones RAC  ON (RAC.RegistrosAuditoriaDetalleId = RD.Id AND RAC.VariableId = VA.Id AND RAC.ItemId = IT.Id ) ' DECLARE @Where VARCHAR(MAX) = ''; IF(@VariableId <> '') BEGIN IF(@Where = '') BEGIN SET @Where = @Where + 'VA.Id IN (' + CAST(@VariableId AS NVARCHAR(MAX)) + ')' END ElSE BEGIN SET @Where = @Where + ' AND VA.Id IN (' + CAST(@VariableId AS NVARCHAR(MAX)) + ')' END END IF(@IdMedicion <> '') BEGIN IF(@Where = '') BEGIN SET @Where = @Where + 'VAIT.IdMedicion IN (' + CAST(@IdMedicion AS NVARCHAR(MAX)) + ')' END ElSE BEGIN SET @Where = @Where + ' AND VAIT.IdMedicion IN (' + CAST(@IdMedicion AS NVARCHAR(MAX)) + ')' END END DECLARE @OrderBy VARCHAR(MAX) = 'ORDER BY RAC.RegistrosAuditoriaId, VA.Id, IT.Id '; IF(@Where <> '' ) BEGIN SET @Where = ' WHERE ' + @Where; END DECLARE @Total VARCHAR(MAX); SET @Total = @Query + '' + @Where + ' ' + @OrderBy; PRINT(@Query); END
CREATE OR ALTER PROCEDURE [dbo].[GetCalificacionesRegistroAuditoriaByVariableId]
(@VariableId VARCHAR(255), @RegistrosAuditoriaId VARCHAR(255))
AS
BEGIN
SET NOCOUNT ON

-- Obtenemos IdMedicion
DECLARE @IdMedicion VARCHAR(MAX) = '';
SET @IdMedicion = (SELECT CAST(IdMedicion AS NVARCHAR(MAX)) FROM RegistrosAuditoria WHERE Id = @RegistrosAuditoriaId);
-- //


-- Guardamos Query inicial.
DECLARE @Query VARCHAR(MAX);
SET @Query = 'SELECT  NEWID() As Idk, ISNULL(RAC.Id,0) As Id, ' + @RegistrosAuditoriaId  + ' As RegistrosAuditoriaId,  ISNULL(RD.Id,0) As RegistrosAuditoriaDetalleId, ISNULL(VA.Id,0) As VariableId, ISNULL(RAC.IpsId,0) As IpsId, ISNULL(IT.Id,0) As ItemId, ISNULL(IT.ItemName,'''') As NombreItem, ISNULL(RAC.Calificacion,0) As Calificacion, ISNULL(RAC.Observacion,'''') As Observacion, ISNULL(RAC.CreatedBy,'''') As CreatedBy, RAC.CreatedDate, ISNULL(RAC.ModifyBy,'''') As ModifyBy, RAC.ModifyDate 
, VAIT.IdMedicion As IdMedicion 
FROM Variables VA
INNER JOIN VariablesXItems VAIT ON (VA.Id = VAIT.VariablesId)
INNER JOIN Item IT ON (VAIT.ItemId = IT.Id)
INNER JOIN RegistrosAuditoriaDetalle RD ON RD.RegistrosAuditoriaId =  ' + @RegistrosAuditoriaId  + '  AND RD.VariableId = VA.Id ' +
'LEFT JOIN RegistroAuditoriaCalificaciones RAC  ON (RAC.RegistrosAuditoriaDetalleId = RD.Id AND RAC.VariableId = VA.Id AND RAC.ItemId = IT.Id ) ' 
-- //


-- Guardamos Condiciones.  
DECLARE @Where VARCHAR(MAX) = '';  
IF(@VariableId <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'VA.Id IN (' + CAST(@VariableId AS NVARCHAR(MAX)) + ')'  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND VA.Id IN (' + CAST(@VariableId AS NVARCHAR(MAX)) + ')'  
 END   
END   
--  
IF(@IdMedicion <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'VAIT.IdMedicion IN (' + CAST(@IdMedicion AS NVARCHAR(MAX)) + ')'  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND VAIT.IdMedicion IN (' + CAST(@IdMedicion AS NVARCHAR(MAX)) + ')'  
 END   
END   
--  //

-- Validamos estado activo.
IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + ' VAIT.Status = 1 ' 
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND VAIT.Status = 1 '
END 
-- //

-- Concatenamos Query, Condiciones y OrderBy. Luego ejecutamos.  
DECLARE @OrderBy VARCHAR(MAX) = 'ORDER BY RAC.RegistrosAuditoriaId, VA.Id, IT.Id ';
IF(@Where <> '' )  
BEGIN   
 SET @Where = ' WHERE ' + @Where;                
END    
DECLARE @Total VARCHAR(MAX);  
SET @Total = @Query + '' + @Where + ' ' + @OrderBy
-- //


-- Ejecutamos.
EXEC(@Total);
--PRINT(@Total);

END
GO
/****** Object:  StoredProcedure [dbo].[GetVariablesFiltrado]    Script Date: 10/08/2022 12:04:16 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetVariablesFiltrado](@PageNumber INT, @MaxRows INT, @Id VARCHAR(MAX), @Variable VARCHAR(MAX), @Activa VARCHAR(MAX), @Orden VARCHAR(MAX), @idVariable VARCHAR(MAX), @idCobertura VARCHAR(MAX), @nombre VARCHAR(MAX), @nemonico VARCHAR(MAX), @descripcion VARCHAR(MAX), @idTipoVariable VARCHAR(MAX), @longitud VARCHAR(MAX), @decimales VARCHAR(MAX), @formato VARCHAR(MAX), @tablaReferencial VARCHAR(MAX), @campoReferencial VARCHAR(MAX), @CreatedBy VARCHAR(MAX), @CreatedDate VARCHAR(MAX), @ModifyBy VARCHAR(MAX), @ModifyDate VARCHAR(MAX), @MotivoVariable VARCHAR(MAX), @Bot VARCHAR(MAX), @TipoVariableItem VARCHAR(MAX), @EstructuraVariable VARCHAR(MAX), @MedicionId VARCHAR(MAX), @EsGlosa VARCHAR(MAX), @EsVisible VARCHAR(MAX), @EsCalificable VARCHAR(MAX), @Activo VARCHAR(MAX), @EnableDC VARCHAR(MAX), @EnableNC VARCHAR(MAX), @EnableND VARCHAR(MAX), @CalificacionXDefecto VARCHAR(MAX), @SubGrupoId VARCHAR(MAX), @Encuesta VARCHAR(MAX), @VxM_Orden VARCHAR(MAX), @Alerta VARCHAR(MAX), @AlertaDescripcion VARCHAR(MAX), @calificacionIPSItem VARCHAR(MAX), @IdRegla VARCHAR(MAX), @Concepto VARCHAR(MAX)) AS BEGIN
SET NOCOUNT ON DECLARE @Query VARCHAR(MAX);
SET @Query='SELECT '' '' As QueryNoRegistrosTotales, NEWID() As Idk, CAST(VA.Id As NVARCHAR(MAX)) As Id, CAST(VA.Id AS NVARCHAR(12)) AS Variable, VA.Activa As Activa, CAST(VA.Orden As NVARCHAR(MAX)) As Orden, CAST(VA.idVariable As NVARCHAR(MAX)) As idVariable, CAST(VA.idCobertura As NVARCHAR(MAX)) As idCobertura, CAST(VA.nombre As NVARCHAR(MAX)) As nombre, CAST(VA.nemonico As NVARCHAR(MAX)) As nemonico, CAST(VA.descripcion As NVARCHAR(MAX)) As descripcion, CAST(VA.idTipoVariable As NVARCHAR(MAX)) As idTipoVariable, CAST(VA.longitud As NVARCHAR(MAX)) As longitud, CAST(VA.decimales As NVARCHAR(MAX)) As decimales, CAST(VA.formato As NVARCHAR(MAX)) As formato, CAST(VA.tablaReferencial As NVARCHAR(MAX)) As tablaReferencial, CAST(VA.campoReferencial As NVARCHAR(MAX)) As campoReferencial, CAST(VA.CreatedBy As NVARCHAR(MAX)) As CreatedBy, CAST(VA.CreatedDate As NVARCHAR(MAX)) As CreatedDate, CAST(VA.ModifyBy As NVARCHAR(MAX)) As ModifyBy, CAST(VA.ModifyDate As NVARCHAR(MAX)) As ModifyDate, CAST(VA.MotivoVariable As NVARCHAR(MAX)) As MotivoVariable, VA.Bot As Bot,VA.TipoVariableItem TipoVariableItem, VA.EstructuraVariable AS EstructuraVariable, CAST(VAXM.VariableId As NVARCHAR(MAX)) As VariableId, CAST(VAXM.MedicionId As NVARCHAR(MAX)) As MedicionId, VAXM.EsGlosa As EsGlosa, VAXM.EsVisible As EsVisible, VAXM.EsCalificable As EsCalificable, VAXM.Hallazgos As Hallazgos, VAXM.Activo As Activo, VAXM.EnableDC As EnableDC, VAXM.EnableNC As EnableNC, VAXM.EnableND As EnableND, VAXM.CalificacionXDefecto As CalificacionXDefecto, VAXM.SubGrupoId As SubGrupoId, CAST(IT.ItemName As NVARCHAR(MAX)) As SubGrupoNombre, VAXM.Encuesta As Encuesta, CAST(VAXM.Orden As NVARCHAR(MAX)) As VxM_Orden, VA.Alerta As Alerta, CAST(VA.AlertaDescripcion As NVARCHAR(MAX)) As AlertaDescripcion, VAI.ItemId As calificacionIPSItem, CAST(IT2.ItemName As NVARCHAR(MAX)) As calificacionIPSItemNombre, CAST(RVA.IdRegla As NVARCHAR(MAX)) As IdRegla, CAST(IT3.ItemName As NVARCHAR(MAX)) As NombreRegla, CAST(RVA.Concepto As NVARCHAR(MAX)) As Concepto, ITTipo.ItemName as TipoVariableDesc, VAXM.TipoCampo, VAXM.Promedio, VAXM.ValidarEntreRangos, VAXM.Desde, VAXM.Hasta, VAXM.Condicionada, VAXM.ValorConstante, VAXM.Lista FROM Variables VA INNER JOIN VariableXMedicion VAXM ON (VA.Id=VAXM.VariableId) INNER JOIN Item IT ON (VAXM.SubGrupoId=IT.Id) INNER JOIN Item ITTipo ON (VA.TipoVariableItem=ITTipo.Id) LEFT JOIN VariablesXItems VAI ON (VA.Id=VAI.VariablesId AND VAI.Status=1 AND VAI.IdMedicion=VAXM.MedicionId) LEFT JOIN ReglasVariable RVA ON (VA.Id=RVA.IdVariable) LEFT JOIN Item IT2 ON (VAI.ItemId=IT2.Id) LEFT JOIN Item IT3 ON (RVA.IdRegla=IT3.Id) ' DECLARE @Paginate INT=@PageNumber * @MaxRows; DECLARE @Where VARCHAR(MAX)=''; IF(@Id <> '') BEGIN IF(@Where='') BEGIN
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
/****** Object:  StoredProcedure [dbo].[SP_Actualiza_Estado_Registro_Auditoria]    Script Date: 10/08/2022 12:04:16 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER PROCEDURE [dbo].[SP_Actualiza_Estado_Registro_Auditoria]
	-- Add the parameters for the stored procedure here
	@userId NVARCHAR(255),
	@RegistroAuditoriaId INT,
	@BotonAccion INT
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
								
		IF (@Estado = 1) --RN Registro nuevo
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
		DECLARE @LogObservation VARCHAR = 'Cronograma: Auditar (Cambio de estado). Radicado: ' + CAST(@IdRadicado AS nvarchar(255)) + ': ' + @Observacion;
		EXEC SP_Insertar_Process_Log 19, @userId, 'OK', @LogObservation;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Asignacion_Lider_Entidad]    Script Date: 10/08/2022 12:04:16 p. m. ******/
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
	LEFT JOIN Lider_EPS LIEPS ON (RA.IdEPS = LIEPS.IdEPS) 
	INNER JOIN Medicion ME ON ME.Id = RA.IdMedicion 
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
			SET @Where = @Where + 'LIEPS.IdCobertura IN (' + CAST(@IdCobertura AS NVARCHAR(MAX)) + ')'  
		END   
		ElSE  
		BEGIN   
			SET @Where = @Where + ' AND LIEPS.IdCobertura IN (' + CAST(@IdCobertura AS NVARCHAR(MAX)) + ')'  
		END   
	END
	--
	IF(@IdPeriodo <> '')  
	BEGIN   
		IF(@Where = '')  
		BEGIN   
			SET @Where = @Where + 'LIEPS.IdPeriodo IN (' + CAST(@IdPeriodo AS NVARCHAR(MAX)) + ')'  
		END   
		ElSE  
		BEGIN   
			SET @Where = @Where + ' AND LIEPS.IdPeriodo IN (' + CAST(@IdPeriodo AS NVARCHAR(MAX)) + ')'  
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
	ORDER BY MAX(RA.IdAuditor)
	OFFSET ' + CAST(@Paginate AS NVARCHAR(MAX)) + ' ROWS  
	FETCH NEXT ' + CAST(@MaxRows AS NVARCHAR(MAX)) + ' ROWS ONLY;'  
	-- //

	-- GROUP BY, ORDER BY
	DECLARE @GroupBy VARCHAR(MAX) = 'GROUP BY RA.IdEPS '
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
	SET @QueryCount = N'SELECT COUNT(*) As NoRegistrosTotalesFiltrado  
	FROM RegistrosAuditoria RA 
	INNER JOIN CatalogoCobertura CCO ON CCO.NombreCatalogo = ' + '''' + @CoberturaEPS + '''' + ' 
	LEFT JOIN CatalogoItemCobertura CIC ON (CCO.Id = CIC.CatalogoCoberturaId AND CIC.ItemId = RA.IdEPS) 
	LEFT JOIN Lider_EPS LIEPS ON (RA.IdEPS = LIEPS.IdEPS) 
	INNER JOIN Medicion ME ON ME.Id = RA.IdMedicion 
	LEFT JOIN AUTH.Usuario USA ON (USA.Id = LIEPS.IdAuditorLider) '  
  
	DECLARE @TotalCount NVARCHAR(MAX);  
	SET @TotalCount = @QueryCount --+ '' + @Where
  
	-- str, old_str. new_str  
	SET @Total = REPLACE(@Total, 'ScriptNoRegistrosTotales', @TotalCount);  
	-- //


	-- Imprimimos/Ejecutamos.  
	--PRINT(@Total); 
	EXEC(@Total); 

END -- END SP
GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Auditores_EPS_Cobertura_Periodo]    Script Date: 10/08/2022 12:04:16 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-08-01
-- Description:	Para consultar Auditores de la EPS por cobertura y periodo
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Consulta_Auditores_EPS_Cobertura_Periodo]
(@PageNumber INT, @MaxRows INT, @IdCobertura VARCHAR(MAX), @IdPeriodo VARCHAR(MAX), @IdEPS VARCHAR(MAX))
AS
BEGIN

	DECLARE @CoberturaEPS VARCHAR(MAX) = 'cacEPS'

	-- Guardamos Query inicial.  
	DECLARE @Query VARCHAR(MAX);  
	SET @Query = 'SELECT (ScriptNoRegistrosTotales) As NoRegistrosTotales, MAX(RA.IdAuditor) As IdAuditor, MAX(USA.Usuario) As UsuarioAuditor ' +
	', MAX(ME.IdCobertura) As IdCobertura, MAX(RA.IdPeriodo) As IdPeriodo, MAX(RA.IdEPS) As IdEPS, MAX(CIC.ItemDescripcion) As EPS ' +
	'FROM RegistrosAuditoria RA 
	INNER JOIN CatalogoCobertura CCO ON CCO.NombreCatalogo = ' + '''' + @CoberturaEPS + '''' + '
	LEFT JOIN CatalogoItemCobertura CIC ON (CCO.Id = CIC.CatalogoCoberturaId AND CIC.ItemId = RA.IdEPS)
	INNER JOIN AUTH.Usuario USA ON (USA.Id = RA.IdAuditor)
	JOIN Medicion ME ON (RA.IdMedicion = ME.Id ) ';
	--JOIN Enfermedad EF ON (EF.idCobertura = ME.IdCobertura)
	--WHERE ME.IdCobertura = @Cobertura AND RA.IdPeriodo = @Periodo
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
			SET @Where = @Where + 'ME.IdPeriodo IN (' + CAST(@IdPeriodo AS NVARCHAR(MAX)) + ')'  
		END   
		ElSE  
		BEGIN   
			SET @Where = @Where + ' AND ME.IdPeriodo IN (' + CAST(@IdPeriodo AS NVARCHAR(MAX)) + ')'  
		END   
	END
	--
	IF(@IdEPS <> '')  
	BEGIN   
		IF(@Where = '')  
		BEGIN   
			SET @Where = @Where + 'RA.IdEPS = (''' + CAST(@IdEPS AS NVARCHAR(MAX)) + ''')'  
		END   
		ElSE  
		BEGIN   
			SET @Where = @Where + ' AND RA.IdEPS = (''' + CAST(@IdEPS AS NVARCHAR(MAX)) + ''')'  
		END   
	END	
	-- //


	-- Validamos estado activo.
	IF(@Where = '')  
	BEGIN   
		SET @Where = @Where + ' RA.Status = 1 AND USA.Enable = 1 AND ME.Status = 1 ' 
	END   
	ElSE  
	BEGIN   
		SET @Where = @Where + ' AND RA.Status = 1 AND USA.Enable = 1 AND ME.Status = 1 '
	END 
	-- //

	-- Paginado  
	DECLARE @Paginado VARCHAR(MAX) = '  
	ORDER BY MAX(RA.IdAuditor)
	OFFSET ' + CAST(@Paginate AS NVARCHAR(MAX)) + ' ROWS  
	FETCH NEXT ' + CAST(@MaxRows AS NVARCHAR(MAX)) + ' ROWS ONLY;'  
	-- //

	-- GROUP BY, ORDER BY
	DECLARE @GroupBy VARCHAR(MAX) = 'GROUP BY RA.IdAuditor'
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
	SET @QueryCount = N'SELECT COUNT(*) As NoRegistrosTotalesFiltrado 
	FROM RegistrosAuditoria RA 
	INNER JOIN CatalogoCobertura CCO ON CCO.NombreCatalogo = ' + '''' + @CoberturaEPS + '''' + '
	LEFT JOIN CatalogoItemCobertura CIC ON (CCO.Id = CIC.CatalogoCoberturaId AND CIC.ItemId = RA.IdEPS)
	INNER JOIN AUTH.Usuario USA ON (USA.Id = RA.IdAuditor)
	JOIN Medicion ME ON (RA.IdMedicion = ME.Id ) '  
  
	DECLARE @TotalCount NVARCHAR(MAX);  
	SET @TotalCount = @QueryCount --+ '' + @Where
  
	-- str, old_str. new_str  
	SET @Total = REPLACE(@Total, 'ScriptNoRegistrosTotales', @TotalCount);  
	-- //


	-- Imprimimos/Ejecutamos.  
	--PRINT(@Total); 
	EXEC(@Total); 

END -- END SP
GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Items_Calificacion]    Script Date: 10/08/2022 12:04:16 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar - IT SENSE
-- Create date: 2022-05-03
-- Description:	Consulta items a califificar de las variables por medicion
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Consulta_Items_Calificacion]
	
	@MedicionId INT
AS
BEGIN
	
	SELECT 
		CAST(NEWID() AS NVARCHAR(525)) AS Id,
		vm.VariableId AS Variableid,
		vi.ItemId AS ItemId

		FROM VariableXMedicion vm 
			INNER JOIN VariablesXItems vi ON vi.VariablesId = vm.VariableId AND vi.IdMedicion = @MedicionId

		WHERE MedicionId = @MedicionId AND vm.Status = 1 AND vi.Status = 1


END
GO
/****** Object:  StoredProcedure [dbo].[SP_Consultar_Hallazgos]    Script Date: 10/08/2022 12:04:16 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-08-02
-- Description:	Consultamos los datos de hallazgos y cruzamos informacion requeirda para informe
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Consultar_Hallazgos]
AS  
BEGIN  
SET NOCOUNT ON  
  
	DECLARE @CoberturaEPS VARCHAR(MAX) = 'cacEPS'

	SELECT 
	EF.idResolutionSiame As IdResolution, EF.definicion As Resolucion,
	EF.definicion As Description,
	ME.Id As IdMeasurement, ME.Nombre As Medicion, ME.FechaInicioAuditoria As DateFromMeasur,
	RA.IdEPS As CodigoEntidad, CIC.ItemDescripcion As RazonSocial,
	RA.Identificacion As IdentificacionPaciente, 
	RA.IdRadicado As IdAuditing, RA.IdRadicado As IdRegistro, -- VALIDAR | --IdAuditing, IdRegistro,
	RA.PrimerNombre As FirstName, RA.PrimerApellido As FirstLastName,
	VAME.Orden As 'Order',

	RAD.Id As IdAuditingDetail, VA.idVariable As IdResolutionField, -- VALIDAR
	VA.nombre As ResolucionCampoVariable, VA.nemonico As Variable,
	CIC_DO.ItemDescripcion As DatoOriginal, -- VALIDAR
	RAD.DatoReportado As IdAuditValue, -- VALIDAR

	RAD.Dato_DC_NC_ND As CalificacionDato, RAD.MotivoVariable As DatoCapturado, 
	VAME.EsGlosa As VariableGlosada, 
	'' As PacienteGlosado, -- VALIDAR
	IT.ItemName As StatusVariable, ERA.Nombre As StatusPaciente,

	VA.ModifyDate As FechaUltimaModifVariable, RALG.ModificationDate As FechaUltimaModifPaciente,

	CASE WHEN RA.ComiteExperto = 1 OR RA.ComiteAdministrativo = 1 THEN 1 END As ReportadoParaComite, -- VALIDAR
	RALG.Observacion As Observaciones, RALG.EstadoActual As IdUserLastActivityAuditing, RALG.ModifyBy As IdUserLastActivityAuditingDetail,
	US.Nombres As AuditorNombre, US.Apellidos As AuditorApellido, US.Email As Mail,
	RALG_RC.CreatedDate As FechaReporte,
	'' As CausaReclamacion,	-- VALIDAR
	RA.IdPeriodo As IdPeriodo,
	'' As TieneReclamacion, 	-- VALIDAR
	VAME.EsVisible As esVisible

	FROM Hallazgos HA
	INNER JOIN RegistrosAuditoriaDetalle RAD ON (HA.RegistrosAuditoriaDetalleId = RAD.RegistrosAuditoriaId)
	INNER JOIN RegistrosAuditoria RA ON (RAD.RegistrosAuditoriaId = RA.Id)
	INNER JOIN Medicion ME ON (RA.IdMedicion = ME.Id) 
	INNER JOIN Enfermedad EF ON (ME.IdCobertura = EF.idCobertura)
	INNER JOIN VariableXMedicion VAME ON (ME.Id = VAME.MedicionId)
	INNER JOIN Variables VA ON (VA.Id = VAME.VariableId) 
	INNER JOIN CatalogoCobertura CCO ON (CCO.NombreCatalogo = @CoberturaEPS)
	LEFT JOIN CatalogoItemCobertura CIC ON (CCO.Id = CIC.CatalogoCoberturaId AND CIC.ItemId = RA.IdEPS)
	--
	LEFT JOIN CatalogoCobertura CC ON (CC.NombreCatalogo = tablaReferencial)  -- VALIDAR
	LEFT JOIN CatalogoItemCobertura CIC_DO ON (CC.Id = CIC_DO.CatalogoCoberturaId AND CIC_DO.ItemId = RAD.DatoReportado)
	--
	INNER JOIN EstadosRegistroAuditoria ERA ON (RA.Estado = ERA.Id)
	INNER JOIN Item IT ON (ME.Estado = IT.Id)
	INNER JOIN RegistroAuditoriaLog RALG ON (RA.Id = RALG.RegistroAuditoriaId)
	INNER JOIN Item IT_LOG ON (RALG.EstadoActual = IT_LOG.Id)
	INNER JOIN AUTH.Usuario US ON (RA.IdAuditor = US.Id)
	LEFT JOIN RegistroAuditoriaLog RALG_RC ON (RA.Id = RALG_RC.RegistroAuditoriaId AND RALG_RC.EstadoActual = 16) -- 16: Registro en estado cerrado

END -- END SP
GO
/****** Object:  StoredProcedure [dbo].[SP_Eliminar_RegistrosAuditoria]    Script Date: 10/08/2022 12:04:16 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-07-21
-- Description:	Para Eliminar registrosAuditorias de una Bolsa.
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Eliminar_RegistrosAuditoria]
(@header NVARCHAR(MAX), @line NVARCHAR(MAX), @MedicionId NVARCHAR(255), @Observacion NVARCHAR(255), @IdUsuario VARCHAR(450))
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- VARIABLES
	DECLARE  @MessageResult NVARCHAR(MAX) = 'OK';
	DECLARE  @idRadiacdo INT;
	DECLARE  @Separador NVARCHAR(255) = ',';
	----
	DECLARE @codigoAuditor NVARCHAR(255) = '';
	DECLARE @perteneceBolsa INT = 0;
	DECLARE @idRadiacdoExiste INT = 0;	
	DECLARE @RegistroAuditoriaId INT;
	DECLARE @EstadoRA INT;
	-- //

	IF CHARINDEX(';',@header) > 0 --CHAR(9). Para tab character. | ORIGINAL: IF CHARINDEX(';',@header) > 0 | ACTUAl: IF CHARINDEX(CHAR(9),@header) > 0
	BEGIN
		SET @Separador = ';' --CHAR(9). Para tab character. | ORIGINAL: SET @Separador = ';' | ACTUAl: SET @Separador = CHAR(9)
	END

	-- Construye tabla cabecera
	DECLARE @HeaderTable TABLE (Id int IDENTITY(1,1) PRIMARY KEY, Header NVARCHAR(MAX))
	INSERT @HeaderTable SELECT CAST(value As VARCHAR(MAX)) FROM STRING_SPLIT_CUSTOM(@header, @Separador)

	-- Construye tabla linea
	DECLARE @LineTable TABLE (Id int IDENTITY(1,1) PRIMARY KEY, Valor NVARCHAR(MAX))
	INSERT @LineTable SELECT CAST(value As VARCHAR(MAX)) FROM STRING_SPLIT_CUSTOM(@line, @Separador)
	
	--Unimos tabla Header y tabla Line.
	DECLARE @UnionTable TABLE (Id INT, header NVARCHAR(MAX), Valor NVARCHAR(MAX))
	INSERT INTO  @UnionTable SELECT H.Id, H.Header, L.Valor FROM @HeaderTable H INNER JOIN @LineTable L ON (H.Id = L.Id);

	-- //
	BEGIN TRY  
		BEGIN TRAN	

			--Capturamos valores.								
			SET @idRadiacdo = (SELECT CAST(Valor AS INT) FROM @UnionTable WHERE Id = 1);
			SET @RegistroAuditoriaId = (SELECT Id FROM RegistrosAuditoria WHERE IdRadicado = @idRadiacdo);
			SET @EstadoRA = (SELECT Estado FROM RegistrosAuditoria WHERE IdRadicado = @idRadiacdo);

			--Validamos si el Id de registro (IdRadicado), esta vinculado con la Bolsa original.
			SET @perteneceBolsa = (SELECT COUNT(Id) FROM RegistrosAuditoria WHERE IdRadicado = (@idRadiacdo) AND IdMedicion IN (@MedicionId));			

			--Validamos si el Id de registro (IdRadicado), Existe.
			SET @idRadiacdoExiste = (SELECT COUNT(Id) FROM RegistrosAuditoria WHERE IdRadicado = (@idRadiacdo));

			-- Capturamos codigo de Auditir
			SET @codigoAuditor = (SELECT Codigo FROM AUTH.Usuario WHERE Id = @IdUsuario);
			-- //

			--Validamos Errores.
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
						    
					-- Actualizamos estado. 
					UPDATE RegistrosAuditoria SET
					Status = 0
					WHERE IdMedicion = @MedicionId AND IdRadicado = @idRadiacdo
					-- //

					-- Registra Log Auditoria (RegistroAuditoriaLog)					
					INSERT INTO [dbo].[RegistroAuditoriaLog] ([RegistroAuditoriaId], [Proceso], [Observacion], [EstadoAnterioId], [EstadoActual], [AsignadoA], [AsingadoPor], [CreatedBy], [CreatedDate], [ModifyBy], [ModificationDate], [Status])
					VALUES(@RegistroAuditoriaId, 'Eliminar registro.', @Observacion, @EstadoRA, @EstadoRA, @IdUsuario, @IdUsuario, @IdUsuario, GETDATE(), @IdUsuario, GETDATE(), 1)
					-- //

					-- Regresamos valores y hacemos commit.
					set @MessageResult = 'OK, ' + CAST(@idRadiacdo AS nvarchar(255)) + ', Registro eliminado correctamente '
					
					-- Insertamos Log de operacion. 
					EXEC SP_Insertar_Process_Log 35, @IdUsuario, 'OK', 'Cronograma: Auditar (Eliminar)'; 

					COMMIT TRAN
					SELECT @idRadiacdo AS id, @MessageResult AS Result														  
					--SELECT (select CAST(Valor AS INT) FROM @UnionTable where RTRIM(LTRIM(header)) = 'idHEMOFILIA') AS id, @MessageResult AS Result
				END

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
		SET @MessageResult = 'ERROR, ' + CAST(@idRadiacdo AS nvarchar(255)) + ', ' + ERROR_MESSAGE() + '- SP LINEA: ' + CAST(ERROR_LINE() AS nvarchar(255));
		SELECT (@idRadiacdo) AS id, @MessageResult AS Result
	END CATCH
END -- END SP
GO
/****** Object:  StoredProcedure [dbo].[SP_Reversar_RegistrosAuditoria]    Script Date: 10/08/2022 12:04:16 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-08-03
-- Description:	Para reversar registrosAuditorias finalizados.
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Reversar_RegistrosAuditoria]
(@IdRegistrosAuditoria INT, @Estado VARCHAR(2), @Observacion NVARCHAR(255), @IdUsuario VARCHAR(450))
AS
BEGIN
SET NOCOUNT ON;

	-- Declaramos variables usadas
	DECLARE @MessageResult VARCHAR(MAX) = '';
	DECLARE @Status VARCHAR(10) = '';	
	--
	DECLARE @EstadoRA INT;
	DECLARE @idRadiacdoExiste INT = 0;	
	DECLARE @RadiacadoRC INT = 0;		
	DECLARE @idRadiacdo INT;
	-- //

	BEGIN TRY  
		BEGIN TRAN	

			--Capturamos valores.								
			SET @EstadoRA = (SELECT Estado FROM RegistrosAuditoria WHERE Id = @IdRegistrosAuditoria);
			
			SET @idRadiacdo = (SELECT IdRadicado FROM RegistrosAuditoria WHERE Id = @IdRegistrosAuditoria);

			--Validamos si el Id de registro (IdRadicado), Existe.
			SET @idRadiacdoExiste = (SELECT COUNT(Id) FROM RegistrosAuditoria WHERE Id = @IdRegistrosAuditoria);

			--Validamos si el Id de registro (IdRadicado), Esta Registro Cerrado.
			SET @RadiacadoRC = (SELECT COUNT(Id) FROM RegistrosAuditoria WHERE Id = @IdRegistrosAuditoria AND Estado = 16); -- 16: RC - Registro cerrado
			SET @RadiacadoRC = 1;
			-- //
			
			--Validamos Errores.
			IF ( @idRadiacdoExiste = 0 )
				BEGIN					
					COMMIT TRAN
					SET @MessageResult = 'ERROR, El registro no existe '
					SELECT 0 AS Id, @MessageResult AS Valor
				END 	
			ELSE IF( @RadiacadoRC = 0 )
				BEGIN
					COMMIT TRAN
					SET @MessageResult = 'ERROR, Radicado ' + CAST(@idRadiacdo AS nvarchar(255)) + ', El registro con IdRadicado ' + CAST(@idRadiacdo AS nvarchar(255)) + ' no se encuentra en estado cerrado RC '
					SELECT @idRadiacdo AS Id, @MessageResult AS Valor
				END
			ELSE
				BEGIN
						    
					-- Actualizamos estado. 
					UPDATE RegistrosAuditoria SET
					Estado = 17 -- RP - Registro pendiente
					WHERE Id = @IdRegistrosAuditoria;
					-- //

					-- Registra Log Auditoria (RegistroAuditoriaLog)					
					INSERT INTO [dbo].[RegistroAuditoriaLog] ([RegistroAuditoriaId], [Proceso], [Observacion], [EstadoAnterioId], [EstadoActual], [AsignadoA], [AsingadoPor], [CreatedBy], [CreatedDate], [ModifyBy], [ModificationDate], [Status])
					VALUES(@IdRegistrosAuditoria, 'Reversado.', @Observacion, @EstadoRA, @EstadoRA, @IdUsuario, @IdUsuario, @IdUsuario, GETDATE(), @IdUsuario, GETDATE(), 1)
					-- //

					-- Regresamos valores y hacemos commit.
					set @MessageResult = 'OK, ' + CAST(@idRadiacdo AS nvarchar(255)) + ', Registro reversado correctamente '
					
					-- Insertamos Log de operacion. 
					-- EXEC SP_Insertar_Process_Log 35, @IdUsuario, 'OK', 'Cronograma: Auditar (Eliminar)'; 

					COMMIT TRAN
					SELECT @idRadiacdo AS Id, @MessageResult AS Valor														  
					--SELECT (select CAST(Valor AS INT) FROM @UnionTable where RTRIM(LTRIM(header)) = 'idHEMOFILIA') AS id, @MessageResult AS Result
				END

	END TRY 
		
	BEGIN CATCH  
		ROLLBACK TRAN
		SET @MessageResult = 'ERROR, ' + CAST(@idRadiacdo AS nvarchar(255)) + ', ' + ERROR_MESSAGE() + '- SP LINEA: ' + CAST(ERROR_LINE() AS nvarchar(255));
		SELECT (@idRadiacdo) AS id, @MessageResult AS Result
	END CATCH
END -- END SP
GO
/****** Object:  StoredProcedure [dbo].[SP_Upsert_Lider_EPS]    Script Date: 10/08/2022 12:04:16 p. m. ******/
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
(@Id VARCHAR(MAX), @IdEPS VARCHAR(10), @IdAuditorLider VARCHAR(450), @IdCobertura INT, @IdPeriodo INT, @Usuario VARCHAR(450))
AS
BEGIN
BEGIN TRANSACTION [Tran1]

	BEGIN TRY		
		DECLARE @ResponseId INT;
		DECLARE @Mensaje VARCHAR(MAX) = '';

		-- Validamos si vamos a Insertar o Actualizar
		IF(@Id = '')
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
			WHERE Id = @Id;
			-- //

			SET @ResponseId = @Id;
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
		
		SET @Id = 0;
		SET @Mensaje = 'Error: ' + ERROR_MESSAGE();
		SELECT @Id AS Id, @Mensaje AS Valor

		INSERT INTO TBL_TX_LOG(Date, Thread, Level, Logger, Message, Exception)
		VALUES(GETDATE(), '1', 'ERROR CATCH', 'Fallo EN SP SP_Upsert_Lider_EPS', ERROR_MESSAGE(), CONCAT('ErrorNumber: ',ERROR_NUMBER(),' - ', 'ERROR_STATE: ',ERROR_STATE(),' - ', 'ERROR_SEVERITY: ',ERROR_SEVERITY(),' - ', 'ERROR_PROCEDURE: ',ERROR_PROCEDURE(),' - ', 'ERROR_LINE: ',ERROR_LINE(),' - ', 'ERROR_MESSAGE: ',ERROR_MESSAGE()) );
	END CATCH 
END -- END SP
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Estado_CA]    Script Date: 10/08/2022 12:04:16 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor cuellar - IT SENSE
-- Create date: 2022-03-02
-- Description:	SP para validaciones del resgistro de auditoria cuando el estado es comite administrativo CA
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Validacion_Estado_CA]
	
	@RegistroAuditoriaId INT,
	@BotonAccion INT,
	@IdUsuario NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		-- Valores respuesta
		DECLARE @HabilitarVariablesCalificar BIT = 0;
		DECLARE @VisibleBotonGuardar BIT = 1;
		DECLARE @HabilitadoBotonGuardar BIT = 0;
		DECLARE @VisibleBotonMantenerCalificacion BIT = 1;
		DECLARE @VisibleBotonEditarCalificacion BIT = 0;
		DECLARE @VisibleBotonComiteExperto BIT = 1;
		DECLARE @VisibleBotonComiteAdministrativo BIT = 1;
		DECLARE @VisibleBotonLevantarGlosa BIT = 0;
		DECLARE @ValidarErroresLogica BIT = 0;
		DECLARE @ObservacionHabilitada BIT = 0;
		DECLARE @ObservacionObligatoria BIT = 0;
		DECLARE @ObservacionRegistrada BIT = 0;
		DECLARE @CalificacionObligatoriaIPS BIT = 0;
		DECLARE @CalificacionIPSRegistrada BIT = 0;
		DECLARE @IdItemGlosa INT = 0; --  0 Para que deshabilite todos
		DECLARE @IdItemDC INT = (SELECT Id FROM Item WHERE ItemName = 'DC' and CatalogId = 6);
		DECLARE @IdItemNC INT = (SELECT Id FROM Item WHERE ItemName = 'NC' and CatalogId = 6);
		DECLARE @TipificacionObservacionDefault INT = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1); -- Item tipificacion General;
		DECLARE @TipificacionObservasionHabilitada BIT = 0;
		DECLARE @IdRegistroNuevo INT = (SELECT Id FROM EstadosRegistroAuditoria WHERE Codigo = 'RN');
		DECLARE @CodigoRegistroPendiente NVARCHAR(255) = (SELECT Codigo FROM EstadosRegistroAuditoria WHERE Id = 17);
		DECLARE @HabilitarBotonMantenerCalificacion BIT = 0;
		DECLARE @HabilitarBotonEditarCalificacion BIT = 0;
		DECLARE @HabilitarBotonComiteExperto BIT = 0;
		DECLARE @HabilitarBotonComiteAdministrativo BIT = 0;
		DECLARE @HabilitarBotonLevantarGlosa BIT = 0;
		DECLARE @CountLevantarGlosa INT = (SELECT LevantarGlosa FROM RegistrosAuditoria WHERE Id = @RegistroAuditoriaId);
		DECLARE @SolicitarMotivo BIT = 0;
		DECLARE @GuardarCadaCambioVariable BIT = 1;
		DECLARE @HabilitarGlosa BIT = 1;
		DECLARE @ErroresReportados BIT = 0;
		--
		DECLARE @VisibleBotonReversar BIT = 1;
		DECLARE @HabilitadoBotonReversar BIT = 1;

		-- Llama SP que valida si hay motivos sin registrar
		EXEC	@SolicitarMotivo = [dbo].[SP_Validacion_Registrar_Motivo]
		@IdRegistroAuditoria = @RegistroAuditoriaId


		--Variables
		--DECLARE @RoleId INT = CAST((SELECT TOP(1) RolId FROM [AUTH].[Usuario] WHERE Id = @IdUsuario) AS INT);
		DECLARE @EsLider BIT = (SELECT R.EsLider FROM [AUTH].[Usuario] US INNER JOIN AspNetRoles R ON (US.RolId = R.Id) WHERE US.Id = @IdUsuario);

			-- Validación Lider
			IF @EsLider = 1 -- Lider
			
			BEGIN
				
				SET @VisibleBotonComiteExperto = 1;
				SET @HabilitarBotonComiteExperto = 1;
				SET @HabilitadoBotonGuardar = 0;
				SET @HabilitarBotonMantenerCalificacion= 1;
				SET @HabilitarBotonLevantarGlosa = 1;
				SET @HabilitarBotonLevantarGlosa = 1;
				SET @VisibleBotonLevantarGlosa = 1


				IF @BotonAccion = 2 -- Levantar glosa
				BEGIN
					SET @HabilitadoBotonGuardar = 1;
					SET @ObservacionHabilitada = 1;	
					SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1) -- Comite
					SET @HabilitarBotonMantenerCalificacion= 0;
					SET @HabilitarBotonComiteExperto = 0;
					SET @HabilitarBotonComiteAdministrativo = 0;
					SET @HabilitarVariablesCalificar = 1;
					SET @GuardarCadaCambioVariable = 0;
					SET @HabilitarGlosa = 0;
					SET @HabilitarBotonLevantarGlosa = 0;
					SET @IdItemGlosa = (SELECT Id FROM Item WHERE ItemName = 'Glosas' and CatalogId = 4);
					
				END

				ELSE IF @BotonAccion = 3 -- MantenerCalificacion
				BEGIN

					SET @ObservacionObligatoria = 1;
					SET @HabilitadoBotonGuardar = 1;
					SET @ObservacionHabilitada = 1;	
					SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'Mantener calificación' and CatalogId = 1); -- Item tipificacion Glosa;
					SET @HabilitarBotonMantenerCalificacion= 0;
					SET @HabilitarBotonEditarCalificacion = 0;
					SET @HabilitarBotonComiteExperto = 0;
					SET @HabilitarBotonComiteAdministrativo = 0;
					SET @HabilitarBotonLevantarGlosa = 0;

					-- Valida Observacion registrada
					EXEC	@ObservacionRegistrada = [dbo].[SP_Validacion_Observacion_Registrada]
					@IdRegistroAuditoria = @RegistroAuditoriaId,
					@TipoObservacion = @TipificacionObservacionDefault

					IF @ObservacionRegistrada = 1
					BEGIN
						SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'GENERAL' and CatalogId = 1); -- Item tipificacion General
					END

				END

				ELSE IF @BotonAccion = 4 -- Comite administrativo
				BEGIN
					SET @HabilitadoBotonGuardar = 1;
					SET @ObservacionHabilitada = 1;	
					SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'Comité Administrativo' and CatalogId = 1); -- Item tipificacion Glosa;
					SET @HabilitarBotonMantenerCalificacion= 0;
					SET @HabilitarBotonComiteExperto = 0;
					SET @HabilitarBotonComiteAdministrativo = 0;
					SET @HabilitarBotonLevantarGlosa = 0;

					

				END

				ELSE IF @BotonAccion = 5 -- Comiteexperto
				BEGIN
					SET @ObservacionObligatoria = 1;
					SET @HabilitadoBotonGuardar = 1;
					SET @ObservacionHabilitada = 1;	
					SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'Comité Experto' and CatalogId = 1); -- Item tipificacion CE;
					SET @HabilitarBotonMantenerCalificacion= 0;
					SET @HabilitarBotonComiteExperto = 0;
					SET @HabilitarBotonComiteAdministrativo = 0;
					SET @HabilitarBotonLevantarGlosa = 0;

					-- Valida Observacion registrada
					EXEC	@ObservacionRegistrada = [dbo].[SP_Validacion_Observacion_Registrada]
					@IdRegistroAuditoria = @RegistroAuditoriaId,
					@TipoObservacion = @TipificacionObservacionDefault


					IF @ObservacionRegistrada = 1
					BEGIN
						SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'GENERAL' and CatalogId = 1); -- Item tipificacion General
					END


				END


				IF @CountLevantarGlosa > 0
				BEGIN 
					SET @HabilitarBotonMantenerCalificacion= 0;
					SET @HabilitarBotonEditarCalificacion = 0;
					SET @HabilitarBotonComiteExperto = 0;
					SET @HabilitarBotonComiteAdministrativo = 0;
					SET @HabilitarBotonLevantarGlosa = 0;
				END

			END
			
			ELSE IF  @EsLider != 1 AND @CountLevantarGlosa > 0 -- Auditor y glosas levantadas
			BEGIN


				SET @HabilitadoBotonGuardar = 1;
				SET @ObservacionHabilitada = 1;
				SET @IdItemGlosa = (SELECT Id FROM Item WHERE ItemName = 'Glosas' and CatalogId = 4);
				-- Validar calificacion obligatoria
				IF
					-- Calificacion Obligatoria para registro auditoria
					((SELECT COUNT(*) FROM RegistrosAuditoria
					WHERE Id = @RegistroAuditoriaId 
					AND Encuesta = 1)
					) > 0
					OR
					-- Calificacion Obligatoria para variable IPS
					((SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rd
					INNER JOIN RegistrosAuditoria ra ON ra.Id = rd.RegistrosAuditoriaId
					INNER JOIN VariableXMedicion vm ON rd.VariableId = vm.VariableId AND vm.Encuesta = 1 AND ra.IdMedicion = vm.MedicionId
					WHERE RegistrosAuditoriaId = @RegistroAuditoriaId
					AND vm.EsCalificable = 1)
					) > 0

					BEGIN
						SET @CalificacionObligatoriaIPS = 1
					END
					ELSE 
					BEGIN
						SET @CalificacionObligatoriaIPS = 0
					END

					-- Validar si se registro calificacion

					-- Contador calificaciones que deben ser registradas
					DECLARE @CountCalificacionIPS INT = (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rd
						INNER JOIN RegistrosAuditoria ra ON ra.Id = rd.RegistrosAuditoriaId
						INNER JOIN VariableXMedicion vm ON rd.VariableId = vm.VariableId AND vm.Encuesta = 1 AND ra.IdMedicion = vm.MedicionId
						WHERE RegistrosAuditoriaId = @RegistroAuditoriaId
						AND vm.EsCalificable = 1)

					-- Contador calificaciones registradas
					DECLARE @CountCalificacionRegistradasIPS INT = (SELECT COUNT(*) AS Countcalificacion
					FROM (SELECT RegistrosAuditoriaDetalleId FROM RegistroAuditoriaCalificaciones
						WHERE RegistrosAuditoriaId = @RegistroAuditoriaId 
						GROUP BY RegistrosAuditoriaDetalleId) OrdCount)

				IF @CountCalificacionRegistradasIPS < @CountCalificacionIPS 

					BEGIN
						SET @CalificacionIPSRegistrada = 0
					END
					ELSE 
					BEGIN
						SET @CalificacionIPSRegistrada = 1
					END


				-- Validacion si hay NC o ND en glosa
				IF (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rg
					INNER JOIN RegistrosAuditoria ra ON rg.RegistrosAuditoriaId = ra.Id
					INNER JOIN VariableXMedicion vm ON vm.VariableId = rg.VariableId AND ra.IdMedicion = vm.MedicionId
				  WHERE rg.RegistrosAuditoriaId = @RegistroAuditoriaId 
					AND vm.SubGrupoId = @IdItemGlosa -- Glosa
					AND rg.Dato_DC_NC_ND <> @IdItemDC -- DC
					AND vm.EsCalificable = 1
					) > 0

					BEGIN
						SET @HabilitarVariablesCalificar = 0
						SET @ObservacionObligatoria = 1
						SET @CalificacionObligatoriaIPS = 0
						SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1); -- Item tipificacion Glosa
					END
					ELSE 
					BEGIN
						SET @HabilitarVariablesCalificar = 1
						SET @ObservacionObligatoria = 0
						SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'GENERAL' and CatalogId = 1); -- Item tipificacion General

					END		


			END

		
		-- Consulta final validaciones
		SELECT
		CAST(NEWID() AS NVARCHAR(255)) AS Id,
		@HabilitarVariablesCalificar AS HabilitarVariablesCalificar,
		@VisibleBotonGuardar AS VisibleBotonGuardar,
		@HabilitadoBotonGuardar AS HabilitadoBotonGuardar,
		@VisibleBotonMantenerCalificacion AS VisibleBotonMantenerCalificacion,
		@VisibleBotonEditarCalificacion AS VisibleBotonEditarCalificacion,
		@VisibleBotonComiteExperto AS VisibleBotonComiteExperto,
		@VisibleBotonComiteAdministrativo AS VisibleBotonComiteAdministrativo,
		@VisibleBotonLevantarGlosa AS VisibleBotonLevantarGlosa,
		@ValidarErroresLogica AS ValidarErroresLogica,
		@ObservacionHabilitada AS ObservacionHabilitada,
		@ObservacionObligatoria AS ObservacionObligatoria,
		@ObservacionRegistrada AS ObservacionRegistrada,
		@CalificacionObligatoriaIPS AS CalificacionObligatoriaIPS,
		@CalificacionIPSRegistrada AS CalificacionIPSRegistrada,
		@IdItemGlosa AS IdItemGlosa,
		@IdItemDC AS IdItemDC,
		@IdItemNC AS IdItemNC,
		@TipificacionObservacionDefault  AS TipificacionObservacionDefault,
		@TipificacionObservasionHabilitada AS TipificacionObservasionHabilitada,
		@IdRegistroNuevo AS IdRegistroNuevo,
		@CodigoRegistroPendiente AS CodigoRegistroPendiente,
		@HabilitarBotonMantenerCalificacion AS HabilitarBotonMantenerCalificacion,
		@HabilitarBotonEditarCalificacion AS HabilitarBotonEditarCalificacion,
		@HabilitarBotonComiteExperto AS HabilitarBotonComiteExperto,
		@HabilitarBotonComiteAdministrativo AS HabilitarBotonComiteAdministrativo,
		@HabilitarBotonLevantarGlosa AS HabilitarBotonLevantarGlosa,
		@SolicitarMotivo AS SolicitarMotivo,
		@GuardarCadaCambioVariable AS GuardarCadaCambioVariable,
		@HabilitarGlosa AS HabilitarGlosa,
		@ErroresReportados AS ErroresReportados,
		@VisibleBotonReversar AS VisibleBotonReversar,
		@HabilitadoBotonReversar AS HabilitadoBotonReversar

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Estado_CE]    Script Date: 10/08/2022 12:04:16 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor cuellar - IT SENSE
-- Create date: 2022-03-02
-- Description:	SP para validaciones del resgistro de auditoria cuando el estado es comite administrativo CE
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Validacion_Estado_CE]
	
	@RegistroAuditoriaId INT,
	@BotonAccion INT,
	@IdUsuario NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		-- Valores respuesta
		DECLARE @HabilitarVariablesCalificar BIT = 0;
		DECLARE @VisibleBotonGuardar BIT = 1;
		DECLARE @HabilitadoBotonGuardar BIT = 0;
		DECLARE @VisibleBotonMantenerCalificacion BIT = 1;
		DECLARE @VisibleBotonEditarCalificacion BIT = 0;
		DECLARE @VisibleBotonComiteExperto BIT = 1;
		DECLARE @VisibleBotonComiteAdministrativo BIT = 1;
		DECLARE @VisibleBotonLevantarGlosa BIT = 0;
		DECLARE @ValidarErroresLogica BIT = 0;
		DECLARE @ObservacionHabilitada BIT = 0;
		DECLARE @ObservacionObligatoria BIT = 0;
		DECLARE @ObservacionRegistrada BIT = 0;
		DECLARE @CalificacionObligatoriaIPS BIT = 0;
		DECLARE @CalificacionIPSRegistrada BIT = 0;
		DECLARE @IdItemGlosa INT = 0; --  0 Para que deshabilite todos
		DECLARE @IdItemDC INT = (SELECT Id FROM Item WHERE ItemName = 'DC' and CatalogId = 6);
		DECLARE @IdItemNC INT = (SELECT Id FROM Item WHERE ItemName = 'NC' and CatalogId = 6);
		DECLARE @TipificacionObservacionDefault INT = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1); -- Item tipificacion General;
		DECLARE @TipificacionObservasionHabilitada BIT = 0;
		DECLARE @IdRegistroNuevo INT = (SELECT Id FROM EstadosRegistroAuditoria WHERE Codigo = 'RN');
		DECLARE @CodigoRegistroPendiente NVARCHAR(255) = (SELECT Codigo FROM EstadosRegistroAuditoria WHERE Id = 17);
		DECLARE @HabilitarBotonMantenerCalificacion BIT = 0;
		DECLARE @HabilitarBotonEditarCalificacion BIT = 0;
		DECLARE @HabilitarBotonComiteExperto BIT = 0;
		DECLARE @HabilitarBotonComiteAdministrativo BIT = 0;
		DECLARE @HabilitarBotonLevantarGlosa BIT = 0;
		DECLARE @CountLevantarGlosa INT = (SELECT LevantarGlosa FROM RegistrosAuditoria WHERE Id = @RegistroAuditoriaId);
		DECLARE @SolicitarMotivo BIT = 0;
		DECLARE @GuardarCadaCambioVariable BIT = 1;
		DECLARE @HabilitarGlosa BIT = 1;
		DECLARE @ErroresReportados BIT = 0;
		--
		DECLARE @VisibleBotonReversar BIT = 1;
		DECLARE @HabilitadoBotonReversar BIT = 1;

		-- Llama SP que valida si hay motivos sin registrar
		EXEC	@SolicitarMotivo = [dbo].[SP_Validacion_Registrar_Motivo]
		@IdRegistroAuditoria = @RegistroAuditoriaId


		--Variables
		--DECLARE @RoleId INT = CAST((SELECT TOP(1) RolId FROM [AUTH].[Usuario] WHERE Id = @IdUsuario) AS INT);
		DECLARE @EsLider BIT = (SELECT R.EsLider FROM [AUTH].[Usuario] US INNER JOIN AspNetRoles R ON (US.RolId = R.Id) WHERE US.Id = @IdUsuario);


			-- Validación Lider
			IF @EsLider = 1 -- Lider
			
			BEGIN
				
				SET @VisibleBotonComiteExperto = 1;
				SET @HabilitarBotonComiteExperto = 0;
				SET @HabilitadoBotonGuardar = 0;
				SET @HabilitarBotonMantenerCalificacion= 1;
				SET @HabilitarBotonLevantarGlosa = 1;
				SET @VisibleBotonLevantarGlosa = 1

				IF @BotonAccion = 2 -- Levantar Glosa
				BEGIN
					SET @HabilitadoBotonGuardar = 1;
					SET @ObservacionHabilitada = 1;	
					SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1) -- Comite
					SET @HabilitarBotonMantenerCalificacion= 0;
					SET @HabilitarBotonComiteExperto = 0;
					SET @HabilitarBotonComiteAdministrativo = 0;
					SET @HabilitarBotonLevantarGlosa = 0;
					SET @VisibleBotonLevantarGlosa = 1;
					SET @GuardarCadaCambioVariable = 0;
					SET @HabilitarGlosa = 0;

					SET @IdItemGlosa = (SELECT Id FROM Item WHERE ItemName = 'Glosas' and CatalogId = 4);

						-- Validar calificacion obligatoria
						IF
							-- Calificacion Obligatoria para registro auditoria
							((SELECT COUNT(*) FROM RegistrosAuditoria
								WHERE Id = @RegistroAuditoriaId 
								AND Encuesta = 1)
								) > 0
							OR
							-- Calificacion Obligatoria para variable IPS
							((SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rd
							INNER JOIN RegistrosAuditoria ra ON ra.Id = rd.RegistrosAuditoriaId
							INNER JOIN VariableXMedicion vm ON rd.VariableId = vm.VariableId AND vm.Encuesta = 1 AND ra.IdMedicion = vm.MedicionId
							WHERE RegistrosAuditoriaId = @RegistroAuditoriaId
							AND vm.EsCalificable = 1)
							) > 0

							BEGIN
								SET @CalificacionObligatoriaIPS = 1
							END
							ELSE 
							BEGIN
								SET @CalificacionObligatoriaIPS = 0
							END

							-- Validar si se registro calificacion

							-- Contador calificaciones que deben ser registradas
							DECLARE @CountCalificacionIPS INT = (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rd
								INNER JOIN RegistrosAuditoria ra ON ra.Id = rd.RegistrosAuditoriaId
								INNER JOIN VariableXMedicion vm ON rd.VariableId = vm.VariableId AND vm.Encuesta = 1 AND ra.IdMedicion = vm.MedicionId
								WHERE RegistrosAuditoriaId = @RegistroAuditoriaId
								AND vm.EsCalificable = 1)

							-- Contador calificaciones registradas
							DECLARE @CountCalificacionRegistradasIPS INT = (SELECT COUNT(*) AS Countcalificacion
							FROM (SELECT RegistrosAuditoriaDetalleId FROM RegistroAuditoriaCalificaciones
								WHERE RegistrosAuditoriaId = @RegistroAuditoriaId 
								GROUP BY RegistrosAuditoriaDetalleId) OrdCount)

						IF @CountCalificacionRegistradasIPS < @CountCalificacionIPS 

							BEGIN
								SET @CalificacionIPSRegistrada = 0
							END
							ELSE 
							BEGIN
								SET @CalificacionIPSRegistrada = 1
							END


						-- Validacion si hay NC o ND en glosa
						IF (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rg
							INNER JOIN RegistrosAuditoria ra ON rg.RegistrosAuditoriaId = ra.Id
							INNER JOIN VariableXMedicion vm ON vm.VariableId = rg.VariableId AND ra.IdMedicion = vm.MedicionId
						  WHERE rg.RegistrosAuditoriaId = @RegistroAuditoriaId 
							AND vm.SubGrupoId = @IdItemGlosa -- Glosa
							AND rg.Dato_DC_NC_ND <> @IdItemDC -- DC
							AND vm.EsCalificable = 1
							) > 0

							BEGIN
								SET @HabilitarVariablesCalificar = 0
								SET @ObservacionObligatoria = 1
								SET @CalificacionObligatoriaIPS = 0
								SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1); -- Item tipificacion Glosa

								EXEC	@ObservacionRegistrada = [dbo].[SP_Validacion_Observacion_Registrada]
									@IdRegistroAuditoria = @RegistroAuditoriaId,
									@TipoObservacion = @TipificacionObservacionDefault

								IF @ObservacionRegistrada = 1
								BEGIN
									SET @ObservacionObligatoria = 0
								END

							END
							ELSE 
							BEGIN
								SET @HabilitarVariablesCalificar = 1
								SET @ObservacionObligatoria = 0
								SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'Comité Experto' and CatalogId = 1); -- Item tipificacion General

							END		

				END

				ELSE IF @BotonAccion = 3 -- MantenerCalificacion
				BEGIN

					SET @ObservacionObligatoria = 1;
					SET @HabilitadoBotonGuardar = 1;
					SET @ObservacionHabilitada = 1;	
					SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'Mantener calificación' and CatalogId = 1); -- Item tipificacion Glosa;
					SET @HabilitarBotonMantenerCalificacion= 0;
					SET @HabilitarBotonComiteExperto = 0;
					SET @HabilitarBotonComiteAdministrativo = 0;
					SET @HabilitarBotonLevantarGlosa = 0;
					SET @HabilitarBotonLevantarGlosa = 0;

					-- Valida Observacion registrada
					EXEC	@ObservacionRegistrada = [dbo].[SP_Validacion_Observacion_Registrada]
					@IdRegistroAuditoria = @RegistroAuditoriaId,
					@TipoObservacion = @TipificacionObservacionDefault

					IF @ObservacionRegistrada = 1
					BEGIN
						SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'GENERAL' and CatalogId = 1); -- Item tipificacion General
					END

				END


				IF @CountLevantarGlosa > 0
				BEGIN 
					SET @HabilitarBotonMantenerCalificacion= 0;
					SET @HabilitarBotonComiteExperto = 0;
					SET @HabilitarBotonComiteAdministrativo = 0;
					SET @HabilitarBotonLevantarGlosa = 0;
					SET @HabilitarBotonLevantarGlosa = 0;
					SET @VisibleBotonLevantarGlosa = 0;
				END

			END

		
		-- Consulta final validaciones
		SELECT
		CAST(NEWID() AS NVARCHAR(255)) AS Id,
		@HabilitarVariablesCalificar AS HabilitarVariablesCalificar,
		@VisibleBotonGuardar AS VisibleBotonGuardar,
		@HabilitadoBotonGuardar AS HabilitadoBotonGuardar,
		@VisibleBotonMantenerCalificacion AS VisibleBotonMantenerCalificacion,
		@VisibleBotonEditarCalificacion AS VisibleBotonEditarCalificacion,
		@VisibleBotonComiteExperto AS VisibleBotonComiteExperto,
		@VisibleBotonComiteAdministrativo AS VisibleBotonComiteAdministrativo,
		@VisibleBotonLevantarGlosa AS VisibleBotonLevantarGlosa,
		@ValidarErroresLogica AS ValidarErroresLogica,
		@ObservacionHabilitada AS ObservacionHabilitada,
		@ObservacionObligatoria AS ObservacionObligatoria,
		@ObservacionRegistrada AS ObservacionRegistrada,
		@CalificacionObligatoriaIPS AS CalificacionObligatoriaIPS,
		@CalificacionIPSRegistrada AS CalificacionIPSRegistrada,
		@IdItemGlosa AS IdItemGlosa,
		@IdItemDC AS IdItemDC,
		@IdItemNC AS IdItemNC,
		@TipificacionObservacionDefault  AS TipificacionObservacionDefault,
		@TipificacionObservasionHabilitada AS TipificacionObservasionHabilitada,
		@IdRegistroNuevo AS IdRegistroNuevo,
		@CodigoRegistroPendiente AS CodigoRegistroPendiente,
		@HabilitarBotonMantenerCalificacion AS HabilitarBotonMantenerCalificacion,
		@HabilitarBotonEditarCalificacion AS HabilitarBotonEditarCalificacion,
		@HabilitarBotonComiteExperto AS HabilitarBotonComiteExperto,
		@HabilitarBotonComiteAdministrativo AS HabilitarBotonComiteAdministrativo,
		@HabilitarBotonLevantarGlosa AS HabilitarBotonLevantarGlosa,
		@SolicitarMotivo AS SolicitarMotivo,
		@GuardarCadaCambioVariable AS GuardarCadaCambioVariable,
		@HabilitarGlosa AS HabilitarGlosa,
		@ErroresReportados AS ErroresReportados,
		@VisibleBotonReversar AS VisibleBotonReversar,
		@HabilitadoBotonReversar AS HabilitadoBotonReversar

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Estado_ELA]    Script Date: 10/08/2022 12:04:16 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor cuellar - IT SENSE
-- Create date: 2022-03-02
-- Description:	SP para validaciones del resgistro de auditoria cuando el estado es Error lógica marcación auditor ELA
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Validacion_Estado_ELA]
	
	@RegistroAuditoriaId INT,
	@BotonAccion INT,
	@IdUsuario NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		-- Valores respuesta
		DECLARE @HabilitarVariablesCalificar BIT = 0;
		DECLARE @VisibleBotonGuardar BIT = 1;
		DECLARE @HabilitadoBotonGuardar BIT =0;
		DECLARE @VisibleBotonMantenerCalificacion BIT = 0;
		DECLARE @VisibleBotonEditarCalificacion BIT = 0;
		DECLARE @VisibleBotonComiteExperto BIT = 0;
		DECLARE @VisibleBotonComiteAdministrativo BIT = 0;
		DECLARE @VisibleBotonLevantarGlosa BIT = 0;
		DECLARE @ValidarErroresLogica BIT = 0;
		DECLARE @ObservacionHabilitada BIT = 1;
		DECLARE @ObservacionObligatoria BIT = 1;
		DECLARE @ObservacionRegistrada BIT = 0;
		DECLARE @CalificacionObligatoriaIPS BIT = 0;
		DECLARE @CalificacionIPSRegistrada BIT = 0;
		DECLARE @IdItemGlosa INT = (SELECT Id FROM Item WHERE ItemName = 'Glosas' and CatalogId = 4);
		DECLARE @IdItemDC INT = (SELECT Id FROM Item WHERE ItemName = 'DC' and CatalogId = 6);
		DECLARE @IdItemNC INT = (SELECT Id FROM Item WHERE ItemName = 'NC' and CatalogId = 6);
		DECLARE @TipificacionObservacionDefault INT = (SELECT Id FROM Item WHERE ItemName = 'Error lógica' and CatalogId = 1); -- Item tipificacion General;
		DECLARE @TipificacionObservasionHabilitada BIT = 0;
		DECLARE @IdRegistroNuevo INT = (SELECT Id FROM EstadosRegistroAuditoria WHERE Codigo = 'RN');
		DECLARE @CodigoRegistroPendiente NVARCHAR(255) = (SELECT Codigo FROM EstadosRegistroAuditoria WHERE Id = 17);
		DECLARE @HabilitarBotonMantenerCalificacion BIT = 0;
		DECLARE @HabilitarBotonEditarCalificacion BIT = 0;
		DECLARE @HabilitarBotonComiteExperto BIT = 0;
		DECLARE @HabilitarBotonComiteAdministrativo BIT = 0;
		DECLARE @HabilitarBotonLevantarGlosa BIT = 0;
		DECLARE @CountLevantarGlosa INT = (SELECT LevantarGlosa FROM RegistrosAuditoria WHERE Id = @RegistroAuditoriaId);
		DECLARE @SolicitarMotivo BIT = 0;
		DECLARE @GuardarCadaCambioVariable BIT = 1;
		DECLARE @HabilitarGlosa BIT = 0;
		DECLARE @ErroresReportados BIT = 0;
		--
		DECLARE @VisibleBotonReversar BIT = 1;
		DECLARE @HabilitadoBotonReversar BIT = 1;

		-- Llama SP que valida si hay motivos sin registrar
		EXEC	@SolicitarMotivo = [dbo].[SP_Validacion_Registrar_Motivo]
		@IdRegistroAuditoria = @RegistroAuditoriaId


		--Variables
		--DECLARE @RoleId INT = CAST((SELECT TOP(1) RolId FROM [AUTH].[Usuario] WHERE Id = @IdUsuario) AS INT);
		DECLARE @EsLider BIT = (SELECT R.EsLider FROM [AUTH].[Usuario] US INNER JOIN AspNetRoles R ON (US.RolId = R.Id) WHERE US.Id = @IdUsuario);

			-- Validación Lider
			IF @EsLider = 1 -- Lider
			
			BEGIN
			
				 SET @HabilitadoBotonGuardar = 1;
				 SET @HabilitarGlosa = 1;
				 SET @ObservacionHabilitada = 1;

				 -- Validacion si hay NC o ND en glosa
					IF (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rg
						INNER JOIN RegistrosAuditoria ra ON rg.RegistrosAuditoriaId = ra.Id
						INNER JOIN VariableXMedicion vm ON vm.VariableId = rg.VariableId AND ra.IdMedicion = vm.MedicionId
					  WHERE rg.RegistrosAuditoriaId = @RegistroAuditoriaId 
						AND vm.SubGrupoId = @IdItemGlosa -- Glosa
						AND rg.Dato_DC_NC_ND <> @IdItemDC -- DC
						AND vm.EsCalificable = 1
						) > 0

						BEGIN
							SET @HabilitarVariablesCalificar = 0
							SET @ObservacionObligatoria = 1
							SET @CalificacionObligatoriaIPS = 0
							SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1); -- Item tipificacion Glosa
							SET @ValidarErroresLogica = 0;
							SET @ErroresReportados = 0;


							-- Validacion para observacion registrada
							IF (SELECT COUNT(*) FROM RegistrosAuditoriaDetalleSeguimiento
								WHERE RegistroAuditoriaId = @RegistroAuditoriaId 
								AND TipoObservacion = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1) -- Glosa
								) > 0

								BEGIN
									SET @ObservacionRegistrada = 1
									SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'GENERAL' and CatalogId = 1); -- Item tipificacion General

								END
								ELSE 
								BEGIN
									SET @ObservacionRegistrada = 0

								END

						END
						ELSE 
						BEGIN
							SET @HabilitarVariablesCalificar = 1
							SET @ObservacionObligatoria = 1
							SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'Error lógica' and CatalogId = 1); -- Item tipificacion General

							--Validar Errores de logica 
							SET @ValidarErroresLogica = 1;

							IF (SELECT COUNT(*) FROM RegistrosAuditoriaDetalleSeguimiento
									WHERE RegistroAuditoriaId = @RegistroAuditoriaId 
									AND TipoObservacion = (SELECT Id FROM Item WHERE ItemName = 'Error lógica' and CatalogId = 1) -- Glosa
									) > 0

									BEGIN
										SET @ErroresReportados = 1
										SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'GENERAL' and CatalogId = 1); -- Item tipificacion General

									END
									ELSE 
									BEGIN
										SET @ErroresReportados = 0
										SET @CalificacionObligatoriaIPS = 0

									END


							 SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'Error lógica' and CatalogId = 1); -- Item tipificacion Error lógica;


							-- Valida si hay Observacion registrada tipo error logica por un lider
							IF( SELECT 
									COUNT(*)
								FROM RegistrosAuditoriaDetalleSeguimiento rs
									INNER JOIN [AUTH].[Usuario] ur ON rs.CreatedBy = ur.Id
													WHERE rs.RegistroAuditoriaId = @RegistroAuditoriaId 
													AND rs.TipoObservacion = @TipificacionObservacionDefault 
													AND ur.RolId = 2 -- Lider
													)
													> 0
								BEGIN 

									  SET @ObservacionRegistrada = 1;
									  SET @ErroresReportados = 1
									  SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'GENERAL' and CatalogId = 1); -- Item tipificacion General
									  SET @ObservacionObligatoria = 0;

								END

						END					
						
			END

		




		
		-- Consulta final validaciones
		SELECT
		CAST(NEWID() AS NVARCHAR(255)) AS Id,
		@HabilitarVariablesCalificar AS HabilitarVariablesCalificar,
		@VisibleBotonGuardar AS VisibleBotonGuardar,
		@HabilitadoBotonGuardar AS HabilitadoBotonGuardar,
		@VisibleBotonMantenerCalificacion AS VisibleBotonMantenerCalificacion,
		@VisibleBotonEditarCalificacion AS VisibleBotonEditarCalificacion,
		@VisibleBotonComiteExperto AS VisibleBotonComiteExperto,
		@VisibleBotonComiteAdministrativo AS VisibleBotonComiteAdministrativo,
		@VisibleBotonLevantarGlosa AS VisibleBotonLevantarGlosa,
		@ValidarErroresLogica AS ValidarErroresLogica,
		@ObservacionHabilitada AS ObservacionHabilitada,
		@ObservacionObligatoria AS ObservacionObligatoria,
		@ObservacionRegistrada AS ObservacionRegistrada,
		@CalificacionObligatoriaIPS AS CalificacionObligatoriaIPS,
		@CalificacionIPSRegistrada AS CalificacionIPSRegistrada,
		@IdItemGlosa AS IdItemGlosa,
		@IdItemDC AS IdItemDC,
		@IdItemNC AS IdItemNC,
		@TipificacionObservacionDefault  AS TipificacionObservacionDefault,
		@TipificacionObservasionHabilitada AS TipificacionObservasionHabilitada,
		@IdRegistroNuevo AS IdRegistroNuevo,
		@CodigoRegistroPendiente AS CodigoRegistroPendiente,
		@HabilitarBotonMantenerCalificacion AS HabilitarBotonMantenerCalificacion,
		@HabilitarBotonEditarCalificacion AS HabilitarBotonEditarCalificacion,
		@HabilitarBotonComiteExperto AS HabilitarBotonComiteExperto,
		@HabilitarBotonComiteAdministrativo AS HabilitarBotonComiteAdministrativo,
		@HabilitarBotonLevantarGlosa AS HabilitarBotonLevantarGlosa,
		@SolicitarMotivo AS SolicitarMotivo,
		@GuardarCadaCambioVariable AS GuardarCadaCambioVariable,
		@HabilitarGlosa AS HabilitarGlosa,
		@ErroresReportados AS ErroresReportados,
		@VisibleBotonReversar AS VisibleBotonReversar,
		@HabilitadoBotonReversar AS HabilitadoBotonReversar

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Estado_ELL]    Script Date: 10/08/2022 12:04:16 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor cuellar - IT SENSE
-- Create date: 2022-03-02
-- Description:	SP para validaciones del resgistro de auditoria cuando el estado es Error lógica marcación lider ELL
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Validacion_Estado_ELL]
	
	@RegistroAuditoriaId INT,
	@BotonAccion INT,
	@IdUsuario NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		-- Valores respuesta
		DECLARE @HabilitarVariablesCalificar BIT = 0;
		DECLARE @VisibleBotonGuardar BIT = 1;
		DECLARE @HabilitadoBotonGuardar BIT =0;
		DECLARE @VisibleBotonMantenerCalificacion BIT = 0;
		DECLARE @VisibleBotonEditarCalificacion BIT = 0;
		DECLARE @VisibleBotonComiteExperto BIT = 0;
		DECLARE @VisibleBotonComiteAdministrativo BIT = 0;
		DECLARE @VisibleBotonLevantarGlosa BIT = 0;
		DECLARE @ValidarErroresLogica BIT = 0;
		DECLARE @ObservacionHabilitada BIT = 0;
		DECLARE @ObservacionObligatoria BIT = 1;
		DECLARE @ObservacionRegistrada BIT = 0;
		DECLARE @CalificacionObligatoriaIPS BIT = 0;
		DECLARE @CalificacionIPSRegistrada BIT = 0;
		DECLARE @IdItemGlosa INT = (SELECT Id FROM Item WHERE ItemName = 'Glosas' and CatalogId = 4);
		DECLARE @IdItemDC INT = (SELECT Id FROM Item WHERE ItemName = 'DC' and CatalogId = 6);
		DECLARE @IdItemNC INT = (SELECT Id FROM Item WHERE ItemName = 'NC' and CatalogId = 6);
		DECLARE @TipificacionObservacionDefault INT = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1); -- Item tipificacion General;
		DECLARE @TipificacionObservasionHabilitada BIT = 0;
		DECLARE @IdRegistroNuevo INT = (SELECT Id FROM EstadosRegistroAuditoria WHERE Codigo = 'RN');
		DECLARE @CodigoRegistroPendiente NVARCHAR(255) = (SELECT Codigo FROM EstadosRegistroAuditoria WHERE Id = 17);
		DECLARE @HabilitarBotonMantenerCalificacion BIT = 0;
		DECLARE @HabilitarBotonEditarCalificacion BIT = 0;
		DECLARE @HabilitarBotonComiteExperto BIT = 0;
		DECLARE @HabilitarBotonComiteAdministrativo BIT = 0;
		DECLARE @HabilitarBotonLevantarGlosa BIT = 0;
		DECLARE @CountLevantarGlosa INT = (SELECT LevantarGlosa FROM RegistrosAuditoria WHERE Id = @RegistroAuditoriaId);
		DECLARE @SolicitarMotivo BIT = 0;
		DECLARE @GuardarCadaCambioVariable BIT = 1;
		DECLARE @HabilitarGlosa BIT = 0;
		DECLARE @ErroresReportados BIT = 0;
		--
		DECLARE @VisibleBotonReversar BIT = 1;
		DECLARE @HabilitadoBotonReversar BIT = 1;

		-- Llama SP que valida si hay motivos sin registrar
		EXEC	@SolicitarMotivo = [dbo].[SP_Validacion_Registrar_Motivo]
		@IdRegistroAuditoria = @RegistroAuditoriaId


		--Variables
		--DECLARE @RoleId INT = CAST((SELECT TOP(1) RolId FROM [AUTH].[Usuario] WHERE Id = @IdUsuario) AS INT);
		DECLARE @EsLider BIT = (SELECT R.EsLider FROM [AUTH].[Usuario] US INNER JOIN AspNetRoles R ON (US.RolId = R.Id) WHERE US.Id = @IdUsuario);


			
				 SET @HabilitadoBotonGuardar = 1;
				 SET @HabilitarGlosa = 1;
				 SET @ObservacionHabilitada = 1;

				 -- Validacion si hay NC o ND en glosa
					IF (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rg
						INNER JOIN RegistrosAuditoria ra ON rg.RegistrosAuditoriaId = ra.Id
						INNER JOIN VariableXMedicion vm ON vm.VariableId = rg.VariableId AND ra.IdMedicion = vm.MedicionId
					  WHERE rg.RegistrosAuditoriaId = @RegistroAuditoriaId 
						AND vm.SubGrupoId = @IdItemGlosa -- Glosa
						AND rg.Dato_DC_NC_ND <> @IdItemDC -- DC
						AND vm.EsCalificable = 1
						) > 0

						BEGIN
							SET @HabilitarVariablesCalificar = 0
							SET @ObservacionObligatoria = 1
							SET @CalificacionObligatoriaIPS = 0
							SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1); -- Item tipificacion Glosa
							SET @ValidarErroresLogica = 0;
							SET @ErroresReportados = 0;


							-- Validacion para observacion registrada
							IF (SELECT COUNT(*) FROM RegistrosAuditoriaDetalleSeguimiento
								WHERE RegistroAuditoriaId = @RegistroAuditoriaId 
								AND TipoObservacion = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1) -- Glosa
								) > 0

								BEGIN
									SET @ObservacionRegistrada = 1
									SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'GENERAL' and CatalogId = 1); -- Item tipificacion General

								END
								ELSE 
								BEGIN
									SET @ObservacionRegistrada = 0

								END

						END
						ELSE 
						BEGIN
							SET @HabilitarVariablesCalificar = 1
							SET @ObservacionObligatoria = 0
							SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'GENERAL' and CatalogId = 1); -- Item tipificacion General

							--Validar Errores de logica 
							SET @ValidarErroresLogica = 1;
							SET @ErroresReportados = 0
							SET @CalificacionObligatoriaIPS = 0



						END					
						

		




		
		-- Consulta final validaciones
		SELECT
		CAST(NEWID() AS NVARCHAR(255)) AS Id,
		@HabilitarVariablesCalificar AS HabilitarVariablesCalificar,
		@VisibleBotonGuardar AS VisibleBotonGuardar,
		@HabilitadoBotonGuardar AS HabilitadoBotonGuardar,
		@VisibleBotonMantenerCalificacion AS VisibleBotonMantenerCalificacion,
		@VisibleBotonEditarCalificacion AS VisibleBotonEditarCalificacion,
		@VisibleBotonComiteExperto AS VisibleBotonComiteExperto,
		@VisibleBotonComiteAdministrativo AS VisibleBotonComiteAdministrativo,
		@VisibleBotonLevantarGlosa AS VisibleBotonLevantarGlosa,
		@ValidarErroresLogica AS ValidarErroresLogica,
		@ObservacionHabilitada AS ObservacionHabilitada,
		@ObservacionObligatoria AS ObservacionObligatoria,
		@ObservacionRegistrada AS ObservacionRegistrada,
		@CalificacionObligatoriaIPS AS CalificacionObligatoriaIPS,
		@CalificacionIPSRegistrada AS CalificacionIPSRegistrada,
		@IdItemGlosa AS IdItemGlosa,
		@IdItemDC AS IdItemDC,
		@IdItemNC AS IdItemNC,
		@TipificacionObservacionDefault  AS TipificacionObservacionDefault,
		@TipificacionObservasionHabilitada AS TipificacionObservasionHabilitada,
		@IdRegistroNuevo AS IdRegistroNuevo,
		@CodigoRegistroPendiente AS CodigoRegistroPendiente,
		@HabilitarBotonMantenerCalificacion AS HabilitarBotonMantenerCalificacion,
		@HabilitarBotonEditarCalificacion AS HabilitarBotonEditarCalificacion,
		@HabilitarBotonComiteExperto AS HabilitarBotonComiteExperto,
		@HabilitarBotonComiteAdministrativo AS HabilitarBotonComiteAdministrativo,
		@HabilitarBotonLevantarGlosa AS HabilitarBotonLevantarGlosa,
		@SolicitarMotivo AS SolicitarMotivo,
		@GuardarCadaCambioVariable AS GuardarCadaCambioVariable,
		@HabilitarGlosa AS HabilitarGlosa,
		@ErroresReportados AS ErroresReportados,
		@VisibleBotonReversar AS VisibleBotonReversar,
		@HabilitadoBotonReversar AS HabilitadoBotonReversar

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Estado_GO1]    Script Date: 10/08/2022 12:04:16 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-02-02
-- Description:	SP para validaciones del resgistro de auditoria cuando el estado es glosa objectada 1
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Validacion_Estado_GO1]
	
	@RegistroAuditoriaId INT,
	@BotonAccion INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		-- Valores respuesta
		DECLARE @HabilitarVariablesCalificar BIT = 0;
		DECLARE @VisibleBotonGuardar BIT = 1;
		DECLARE @HabilitadoBotonGuardar BIT = 0;
		DECLARE @VisibleBotonMantenerCalificacion BIT = 1;
		DECLARE @VisibleBotonEditarCalificacion BIT = 1;
		DECLARE @VisibleBotonComiteExperto BIT = 0;
		DECLARE @VisibleBotonComiteAdministrativo BIT = 0;
		DECLARE @VisibleBotonLevantarGlosa BIT = 0;
		DECLARE @ValidarErroresLogica BIT = 0;
		DECLARE @ObservacionHabilitada BIT = 0;
		DECLARE @ObservacionObligatoria BIT = 0;
		DECLARE @ObservacionRegistrada BIT = 0;
		DECLARE @CalificacionObligatoriaIPS BIT = 0;
		DECLARE @CalificacionIPSRegistrada BIT = 0;
		DECLARE @IdItemGlosa INT = 0; --  0 Para que deshabilite todos
		DECLARE @IdItemDC INT = (SELECT Id FROM Item WHERE ItemName = 'DC' and CatalogId = 6);
		DECLARE @IdItemNC INT = (SELECT Id FROM Item WHERE ItemName = 'NC' and CatalogId = 6);
		DECLARE @TipificacionObservacionDefault INT = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1); -- Item tipificacion General;
		DECLARE @TipificacionObservasionHabilitada BIT = 0;
		DECLARE @IdRegistroNuevo INT = (SELECT Id FROM EstadosRegistroAuditoria WHERE Codigo = 'RN');
		DECLARE @CodigoRegistroPendiente NVARCHAR(255) = (SELECT Codigo FROM EstadosRegistroAuditoria WHERE Id = 17);
		DECLARE @HabilitarBotonMantenerCalificacion BIT = 1;
		DECLARE @HabilitarBotonEditarCalificacion BIT = 1;
		DECLARE @HabilitarBotonComiteExperto BIT = 0;
		DECLARE @HabilitarBotonComiteAdministrativo BIT = 0;
		DECLARE @HabilitarBotonLevantarGlosa BIT = 0;
		DECLARE @SolicitarMotivo BIT = 0;
		DECLARE @GuardarCadaCambioVariable BIT = 1;
		DECLARE @HabilitarGlosa BIT = 1;
		DECLARE @ErroresReportados BIT = 0;
		--
		DECLARE @VisibleBotonReversar BIT = 1;
		DECLARE @HabilitadoBotonReversar BIT = 1;

		-- Llama SP que valida si hay motivos sin registrar
		EXEC	@SolicitarMotivo = [dbo].[SP_Validacion_Registrar_Motivo]
		@IdRegistroAuditoria = @RegistroAuditoriaId

		-- Valida accion editar
			IF @BotonAccion = 1 			
			BEGIN
				SET @HabilitarBotonMantenerCalificacion = 0;
				SET @IdItemGlosa = (SELECT Id FROM Item WHERE ItemName = 'Glosas' and CatalogId = 4);
				SET @HabilitadoBotonGuardar = 1;
				SET @ObservacionHabilitada = 1;

				-- Validar calificacion obligatoria
					IF
						-- Calificacion Obligatoria para registro auditoria
						((SELECT COUNT(*) FROM RegistrosAuditoria
							WHERE Id = @RegistroAuditoriaId 
							AND Encuesta = 1)
							) > 0

						BEGIN
							SET @CalificacionObligatoriaIPS = 1
						END
						ELSE 
						BEGIN
							SET @CalificacionObligatoriaIPS = 0
						END

						-- Validar si se registro calificacion

						-- Contador calificaciones que deben ser registradas
						DECLARE @CountCalificacionIPS INT = (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rd
							INNER JOIN RegistrosAuditoria ra ON ra.Id = rd.RegistrosAuditoriaId
							INNER JOIN VariableXMedicion vm ON rd.VariableId = vm.VariableId AND vm.Encuesta = 1 AND ra.IdMedicion = vm.MedicionId
							WHERE RegistrosAuditoriaId = @RegistroAuditoriaId
									AND vm.EsCalificable = 1)

						-- Contador calificaciones registradas
						DECLARE @CountCalificacionRegistradasIPS INT = (SELECT COUNT(*) AS Countcalificacion
						FROM (SELECT RegistrosAuditoriaDetalleId FROM RegistroAuditoriaCalificaciones
							WHERE RegistrosAuditoriaId = @RegistroAuditoriaId 
							GROUP BY RegistrosAuditoriaDetalleId) OrdCount)

					IF @CountCalificacionRegistradasIPS < @CountCalificacionIPS 

						BEGIN
							SET @CalificacionIPSRegistrada = 0
						END
						ELSE 
						BEGIN
							SET @CalificacionIPSRegistrada = 1
						END


					-- Validacion si hay NC o ND en glosa
					IF (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rg
						INNER JOIN RegistrosAuditoria ra ON rg.RegistrosAuditoriaId = ra.Id
						INNER JOIN VariableXMedicion vm ON vm.VariableId = rg.VariableId AND ra.IdMedicion = vm.MedicionId
					  WHERE rg.RegistrosAuditoriaId = @RegistroAuditoriaId 
						AND vm.SubGrupoId = @IdItemGlosa -- Glosa
						AND rg.Dato_DC_NC_ND <> @IdItemDC -- DC
							AND vm.EsCalificable = 1
						) > 0

						BEGIN
							SET @HabilitarVariablesCalificar = 0
							SET @ObservacionObligatoria = 1
							SET @CalificacionObligatoriaIPS = 0
							SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1); -- Item tipificacion Glosa
						END
						ELSE 
						BEGIN
							SET @HabilitarVariablesCalificar = 1
							SET @ObservacionObligatoria = 0
							SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'GENERAL' and CatalogId = 1); -- Item tipificacion General

						END

					-- Validacion para observacion registrada
					IF (SELECT COUNT(*) FROM RegistrosAuditoriaDetalleSeguimiento
						WHERE RegistroAuditoriaId = @RegistroAuditoriaId 
						AND TipoObservacion = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1) -- Glosa
						) > 0

						BEGIN
							SET @ObservacionRegistrada = 1
							SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'GENERAL' and CatalogId = 1); -- Item tipificacion General

						END
						ELSE 
						BEGIN
							SET @ObservacionRegistrada = 0

						END

			END

			ELSE IF @BotonAccion = 3 
			BEGIN
				SET @HabilitarBotonEditarCalificacion  = 0;
				SET @HabilitadoBotonGuardar = 1;
				SET @ObservacionHabilitada = 1;
			END

			ELSE 
			BEGIN
				SET @HabilitadoBotonGuardar = 0;
				SET @ObservacionHabilitada = 0;
			END
		
		

		
		-- Consulta final validaciones
		SELECT
		CAST(NEWID() AS NVARCHAR(255)) AS Id,
		@HabilitarVariablesCalificar AS HabilitarVariablesCalificar,
		@VisibleBotonGuardar AS VisibleBotonGuardar,
		@HabilitadoBotonGuardar AS HabilitadoBotonGuardar,
		@VisibleBotonMantenerCalificacion AS VisibleBotonMantenerCalificacion,
		@VisibleBotonEditarCalificacion AS VisibleBotonEditarCalificacion,
		@VisibleBotonComiteExperto AS VisibleBotonComiteExperto,
		@VisibleBotonComiteAdministrativo AS VisibleBotonComiteAdministrativo,
		@VisibleBotonLevantarGlosa AS VisibleBotonLevantarGlosa,
		@ValidarErroresLogica AS ValidarErroresLogica,
		@ObservacionHabilitada AS ObservacionHabilitada,
		@ObservacionObligatoria AS ObservacionObligatoria,
		@ObservacionRegistrada AS ObservacionRegistrada,
		@CalificacionObligatoriaIPS AS CalificacionObligatoriaIPS,
		@CalificacionIPSRegistrada AS CalificacionIPSRegistrada,
		@IdItemGlosa AS IdItemGlosa,
		@IdItemDC AS IdItemDC,
		@IdItemNC AS IdItemNC,
		@TipificacionObservacionDefault  AS TipificacionObservacionDefault,
		@TipificacionObservasionHabilitada AS TipificacionObservasionHabilitada,
		@IdRegistroNuevo AS IdRegistroNuevo,
		@CodigoRegistroPendiente AS CodigoRegistroPendiente,
		@HabilitarBotonMantenerCalificacion AS HabilitarBotonMantenerCalificacion,
		@HabilitarBotonEditarCalificacion AS HabilitarBotonEditarCalificacion,
		@HabilitarBotonComiteExperto AS HabilitarBotonComiteExperto,
		@HabilitarBotonComiteAdministrativo AS HabilitarBotonComiteAdministrativo,
		@HabilitarBotonLevantarGlosa AS HabilitarBotonLevantarGlosa,
		@SolicitarMotivo AS SolicitarMotivo,
		@GuardarCadaCambioVariable AS GuardarCadaCambioVariable,
		@HabilitarGlosa AS HabilitarGlosa,
		@ErroresReportados AS ErroresReportados,
		@VisibleBotonReversar AS VisibleBotonReversar,
		@HabilitadoBotonReversar AS HabilitadoBotonReversar

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Estado_GO2]    Script Date: 10/08/2022 12:04:16 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-02-02
-- Description:	SP para validaciones del resgistro de auditoria cuando el estado es glosa objectada 2
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Validacion_Estado_GO2]
	
	@RegistroAuditoriaId INT,
	@BotonAccion INT,
	@IdUsuario NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		-- Valores respuesta
		DECLARE @HabilitarVariablesCalificar BIT = 0;
		DECLARE @VisibleBotonGuardar BIT = 1;
		DECLARE @HabilitadoBotonGuardar BIT = 0;
		DECLARE @VisibleBotonMantenerCalificacion BIT = 0;
		DECLARE @VisibleBotonEditarCalificacion BIT = 0;
		DECLARE @VisibleBotonComiteExperto BIT = 0;
		DECLARE @VisibleBotonComiteAdministrativo BIT = 0;
		DECLARE @VisibleBotonLevantarGlosa BIT = 0;
		DECLARE @ValidarErroresLogica BIT = 0;
		DECLARE @ObservacionHabilitada BIT = 0;
		DECLARE @ObservacionObligatoria BIT = 0;
		DECLARE @ObservacionRegistrada BIT = 0;
		DECLARE @CalificacionObligatoriaIPS BIT = 0;
		DECLARE @CalificacionIPSRegistrada BIT = 0;
		DECLARE @IdItemGlosa INT = 0; --  0 Para que deshabilite todos
		DECLARE @IdItemDC INT = (SELECT Id FROM Item WHERE ItemName = 'DC' and CatalogId = 6);
		DECLARE @IdItemNC INT = (SELECT Id FROM Item WHERE ItemName = 'NC' and CatalogId = 6);
		DECLARE @TipificacionObservacionDefault INT = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1); -- Item tipificacion General;
		DECLARE @TipificacionObservasionHabilitada BIT = 0;
		DECLARE @IdRegistroNuevo INT = (SELECT Id FROM EstadosRegistroAuditoria WHERE Codigo = 'RN');
		DECLARE @CodigoRegistroPendiente NVARCHAR(255) = (SELECT Codigo FROM EstadosRegistroAuditoria WHERE Id = 17);
		DECLARE @HabilitarBotonMantenerCalificacion BIT = 1;
		DECLARE @HabilitarBotonEditarCalificacion BIT = 1;
		DECLARE @HabilitarBotonComiteExperto BIT = 1;
		DECLARE @HabilitarBotonComiteAdministrativo BIT = 1;
		DECLARE @HabilitarBotonLevantarGlosa BIT = 1;
		DECLARE @CountLevantarGlosa INT = (SELECT LevantarGlosa FROM RegistrosAuditoria WHERE Id = @RegistroAuditoriaId);
		DECLARE @SolicitarMotivo BIT = 0;
		DECLARE @GuardarCadaCambioVariable BIT = 1;
		DECLARE @HabilitarGlosa BIT = 1;
		DECLARE @ErroresReportados BIT = 0;
		--
		DECLARE @VisibleBotonReversar BIT = 1;
		DECLARE @HabilitadoBotonReversar BIT = 1;

		-- Llama SP que valida si hay motivos sin registrar
		EXEC	@SolicitarMotivo = [dbo].[SP_Validacion_Registrar_Motivo]
		@IdRegistroAuditoria = @RegistroAuditoriaId


		--Variables
		--DECLARE @RoleId INT = CAST((SELECT TOP(1) RolId FROM [AUTH].[Usuario] WHERE Id = @IdUsuario) AS INT);
		DECLARE @EsLider BIT = (SELECT R.EsLider FROM [AUTH].[Usuario] US INNER JOIN AspNetRoles R ON (US.RolId = R.Id) WHERE US.Id = @IdUsuario);

			-- Validación Lider
			IF @EsLider = 1 -- Lider
			
			BEGIN
				
				SET @VisibleBotonMantenerCalificacion = 1;
				SET @VisibleBotonEditarCalificacion = 0;
				SET @VisibleBotonComiteExperto = 1;
				SET @VisibleBotonComiteAdministrativo = 1;
				SET @VisibleBotonLevantarGlosa = 1;


				IF @BotonAccion = 2 -- LevantarGlosa
				BEGIN
					SET @HabilitadoBotonGuardar = 1;
					SET @ObservacionHabilitada = 1;	
					SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1); -- Item tipificacion Glosa;
					SET @HabilitarBotonMantenerCalificacion = 0;
					SET @HabilitarBotonEditarCalificacion = 0;
					SET @HabilitarBotonComiteExperto = 0;
					SET @HabilitarBotonComiteAdministrativo = 0;
					SET @HabilitarBotonLevantarGlosa = 0;
				END

				ELSE IF @BotonAccion = 3 -- MantenerCalificacion
				BEGIN
					SET @HabilitadoBotonGuardar = 1;
					SET @ObservacionHabilitada = 1;	
					SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'Mantener calificación' and CatalogId = 1); -- Item tipificacion Glosa;
					SET @HabilitarBotonMantenerCalificacion= 0;
					SET @HabilitarBotonEditarCalificacion = 0;
					SET @HabilitarBotonComiteExperto = 0;
					SET @HabilitarBotonComiteAdministrativo = 0;
					SET @HabilitarBotonLevantarGlosa = 0;
				END

				ELSE IF @BotonAccion = 4 -- Comiteadministrativo
				BEGIN
					SET @HabilitadoBotonGuardar = 1;
					SET @ObservacionHabilitada = 1;	
					SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'Comité Administrativo' and CatalogId = 1); -- Item tipificacion Glosa;
					SET @HabilitarBotonMantenerCalificacion= 0;
					SET @HabilitarBotonEditarCalificacion = 0;
					SET @HabilitarBotonComiteExperto = 0;
					SET @HabilitarBotonComiteAdministrativo = 0;
					SET @HabilitarBotonLevantarGlosa = 0;
				END

				ELSE IF @BotonAccion = 5 -- Comiteexperto
				BEGIN
					SET @HabilitadoBotonGuardar = 1;
					SET @ObservacionHabilitada = 1;	
					SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'Comité Experto' and CatalogId = 1); -- Item tipificacion Glosa;
					SET @HabilitarBotonMantenerCalificacion= 0;
					SET @HabilitarBotonEditarCalificacion = 0;
					SET @HabilitarBotonComiteExperto = 0;
					SET @HabilitarBotonComiteAdministrativo = 0;
					SET @HabilitarBotonLevantarGlosa = 0;
				END


				IF @CountLevantarGlosa > 0
				BEGIN 
					SET @HabilitarBotonMantenerCalificacion= 0;
					SET @HabilitarBotonEditarCalificacion = 0;
					SET @HabilitarBotonComiteExperto = 0;
					SET @HabilitarBotonComiteAdministrativo = 0;
					SET @HabilitarBotonLevantarGlosa = 0;
				END

			END
			
			ELSE IF  @EsLider != 1 AND @CountLevantarGlosa > 0 -- Auditor y glosas levantadas
			BEGIN


				SET @HabilitadoBotonGuardar = 1;
				SET @ObservacionHabilitada = 1;
				SET @IdItemGlosa = (SELECT Id FROM Item WHERE ItemName = 'Glosas' and CatalogId = 4);
				-- Validar calificacion obligatoria
				IF
					-- Calificacion Obligatoria para registro auditoria
					((SELECT COUNT(*) FROM RegistrosAuditoria
						WHERE Id = @RegistroAuditoriaId 
						AND Encuesta = 1)
						) > 0

					BEGIN
						SET @CalificacionObligatoriaIPS = 1
					END
					ELSE 
					BEGIN
						SET @CalificacionObligatoriaIPS = 0
					END

					-- Validar si se registro calificacion

					-- Contador calificaciones que deben ser registradas
					DECLARE @CountCalificacionIPS INT = (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rd
						INNER JOIN RegistrosAuditoria ra ON ra.Id = rd.RegistrosAuditoriaId
						INNER JOIN VariableXMedicion vm ON rd.VariableId = vm.VariableId AND vm.Encuesta = 1 AND ra.IdMedicion = vm.MedicionId
															AND vm.EsCalificable = 1
						WHERE RegistrosAuditoriaId = @RegistroAuditoriaId)

					-- Contador calificaciones registradas
					DECLARE @CountCalificacionRegistradasIPS INT = (SELECT COUNT(*) AS Countcalificacion
					FROM (SELECT RegistrosAuditoriaDetalleId FROM RegistroAuditoriaCalificaciones
						WHERE RegistrosAuditoriaId = @RegistroAuditoriaId 
						GROUP BY RegistrosAuditoriaDetalleId) OrdCount)

				IF @CountCalificacionRegistradasIPS < @CountCalificacionIPS 

					BEGIN
						SET @CalificacionIPSRegistrada = 0
					END
					ELSE 
					BEGIN
						SET @CalificacionIPSRegistrada = 1
					END


				-- Validacion si hay NC o ND en glosa
				IF (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rg
					INNER JOIN RegistrosAuditoria ra ON rg.RegistrosAuditoriaId = ra.Id
					INNER JOIN VariableXMedicion vm ON vm.VariableId = rg.VariableId AND ra.IdMedicion = vm.MedicionId
														AND vm.EsCalificable = 1
				  WHERE rg.RegistrosAuditoriaId = @RegistroAuditoriaId 
					AND vm.SubGrupoId = @IdItemGlosa -- Glosa
					AND rg.Dato_DC_NC_ND <> @IdItemDC -- DC
					) > 0

					BEGIN
						SET @HabilitarVariablesCalificar = 0
						SET @ObservacionObligatoria = 1
						SET @CalificacionObligatoriaIPS = 0
						SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1); -- Item tipificacion Glosa
					END
					ELSE 
					BEGIN
						SET @HabilitarVariablesCalificar = 1
						SET @ObservacionObligatoria = 0
						SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'GENERAL' and CatalogId = 1); -- Item tipificacion General

					END

				-- Validacion para observacion registrada
				IF (SELECT COUNT(*) FROM RegistrosAuditoriaDetalleSeguimiento
					WHERE RegistroAuditoriaId = @RegistroAuditoriaId 
					AND TipoObservacion = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1) -- Glosa
					) > 0

					BEGIN
						SET @ObservacionRegistrada = 1
						SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'GENERAL' and CatalogId = 1); -- Item tipificacion General

					END
					ELSE 
					BEGIN
						SET @ObservacionRegistrada = 0

					END


			END


		




		
		-- Consulta final validaciones
		SELECT
		CAST(NEWID() AS NVARCHAR(255)) AS Id,
		@HabilitarVariablesCalificar AS HabilitarVariablesCalificar,
		@VisibleBotonGuardar AS VisibleBotonGuardar,
		@HabilitadoBotonGuardar AS HabilitadoBotonGuardar,
		@VisibleBotonMantenerCalificacion AS VisibleBotonMantenerCalificacion,
		@VisibleBotonEditarCalificacion AS VisibleBotonEditarCalificacion,
		@VisibleBotonComiteExperto AS VisibleBotonComiteExperto,
		@VisibleBotonComiteAdministrativo AS VisibleBotonComiteAdministrativo,
		@VisibleBotonLevantarGlosa AS VisibleBotonLevantarGlosa,
		@ValidarErroresLogica AS ValidarErroresLogica,
		@ObservacionHabilitada AS ObservacionHabilitada,
		@ObservacionObligatoria AS ObservacionObligatoria,
		@ObservacionRegistrada AS ObservacionRegistrada,
		@CalificacionObligatoriaIPS AS CalificacionObligatoriaIPS,
		@CalificacionIPSRegistrada AS CalificacionIPSRegistrada,
		@IdItemGlosa AS IdItemGlosa,
		@IdItemDC AS IdItemDC,
		@IdItemNC AS IdItemNC,
		@TipificacionObservacionDefault  AS TipificacionObservacionDefault,
		@TipificacionObservasionHabilitada AS TipificacionObservasionHabilitada,
		@IdRegistroNuevo AS IdRegistroNuevo,
		@CodigoRegistroPendiente AS CodigoRegistroPendiente,
		@HabilitarBotonMantenerCalificacion AS HabilitarBotonMantenerCalificacion,
		@HabilitarBotonEditarCalificacion AS HabilitarBotonEditarCalificacion,
		@HabilitarBotonComiteExperto AS HabilitarBotonComiteExperto,
		@HabilitarBotonComiteAdministrativo AS HabilitarBotonComiteAdministrativo,
		@HabilitarBotonLevantarGlosa AS HabilitarBotonLevantarGlosa,
		@SolicitarMotivo AS SolicitarMotivo,
		@GuardarCadaCambioVariable AS GuardarCadaCambioVariable,
		@HabilitarGlosa AS HabilitarGlosa,
		@ErroresReportados AS ErroresReportados,
		@VisibleBotonReversar AS VisibleBotonReversar,
		@HabilitadoBotonReversar AS HabilitadoBotonReversar

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Estado_GRE]    Script Date: 10/08/2022 12:04:16 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-02-14
-- Description:	SP para validaciones del resgistro de auditoria cuando el estado es glosa revision por la entidad GRE
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Validacion_Estado_GRE]
	
	@RegistroAuditoriaId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		-- Valores respuesta
		DECLARE @HabilitarVariablesCalificar BIT = 0;
		DECLARE @VisibleBotonGuardar BIT = 1;
		DECLARE @HabilitadoBotonGuardar BIT = 0;
		DECLARE @VisibleBotonMantenerCalificacion BIT = 0;
		DECLARE @VisibleBotonEditarCalificacion BIT = 0;
		DECLARE @VisibleBotonComiteExperto BIT = 0;
		DECLARE @VisibleBotonComiteAdministrativo BIT = 0;
		DECLARE @VisibleBotonLevantarGlosa BIT = 0;
		DECLARE @ValidarErroresLogica BIT = 1;
		DECLARE @ObservacionHabilitada BIT = 0;
		DECLARE @ObservacionObligatoria BIT = 0;
		DECLARE @ObservacionRegistrada BIT = 0;
		DECLARE @CalificacionObligatoriaIPS BIT = 0;
		DECLARE @CalificacionIPSRegistrada BIT = 0;
		DECLARE @IdItemGlosa INT = 0; --  0 Para que deshabilite todo
		DECLARE @IdItemDC INT = (SELECT Id FROM Item WHERE ItemName = 'DC' and CatalogId = 6);
		DECLARE @IdItemNC INT = (SELECT Id FROM Item WHERE ItemName = 'NC' and CatalogId = 6);
		DECLARE @TipificacionObservacionDefault INT = 0;
		DECLARE @TipificacionObservasionHabilitada BIT = 0;
		DECLARE @IdRegistroNuevo INT = (SELECT Id FROM EstadosRegistroAuditoria WHERE Codigo = 'RN');
		DECLARE @CodigoRegistroPendiente NVARCHAR(255) = (SELECT Codigo FROM EstadosRegistroAuditoria WHERE Id = 17);
		DECLARE @HabilitarBotonMantenerCalificacion BIT = 0;
		DECLARE @HabilitarBotonEditarCalificacion BIT = 0;
		DECLARE @HabilitarBotonComiteExperto BIT = 0;
		DECLARE @HabilitarBotonComiteAdministrativo BIT = 0;
		DECLARE @HabilitarBotonLevantarGlosa BIT = 0;
		DECLARE @SolicitarMotivo BIT = 0;
		DECLARE @GuardarCadaCambioVariable BIT = 1;
		DECLARE @HabilitarGlosa BIT = 0;
		DECLARE @ErroresReportados BIT = 0;
		--
		DECLARE @VisibleBotonReversar BIT = 1;
		DECLARE @HabilitadoBotonReversar BIT = 1;

		-- Llama SP que valida si hay motivos sin registrar
		EXEC	@SolicitarMotivo = [dbo].[SP_Validacion_Registrar_Motivo]
		@IdRegistroAuditoria = @RegistroAuditoriaId


		-- Consulta final validaciones
		SELECT
		CAST(NEWID() AS NVARCHAR(255)) AS Id,
		@HabilitarVariablesCalificar AS HabilitarVariablesCalificar,
		@VisibleBotonGuardar AS VisibleBotonGuardar,
		@HabilitadoBotonGuardar AS HabilitadoBotonGuardar,
		@VisibleBotonMantenerCalificacion AS VisibleBotonMantenerCalificacion,
		@VisibleBotonEditarCalificacion AS VisibleBotonEditarCalificacion,
		@VisibleBotonComiteExperto AS VisibleBotonComiteExperto,
		@VisibleBotonComiteAdministrativo AS VisibleBotonComiteAdministrativo,
		@VisibleBotonLevantarGlosa AS VisibleBotonLevantarGlosa,
		@ValidarErroresLogica AS ValidarErroresLogica,
		@ObservacionHabilitada AS ObservacionHabilitada,
		@ObservacionObligatoria AS ObservacionObligatoria,
		@ObservacionRegistrada AS ObservacionRegistrada,
		@CalificacionObligatoriaIPS AS CalificacionObligatoriaIPS,
		@CalificacionIPSRegistrada AS CalificacionIPSRegistrada,
		@IdItemGlosa AS IdItemGlosa,
		@IdItemDC AS IdItemDC,
		@IdItemNC AS IdItemNC,
		@TipificacionObservacionDefault  AS TipificacionObservacionDefault,
		@TipificacionObservasionHabilitada AS TipificacionObservasionHabilitada,
		@IdRegistroNuevo AS IdRegistroNuevo,
		@CodigoRegistroPendiente AS CodigoRegistroPendiente,
		@HabilitarBotonMantenerCalificacion AS HabilitarBotonMantenerCalificacion,
		@HabilitarBotonEditarCalificacion AS HabilitarBotonEditarCalificacion,
		@HabilitarBotonComiteExperto AS HabilitarBotonComiteExperto,
		@HabilitarBotonComiteAdministrativo AS HabilitarBotonComiteAdministrativo,
		@HabilitarBotonLevantarGlosa AS HabilitarBotonLevantarGlosa,
		@SolicitarMotivo AS SolicitarMotivo,
		@GuardarCadaCambioVariable AS GuardarCadaCambioVariable,
		@HabilitarGlosa AS HabilitarGlosa,
		@ErroresReportados AS ErroresReportados,
		@VisibleBotonReversar AS VisibleBotonReversar,
		@HabilitadoBotonReversar AS HabilitadoBotonReversar

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Estado_H1]    Script Date: 10/08/2022 12:04:16 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-06-06
-- Description:	SP para validaciones del resgistro de auditoria cuando el estado es Hallazgo 1 H1
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Validacion_Estado_H1]
	
	@RegistroAuditoriaId INT,
	@BotonAccion INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		-- Valores respuesta
		DECLARE @HabilitarVariablesCalificar BIT = 0;
		DECLARE @VisibleBotonGuardar BIT = 1;
		DECLARE @HabilitadoBotonGuardar BIT = 0;
		DECLARE @VisibleBotonMantenerCalificacion BIT = 1;
		DECLARE @VisibleBotonEditarCalificacion BIT = 1;
		DECLARE @VisibleBotonComiteExperto BIT = 0;
		DECLARE @VisibleBotonComiteAdministrativo BIT = 0;
		DECLARE @VisibleBotonLevantarGlosa BIT = 0;
		DECLARE @ValidarErroresLogica BIT = 0;
		DECLARE @ObservacionHabilitada BIT = 0;
		DECLARE @ObservacionObligatoria BIT = 0;
		DECLARE @ObservacionRegistrada BIT = 0;
		DECLARE @CalificacionObligatoriaIPS BIT = 0;
		DECLARE @CalificacionIPSRegistrada BIT = 0;
		DECLARE @IdItemGlosa INT = 0; --  0 Para que deshabilite todos
		DECLARE @IdItemDC INT = (SELECT Id FROM Item WHERE ItemName = 'DC' and CatalogId = 6);
		DECLARE @IdItemNC INT = (SELECT Id FROM Item WHERE ItemName = 'NC' and CatalogId = 6);
		DECLARE @TipificacionObservacionDefault INT = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1); -- Item tipificacion General;
		DECLARE @TipificacionObservasionHabilitada BIT = 0;
		DECLARE @IdRegistroNuevo INT = (SELECT Id FROM EstadosRegistroAuditoria WHERE Codigo = 'RN');
		DECLARE @CodigoRegistroPendiente NVARCHAR(255) = (SELECT Codigo FROM EstadosRegistroAuditoria WHERE Id = 17);
		DECLARE @HabilitarBotonMantenerCalificacion BIT = 1;
		DECLARE @HabilitarBotonEditarCalificacion BIT = 1;
		DECLARE @HabilitarBotonComiteExperto BIT = 0;
		DECLARE @HabilitarBotonComiteAdministrativo BIT = 0;
		DECLARE @HabilitarBotonLevantarGlosa BIT = 0;
		DECLARE @SolicitarMotivo BIT = 0;
		DECLARE @GuardarCadaCambioVariable BIT = 1;
		DECLARE @HabilitarGlosa BIT = 0;
		DECLARE @ErroresReportados BIT = 0;
		--
		DECLARE @VisibleBotonReversar BIT = 1;
		DECLARE @HabilitadoBotonReversar BIT = 1;

		-- Llama SP que valida si hay motivos sin registrar
		EXEC	@SolicitarMotivo = [dbo].[SP_Validacion_Registrar_Motivo]
		@IdRegistroAuditoria = @RegistroAuditoriaId

		-- Valida accion editar
			IF @BotonAccion = 1 -- Editar			
			BEGIN
				SET @HabilitarBotonMantenerCalificacion = 0;
				SET @IdItemGlosa = (SELECT Id FROM Item WHERE ItemName = 'Glosas' and CatalogId = 4);
				SET @HabilitadoBotonGuardar = 1;
				SET @ObservacionHabilitada = 1;
				SET @ValidarErroresLogica = 0;
				-- Validar calificacion obligatoria
					IF
						-- Calificacion Obligatoria para registro auditoria
						((SELECT COUNT(*) FROM RegistrosAuditoria
							WHERE Id = @RegistroAuditoriaId 
							AND Encuesta = 1)
							) > 0

						BEGIN
							SET @CalificacionObligatoriaIPS = 1
						END
						ELSE 
						BEGIN
							SET @CalificacionObligatoriaIPS = 0
						END

						-- Validar si se registro calificacion

						-- Contador calificaciones que deben ser registradas
						DECLARE @CountCalificacionIPS INT = (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rd
							INNER JOIN RegistrosAuditoria ra ON ra.Id = rd.RegistrosAuditoriaId
							INNER JOIN VariableXMedicion vm ON rd.VariableId = vm.VariableId AND vm.Encuesta = 1 AND ra.IdMedicion = vm.MedicionId
							WHERE RegistrosAuditoriaId = @RegistroAuditoriaId
									AND vm.EsCalificable = 1)

						-- Contador calificaciones registradas
						DECLARE @CountCalificacionRegistradasIPS INT = (SELECT COUNT(*) AS Countcalificacion
						FROM (SELECT RegistrosAuditoriaDetalleId FROM RegistroAuditoriaCalificaciones
							WHERE RegistrosAuditoriaId = @RegistroAuditoriaId 
							GROUP BY RegistrosAuditoriaDetalleId) OrdCount)

					IF @CountCalificacionRegistradasIPS < @CountCalificacionIPS 

						BEGIN
							SET @CalificacionIPSRegistrada = 0
						END
						ELSE 
						BEGIN
							SET @CalificacionIPSRegistrada = 1
						END


					-- Validacion si hay NC o ND en glosa
					IF (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rg
						INNER JOIN RegistrosAuditoria ra ON rg.RegistrosAuditoriaId = ra.Id
						INNER JOIN VariableXMedicion vm ON vm.VariableId = rg.VariableId AND ra.IdMedicion = vm.MedicionId
					  WHERE rg.RegistrosAuditoriaId = @RegistroAuditoriaId 
						AND vm.SubGrupoId = @IdItemGlosa -- Glosa
						AND rg.Dato_DC_NC_ND <> @IdItemDC -- DC
							AND vm.EsCalificable = 1
						) > 0

						BEGIN
							SET @HabilitarVariablesCalificar = 0
							SET @ObservacionObligatoria = 1
							SET @CalificacionObligatoriaIPS = 0
							SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1); -- Item tipificacion Glosa
						END
						ELSE 
						BEGIN
							SET @HabilitarVariablesCalificar = 1
							SET @ObservacionObligatoria = 0
							SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'GENERAL' and CatalogId = 1); -- Item tipificacion General

						END

					-- Validacion para observacion registrada
					IF (SELECT COUNT(*) FROM RegistrosAuditoriaDetalleSeguimiento
						WHERE RegistroAuditoriaId = @RegistroAuditoriaId 
						AND TipoObservacion = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1) -- Glosa
						) > 0

						BEGIN
							SET @ObservacionRegistrada = 1
							SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'GENERAL' and CatalogId = 1); -- Item tipificacion General

						END
						ELSE 
						BEGIN
							SET @ObservacionRegistrada = 0

						END

			END

			ELSE IF @BotonAccion = 3  -- MantenerCalificacion
			BEGIN
				SET @HabilitarBotonEditarCalificacion  = 0;
				SET @HabilitadoBotonGuardar = 1;
				SET @ObservacionHabilitada = 1;
			END

			ELSE 
			BEGIN
				SET @HabilitadoBotonGuardar = 0;
				SET @ObservacionHabilitada = 0;
			END
		
		

		
		-- Consulta final validaciones
		SELECT
		CAST(NEWID() AS NVARCHAR(255)) AS Id,
		@HabilitarVariablesCalificar AS HabilitarVariablesCalificar,
		@VisibleBotonGuardar AS VisibleBotonGuardar,
		@HabilitadoBotonGuardar AS HabilitadoBotonGuardar,
		@VisibleBotonMantenerCalificacion AS VisibleBotonMantenerCalificacion,
		@VisibleBotonEditarCalificacion AS VisibleBotonEditarCalificacion,
		@VisibleBotonComiteExperto AS VisibleBotonComiteExperto,
		@VisibleBotonComiteAdministrativo AS VisibleBotonComiteAdministrativo,
		@VisibleBotonLevantarGlosa AS VisibleBotonLevantarGlosa,
		@ValidarErroresLogica AS ValidarErroresLogica,
		@ObservacionHabilitada AS ObservacionHabilitada,
		@ObservacionObligatoria AS ObservacionObligatoria,
		@ObservacionRegistrada AS ObservacionRegistrada,
		@CalificacionObligatoriaIPS AS CalificacionObligatoriaIPS,
		@CalificacionIPSRegistrada AS CalificacionIPSRegistrada,
		@IdItemGlosa AS IdItemGlosa,
		@IdItemDC AS IdItemDC,
		@IdItemNC AS IdItemNC,
		@TipificacionObservacionDefault  AS TipificacionObservacionDefault,
		@TipificacionObservasionHabilitada AS TipificacionObservasionHabilitada,
		@IdRegistroNuevo AS IdRegistroNuevo,
		@CodigoRegistroPendiente AS CodigoRegistroPendiente,
		@HabilitarBotonMantenerCalificacion AS HabilitarBotonMantenerCalificacion,
		@HabilitarBotonEditarCalificacion AS HabilitarBotonEditarCalificacion,
		@HabilitarBotonComiteExperto AS HabilitarBotonComiteExperto,
		@HabilitarBotonComiteAdministrativo AS HabilitarBotonComiteAdministrativo,
		@HabilitarBotonLevantarGlosa AS HabilitarBotonLevantarGlosa,
		@SolicitarMotivo AS SolicitarMotivo,
		@GuardarCadaCambioVariable AS GuardarCadaCambioVariable,
		@HabilitarGlosa AS HabilitarGlosa,
		@ErroresReportados AS ErroresReportados,
		@VisibleBotonReversar AS VisibleBotonReversar,
		@HabilitadoBotonReversar AS HabilitadoBotonReversar

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Estado_H2A]    Script Date: 10/08/2022 12:04:16 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-06-06
-- Description:	SP para validaciones del resgistro de auditoria cuando el estado es Hallazgo 2 Auditor
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Validacion_Estado_H2A]
	
	@RegistroAuditoriaId INT,
	@BotonAccion INT,
	@IdUsuario NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		-- Valores respuesta
		DECLARE @HabilitarVariablesCalificar BIT = 0;
		DECLARE @VisibleBotonGuardar BIT = 1;
		DECLARE @HabilitadoBotonGuardar BIT = 0;
		DECLARE @VisibleBotonMantenerCalificacion BIT = 0;
		DECLARE @VisibleBotonEditarCalificacion BIT = 1;
		DECLARE @VisibleBotonComiteExperto BIT = 0;
		DECLARE @VisibleBotonComiteAdministrativo BIT = 0;
		DECLARE @VisibleBotonLevantarGlosa BIT = 0;
		DECLARE @ValidarErroresLogica BIT = 0;
		DECLARE @ObservacionHabilitada BIT = 0;
		DECLARE @ObservacionObligatoria BIT = 0;
		DECLARE @ObservacionRegistrada BIT = 0;
		DECLARE @CalificacionObligatoriaIPS BIT = 0;
		DECLARE @CalificacionIPSRegistrada BIT = 0;
		DECLARE @IdItemGlosa INT = 0; --  0 Para que deshabilite todos
		DECLARE @IdItemDC INT = (SELECT Id FROM Item WHERE ItemName = 'DC' and CatalogId = 6);
		DECLARE @IdItemNC INT = (SELECT Id FROM Item WHERE ItemName = 'NC' and CatalogId = 6);
		DECLARE @TipificacionObservacionDefault INT = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1); -- Item tipificacion General;
		DECLARE @TipificacionObservasionHabilitada BIT = 0;
		DECLARE @IdRegistroNuevo INT = (SELECT Id FROM EstadosRegistroAuditoria WHERE Codigo = 'RN');
		DECLARE @CodigoRegistroPendiente NVARCHAR(255) = (SELECT Codigo FROM EstadosRegistroAuditoria WHERE Id = 17);
		DECLARE @HabilitarBotonMantenerCalificacion BIT = 0;
		DECLARE @HabilitarBotonEditarCalificacion BIT = 1;
		DECLARE @HabilitarBotonComiteExperto BIT = 0;
		DECLARE @HabilitarBotonComiteAdministrativo BIT = 0;
		DECLARE @HabilitarBotonLevantarGlosa BIT = 0;
		DECLARE @SolicitarMotivo BIT = 0;
		DECLARE @GuardarCadaCambioVariable BIT = 1;
		DECLARE @HabilitarGlosa BIT = 0;
		DECLARE @ErroresReportados BIT = 0;
		--
		DECLARE @VisibleBotonReversar BIT = 1;
		DECLARE @HabilitadoBotonReversar BIT = 1;

		-- Llama SP que valida si hay motivos sin registrar
		EXEC	@SolicitarMotivo = [dbo].[SP_Validacion_Registrar_Motivo]
		@IdRegistroAuditoria = @RegistroAuditoriaId

		

			IF @BotonAccion = 1 -- Editar			
			BEGIN
				SET @HabilitarBotonMantenerCalificacion = 0;
				SET @IdItemGlosa = (SELECT Id FROM Item WHERE ItemName = 'Glosas' and CatalogId = 4);
				SET @HabilitadoBotonGuardar = 1;
				SET @ObservacionHabilitada = 1;
				SET @ValidarErroresLogica = 0;
				-- Validar calificacion obligatoria
					IF
						-- Calificacion Obligatoria para registro auditoria
						((SELECT COUNT(*) FROM RegistrosAuditoria
							WHERE Id = @RegistroAuditoriaId 
							AND Encuesta = 1)
							) > 0

						BEGIN
							SET @CalificacionObligatoriaIPS = 1
						END
						ELSE 
						BEGIN
							SET @CalificacionObligatoriaIPS = 0
						END

						-- Validar si se registro calificacion

						-- Contador calificaciones que deben ser registradas
						DECLARE @CountCalificacionIPS INT = (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rd
							INNER JOIN RegistrosAuditoria ra ON ra.Id = rd.RegistrosAuditoriaId
							INNER JOIN VariableXMedicion vm ON rd.VariableId = vm.VariableId AND vm.Encuesta = 1 AND ra.IdMedicion = vm.MedicionId
							WHERE RegistrosAuditoriaId = @RegistroAuditoriaId
									AND vm.EsCalificable = 1)

						-- Contador calificaciones registradas
						DECLARE @CountCalificacionRegistradasIPS INT = (SELECT COUNT(*) AS Countcalificacion
						FROM (SELECT RegistrosAuditoriaDetalleId FROM RegistroAuditoriaCalificaciones
							WHERE RegistrosAuditoriaId = @RegistroAuditoriaId 
							GROUP BY RegistrosAuditoriaDetalleId) OrdCount)

					IF @CountCalificacionRegistradasIPS < @CountCalificacionIPS 

						BEGIN
							SET @CalificacionIPSRegistrada = 0
						END
						ELSE 
						BEGIN
							SET @CalificacionIPSRegistrada = 1
						END


					-- Validacion para observacion registrada
					IF (SELECT COUNT(*) FROM RegistrosAuditoriaDetalleSeguimiento
						WHERE RegistroAuditoriaId = @RegistroAuditoriaId 
						AND TipoObservacion = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1) -- Glosa
						) > 0

						BEGIN
							SET @ObservacionRegistrada = 1
							SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'GENERAL' and CatalogId = 1); -- Item tipificacion General

						END
						ELSE 
						BEGIN
							SET @ObservacionRegistrada = 0

						END

			END

			ELSE IF @BotonAccion = 3  -- MantenerCalificacion
			BEGIN
				SET @HabilitarBotonEditarCalificacion  = 0;
				SET @HabilitadoBotonGuardar = 1;
				SET @ObservacionHabilitada = 1;
			END

			ELSE 
			BEGIN
				SET @HabilitadoBotonGuardar = 0;
				SET @ObservacionHabilitada = 0;
			END
		
		

		
		-- Consulta final validaciones
		SELECT
		CAST(NEWID() AS NVARCHAR(255)) AS Id,
		@HabilitarVariablesCalificar AS HabilitarVariablesCalificar,
		@VisibleBotonGuardar AS VisibleBotonGuardar,
		@HabilitadoBotonGuardar AS HabilitadoBotonGuardar,
		@VisibleBotonMantenerCalificacion AS VisibleBotonMantenerCalificacion,
		@VisibleBotonEditarCalificacion AS VisibleBotonEditarCalificacion,
		@VisibleBotonComiteExperto AS VisibleBotonComiteExperto,
		@VisibleBotonComiteAdministrativo AS VisibleBotonComiteAdministrativo,
		@VisibleBotonLevantarGlosa AS VisibleBotonLevantarGlosa,
		@ValidarErroresLogica AS ValidarErroresLogica,
		@ObservacionHabilitada AS ObservacionHabilitada,
		@ObservacionObligatoria AS ObservacionObligatoria,
		@ObservacionRegistrada AS ObservacionRegistrada,
		@CalificacionObligatoriaIPS AS CalificacionObligatoriaIPS,
		@CalificacionIPSRegistrada AS CalificacionIPSRegistrada,
		@IdItemGlosa AS IdItemGlosa,
		@IdItemDC AS IdItemDC,
		@IdItemNC AS IdItemNC,
		@TipificacionObservacionDefault  AS TipificacionObservacionDefault,
		@TipificacionObservasionHabilitada AS TipificacionObservasionHabilitada,
		@IdRegistroNuevo AS IdRegistroNuevo,
		@CodigoRegistroPendiente AS CodigoRegistroPendiente,
		@HabilitarBotonMantenerCalificacion AS HabilitarBotonMantenerCalificacion,
		@HabilitarBotonEditarCalificacion AS HabilitarBotonEditarCalificacion,
		@HabilitarBotonComiteExperto AS HabilitarBotonComiteExperto,
		@HabilitarBotonComiteAdministrativo AS HabilitarBotonComiteAdministrativo,
		@HabilitarBotonLevantarGlosa AS HabilitarBotonLevantarGlosa,
		@SolicitarMotivo AS SolicitarMotivo,
		@GuardarCadaCambioVariable AS GuardarCadaCambioVariable,
		@HabilitarGlosa AS HabilitarGlosa,
		@ErroresReportados AS ErroresReportados,
		@VisibleBotonReversar AS VisibleBotonReversar,
		@HabilitadoBotonReversar AS HabilitadoBotonReversar

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Estado_H2L]    Script Date: 10/08/2022 12:04:16 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-06-06
-- Description:	SP para validaciones del resgistro de auditoria cuando el estado es Hallazgo 2 lider
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Validacion_Estado_H2L]
	
	@RegistroAuditoriaId INT,
	@BotonAccion INT,
	@IdUsuario NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		-- Valores respuesta
		DECLARE @HabilitarVariablesCalificar BIT = 0;
		DECLARE @VisibleBotonGuardar BIT = 1;
		DECLARE @HabilitadoBotonGuardar BIT = 0;
		DECLARE @VisibleBotonMantenerCalificacion BIT = 1;
		DECLARE @VisibleBotonEditarCalificacion BIT = 1;
		DECLARE @VisibleBotonComiteExperto BIT = 0;
		DECLARE @VisibleBotonComiteAdministrativo BIT = 0;
		DECLARE @VisibleBotonLevantarGlosa BIT = 0;
		DECLARE @ValidarErroresLogica BIT = 0;
		DECLARE @ObservacionHabilitada BIT = 0;
		DECLARE @ObservacionObligatoria BIT = 0;
		DECLARE @ObservacionRegistrada BIT = 0;
		DECLARE @CalificacionObligatoriaIPS BIT = 0;
		DECLARE @CalificacionIPSRegistrada BIT = 0;
		DECLARE @IdItemGlosa INT = 0; --  0 Para que deshabilite todos
		DECLARE @IdItemDC INT = (SELECT Id FROM Item WHERE ItemName = 'DC' and CatalogId = 6);
		DECLARE @IdItemNC INT = (SELECT Id FROM Item WHERE ItemName = 'NC' and CatalogId = 6);
		DECLARE @TipificacionObservacionDefault INT = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1); -- Item tipificacion General;
		DECLARE @TipificacionObservasionHabilitada BIT = 0;
		DECLARE @IdRegistroNuevo INT = (SELECT Id FROM EstadosRegistroAuditoria WHERE Codigo = 'RN');
		DECLARE @CodigoRegistroPendiente NVARCHAR(255) = (SELECT Codigo FROM EstadosRegistroAuditoria WHERE Id = 17);
		DECLARE @HabilitarBotonMantenerCalificacion BIT = 1;
		DECLARE @HabilitarBotonEditarCalificacion BIT = 1;
		DECLARE @HabilitarBotonComiteExperto BIT = 0;
		DECLARE @HabilitarBotonComiteAdministrativo BIT = 0;
		DECLARE @HabilitarBotonLevantarGlosa BIT = 0;
		DECLARE @SolicitarMotivo BIT = 0;
		DECLARE @GuardarCadaCambioVariable BIT = 1;
		DECLARE @HabilitarGlosa BIT = 0;
		DECLARE @ErroresReportados BIT = 0;
		--
		DECLARE @VisibleBotonReversar BIT = 1;
		DECLARE @HabilitadoBotonReversar BIT = 1;

		-- Llama SP que valida si hay motivos sin registrar
		EXEC	@SolicitarMotivo = [dbo].[SP_Validacion_Registrar_Motivo]
		@IdRegistroAuditoria = @RegistroAuditoriaId

		
		--Variables
		--DECLARE @RoleId INT = CAST((SELECT TOP(1) RolId FROM [AUTH].[Usuario] WHERE Id = @IdUsuario) AS INT);
		DECLARE @EsLider BIT = (SELECT R.EsLider FROM [AUTH].[Usuario] US INNER JOIN AspNetRoles R ON (US.RolId = R.Id) WHERE US.Id = @IdUsuario);

			-- Validación Lider
			IF @EsLider != 1 -- Lider
			BEGIN
				SET @HabilitarBotonMantenerCalificacion = 0;
				SET @HabilitadoBotonGuardar = 0;
				SET @ObservacionHabilitada = 0;
				SET @HabilitarBotonEditarCalificacion = 0;
			END
			-- Valida accion editar
			ELSE IF @BotonAccion = 1 -- Editar			
			BEGIN
				SET @HabilitarBotonMantenerCalificacion = 0;
				SET @IdItemGlosa = (SELECT Id FROM Item WHERE ItemName = 'Glosas' and CatalogId = 4);
				SET @HabilitadoBotonGuardar = 1;
				SET @ObservacionHabilitada = 1;
				SET @ValidarErroresLogica = 0;
				-- Validar calificacion obligatoria
					IF
						-- Calificacion Obligatoria para registro auditoria
						((SELECT COUNT(*) FROM RegistrosAuditoria
							WHERE Id = @RegistroAuditoriaId 
							AND Encuesta = 1)
							) > 0

						BEGIN
							SET @CalificacionObligatoriaIPS = 1
						END
						ELSE 
						BEGIN
							SET @CalificacionObligatoriaIPS = 0
						END

						-- Validar si se registro calificacion

						-- Contador calificaciones que deben ser registradas
						DECLARE @CountCalificacionIPS INT = (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rd
							INNER JOIN RegistrosAuditoria ra ON ra.Id = rd.RegistrosAuditoriaId
							INNER JOIN VariableXMedicion vm ON rd.VariableId = vm.VariableId AND vm.Encuesta = 1 AND ra.IdMedicion = vm.MedicionId
							WHERE RegistrosAuditoriaId = @RegistroAuditoriaId
									AND vm.EsCalificable = 1)

						-- Contador calificaciones registradas
						DECLARE @CountCalificacionRegistradasIPS INT = (SELECT COUNT(*) AS Countcalificacion
						FROM (SELECT RegistrosAuditoriaDetalleId FROM RegistroAuditoriaCalificaciones
							WHERE RegistrosAuditoriaId = @RegistroAuditoriaId 
							GROUP BY RegistrosAuditoriaDetalleId) OrdCount)

					IF @CountCalificacionRegistradasIPS < @CountCalificacionIPS 

						BEGIN
							SET @CalificacionIPSRegistrada = 0
						END
						ELSE 
						BEGIN
							SET @CalificacionIPSRegistrada = 1
						END


					-- Validacion si hay NC o ND en glosa
					IF (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rg
						INNER JOIN RegistrosAuditoria ra ON rg.RegistrosAuditoriaId = ra.Id
						INNER JOIN VariableXMedicion vm ON vm.VariableId = rg.VariableId AND ra.IdMedicion = vm.MedicionId
					  WHERE rg.RegistrosAuditoriaId = @RegistroAuditoriaId 
						AND vm.SubGrupoId = @IdItemGlosa -- Glosa
						AND rg.Dato_DC_NC_ND <> @IdItemDC -- DC
							AND vm.EsCalificable = 1
						) > 0

						BEGIN
							SET @HabilitarVariablesCalificar = 0
							SET @ObservacionObligatoria = 1
							SET @CalificacionObligatoriaIPS = 0
							SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1); -- Item tipificacion Glosa
						END
						ELSE 
						BEGIN
							SET @HabilitarVariablesCalificar = 1
							SET @ObservacionObligatoria = 0
							SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'GENERAL' and CatalogId = 1); -- Item tipificacion General

						END

					-- Validacion para observacion registrada
					IF (SELECT COUNT(*) FROM RegistrosAuditoriaDetalleSeguimiento
						WHERE RegistroAuditoriaId = @RegistroAuditoriaId 
						AND TipoObservacion = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1) -- Glosa
						) > 0

						BEGIN
							SET @ObservacionRegistrada = 1
							SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'GENERAL' and CatalogId = 1); -- Item tipificacion General

						END
						ELSE 
						BEGIN
							SET @ObservacionRegistrada = 0

						END

			END

			ELSE IF @BotonAccion = 3  -- MantenerCalificacion
			BEGIN
				SET @HabilitarBotonEditarCalificacion  = 0;
				SET @HabilitadoBotonGuardar = 1;
				SET @ObservacionHabilitada = 1;
			END

			ELSE 
			BEGIN
				SET @HabilitadoBotonGuardar = 0;
				SET @ObservacionHabilitada = 0;
			END
		
		

		
		-- Consulta final validaciones
		SELECT
		CAST(NEWID() AS NVARCHAR(255)) AS Id,
		@HabilitarVariablesCalificar AS HabilitarVariablesCalificar,
		@VisibleBotonGuardar AS VisibleBotonGuardar,
		@HabilitadoBotonGuardar AS HabilitadoBotonGuardar,
		@VisibleBotonMantenerCalificacion AS VisibleBotonMantenerCalificacion,
		@VisibleBotonEditarCalificacion AS VisibleBotonEditarCalificacion,
		@VisibleBotonComiteExperto AS VisibleBotonComiteExperto,
		@VisibleBotonComiteAdministrativo AS VisibleBotonComiteAdministrativo,
		@VisibleBotonLevantarGlosa AS VisibleBotonLevantarGlosa,
		@ValidarErroresLogica AS ValidarErroresLogica,
		@ObservacionHabilitada AS ObservacionHabilitada,
		@ObservacionObligatoria AS ObservacionObligatoria,
		@ObservacionRegistrada AS ObservacionRegistrada,
		@CalificacionObligatoriaIPS AS CalificacionObligatoriaIPS,
		@CalificacionIPSRegistrada AS CalificacionIPSRegistrada,
		@IdItemGlosa AS IdItemGlosa,
		@IdItemDC AS IdItemDC,
		@IdItemNC AS IdItemNC,
		@TipificacionObservacionDefault  AS TipificacionObservacionDefault,
		@TipificacionObservasionHabilitada AS TipificacionObservasionHabilitada,
		@IdRegistroNuevo AS IdRegistroNuevo,
		@CodigoRegistroPendiente AS CodigoRegistroPendiente,
		@HabilitarBotonMantenerCalificacion AS HabilitarBotonMantenerCalificacion,
		@HabilitarBotonEditarCalificacion AS HabilitarBotonEditarCalificacion,
		@HabilitarBotonComiteExperto AS HabilitarBotonComiteExperto,
		@HabilitarBotonComiteAdministrativo AS HabilitarBotonComiteAdministrativo,
		@HabilitarBotonLevantarGlosa AS HabilitarBotonLevantarGlosa,
		@SolicitarMotivo AS SolicitarMotivo,
		@GuardarCadaCambioVariable AS GuardarCadaCambioVariable,
		@HabilitarGlosa AS HabilitarGlosa,
		@ErroresReportados AS ErroresReportados,
		@VisibleBotonReversar AS VisibleBotonReversar,
		@HabilitadoBotonReversar AS HabilitadoBotonReversar

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Estado_RC]    Script Date: 10/08/2022 12:04:16 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-06-06
-- Description:	SP para validaciones del resgistro de auditoria cuando el estado es Registro Cerrado
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Validacion_Estado_RC]
	
	@RegistroAuditoriaId INT,
	@BotonAccion INT,
	@IdUsuario NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		-- Valores respuesta
		DECLARE @HabilitarVariablesCalificar BIT = 0;
		DECLARE @VisibleBotonGuardar BIT = 0;
		DECLARE @HabilitadoBotonGuardar BIT = 0;
		DECLARE @VisibleBotonMantenerCalificacion BIT = 0;
		DECLARE @VisibleBotonEditarCalificacion BIT = 0;
		DECLARE @VisibleBotonComiteExperto BIT = 0;
		DECLARE @VisibleBotonComiteAdministrativo BIT = 0;
		DECLARE @VisibleBotonLevantarGlosa BIT = 0;
		DECLARE @ValidarErroresLogica BIT = 0;
		DECLARE @ObservacionHabilitada BIT = 0;
		DECLARE @ObservacionObligatoria BIT = 0;
		DECLARE @ObservacionRegistrada BIT = 0;
		DECLARE @CalificacionObligatoriaIPS BIT = 0;
		DECLARE @CalificacionIPSRegistrada BIT = 0;
		DECLARE @IdItemGlosa INT = 0; --  0 Para que deshabilite todos
		DECLARE @IdItemDC INT = (SELECT Id FROM Item WHERE ItemName = 'DC' and CatalogId = 6);
		DECLARE @IdItemNC INT = (SELECT Id FROM Item WHERE ItemName = 'NC' and CatalogId = 6);
		DECLARE @TipificacionObservacionDefault INT = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1); -- Item tipificacion General;
		DECLARE @TipificacionObservasionHabilitada BIT = 0;
		DECLARE @IdRegistroNuevo INT = (SELECT Id FROM EstadosRegistroAuditoria WHERE Codigo = 'RN');
		DECLARE @CodigoRegistroPendiente NVARCHAR(255) = (SELECT Codigo FROM EstadosRegistroAuditoria WHERE Id = 17);
		DECLARE @HabilitarBotonMantenerCalificacion BIT = 0;
		DECLARE @HabilitarBotonEditarCalificacion BIT = 0;
		DECLARE @HabilitarBotonComiteExperto BIT = 0;
		DECLARE @HabilitarBotonComiteAdministrativo BIT = 0;
		DECLARE @HabilitarBotonLevantarGlosa BIT = 0;
		DECLARE @SolicitarMotivo BIT = 0;
		DECLARE @GuardarCadaCambioVariable BIT = 0;
		DECLARE @HabilitarGlosa BIT = 0;
		DECLARE @ErroresReportados BIT = 0;
		--
		DECLARE @VisibleBotonReversar BIT = 1;
		DECLARE @HabilitadoBotonReversar BIT = 1;

		--Variables
		--DECLARE @RoleId INT = CAST((SELECT TOP(1) RolId FROM [AUTH].[Usuario] WHERE Id = @IdUsuario) AS INT);
		DECLARE @EsLider BIT = (SELECT R.EsLider FROM [AUTH].[Usuario] US INNER JOIN AspNetRoles R ON (US.RolId = R.Id) WHERE US.Id = @IdUsuario);

			
			IF @EsLider = 1 -- Editar AND Lider
			BEGIN
				SET @HabilitadoBotonGuardar = 1;
				SET @VisibleBotonGuardar = 1;
			END

		
		

		
		-- Consulta final validaciones
		SELECT
		CAST(NEWID() AS NVARCHAR(255)) AS Id,
		@HabilitarVariablesCalificar AS HabilitarVariablesCalificar,
		@VisibleBotonGuardar AS VisibleBotonGuardar,
		@HabilitadoBotonGuardar AS HabilitadoBotonGuardar,
		@VisibleBotonMantenerCalificacion AS VisibleBotonMantenerCalificacion,
		@VisibleBotonEditarCalificacion AS VisibleBotonEditarCalificacion,
		@VisibleBotonComiteExperto AS VisibleBotonComiteExperto,
		@VisibleBotonComiteAdministrativo AS VisibleBotonComiteAdministrativo,
		@VisibleBotonLevantarGlosa AS VisibleBotonLevantarGlosa,
		@ValidarErroresLogica AS ValidarErroresLogica,
		@ObservacionHabilitada AS ObservacionHabilitada,
		@ObservacionObligatoria AS ObservacionObligatoria,
		@ObservacionRegistrada AS ObservacionRegistrada,
		@CalificacionObligatoriaIPS AS CalificacionObligatoriaIPS,
		@CalificacionIPSRegistrada AS CalificacionIPSRegistrada,
		@IdItemGlosa AS IdItemGlosa,
		@IdItemDC AS IdItemDC,
		@IdItemNC AS IdItemNC,
		@TipificacionObservacionDefault  AS TipificacionObservacionDefault,
		@TipificacionObservasionHabilitada AS TipificacionObservasionHabilitada,
		@IdRegistroNuevo AS IdRegistroNuevo,
		@CodigoRegistroPendiente AS CodigoRegistroPendiente,
		@HabilitarBotonMantenerCalificacion AS HabilitarBotonMantenerCalificacion,
		@HabilitarBotonEditarCalificacion AS HabilitarBotonEditarCalificacion,
		@HabilitarBotonComiteExperto AS HabilitarBotonComiteExperto,
		@HabilitarBotonComiteAdministrativo AS HabilitarBotonComiteAdministrativo,
		@HabilitarBotonLevantarGlosa AS HabilitarBotonLevantarGlosa,
		@SolicitarMotivo AS SolicitarMotivo,
		@GuardarCadaCambioVariable AS GuardarCadaCambioVariable,
		@HabilitarGlosa AS HabilitarGlosa,
		@ErroresReportados AS ErroresReportados,
		@VisibleBotonReversar AS VisibleBotonReversar,
		@HabilitadoBotonReversar AS HabilitadoBotonReversar

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Estado_RN]    Script Date: 10/08/2022 12:04:16 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-02-02
-- Description:	SP para validaciones del resgistro de auditoria cuando el estado es registro nuevo
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Validacion_Estado_RN]
	
	@RegistroAuditoriaId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		-- Valores respuesta
		DECLARE @HabilitarVariablesCalificar BIT = 0;
		DECLARE @VisibleBotonGuardar BIT = 1;
		DECLARE @HabilitadoBotonGuardar BIT = 1;
		DECLARE @VisibleBotonMantenerCalificacion BIT = 0;
		DECLARE @VisibleBotonEditarCalificacion BIT = 0;
		DECLARE @VisibleBotonComiteExperto BIT = 0;
		DECLARE @VisibleBotonComiteAdministrativo BIT = 0;
		DECLARE @VisibleBotonLevantarGlosa BIT = 0;
		DECLARE @ValidarErroresLogica BIT = 0;
		DECLARE @ObservacionHabilitada BIT = 1;
		DECLARE @ObservacionObligatoria BIT = 0;
		DECLARE @ObservacionRegistrada BIT = 0;
		DECLARE @CalificacionObligatoriaIPS BIT = 0;
		DECLARE @CalificacionIPSRegistrada BIT = 0;
		DECLARE @IdItemGlosa INT = (SELECT Id FROM Item WHERE ItemName = 'Glosas' and CatalogId = 4);
		DECLARE @IdItemDC INT = (SELECT Id FROM Item WHERE ItemName = 'DC' and CatalogId = 6);
		DECLARE @IdItemNC INT = (SELECT Id FROM Item WHERE ItemName = 'NC' and CatalogId = 6);
		DECLARE @TipificacionObservacionDefault INT = 0;
		DECLARE @TipificacionObservasionHabilitada BIT = 0;
		DECLARE @IdRegistroNuevo INT = (SELECT Id FROM EstadosRegistroAuditoria WHERE Codigo = 'RN');
		DECLARE @CodigoRegistroPendiente NVARCHAR(255) = (SELECT Codigo FROM EstadosRegistroAuditoria WHERE Id = 17);
		DECLARE @HabilitarBotonMantenerCalificacion BIT = 0;
		DECLARE @HabilitarBotonEditarCalificacion BIT = 0;
		DECLARE @HabilitarBotonComiteExperto BIT = 0;
		DECLARE @HabilitarBotonComiteAdministrativo BIT = 0;
		DECLARE @HabilitarBotonLevantarGlosa BIT = 0;
		DECLARE @SolicitarMotivo BIT = 0;
		DECLARE @GuardarCadaCambioVariable BIT = 1;
		DECLARE @HabilitarGlosa BIT = 1;
		DECLARE @ErroresReportados BIT = 0;
		--
		DECLARE @VisibleBotonReversar BIT = 0;
		DECLARE @HabilitadoBotonReversar BIT = 0;

		-- Llama SP que valida si hay motivos sin registrar
		EXEC	@SolicitarMotivo = [dbo].[SP_Validacion_Registrar_Motivo]
		@IdRegistroAuditoria = @RegistroAuditoriaId

		-- Validar calificacion obligatoria
		IF
			-- Calificacion Obligatoria para registro auditoria
			((SELECT COUNT(*) FROM RegistrosAuditoria
			WHERE Id = @RegistroAuditoriaId 
			AND Encuesta = 1)
			) > 0

			BEGIN
				SET @CalificacionObligatoriaIPS = 1
			END
			ELSE 
			BEGIN
				SET @CalificacionObligatoriaIPS = 0
			END

			-- Validar si se registro calificacion

			-- Contador calificaciones que deben ser registradas
			DECLARE @CountCalificacionIPS INT = (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rd
				INNER JOIN RegistrosAuditoria ra ON ra.Id = rd.RegistrosAuditoriaId
				INNER JOIN VariableXMedicion vm ON rd.VariableId = vm.VariableId AND vm.Encuesta = 1 AND ra.IdMedicion = vm.MedicionId
				WHERE RegistrosAuditoriaId = @RegistroAuditoriaId
					  AND vm.EsCalificable = 1)

			-- Contador calificaciones registradas
			DECLARE @CountCalificacionRegistradasIPS INT = (SELECT COUNT(*) AS Countcalificacion
			FROM (SELECT RegistrosAuditoriaDetalleId FROM RegistroAuditoriaCalificaciones
				WHERE RegistrosAuditoriaId = @RegistroAuditoriaId 
				GROUP BY RegistrosAuditoriaDetalleId) OrdCount)

		IF @CountCalificacionRegistradasIPS < @CountCalificacionIPS 

			BEGIN
				SET @CalificacionIPSRegistrada = 0
			END
			ELSE 
			BEGIN
				SET @CalificacionIPSRegistrada = 1
			END


		-- Validacion si hay NC o ND en glosa
		IF (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rg
			INNER JOIN RegistrosAuditoria ra ON rg.RegistrosAuditoriaId = ra.Id
			INNER JOIN VariableXMedicion vm ON vm.VariableId = rg.VariableId AND ra.IdMedicion = vm.MedicionId
		  WHERE rg.RegistrosAuditoriaId = @RegistroAuditoriaId 
			AND vm.SubGrupoId = @IdItemGlosa -- Glosa
			AND rg.Dato_DC_NC_ND <> @IdItemDC -- DC
			AND vm.EsCalificable = 1
			) > 0

			BEGIN
				SET @HabilitarVariablesCalificar = 0
				SET @ObservacionObligatoria = 1
				SET @CalificacionObligatoriaIPS = 0
				SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1); -- Item tipificacion Glosa
				SET @ValidarErroresLogica = 0;
				SET @ErroresReportados = 0;
			END
			ELSE 
			BEGIN
				SET @HabilitarVariablesCalificar = 1
				SET @ObservacionObligatoria = 0
				SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'GENERAL' and CatalogId = 1); -- Item tipificacion General

				--Validar Errores de logica 
				SET @ValidarErroresLogica = 1;

				IF (SELECT COUNT(*) FROM RegistrosAuditoriaDetalleSeguimiento
						WHERE RegistroAuditoriaId = @RegistroAuditoriaId 
						AND TipoObservacion = (SELECT Id FROM Item WHERE ItemName = 'Error lógica' and CatalogId = 1) -- Glosa
						) > 0

						BEGIN
							SET @ErroresReportados = 1
							--SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'GENERAL' and CatalogId = 1); -- Item tipificacion General

						END
						ELSE 
						BEGIN
							SET @ErroresReportados = 0
							--SET @CalificacionObligatoriaIPS = 0

						END

			END

		-- Validacion para observacion registrada
		IF (SELECT COUNT(*) FROM RegistrosAuditoriaDetalleSeguimiento
			WHERE RegistroAuditoriaId = @RegistroAuditoriaId 
			AND TipoObservacion = (SELECT Id FROM Item WHERE ItemName = 'Glosa' and CatalogId = 1) -- Glosa
			) > 0

			BEGIN
				SET @ObservacionRegistrada = 1
				SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'GENERAL' and CatalogId = 1); -- Item tipificacion General

			END
			ELSE 
			BEGIN
				SET @ObservacionRegistrada = 0

			END

		

		-- Consulta final validaciones
		SELECT
		CAST(NEWID() AS NVARCHAR(255)) AS Id,
		@HabilitarVariablesCalificar AS HabilitarVariablesCalificar,
		@VisibleBotonGuardar AS VisibleBotonGuardar,
		@HabilitadoBotonGuardar AS HabilitadoBotonGuardar,
		@VisibleBotonMantenerCalificacion AS VisibleBotonMantenerCalificacion,
		@VisibleBotonEditarCalificacion AS VisibleBotonEditarCalificacion,
		@VisibleBotonComiteExperto AS VisibleBotonComiteExperto,
		@VisibleBotonComiteAdministrativo AS VisibleBotonComiteAdministrativo,
		@VisibleBotonLevantarGlosa AS VisibleBotonLevantarGlosa,
		@ValidarErroresLogica AS ValidarErroresLogica,
		@ObservacionHabilitada AS ObservacionHabilitada,
		@ObservacionObligatoria AS ObservacionObligatoria,
		@ObservacionRegistrada AS ObservacionRegistrada,
		@CalificacionObligatoriaIPS AS CalificacionObligatoriaIPS,
		@CalificacionIPSRegistrada AS CalificacionIPSRegistrada,
		@IdItemGlosa AS IdItemGlosa,
		@IdItemDC AS IdItemDC,
		@IdItemNC AS IdItemNC,
		@TipificacionObservacionDefault  AS TipificacionObservacionDefault,
		@TipificacionObservasionHabilitada AS TipificacionObservasionHabilitada,
		@IdRegistroNuevo AS IdRegistroNuevo,
		@CodigoRegistroPendiente AS CodigoRegistroPendiente,
		@HabilitarBotonMantenerCalificacion AS HabilitarBotonMantenerCalificacion,
		@HabilitarBotonEditarCalificacion AS HabilitarBotonEditarCalificacion,
		@HabilitarBotonComiteExperto AS HabilitarBotonComiteExperto,
		@HabilitarBotonComiteAdministrativo AS HabilitarBotonComiteAdministrativo,
		@HabilitarBotonLevantarGlosa AS HabilitarBotonLevantarGlosa,
		@SolicitarMotivo AS SolicitarMotivo,
		@GuardarCadaCambioVariable AS GuardarCadaCambioVariable,
		@HabilitarGlosa AS HabilitarGlosa,
		@ErroresReportados AS ErroresReportados,
		@VisibleBotonReversar AS VisibleBotonReversar,
		@HabilitadoBotonReversar AS HabilitadoBotonReversar

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Reversar]    Script Date: 10/08/2022 12:04:16 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-08-04
-- Description:	SP para validaciones del resgistro de auditoria cuando se va Reversar un registro
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[SP_Validacion_Reversar]
	
	@RegistroAuditoriaId INT,
	@BotonAccion INT,
	@IdUsuario NVARCHAR(255)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		-- Valores respuesta
		DECLARE @HabilitarVariablesCalificar BIT = 0;
		DECLARE @VisibleBotonGuardar BIT = 0;
		DECLARE @HabilitadoBotonGuardar BIT = 0;
		DECLARE @VisibleBotonMantenerCalificacion BIT = 0;
		DECLARE @VisibleBotonEditarCalificacion BIT = 0;
		DECLARE @VisibleBotonComiteExperto BIT = 0;
		DECLARE @VisibleBotonComiteAdministrativo BIT = 0;
		DECLARE @VisibleBotonLevantarGlosa BIT = 0;
		DECLARE @ValidarErroresLogica BIT = 0;
		DECLARE @ObservacionHabilitada BIT = 0;
		DECLARE @ObservacionObligatoria BIT = 1;
		DECLARE @ObservacionRegistrada BIT = 0;
		DECLARE @CalificacionObligatoriaIPS BIT = 0;
		DECLARE @CalificacionIPSRegistrada BIT = 0;
		DECLARE @IdItemGlosa INT = 0; --  0 Para que deshabilite todos
		DECLARE @IdItemDC INT = (SELECT Id FROM Item WHERE ItemName = 'DC' and CatalogId = 6);
		DECLARE @IdItemNC INT = (SELECT Id FROM Item WHERE ItemName = 'NC' and CatalogId = 6);
		DECLARE @TipificacionObservacionDefault INT = (SELECT Id FROM Item WHERE ItemName = 'Reversar' and CatalogId = 1); -- Item tipificacion General;
		DECLARE @TipificacionObservasionHabilitada BIT = 0;
		DECLARE @IdRegistroNuevo INT = (SELECT Id FROM EstadosRegistroAuditoria WHERE Codigo = 'RN');
		DECLARE @CodigoRegistroPendiente NVARCHAR(255) = (SELECT Codigo FROM EstadosRegistroAuditoria WHERE Id = 17);
		DECLARE @HabilitarBotonMantenerCalificacion BIT = 0;
		DECLARE @HabilitarBotonEditarCalificacion BIT = 0;
		DECLARE @HabilitarBotonComiteExperto BIT = 0;
		DECLARE @HabilitarBotonComiteAdministrativo BIT = 0;
		DECLARE @HabilitarBotonLevantarGlosa BIT = 0;
		DECLARE @SolicitarMotivo BIT = 0;
		DECLARE @GuardarCadaCambioVariable BIT = 0;
		DECLARE @HabilitarGlosa BIT = 0;
		DECLARE @ErroresReportados BIT = 0;
		--
		DECLARE @VisibleBotonReversar BIT = 1;
		DECLARE @HabilitadoBotonReversar BIT = 1;

		--Variables
		--DECLARE @RoleId INT = CAST((SELECT TOP(1) RolId FROM [AUTH].[Usuario] WHERE Id = @IdUsuario) AS INT);
		DECLARE @EsLider BIT = (SELECT R.EsLider FROM [AUTH].[Usuario] US INNER JOIN AspNetRoles R ON (US.RolId = R.Id) WHERE US.Id = @IdUsuario);

			
			IF @EsLider = 1 -- Editar AND Lider
			BEGIN
				SET @HabilitadoBotonGuardar = 1;
				SET @VisibleBotonGuardar = 1;
			END

		
		

		
		-- Consulta final validaciones
		SELECT
		CAST(NEWID() AS NVARCHAR(255)) AS Id,
		@HabilitarVariablesCalificar AS HabilitarVariablesCalificar,
		@VisibleBotonGuardar AS VisibleBotonGuardar,
		@HabilitadoBotonGuardar AS HabilitadoBotonGuardar,
		@VisibleBotonMantenerCalificacion AS VisibleBotonMantenerCalificacion,
		@VisibleBotonEditarCalificacion AS VisibleBotonEditarCalificacion,
		@VisibleBotonComiteExperto AS VisibleBotonComiteExperto,
		@VisibleBotonComiteAdministrativo AS VisibleBotonComiteAdministrativo,
		@VisibleBotonLevantarGlosa AS VisibleBotonLevantarGlosa,
		@ValidarErroresLogica AS ValidarErroresLogica,
		@ObservacionHabilitada AS ObservacionHabilitada,
		@ObservacionObligatoria AS ObservacionObligatoria,
		@ObservacionRegistrada AS ObservacionRegistrada,
		@CalificacionObligatoriaIPS AS CalificacionObligatoriaIPS,
		@CalificacionIPSRegistrada AS CalificacionIPSRegistrada,
		@IdItemGlosa AS IdItemGlosa,
		@IdItemDC AS IdItemDC,
		@IdItemNC AS IdItemNC,
		@TipificacionObservacionDefault  AS TipificacionObservacionDefault,
		@TipificacionObservasionHabilitada AS TipificacionObservasionHabilitada,
		@IdRegistroNuevo AS IdRegistroNuevo,
		@CodigoRegistroPendiente AS CodigoRegistroPendiente,
		@HabilitarBotonMantenerCalificacion AS HabilitarBotonMantenerCalificacion,
		@HabilitarBotonEditarCalificacion AS HabilitarBotonEditarCalificacion,
		@HabilitarBotonComiteExperto AS HabilitarBotonComiteExperto,
		@HabilitarBotonComiteAdministrativo AS HabilitarBotonComiteAdministrativo,
		@HabilitarBotonLevantarGlosa AS HabilitarBotonLevantarGlosa,
		@SolicitarMotivo AS SolicitarMotivo,
		@GuardarCadaCambioVariable AS GuardarCadaCambioVariable,
		@HabilitarGlosa AS HabilitarGlosa,
		@ErroresReportados AS ErroresReportados,
		@VisibleBotonReversar AS VisibleBotonReversar,
		@HabilitadoBotonReversar AS HabilitadoBotonReversar

END
GO
