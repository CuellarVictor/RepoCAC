USE [AuditCAC_QAInterno] -- db segun ambiente
GO
/****** Object:  StoredProcedure [dbo].[ActualizarBanderasRegistrosAuditoria]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



--Creacion/Edicion Procedure
CREATE PROCEDURE [dbo].[ActualizarBanderasRegistrosAuditoria]
(@IdUnico VARCHAR(100), @LevantarGlosa VARCHAR(100), @MantenerCalificacion VARCHAR(100), @ComiteExperto VARCHAR(100), @ComiteAdministrativo VARCHAR(100), @AccionLider VARCHAR(100), @AccionAuditor VARCHAR(100))
AS
BEGIN
SET NOCOUNT ON


--Guardamos Query inicial.
DECLARE @Query VARCHAR(MAX) = '';

--Validamos campos.
DECLARE @CamposUpdate VARCHAR(MAX) = '';
IF(@LevantarGlosa <> '')
BEGIN 
	IF(@CamposUpdate = '')
	BEGIN 
		SET @CamposUpdate = @CamposUpdate + 'LevantarGlosa = ' + CAST(@LevantarGlosa AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @CamposUpdate = @CamposUpdate + ', LevantarGlosa = ' + CAST(@LevantarGlosa AS NVARCHAR(MAX))
	END 
END 
--
IF(@MantenerCalificacion <> '')
BEGIN 
	IF(@CamposUpdate = '')
	BEGIN 
		SET @CamposUpdate = @CamposUpdate + 'MantenerCalificacion = ' + CAST(@MantenerCalificacion AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @CamposUpdate = @CamposUpdate + ', MantenerCalificacion = ' + CAST(@MantenerCalificacion AS NVARCHAR(MAX))
	END 
END 
--
IF(@ComiteExperto <> '')
BEGIN 
	IF(@CamposUpdate = '')
	BEGIN 
		SET @CamposUpdate = @CamposUpdate + 'ComiteExperto = ' + CAST(@ComiteExperto AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @CamposUpdate = @CamposUpdate + ', ComiteExperto = ' + CAST(@ComiteExperto AS NVARCHAR(MAX))
	END 
END 
--
IF(@ComiteAdministrativo <> '')
BEGIN 
	IF(@CamposUpdate = '')
	BEGIN 
		SET @CamposUpdate = @CamposUpdate + 'ComiteAdministrativo = ' + CAST(@ComiteAdministrativo AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @CamposUpdate = @CamposUpdate + ', ComiteAdministrativo = ' + CAST(@ComiteAdministrativo AS NVARCHAR(MAX))
	END 
END 
--
IF(@AccionLider <> '')
BEGIN 
	IF(@CamposUpdate = '')
	BEGIN 
		SET @CamposUpdate = @CamposUpdate + 'AccionLider = ' + CAST(@AccionLider AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @CamposUpdate = @CamposUpdate + ', AccionLider = ' + CAST(@AccionLider AS NVARCHAR(MAX))
	END 
END 
--
IF(@AccionAuditor <> '')
BEGIN 
	IF(@CamposUpdate = '')
	BEGIN 
		SET @CamposUpdate = @CamposUpdate + 'AccionAuditor = ' + CAST(@AccionAuditor AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @CamposUpdate = @CamposUpdate + ', AccionAuditor = ' + CAST(@AccionAuditor AS NVARCHAR(MAX))
	END 
END 


--Validamos si se va a ejecutar el Query (tenemos campos por actualizar).
DECLARE @Where VARCHAR(MAX) = '';
IF(@CamposUpdate <> '' )
BEGIN 
	SET @Query = 'UPDATE RegistrosAuditoria SET ';
	SET @Where = ' WHERE Id = ' + CAST(@IdUnico AS NVARCHAR(MAX));
END  

--Concatenamos Query, Campos por actualizar y Condiciones. Luego ejecutamos.
DECLARE @Total VARCHAR(MAX);
SET @Total = @Query + @CamposUpdate + @Where
EXEC(@Total);

END
GO
/****** Object:  StoredProcedure [dbo].[ActualizarCampo_Dato_DC_NC_ND]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ActualizarCampo_Dato_DC_NC_ND]
--(@IdEstadoOriginal INT, @IdEstadoNuevo INT)
(@RegistrosAuditoriaDetalle INT, @MotivoVariable VARCHAR(50), @Dato_DC_NC_ND INT)
AS
BEGIN
SET NOCOUNT ON

--Actualizamos campo del registro.
UPDATE [RegistrosAuditoriaDetalle] SET MotivoVariable = @MotivoVariable, Dato_DC_NC_ND = @Dato_DC_NC_ND WHERE Id = @RegistrosAuditoriaDetalle;

END
GO
/****** Object:  StoredProcedure [dbo].[ActualizarVariablesLider]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
CREATE PROCEDURE [dbo].[ActualizarVariablesLider]
(@VariableId VARCHAR(MAX), @SubGrupoId VARCHAR(MAX), @Default VARCHAR(MAX), @Auditable VARCHAR(MAX), @Visible VARCHAR(MAX))
AS
BEGIN
SET NOCOUNT ON

BEGIN TRANSACTION [Tran1]

	BEGIN TRY
		
		IF(@VariableId <> '')
		BEGIN
			UPDATE VariableXMedicion SET SubGrupoId = @SubGrupoId, CalificacionXDefecto = @Default, EsCalificable = @Auditable, EsVisible = @Visible WHERE VariableId = @VariableId			
		END

		COMMIT TRANSACTION [Tran1]
	END TRY

	BEGIN CATCH

		ROLLBACK TRANSACTION [Tran1]
		-- PRINT('FAIL');
		-- SELECT ERROR_NUMBER() AS ErrorNumber, ERROR_STATE() AS ErrorState, ERROR_SEVERITY() AS ErrorSeverity, ERROR_PROCEDURE() AS ErrorProcedure, ERROR_LINE() AS ErrorLine, ERROR_MESSAGE() AS ErrorMessage;
	END CATCH 
END
GO
/****** Object:  StoredProcedure [dbo].[ActualizarVariablesLiderMasivo]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
CREATE PROCEDURE [dbo].[ActualizarVariablesLiderMasivo]
(@QueryUpdate VARCHAR(MAX))
AS
BEGIN
SET NOCOUNT ON

BEGIN TRANSACTION [Tran1]

	BEGIN TRY
		
		--Actualizamos campos.
		EXEC(@QueryUpdate);

		COMMIT TRANSACTION [Tran1]
	END TRY

	BEGIN CATCH

		ROLLBACK TRANSACTION [Tran1]
		-- PRINT('FAIL');
		-- SELECT ERROR_NUMBER() AS ErrorNumber, ERROR_STATE() AS ErrorState, ERROR_SEVERITY() AS ErrorSeverity, ERROR_PROCEDURE() AS ErrorProcedure, ERROR_LINE() AS ErrorLine, ERROR_MESSAGE() AS ErrorMessage;
	END CATCH 
END
GO
/****** Object:  StoredProcedure [dbo].[AtarListadoVariablesMedicion]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creamos Procedure.
--DROP PROCEDURE AtarListadoVariablesMedicion
CREATE PROCEDURE [dbo].[AtarListadoVariablesMedicion]
(@Listado DT_Variables READONLY)
AS
BEGIN

	BEGIN TRANSACTION [Tran1]

	BEGIN TRY
		
		----Consultamos e insertamos datos en Variables.
		--INSERT INTO Variables(NaturalezaVariableId, Activa, Orden, idVariable, idCobertura, nombre, nemonico, descripcion, idTipoVariable, longitud, decimales, formato, idErrorTipo, tablaReferencial, campoReferencial, idErrorReferencial, idTipoVariableAlterno, formatoAlterno, permiteVacio, idErrorPermiteVacio, identificadorRegistro, clavePrimaria, idTipoAnalisisEpidemiologico, sistema, exportable, enmascarado, CreatedBy, CreatedDate, ModifyBy, ModifyDate, MotivoVariable, Bot)
		--SELECT NaturalezaVariableId, Activa, Orden, idVariable, idCobertura, nombre, nemonico, descripcion, idTipoVariable, longitud, decimales, formato, idErrorTipo, tablaReferencial, campoReferencial, idErrorReferencial, idTipoVariableAlterno, formatoAlterno, permiteVacio, idErrorPermiteVacio, identificadorRegistro, clavePrimaria, idTipoAnalisisEpidemiologico, sistema, exportable, enmascarado, CreatedBy, CreatedDate, ModifyBy, ModifyDate, MotivoVariable, Bot 
		--FROM @Listado;

		--Creamos tabla temporal para almacenar datos de VariableXMedicion
		CREATE TABLE #TTVariables (Id INT IDENTITY(1,1) NOT NULL, Activa BIT NOT NULL, Orden INT NOT NULL, idVariable INT NOT NULL, idCobertura INT NOT NULL, nombre VARCHAR(100) NOT NULL, nemonico VARCHAR(50) NOT NULL, descripcion VARCHAR(500) NOT NULL, idTipoVariable VARCHAR(10) NOT NULL, longitud INT NULL, 
		decimales INT NULL, formato VARCHAR(300), tablaReferencial VARCHAR(128) NULL, campoReferencial VARCHAR(128) NULL, CreatedBy INT NOT NULL, ModifyBy INT NULL, MotivoVariable NVARCHAR(MAX) NULL, Bot BIT NULL, TipoVariableItem INT NULL, EstructuraVariable INT NULL); 

		--Insertamos datos de VariableXMedicion
		INSERT INTO #TTVariables
		SELECT Activa, Orden, idVariable, idCobertura, nombre, nemonico, descripcion, idTipoVariable, longitud, decimales, formato, tablaReferencial, campoReferencial, CreatedBy, ModifyBy, MotivoVariable, Bot, TipoVariableItem, EstructuraVariable 
		FROM @Listado;

		INSERT INTO Variables(Activa, Orden, idVariable, idCobertura, nombre, nemonico, descripcion, idTipoVariable, longitud, decimales, formato, tablaReferencial, campoReferencial, CreatedBy, CreatedDate, ModifyBy, ModifyDate, MotivoVariable, Bot, TipoVariableItem, EstructuraVariable)
		SELECT Activa, Orden, idVariable, idCobertura, nombre, nemonico, descripcion, idTipoVariable, longitud, decimales, formato, tablaReferencial, campoReferencial, CreatedBy, GETDATE(), ModifyBy, GETDATE(), MotivoVariable, Bot, TipoVariableItem, EstructuraVariable
		FROM #TTVariables;

		/* --- --- */

		--Recorremos datos de tabla temporal.
		DECLARE @Count INT = 0;
		DECLARE @MaxCount INT = (SELECT COUNT(*) FROM #TTVariables);

		WHILE @Count <= @MaxCount --Numero de ciclos Maximos
		BEGIN
			--Creamos tabla temporal para almacenar datos de VariableXMedicion
			CREATE TABLE #TTVariableXMedicion (Id INT IDENTITY(1, 1) PRIMARY KEY, VariableId INT NOT NULL, MedicionId INT NOT NULL, EsGlosa BIT NOT NULL, EsVisible BIT NOT NULL, EsCalificable BIT NOT NULL, Activo BIT NULL, EnableDC BIT NOT NULL, EnableNC BIT NOT NULL, EnableND BIT NOT NULL, 
			CalificacionXDefecto BIT NOT NULL, SubGrupoId INT NULL, Encuesta BIT NOT NULL, VxM_Orden INT NOT NULL);
			
			--Consultamos e insertamos datos en VariableXMedicion.
			INSERT INTO VariableXMedicion(VariableId, MedicionId, EsGlosa, EsVisible, EsCalificable, Activo, CreatedBy, CreationDate, ModifyBy, ModificationDate, EnableDC, EnableNC, EnableND, CalificacionXDefecto, SubGrupoId, Encuesta, Orden)
			SELECT VariableId, MedicionId, EsGlosa, EsVisible, EsCalificable, 1, CreatedBy, GETDATE(), ModifyBy, GETDATE(), EnableDC, EnableNC, EnableND, CalificacionXDefecto, SubGrupoId, Encuesta, Orden
			FROM #TTVariables WHERE Id = @Count;
		END

		COMMIT TRANSACTION [Tran1]
	END TRY

	BEGIN CATCH

		ROLLBACK TRANSACTION [Tran1]
		-- PRINT('FAIL');
		-- SELECT ERROR_NUMBER() AS ErrorNumber, ERROR_STATE() AS ErrorState, ERROR_SEVERITY() AS ErrorSeverity, ERROR_PROCEDURE() AS ErrorProcedure, ERROR_LINE() AS ErrorLine, ERROR_MESSAGE() AS ErrorMessage;
	END CATCH 	
END
GO
/****** Object:  StoredProcedure [dbo].[CalificarRegistroAuditoria]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CalificarRegistroAuditoria]
(@QueryToExecute VARCHAR(MAX))
AS
BEGIN
SET NOCOUNT ON

BEGIN TRANSACTION [Tran1]

	BEGIN TRY			
		--Ejecutamos Query.
		EXEC(@QueryToExecute);

		COMMIT TRANSACTION [Tran1]
	END TRY

	BEGIN CATCH

		ROLLBACK TRANSACTION [Tran1]
	END CATCH 

--END Procedure.
END
GO
/****** Object:  StoredProcedure [dbo].[CambiarEstadoRegistroAuditoria]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- TODO DELETE
--Creacion/Edicion Procedure
CREATE PROCEDURE [dbo].[CambiarEstadoRegistroAuditoria]
(@RegistroAuditoriaId INT, @Proceso VARCHAR(100), @Observacion VARCHAR(MAX), @EstadoAnterioId INT, @EstadoActual INT, @AsignadoA NVARCHAR(450), @AsingadoPor NVARCHAR(450), @CreatedBy NVARCHAR(450))
AS
BEGIN
SET NOCOUNT ON

BEGIN TRANSACTION [Tran1]

	BEGIN TRY
		
		-- Consultamos estado original del registro, para validar si es necesario restar a DisplayOrder.
		DECLARE @Estado INT = (SELECT Estado FROM RegistrosAuditoria WHERE Id = @RegistroAuditoriaId); 

		-- Actualizamos estado del registro.
		UPDATE [RegistrosAuditoria] SET Estado = @EstadoActual WHERE Id = @RegistroAuditoriaId;

		-- Actualizamos DisplayOrder del registro auditado, para enviarlo al final del listado de registros asignados al auditor para el dia actual.
		UPDATE [RegistrosAuditoria] SET DisplayOrder = ((select top(1) DisplayOrder from RegistrosAuditoria WHERE FechaAsignacion = CAST(GETDATE() AS DATE) order by DisplayOrder desc) + 1) WHERE Id = @RegistroAuditoriaId;

		-- Actualizamos DisplayOrder de todos los registros asignados al auditor para el dia actual, restando 1.
		IF(@Estado = 1)
			BEGIN 
			UPDATE registrosauditoria SET displayorder = displayorder - 1 WHERE FechaAsignacion = CAST(GETDATE() AS DATE)
		END 

		-- Actualizamos Fecha y Usuario de edicion.
		UPDATE [RegistrosAuditoria] SET ModifyBy = @CreatedBy, ModifyDate = CAST(GETDATE() AS DATE) WHERE Id = @RegistroAuditoriaId;
		
		-- Insertamos datos en RegistroAuditoriaLog.
		INSERT INTO RegistroAuditoriaLog (RegistroAuditoriaId, Proceso, Observacion, EstadoAnterioId, EstadoActual, AsignadoA, AsingadoPor, CreatedBy, CreatedDate)
			 VALUES(@RegistroAuditoriaId, @Proceso, @Observacion, @EstadoAnterioId, @EstadoActual, @AsignadoA, @AsingadoPor, @CreatedBy, CAST(GETDATE() AS DATE))

		COMMIT TRANSACTION [Tran1]
	END TRY

	BEGIN CATCH

		ROLLBACK TRANSACTION [Tran1]
		-- PRINT('FAIL');
		-- SELECT ERROR_NUMBER() AS ErrorNumber, ERROR_STATE() AS ErrorState, ERROR_SEVERITY() AS ErrorSeverity, ERROR_PROCEDURE() AS ErrorProcedure, ERROR_LINE() AS ErrorLine, ERROR_MESSAGE() AS ErrorMessage;
	END CATCH 

--END Procedure.
END
GO
/****** Object:  StoredProcedure [dbo].[CambiarEstadoRegistroAuditoriaMasivo]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
CREATE PROCEDURE [dbo].[CambiarEstadoRegistroAuditoriaMasivo]
--(@RegistroAuditoriaId INT, @IdEstadoNuevo INT, @registroAuditoriaDetalleId INT, @Listado_Dato_DC_NC_ND VARCHAR(MAX), @Listado_Observaciones VARCHAR(MAX))
(@RegistroAuditoriaId INT, @Proceso VARCHAR(100), @Observacion VARCHAR(MAX), @EstadoAnterioId INT, @EstadoActual INT, @AsignadoA NVARCHAR(450), @AsingadoPor NVARCHAR(450), @CreatedBy NVARCHAR(450), @QueryUpdate VARCHAR(MAX))
AS
BEGIN
SET NOCOUNT ON

BEGIN TRANSACTION [Tran1]

	BEGIN TRY
		-- Consultamos estado original del registro, para validar si es necesario restar a DisplayOrder.
		DECLARE @Estado INT = (SELECT Estado FROM RegistrosAuditoria WHERE Id = @RegistroAuditoriaId); 

		-- Actualizamos estado del registro.
		UPDATE [RegistrosAuditoria] SET Estado = @EstadoActual WHERE Id = @RegistroAuditoriaId;

		-- Actualizamos DisplayOrder del registro auditado, para enviarlo al final del listado de registros asignados al auditor para el dia actual.
		UPDATE [RegistrosAuditoria] SET DisplayOrder = ((select top(1) DisplayOrder from RegistrosAuditoria WHERE FechaAsignacion = CAST(GETDATE() AS DATE) order by DisplayOrder desc) + 1) WHERE Id = @RegistroAuditoriaId;

		-- Actualizamos DisplayOrder de todos los registros asignados al auditor para el dia actual, restando 1.
		IF(@Estado = 1)
			BEGIN 
			UPDATE registrosauditoria SET displayorder = displayorder - 1 WHERE FechaAsignacion = CAST(GETDATE() AS DATE)
		END 
		
		-- Insertamos datos en RegistroAuditoriaLog.
		INSERT INTO RegistroAuditoriaLog (RegistroAuditoriaId, Proceso, Observacion, EstadoAnterioId, EstadoActual, AsignadoA, AsingadoPor, CreatedBy, CreatedDate)
			 VALUES(@RegistroAuditoriaId, @Proceso, @Observacion, @EstadoAnterioId, @EstadoActual, @AsignadoA, @AsingadoPor, @CreatedBy, CAST(GETDATE() AS DATE))

		--Actualizamos MotivoVariable y Dato_DC_NC_ND de RegistrosAuditoriaDetalle.
		EXEC(@QueryUpdate);

		COMMIT TRANSACTION [Tran1]
	END TRY

	BEGIN CATCH

		ROLLBACK TRANSACTION [Tran1]

	END CATCH 

--END Procedure.
END
GO
/****** Object:  StoredProcedure [dbo].[consultaPascientes]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author, juan camilo aguilar>
-- Create date: <Create Date, 21/09/2021,>
-- Description:	<Description, SP para traer los pascientes por un id>
-- =============================================
CREATE PROCEDURE [dbo].[consultaPascientes] (
@Id VARCHAR(MAX),
@IdMedicion VARCHAR(MAX))

AS
BEGIN
SET NOCOUNT ON

DECLARE @Nemonico VARCHAR(50);
DECLARE @Tabla varchar(50);
DECLARE @Query VARCHAR(MAX);

	SET @Nemonico = (SELECT nemonico FROM [CAC].[dbo].[cacCobertura] WHERE IdCobertura = (SELECT IdCobertura FROM [AuditCAC].[dbo].[Medicion] WHERE Id = @IdMedicion));	
	
	SET @Tabla = 'cob' + @Nemonico;
	
	SET @Query = 'select
		id'+@Nemonico+'
		[idEPS]
      ,[estado]
      ,[idRegistro]
      ,[idPeriodo]
      ,[PrimerNombre]
      ,[SegundoNombre]
      ,[PrimerApellido]
      ,[SegundoApellido]
      ,[TipoIdentificacion]
      ,[Identificacion]
      ,[FechaNacimiento]
      ,[Sexo]
      ,[Ocupacion]
      ,[Regimen]
      ,[idPertenenciaEtnica]
      ,[idGrupoPoblacional]
      ,[MunicipioDeResidencia]
      ,[TelefonoPaciente]
      ,[FechaAfiliacion]
      ,[GestacionAlCorte]
      ,[EnPlanificacion]
      ,[EdadUsuarioMomentoDx]
      ,[MotivoPruebaDx]
      ,[FechaDx]
      ,[IpsRealizaConfirmacionDx]
      ,[TipoDeficienciaDiagnosticada]
      ,[SeveridadSegunNivelFactor]
      ,[ActividadCoagulanteDelFactor]
      ,[AntecedentesFamilares]
      ,[FactorRecibidoTtoIni]
      ,[EsquemaTtoIni]
      ,[FechaDeIniPrimerTto]
      ,[FactorRecibidoTtoAct]
      ,[EsquemaTtoAct]
      ,[Peso]
      ,[Dosis]
      ,[FrecuenciaPorSemana]
      ,[UnidadesTotalesEnElPeriodo]
      ,[AplicacionesDelFactorEnElPeriodo]
      ,[ModalidadAplicacionTratamiento]
      ,[ViaDeAdministracion]
      ,[CodigoCumFactorPosRecibido]
      ,[CodigoCumFactorNoPosRecibido]
      ,[CodigoCumDeOtrosTratamientosUtilizadosI]
      ,[CodigoCumDeOtrosTratamientosUtilizadosII]
      ,[IpsSeguimientoActual]
      ,[Hemartrosis]
      ,[CantHemartrosisEspontaneasUlt12Meses]
      ,[CantHemartrosisTraumaticasUlt12Meses]
      ,[HemorragiaIlioPsoas]
      ,[HemorragiaDeOtrosMusculosTejidos]
      ,[HemorragiaIntracraneal]
      ,[HemorragiaEnCuelloOGarganta]
      ,[HemorragiaOral]
      ,[OtrasHemorragias]
      ,[CantOtrasHemorragiasEspontaneasDiffHemartrosis]
      ,[CantOtrasHemorragiasTraumaticasDiffHemartrosis]
      ,[CantOtrasHemorragAsocProcedimientoDiffHemartrosis]
      ,[PresenciaDeInhibidor]
      ,[FechaDeterminacionTitulosInhibidor]
      ,[HaRecibidoITI]
      ,[EstaRecibiendoITI]
      ,[DiasEnITI]
      ,[ArtropatiaHemofilicaCronica]
      ,[CantArticulacionesComprometidas]
      ,[UsuarioInfectadoPorVhc]
      ,[UsuarioInfectadoPorVhb]
      ,[UsuarioInfectadoPorVih]
      ,[Pseudotumores]
      ,[Fracturas]
      ,[Anafilaxis]
      ,[FactorAtribuyeReaccionAnafilactica]
      ,[ReemplazosArticulares]
      ,[ReemplazosArticularesEnPeriodoDeCorte]
      ,[LiderAtencion]
      ,[ConsultasConHematologo]
      ,[ConsultasConOrtopedista]
      ,[IntervencionProfesionalEnfermeria]
      ,[ConsultasOdontologo]
      ,[ConsultasNutricionista]
      ,[IntervencionTrabajoSocial]
      ,[ConsultasConFisiatria]
      ,[ConsultasConPsicologia]
      ,[IntervencionQuimicoFarmaceutico]
      ,[IntervencionFisioterapia]
      ,[PrimerNombreMedicoTratantePrincipal]
      ,[SegundoNombreMedicoTratantePrincipal]
      ,[PrimerApellidoMedicoTratantePrincipal]
      ,[SegundoApellidoMedicoTratantePrincipal]
      ,[CantAtencionesUrgencias]
      ,[CantEventosHospitalarios]
      ,[CostoFactoresPos]
      ,[CostoFactoresNoPos]
      ,[CostoTotalManejo]
      ,[CostoIncapacidadesLaborales]
      ,[Novedades]
      ,[CausaMuerte]
      ,[FechaMuerte]
      ,[FechaCreacionRegistro]
      ,[SerialBDUA]
      ,[CantidadReemplazosArticulares]
      ,[V66FechaCorte] from [CAC].[dbo].['+@Tabla+'] where idHEMOFILIA IN (SELECT value FROM STRING_SPLIT('''+@Id+''','',''))';

	EXEC(@Query);

END

GO
/****** Object:  StoredProcedure [dbo].[ConsultarOrderVariables]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
--DROP PROCEDURE ConsultarOrderVariables;
CREATE PROCEDURE [dbo].[ConsultarOrderVariables]
(@Variable VARCHAR(MAX), @idCobertura INT, @MedicionId VARCHAR(MAX))
AS
BEGIN
SET NOCOUNT ON

BEGIN TRANSACTION [Tran1]

	BEGIN TRY		
	
		--Declaramos variables
		DECLARE @OrdenVAIni INT = 0;
		DECLARE @OrdenVAFin INT = 0;
		--
		DECLARE @OrdenVxIIni INT = 0;
		DECLARE @OrdenVxIFin INT = 0;

		--Consultamos valores y guardamos.
		SET @OrdenVAIni = (SELECT TOP(1) Orden FROM Variables WHERE idCobertura = @idCobertura ORDER BY  Orden DESC);
		SET @OrdenVxIIni = (SELECT TOP(1) Orden FROM VariableXMedicion WHERE VariableId = @Variable AND MedicionId = @MedicionId ORDER BY  Orden DESC);
		--
		SET @OrdenVAFin = (SELECT TOP(1) Orden FROM Variables WHERE idCobertura = @idCobertura ORDER BY  Orden ASC);
		SET @OrdenVxIFin = (SELECT TOP(1) Orden FROM VariableXMedicion WHERE VariableId = @Variable AND MedicionId = @MedicionId ORDER BY  Orden ASC);

		--Validamos valores 
		IF(@OrdenVAIni IS NULL)
		BEGIN
			SET @OrdenVAIni = 0;			
		END --END IF
		--
		IF(@OrdenVxIIni IS NULL)
		BEGIN
			SET @OrdenVxIIni = 0;
		END --END IF
		--
		IF(@OrdenVAFin IS NULL)
		BEGIN
			SET @OrdenVAFin = 0;			
		END --END IF
		--
		IF(@OrdenVxIFin IS NULL)
		BEGIN
			SET @OrdenVxIFin = 0;
		END --END IF

		--Consultamos valores.
		SELECT @OrdenVAIni As OrdenVariableInicial, @OrdenVAFin As OrdenVariableFinal, 
			@OrdenVxIIni As OrdenVxIInicial, @OrdenVxIFin As OrdenVxIFinal;

		COMMIT TRANSACTION [Tran1]
	END TRY

	BEGIN CATCH

		ROLLBACK TRANSACTION [Tran1]
		--PRINT('FAIL');
		--SELECT ERROR_NUMBER() AS ErrorNumber, ERROR_STATE() AS ErrorState, ERROR_SEVERITY() AS ErrorSeverity, ERROR_PROCEDURE() AS ErrorProcedure, ERROR_LINE() AS ErrorLine, ERROR_MESSAGE() AS ErrorMessage;
		INSERT INTO TBL_TX_LOG(Date, Thread, Level, Logger, Message, Exception)
		VALUES(GETDATE(), '1', 'ERROR CATCH', 'Fallo EN SP ConsultarOrderVariables', ERROR_MESSAGE(), CONCAT('ErrorNumber: ',ERROR_NUMBER(),' - ', 'ERROR_STATE: ',ERROR_STATE(),' - ', 'ERROR_SEVERITY: ',ERROR_SEVERITY(),' - ', 'ERROR_PROCEDURE: ',ERROR_PROCEDURE(),' - ', 'ERROR_LINE: ',ERROR_LINE(),' - ', 'ERROR_MESSAGE: ',ERROR_MESSAGE()) );
	END CATCH 
END --END PROCEDURE
GO
/****** Object:  StoredProcedure [dbo].[CrearVariables]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
--DROP PROCEDURE CrearVariables;
CREATE PROCEDURE [dbo].[CrearVariables]
(@Variable VARCHAR(MAX), @Orden INT, @idCobertura INT, @nombre VARCHAR(100), @descripcion VARCHAR(500), @idTipoVariable VARCHAR(500), @longitud INT, @decimales INT, @formato VARCHAR(300), @tablaReferencial VARCHAR(128), @CreatedBy VARCHAR(MAX), @ModifyBy VARCHAR(50), @TipoVariableItem INT, 
 @EstructuraVariable INT, @Alerta BIT, @AlertaDescripcion VARCHAR(MAX), @MedicionId VARCHAR(MAX), @EsVisible BIT, @EsCalificable BIT, @EnableDC BIT, @EnableNC BIT, @EnableND BIT, @CalificacionXDefecto INT, @SubGrupoId INT, @Encuesta BIT, @CalificacionIPSItem VARCHAR(MAX), @IdRegla INT, @Concepto VARCHAR(MAX))
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
		--Declaramos variables
		DECLARE @OutputTbl TABLE (Id INT)
		DECLARE @ItemValueVxM INT;
		DECLARE @ItemValueVxI INT;
		DECLARE @VariableId INT;


		--TODO: Temporal para la presentación
		Declare @cob int = 0
		select @cob = IdCobertura from Medicion where Id = @idCobertura
	
		--Insertamos en Variables.
		INSERT INTO Variables(Activa, Orden, idVariable, idCobertura, nombre, nemonico, descripcion, idTipoVariable, longitud, decimales, formato, tablaReferencial, campoReferencial, CreatedBy, CreatedDate, ModifyBy, ModifyDate, MotivoVariable, Bot, TipoVariableItem, EstructuraVariable, Alerta, AlertaDescripcion) OUTPUT INSERTED.ID INTO @OutputTbl(Id)
		VALUES (@Activa, @Orden, @idVariable, @cob, @nombre, @nemonico, @descripcion, @idTipoVariable, @longitud, @decimales, @formato, @tablaReferencial, @campoReferencial, @CreatedBy, GETDATE(), @ModifyBy, GETDATE(), @MotivoVariable, @Bot, @TipoVariableItem, @EstructuraVariable, @Alerta, @AlertaDescripcion);

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
		SELECT value FROM STRING_SPLIT(@MedicionId,',')

		--Recorremos datos de tabla temporal.
		DECLARE @CountVxM INT = 1;
		DECLARE @MaxCountVxM INT = (SELECT COUNT(*) FROM #TablaTemporalVxM);

		WHILE @CountVxM <= @MaxCountVxM --Numero de ciclos Maximos
		BEGIN
			--Capturaramos valor de @@CalificacionIPSItem.
			SET @ItemValueVxM = (SELECT campo FROM #TablaTemporalVxM WHERE Id = @CountVxM)
	
			--Insertamos.
			INSERT INTO dbo.VariableXMedicion(VariableId, MedicionId, EsGlosa, EsVisible, EsCalificable, Activo, CreatedBy, CreationDate, ModifyBy, ModificationDate, EnableDC, EnableNC, EnableND, CalificacionXDefecto, SubGrupoId, Encuesta, Orden)
			VALUES(@VariableId, @idCobertura, @EsGlosa, @EsVisible, @EsCalificable, @Activo, @CreatedBy, GETDATE(), @ModifyBy, GETDATE(), @EnableDC, @EnableNC, @EnableND, @CalificacionXDefecto, @SubGrupoId, @Encuesta, @VxM_Orden)

			--Sumamos al contador del ciclo.
			SET @CountVxM = @CountVxM + 1
		END -- END WHILE Tabla temporal.		

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
			SELECT value FROM STRING_SPLIT(@CalificacionIPSItem,',')

			--Recorremos datos de tabla temporal.
			DECLARE @CountVxI INT = 1;
			DECLARE @MaxCountVxI INT = (SELECT COUNT(*) FROM #TablaTemporalVxI);

			WHILE @CountVxI <= @MaxCountVxI --Numero de ciclos Maximos
			BEGIN
				--Capturaramos valor de @@CalificacionIPSItem.
				SET @ItemValueVxI = (SELECT campo FROM #TablaTemporalVxI WHERE Id = @CountVxI)
	
				--Insertamos.
				INSERT INTO VariablesXItems(VariablesId, ItemId, CreatedBy, CreatedDate, ModifyBy, ModifyDate)
				VALUES(@VariableId, @ItemValueVxI, @CreatedBy, GETDATE(), @ModifyBy, GETDATE())

				--Sumamos al contador del ciclo.
				SET @CountVxI = @CountVxI + 1
			END -- END WHILE Tabla temporal.		
		END
		-- // --
		

		--Insertamos en Reglas de Variables, en ReglasVariable.		
		INSERT INTO ReglasVariable(IdRegla, IdVariable, Concepto, DateCreate, CreatedBy, ModifyDate, ModifyBy, enable) VALUES (@IdRegla, @VariableId, @Concepto, GETDATE(), @CreatedBy, GETDATE(), @ModifyBy, 1)
		
		-- // --
		

		--Actualizamos campos Orden.
		-- CODE
		-- // --

		COMMIT TRANSACTION [Tran1]
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
/****** Object:  StoredProcedure [dbo].[Delete_User]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[Delete_User] (@correo Varchar(150)) 
AS 
BEGIN 

	update [dbo].[AspNetUsers]
	   set UserDeleted = 1
		 where Email = @correo;
	
	delete from UsuarioXEnfermedad
		where Id in (
			select UxE.Id from
				[dbo].[AspNetUsers] as aspn
				inner join UsuarioXEnfermedad as UxE 
				on aspn.Id = UxE.IdUsuario
				where aspn.UserName =  @correo
		);

	select 'Usuario Eliminado' as Respuesta;

END
GO
/****** Object:  StoredProcedure [dbo].[DuplicarMedicion]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure Para duplicar
CREATE PROCEDURE [dbo].[DuplicarMedicion]
(@MedicionId INT, @UserCreatedBy VARCHAR(50))
AS
BEGIN
SET NOCOUNT ON

	BEGIN TRANSACTION [Tran1]

	BEGIN TRY		
		
			--Declaramos variables usadas.
			DECLARE @OutputTbl TABLE (Id INT)
			DECLARE @OutputTblVariables TABLE (Id INT)
			DECLARE @Nombre VARCHAR(255) = '';
			DECLARE @MedicionIdNuevo INT;
			DECLARE @VariableId INT;
			DECLARE @ItemVariableId INT;
			
			--Capturamos el nombre de la medion a copiar
			SET @Nombre = 'Copia - ' + (SELECT Nombre FROM Medicion WHERE Id = @MedicionId AND Status = 1);
			SET @Nombre = SUBSTRING(@Nombre, 1, 255);

			--Validamos si existe una medicion con el mismo nombre. En ese caso, se le concatena el numero de copia
			DECLARE @ValidNombreDuplicado INT = 0;			
			DECLARE @Name VARCHAR(155) = SUBSTRING(@Nombre, 9, 255);
			SET @ValidNombreDuplicado = (SELECT COUNT(Nombre) FROM Medicion WHERE Nombre LIKE '%' + @Name + '%'  AND Status = 1);
			IF(@ValidNombreDuplicado > 0)
			BEGIN
				SET @ValidNombreDuplicado = @ValidNombreDuplicado + 1;
				SET @Nombre = 'Copia' + CAST(@ValidNombreDuplicado AS VARCHAR(5)) + ' - ' + (SELECT Nombre FROM Medicion WHERE Id = @MedicionId AND Status = 1);
				SET @Nombre = SUBSTRING(@Nombre, 1, 255);
			END			

			--Insertamos nueva medición
			--INSERT INTO Medicion(IdCobertura, IdPeriodo, Descripcion, Activo, CreatedBy, CreatedDate, ModifyBy, ModifyDate, Estado, FechaInicioAuditoria, FechaFinAuditoria, FechaCorteAuditoria, Lider, Resolucion, Nombre, Status) OUTPUT INSERTED.ID INTO @OutputTbl(Id)
			INSERT INTO Medicion(IdCobertura, IdPeriodo, Descripcion, Activo, CreatedBy, CreatedDate, ModifyBy, ModifyDate, Estado, Lider, Resolucion, Nombre, Status) OUTPUT INSERTED.ID INTO @OutputTbl(Id)
			SELECT IdCobertura, IdPeriodo, Descripcion, Activo, @UserCreatedBy As CreatedBy, GETDATE() As CreatedDate, @UserCreatedBy As ModifyBy, GETDATE() As ModifyDate, 31 As Estado, Lider, Resolucion, @Nombre As Nombre, 1 As Status
			FROM Medicion
			WHERE Id = @MedicionId AND Status = 1;
			--
			SET @MedicionIdNuevo = (SELECT Id FROM @OutputTbl);			
			

			--Insertamos Datos VariableXMedicion
			
			INSERT INTO VariableXMedicion (VariableId, MedicionId, EsGlosa, EsVisible, EsCalificable, Activo, CreatedBy, CreationDate, ModifyBy, ModificationDate, EnableDC, EnableNC, EnableND, CalificacionXDefecto, SubGrupoId, Encuesta, Orden, Status)
			--Consultamos registros a duplicar
			SELECT VariableId As VariableId, @MedicionIdNuevo As MedicionId, EsGlosa As EsGlosa, EsVisible As EsVisible, EsCalificable As EsCalificable, Activo As Activo, @UserCreatedBy As CreatedBy, GETDATE() As CreationDate, @UserCreatedBy As ModifyBy, GETDATE() As ModificationDate, EnableDC, EnableNC, EnableND, 
			CalificacionXDefecto, SubGrupoId, Encuesta, Orden, 1 As Status
			FROM VariableXMedicion 
			WHERE MedicionId = @MedicionId AND Status = 1;


			-- // --


			--Medicion. OK 
			--Variables. NO VA
			--VariableXMedicion. OK
			----
			--VariablesXItems. NO VA
			--ReglasVariable. NO VA

			-- //
			--Regresamos datos confirmación.
			--SELECT '1' As Codigo, 'Ok' As Mensaje;

		COMMIT TRANSACTION [Tran1]
	END TRY

	BEGIN CATCH

		ROLLBACK TRANSACTION [Tran1]
		-- PRINT('FAIL');
		-- SELECT ERROR_NUMBER() AS ErrorNumber, ERROR_STATE() AS ErrorState, ERROR_SEVERITY() AS ErrorSeverity, ERROR_PROCEDURE() AS ErrorProcedure, ERROR_LINE() AS ErrorLine, ERROR_MESSAGE() AS ErrorMessage;
		INSERT INTO TBL_TX_LOG(Date, Thread, Level, Logger, Message, Exception)
		VALUES(GETDATE(), '1', 'ERROR CATCH', 'Fallo EN SP DuplicarMedicion', ERROR_MESSAGE(), CONCAT('ErrorNumber: ',ERROR_NUMBER(),' - ', 'ERROR_STATE: ',ERROR_STATE(),' - ', 'ERROR_SEVERITY: ',ERROR_SEVERITY(),' - ', 'ERROR_PROCEDURE: ',ERROR_PROCEDURE(),' - ', 'ERROR_LINE: ',ERROR_LINE(),' - ', 'ERROR_MESSAGE: ',ERROR_MESSAGE()) );
	END CATCH
END -- END SP
GO
/****** Object:  StoredProcedure [dbo].[DuplicarvariablesMedicion]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure Para duplicar
CREATE PROCEDURE [dbo].[DuplicarvariablesMedicion]
--(@MedicionIdOrigional VARCHAR(MAX), @MedicionIdNuevo VARCHAR(MAX))
--(@MedicionIdOrigional INT, @MedicionIdNuevo INT, @Descripcion VARCHAR(200) = '')
(@MedicionIdOrigional INT, @MedicionIdNuevo INT, @UserCreatedBy INT, @Descripcion VARCHAR(200))
AS
BEGIN
SET NOCOUNT ON

	BEGIN TRANSACTION [Tran1]

	BEGIN TRY
		
			--Declaramos variables usadas.
			DECLARE @OutputTbl TABLE (Id INT)
			DECLARE @DescripcionDefault VARCHAR(200) = '';

			--Validamos si recibimos una descripción.
			IF(@Descripcion = '')
			BEGIN 
				--Definimos valor por defecto.
				SET @DescripcionDefault = 'Copia - ' + (SELECT Descripcion FROM Medicion WHERE Id = @MedicionIdOrigional AND Status = 1);
				SET @Descripcion = SUBSTRING(@DescripcionDefault, 1, 200);
			END 

			--Validamos si recibimos un @MedicionIdNuevo.
			IF(@MedicionIdNuevo = 0)
			BEGIN 
				--Insertamos nueva medición
				INSERT INTO Medicion(IdCobertura, IdPeriodo, Descripcion, Activo, CreatedBy, CreatedDate, ModifyBy, ModifyDate) OUTPUT INSERTED.ID INTO @OutputTbl(Id)
				SELECT IdCobertura, IdPeriodo, @Descripcion As Descripcion, Activo, @UserCreatedBy As CreatedBy, GETDATE() As CreatedDate, @UserCreatedBy As ModifyBy, GETDATE() As ModifyDate 
				FROM Medicion
				WHERE Id = @MedicionIdOrigional AND Status = 1;

				SET @MedicionIdNuevo = (SELECT Id FROM @OutputTbl);
			END 

			--Insertamos Datos Variables
			INSERT INTO Variables (Activa, Orden, idVariable, idCobertura, nombre, nemonico, descripcion, idTipoVariable, longitud, decimales, formato, tablaReferencial, campoReferencial, CreatedBy, CreatedDate, ModifyBy, ModifyDate, MotivoVariable, Bot, TipoVariableItem, EstructuraVariable)
			--Consultamos registros a duplicar
			SELECT VA.Activa, VA.Orden, VA.idVariable, VA.idCobertura, VA.nombre, VA.nemonico, VA.descripcion, VA.idTipoVariable, VA.longitud, VA.decimales, VA.formato, VA.tablaReferencial, VA.campoReferencial, @UserCreatedBy As CreatedBy, GETDATE() As CreatedDate, @UserCreatedBy As ModifyBy, GETDATE() As ModifyDate, VA.MotivoVariable, VA.Bot, VA.TipoVariableItem, VA.EstructuraVariable 
			FROM Variables VA
			INNER JOIN VariableXMedicion VAXM ON (VA.Id = VAXM.VariableId)
			WHERE VAXM.MedicionId = @MedicionIdOrigional AND VA.Status = 1 AND VAXM.Status = 1;

			--Insertamos Datos VariableXMedicion
			INSERT INTO [VariableXMedicion] (VariableId, MedicionId, EsGlosa, EsVisible, EsCalificable, Activo, CreatedBy, CreationDate, ModifyBy, ModificationDate, EnableDC, EnableNC, EnableND, CalificacionXDefecto, SubGrupoId, Encuesta, Orden)
			--Consultamos registros a duplicar
			SELECT VariableId As VariableId, @MedicionIdNuevo As MedicionId, EsGlosa As EsGlosa, EsVisible As EsVisible, EsCalificable As EsCalificable, 1 As Activo, @UserCreatedBy As CreatedBy, GETDATE() As CreationDate, @UserCreatedBy As ModifyBy, GETDATE() As ModificationDate, EnableDC, EnableNC, EnableND, 
			CalificacionXDefecto, SubGrupoId, Encuesta, Orden
			FROM VariableXMedicion 
			WHERE MedicionId = @MedicionIdOrigional AND Status = 1;

			--Regresamos datos confirmación.
			--SELECT '1' As Codigo, 'Ok' As Mensaje;

		COMMIT TRANSACTION [Tran1]
	END TRY

	BEGIN CATCH

		ROLLBACK TRANSACTION [Tran1]
		-- PRINT('FAIL');
		-- SELECT ERROR_NUMBER() AS ErrorNumber, ERROR_STATE() AS ErrorState, ERROR_SEVERITY() AS ErrorSeverity, ERROR_PROCEDURE() AS ErrorProcedure, ERROR_LINE() AS ErrorLine, ERROR_MESSAGE() AS ErrorMessage;
	END CATCH
END -- END SP
GO
/****** Object:  StoredProcedure [dbo].[EditarVariables]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
--DROP PROCEDURE CrearVariables;
CREATE PROCEDURE [dbo].[EditarVariables]
(@Variable VARCHAR(MAX), @Orden INT, @idCobertura INT, @nombre VARCHAR(100), @descripcion VARCHAR(500), @idTipoVariable VARCHAR(500), @longitud INT, @decimales INT, @formato VARCHAR(300), @tablaReferencial VARCHAR(128), @CreatedBy VARCHAR(MAX), @ModifyBy VARCHAR(50), @TipoVariableItem INT, 
 @EstructuraVariable INT, @Alerta BIT, @AlertaDescripcion VARCHAR(MAX), @MedicionId VARCHAR(MAX), @EsVisible BIT, @EsCalificable BIT, @EnableDC BIT, @EnableNC BIT, @EnableND BIT, @CalificacionXDefecto INT, @SubGrupoId INT, @Encuesta BIT, @CalificacionIPSItem VARCHAR(MAX), @IdRegla INT, @Concepto VARCHAR(MAX))
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
			SELECT value FROM STRING_SPLIT(@MedicionId,',')

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
					UPDATE VariableXMedicion SET VariableId = @Variable, MedicionId = @ItemValueVxM, EsVisible = @EsVisible, EsCalificable = @EsCalificable, ModifyBy = @ModifyBy, ModificationDate = GETDATE(), EnableDC = @EnableDC, EnableNC = @EnableNC, EnableND = @EnableND, 
					CalificacionXDefecto = @CalificacionXDefecto, SubGrupoId = @SubGrupoId, Encuesta = @Encuesta, Orden = @Orden, Status = 1
					--VALUES(@VariableId, @MedicionId, @EsGlosa, @EsVisible, @EsCalificable, @Activo, @CreatedBy, GETDATE(), @ModifyBy, GETDATE(), @EnableDC, @EnableNC, @EnableND, @CalificacionXDefecto, @SubGrupoId, @Encuesta, @VxM_Orden)
					WHERE VariableId = @Variable AND MedicionId = @ItemValueVxM;
				END --END IF
				ELSE
				BEGIN
					--Insertamos.
					INSERT INTO dbo.VariableXMedicion(VariableId, MedicionId, EsGlosa, EsVisible, EsCalificable, Activo, CreatedBy, CreationDate, ModifyBy, ModificationDate, EnableDC, EnableNC, EnableND, CalificacionXDefecto, SubGrupoId, Encuesta, Orden)
					VALUES(@Variable, @ItemValueVxM, @EsGlosa, @EsVisible, @EsCalificable, @Activo, @CreatedBy, GETDATE(), @ModifyBy, GETDATE(), @EnableDC, @EnableNC, @EnableND, @CalificacionXDefecto, @SubGrupoId, @Encuesta, @VxM_Orden);				
				END --END ELSE IF

				--Sumamos al contador del ciclo.
				SET @CountVxM = @CountVxM + 1
			END -- END WHILE Tabla temporal.			

			-- // --
		
			
			----Actualizamos en VariablesXItems

			----Verificamos si debemos eliminar tabla temporal.
			--BEGIN TRY
			--DROP TABLE #TablaTemporal
			--END TRY
			--BEGIN CATCH  END CATCH

			----Creamos Tabla Temporal.
			--CREATE TABLE #TablaTemporal (Id INT IDENTITY(1, 1) PRIMARY KEY, campo VARCHAR(128));
			--INSERT INTO #TablaTemporal
			--SELECT value FROM STRING_SPLIT(@CalificacionIPSItem,',')

			----Recorremos datos de tabla temporal.
			--DECLARE @Count INT = 1;
			--DECLARE @MaxCount INT = (SELECT COUNT(*) FROM #TablaTemporal);

			--WHILE @Count <= @MaxCount --Numero de ciclos Maximos
			--BEGIN
			--	--Capturaramos valor de @@CalificacionIPSItem.
			--	SET @ItemValueVxI = (SELECT campo FROM #TablaTemporal WHERE Id = @Count)
	
			--	--Insertamos.
			--	UPDATE VariablesXItems SET VariablesId = @Variable, ItemId = @ItemValueVxI, ModifyBy = @ModifyBy, ModifyDate = GETDATE()
			--	--VALUES(@VariableId, @ItemValueVxI, @CreatedBy, GETDATE(), @ModifyBy, GETDATE())
			--	WHERE VariablesId = @Variable;

			--	--Sumamos al contador del ciclo.
			--	SET @Count = @Count + 1
			--END -- END WHILE Tabla temporal.

			
			--Insertamos en Reglas de Variables, en ReglasVariable.
			UPDATE ReglasVariable SET IdRegla = @IdRegla, IdVariable = @Variable, Concepto = @Concepto, ModifyDate = GETDATE(), ModifyBy = @ModifyBy
			WHERE IdVariable = @Variable;

			-- // --
			
		END --END IF(@Variable <> '')
		--ELSE	
		--BEGIN
		--PRINT('SIN VARIABLE');
		--END --END ELSE IF(@Variable <> NULL)
		COMMIT TRANSACTION [Tran1]
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
/****** Object:  StoredProcedure [dbo].[EliminarVariable]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
--DROP PROCEDURE CrearVariables;
CREATE PROCEDURE [dbo].[EliminarVariable]
(@VariableId INT)  
AS
BEGIN
SET NOCOUNT ON

BEGIN TRANSACTION [Tran1]

	BEGIN TRY
		--Eliminar (Inactivar) en: Variables, VariableXMedicion, VariablesXItems, ReglasVariable
		
		--Eliminamos Variables (Inactivamos).
		UPDATE Variables SET Status = 0 WHERE Id = @VariableId

		--Eliminamos VariableXMedicion (Inactivamos).
		UPDATE VariableXMedicion SET Status = 0 WHERE VariableId = @VariableId

		--Eliminamos VariablesXItems (Inactivamos).
		UPDATE VariablesXItems SET Status = 0 WHERE VariablesId = @VariableId

		--Eliminamos ReglasVariable (Inactivamos).
		UPDATE ReglasVariable SET enable = 0 WHERE IdVariable = @VariableId

		COMMIT TRANSACTION [Tran1]
	END TRY

	BEGIN CATCH

		ROLLBACK TRANSACTION [Tran1]
		--PRINT('FAIL');
		--SELECT ERROR_NUMBER() AS ErrorNumber, ERROR_STATE() AS ErrorState, ERROR_SEVERITY() AS ErrorSeverity, ERROR_PROCEDURE() AS ErrorProcedure, ERROR_LINE() AS ErrorLine, ERROR_MESSAGE() AS ErrorMessage;
	END CATCH 
END --END SP
GO
/****** Object:  StoredProcedure [dbo].[GetAlertasRegistrosAuditoria]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Creacion/Edicion Procedure
CREATE PROCEDURE [dbo].[GetAlertasRegistrosAuditoria]
(@IdUsuario VARCHAR(150))
AS
BEGIN
SET NOCOUNT ON

DECLARE @NoAlertas INT = (SELECT COUNT(RA.Id) As NoAlertas
FROM RegistrosAuditoria RA
INNER JOIN EstadosRegistroAuditoria ERA ON (RA.Estado = ERA.Id)
WHERE RA.IdAuditor = @IdUsuario AND
RA.Estado IN (5,6,9,10,14))

SELECT (CAST(@NoAlertas AS NVARCHAR(MAX))) As NoAlertas;
--

END
GO
/****** Object:  StoredProcedure [dbo].[GetBancoInformacionByPalabraClave]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
CREATE PROCEDURE [dbo].[GetBancoInformacionByPalabraClave]
(@PalabraClave VARCHAR(100))
AS
BEGIN
SET NOCOUNT ON

--Guardamos Query inicial.
DECLARE @Query VARCHAR(MAX);
SET @Query = 'SELECT BI.Id, BI.Nombre, BI.Tipo, I.ItemName As NombreTipo, BI.Codigo, BI.CreatedBy, BI.CreatedDate, BI.ModifyBy, BI.ModifyDate  
FROM BancoInformacion BI 
INNER JOIN Item I ON (BI.Tipo = I.Id) ' 

--Calculo de paginado.
--DECLARE @Paginate INT = @PageNumber * @MaxRows;

--Guardamos Condiciones.
DECLARE @Where VARCHAR(MAX) = '';
IF(@PalabraClave <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'BI.Nombre LIKE ''%' + CAST(@PalabraClave AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' OR BI.Nombre LIKE ''%' + CAST(@PalabraClave AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@PalabraClave <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'BI.Codigo LIKE ''%' + CAST(@PalabraClave AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' OR BI.Codigo LIKE ''%' + CAST(@PalabraClave AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@PalabraClave <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'I.ItemName LIKE ''%' + CAST(@PalabraClave AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' OR I.ItemName LIKE ''%' + CAST(@PalabraClave AS NVARCHAR(MAX)) + '%'''
	END 
END 
--Paginado
--DECLARE @Paginado VARCHAR(MAX) = '
--ORDER BY idCobertura
--OFFSET ' + CAST(@Paginate AS NVARCHAR(MAX)) + ' ROWS
--FETCH NEXT ' + CAST(@MaxRows AS NVARCHAR(MAX)) + ' ROWS ONLY;'
 
--Concatenamos Query, Condiciones y Paginado. Luego ejecutamos.
IF(@Where <> '' )
BEGIN 
	SET @Where = ' WHERE ' + @Where;              
END  
DECLARE @Total VARCHAR(MAX);
SET @Total = @Query + '' + @Where --+ ' ' + @Paginado
EXEC(@Total);
--EXEC(@Total);

END
GO
/****** Object:  StoredProcedure [dbo].[GetCalificacionEsCompletas]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
CREATE PROCEDURE [dbo].[GetCalificacionEsCompletas]
(@RegistrosAuditoriaId INT)
AS
BEGIN

--Declaramos variables usadas.
DECLARE @NoItemCalificar INT;
DECLARE @NoVariablesCalificar INT;
DECLARE @NoVariablesCalificadas INT;
DECLARE @Valid BIT = 0;

--Consultamos Numero de Items a calificar.
SET @NoItemCalificar = (SELECT COUNT(*) As NoItemCalificar FROM Item WHERE CatalogId = 3 AND Enable = 1);

--Consultamos Numero de Variables calificables del RegistrosAuditoriaDetalle.
SET @NoVariablesCalificar = (SELECT COUNT(*) As NoVariablesCalificar FROM RegistrosAuditoriaDetalle RAD 
INNER JOIN Variables VA ON (RAD.VariableId = VA.Id) 
INNER JOIN VariableXMedicion VAXM ON (VA.Id = VAXM.VariableId)
WHERE RAD.RegistrosAuditoriaId = @RegistrosAuditoriaId AND VAXM.Encuesta = 1);

--Consultamos el Numero de Variables que ya han sido calificadas del RegistrosAuditoriaDetalle.
--SELECT TOP 1 SUM(COUNT(*)) OVER() AS NoVariablesCalificadas FROM RegistroAuditoriaCalificaciones WHERE RegistrosAuditoriaId = @RegistrosAuditoriaId GROUP BY VariableId HAVING COUNT(*) = @NoItemCalificar
SET @NoVariablesCalificadas = (SELECT TOP 1 SUM(COUNT(*)) OVER() AS NoVariablesCalificadas FROM RegistroAuditoriaCalificaciones WHERE RegistrosAuditoriaId = @RegistrosAuditoriaId GROUP BY VariableId HAVING COUNT(*) = @NoItemCalificar);

--PRINT('@NoItemCalificar: ' + CAST(@NoItemCalificar AS NVARCHAR(MAX)));
--PRINT('@NoVariablesCalificar: ' + CAST(@NoVariablesCalificar AS NVARCHAR(MAX)) );
--PRINT('@NoVariablesCalificadas: ' + CAST(@NoVariablesCalificadas AS NVARCHAR(MAX)) );

--Validamos
IF((@NoVariablesCalificar * @NoItemCalificar) = @NoVariablesCalificadas OR @NoVariablesCalificar = 0)
BEGIN
	SET @Valid = 1;
END

SELECT NEWID() As Idk, @Valid As Valid;

END --END SP
GO
/****** Object:  StoredProcedure [dbo].[GetCalificacionesRegistroAuditoriaByRegistrosAuditoriaId]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetCalificacionesRegistroAuditoriaByRegistrosAuditoriaId]
(@RegistrosAuditoriaId VARCHAR(255))
AS
BEGIN
SET NOCOUNT ON

--Guardamos Query inicial.
DECLARE @Query VARCHAR(MAX);
SET @Query = 'SELECT RA.Id, RA.RegistrosAuditoriaId, RA.RegistrosAuditoriaDetalleId, RA.VariableId, RA.IpsId, RA.ItemId, IT.ItemName As NombreItem, RA.Calificacion, RA.Observacion, RA.CreatedBy, RA.CreatedDate, RA.ModifyBy, RA.ModifyDate 
FROM RegistroAuditoriaCalificaciones RA 
INNER JOIN Item IT ON (IT.Id = RA.ItemId)
WHERE RA.RegistrosAuditoriaId =  ' + CAST(@RegistrosAuditoriaId AS NVARCHAR(MAX))  

EXEC(@Query);

END
GO
/****** Object:  StoredProcedure [dbo].[GetCalificacionesRegistroAuditoriaByVariableId]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Creacion/Edicion Procedure
--Original: GetCalificacionesRegistroAuditoriaByItemId
--DROP PROCEDURE GetCalificacionesRegistroAuditoriaByItemId
CREATE PROCEDURE [dbo].[GetCalificacionesRegistroAuditoriaByVariableId]
(@VariableId VARCHAR(255), @RegistrosAuditoriaId VARCHAR(255))
AS
BEGIN
SET NOCOUNT ON

--Guardamos Query inicial.
DECLARE @Query VARCHAR(MAX);
SET @Query = 'SELECT RA.Id, RA.RegistrosAuditoriaId, RA.RegistrosAuditoriaDetalleId, RA.VariableId, RA.IpsId, RA.ItemId, IT.ItemName As NombreItem, RA.Calificacion, RA.Observacion, RA.CreatedBy, RA.CreatedDate, RA.ModifyBy, RA.ModifyDate 
FROM RegistroAuditoriaCalificaciones RA 
INNER JOIN Item IT ON (IT.Id = RA.ItemId)
WHERE RA.VariableId =  ' + CAST(@VariableId AS NVARCHAR(MAX)) + ' AND RA.RegistrosAuditoriaId =  ' + CAST(@RegistrosAuditoriaId AS NVARCHAR(MAX)) 

EXEC(@Query);

END
GO
/****** Object:  StoredProcedure [dbo].[GetCountLeaderIssues]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[GetCountLeaderIssues] 
(@id_auditor Varchar(75)) 
AS 
BEGIN 

DECLARE @conteo INT

 SET @conteo = (SELECT count(*)
	FROM [RegistrosAuditoria] as RA  
	 JOIN Medicion as Med
		ON RA.IdMedicion = Med.Id
			WHERE RA.Estado in (5, 6, 9, 10, 14)
				AND RA.FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX))
				AND Med.IdCobertura IN (
					SELECT IdCobertura from UsuarioXEnfermedad
						where IdUsuario = @id_auditor
				))
				 
	SELECT @conteo as NroIssues
END
GO
/****** Object:  StoredProcedure [dbo].[GetCountPaginator]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
CREATE PROCEDURE [dbo].[GetCountPaginator]
(@Query VARCHAR(MAX))
AS
BEGIN
SET NOCOUNT ON

IF(@Query = '')
BEGIN 
	EXEC('SELECT NEWID() As Idk, 0 As NoRegistrosTotalesFiltrado');
END 
ElSE
BEGIN 
	EXEC(@Query)
END

--Return(EXEC(@Query));
--Return(585);
--Return(SELECT COUNT(*) FROM RegistrosAuditoria);

END --END Procedure
GO
/****** Object:  StoredProcedure [dbo].[GetDataMedicioneslider]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
--DROP PROCEDURE GetDataMedicionesliderTEMP
CREATE PROCEDURE [dbo].[GetDataMedicioneslider]
(@PageNumber INT, @MaxRows INT, @IdLider VARCHAR(MAX), @IdCobertura VARCHAR(MAX), @IdEstado VARCHAR(MAX))
AS  
BEGIN  
SET NOCOUNT ON  
      
--Declaramos variable para Query    
DECLARE @Query VARCHAR(MAX);    
    
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
    (SELECT ISNULL((SELECT TOP 1 CAST(Progress AS NVARCHAR(255)) AS Progreso from Current_Process WHERE Result LIKE ''%Medicion' + '''' +' + CAST(me.Id AS nvarchar(255))' + '+' + '''' +'%''), 0)) AS Progreso ' + '    
FROM Medicion me     
 JOIN Enfermedad e on e.idCobertura = me.IdCobertura    
 JOIN Item est on est.Id = me.Estado     
 LEFT JOIN (select med.id, COUNT(reg.Id) as TotalAuditados from RegistrosAuditoria reg     
  INNER JOIN Medicion med on med.id = reg.IdMedicion    
  where reg.Estado not in (1,17)    
  group by med.id) as RA on RA.Id = me.Id    
 LEFT JOIN (select med.id, COUNT(reg.Id) as TotalRegistros from RegistrosAuditoria reg     
   INNER JOIN Medicion med on med.id = reg.IdMedicion    
   group by med.id) as TR on TR.Id = me.Id 
   JOIN UsuarioXEnfermedad US on e.idCobertura = me.IdCobertura
   JOIN AspNetUsers U on US.IdUsuario = U.Id ';    


--Calculo de paginado.  
--SET @PageNumber = @PageNumber - 1;  
DECLARE @Paginate INT = @PageNumber * @MaxRows;  


--Guardamos Condiciones.      
DECLARE @Where VARCHAR(MAX) = '';      
--      
IF(@IdLider <> '' or @IdLider <> '0')      
BEGIN       

	--Obtenemos UserName    
	DECLARE @UserName VARCHAR(50) = ''    
	SELECT @UserName = UserName 
	FROM AspNetUsers 
	WHERE Id = @IdLider  

	--Obtenemos el roll
	DECLARE @RolId INT = 0
	SELECT @RolId = RoleId FROM AspNetUserRoles WHERE UserId = @IdLider

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
/****** Object:  StoredProcedure [dbo].[GetDataMedicioneslider_Prueba]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetDataMedicioneslider_Prueba]
(@PageNumber INT, @MaxRows INT, @IdLider VARCHAR(MAX), @IdCobertura VARCHAR(MAX), @IdEstado VARCHAR(MAX))
AS  
BEGIN  
SET NOCOUNT ON  
      
--Declaramos variable para Query    
DECLARE @Query VARCHAR(MAX);    
    
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
   MAX(me.ModifyDate) AS UltimaModificacion,     
    (SELECT ISNULL((SELECT TOP 1 CAST(Progress AS NVARCHAR(255)) AS Progreso from Current_Process WHERE Result LIKE ''%Medicion' + '''' +' + CAST(me.Id AS nvarchar(255))' + '+' + '''' +'%''), 0)) AS Progreso ' + '    
FROM Medicion me     
 JOIN Enfermedad e on e.idCobertura = me.IdCobertura    
 JOIN Item est on est.Id = me.Estado     
 LEFT JOIN (select med.id, COUNT(reg.Id) as TotalAuditados from RegistrosAuditoria reg     
  INNER JOIN Medicion med on med.id = reg.IdMedicion    
  where reg.Estado not in (1,17)    
  group by med.id) as RA on RA.Id = me.Id    
 LEFT JOIN (select med.id, COUNT(reg.Id) as TotalRegistros from RegistrosAuditoria reg     
   INNER JOIN Medicion med on med.id = reg.IdMedicion    
   group by med.id) as TR on TR.Id = me.Id 
   JOIN UsuarioXEnfermedad US on e.idCobertura = me.IdCobertura
   JOIN AspNetUsers U on US.IdUsuario = U.Id ';    


--Calculo de paginado.  
--SET @PageNumber = @PageNumber - 1;  
DECLARE @Paginate INT = @PageNumber * @MaxRows;  


--Guardamos Condiciones.      
DECLARE @Where VARCHAR(MAX) = '';      
--      
IF(@IdLider <> '' or @IdLider <> '0')      
BEGIN       

	--Obtenemos UserName    
	DECLARE @UserName VARCHAR(50) = ''    
	SELECT @UserName = UserName 
	FROM AspNetUsers 
	WHERE Id = @IdLider  

	--Obtenemos el roll
	DECLARE @RolId INT = 0
	SELECT @RolId = RoleId FROM AspNetUserRoles WHERE UserId = @IdLider

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
SET @Query2 = N'SELECT NEWID() As Idk, COUNT(*) As NoRegistrosTotalesFiltrado   
FROM Medicion me     
 JOIN Enfermedad e on e.idCobertura = me.IdCobertura    
 JOIN Item est on est.Id = me.Estado     
 LEFT JOIN (select med.id, COUNT(reg.Id) as TotalAuditados from RegistrosAuditoria reg     
  INNER JOIN Medicion med on med.id = reg.IdMedicion    
  where reg.Estado not in (1,17)    
  group by med.id) as RA on RA.Id = me.Id    
 LEFT JOIN (select med.id, COUNT(reg.Id) as TotalRegistros from RegistrosAuditoria reg     
   INNER JOIN Medicion med on med.id = reg.IdMedicion    
   group by med.id) as TR on TR.Id = me.Id 
   JOIN UsuarioXEnfermedad US on e.idCobertura = me.IdCobertura
   JOIN AspNetUsers U on US.IdUsuario = U.Id'  
  
DECLARE @Total2 NVARCHAR(MAX);  
SET @Total2 = @Query2 + '' + REPLACE(@Where, '''', '''''')  

--str, old_str. new_str  
SET @Total = REPLACE(@Total, ''' '' As QueryNoRegistrosTotales', '''' + CAST(@Total2 AS NVARCHAR(MAX)) + ''' As QueryNoRegistrosTotales');  

--Imprimimos/Ejecutamos.	
EXEC(@Total);      
--SELECT @Total;      
    
END --END PROCEDURE
GO
/****** Object:  StoredProcedure [dbo].[getEnfermedades]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[getEnfermedades]
AS 
BEGIN 

	select  
		IdEnfermedad,
		IdCobertura,
		Nombre,
		Status
	from Enfermedad;

END
GO
/****** Object:  StoredProcedure [dbo].[GETENFERMEDADESMADREXUSUARIO]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GETENFERMEDADESMADREXUSUARIO]
(@idUsuario VARCHAR(50))
AS
BEGIN
SET NOCOUNT ON

declare @rol int = 0
select @rol = r.RoleId from AspNetUserRoles r where  r.UserId = @idUsuario 

if(@rol = 1)
begin
  select NEWID() As Idk, e.idCobertura, e.nombre, @idUsuario as IdUsuario  
  from Enfermedad e

end
else
begin

  select NEWID() As Idk, e.idCobertura, e.nombre, u.IdUsuario from UsuarioXEnfermedad u
  inner join Enfermedad e on (u.idCobertura = e.idCobertura)
  where u.IdUsuario = @idUsuario

end

END
GO
/****** Object:  StoredProcedure [dbo].[GetExportToExcle]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetExportToExcle]    
@IdAuditor varchar(50)  
  
AS BEGIN       
  
if @IdAuditor = 'all'  
BEGIN  
SELECT  '' As QueryNoRegistrosTotales, RA.Id, RA.IdRadicado, RA.IdMedicion, ME.Descripcion As NombreMedicion, RA.IdPeriodo, RA.IdAuditor, 1 As IdEntidad, 'EntidadFakeName' As Entidad, RA.PrimerNombre, RA.SegundoNombre, RA.PrimerApellido,
 RA.SegundoApellido, RA.Sexo, RA.TipoIdentificacion, RA.Identificacion, RA.FechaNacimiento, RA.FechaCreacion, RA.FechaAuditoria, RA.FechaMinConsultaAudit, RA.FechaMaxConsultaAudit, RA.FechaAsignacion, RA.Activo, RA.Conclusion, RA.UrlSoportes, RA.Reverse, 
RA.DisplayOrder, RA.Ara, RA.Eps, RA.FechaReverso, RA.AraAtendido, RA.EpsAtendido, RA.Revisar, RA.Extemporaneo, RA.Estado, ERA.Codigo As EstadoCodigo, ERA.Nombre As EstadoNombre, RA.LevantarGlosa AS LevantarGlosa, RA.MantenerCalificacion AS MantenerCalificacion, RA.ComiteExperto AS ComiteExperto, RA.ComiteAdministrativo AS ComiteAdministrativo, RA.AccionLider AS AccionLider, RA.AccionAuditor AS AccionAuditor, RA.Encuesta As Encuesta, US.UserName as CreatedBy, RA.CreatedDate, US.UserName as ModifyBy, RA.ModifyDate, RIC.ImagePath, US.UserName as NombreAuditor, ME.IdCobertura, EF.nombre as EnfermedadMadre,
RA.Status
FROM RegistrosAuditoria RA   
JOIN Medicion ME ON (RA.IdMedicion = ME.Id)   
JOIN EstadosRegistroAuditoria ERA ON (RA.Estado = ERA.Id)   
JOIN AspNetUsers US ON (US.Id = RA.IdAuditor)  
JOIN AspNetUserRoles UR ON (UR.UserId = RA.IdAuditor)  
JOIN EstadoRegistroAuditoriaIcono RIC ON (RIC.Estado = RA.Estado AND RIC.RolId = UR.RoleId) AND RA.Revisar = RIC.Revisar 
JOIN Enfermedad EF ON EF.IdCobertura = ME.IdCobertura 
END  
ELSE  
BEGIN  
SELECT '' As QueryNoRegistrosTotales, RA.Id, RA.IdRadicado, RA.IdMedicion, ME.Descripcion As NombreMedicion, RA.IdPeriodo, RA.IdAuditor, 1 As IdEntidad, 'EntidadFakeName' As Entidad, RA.PrimerNombre, RA.SegundoNombre, RA.PrimerApellido,
 RA.SegundoApellido, RA.Sexo, RA.TipoIdentificacion, RA.Identificacion, RA.FechaNacimiento, RA.FechaCreacion, RA.FechaAuditoria, RA.FechaMinConsultaAudit, RA.FechaMaxConsultaAudit, RA.FechaAsignacion, RA.Activo, RA.Conclusion, RA.UrlSoportes, RA.Reverse, 
RA.DisplayOrder, RA.Ara, RA.Eps, RA.FechaReverso, RA.AraAtendido, RA.EpsAtendido, RA.Revisar, RA.Extemporaneo, RA.Estado, ERA.Codigo As EstadoCodigo, ERA.Nombre As EstadoNombre, RA.LevantarGlosa AS LevantarGlosa, RA.MantenerCalificacion AS MantenerCalificacion, RA.ComiteExperto AS ComiteExperto, RA.ComiteAdministrativo AS ComiteAdministrativo, RA.AccionLider AS AccionLider, RA.AccionAuditor AS AccionAuditor, RA.Encuesta As Encuesta, US.UserName as CreatedBy, RA.CreatedDate, US.UserName as ModifyBy, RA.ModifyDate, RIC.ImagePath, US.UserName as NombreAuditor, ME.IdCobertura, EF.nombre as EnfermedadMadre,
RA.Status
FROM RegistrosAuditoria RA   
JOIN Medicion ME ON (RA.IdMedicion = ME.Id)   
JOIN EstadosRegistroAuditoria ERA ON (RA.Estado = ERA.Id)   
JOIN AspNetUsers US ON (US.Id = RA.IdAuditor)  
JOIN AspNetUserRoles UR ON (UR.UserId = RA.IdAuditor)  
JOIN EstadoRegistroAuditoriaIcono RIC ON (RIC.Estado = RA.Estado AND RIC.RolId = UR.RoleId) AND RA.Revisar = RIC.Revisar 
JOIN Enfermedad EF ON EF.IdCobertura = ME.IdCobertura where RA.IdAuditor = @IdAuditor  
END     
END



--[GetExportToExcle] 'all'
GO
/****** Object:  StoredProcedure [dbo].[GetFiltrosBolsaMedicion]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetFiltrosBolsaMedicion]
(@PageNumber INT, 
@MaxRows INT, 
@IdLider VARCHAR(MAX))
AS
BEGIN
SET NOCOUNT ON

--Guardamos Query inicial.
DECLARE @Query VARCHAR(MAX);

SET @Query = 'SELECT NEWID() As Idk, CO.idCobertura as IdCobertura, CO.nombre as Nombre, CO.nemonico as Neomonico, PE.idPeriodo as IdPeriodo, PE.fechaCorte as FechaCorte FROM [CAC].[dbo].[cacCobertura] CO
inner join [CAC].[dbo].[cacperiodo] PE on (co.idCobertura = PE.idCobertura)
inner join [AUDITCAC_Dev2].[dbo].[UsuarioXEnfermedad] US on (US.IdCobertura = CO.idCobertura) where US.IdUsuario = '''+@IdLider+''''


EXEC(@Query);

--PRINT(' --- ');
--PRINT(@Total2);

END --END Procedure
GO
/****** Object:  StoredProcedure [dbo].[GetMedicionCriterio]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
CREATE PROCEDURE [dbo].[GetMedicionCriterio]
@PageNumber INT, @MaxRows INT, @IdUsuario VARCHAR(MAX), @Id VARCHAR(MAX), @IdCobertura VARCHAR(MAX), @IdPeriodo VARCHAR(MAX), @Descripcion VARCHAR(MAX), @Activo VARCHAR(MAX), @CreatedBy VARCHAR(MAX), @CreatedDate VARCHAR(MAX), @ModifyBy VARCHAR(MAX), @ModifyDate VARCHAR(MAX)

AS
BEGIN
	SET NOCOUNT ON;


--Guardamos Query inicial.
DECLARE @Query VARCHAR(MAX);
SET @Query = 'SELECT '' '' As QueryNoRegistrosTotales, ME.Id As Id, ME.IdCobertura As IdCobertura, ME.IdPeriodo As IdPeriodo, ME.Descripcion As Descripcion, ME.Activo As Activo, ME.CreatedBy As CreatedBy, ME.CreatedDate As CreatedDate, ME.ModifyBy As ModifyBy, ME.ModifyDate As ModifyDate 
FROM Medicion ME 
INNER JOIN UsuarioXEnfermedad UXE ON (UXE.IdCobertura = ME.IdCobertura) '


--Calculo de paginado.
DECLARE @Paginate INT = @PageNumber * @MaxRows;


--Guardamos Condiciones.
DECLARE @Where VARCHAR(MAX) = '';
IF(@Id <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'ME.Id = IN (' + CAST(@Id AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND ME.Id = IN (' + CAST(@Id AS NVARCHAR(MAX)) + ')'
	END 
END 
--
IF(@IdUsuario <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'UXE.IdUsuario = ''' + CAST(@IdUsuario AS VARCHAR(100)) + ''''
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND UXE.IdUsuario = ''' + CAST(@IdUsuario AS VARCHAR(100)) + ''''
	END 
END 
--
IF(@IdCobertura <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'ME.IdCobertura = IN (' + CAST(@IdCobertura AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND ME.IdCobertura = IN (' + CAST(@IdCobertura AS NVARCHAR(MAX)) + ')'
	END 
END 
--
IF(@IdPeriodo <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'ME.IdPeriodo = IN (' + CAST(@IdPeriodo AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND ME.IdPeriodo = IN (' + CAST(@IdPeriodo AS NVARCHAR(MAX)) + ')'
	END 
END 
--
IF(@Descripcion <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'ME.Descripcion  LIKE ''%' + CAST(@Descripcion AS NVARCHAR(12)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND ME.Descripcion  LIKE ''%' + CAST(@Descripcion AS NVARCHAR(12)) + '%'''
	END 
END 
--
IF(@Activo <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'ME.Activo = IN (' + CAST(@Activo AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND ME.Activo = IN (' + CAST(@Activo AS NVARCHAR(MAX)) + ')'
	END 
END 
--
IF(@CreatedBy <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'ME.CreatedBy = ''' + CAST(@CreatedBy AS VARCHAR(100)) + ''''
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND ME.CreatedBy = ''' + CAST(@CreatedBy AS VARCHAR(100)) + ''''
	END 
END 
--
IF(@CreatedDate <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'ME.CreatedDate = ''' + CAST(@CreatedDate AS VARCHAR(100)) + '''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND ME.CreatedDate = ''' + CAST(@CreatedDate AS VARCHAR(100)) + ''''
	END 
END 
--
IF(@ModifyBy <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'ME.ModifyBy = ''' + CAST(@ModifyBy AS VARCHAR(100)) + ''''
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND ME.ModifyBy = ''' + CAST(@ModifyBy AS VARCHAR(100)) + ''''
	END 
END 
--
IF(@ModifyDate <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'ME.ModifyDate =  ''' + CAST(@ModifyDate AS VARCHAR(100)) + ''''
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND ME.ModifyDate = ''' +  + CAST(@ModifyDate AS VARCHAR(100)) + ''''
	END 
END 


--Paginado
DECLARE @Paginado VARCHAR(MAX) = '
ORDER BY Id
OFFSET ' + CAST(@Paginate AS NVARCHAR(12)) + ' ROWS
FETCH NEXT ' + CAST(@MaxRows AS NVARCHAR(12)) + ' ROWS ONLY;'


--Concatenamos Query, Condiciones y Paginado. Luego ejecutamos.
IF(@Where <> '' )
BEGIN 
	SET @Where = ' WHERE ' + @Where;              
END  
DECLARE @Total VARCHAR(MAX);
SET @Total = @Query + '' + @Where + ' ' + @Paginado 

--Para calcular total registros filtrados.
DECLARE @Query2 NVARCHAR(MAX);
SET @Query2 = N'SELECT NEWID() As Idk, COUNT(*) As NoRegistrosTotalesFiltrado 
FROM Medicion ME 
INNER JOIN UsuarioXEnfermedad UXE ON (UXE.IdCobertura = ME.IdCobertura)'


DECLARE @Total2 NVARCHAR(MAX);
SET @Total2 = @Query2 + '' + REPLACE(@Where, '''', '''''')


--str, old_str. new_str
SET @Total = REPLACE(@Total, ''' '' As QueryNoRegistrosTotales', '''' + CAST(@Total2 AS NVARCHAR(MAX)) + ''' As QueryNoRegistrosTotales');

EXEC(@Total);
--PRINT(@Total);

END
GO
/****** Object:  StoredProcedure [dbo].[GetObservacionesByRegistroAuditoriaId]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
CREATE PROCEDURE [dbo].[GetObservacionesByRegistroAuditoriaId]
(@RegistroAuditoriaId VARCHAR(100))
AS
BEGIN
SET NOCOUNT ON

--Guardamos Query inicial.
DECLARE @Query VARCHAR(MAX);
SET @Query = 'SELECT RADS.Id, RADS.RegistroAuditoriaId, RADS.TipoObservacion, IT.ItemName As NombreTipoObservacion, RADS.Observacion, RADS.Soporte, RADS.EstadoActual, ERA1.Nombre As EstadoActualNombre, RADS.EstadoNuevo, ERA2.Nombre As EstadoNuevoNombre, R.Id As IdRol, R.Name As NombreRol, RADS.CreatedBy, RADS.CreatedDate, RADS.ModifyBy, RADS.ModifyDate, U.UserName, UD.Nombres, UD.Apellidos, UD.Codigo
FROM RegistrosAuditoriaDetalleSeguimiento RADS 
INNER JOIN EstadosRegistroAuditoria ERA1 ON (RADS.EstadoActual = ERA1.Id) 
INNER JOIN EstadosRegistroAuditoria ERA2 ON (RADS.EstadoNuevo = ERA2.Id) 
INNER JOIN AspNetUsers U ON (RADS.CreatedBy = U.Id)
LEFT JOIN AspNetUsersDetelles UD ON UD.AspNetUsersId = U.Id
INNER JOIN AspNetUserRoles UR ON (U.Id = UR.UserId)
INNER JOIN AspNetRoles R ON (UR.RoleId = R.Id)
INNER JOIN Item IT ON (RADS.TipoObservacion = IT.Id) ' 

--Calculo de paginado.
--DECLARE @Paginate INT = @PageNumber * @MaxRows;

--Guardamos Condiciones.
DECLARE @Where VARCHAR(MAX) = '';
IF(@RegistroAuditoriaId <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RegistroAuditoriaId = ' + CAST(@RegistroAuditoriaId AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RegistroAuditoriaId = ' + CAST(@RegistroAuditoriaId AS NVARCHAR(MAX))
	END 
END 
--

--Paginado
--DECLARE @Paginado VARCHAR(MAX) = '
--ORDER BY idCobertura
--OFFSET ' + CAST(@Paginate AS NVARCHAR(MAX)) + ' ROWS
--FETCH NEXT ' + CAST(@MaxRows AS NVARCHAR(MAX)) + ' ROWS ONLY;'
 
--Concatenamos Query, Condiciones y Paginado. Luego ejecutamos.
IF(@Where <> '' )
BEGIN 
	SET @Where = ' WHERE ' + @Where;              
END  
DECLARE @Total VARCHAR(MAX);
SET @Total = @Query + '' + @Where + ' ORDER BY RADS.Id DESC' --+ ' ' + @Paginado
EXEC(@Total);

END
GO
/****** Object:  StoredProcedure [dbo].[GetPercentageProcess]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author, juan camilo aguilar>
-- Create date: <Create Date, 16/09/2021,>
-- Description:	<Description, SP para consultar el porcentaje del proceso>
-- =============================================
CREATE PROCEDURE [dbo].[GetPercentageProcess]

--@PageNumber INT,
--@MaxRows INT,
--@ProcessId INT,
--@Status INT,
--@Name VARCHAR(50),
--@Class VARCHAR(50),
--@Method VARCHAR(50),
--@LifeTime VARCHAR(50)

AS
BEGIN

	SELECT top(1) *  FROM [AuditCAC].[dbo].[Current_Process] where [Progress] = -1   

END
GO
/****** Object:  StoredProcedure [dbo].[GetRegistrosAuditoria]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
--Original: GetRegistrosAuditoria
--DROP PROCEDURE GetRegistrosAuditoria2
CREATE  PROCEDURE [dbo].[GetRegistrosAuditoria]
(@PageNumber INT, @MaxRows INT, @Id VARCHAR(MAX), @IdRadicado VARCHAR(MAX), @IdMedicion VARCHAR(MAX), @IdPeriodo VARCHAR(MAX), @IdAuditor VARCHAR(MAX), @IdLider VARCHAR(MAX), @PrimerNombre VARCHAR(MAX), @SegundoNombre VARCHAR(MAX), @PrimerApellido VARCHAR(MAX), @SegundoApellido VARCHAR(MAX), @Sexo VARCHAR(MAX), @TipoIdentificacion VARCHAR(MAX), @Identificacion VARCHAR(MAX), @FechaNacimientoIni VARCHAR(MAX), @FechaNacimientoFin VARCHAR(MAX), @FechaCreacionIni VARCHAR(MAX), @FechaCreacionFin VARCHAR(MAX), @FechaAuditoriaIni VARCHAR(MAX), @FechaAuditoriaFin VARCHAR(MAX), @FechaMinConsultaAudit VARCHAR(MAX), @FechaMaxConsultaAudit VARCHAR(MAX), @FechaAsignacionIni VARCHAR(MAX), @FechaAsignacionFin VARCHAR(MAX), @Activo VARCHAR(MAX), @Conclusion VARCHAR(MAX), @UrlSoportes VARCHAR(MAX), @Reverse VARCHAR(MAX), @DisplayOrder VARCHAR(MAX), @Ara VARCHAR(MAX), @Eps VARCHAR(MAX), @FechaReversoIni VARCHAR(MAX), @FechaReversoFin VARCHAR(MAX), @AraAtendido VARCHAR(MAX), @EpsAtendido VARCHAR(MAX), @Revisar VARCHAR(MAX), @Estado VARCHAR(MAX), @AccionLider VARCHAR(MAX), @AccionAuditor VARCHAR(MAX), @CodigoEps VARCHAR(MAX))
AS  
BEGIN  
SET NOCOUNT ON  
  
--Para convertir 'all' a vacio.  
IF @IdAuditor = 'all'  
BEGIN  
SET @IdAuditor = ''  
END  
  
--Guardamos Query inicial.  
DECLARE @Query VARCHAR(MAX);  

DECLARE @Ips NVARCHAR(255) = (SELECT TOP (1) Valor  FROM [dbo].[ParametrosGenerales] where Nombre = 'CampoReferencialIps')

SET @Query = 'SELECT '' '' As QueryNoRegistrosTotales, RA.Id, RA.IdRadicado, RA.IdMedicion, ME.Nombre As NombreMedicion, RA.IdPeriodo, RA.IdAuditor, 1 As IdEntidad, E.Nombre As Entidad, RA.PrimerNombre, RA.SegundoNombre, RA.PrimerApellido, RA.SegundoApellido, RA.Sexo, RA.TipoIdentificacion, RA.Identificacion, RA.FechaNacimiento, RA.FechaCreacion, RA.FechaAuditoria, RA.FechaMinConsultaAudit, RA.FechaMaxConsultaAudit, RA.FechaAsignacion, RA.Activo, RA.Conclusion, RA.UrlSoportes, RA.Reverse, RA.DisplayOrder, RA.Ara, RA.Eps, RA.FechaReverso, RA.AraAtendido, RA.EpsAtendido, RA.Revisar, RA.Extemporaneo, RA.Estado, ERA.Codigo As EstadoCodigo, ERA.Nombre As EstadoNombre, RA.LevantarGlosa AS LevantarGlosa, RA.MantenerCalificacion AS MantenerCalificacion, RA.ComiteExperto AS ComiteExperto, RA.ComiteAdministrativo AS ComiteAdministrativo, RA.AccionLider AS AccionLider, RA.AccionAuditor AS AccionAuditor, RA.Encuesta As Encuesta, US.UserName as CreatedBy, RA.CreatedDate, US.UserName as ModifyBy, RA.ModifyDate, RIC.ImagePath, US.UserName as NombreAuditor, UD.Codigo AS CodigoUsuario, ME.IdCobertura, EF.nombre as EnfermedadMadre, RA.IdEPS As Data_IdEPS, E.Nombre As Data_NombreEPS' +
', (SELECT TOP(1)  ItemDescripcion FROM CatalogoItemCobertura WHERE ItemId =  (SELECT TOP (1) rad.DatoReportado FROM RegistrosAuditoriaDetalle rad INNER JOIN Variables va ON va.Id = rad.VariableId WHERE va.campoReferencial = ''' + @Ips + ''' AND rad.RegistrosAuditoriaId = RA.Id)) AS IPS '

+ ' FROM RegistrosAuditoria RA   
JOIN Medicion ME ON (RA.IdMedicion = ME.Id)   
JOIN EstadosRegistroAuditoria ERA ON (RA.Estado = ERA.Id)   
JOIN AspNetUsers US ON (US.Id = RA.IdAuditor)  
LEFT JOIN AspNetUsersDetelles UD ON UD.AspNetUsersId = US.Id
JOIN AspNetUserRoles UR ON (UR.UserId = RA.IdAuditor)  
JOIN EstadoRegistroAuditoriaIcono RIC ON (RIC.Estado = RA.Estado AND RIC.RolId = UR.RoleId) AND RA.Revisar = RIC.Revisar 
JOIN Enfermedad EF ON EF.idCobertura = ME.IdCobertura
JOIN Eps E ON (E.EpsId = RA.IdEPS)'
  
-- Si se consulta EPS es necesario agregar un join   
--BEGIN   
-- IF(@CodigoEps <> '')  
-- BEGIN                                  -- Id Variable EPS  
--  SET @Query = @Query + 'INNER JOIN RegistrosAuditoriaDetalle RD ON RA.Id = rd.RegistrosAuditoriaId and RD.VariableId = 35 '      + 'AND RD.DatoReportado = ''' + CAST(@CodigoEps AS NVARCHAR(MAX)) + ''''    
-- END   
--END   
  
--Calculo de paginado.  
--SET @PageNumber = @PageNumber - 1;  
DECLARE @Paginate INT = @PageNumber * @MaxRows;  
  
--Guardamos Condiciones.  
DECLARE @Where VARCHAR(MAX) = '';  
IF(@Id <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'RA.Id IN (' + CAST(@Id AS NVARCHAR(MAX)) + ')'  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND RA.Id IN (' + CAST(@Id AS NVARCHAR(MAX)) + ')'  
 END   
END   
--  
IF(@IdRadicado <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'IdRadicado LIKE ''%' + CAST(@IdRadicado AS NVARCHAR(MAX)) + '%'''   
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND IdRadicado LIKE ''%' + CAST(@IdRadicado AS NVARCHAR(MAX)) + '%'''  
 END   
END   
--  
IF(@IdMedicion <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'RA.IdMedicion IN (' + CAST(@IdMedicion AS NVARCHAR(MAX)) + ')'  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND RA.IdMedicion IN (' + CAST(@IdMedicion AS NVARCHAR(MAX)) + ')'  
 END   
END   
--  
IF(@IdPeriodo <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'RA.IdPeriodo = ' + CAST(@IdPeriodo AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND RA.IdPeriodo = ' + CAST(@IdPeriodo AS NVARCHAR(MAX))  
 END   
END   
--  
IF(@IdAuditor <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'RA.IdAuditor IN (''' + CAST(@IdAuditor AS NVARCHAR(MAX)) + ''')'  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND RA.IdAuditor IN (''' + CAST(@IdAuditor AS NVARCHAR(MAX)) + ''')'  
 END   
END   
ELSE
BEGIN

IF(@IdLider <> '')  
BEGIN   
  
 declare @countLider int = 0, @registerlider int = 1  
 declare @tablelider as table   
 (  
  Id int not null primary key identity(1,1),  
  IdCobertura int not null  
 )  
 insert @tablelider Select IdCobertura from UsuarioXEnfermedad --where IdUsuario = @IdLider  
 select @countLider = COUNT(*) from @tablelider   
  
 DECLARE @resultlider varchar(200) = ''  
  
 WHILE(@registerlider <= @countLider)  
 BEGIN   
  declare @regtemp INT = ''  
  SELECT @regtemp = IdCobertura FROM @tablelider WHERE Id = @registerlider  
  SET @resultlider = @resultlider + CAST(@regtemp AS varchar(2)) + ','   
  SET @registerlider = @registerlider + 1  
 END   
  
 SET @resultlider = @resultlider + '0'  
  
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'ME.IdCobertura IN (' +  @resultlider + ')'  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND ME.IdCobertura IN (' + @resultlider + ')'  
 END   
END 


END
--  
IF(@PrimerNombre <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'PrimerNombre LIKE ''%' + CAST(@PrimerNombre AS NVARCHAR(MAX)) + '%'''   
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND PrimerNombre LIKE ''%' + CAST(@PrimerNombre AS NVARCHAR(MAX)) + '%'''  
 END   
END   
--  
IF(@SegundoNombre <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'SegundoNombre LIKE ''%' + CAST(@SegundoNombre AS NVARCHAR(MAX)) + '%'''   
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND SegundoNombre LIKE ''%' + CAST(@SegundoNombre AS NVARCHAR(MAX)) + '%'''  
 END   
END   
--  
IF(@PrimerApellido <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'PrimerApellido LIKE ''%' + CAST(@PrimerApellido AS NVARCHAR(MAX)) + '%'''   
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND PrimerApellido LIKE ''%' + CAST(@PrimerApellido AS NVARCHAR(MAX)) + '%'''  
 END   
END   
--  
IF(@SegundoApellido <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'SegundoApellido LIKE ''%' + CAST(@SegundoApellido AS NVARCHAR(MAX)) + '%'''   
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND SegundoApellido LIKE ''%' + CAST(@SegundoApellido AS NVARCHAR(MAX)) + '%'''  
 END   
END   
--  
IF(@Sexo <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Sexo IN (' + CAST(@Sexo AS NVARCHAR(MAX)) + ')'  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Sexo IN (' + CAST(@Sexo AS NVARCHAR(MAX)) + ')'  
 END   
END   
--  
IF(@TipoIdentificacion <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'TipoIdentificacion LIKE ''%' + CAST(@TipoIdentificacion AS NVARCHAR(MAX)) + '%'''   
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND TipoIdentificacion LIKE ''%' + CAST(@TipoIdentificacion AS NVARCHAR(MAX)) + '%'''  
 END   
END   
--  
IF(@Identificacion <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Identificacion = ' + CAST(@Identificacion AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Identificacion = ' + CAST(@Identificacion AS NVARCHAR(MAX))  
 END   
END   
--  
IF(@FechaNacimientoIni <> '' OR @FechaNacimientoFin <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'FechaNacimiento >= ''' + CAST(@FechaNacimientoIni AS NVARCHAR(MAX)) + ''' AND FechaNacimiento <= ''' + CAST(@FechaNacimientoFin AS NVARCHAR(MAX)) + ''''  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND FechaNacimiento >= ''' + CAST(@FechaNacimientoIni AS NVARCHAR(MAX)) + ''' AND FechaNacimiento <= ''' + CAST(@FechaNacimientoFin AS NVARCHAR(MAX)) + ''''  
 END   
END   
--  
IF(@FechaCreacionIni <> '' OR @FechaCreacionFin <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'FechaCreacion >= ''' + CAST(@FechaCreacionIni AS NVARCHAR(MAX)) + ''' AND FechaCreacion <= ''' + CAST(@FechaCreacionFin AS NVARCHAR(MAX)) + ''''  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND FechaCreacion >= ''' + CAST(@FechaCreacionIni AS NVARCHAR(MAX)) + ''' AND FechaCreacion <= ''' + CAST(@FechaCreacionFin AS NVARCHAR(MAX)) + ''''  
 END   
END   
--  
IF(@FechaAuditoriaIni <> '' OR @FechaAuditoriaFin <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'FechaAuditoria >= ''' + CAST(@FechaAuditoriaIni AS NVARCHAR(MAX)) + ''' AND FechaAuditoria <= ''' + CAST(@FechaAuditoriaFin AS NVARCHAR(MAX)) + ''''  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND FechaAuditoria >= ''' + CAST(@FechaAuditoriaIni AS NVARCHAR(MAX)) + ''' AND FechaAuditoria <= ''' + CAST(@FechaAuditoriaFin AS NVARCHAR(MAX)) + ''''  
 END   
END   
--  
IF(@FechaMinConsultaAudit <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'FechaMinConsultaAudit >= ''' + CAST(@FechaMinConsultaAudit AS NVARCHAR(MAX)) + ''  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND FechaMinConsultaAudit >= ''' + CAST(@FechaMinConsultaAudit AS NVARCHAR(MAX)) + ''  
 END   
END   
--  
IF(@FechaMaxConsultaAudit <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'FechaMaxConsultaAudit <= ''' + CAST(@FechaMaxConsultaAudit AS NVARCHAR(MAX)) + ''  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND FechaMaxConsultaAudit <= ''' + CAST(@FechaMaxConsultaAudit AS NVARCHAR(MAX)) + ''  
 END   
END   
--  
IF(@FechaAsignacionIni <> '' OR @FechaAsignacionFin <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'FechaAsignacion >= ''' + CAST(@FechaAsignacionIni AS NVARCHAR(MAX)) + ''' AND FechaAsignacion <= ''' + CAST(@FechaAsignacionFin AS NVARCHAR(MAX)) + ''''  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND FechaAsignacion >= ''' + CAST(@FechaAsignacionIni AS NVARCHAR(MAX)) + ''' AND FechaAsignacion <= ''' + CAST(@FechaAsignacionFin AS NVARCHAR(MAX)) + ''''  
 END   
END   
ElSE  
 BEGIN   
  IF(@Where = '')  
  BEGIN   
   SET @Where = @Where + 'FechaAsignacion >= ''' + CAST(GETDATE() AS NVARCHAR(MAX)) + ''' AND FechaAsignacion <= ''' + CAST(GETDATE() AS NVARCHAR(MAX)) + ''''  
  END   
  ElSE  
  BEGIN   
   SET @Where = @Where + ' AND FechaAsignacion >= ''' + CAST(GETDATE() AS NVARCHAR(MAX)) + ''' AND FechaAsignacion <= ''' + CAST(GETDATE() AS NVARCHAR(MAX)) + ''''  
  END     
 END   
--  
IF(@Activo = 'true')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Activo = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Activo = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
END   
ElSE IF(@Activo = 'false')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Activo = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Activo = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
END   
--  
IF(@Conclusion <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Conclusion LIKE ''%' + CAST(@Conclusion AS NVARCHAR(MAX)) + '%'''   
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Conclusion LIKE ''%' + CAST(@Conclusion AS NVARCHAR(MAX)) + '%'''  
 END   
END   
--  
IF(@UrlSoportes <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'UrlSoportes LIKE ''%' + CAST(@UrlSoportes AS NVARCHAR(MAX)) + '%'''   
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND UrlSoportes LIKE ''%' + CAST(@UrlSoportes AS NVARCHAR(MAX)) + '%'''  
 END   
END   
--  
IF(@Reverse <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Reverse LIKE ''%' + CAST(@Reverse AS NVARCHAR(MAX)) + '%'''   
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Reverse LIKE ''%' + CAST(@Reverse AS NVARCHAR(MAX)) + '%'''  
 END   
END   
--  
IF(@DisplayOrder <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'DisplayOrder = ' + CAST(@DisplayOrder AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND DisplayOrder = ' + CAST(@DisplayOrder AS NVARCHAR(MAX))  
 END   
END  
--  
IF(@Ara = 'true')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Ara = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Ara = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
END   
ElSE IF(@Ara = 'false')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Ara = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Ara = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
END   
--  
IF(@Eps = 'true')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Eps = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Eps = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
END   
ElSE IF(@Eps = 'false')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Eps = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Eps = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
END   
--  
IF(@FechaReversoIni <> '' OR @FechaReversoFin <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'FechaReverso >= ''' + CAST(@FechaReversoIni AS NVARCHAR(MAX)) + ''' AND FechaReverso <= ''' + CAST(@FechaReversoFin AS NVARCHAR(MAX)) + ''''  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND FechaReverso >= ''' + CAST(@FechaReversoIni AS NVARCHAR(MAX)) + ''' AND FechaReverso <= ''' + CAST(@FechaReversoFin AS NVARCHAR(MAX)) + ''''  
 END   
END   
--  
IF(@AraAtendido = 'true')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'AraAtendido = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND AraAtendido = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
END   
ElSE IF(@AraAtendido = 'false')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'AraAtendido = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND AraAtendido = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
END   
--  
IF(@EpsAtendido = 'true')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'EpsAtendido = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND EpsAtendido = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
END   
ElSE IF(@EpsAtendido = 'false')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'EpsAtendido = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND EpsAtendido = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
END   
--  
IF(@Revisar = 'true')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Revisar = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Revisar = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
END   
ElSE IF(@Revisar = 'false')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Revisar = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Revisar = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
END   
--  
IF(@Estado <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'RA.Estado IN (' + CAST(@Estado AS NVARCHAR(MAX)) + ')'  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND RA.Estado IN (' + CAST(@Estado AS NVARCHAR(MAX)) + ')'  
 END   
END   
--  
IF(@AccionLider <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'RA.AccionLider IN (' + CAST(@AccionLider AS NVARCHAR(MAX)) + ')'  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND RA.AccionLider IN (' + CAST(@AccionLider AS NVARCHAR(MAX)) + ')'  
 END   
END   
--  
IF(@AccionAuditor <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'RA.AccionAuditor IN (' + CAST(@AccionAuditor AS NVARCHAR(MAX)) + ')'  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND RA.AccionAuditor IN (' + CAST(@AccionAuditor AS NVARCHAR(MAX)) + ')'  
 END   
END   
-- 
IF(@CodigoEps <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  --SET @Where = @Where + 'RA.IdEPS IN (' + CAST(@CodigoEps AS NVARCHAR(MAX)) + ')'  
  SET @Where = @Where + 'RA.IdEPS = ''' + CAST(@CodigoEps AS NVARCHAR(MAX)) + '''' 
 END   
    ElSE  
 BEGIN   
  --SET @Where = @Where + ' AND RA.IdEPS IN (' + CAST(@CodigoEps AS NVARCHAR(MAX)) + ')'  
  SET @Where = @Where + ' AND RA.IdEPS = ''' + CAST(@CodigoEps AS NVARCHAR(MAX)) + '''' 
 END   
END   
---

--Validamos estado activo.
IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + ' RA.Status = 1 AND ME.Status = 1 AND ERA.Status = 1 AND RIC.Status = 1 AND EF.Status = 1 AND E.Status = 1 ' 
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND RA.Status = 1 AND ME.Status = 1 AND ERA.Status = 1 AND RIC.Status = 1 AND EF.Status = 1 AND E.Status = 1 '
END 
--  
  
--Paginado  
--ORDER BY RA.DisplayOrder  
--ORDER BY RA.FechaAsignacion, RA.DisplayOrder, RA.Estado    
DECLARE @Paginado VARCHAR(MAX) = '  
ORDER BY RA.FechaAsignacion, RA.DisplayOrder    
OFFSET ' + CAST(@Paginate AS NVARCHAR(MAX)) + ' ROWS  
FETCH NEXT ' + CAST(@MaxRows AS NVARCHAR(MAX)) + ' ROWS ONLY;'  
   
--Concatenamos Query, Condiciones y Paginado. Luego ejecutamos.  
IF(@Where <> '' )  
BEGIN   
 SET @Where = ' WHERE ' + @Where;                
END    
DECLARE @Total VARCHAR(MAX);  
SET @Total = @Query + '' + @Where + ' ' + @Paginado  
  
  
--Para calcular total registros filtrados.  
DECLARE @Query2 NVARCHAR(MAX);  
--SET @Query2 = N'SELECT NEWID() As Idk, COUNT(*) As NoRegistrosTotalesFiltrado   
--FROM RegistrosAuditoria RA   
--INNER JOIN Medicion ME ON (RA.IdMedicion = ME.Id)   
--INNER JOIN EstadosRegistroAuditoria ERA ON (RA.Estado = ERA.Id) '  
SET @Query2 = N'SELECT NEWID() As Idk, COUNT(*) As NoRegistrosTotalesFiltrado   
FROM RegistrosAuditoria RA  
JOIN Medicion ME ON (RA.IdMedicion = ME.Id)   
JOIN EstadosRegistroAuditoria ERA ON (RA.Estado = ERA.Id)   
JOIN AspNetUsers US ON (US.Id = RA.IdAuditor)  
JOIN AspNetUserRoles UR ON (UR.UserId = RA.IdAuditor)  
JOIN EstadoRegistroAuditoriaIcono RIC ON (RIC.Estado = RA.Estado AND RIC.RolId = UR.RoleId) AND RA.Revisar = RIC.Revisar 
JOIN Enfermedad EF ON EF.IdCobertura = ME.IdCobertura
JOIN Eps E ON (E.EpsId = RA.IdEPS) '  
  
DECLARE @Total2 NVARCHAR(MAX);  
SET @Total2 = @Query2 + '' + REPLACE(@Where, '''', '''''')  
  
--DECLARE @QueryNoRegistrosTotales NVARCHAR(MAX);  
--EXEC @QueryNoRegistrosTotales = GetCountAlertasRegistrosAuditoria @Total2;  
  
--str, old_str. new_str  
SET @Total = REPLACE(@Total, ''' '' As QueryNoRegistrosTotales', '''' + CAST(@Total2 AS NVARCHAR(MAX)) + ''' As QueryNoRegistrosTotales');  
  
--Imprimimos/Ejecutamos.  
EXEC(@Total);  
  
--PRINT(@Total); 
--PRINT(' --- ');  
--PRINT(@Total2);  
  
END --END Procedure
GO
/****** Object:  StoredProcedure [dbo].[GetRegistrosAuditoria2]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
--Original: GetRegistrosAuditoria
--DROP PROCEDURE GetRegistrosAuditoria2
CREATE  PROCEDURE [dbo].[GetRegistrosAuditoria2]
(@PageNumber INT, @MaxRows INT, @Id VARCHAR(MAX), @IdRadicado VARCHAR(MAX), @IdMedicion VARCHAR(MAX), @IdPeriodo VARCHAR(MAX), @IdAuditor VARCHAR(MAX), @IdLider VARCHAR(MAX), @PrimerNombre VARCHAR(MAX), @SegundoNombre VARCHAR(MAX), @PrimerApellido VARCHAR(MAX), @SegundoApellido VARCHAR(MAX), @Sexo VARCHAR(MAX), @TipoIdentificacion VARCHAR(MAX), @Identificacion VARCHAR(MAX), @FechaNacimientoIni VARCHAR(MAX), @FechaNacimientoFin VARCHAR(MAX), @FechaCreacionIni VARCHAR(MAX), @FechaCreacionFin VARCHAR(MAX), @FechaAuditoriaIni VARCHAR(MAX), @FechaAuditoriaFin VARCHAR(MAX), @FechaMinConsultaAudit VARCHAR(MAX), @FechaMaxConsultaAudit VARCHAR(MAX), @FechaAsignacionIni VARCHAR(MAX), @FechaAsignacionFin VARCHAR(MAX), @Activo VARCHAR(MAX), @Conclusion VARCHAR(MAX), @UrlSoportes VARCHAR(MAX), @Reverse VARCHAR(MAX), @DisplayOrder VARCHAR(MAX), @Ara VARCHAR(MAX), @Eps VARCHAR(MAX), @FechaReversoIni VARCHAR(MAX), @FechaReversoFin VARCHAR(MAX), @AraAtendido VARCHAR(MAX), @EpsAtendido VARCHAR(MAX), @Revisar VARCHAR(MAX), @Estado VARCHAR(MAX), @AccionLider VARCHAR(MAX), @AccionAuditor VARCHAR(MAX), @CodigoEps VARCHAR(MAX))
AS  
BEGIN  
SET NOCOUNT ON  
  
--Para convertir 'all' a vacio.  
IF @IdAuditor = 'all'  
BEGIN  
SET @IdAuditor = ''  
END  
  
--Guardamos Query inicial.  
DECLARE @Query VARCHAR(MAX);  

DECLARE @Ips NVARCHAR(255) = (SELECT TOP (1) Valor  FROM [dbo].[ParametrosGenerales] where Nombre = 'CampoReferencialIps')

SET @Query = 'SELECT '' '' As QueryNoRegistrosTotales, RA.Id, RA.IdRadicado, RA.IdMedicion, ME.Nombre As NombreMedicion, RA.IdPeriodo, RA.IdAuditor, 1 As IdEntidad, E.Nombre As Entidad, RA.PrimerNombre, RA.SegundoNombre, RA.PrimerApellido, RA.SegundoApellido, RA.Sexo, RA.TipoIdentificacion, RA.Identificacion, RA.FechaNacimiento, RA.FechaCreacion, RA.FechaAuditoria, RA.FechaMinConsultaAudit, RA.FechaMaxConsultaAudit, RA.FechaAsignacion, RA.Activo, RA.Conclusion, RA.UrlSoportes, RA.Reverse, RA.DisplayOrder, RA.Ara, RA.Eps, RA.FechaReverso, RA.AraAtendido, RA.EpsAtendido, RA.Revisar, RA.Extemporaneo, RA.Estado, ERA.Codigo As EstadoCodigo, ERA.Nombre As EstadoNombre, RA.LevantarGlosa AS LevantarGlosa, RA.MantenerCalificacion AS MantenerCalificacion, RA.ComiteExperto AS ComiteExperto, RA.ComiteAdministrativo AS ComiteAdministrativo, RA.AccionLider AS AccionLider, RA.AccionAuditor AS AccionAuditor, RA.Encuesta As Encuesta, US.UserName as CreatedBy, RA.CreatedDate, US.UserName as ModifyBy, RA.ModifyDate, RIC.ImagePath, US.UserName as NombreAuditor, ME.IdCobertura, EF.nombre as EnfermedadMadre, RA.IdEPS As Data_IdEPS, E.Nombre As Data_NombreEPS, IPS.IPS
FROM RegistrosAuditoria RA   
JOIN Medicion ME ON (RA.IdMedicion = ME.Id)   
JOIN EstadosRegistroAuditoria ERA ON (RA.Estado = ERA.Id)   
JOIN AspNetUsers US ON (US.Id = RA.IdAuditor)  
JOIN AspNetUserRoles UR ON (UR.UserId = RA.IdAuditor)  
JOIN EstadoRegistroAuditoriaIcono RIC ON (RIC.Estado = RA.Estado AND RIC.RolId = UR.RoleId) AND RA.Revisar = RIC.Revisar 
JOIN Enfermedad EF ON EF.idCobertura = ME.IdCobertura
JOIN Eps E ON (E.EpsId = RA.IdEPS)   
LEFT JOIN (SELECT TOP 1 rad.RegistrosAuditoriaId, cc.ItemDescripcion as IPS 
FROM RegistrosAuditoriaDetalle rad
inner join Variables va on va.Id = rad.VariableId
inner join CatalogoCobertura c on c.NombreCatalogo = va.tablaReferencial
inner join CatalogoItemCobertura cc on c.Id = cc.CatalogoCoberturaId
where va.campoReferencial =''' + @Ips + ''') IPS on IPS.RegistrosAuditoriaId = RA.Id'
  
-- Si se consulta EPS es necesario agregar un join   
--BEGIN   
-- IF(@CodigoEps <> '')  
-- BEGIN                                  -- Id Variable EPS  
--  SET @Query = @Query + 'INNER JOIN RegistrosAuditoriaDetalle RD ON RA.Id = rd.RegistrosAuditoriaId and RD.VariableId = 35 '      + 'AND RD.DatoReportado = ''' + CAST(@CodigoEps AS NVARCHAR(MAX)) + ''''    
-- END   
--END   
  
--Calculo de paginado.  
--SET @PageNumber = @PageNumber - 1;  
DECLARE @Paginate INT = @PageNumber * @MaxRows;  
  
--Guardamos Condiciones.  
DECLARE @Where VARCHAR(MAX) = '';  
IF(@Id <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'RA.Id IN (' + CAST(@Id AS NVARCHAR(MAX)) + ')'  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND RA.Id IN (' + CAST(@Id AS NVARCHAR(MAX)) + ')'  
 END   
END   
--  
IF(@IdRadicado <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'IdRadicado LIKE ''%' + CAST(@IdRadicado AS NVARCHAR(MAX)) + '%'''   
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND IdRadicado LIKE ''%' + CAST(@IdRadicado AS NVARCHAR(MAX)) + '%'''  
 END   
END   
--  
IF(@IdMedicion <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'RA.IdMedicion IN (' + CAST(@IdMedicion AS NVARCHAR(MAX)) + ')'  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND RA.IdMedicion IN (' + CAST(@IdMedicion AS NVARCHAR(MAX)) + ')'  
 END   
END   
--  
IF(@IdPeriodo <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'RA.IdPeriodo = ' + CAST(@IdPeriodo AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND RA.IdPeriodo = ' + CAST(@IdPeriodo AS NVARCHAR(MAX))  
 END   
END   
--  
IF(@IdAuditor <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'RA.IdAuditor IN (''' + CAST(@IdAuditor AS NVARCHAR(MAX)) + ''')'  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND RA.IdAuditor IN (''' + CAST(@IdAuditor AS NVARCHAR(MAX)) + ''')'  
 END   
END   
ELSE
BEGIN

IF(@IdLider <> '')  
BEGIN   
  
 declare @countLider int = 0, @registerlider int = 1  
 declare @tablelider as table   
 (  
  Id int not null primary key identity(1,1),  
  IdCobertura int not null  
 )  
 insert @tablelider Select IdCobertura from UsuarioXEnfermedad --where IdUsuario = @IdLider  
 select @countLider = COUNT(*) from @tablelider   
  
 DECLARE @resultlider varchar(200) = ''  
  
 WHILE(@registerlider <= @countLider)  
 BEGIN   
  declare @regtemp INT = ''  
  SELECT @regtemp = IdCobertura FROM @tablelider WHERE Id = @registerlider  
  SET @resultlider = @resultlider + CAST(@regtemp AS varchar(2)) + ','   
  SET @registerlider = @registerlider + 1  
 END   
  
 SET @resultlider = @resultlider + '0'  
  
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'ME.IdCobertura IN (' +  @resultlider + ')'  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND ME.IdCobertura IN (' + @resultlider + ')'  
 END   
END 


END
--  
IF(@PrimerNombre <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'PrimerNombre LIKE ''%' + CAST(@PrimerNombre AS NVARCHAR(MAX)) + '%'''   
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND PrimerNombre LIKE ''%' + CAST(@PrimerNombre AS NVARCHAR(MAX)) + '%'''  
 END   
END   
--  
IF(@SegundoNombre <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'SegundoNombre LIKE ''%' + CAST(@SegundoNombre AS NVARCHAR(MAX)) + '%'''   
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND SegundoNombre LIKE ''%' + CAST(@SegundoNombre AS NVARCHAR(MAX)) + '%'''  
 END   
END   
--  
IF(@PrimerApellido <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'PrimerApellido LIKE ''%' + CAST(@PrimerApellido AS NVARCHAR(MAX)) + '%'''   
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND PrimerApellido LIKE ''%' + CAST(@PrimerApellido AS NVARCHAR(MAX)) + '%'''  
 END   
END   
--  
IF(@SegundoApellido <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'SegundoApellido LIKE ''%' + CAST(@SegundoApellido AS NVARCHAR(MAX)) + '%'''   
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND SegundoApellido LIKE ''%' + CAST(@SegundoApellido AS NVARCHAR(MAX)) + '%'''  
 END   
END   
--  
IF(@Sexo <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Sexo IN (' + CAST(@Sexo AS NVARCHAR(MAX)) + ')'  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Sexo IN (' + CAST(@Sexo AS NVARCHAR(MAX)) + ')'  
 END   
END   
--  
IF(@TipoIdentificacion <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'TipoIdentificacion LIKE ''%' + CAST(@TipoIdentificacion AS NVARCHAR(MAX)) + '%'''   
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND TipoIdentificacion LIKE ''%' + CAST(@TipoIdentificacion AS NVARCHAR(MAX)) + '%'''  
 END   
END   
--  
IF(@Identificacion <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Identificacion = ' + CAST(@Identificacion AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Identificacion = ' + CAST(@Identificacion AS NVARCHAR(MAX))  
 END   
END   
--  
IF(@FechaNacimientoIni <> '' OR @FechaNacimientoFin <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'FechaNacimiento >= ''' + CAST(@FechaNacimientoIni AS NVARCHAR(MAX)) + ''' AND FechaNacimiento <= ''' + CAST(@FechaNacimientoFin AS NVARCHAR(MAX)) + ''''  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND FechaNacimiento >= ''' + CAST(@FechaNacimientoIni AS NVARCHAR(MAX)) + ''' AND FechaNacimiento <= ''' + CAST(@FechaNacimientoFin AS NVARCHAR(MAX)) + ''''  
 END   
END   
--  
IF(@FechaCreacionIni <> '' OR @FechaCreacionFin <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'FechaCreacion >= ''' + CAST(@FechaCreacionIni AS NVARCHAR(MAX)) + ''' AND FechaCreacion <= ''' + CAST(@FechaCreacionFin AS NVARCHAR(MAX)) + ''''  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND FechaCreacion >= ''' + CAST(@FechaCreacionIni AS NVARCHAR(MAX)) + ''' AND FechaCreacion <= ''' + CAST(@FechaCreacionFin AS NVARCHAR(MAX)) + ''''  
 END   
END   
--  
IF(@FechaAuditoriaIni <> '' OR @FechaAuditoriaFin <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'FechaAuditoria >= ''' + CAST(@FechaAuditoriaIni AS NVARCHAR(MAX)) + ''' AND FechaAuditoria <= ''' + CAST(@FechaAuditoriaFin AS NVARCHAR(MAX)) + ''''  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND FechaAuditoria >= ''' + CAST(@FechaAuditoriaIni AS NVARCHAR(MAX)) + ''' AND FechaAuditoria <= ''' + CAST(@FechaAuditoriaFin AS NVARCHAR(MAX)) + ''''  
 END   
END   
--  
IF(@FechaMinConsultaAudit <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'FechaMinConsultaAudit >= ''' + CAST(@FechaMinConsultaAudit AS NVARCHAR(MAX)) + ''  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND FechaMinConsultaAudit >= ''' + CAST(@FechaMinConsultaAudit AS NVARCHAR(MAX)) + ''  
 END   
END   
--  
IF(@FechaMaxConsultaAudit <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'FechaMaxConsultaAudit <= ''' + CAST(@FechaMaxConsultaAudit AS NVARCHAR(MAX)) + ''  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND FechaMaxConsultaAudit <= ''' + CAST(@FechaMaxConsultaAudit AS NVARCHAR(MAX)) + ''  
 END   
END   
--  
IF(@FechaAsignacionIni <> '' OR @FechaAsignacionFin <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'FechaAsignacion >= ''' + CAST(@FechaAsignacionIni AS NVARCHAR(MAX)) + ''' AND FechaAsignacion <= ''' + CAST(@FechaAsignacionFin AS NVARCHAR(MAX)) + ''''  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND FechaAsignacion >= ''' + CAST(@FechaAsignacionIni AS NVARCHAR(MAX)) + ''' AND FechaAsignacion <= ''' + CAST(@FechaAsignacionFin AS NVARCHAR(MAX)) + ''''  
 END   
END   
ElSE  
 BEGIN   
  IF(@Where = '')  
  BEGIN   
   SET @Where = @Where + 'FechaAsignacion >= ''' + CAST(GETDATE() AS NVARCHAR(MAX)) + ''' AND FechaAsignacion <= ''' + CAST(GETDATE() AS NVARCHAR(MAX)) + ''''  
  END   
  ElSE  
  BEGIN   
   SET @Where = @Where + ' AND FechaAsignacion >= ''' + CAST(GETDATE() AS NVARCHAR(MAX)) + ''' AND FechaAsignacion <= ''' + CAST(GETDATE() AS NVARCHAR(MAX)) + ''''  
  END     
 END   
--  
IF(@Activo = 'true')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Activo = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Activo = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
END   
ElSE IF(@Activo = 'false')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Activo = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Activo = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
END   
--  
IF(@Conclusion <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Conclusion LIKE ''%' + CAST(@Conclusion AS NVARCHAR(MAX)) + '%'''   
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Conclusion LIKE ''%' + CAST(@Conclusion AS NVARCHAR(MAX)) + '%'''  
 END   
END   
--  
IF(@UrlSoportes <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'UrlSoportes LIKE ''%' + CAST(@UrlSoportes AS NVARCHAR(MAX)) + '%'''   
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND UrlSoportes LIKE ''%' + CAST(@UrlSoportes AS NVARCHAR(MAX)) + '%'''  
 END   
END   
--  
IF(@Reverse <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Reverse LIKE ''%' + CAST(@Reverse AS NVARCHAR(MAX)) + '%'''   
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Reverse LIKE ''%' + CAST(@Reverse AS NVARCHAR(MAX)) + '%'''  
 END   
END   
--  
IF(@DisplayOrder <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'DisplayOrder = ' + CAST(@DisplayOrder AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND DisplayOrder = ' + CAST(@DisplayOrder AS NVARCHAR(MAX))  
 END   
END  
--  
IF(@Ara = 'true')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Ara = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Ara = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
END   
ElSE IF(@Ara = 'false')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Ara = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Ara = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
END   
--  
IF(@Eps = 'true')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Eps = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Eps = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
END   
ElSE IF(@Eps = 'false')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Eps = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Eps = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
END   
--  
IF(@FechaReversoIni <> '' OR @FechaReversoFin <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'FechaReverso >= ''' + CAST(@FechaReversoIni AS NVARCHAR(MAX)) + ''' AND FechaReverso <= ''' + CAST(@FechaReversoFin AS NVARCHAR(MAX)) + ''''  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND FechaReverso >= ''' + CAST(@FechaReversoIni AS NVARCHAR(MAX)) + ''' AND FechaReverso <= ''' + CAST(@FechaReversoFin AS NVARCHAR(MAX)) + ''''  
 END   
END   
--  
IF(@AraAtendido = 'true')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'AraAtendido = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND AraAtendido = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
END   
ElSE IF(@AraAtendido = 'false')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'AraAtendido = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND AraAtendido = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
END   
--  
IF(@EpsAtendido = 'true')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'EpsAtendido = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND EpsAtendido = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
END   
ElSE IF(@EpsAtendido = 'false')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'EpsAtendido = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND EpsAtendido = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
END   
--  
IF(@Revisar = 'true')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Revisar = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Revisar = ' + CAST(1 AS NVARCHAR(MAX))  
 END   
END   
ElSE IF(@Revisar = 'false')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'Revisar = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND Revisar = ' + CAST(0 AS NVARCHAR(MAX))  
 END   
END   
--  
IF(@Estado <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'RA.Estado IN (' + CAST(@Estado AS NVARCHAR(MAX)) + ')'  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND RA.Estado IN (' + CAST(@Estado AS NVARCHAR(MAX)) + ')'  
 END   
END   
--  
IF(@AccionLider <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'RA.AccionLider IN (' + CAST(@AccionLider AS NVARCHAR(MAX)) + ')'  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND RA.AccionLider IN (' + CAST(@AccionLider AS NVARCHAR(MAX)) + ')'  
 END   
END   
--  
IF(@AccionAuditor <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'RA.AccionAuditor IN (' + CAST(@AccionAuditor AS NVARCHAR(MAX)) + ')'  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND RA.AccionAuditor IN (' + CAST(@AccionAuditor AS NVARCHAR(MAX)) + ')'  
 END   
END   
-- 
IF(@CodigoEps <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  --SET @Where = @Where + 'RA.IdEPS IN (' + CAST(@CodigoEps AS NVARCHAR(MAX)) + ')'  
  SET @Where = @Where + 'RA.IdEPS = ''' + CAST(@CodigoEps AS NVARCHAR(MAX)) + '''' 
 END   
    ElSE  
 BEGIN   
  --SET @Where = @Where + ' AND RA.IdEPS IN (' + CAST(@CodigoEps AS NVARCHAR(MAX)) + ')'  
  SET @Where = @Where + ' AND RA.IdEPS = ''' + CAST(@CodigoEps AS NVARCHAR(MAX)) + '''' 
 END   
END   
---

--Validamos estado activo.
IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + ' RA.Status = 1 AND ME.Status = 1 AND ERA.Status = 1 AND RIC.Status = 1 AND EF.Status = 1 AND E.Status = 1 ' 
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND RA.Status = 1 AND ME.Status = 1 AND ERA.Status = 1 AND RIC.Status = 1 AND EF.Status = 1 AND E.Status = 1 '
END 
--  
  
--Paginado  
--ORDER BY RA.DisplayOrder  
--ORDER BY RA.FechaAsignacion, RA.DisplayOrder, RA.Estado    
DECLARE @Paginado VARCHAR(MAX) = '  
ORDER BY RA.FechaAsignacion, RA.DisplayOrder    
OFFSET ' + CAST(@Paginate AS NVARCHAR(MAX)) + ' ROWS  
FETCH NEXT ' + CAST(@MaxRows AS NVARCHAR(MAX)) + ' ROWS ONLY;'  
   
--Concatenamos Query, Condiciones y Paginado. Luego ejecutamos.  
IF(@Where <> '' )  
BEGIN   
 SET @Where = ' WHERE ' + @Where;                
END    
DECLARE @Total VARCHAR(MAX);  
SET @Total = @Query + '' + @Where + ' ' + @Paginado  
  
  
--Para calcular total registros filtrados.  
DECLARE @Query2 NVARCHAR(MAX);  
--SET @Query2 = N'SELECT NEWID() As Idk, COUNT(*) As NoRegistrosTotalesFiltrado   
--FROM RegistrosAuditoria RA   
--INNER JOIN Medicion ME ON (RA.IdMedicion = ME.Id)   
--INNER JOIN EstadosRegistroAuditoria ERA ON (RA.Estado = ERA.Id) '  
SET @Query2 = N'SELECT NEWID() As Idk, COUNT(*) As NoRegistrosTotalesFiltrado   
FROM RegistrosAuditoria RA  
JOIN Medicion ME ON (RA.IdMedicion = ME.Id)   
JOIN EstadosRegistroAuditoria ERA ON (RA.Estado = ERA.Id)   
JOIN AspNetUsers US ON (US.Id = RA.IdAuditor)  
JOIN AspNetUserRoles UR ON (UR.UserId = RA.IdAuditor)  
JOIN EstadoRegistroAuditoriaIcono RIC ON (RIC.Estado = RA.Estado AND RIC.RolId = UR.RoleId) AND RA.Revisar = RIC.Revisar 
JOIN Enfermedad EF ON EF.IdCobertura = ME.IdCobertura
JOIN Eps E ON (E.EpsId = RA.IdEPS) '  
  
DECLARE @Total2 NVARCHAR(MAX);  
SET @Total2 = @Query2 + '' + REPLACE(@Where, '''', '''''')  
  
--DECLARE @QueryNoRegistrosTotales NVARCHAR(MAX);  
--EXEC @QueryNoRegistrosTotales = GetCountAlertasRegistrosAuditoria @Total2;  
  
--str, old_str. new_str  
SET @Total = REPLACE(@Total, ''' '' As QueryNoRegistrosTotales', '''' + CAST(@Total2 AS NVARCHAR(MAX)) + ''' As QueryNoRegistrosTotales');  
  
--Imprimimos/Ejecutamos.  
select @Total;  
  
--PRINT(@Total); 
--PRINT(' --- ');  
--PRINT(@Total2);  
  
END --END Procedure
GO
/****** Object:  StoredProcedure [dbo].[GetRegistrosAuditoriaPrueba]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
--Original: GetRegistrosAuditoria
--DROP PROCEDURE GetRegistrosAuditoria2
CREATE  PROCEDURE [dbo].[GetRegistrosAuditoriaPrueba]
(@PageNumber INT, 
@MaxRows INT, 
@Id VARCHAR(MAX), 
@IdRadicado VARCHAR(MAX), 
@IdMedicion VARCHAR(MAX), 
@IdPeriodo VARCHAR(MAX), 
@IdAuditor VARCHAR(MAX), 
@PrimerNombre VARCHAR(MAX), 
@SegundoNombre VARCHAR(MAX), 
@PrimerApellido VARCHAR(MAX), 
@SegundoApellido VARCHAR(MAX), 
@Sexo VARCHAR(MAX), 
@TipoIdentificacion VARCHAR(MAX), 
@Identificacion VARCHAR(MAX), 
@FechaNacimientoIni VARCHAR(MAX),
@FechaNacimientoFin VARCHAR(MAX), 
@FechaCreacionIni VARCHAR(MAX), 
@FechaCreacionFin VARCHAR(MAX), 
@FechaAuditoriaIni VARCHAR(MAX), 
@FechaAuditoriaFin VARCHAR(MAX), 
@FechaMinConsultaAudit VARCHAR(MAX), 
@FechaMaxConsultaAudit VARCHAR(MAX), 
@FechaAsignacionIni VARCHAR(MAX), 
@FechaAsignacionFin VARCHAR(MAX), 
@Activo VARCHAR(MAX), 
@Conclusion VARCHAR(MAX), 
@UrlSoportes VARCHAR(MAX), 
@Reverse VARCHAR(MAX), 
@DisplayOrder VARCHAR(MAX), 
@Ara VARCHAR(MAX), @Eps VARCHAR(MAX), 
@FechaReversoIni VARCHAR(MAX), 
@FechaReversoFin VARCHAR(MAX), 
@AraAtendido VARCHAR(MAX), 
@EpsAtendido VARCHAR(MAX), 
@Revisar VARCHAR(MAX), 
@Estado VARCHAR(MAX), 
@AccionLider VARCHAR(MAX), 
@AccionAuditor VARCHAR(MAX), 
@CodigoEps VARCHAR(MAX))
AS
BEGIN
SET NOCOUNT ON

--Guardamos Query inicial.
DECLARE @Query VARCHAR(MAX);
DECLARE @RolUser VARCHAR;

SET @RolUser = (select RoleId from AspNetUserRoles where UserId = @IdAuditor);

--SET @Query = 'SELECT '' '' As QueryNoRegistrosTotales, RA.Id, RA.IdRadicado, RA.IdMedicion, ME.Descripcion As NombreMedicion, RA.IdPeriodo, RA.IdAuditor, 1 As IdEntidad, ''EntidadFakeName'' As Entidad, RA.PrimerNombre, RA.SegundoNombre, RA.PrimerApellido, RA.SegundoApellido, RA.Sexo, RA.TipoIdentificacion, RA.Identificacion, RA.FechaNacimiento, RA.FechaCreacion, RA.FechaAuditoria, RA.FechaMinConsultaAudit, RA.FechaMaxConsultaAudit, RA.FechaAsignacion, RA.Activo, RA.Conclusion, RA.UrlSoportes, RA.Reverse, RA.DisplayOrder, RA.Ara, RA.Eps, RA.FechaReverso, RA.AraAtendido, RA.EpsAtendido, RA.Revisar, RA.Extemporaneo, RA.Estado, ERA.Codigo As EstadoCodigo, ERA.Nombre As EstadoNombre, RA.LevantarGlosa AS LevantarGlosa, RA.MantenerCalificacion AS MantenerCalificacion, RA.ComiteExperto AS ComiteExperto, RA.ComiteAdministrativo AS ComiteAdministrativo, RA.AccionLider AS AccionLider, RA.AccionAuditor AS AccionAuditor, RA.Encuesta As Encuesta, RA.CreatedBy, RA.CreatedDate, RA.ModifyBy, RA.ModifyDate
--FROM RegistrosAuditoria RA 
--INNER JOIN Medicion ME ON (RA.IdMedicion = ME.Id) 
--INNER JOIN EstadosRegistroAuditoria ERA ON (RA.Estado = ERA.Id) 
--INNER JOIN AspNetUsers US ON (US.Id = RA.IdAuditor) '
SET @Query = 'SELECT '' '' As QueryNoRegistrosTotales, RA.Id, RA.IdRadicado,UR.RoleId As Rol, RA.IdMedicion, ME.Descripcion As NombreMedicion, RA.IdPeriodo, RA.IdAuditor, 1 As IdEntidad, ''EntidadFakeName'' As Entidad, RA.PrimerNombre, RA.SegundoNombre, RA.PrimerApellido, RA.SegundoApellido, RA.Sexo, RA.TipoIdentificacion, RA.Identificacion, RA.FechaNacimiento, RA.FechaCreacion, RA.FechaAuditoria, RA.FechaMinConsultaAudit, RA.FechaMaxConsultaAudit, RA.FechaAsignacion, RA.Activo, RA.Conclusion, RA.UrlSoportes, RA.Reverse, RA.DisplayOrder, RA.Ara, RA.Eps, RA.FechaReverso, RA.AraAtendido, RA.EpsAtendido, RA.Revisar, RA.Extemporaneo, RA.Estado, ERA.Codigo As EstadoCodigo, ERA.Nombre As EstadoNombre, RA.LevantarGlosa AS LevantarGlosa, RA.MantenerCalificacion AS MantenerCalificacion, RA.ComiteExperto AS ComiteExperto, RA.ComiteAdministrativo AS ComiteAdministrativo, RA.AccionLider AS AccionLider, RA.AccionAuditor AS AccionAuditor, RA.Encuesta As Encuesta, RA.CreatedBy, RA.CreatedDate, RA.ModifyBy, RA.ModifyDate, RIC.ImagePath 
FROM RegistrosAuditoria RA 
INNER JOIN Medicion ME ON (RA.IdMedicion = ME.Id) 
INNER JOIN EstadosRegistroAuditoria ERA ON (RA.Estado = ERA.Id) 
INNER JOIN AspNetUsers US ON (US.Id = RA.IdAuditor)
INNER JOIN AspNetUserRoles UR ON (UR.UserId = RA.IdAuditor)
INNER JOIN EstadoRegistroAuditoriaIcono RIC ON (RIC.Estado = RA.Estado AND RIC.RolId = UR.RoleId) AND RA.Revisar = RIC.Revisar '

-- Si se consulta EPS es necesario agregar un join 
BEGIN 
	IF(@CodigoEps <> '')
	BEGIN																													     -- Id Variable EPS
		SET @Query = @Query + 'INNER JOIN RegistrosAuditoriaDetalle RD ON RA.Id = rd.RegistrosAuditoriaId and RD.VariableId = 35 '      + 'AND RD.DatoReportado = ''' + CAST(@CodigoEps AS NVARCHAR(MAX)) + ''''  
	END 
END 

--Calculo de paginado.
--SET @PageNumber = @PageNumber - 1;
DECLARE @Paginate INT = @PageNumber * @MaxRows;

--Guardamos Condiciones.
DECLARE @Where VARCHAR(MAX) = '';
IF(@Id <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.Id IN (' + CAST(@Id AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.Id IN (' + CAST(@Id AS NVARCHAR(MAX)) + ')'
	END 
END 
--
IF(@IdRadicado <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'IdRadicado LIKE ''%' + CAST(@IdRadicado AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND IdRadicado LIKE ''%' + CAST(@IdRadicado AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@IdMedicion <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.IdMedicion IN (' + CAST(@IdMedicion AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.IdMedicion IN (' + CAST(@IdMedicion AS NVARCHAR(MAX)) + ')'
	END 
END 
--
IF(@IdPeriodo <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.IdPeriodo = ' + CAST(@IdPeriodo AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.IdPeriodo = ' + CAST(@IdPeriodo AS NVARCHAR(MAX))
	END 
END 
--
IF(@IdAuditor <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.IdAuditor IN (''' + CAST(@IdAuditor AS NVARCHAR(MAX)) + ''')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.IdAuditor IN (''' + CAST(@IdAuditor AS NVARCHAR(MAX)) + ''')'
	END 
END 
--
IF(@PrimerNombre <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'PrimerNombre LIKE ''%' + CAST(@PrimerNombre AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND PrimerNombre LIKE ''%' + CAST(@PrimerNombre AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@SegundoNombre <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'SegundoNombre LIKE ''%' + CAST(@SegundoNombre AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND SegundoNombre LIKE ''%' + CAST(@SegundoNombre AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@PrimerApellido <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'PrimerApellido LIKE ''%' + CAST(@PrimerApellido AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND PrimerApellido LIKE ''%' + CAST(@PrimerApellido AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@SegundoApellido <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'SegundoApellido LIKE ''%' + CAST(@SegundoApellido AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND SegundoApellido LIKE ''%' + CAST(@SegundoApellido AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@Sexo <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'Sexo IN (' + CAST(@Sexo AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND Sexo IN (' + CAST(@Sexo AS NVARCHAR(MAX)) + ')'
	END 
END 
--
IF(@TipoIdentificacion <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'TipoIdentificacion LIKE ''%' + CAST(@TipoIdentificacion AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND TipoIdentificacion LIKE ''%' + CAST(@TipoIdentificacion AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@Identificacion <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'Identificacion = ' + CAST(@Identificacion AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND Identificacion = ' + CAST(@Identificacion AS NVARCHAR(MAX))
	END 
END 
--
IF(@FechaNacimientoIni <> '' OR @FechaNacimientoFin <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'FechaNacimiento >= ''' + CAST(@FechaNacimientoIni AS NVARCHAR(MAX)) + ''' AND FechaNacimiento <= ''' + CAST(@FechaNacimientoFin AS NVARCHAR(MAX)) + ''''
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND FechaNacimiento >= ''' + CAST(@FechaNacimientoIni AS NVARCHAR(MAX)) + ''' AND FechaNacimiento <= ''' + CAST(@FechaNacimientoFin AS NVARCHAR(MAX)) + ''''
	END 
END 
--
IF(@FechaCreacionIni <> '' OR @FechaCreacionFin <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'FechaCreacion >= ''' + CAST(@FechaCreacionIni AS NVARCHAR(MAX)) + ''' AND FechaCreacion <= ''' + CAST(@FechaCreacionFin AS NVARCHAR(MAX)) + ''''
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND FechaCreacion >= ''' + CAST(@FechaCreacionIni AS NVARCHAR(MAX)) + ''' AND FechaCreacion <= ''' + CAST(@FechaCreacionFin AS NVARCHAR(MAX)) + ''''
	END 
END 
--
IF(@FechaAuditoriaIni <> '' OR @FechaAuditoriaFin <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'FechaAuditoria >= ''' + CAST(@FechaAuditoriaIni AS NVARCHAR(MAX)) + ''' AND FechaAuditoria <= ''' + CAST(@FechaAuditoriaFin AS NVARCHAR(MAX)) + ''''
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND FechaAuditoria >= ''' + CAST(@FechaAuditoriaIni AS NVARCHAR(MAX)) + ''' AND FechaAuditoria <= ''' + CAST(@FechaAuditoriaFin AS NVARCHAR(MAX)) + ''''
	END 
END 
--
IF(@FechaMinConsultaAudit <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'FechaMinConsultaAudit >= ''' + CAST(@FechaMinConsultaAudit AS NVARCHAR(MAX)) + ''
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND FechaMinConsultaAudit >= ''' + CAST(@FechaMinConsultaAudit AS NVARCHAR(MAX)) + ''
	END 
END 
--
IF(@FechaMaxConsultaAudit <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'FechaMaxConsultaAudit <= ''' + CAST(@FechaMaxConsultaAudit AS NVARCHAR(MAX)) + ''
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND FechaMaxConsultaAudit <= ''' + CAST(@FechaMaxConsultaAudit AS NVARCHAR(MAX)) + ''
	END 
END 
--
IF(@FechaAsignacionIni <> '' OR @FechaAsignacionFin <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'FechaAsignacion >= ''' + CAST(@FechaAsignacionIni AS NVARCHAR(MAX)) + ''' AND FechaAsignacion <= ''' + CAST(@FechaAsignacionFin AS NVARCHAR(MAX)) + ''''
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND FechaAsignacion >= ''' + CAST(@FechaAsignacionIni AS NVARCHAR(MAX)) + ''' AND FechaAsignacion <= ''' + CAST(@FechaAsignacionFin AS NVARCHAR(MAX)) + ''''
	END 
END 
ElSE
	BEGIN 
		SET @Where = @Where + ' AND FechaAsignacion >= ''' + CAST(GETDATE() AS NVARCHAR(MAX)) + ''' AND FechaAsignacion <= ''' + CAST(GETDATE() AS NVARCHAR(MAX)) + ''''
	END 
--
IF(@Activo = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'Activo = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND Activo = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@Activo = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'Activo = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND Activo = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@Conclusion <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'Conclusion LIKE ''%' + CAST(@Conclusion AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND Conclusion LIKE ''%' + CAST(@Conclusion AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@UrlSoportes <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'UrlSoportes LIKE ''%' + CAST(@UrlSoportes AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND UrlSoportes LIKE ''%' + CAST(@UrlSoportes AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@Reverse <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'Reverse LIKE ''%' + CAST(@Reverse AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND Reverse LIKE ''%' + CAST(@Reverse AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@DisplayOrder <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'DisplayOrder = ' + CAST(@DisplayOrder AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND DisplayOrder = ' + CAST(@DisplayOrder AS NVARCHAR(MAX))
	END 
END
--
IF(@Ara = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'Ara = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND Ara = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@Ara = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'Ara = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND Ara = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@Eps = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'Eps = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND Eps = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@Eps = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'Eps = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND Eps = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@FechaReversoIni <> '' OR @FechaReversoFin <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'FechaReverso >= ''' + CAST(@FechaReversoIni AS NVARCHAR(MAX)) + ''' AND FechaReverso <= ''' + CAST(@FechaReversoFin AS NVARCHAR(MAX)) + ''''
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND FechaReverso >= ''' + CAST(@FechaReversoIni AS NVARCHAR(MAX)) + ''' AND FechaReverso <= ''' + CAST(@FechaReversoFin AS NVARCHAR(MAX)) + ''''
	END 
END 
--
IF(@AraAtendido = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'AraAtendido = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND AraAtendido = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@AraAtendido = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'AraAtendido = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND AraAtendido = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@EpsAtendido = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'EpsAtendido = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND EpsAtendido = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@EpsAtendido = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'EpsAtendido = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND EpsAtendido = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@Revisar = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'Revisar = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND Revisar = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@Revisar = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'Revisar = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND Revisar = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@Estado <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.Estado IN (' + CAST(@Estado AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.Estado IN (' + CAST(@Estado AS NVARCHAR(MAX)) + ')'
	END 
END 
--










IF(@RolUser <> '')
BEGIN     
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + '(UR.RoleId = 3 AND RA.Estado not in (5,6,9,10,14)) OR (UR.RoleId = 2 AND RA.Estado not in (1,3,5,7,11,12,15,17))'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND (UR.RoleId = 3 AND RA.Estado not in (5,6,9,10,14)) OR (UR.RoleId = 2 AND RA.Estado not in (1,3,5,7,11,12,15,17))'
	END 
END










--
IF(@AccionLider <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.AccionLider IN (' + CAST(@AccionLider AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.AccionLider IN (' + CAST(@AccionLider AS NVARCHAR(MAX)) + ')'
	END 
END 
--
IF(@AccionAuditor <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.AccionAuditor IN (' + CAST(@AccionAuditor AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.AccionAuditor IN (' + CAST(@AccionAuditor AS NVARCHAR(MAX)) + ')'
	END 
END 
--

--Paginado
--ORDER BY RA.DisplayOrder
--ORDER BY RA.FechaAsignacion, RA.DisplayOrder, RA.Estado  
DECLARE @Paginado VARCHAR(MAX) = '
ORDER BY RA.FechaAsignacion, RA.DisplayOrder  
OFFSET ' + CAST(@Paginate AS NVARCHAR(MAX)) + ' ROWS
FETCH NEXT ' + CAST(@MaxRows AS NVARCHAR(MAX)) + ' ROWS ONLY;'
 
--Concatenamos Query, Condiciones y Paginado. Luego ejecutamos.
IF(@Where <> '' )
BEGIN 
	SET @Where = ' WHERE ' + @Where;              
END  
DECLARE @Total VARCHAR(MAX);
SET @Total = @Query + '' + @Where + ' ' + @Paginado


--Para calcular total registros filtrados.
DECLARE @Query2 NVARCHAR(MAX);
--SET @Query2 = N'SELECT NEWID() As Idk, COUNT(*) As NoRegistrosTotalesFiltrado 
--FROM RegistrosAuditoria RA 
--INNER JOIN Medicion ME ON (RA.IdMedicion = ME.Id) 
--INNER JOIN EstadosRegistroAuditoria ERA ON (RA.Estado = ERA.Id) '
SET @Query2 = N'SELECT NEWID() As Idk, COUNT(*) As NoRegistrosTotalesFiltrado 
FROM RegistrosAuditoria RA 
INNER JOIN Medicion ME ON (RA.IdMedicion = ME.Id) 
INNER JOIN EstadosRegistroAuditoria ERA ON (RA.Estado = ERA.Id) 
INNER JOIN AspNetUsers US ON (US.Id = RA.IdAuditor)
INNER JOIN AspNetUserRoles UR ON (UR.UserId = RA.IdAuditor)
INNER JOIN EstadoRegistroAuditoriaIcono RIC ON (RIC.Estado = RA.Estado AND RIC.RolId = UR.RoleId) AND RA.Revisar = RIC.Revisar '

DECLARE @Total2 NVARCHAR(MAX);
SET @Total2 = @Query2 + '' + REPLACE(@Where, '''', '''''')

--DECLARE @QueryNoRegistrosTotales NVARCHAR(MAX);
--EXEC @QueryNoRegistrosTotales = GetCountAlertasRegistrosAuditoria @Total2;

--str, old_str. new_str
SET @Total = REPLACE(@Total, ''' '' As QueryNoRegistrosTotales', '''' + CAST(@Total2 AS NVARCHAR(MAX)) + ''' As QueryNoRegistrosTotales');

--Imprimimos/Ejecutamos.
PRINT(@RolUser);

EXEC(@Total);

--PRINT(@Total);
--PRINT(' --- ');
--PRINT(@Total2);

END --END Procedure
GO
/****** Object:  StoredProcedure [dbo].[GetRegistrosXMedicion]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRegistrosXMedicion]
(@IdMedicion VARCHAR(MAX))
AS
BEGIN
SET NOCOUNT ON

--Guardamos Query inicial.
DECLARE @Query VARCHAR(MAX);

SET @Query = 'SELECT NEWID() As Idk, COUNT(Id) AS Registros, COUNT(IdAuditor) AS Asignados FROM [AUDITCAC_Dev2].[dbo].[RegistrosAuditoria] where IdMedicion = '''+@IdMedicion+''''


EXEC(@Query);

--PRINT(' --- ');
--PRINT(@Total2);

END --END Procedure
GO
/****** Object:  StoredProcedure [dbo].[GetSoportesEntidad]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
CREATE PROCEDURE [dbo].[GetSoportesEntidad]
(@IdRegistrosAuditoria VARCHAR(100))
AS
BEGIN
SET NOCOUNT ON

--Guardamos Query inicial.
DECLARE @Query VARCHAR(MAX);
SET @Query = 'SELECT Id, IdRegistrosAuditoria, IdSoporte, IdEntidad, NombreSoporte, UrlSoporte, CreatedBy, CreatedDate, ModifyBy, ModifyDate, Status 
FROM RegistroAuditoriaSoporte RA ' 

--Calculo de paginado.
--DECLARE @Paginate INT = @PageNumber * @MaxRows;

--Guardamos Condiciones.
DECLARE @Where VARCHAR(MAX) = '';
IF(@IdRegistrosAuditoria <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'IdRegistrosAuditoria = ' + CAST(@IdRegistrosAuditoria AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND IdRegistrosAuditoria = ' + CAST(@IdRegistrosAuditoria AS NVARCHAR(MAX))
	END 
END 
--

--Paginado
--DECLARE @Paginado VARCHAR(MAX) = '
--ORDER BY idCobertura
--OFFSET ' + CAST(@Paginate AS NVARCHAR(MAX)) + ' ROWS
--FETCH NEXT ' + CAST(@MaxRows AS NVARCHAR(MAX)) + ' ROWS ONLY;'
 
--Concatenamos Query, Condiciones y Paginado. Luego ejecutamos.
IF(@Where <> '' )
BEGIN 
	SET @Where = ' WHERE ' + @Where;              
END  
DECLARE @Total VARCHAR(MAX);
SET @Total = @Query + '' + @Where --+ ' ' + @Paginado
EXEC(@Total);

END
GO
/****** Object:  StoredProcedure [dbo].[getUserCode]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[getUserCode]
AS 
BEGIN 

	select  
		cast(count(*)+1 as varchar(50)) as Respuesta
	from AspNetUsers;

END
GO
/****** Object:  StoredProcedure [dbo].[GetUsuariosByRoleCoberturaId]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUsuariosByRoleCoberturaId]
(@CoberturaId INT)
AS
BEGIN
SET NOCOUNT ON

--Obtenemos registros.
SELECT US.Id, US.UserName, US.Email FROM UsuarioXEnfermedad E 
INNER JOIN AspNetUsers US ON US.Id = E.IdUsuario
WHERE E.IdCobertura = @CoberturaId


END
GO
/****** Object:  StoredProcedure [dbo].[GetUsuariosByRoleId]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUsuariosByRoleId]
--(@IdEstadoOriginal INT, @IdEstadoNuevo INT)
(@RoleId INT, @UserId VARCHAR(450))
AS
BEGIN
SET NOCOUNT ON

--Obtenemos registros.
SELECT U.Id, U.UserName, U.Email FROM AspNetUsers U
INNER JOIN AspNetUserRoles UR ON (U.Id = UR.UserId)
INNER JOIN AspNetRoles R ON (UR.RoleId = R.Id)
WHERE R.Id = @RoleId


END
GO
/****** Object:  StoredProcedure [dbo].[GetUsuarioXEnfermedad]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetUsuarioXEnfermedad]
(@PageNumber INT, 
@MaxRows INT, 
@IdLider VARCHAR(MAX))
AS
BEGIN
SET NOCOUNT ON

--Guardamos Query inicial.
DECLARE @Query VARCHAR(MAX);

SET @Query = 'SELECT * FROM [AUDITCAC_Dev2].[dbo].[UsuarioXEnfermedad] US where US.IdUsuario = '''+@IdLider+''''


EXEC(@Query);

--PRINT(' --- ');
--PRINT(@Total2);

END --END Procedure
GO
/****** Object:  StoredProcedure [dbo].[GetVariablesFiltrado]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
--DROP PROCEDURE GetVariables;
CREATE PROCEDURE [dbo].[GetVariablesFiltrado]
(@PageNumber INT, @MaxRows INT, @Id VARCHAR(MAX), @Variable VARCHAR(MAX), @Activa VARCHAR(MAX), @Orden VARCHAR(MAX), @idVariable VARCHAR(MAX), @idCobertura VARCHAR(MAX), @nombre VARCHAR(MAX), @nemonico VARCHAR(MAX), @descripcion VARCHAR(MAX), @idTipoVariable VARCHAR(MAX), @longitud VARCHAR(MAX), 
 @decimales VARCHAR(MAX), @formato VARCHAR(MAX), @tablaReferencial VARCHAR(MAX), @campoReferencial VARCHAR(MAX), @CreatedBy VARCHAR(MAX), @CreatedDate VARCHAR(MAX), @ModifyBy VARCHAR(MAX), @ModifyDate VARCHAR(MAX), @MotivoVariable VARCHAR(MAX), @Bot VARCHAR(MAX), @TipoVariableItem VARCHAR(MAX), @EstructuraVariable VARCHAR(MAX),
 @MedicionId VARCHAR(MAX), @EsGlosa VARCHAR(MAX), @EsVisible VARCHAR(MAX), @EsCalificable VARCHAR(MAX), @Activo VARCHAR(MAX), @EnableDC VARCHAR(MAX), @EnableNC VARCHAR(MAX), @EnableND VARCHAR(MAX), @CalificacionXDefecto VARCHAR(MAX), @SubGrupoId VARCHAR(MAX), @Encuesta VARCHAR(MAX), @VxM_Orden VARCHAR(MAX), @Alerta VARCHAR(MAX), @AlertaDescripcion VARCHAR(MAX), @calificacionIPSItem VARCHAR(MAX), @IdRegla VARCHAR(MAX), @Concepto VARCHAR(MAX))
AS
BEGIN
SET NOCOUNT ON

--Guardamos Query inicial.
DECLARE @Query VARCHAR(MAX);
SET @Query = 'SELECT  '''' As QueryNoRegistrosTotales, NEWID() As Idk, CAST(VA.Id As NVARCHAR(MAX)) As Id, CAST(VA.Id AS NVARCHAR(12)) AS Variable, CAST(VA.Activa As NVARCHAR(MAX)) As Activa, CAST(VA.Orden As NVARCHAR(MAX)) As Orden, CAST(VA.idVariable As NVARCHAR(MAX)) As idVariable, 
CAST(VA.idCobertura As NVARCHAR(MAX)) As idCobertura, CAST(VA.nombre As NVARCHAR(MAX)) As nombre, 
CAST(VA.nemonico As NVARCHAR(MAX)) As nemonico, CAST(VA.descripcion As NVARCHAR(MAX)) As descripcion, CAST(VA.idTipoVariable As NVARCHAR(MAX)) As idTipoVariable, CAST(VA.longitud As NVARCHAR(MAX)) As longitud, CAST(VA.decimales As NVARCHAR(MAX)) As decimales, 
CAST(VA.formato As NVARCHAR(MAX)) As formato, CAST(VA.tablaReferencial As NVARCHAR(MAX)) As tablaReferencial, CAST(VA.campoReferencial As NVARCHAR(MAX)) As campoReferencial, CAST(VA.CreatedBy As NVARCHAR(MAX)) As CreatedBy, CAST(VA.CreatedDate As NVARCHAR(MAX)) As CreatedDate, 
CAST(VA.ModifyBy As NVARCHAR(MAX)) As ModifyBy, CAST(VA.ModifyDate As NVARCHAR(MAX)) As ModifyDate, CAST(VA.MotivoVariable As NVARCHAR(MAX)) As MotivoVariable, CAST(VA.Bot As NVARCHAR(MAX)) As Bot, CAST(VA.TipoVariableItem As NVARCHAR(MAX)) As TipoVariableItem, CAST(VA.EstructuraVariable As NVARCHAR(MAX)) As EstructuraVariable,  
CAST(VAXM.VariableId As NVARCHAR(MAX)) As VariableId, CAST(VAXM.MedicionId As NVARCHAR(MAX)) As MedicionId, CAST(VAXM.EsGlosa As NVARCHAR(MAX)) As EsGlosa, CAST(VAXM.EsVisible As NVARCHAR(MAX)) As EsVisible, CAST(VAXM.EsCalificable As NVARCHAR(MAX)) As EsCalificable, 
CAST(VAXM.Activo As NVARCHAR(MAX)) As Activo, CAST(VAXM.EnableDC As NVARCHAR(MAX)) As EnableDC, CAST(VAXM.EnableNC As NVARCHAR(MAX)) As EnableNC, CAST(VAXM.EnableND As NVARCHAR(MAX)) As EnableND, CAST(VAXM.CalificacionXDefecto As NVARCHAR(MAX)) As CalificacionXDefecto, VAXM.SubGrupoId As SubGrupoId, 
CAST(IT.ItemName As NVARCHAR(MAX)) As SubGrupoNombre, CAST(VAXM.Encuesta As NVARCHAR(MAX)) As Encuesta, CAST(VAXM.Orden As NVARCHAR(MAX)) As VxM_Orden, 
CAST(VA.Alerta As NVARCHAR(MAX)) As Alerta, CAST(VA.AlertaDescripcion As NVARCHAR(MAX)) As AlertaDescripcion, CAST(VAI.ItemId As NVARCHAR(MAX)) As calificacionIPSItem, CAST(IT2.ItemName As NVARCHAR(MAX)) As calificacionIPSItemNombre, CAST(RVA.IdRegla As NVARCHAR(MAX)) As IdRegla, CAST(IT3.ItemName As NVARCHAR(MAX)) As NombreRegla, CAST(RVA.Concepto As NVARCHAR(MAX)) As Concepto, ITTipo.ItemName as TipoVariableDesc
FROM Variables VA 
INNER JOIN VariableXMedicion VAXM ON (VA.Id = VAXM.VariableId) 
INNER JOIN Item IT ON (VAXM.SubGrupoId = IT.Id) 
INNER JOIN Item ITTipo ON (VA.TipoVariableItem = ITTipo.Id) 
LEFT JOIN VariablesXItems VAI ON (VA.Id = VAI.VariablesId) 
LEFT JOIN ReglasVariable RVA ON (VA.Id = RVA.IdVariable) 
LEFT JOIN Item IT2 ON (VAI.ItemId = IT2.Id) 
LEFT JOIN Item IT3 ON (RVA.IdRegla = IT3.Id) '

--@Alerta, @AlertaDescripcion, @calificacionIPSItem, @IdRegla, @Concepto

--Calculo de paginado.
DECLARE @Paginate INT = @PageNumber * @MaxRows;

--Guardamos Condiciones.
DECLARE @Where VARCHAR(MAX) = '';

IF(@Id <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.Id IN (' + CAST(@Id AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VA.Id IN (' + CAST(@Id AS NVARCHAR(MAX)) + ')'
	END 
END 
--
IF(@Variable <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + '''VAR'' + CAST(VA.Id AS NVARCHAR(MAX)) LIKE ''%' + CAST(@Variable AS NVARCHAR(MAX)) + '%'''
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' OR ' + '''VAR'' + CAST(VA.Id AS NVARCHAR(MAX)) LIKE ''%' + CAST(@Variable AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@Activa = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.Activa = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VA.Activa = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@Activa = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.Activa = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VA.Activa = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@Orden <> '' AND ISNUMERIC(@Orden) <> 0)
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.Orden IN (' + CAST(@Orden AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' OR VA.Orden IN (' + CAST(@Orden AS NVARCHAR(MAX)) + ')'
	END 
END 
--
IF(@idVariable <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.idVariable IN (' + CAST(@idVariable AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VA.idVariable IN (' + CAST(@idVariable AS NVARCHAR(MAX)) + ')'
	END 
END 
--
IF(@idCobertura <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.idCobertura IN (' + CAST(@idCobertura AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VA.idCobertura IN (' + CAST(@idCobertura AS NVARCHAR(MAX)) + ')'
	END 
END 
--
IF(@nombre <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.nombre LIKE ''%' + CAST(@nombre AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' OR VA.nombre LIKE ''%' + CAST(@nombre AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@nemonico <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.nemonico LIKE ''%' + CAST(@nemonico AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VA.nemonico LIKE ''%' + CAST(@nemonico AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@descripcion <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.descripcion LIKE ''%' + CAST(@descripcion AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' OR VA.descripcion LIKE ''%' + CAST(@descripcion AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@idTipoVariable <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.idTipoVariable LIKE ''%' + CAST(@idTipoVariable AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VA.idTipoVariable LIKE ''%' + CAST(@idTipoVariable AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@longitud <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.longitud IN (' + CAST(@longitud AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VA.longitud IN (' + CAST(@longitud AS NVARCHAR(MAX)) + ')'
	END 
END 
--
IF(@decimales <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.decimales IN (' + CAST(@decimales AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VA.decimales IN (' + CAST(@decimales AS NVARCHAR(MAX)) + ')'
	END 
END 
--
IF(@formato <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.formato LIKE ''%' + CAST(@formato AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VA.formato LIKE ''%' + CAST(@formato AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@tablaReferencial <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.tablaReferencial LIKE ''%' + CAST(@tablaReferencial AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VA.tablaReferencial LIKE ''%' + CAST(@tablaReferencial AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@campoReferencial <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.campoReferencial LIKE ''%' + CAST(@campoReferencial AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VA.campoReferencial LIKE ''%' + CAST(@campoReferencial AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@CreatedBy <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.CreatedBy IN (' + CAST(@CreatedBy AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VA.CreatedBy IN (' + CAST(@CreatedBy AS NVARCHAR(MAX)) + ')'
	END 
END 
--
IF(@CreatedDate <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.CreatedDate LIKE ''%' + CAST(@CreatedDate AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VA.CreatedDate LIKE ''%' + CAST(@CreatedDate AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@ModifyBy <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.ModifyBy IN (' + CAST(@ModifyBy AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VA.ModifyBy IN (' + CAST(@ModifyBy AS NVARCHAR(MAX)) + ')'
	END 
END 
--
IF(@ModifyDate <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.ModifyDate LIKE ''%' + CAST(@ModifyDate AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VA.ModifyDate LIKE ''%' + CAST(@ModifyDate AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@MotivoVariable <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.MotivoVariable LIKE ''%' + CAST(@MotivoVariable AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VA.MotivoVariable LIKE ''%' + CAST(@MotivoVariable AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@Bot = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.Bot = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VA.Bot = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@Bot = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.Bot = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VA.Bot = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@TipoVariableItem <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.TipoVariableItem IN (' + CAST(@TipoVariableItem AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VA.TipoVariableItem IN (' + CAST(@TipoVariableItem AS NVARCHAR(MAX)) + ')'
	END 
END 
--
IF(@EstructuraVariable <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.EstructuraVariable IN (' + CAST(@EstructuraVariable AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VA.EstructuraVariable IN (' + CAST(@EstructuraVariable AS NVARCHAR(MAX)) + ')'
	END 
END
--
IF(@Id <> '') -- @VariableId
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VAXM.VariableId IN (' + CAST(@Id AS NVARCHAR(MAX)) + ')'      -- @VariableId
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VAXM.VariableId IN (' + CAST(@Id AS NVARCHAR(MAX)) + ')' -- @VariableId
	END 
END 
--
IF(@MedicionId <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VAXM.MedicionId IN (' + CAST(@MedicionId AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VAXM.MedicionId IN (' + CAST(@MedicionId AS NVARCHAR(MAX)) + ')'
	END 
END 
--
IF(@EsGlosa = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VAXM.EsGlosa = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VAXM.EsGlosa = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@EsGlosa = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VAXM.EsGlosa = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VAXM.EsGlosa = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@EsVisible = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VAXM.EsVisible = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VAXM.EsVisible = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@EsVisible = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VAXM.EsVisible = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VAXM.EsVisible = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@EsCalificable = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VAXM.EsCalificable = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VAXM.EsCalificable = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@EsCalificable = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VAXM.EsCalificable = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VAXM.EsCalificable = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@Activo = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VAXM.Activo = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VAXM.Activo = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@Activo = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VAXM.Activo = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VAXM.Activo = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@EnableDC = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VAXM.EnableDC = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VAXM.EnableDC = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@EnableDC = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VAXM.EnableDC = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VAXM.EnableDC = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@EnableNC = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VAXM.EnableNC = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VAXM.EnableNC = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@EnableNC = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VAXM.EnableNC = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VAXM.EnableNC = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@EnableND = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VAXM.EnableND = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VAXM.EnableND = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@EnableND = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VAXM.EnableND = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VAXM.EnableND = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@CalificacionXDefecto <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VAXM.CalificacionXDefecto IN (' + CAST(@CalificacionXDefecto AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VAXM.CalificacionXDefecto IN (' + CAST(@CalificacionXDefecto AS NVARCHAR(MAX)) + ')'
	END 
END 
--
IF(@SubGrupoId <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VAXM.SubGrupoId IN (' + CAST(@SubGrupoId AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VAXM.SubGrupoId IN (' + CAST(@SubGrupoId AS NVARCHAR(MAX)) + ')'
	END 
END 
--
IF(@Encuesta = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VAXM.Encuesta = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VAXM.Encuesta = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@Encuesta = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VAXM.Encuesta = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VAXM.Encuesta = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@Orden <> '' AND ISNUMERIC(@Orden) <> 0)
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VAXM.Orden IN (' + CAST(@Orden AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' OR VAXM.Orden IN (' + CAST(@Orden AS NVARCHAR(MAX)) + ')'
	END 
END
--
IF(@Alerta = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.Alerta = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VA.Alerta = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@Alerta = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.Alerta = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VA.Alerta = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@AlertaDescripcion <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VA.AlertaDescripcion LIKE ''%' + CAST(@AlertaDescripcion AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' OR VA.AlertaDescripcion LIKE ''%' + CAST(@AlertaDescripcion AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@calificacionIPSItem <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'VAI.ItemId IN (' + CAST(@calificacionIPSItem AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND VAI.ItemId IN (' + CAST(@calificacionIPSItem AS NVARCHAR(MAX)) + ')'
	END 
END 
--
IF(@IdRegla <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RVA.IdRegla IN (' + CAST(@IdRegla AS NVARCHAR(MAX)) + ')'
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RVA.IdRegla IN (' + CAST(@IdRegla AS NVARCHAR(MAX)) + ')'
	END 
END 
--
IF(@Concepto <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RVA.Concepto LIKE ''%' + CAST(@Concepto AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RVA.Concepto LIKE ''%' + CAST(@Concepto AS NVARCHAR(MAX)) + '%''' 
	END 
END 
---


--Validamos estado activo.
IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'VA.Status = 1 AND VAXM.Status = 1 AND IT.Status = 1 ' 
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND VA.Status = 1 AND VAXM.Status = 1 AND IT.Status = 1 '
END 
--  

--Paginado
DECLARE @Paginado VARCHAR(MAX) = '
ORDER BY VAXM.Orden ASC 
OFFSET ' + CAST(@Paginate AS NVARCHAR(MAX)) + ' ROWS
FETCH NEXT ' + CAST(@MaxRows AS NVARCHAR(MAX)) + ' ROWS ONLY;'

--Concatenamos Query y Condiciones.
IF(@Where <> '' )
BEGIN 
	SET @Where = ' WHERE ' + @Where;              
END  
DECLARE @Total VARCHAR(MAX);
SET @Total = @Query + '' + @Where + ' ' + @Paginado

PRINT @Total
--Para calcular total registros filtrados.
DECLARE @Query2 NVARCHAR(MAX);
SET @Query2 = N'SELECT NEWID() As Idk, COUNT(*) As NoRegistrosTotalesFiltrado 
FROM Variables VA 
INNER JOIN VariableSubgrupo VASG ON (VA.SubGrupoId = VASG.Id) 
WHERE VA.Status = 1 AND VAXM.Status = 1 AND IT.Status = 1 '
--INNER JOIN RegistrosAuditoriaDetalle RAD ON (VA.Id = RAD.VariableId)

DECLARE @Total2 NVARCHAR(MAX);
SET @Total2 = @Query2 + '' + REPLACE(@Where, '''', '''''')

--str, old_str. new_str
SET @Total = REPLACE(@Total, ''' '' As QueryNoRegistrosTotales', '''' + CAST(@Total2 AS NVARCHAR(MAX)) + ''' As QueryNoRegistrosTotales');

--Imprimimos/Ejecutamos.
EXEC(@Total);
--PRINT(@Total);

END

GO
/****** Object:  StoredProcedure [dbo].[Inactive_User]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[Inactive_User] 
	@correo Varchar(150)

AS 
BEGIN 

	update [dbo].[AspNetUsers] set Active = IIF(Active = 1, 0, 1) where Email = @correo;

	select 'Usuario Actualizado' as Respuesta;

END
GO
/****** Object:  StoredProcedure [dbo].[InsertarPascientes]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
CREATE PROCEDURE [dbo].[InsertarPascientes] (
@Pascientes DT_Pascientes READONLY
)
AS
BEGIN
SET NOCOUNT ON	
   INSERT INTO RegistrosAuditoria(IdRadicado,IdMedicion,IdPeriodo,PrimerNombre,SegundoNombre,PrimerApellido,SegundoApellido,TipoIdentificacion,Identificacion,FechaNacimiento,FechaCreacion,FechaAuditoria,Activo,Conclusion,UrlSoportes,Reverse,DisplayOrder,Ara,Eps,FechaReverso,AraAtendido,EpsAtendido,Revisar,Estado,LevantarGlosa,MantenerCalificacion,ComiteExperto,ComiteAdministrativo,AccionLider,AccionAuditor,CreatedBy,CreatedDate,ModifyBy,ModifyDate) --OUTPUT INSERTED.id
   SELECT IdRadicado,IdMedicion,IdPeriodo,PrimerNombre,SegundoNombre,PrimerApellido,SegundoApellido,TipoIdentificacion,Identificacion,FechaNacimiento,FechaCreacion,FechaAuditoria,Activo,Conclusion,UrlSoportes,Reverse,DisplayOrder,Ara,Eps,FechaReverso,AraAtendido,EpsAtendido,Revisar,Estado, 0 As LevantarGlosa, 0 As MantenerCalificacion, 0 As ComiteExperto, 0 As ComiteAdministrativo, 0 As AccionLider, 0 As AccionAuditor,CreatedBy,CreatedDate,ModifyBy,ModifyDate FROM @Pascientes;
END
GO
/****** Object:  StoredProcedure [dbo].[MoverRegistrosAuditoria]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Creacion/Edicion Procedure
CREATE PROCEDURE [dbo].[MoverRegistrosAuditoria]
(@Id VARCHAR(MAX), @IdMedicion INT)
AS
BEGIN
SET NOCOUNT ON

--Creamos Update
UPDATE RegistrosAuditoria SET IdMedicion = CAST(@IdMedicion AS NVARCHAR(MAX)) WHERE Id IN (SELECT value FROM STRING_SPLIT(@Id,','));

--Regresamos datos confirmación.
--SELECT '1' As Codigo, 'Ok' As Mensaje;

END
GO
/****** Object:  StoredProcedure [dbo].[RegistroAuditoriaLogSP]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
CREATE PROCEDURE [dbo].[RegistroAuditoriaLogSP]
(@PageNumber INT, @MaxRows INT, @IdAuditor varchar(255))
AS  
BEGIN  
SET NOCOUNT ON  

--Calculo de paginado.  
--SET @PageNumber = @PageNumber - 1;  
DECLARE @Paginate INT = @PageNumber * @MaxRows; 

--Ejecutamos consulta.
SELECT 'SELECT NEWID() As Idk, COUNT(*) As NoRegistrosTotalesFiltrado 
		FROM [RegistroAuditoriaLog] RA 
		JOIN [AspNetUsers] AU on RA.AsignadoA = AU.Id ' As QueryNoRegistrosTotales 
		,rsa.IdRadicado AS Id
		,RA.[RegistroAuditoriaId]
		,RA.[Proceso]
		,RA.[Observacion]
		,RA.[EstadoAnterioId]
		,RA.[EstadoActual]
		,AU.[UserName] as AsignadoA
		,(Select top 1 AU.UserName FROM AspNetUsers AU inner join [RegistroAuditoriaLog] RA on AU.Id = RA.AsingadoPor) as AsignadoPor
		,(Select top 1 AU.UserName FROM AspNetUsers AU inner join [RegistroAuditoriaLog] RA on AU.Id = RA.CreatedBy) as CreatedBy
		,RA.[CreatedDate]
		,RA.[ModifyBy]
		,RA.[ModificationDate],
		RA.Status
FROM [RegistroAuditoriaLog] RA
JOIN [AspNetUsers] AU on RA.AsignadoA = AU.Id
INNER JOIN RegistrosAuditoria rsa ON RA.RegistroAuditoriaId = rsa.Id
-- WHERE RA.AsingadoPor = @IdAuditor 

--Paginado
ORDER BY RA.[Id]
OFFSET @Paginate ROWS  
FETCH NEXT @MaxRows ROWS ONLY;

END -- END SP.
GO
/****** Object:  StoredProcedure [dbo].[RegistrosAuditoriaAsignadoAuditorEstado]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
CREATE PROCEDURE [dbo].[RegistrosAuditoriaAsignadoAuditorEstado]
(@PageNumber INT, @MaxRows INT, @IdAuditor VARCHAR(150), @Estado VARCHAR(150))
AS
BEGIN
SET NOCOUNT ON

DECLARE @Query VARCHAR(MAX);
--SET @Query = 'SELECT Id, IdRadicado, IdMedicion, IdPeriodo, IdAuditor, PrimerNombre, SegundoNombre, PrimerApellido, SegundoApellido, Sexo, TipoIdentificacion, Identificacion, FechaNacimiento, FechaCreacion, FechaAuditoria, FechaMinConsultaAudit, FechaMaxConsultaAudit, Activo, Conclusion, UrlSoportes, Reverse, DisplayOrder, Ara, Eps, FechaReverso, AraAtendido, EpsAtendido, Revisar, Estado, CreatedBy, CreatedDate, ModifyBy, ModifyDate  
--FROM RegistrosAuditoria '
SET @Query = 'SELECT RA.Id, RA.IdRadicado, RA.IdMedicion, ME.Descripcion As NombreMedicion, RA.IdPeriodo, RA.IdAuditor, 1 As IdEntidad, ''EntidadFakeName'' As Entidad, RA.PrimerNombre, RA.SegundoNombre, RA.PrimerApellido, RA.SegundoApellido, RA.Sexo, RA.TipoIdentificacion, RA.Identificacion, RA.FechaNacimiento, RA.FechaCreacion, RA.FechaAuditoria, RA.FechaMinConsultaAudit, RA.FechaMaxConsultaAudit, RA.FechaAsignacion, RA.Activo, RA.Conclusion, RA.UrlSoportes, RA.Reverse, RA.DisplayOrder, RA.Ara, RA.Eps, RA.FechaReverso, RA.AraAtendido, RA.EpsAtendido, RA.Revisar, RA.Extemporaneo, RA.Estado, ERA.Codigo As EstadoCodigo, ERA.Nombre As EstadoNombre, RA.CreatedBy, RA.CreatedDate, RA.ModifyBy, RA.ModifyDate 
FROM RegistrosAuditoria RA 
INNER JOIN Medicion ME ON (RA.IdMedicion = ME.Id) 
INNER JOIN EstadosRegistroAuditoria ERA ON (RA.Estado = ERA.Id) '

--Calculo de paginado.
DECLARE @Paginate INT = @PageNumber * @MaxRows;

--Guardamos Condiciones.
DECLARE @Where VARCHAR(MAX) = '';
IF(@IdAuditor <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'IdAuditor LIKE ''%' + CAST(@IdAuditor AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND IdAuditor LIKE ''%' + CAST(@IdAuditor AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@Estado <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'Estado LIKE ''%' + CAST(@Estado AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND Estado LIKE ''%' + CAST(@Estado AS NVARCHAR(MAX)) + '%'''
	END 
END 
--

--Paginado
DECLARE @Paginado VARCHAR(MAX) = '
ORDER BY RA.DisplayOrder
OFFSET ' + CAST(@Paginate AS NVARCHAR(MAX)) + ' ROWS
FETCH NEXT ' + CAST(@MaxRows AS NVARCHAR(MAX)) + ' ROWS ONLY;'
 
--Concatenamos Query y Condiciones.
IF(@Where <> '' )
BEGIN 
	SET @Where = ' WHERE ' + @Where;              
END  
DECLARE @Total VARCHAR(MAX);
SET @Total = @Query + '' + @Where + ' ' + @Paginado
EXEC(@Total);

END
GO
/****** Object:  StoredProcedure [dbo].[RegistrosAuditoriaDetallesAsignacion]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure  
CREATE PROCEDURE [dbo].[RegistrosAuditoriaDetallesAsignacion]  
(@IdAuditor VARCHAR(150), @UserRolId INT)  
AS  
BEGIN  
SET NOCOUNT ON  
   DECLARE @Query NVARCHAR(MAX) = '' 
   DECLARE @EstadosAuditado NVARCHAR(255) = '2,3,4,5,6,7,8,9,10,11,12,13,14,15,16' 
   DECLARE @EstadosGlosado NVARCHAR(255) = '2,3,4,5' -- 2,3,4,5 | 3,4,5  
   DECLARE @EstadosError NVARCHAR(255) = '6,7'   
   DECLARE @EstadosCerrado NVARCHAR(255) = '16'  
   DECLARE @EstadosPendiente NVARCHAR(255) = '17' 
   DECLARE @EstadosHallazgo NVARCHAR(255) = '12,13,14,15'  
   DECLARE @EstadosEntidad NVARCHAR(255) = '2,4,8,13'  
   DECLARE @EstadosComiteExperto NVARCHAR(255) = '10'  
   DECLARE @EstadosAdministrativo NVARCHAR(255) = '9'  
 --Para convertir 'all' a vacio.  
 IF @IdAuditor = 'all'  
 BEGIN  
 SET @IdAuditor = ''
 -- Asignados  
 SET @Query = 'SELECT 1 AS [Order], ''Asignados'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 0 AS Extemporaneo, ' + ''''+  + '''' + 'AS Estados FROM  RegistrosAuditoria WHERE  FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX))'  
  
 --Auditados  
 
 SET @Query = @Query + ' UNION SELECT 2 AS [Order], ''Auditados'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 0 AS Extemporaneo, ' + ''''+ @EstadosAuditado + '''' + 'AS Estados FROM  RegistrosAuditoria WHERE Estado IN (' +  @EstadosAuditado 
+ ') ' +  ' AND FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX)) '  
  
 -- Glosados  
 
 SET @Query = @Query + ' UNION SELECT 3 AS [Order], ''Glosados'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 0 AS Extemporaneo, ' + ''''+ @EstadosGlosado + '''' + 'AS Estados FROM  RegistrosAuditoria WHERE Estado IN (' +  @EstadosGlosado + '
) AND FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX)) '  
  
 -- Error  
 
 SET @Query = @Query + ' UNION SELECT 4 AS [Order], ''Error'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 0 AS Extemporaneo, ' + ''''+ @EstadosError + '''' + 'AS Estados FROM  RegistrosAuditoria WHERE Estado IN (' +  @EstadosError + ') AND FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX)) '  
  
  
 IF @UserRolId = 2 -- Lider  
 BEGIN  
  -- Extemporaneo  
  SET @Query = @Query + 'UNION SELECT 5 AS [Order], ''Extemporaneo'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 1 AS Extemporaneo, ' + ''''+  + '''' + 'AS Estados FROM  RegistrosAuditoria WHERE Extemporaneo = 1 AND FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX)) '  
  
 END  
   
 -- Cerrado  

 SET @Query = @Query + ' UNION SELECT 6 AS [Order], ''Cerrados'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 0 AS Extemporaneo, ' + ''''+ @EstadosCerrado + '''' + 'AS Estados FROM  RegistrosAuditoria WHERE Estado IN (' +  @EstadosCerrado + ') AND FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX)) '  
  
 -- Pendientes  

 SET @Query = @Query + ' UNION SELECT 7 AS [Order], ''Pendientes'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 0 AS Extemporaneo, ' + ''''+ @EstadosPendiente + '''' + 'AS Estados FROM  RegistrosAuditoria WHERE Estado IN (' +  @EstadosPendiente + ') AND FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX)) '  
  
 -- Hallazgo  
 
 SET @Query = @Query + ' UNION SELECT 8 AS [Order], ''Hallazgo'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 0 AS Extemporaneo, ' + ''''+ @EstadosHallazgo + '''' + 'AS Estados FROM  RegistrosAuditoria WHERE Estado IN (' +  @EstadosHallazgo +')  AND FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX)) '  
  
 -- Comite Experto  
 -- DECLARE @EstadosComiteExperto NVARCHAR(255) = '10'  
 -- SET @Query = @Query + ' UNION SELECT 9 AS [Order], ''Comité Experto'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 0 AS Extemporaneo, ' + ''''+ @EstadosComiteExperto + '''' + 'AS Estados FROM  RegistrosAuditoria WHERE Estado IN (' +  @EstadosComiteExperto + ') AND' +  ' IdAuditor IN (' + '''' + CAST(REPLACE(@IdAuditor, ',', '''' + ',' +''''  )  AS NVARCHAR(255)) + '''' + ') AND FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX)) '  
  
 -- Comite Administrativo  
 -- DECLARE @EstadosAdministrativo NVARCHAR(255) = '9'  
 -- SET @Query = @Query + ' UNION SELECT 10 AS [Order], ''Comité Administrativo'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 0 AS Extemporaneo, ' + ''''+ @EstadosComiteExperto + '''' + 'AS Estados FROM  RegistrosAuditoria WHERE Estado IN ('  @EstadosComiteExperto + ') AND' +  ' IdAuditor IN (' + '''' + CAST(REPLACE(@IdAuditor, ',', '''' + ',' +''''  )  AS NVARCHAR(255)) + '''' + ') AND FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX)) '  
  
 -- Comite Experto y Administrativo  

 SET @Query = @Query + ' UNION SELECT 9 AS [Order], ''Comité Experto y Administrativo'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 0 AS Extemporaneo, ' + ''''+ @EstadosAdministrativo + ', ' + @EstadosComiteExperto + '''' + 'AS Estados FROM RegistrosAuditoria WHERE Estado IN (' +  @EstadosComiteExperto + ', ' + @EstadosAdministrativo + ')  AND FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX)) '  
  
 -- Entidad  

 SET @Query = @Query + ' UNION SELECT 10 AS [Order], ''Entidad'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 0 AS Extemporaneo, ' + ''''+ @EstadosEntidad + '''' + 'AS Estados FROM  RegistrosAuditoria WHERE Estado IN (' +  @EstadosEntidad + ') AND  FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX)) '  
 END  
  
ELSE 
BEGIN
 -- Asignados  
 SET @Query = 'SELECT 1 AS [Order], ''Asignados'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 0 AS Extemporaneo, ' + ''''+  + '''' + 'AS Estados FROM  RegistrosAuditoria WHERE IdAuditor IN (' + '''' + CAST(REPLACE(@IdAuditor, ',', '''' + ','
 +''''  )  AS NVARCHAR(255)) + '''' + ') AND FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX))'  
  
 --Auditados  

 SET @Query = @Query + ' UNION SELECT 2 AS [Order], ''Auditados'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 0 AS Extemporaneo, ' + ''''+ @EstadosAuditado + '''' + 'AS Estados FROM  RegistrosAuditoria WHERE Estado IN (' +  @EstadosAuditado 
+ ') AND' +  ' IdAuditor IN (' + '''' + CAST(REPLACE(@IdAuditor, ',', '''' + ',' +''''  )  AS NVARCHAR(255)) + '''' + ') AND FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX)) '  
  
 -- Glosados  
 
 SET @Query = @Query + ' UNION SELECT 3 AS [Order], ''Glosados'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 0 AS Extemporaneo, ' + ''''+ @EstadosGlosado + '''' + 'AS Estados FROM  RegistrosAuditoria WHERE Estado IN (' +  @EstadosGlosado + '
) AND' +  ' IdAuditor IN (' + '''' + CAST(REPLACE(@IdAuditor, ',', '''' + ',' +''''  )  AS NVARCHAR(255)) + '''' + ') AND FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX)) '  
  
 -- Error  

 SET @Query = @Query + ' UNION SELECT 4 AS [Order], ''Error'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 0 AS Extemporaneo, ' + ''''+ @EstadosError + '''' + 'AS Estados FROM  RegistrosAuditoria WHERE Estado IN (' +  @EstadosError + ') AND' 
+  ' IdAuditor IN (' + '''' + CAST(REPLACE(@IdAuditor, ',', '''' + ',' +''''  )  AS NVARCHAR(255)) + '''' + ') AND FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX)) '  
  
  
 IF @UserRolId = 2 -- Lider  
 BEGIN  
  -- Extemporaneo  
  SET @Query = @Query + 'UNION SELECT 5 AS [Order], ''Extemporaneo'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 1 AS Extemporaneo, ' + ''''+  + '''' + 'AS Estados FROM  RegistrosAuditoria WHERE Extemporaneo = 1 AND IdAuditor IN (' + '''' + 
CAST(REPLACE(@IdAuditor, ',', '''' + ',' +''''  )  AS NVARCHAR(255)) + '''' + ') AND FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX)) '  
  
 END  
   
 -- Cerrado  

 SET @Query = @Query + ' UNION SELECT 6 AS [Order], ''Cerrados'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 0 AS Extemporaneo, ' + ''''+ @EstadosCerrado + '''' + 'AS Estados FROM  RegistrosAuditoria WHERE Estado IN (' +  @EstadosCerrado + '
) AND' +  ' IdAuditor IN (' + '''' + CAST(REPLACE(@IdAuditor, ',', '''' + ',' +''''  )  AS NVARCHAR(255)) + '''' + ') AND FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX)) '  
  
 -- Pendientes  

 SET @Query = @Query + ' UNION SELECT 7 AS [Order], ''Pendientes'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 0 AS Extemporaneo, ' + ''''+ @EstadosPendiente + '''' + 'AS Estados FROM  RegistrosAuditoria WHERE Estado IN (' +  @EstadosPendiente + ') AND' +  ' IdAuditor IN (' + '''' + CAST(REPLACE(@IdAuditor, ',', '''' + ',' +''''  )  AS NVARCHAR(255)) + '''' + ') AND FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX)) '  
  
 -- Hallazgo  

 SET @Query = @Query + ' UNION SELECT 8 AS [Order], ''Hallazgo'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 0 AS Extemporaneo, ' + ''''+ @EstadosHallazgo + '''' + 'AS Estados FROM  RegistrosAuditoria WHERE Estado IN (' +  @EstadosHallazgo +
 ') AND' +  ' IdAuditor IN (' + '''' + CAST(REPLACE(@IdAuditor, ',', '''' + ',' +''''  )  AS NVARCHAR(255)) + '''' + ') AND FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX)) '  
  
 -- Comite Experto  
 -- DECLARE @EstadosComiteExperto NVARCHAR(255) = '10'  
 -- SET @Query = @Query + ' UNION SELECT 9 AS [Order], ''Comité Experto'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 0 AS Extemporaneo, ' + ''''+ @EstadosComiteExperto + '''' + 'AS Estados FROM  RegistrosAuditoria WHERE Estado IN (' +  @EstadosComiteExperto + ') AND' +  ' IdAuditor IN (' + '''' + CAST(REPLACE(@IdAuditor, ',', '''' + ',' +''''  )  AS NVARCHAR(255)) + '''' + ') AND FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX)) '  
  
 -- Comite Administrativo  
 -- DECLARE @EstadosAdministrativo NVARCHAR(255) = '9'  
 -- SET @Query = @Query + ' UNION SELECT 10 AS [Order], ''Comité Administrativo'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 0 AS Extemporaneo, ' + ''''+ @EstadosComiteExperto + '''' + 'AS Estados FROM  RegistrosAuditoria WHERE Estado IN ('  @EstadosComiteExperto + ') AND' +  ' IdAuditor IN (' + '''' + CAST(REPLACE(@IdAuditor, ',', '''' + ',' +''''  )  AS NVARCHAR(255)) + '''' + ') AND FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX)) '  
  
 -- Comite Experto y Administrativo  

 SET @Query = @Query + ' UNION SELECT 9 AS [Order], ''Comité Experto y Administrativo'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 0 AS Extemporaneo, ' + ''''+ @EstadosAdministrativo + ', ' + @EstadosComiteExperto + '''' + 'AS Estados FROM 
 RegistrosAuditoria WHERE Estado IN (' +  @EstadosComiteExperto + ', ' + @EstadosAdministrativo + ') AND' +  ' IdAuditor IN (' + '''' + CAST(REPLACE(@IdAuditor, ',', '''' + ',' +''''  )  AS NVARCHAR(255)) + '''' + ') AND FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX)) '  
  
 -- Entidad  
 
 SET @Query = @Query + ' UNION SELECT 10 AS [Order], ''Entidad'' AS Nombre, CAST(COUNT(*) AS NVARCHAR(12)) AS NoRegistros, 0 AS Extemporaneo, ' + ''''+ @EstadosEntidad + '''' + 'AS Estados FROM  RegistrosAuditoria WHERE Estado IN (' +  @EstadosEntidad + '
) AND' +  ' IdAuditor IN (' + '''' + CAST(REPLACE(@IdAuditor, ',', '''' + ',' +''''  )  AS NVARCHAR(255)) + '''' + ') AND FechaAsignacion = CAST(GETDATE() AS NVARCHAR(MAX)) '  
END
  
 EXEC(@Query)  
 --PRINT(@Query)  
 END

--[RegistrosAuditoriaDetallesAsignacion]     'all', 2
GO
/****** Object:  StoredProcedure [dbo].[RegistrosAuditoriaFiltrado]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Creacion/Edicion Procedure
CREATE PROCEDURE [dbo].[RegistrosAuditoriaFiltrado]
(@PageNumber INT, @MaxRows INT, @IdRadicado INT, @IdMedicion INT, @IdPeriodo INT, @IdAuditor INT, @PrimerNombre VARCHAR(50), @SegundoNombre VARCHAR(50), @PrimerApellido VARCHAR(50), @SegundoApellido VARCHAR(50), @Sexo VARCHAR(50), @TipoIdentificacion VARCHAR(2), @Identificacion INT, @FechaNacimientoIni VARCHAR(255), @FechaNacimientoFin VARCHAR(255), @FechaCreacionIni VARCHAR(255), @FechaCreacionFin VARCHAR(255), @FechaAuditoriaIni VARCHAR(255), @FechaAuditoriaFin VARCHAR(255), @FechaMinConsultaAudit VARCHAR(255), @FechaMaxConsultaAudit VARCHAR(255), @Activo VARCHAR(50), @Conclusion VARCHAR(200), @UrlSoportes VARCHAR(200), @Reverse VARCHAR(50), @DisplayOrder INT, @Ara VARCHAR(50), @Eps VARCHAR(50), @FechaReversoIni VARCHAR(255), @FechaReversoFin VARCHAR(255), @AraAtendido VARCHAR(50), @EpsAtendido VARCHAR(50), @Revisar VARCHAR(50), @Estado INT)
AS
BEGIN
SET NOCOUNT ON

--Guardamos Query inicial.
DECLARE @Query VARCHAR(MAX);
--SET @Query = 'SELECT Id, IdRadicado, IdMedicion, IdPeriodo, IdAuditor, PrimerNombre, SegundoNombre, PrimerApellido, SegundoApellido, Sexo, TipoIdentificacion, Identificacion, FechaNacimiento, FechaCreacion, FechaAuditoria, FechaMinConsultaAudit, FechaMaxConsultaAudit, Activo, Conclusion, UrlSoportes, Reverse, DisplayOrder, Ara, Eps, FechaReverso, AraAtendido, EpsAtendido, Revisar, Estado, CreatedBy, CreatedDate, ModifyBy, ModifyDate  
SET @Query = 'SELECT RA.Id, RA.IdRadicado, RA.IdMedicion, ME.Descripcion As NombreMedicion, RA.IdPeriodo, RA.IdAuditor, RA.PrimerNombre, RA.SegundoNombre, RA.PrimerApellido, RA.SegundoApellido, RA.Sexo, RA.TipoIdentificacion, RA.Identificacion, RA.FechaNacimiento, RA.FechaCreacion, RA.FechaAuditoria, RA.FechaMinConsultaAudit, RA.FechaMaxConsultaAudit, RA.Activo, RA.Conclusion, RA.UrlSoportes, RA.Reverse, RA.DisplayOrder, RA.Ara, RA.Eps, RA.FechaReverso, RA.AraAtendido, RA.EpsAtendido, RA.Revisar, RA.Estado, RA.CreatedBy, RA.CreatedDate, RA.ModifyBy, RA.ModifyDate, RIC.ImagePath 
FROM RegistrosAuditoria RA 
INNER JOIN Medicion ME ON (RA.IdMedicion = ME.Id)
INNER JOIN AspNetUsers US ON (US.Id = RA.IdAuditor)
INNER JOIN AspNetUserRoles UR ON (UR.UserId = RA.IdAuditor)
INNER JOIN EstadoRegistroAuditoriaIcono RIC ON (RIC.Estado = RA.Estado AND RIC.RolId = UR.RoleId AND RA.Revisar = RIC.Revisar)'

--Calculo de paginado.
DECLARE @Paginate INT = @PageNumber * @MaxRows;

--Guardamos Condiciones.
DECLARE @Where VARCHAR(MAX) = '';
--IF(@Id <> 0)
--BEGIN 
--	IF(@Where = '')
--	BEGIN 
--		SET @Where = @Where + 'RA.Id = ' + CAST(@Id AS NVARCHAR(MAX))
--	END 
--    ElSE
--	BEGIN 
--		SET @Where = @Where + ' AND RA.Id = ' + CAST(@Id AS NVARCHAR(MAX))
--	END 
--END 
--
IF(@IdRadicado <> 0)
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.IdRadicado = ' + CAST(@IdRadicado AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.IdRadicado = ' + CAST(@IdRadicado AS NVARCHAR(MAX))
	END 
END 
--
IF(@IdMedicion <> 0)
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.IdMedicion = ' + CAST(@IdMedicion AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.IdMedicion = ' + CAST(@IdMedicion AS NVARCHAR(MAX))
	END 
END 
--
IF(@IdPeriodo <> 0)
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.IdPeriodo = ' + CAST(@IdPeriodo AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.IdPeriodo = ' + CAST(@IdPeriodo AS NVARCHAR(MAX))
	END 
END 
--
IF(@IdAuditor <> 0)
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.IdAuditor = ' + CAST(@IdAuditor AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.IdAuditor = ' + CAST(@IdAuditor AS NVARCHAR(MAX))
	END 
END 
--
IF(@PrimerNombre <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.PrimerNombre LIKE ''%' + CAST(@PrimerNombre AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.PrimerNombre LIKE ''%' + CAST(@PrimerNombre AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@SegundoNombre <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.SegundoNombre LIKE ''%' + CAST(@SegundoNombre AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.SegundoNombre LIKE ''%' + CAST(@SegundoNombre AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@PrimerApellido <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.PrimerApellido LIKE ''%' + CAST(@PrimerApellido AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.PrimerApellido LIKE ''%' + CAST(@PrimerApellido AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@SegundoApellido <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.SegundoApellido LIKE ''%' + CAST(@SegundoApellido AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.SegundoApellido LIKE ''%' + CAST(@SegundoApellido AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@Sexo <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.Sexo LIKE ''%' + CAST(@Sexo AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.Sexo LIKE ''%' + CAST(@Sexo AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@TipoIdentificacion <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.TipoIdentificacion LIKE ''%' + CAST(@TipoIdentificacion AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.TipoIdentificacion LIKE ''%' + CAST(@TipoIdentificacion AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@Identificacion <> 0)
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.Identificacion = ' + CAST(@Identificacion AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.Identificacion = ' + CAST(@Identificacion AS NVARCHAR(MAX))
	END 
END 
--
IF(@FechaNacimientoIni <> '' OR @FechaNacimientoFin <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.FechaNacimiento >= ''' + CAST(@FechaNacimientoIni AS NVARCHAR(MAX)) + ''' AND RA.FechaNacimiento <= ''' + CAST(@FechaNacimientoFin AS NVARCHAR(MAX)) + ''''
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.FechaNacimiento >= ''' + CAST(@FechaNacimientoIni AS NVARCHAR(MAX)) + ''' AND RA.FechaNacimiento <= ''' + CAST(@FechaNacimientoFin AS NVARCHAR(MAX)) + ''''
	END 
END 
--
IF(@FechaCreacionIni <> '' OR @FechaCreacionFin <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.FechaCreacion >= ''' + CAST(@FechaCreacionIni AS NVARCHAR(MAX)) + ''' AND RA.FechaCreacion <= ''' + CAST(@FechaCreacionFin AS NVARCHAR(MAX)) + ''''
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.FechaCreacion >= ''' + CAST(@FechaCreacionIni AS NVARCHAR(MAX)) + ''' AND RA.FechaCreacion <= ''' + CAST(@FechaCreacionFin AS NVARCHAR(MAX)) + ''''
	END 
END 
--
IF(@FechaAuditoriaIni <> '' OR @FechaAuditoriaFin <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.FechaAuditoria >= ''' + CAST(@FechaAuditoriaIni AS NVARCHAR(MAX)) + ''' AND RA.FechaAuditoria <= ''' + CAST(@FechaAuditoriaFin AS NVARCHAR(MAX)) + ''''
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.FechaAuditoria >= ''' + CAST(@FechaAuditoriaIni AS NVARCHAR(MAX)) + ''' AND RA.FechaAuditoria <= ''' + CAST(@FechaAuditoriaFin AS NVARCHAR(MAX)) + ''''
	END 
END 
--
IF(@FechaMinConsultaAudit <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.FechaMinConsultaAudit >= ''' + CAST(@FechaMinConsultaAudit AS NVARCHAR(MAX)) + ''
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.FechaMinConsultaAudit >= ''' + CAST(@FechaMinConsultaAudit AS NVARCHAR(MAX)) + ''
	END 
END 
--
IF(@FechaMaxConsultaAudit <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.FechaMaxConsultaAudit <= ''' + CAST(@FechaMaxConsultaAudit AS NVARCHAR(MAX)) + ''
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.FechaMaxConsultaAudit <= ''' + CAST(@FechaMaxConsultaAudit AS NVARCHAR(MAX)) + ''
	END 
END 
--
IF(@Activo = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.Activo = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.Activo = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@Activo = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.Activo = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.Activo = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@Conclusion <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.Conclusion LIKE ''%' + CAST(@Conclusion AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.Conclusion LIKE ''%' + CAST(@Conclusion AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@UrlSoportes <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.UrlSoportes LIKE ''%' + CAST(@UrlSoportes AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.UrlSoportes LIKE ''%' + CAST(@UrlSoportes AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@Reverse = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.Reverse = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.Reverse = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@Reverse = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.Reverse = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.Reverse = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@DisplayOrder <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.DisplayOrder LIKE ''%' + CAST(@DisplayOrder AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.DisplayOrder LIKE ''%' + CAST(@DisplayOrder AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@Ara = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.Ara = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.Ara = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@Ara = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.Ara = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.Ara = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@Eps = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.Eps = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.Eps = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@Eps = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.Eps = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.Eps = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@FechaReversoIni <> '' OR @FechaReversoFin <> '')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.FechaReverso >= ''' + CAST(@FechaReversoIni AS NVARCHAR(MAX)) + ''' AND RA.FechaReverso <= ''' + CAST(@FechaReversoFin AS NVARCHAR(MAX)) + ''''
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.FechaReverso >= ''' + CAST(@FechaReversoIni AS NVARCHAR(MAX)) + ''' AND RA.FechaReverso <= ''' + CAST(@FechaReversoFin AS NVARCHAR(MAX)) + ''''
	END 
END 
--
IF(@AraAtendido = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.AraAtendido = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.AraAtendido = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@AraAtendido = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.AraAtendido = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.AraAtendido = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@EpsAtendido = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.EpsAtendido = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.EpsAtendido = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@EpsAtendido = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.EpsAtendido = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.EpsAtendido = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@Revisar = 'true')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.Revisar = ' + CAST(1 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.Revisar = ' + CAST(1 AS NVARCHAR(MAX))
	END 
END 
ElSE IF(@Revisar = 'false')
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.Revisar = ' + CAST(0 AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.Revisar = ' + CAST(0 AS NVARCHAR(MAX))
	END 
END 
--
IF(@Estado <> 0)
BEGIN 
	IF(@Where = '')
	BEGIN 
		SET @Where = @Where + 'RA.Estado = ' + CAST(@Estado AS NVARCHAR(MAX))
	END 
    ElSE
	BEGIN 
		SET @Where = @Where + ' AND RA.Estado = ' + CAST(@Estado AS NVARCHAR(MAX))
	END 
END 
--


--Paginado
DECLARE @Paginado VARCHAR(MAX) = '
ORDER BY RA.Id
OFFSET ' + CAST(@Paginate AS NVARCHAR(MAX)) + ' ROWS
FETCH NEXT ' + CAST(@MaxRows AS NVARCHAR(MAX)) + ' ROWS ONLY;'

--Concatenamos Query y Condiciones.
IF(@Where <> '' )
BEGIN 
	SET @Where = ' WHERE ' + @Where;              
END  
DECLARE @Total VARCHAR(MAX);
SET @Total = @Query + '' + @Where + ' ' + @Paginado
EXEC(@Total);

END
GO
/****** Object:  StoredProcedure [dbo].[RegistrosAuditoriaFiltros]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
CREATE PROCEDURE [dbo].[RegistrosAuditoriaFiltros]
(@IdAuditor VARCHAR(150))
AS
BEGIN
SET NOCOUNT ON

--Para convertir 'all' a vacio.
IF @IdAuditor = 'all'
BEGIN
SET @IdAuditor = ''
END

--Guardamos Query inicial.  
DECLARE @Query VARCHAR(MAX) = '';  


--Consultamos IPS. FALTA. PENDIENTE
--SET @Query = @Query + ''
--UNION

--Consultamos IdMedicion.
SET @Query = @Query + 'SELECT NEWID() As Idk, ''Bolsa medicion'' As NombreFiltro, CAST(MAX(ME.Id) AS NVARCHAR(MAX)) As Id, CAST(MAX(ME.Nombre) AS NVARCHAR(MAX)) As Valor 
FROM medicion ME 
INNER JOIN RegistrosAuditoria RE ON (ME.Id = RE.IdMedicion) '
IF(@IdAuditor <> '')
BEGIN
	SET @Query = @Query + 'WHERE RE.IdAuditor IN (''' + CAST(@IdAuditor AS NVARCHAR(MAX)) + ''') AND ME.Status = 1 AND RE.Status = 1'
END
SET @Query = @Query + 'GROUP BY RE.IdMedicion '
--
  
SET @Query = @Query + ' UNION '

--Consultamos Fecha inicio de auditoria. CONFIRMAR. CAC [FechaMinConsultaAudit]. Antiguo FechaAuditoria.
--SELECT 'Fecha inicio de auditoria' As NombreFiltro, Id As Id, CAST(FechaMinConsultaAudit AS NVARCHAR(MAX)) As Valor
--SELECT 'Fecha inicio de auditoria' As NombreFiltro, CAST(FechaMinConsultaAudit AS NVARCHAR(MAX)) As Valor
--FROM RegistrosAuditoria
--WHERE IdAuditor = @IdAuditor
--

--UNION

--Consultamos Fecha cierre de auditoria. CONFIRMAR. CAC [FechaMaxConsultaAudit]. Antiguo FechaAuditoria.
--SELECT 'Fecha cierre de auditoria' As NombreFiltro, Id As Id, CAST(FechaMaxConsultaAudit AS NVARCHAR(MAX)) As Valor
--SELECT 'Fecha cierre de auditoria' As NombreFiltro, CAST(FechaMaxConsultaAudit AS NVARCHAR(MAX)) As Valor
--FROM RegistrosAuditoria
--WHERE IdAuditor = @IdAuditor
--

--UNION

--Consultamos Entidad. AÑADIR CAMPO IdEPS. Queda pendiente disponible tabla.
--Tablas en cac para alamacenar IPS y EPS y como se relacionan con el registro del paciente.
SET @Query = @Query + 'SELECT NEWID() As Idk, ''Entidad'' As NombreFiltro, CAST(MAX(E.EpsId) AS NVARCHAR(MAX)) As Id, CAST(MAX(E.Nombre) AS NVARCHAR(MAX)) As Valor 
FROM RegistrosAuditoria RA 
INNER JOIN Eps E ON (E.EpsId = RA.IdEPS) '
IF(@IdAuditor <> '')
BEGIN
	SET @Query = @Query + 'WHERE RA.IdAuditor IN (''' + CAST(@IdAuditor AS NVARCHAR(MAX)) + ''')  AND RA.Status = 1 AND E.Status = 1'
END
SET @Query = @Query + 'GROUP BY E.Nombre '
--

SET @Query = @Query + ' UNION '

--Consultamos Estado de registro. CONFIRMAR.
SET @Query = @Query + 'SELECT NEWID() As Idk, ''Estado de registro'' As NombreFiltro, CAST(MAX(RA.Estado) AS NVARCHAR(MAX)) As Id, CAST(MAX(ERA.Descripción) AS NVARCHAR(MAX)) As Valor 
FROM RegistrosAuditoria RA 
INNER JOIN EstadosRegistroAuditoria ERA ON (ERA.Id = RA.Estado) '
IF(@IdAuditor <> '')
BEGIN
	SET @Query = @Query + 'WHERE RA.IdAuditor IN (''' + CAST(@IdAuditor AS NVARCHAR(MAX)) + ''')  AND RA.Status = 1 AND ERA.Status = 1'
END
SET @Query = @Query + 'GROUP BY RA.Estado '
--

SET @Query = @Query + ' UNION '

--Consultamos ModifyBy.
SET @Query = @Query + 'SELECT NEWID() As Idk, ''ModifyBy'' As NombreFiltro, CAST(MAX(ModifyBy) AS NVARCHAR(MAX)) As Id, CAST(MAX(ModifyBy) AS NVARCHAR(MAX)) As Valor 
FROM RegistrosAuditoria '
--AGREGAR JOIN A USUARIOS ?
IF(@IdAuditor <> '')
BEGIN
	SET @Query = @Query + 'WHERE IdAuditor IN (''' + CAST(@IdAuditor AS NVARCHAR(MAX)) + ''')  AND Status = 1'
END
SET @Query = @Query + 'GROUP BY ModifyBy '
--

SET @Query = @Query + ' UNION '

--Consultamos ModifyDate.
--SELECT 'ModifyDate' As NombreFiltro, Id As Id, CAST(ModifyDate AS NVARCHAR(MAX)) As Valor
--SELECT DISTINCT 'ModifyDate' As NombreFiltro, CAST(ModifyDate AS NVARCHAR(MAX)) As Valor
--FROM RegistrosAuditoria
--WHERE IdAuditor = @IdAuditor
--

--UNION

--Consultamos Sexo. Crear campo en registro auditoria.
SET @Query = @Query + 'SELECT NEWID() As Idk, ''Sexo'' As NombreFiltro, CAST(MAX(Sexo) AS NVARCHAR(MAX)) As Id, CAST(MAX(Sexo) AS NVARCHAR(MAX)) As Valor 
FROM RegistrosAuditoria '
IF(@IdAuditor <> '')
BEGIN
	SET @Query = @Query + 'WHERE IdAuditor IN (''' + CAST(@IdAuditor AS NVARCHAR(MAX)) + ''')  AND Status = 1'
END
SET @Query = @Query + 'GROUP BY Sexo '
--

--ORDER BY
--ORDER BY NombreFiltro, CAST(Id AS INT) ASC
SET @Query = @Query + ' ORDER BY NombreFiltro, Id '

--Imprimimos/Ejecutamos.  
EXEC(@Query); 
--SELECT(@Query); 
--PRINT(@Query); 

END
GO
/****** Object:  StoredProcedure [dbo].[RegistrosAuditoriaFiltrosTest]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
CREATE PROCEDURE [dbo].[RegistrosAuditoriaFiltrosTest]
(@IdAuditor VARCHAR(150))
AS
BEGIN
SET NOCOUNT ON

--Para convertir 'all' a vacio.
IF @IdAuditor = 'all'
BEGIN
SELECT NEWID() As Idk, 'Bolsa medicion' As NombreFiltro, CAST(MAX(ME.Id) AS NVARCHAR(MAX)) As Id, CAST(MAX(ME.Descripcion) AS NVARCHAR(MAX)) As Valor
FROM Medicion ME
INNER JOIN RegistrosAuditoria RE ON (ME.Id = RE.IdMedicion)
GROUP BY RE.IdMedicion
--
UNION
SELECT NEWID() As Idk, 'Estado de registro' As NombreFiltro, CAST(MAX(RA.Estado) AS NVARCHAR(MAX)) As Id, CAST(MAX(ERA.Descripción) AS NVARCHAR(MAX)) As Valor
FROM RegistrosAuditoria RA
INNER JOIN EstadosRegistroAuditoria ERA ON (ERA.Id = RA.Estado)
GROUP BY RA.Estado
--
UNION
SELECT NEWID() As Idk, 'ModifyBy' As NombreFiltro, CAST(MAX(ModifyBy) AS NVARCHAR(MAX)) As Id, CAST(MAX(ModifyBy) AS NVARCHAR(MAX)) As Valor
FROM RegistrosAuditoria
GROUP BY ModifyBy
--
UNION
SELECT NEWID() As Idk, 'Sexo' As NombreFiltro, CAST(MAX(Sexo) AS NVARCHAR(MAX)) As Id, CAST(MAX(Sexo) AS NVARCHAR(MAX)) As Valor
FROM RegistrosAuditoria
GROUP BY Sexo
--
--ORDER BY
--ORDER BY NombreFiltro, CAST(Id AS INT) ASC
ORDER BY NombreFiltro, Id
END
ELSE 
BEGIN
--Consultamos IdMedicion.
SELECT NEWID() As Idk, 'Bolsa medicion' As NombreFiltro, CAST(MAX(ME.Id) AS NVARCHAR(MAX)) As Id, CAST(MAX(ME.Descripcion) AS NVARCHAR(MAX)) As Valor
--SELECT 'Bolsa medicion' As NombreFiltro, CAST(ME.Descripcion AS NVARCHAR(MAX)) As Valor
FROM medicion ME
INNER JOIN RegistrosAuditoria RE ON (ME.Id = RE.IdMedicion)
WHERE RE.IdAuditor = @IdAuditor
GROUP BY RE.IdMedicion
--
UNION
--Consultamos Estado de registro. CONFIRMAR.
SELECT NEWID() As Idk, 'Estado de registro' As NombreFiltro, CAST(MAX(RA.Estado) AS NVARCHAR(MAX)) As Id, CAST(MAX(ERA.Descripción) AS NVARCHAR(MAX)) As Valor
FROM RegistrosAuditoria RA
INNER JOIN EstadosRegistroAuditoria ERA ON (ERA.Id = RA.Estado)
WHERE RA.IdAuditor = @IdAuditor
GROUP BY RA.Estado
--
UNION
--Consultamos ModifyBy.
SELECT NEWID() As Idk, 'ModifyBy' As NombreFiltro, CAST(MAX(ModifyBy) AS NVARCHAR(MAX)) As Id, CAST(MAX(ModifyBy) AS NVARCHAR(MAX)) As Valor
--SELECT DISTINCT 'ModifyBy' As NombreFiltro, CAST(ModifyBy AS NVARCHAR(MAX)) As Valor
FROM RegistrosAuditoria
--AGREGAR JOIN A USUARIOS ?
WHERE IdAuditor = @IdAuditor
GROUP BY ModifyBy
--
UNION
--Consultamos Sexo. Crear campo en registro auditoria.
SELECT NEWID() As Idk, 'Sexo' As NombreFiltro, CAST(MAX(Sexo) AS NVARCHAR(MAX)) As Id, CAST(MAX(Sexo) AS NVARCHAR(MAX)) As Valor
--SELECT DISTINCT 'Sexo' As NombreFiltro, CAST(Sexo AS NVARCHAR(MAX)) As Valor
FROM RegistrosAuditoria
WHERE IdAuditor = @IdAuditor
GROUP BY Sexo
--

--ORDER BY
--ORDER BY NombreFiltro, CAST(Id AS INT) ASC
ORDER BY NombreFiltro, Id
END
END
GO
/****** Object:  StoredProcedure [dbo].[RegistrosAuditoriaProgresoDiario]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Creacion/Edicion Procedure
CREATE PROCEDURE [dbo].[RegistrosAuditoriaProgresoDiario]
(@IdAuditor VARCHAR(150))
AS
BEGIN
SET NOCOUNT ON

----Consultamos Fechas del rango.
--DECLARE @FechaActual DATE = (SELECT FORMAT(GETDATE(), 'yyyy-MM-dd') As date)
--DECLARE @FechaIniRango DATE = (SELECT FORMAT(CONVERT(datetime,dateadd(d,-(day(getdate()-1)),getdate()),106), 'yyyy-MM-dd') As date)
--DECLARE @FechaFinRango DATE = (SELECT FORMAT(CONVERT(datetime,dateadd(d,-(day(dateadd(m,1,getdate()))),dateadd(m,1,getdate())),106), 'yyyy-MM-dd') As date)

----Consultamos el tiempo (Tiempo del periodo).
--DECLARE @FechaMinConsultaAudit DATETIME = (
--SELECT DISTINCT FechaMinConsultaAudit As Valor
--FROM RegistrosAuditoria RA
--INNER JOIN EstadosRegistroAuditoria ERA ON (ERA.id = RA.Estado)
--WHERE RA.IdAuditor = @IdAuditor AND ERA.Id != 7
--AND (FORMAT(ModifyDate, 'yyyy-MM-dd') >= @FechaIniRango AND FORMAT(ModifyDate, 'yyyy-MM-dd') <= @FechaFinRango ) AND ERA.Id != 7);  --REVISAR RANGO DE FECHAS.
----
--DECLARE @FechaMaxConsultaAudit DATETIME = (
--SELECT DISTINCT FechaMaxConsultaAudit As Valor
--FROM RegistrosAuditoria RA
--INNER JOIN EstadosRegistroAuditoria ERA ON (ERA.id = RA.Estado)
--WHERE RA.IdAuditor = @IdAuditor AND ERA.Id != 7 
--AND (FORMAT(ModifyDate, 'yyyy-MM-dd') >= @FechaIniRango AND FORMAT(ModifyDate, 'yyyy-MM-dd') <= @FechaFinRango ) AND ERA.Id != 7);  --REVISAR RANGO DE FECHAS.


----Calculamos dias.
--DECLARE @NoDias INT = (
--SELECT DATEDIFF (DAY, @FechaMinConsultaAudit, @FechaMaxConsultaAudit) As Resultado)


----Consultamos Numero de registros que no esten en estado pendiente.
--DECLARE @NoRegistros INT = (
--SELECT COUNT(RA.Id) As NoRegistros
--FROM RegistrosAuditoria RA
--INNER JOIN EstadosRegistroAuditoria ERA ON (ERA.id = RA.Estado)
--WHERE RA.IdAuditor = @IdAuditor AND ERA.Id != 7); 
----
--DECLARE @NoRegistrosActuales INT = (
--SELECT COUNT(RA.Id) As NoRegistros
--FROM RegistrosAuditoria RA
--INNER JOIN EstadosRegistroAuditoria ERA ON (ERA.id = RA.Estado)
--WHERE RA.IdAuditor = @IdAuditor AND (@FechaActual = FORMAT(ModifyDate, 'yyyy-MM-dd')) AND ERA.Id = 7);  --REVISAR ESTADO.


----Calculamos.
--DECLARE @Total INT = @NoRegistros / @NoDias;
--SELECT CAST(@NoRegistrosActuales AS NVARCHAR(MAX)) As ProgresoDia, CAST(@Total AS NVARCHAR(MAX)) As Totales;
---- 0/@Total

----PRINT(@NoDias);
----PRINT(@NoRegistros);

----FALTA AÑADIR EL PERIODO DE TIEMPO EN LA CONDICIÓN.

DECLARE @Total NVARCHAR(50);
DECLARE @NoRegistrosActuales NVARCHAR(50);

SET @Total = CAST((SELECT  COUNT(*) FROM RegistrosAuditoria where IdAuditor = @IdAuditor and FechaAsignacion = CAST(GETDATE() AS DATE)) AS NVARCHAR(50))
SET @NoRegistrosActuales = CAST((SELECT  COUNT(*) FROM RegistrosAuditoria where IdAuditor = @IdAuditor AND FechaAsignacion = CAST(GETDATE() AS DATE) AND  Estado NOT IN(1,17)) AS NVARCHAR(50))  --Registro Nuevo (1) y Registro Pendiente (17).


SELECT CAST(@NoRegistrosActuales AS NVARCHAR(MAX)) As ProgresoDia, CAST(@Total AS NVARCHAR(MAX)) As Totales;

END
GO
/****** Object:  StoredProcedure [dbo].[showUserCode]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE   PROCEDURE [dbo].[showUserCode]
	@correo Varchar(150)

AS 
BEGIN 

	select (select aud.Codigo
		from AspNetUsers aspn
			inner join AspNetUsersDetelles as aud 
			ON aspn.Id = aud.AspNetUsersId
		where aspn.UserName = @correo) as Respuesta;

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Actualiza_Estado_Registro_Auditoria]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Actualiza_Estado_Registro_Auditoria]
	-- Add the parameters for the stored procedure here
	@userId NVARCHAR(255),
	@RegistroAuditoriaId INT,
	@BotonAccion INT
AS
BEGIN
		-- Variables
		DECLARE @Estado INT = (SELECT Estado FROM RegistrosAuditoria where Id = @RegistroAuditoriaId);
		DECLARE @NombreEstado NVARCHAR(255);
		DECLARE @NuevoEstado INT = @Estado;
		DECLARE @NombreNuevoEstado NVARCHAR(255);
		DECLARE @Observacion NVARCHAR(1024) = '';
		DECLARE @Observacion2 NVARCHAR(1024) = '';
		DECLARE @IdItemGlosa INT = (SELECT Id FROM Item WHERE ItemName = 'Glosas' and CatalogId = 4);
		DECLARE @IdItemDC INT = (SELECT Id FROM Item WHERE ItemName = 'DC' and CatalogId = 6);
		DECLARE @IdItemNC INT = (SELECT Id FROM Item WHERE ItemName = 'NC' and CatalogId = 6);
		DECLARE @CountGlosaNCND INT = 0;
		DECLARE @TipoObservacion INT = (SELECT Id FROM  Item where ItemName = 'General' AND CatalogId = 1)
		DECLARE @LevantarGlosa INT = (SELECT LevantarGlosa FROM RegistrosAuditoria where Id = @RegistroAuditoriaId);
		DECLARE @MantenerCalifiacion INT = (SELECT MantenerCalificacion FROM RegistrosAuditoria where Id = @RegistroAuditoriaId);


		-- Count para saber si hay glosas NC o ND
		SET @CountGlosaNCND = (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rg
								INNER JOIN RegistrosAuditoria ra ON rg.RegistrosAuditoriaId = ra.Id
								INNER JOIN VariableXMedicion vm ON vm.VariableId = rg.VariableId AND ra.IdMedicion = vm.MedicionId
							  WHERE rg.RegistrosAuditoriaId = @RegistroAuditoriaId 
								AND vm.SubGrupoId = @IdItemGlosa -- Glosa
								AND rg.Dato_DC_NC_ND <> @IdItemDC -- DC
								)
								
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

		ELSE IF (@Estado = 17 AND @CountGlosaNCND = 0) -- RP Registro pendiente
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
END
GO
/****** Object:  StoredProcedure [dbo].[SP_AsignaVariablesMedicion]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create   PROCEDURE [dbo].[SP_AsignaVariablesMedicion] 
	@IdMedicion INT,
	@UserId varchar(50)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @IdCobertura INT
	SELECT @IdCobertura = IdCobertura FROM Medicion WHERE Id = @IdMedicion

	DECLARE @TableVariables as table (
		Id int not null primary key identity(1,1), 
		IdVariable int not null,
		Orden int not null
	)
	
	DECLARE @Count int = 0, @Register int = 1

	INSERT @TableVariables SELECT  Id, Orden FROM Variables WHERE idCobertura = @IdCobertura

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
	
	WHILE(@Register <= @Count)
	BEGIN
		
		SELECT @VariableId = IdVariable, @Orden = Orden 
		FROM @TableVariables WHERE Id = @Register

		INSERT VariableXMedicion (
			VariableId, 
			MedicionId, 
			Orden,
			EsGlosa, 
			EsVisible, 
			EsCalificable, 
			Activo, 
			EnableDC, 
			EnableNC, 
			EnableND, 
			Encuesta,
			SubGrupoId,
			CalificacionXDefecto,
			CreatedBy,
			CreationDate,
			ModifyBy,
			ModificationDate,
			[Status]
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
			1,
			1,
			1,
			0,
			@Subgrupo,
			@CalificacionXDefecto,
			@UserId,
			GETDATE(),
			@UserId,
			GETDATE(),
			1
	   )		

		SET @Register = @Register + 1
	END

END
GO
/****** Object:  StoredProcedure [dbo].[SP_BolsasMedicion_XEnfermedadMadre]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-03-02
-- Description:	Para cargar listado de Bolsas de una enfermedad madre.
-- =============================================
CREATE PROCEDURE [dbo].[SP_BolsasMedicion_XEnfermedadMadre]
(@IdEnfermedadMadre NVARCHAR(MAX), @IdMedicion NVARCHAR(MAX))	
AS
BEGIN
	--Guardamos Query inicial.  
	DECLARE @Query VARCHAR(MAX);  
	SET @Query = 'SELECT ME.Id, ME.Nombre, (CAST(ME.id As NVARCHAR(255))+ '' - '' + ME.Nombre) As Total FROM Medicion ME'

	--Guardamos Condiciones.  
	DECLARE @Where VARCHAR(MAX) = '';  
	--  
	IF(@IdEnfermedadMadre <> '')  
	BEGIN   
	 IF(@Where = '')  
	 BEGIN   
	  SET @Where = @Where + 'ME.IdCobertura IN (''' + CAST(@IdEnfermedadMadre AS NVARCHAR(MAX)) + ''')'  
	 END   
		ElSE  
	 BEGIN   
	  SET @Where = @Where + ' AND ME.IdCobertura IN (''' + CAST(@IdEnfermedadMadre AS NVARCHAR(MAX)) + ''')'  
	 END   
	END   
	--  
	IF(@IdMedicion <> '')  
	BEGIN   
	 IF(@Where = '')  
	 BEGIN   
	  SET @Where = @Where + 'ME.Id <> ' + CAST(@IdMedicion AS NVARCHAR(MAX))
	 END   
		ElSE  
	 BEGIN   
	  SET @Where = @Where + ' AND ME.Id <> ' + CAST(@IdMedicion AS NVARCHAR(MAX))
	 END   
	END   
	--
	IF(@Where = '')  
	BEGIN   
		SET @Where = @Where + 'ME.Estado <> 30'
	END   
	ElSE  
	BEGIN   
		SET @Where = @Where + ' AND ME.Estado <> 30'
	END  
	-- //

	--Validamos estado activo.
	IF(@Where = '')  
	 BEGIN   
	  SET @Where = @Where + ' ME.Status = 1 ' 
	 END   
		ElSE  
	 BEGIN   
	  SET @Where = @Where + ' AND ME.Status = 1 '
	END 
	--  

	--Concatenamos Query, Condiciones y Paginado. Luego ejecutamos.  
	IF(@Where <> '' )  
	BEGIN   
	 SET @Where = ' WHERE ' + @Where;                
	END    
	DECLARE @Total VARCHAR(MAX);  
	SET @Total = @Query + '' + @Where   
	-- //

	--Imprimimos/Ejecutamos.  
	EXEC(@Total);
	--PRINT(@Total);

END --END SP
GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Detalle_Registros_Auditoria]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
-- SP_Consulta_Detalle_Registros_Auditoria 7673
--DROP PROCEDURE SP_Consulta_Detalle_Registros_Auditoria
CREATE PROCEDURE [dbo].[SP_Consulta_Detalle_Registros_Auditoria]
-- Add the parameters for the stored procedure here
	@IdRegistroAuditoria INT
AS
BEGIN
	
	DECLARE @IdItem INT Select @IdItem  = Id from Item where ItemName = 'Variables ocultas'

	SELECT  
		 rad.Id,
		 rad.RegistrosAuditoriaId,
		 'VAR' + CAST(vr.Id AS NVARCHAR(12)) AS CodigoVariable,
		 rad.VariableId AS VariableId,
		 --vr.nombre AS NombreVariable,
		 vr.descripcion AS NombreVariable,
		 rad.DatoReportado AS DatoReportado,
		 rad.EstadoVariableId AS EstadoVariableId,
		 ev.Descripcion AS NombreEstadoVariable,
		 CASE WHEN  rad.Visible = 0 THEN @IdItem ELSE vaxm.SubGrupoId END SubGrupoId,
		 CASE WHEN  rad.Visible = 0 THEN 'Variables ocultas' ELSE it.ItemName END SubGrupoDescripcion,
		 vr.MotivoVariable AS MotivoVariable,
		 rad.MotivoVariable AS Motivo,
		 --rad.Dato_DC_NC_ND As Dato_DC_NC_ND,
		 CASE WHEN  rad.Dato_DC_NC_ND IS NULL THEN vaxm.CalificacionXDefecto ELSE rad.Dato_DC_NC_ND END As Dato_DC_NC_ND,
	     rad.Visible As Visible,
		 vr.Bot As Bot,
		 vaxm.Encuesta As VariableEncuesta,
		 ra.Encuesta As RegistrosAuditoriaEncuesta,
		 --1 As IpsId,
		 '' As Nombre, 
		 tablaReferencial As TablaReferencial,
		 campoReferencial As CampoReferencial,
		 vr.idTipoVariable As IdTipoVariable,
		 vr.longitud As Longitud,
		 vr.decimales As Decimales,
		 cic.ItemDescripcion AS ValorDatoReportado
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
	 --LEFT JOIN VariableSubgrupo vsb ON vr.SubGrupoId = vsb.Id
	 WHERE RAD.RegistrosAuditoriaId = @IdRegistroAuditoria
	 ORDER BY vaxm.SubGrupoId
END --END PROCEDURE

GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Detalle_Registros_Auditoria2]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--Creacion/Edicion Procedure
--DROP PROCEDURE SP_Consulta_Detalle_Registros_Auditoria2
CREATE PROCEDURE [dbo].[SP_Consulta_Detalle_Registros_Auditoria2]
-- Add the parameters for the stored procedure here
	@IdRegistroAuditoria INT
AS
BEGIN
	
	SELECT  
		 rad.Id,
		 rad.RegistrosAuditoriaId,
		 'VAR' + CAST(vr.Id AS NVARCHAR(12)) AS CodigoVariable,
		 rad.VariableId AS VariableId,
		 --vr.nombre AS NombreVariable,
		 vr.descripcion AS NombreVariable,
		 rad.DatoReportado AS DatoReportado,
		 rad.EstadoVariableId AS EstadoVariableId,
		 ev.Descripcion AS NombreEstadoVariable,
		 CASE WHEN  rad.Visible = 1 THEN 10 ELSE vaxm.SubGrupoId END SubGrupoId,
		 CASE WHEN  rad.Visible = 1 THEN 'Variables ocultas' ELSE it.ItemName END SubGrupoDescripcion,
		 vr.MotivoVariable AS MotivoVariable,
		 rad.MotivoVariable AS Motivo,
		 --rad.Dato_DC_NC_ND As Dato_DC_NC_ND,
		 CASE WHEN  rad.Dato_DC_NC_ND IS NULL THEN vaxm.CalificacionXDefecto ELSE rad.Dato_DC_NC_ND END As Dato_DC_NC_ND,
	     rad.Visible As Visible,
		 vr.Bot As Bot,
		 vaxm.Encuesta As VariableEncuesta,
		 ra.Encuesta As RegistrosAuditoriaEncuesta,
		 --1 As IpsId,
		 '' As Nombre, 
		 tablaReferencial As TablaReferencial,
		 campoReferencial As CampoReferencial

	FROM  

	 registrosauditoriadetalle rad
	 INNER JOIN Variables vr ON rad.VariableId = vr.Id
	 INNER JOIN VariableXMedicion vaxm ON vr.id = vaxm.VariableId
	 INNER JOIN EstadoVariable ev ON rad.EstadoVariableId = ev.Id
	 LEFT JOIN Item it ON vaxm.SubGrupoId = it.Id
	 INNER JOIN RegistrosAuditoria ra ON (rad.RegistrosAuditoriaId = ra.Id)
	 --LEFT JOIN VariableSubgrupo vsb ON vr.SubGrupoId = vsb.Id
	 WHERE RAD.RegistrosAuditoriaId = @IdRegistroAuditoria
END --END PROCEDURE
GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_ErrorRegistroAuditoria]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar - IT SENSE
-- Create date: 2022-03-04
-- Description: Consulta RegistroAuditoriaDetalleError
-- =============================================
CREATE PROCEDURE [dbo].[SP_Consulta_ErrorRegistroAuditoria]	
	@RegistrosAuditoriaId INT

AS 
BEGIN

					SELECT re.Id
						  ,re.RegistrosAuditoriaDetalleId
						  ,re.Reducido
						  ,re.VariableId
						  ,re.ErrorId
						  ,re.Descripcion
						  ,re.Enable
						  ,re.CreatedBy
						  ,re.CreatedDate
						  ,re.ModifyBy
						  ,re.ModifyDate
					  FROM RegistrosAuditoriaDetalle rd
							INNER JOIN  RegistroAuditoriaDetalleError re ON rd.Id = RegistrosAuditoriaDetalleId
							WHERE rd.RegistrosAuditoriaId = @RegistrosAuditoriaId


END

GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Log_Accion]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar - IT SENSE
-- Create date: 2022-02-22
-- Description:	consulta el log de accion filtrando por fecha o nombre, observacion, codigo auditor
-- =============================================
CREATE PROCEDURE [dbo].[SP_Consulta_Log_Accion]

	@ParametroBusqueda NVARCHAR(255),
	@FechaInicial NVARCHAR(255),
	@FechaFinal NVARCHAR(255),
	@IdAuditores NVARCHAR(255),
	@Paginate INT,
	@MaxRows INT
AS
BEGIN
	
		--- Query Variables
		DECLARE @QueryTotal VARCHAR(MAX) = '';
		DECLARE @QueryTotalCount VARCHAR(MAX) = '';
		DECLARE @QueryBase VARCHAR(MAX) = '';
		DECLARE @QueryBaseCount VARCHAR(MAX) = '';
		DECLARE @QueryWhere VARCHAR(MAX) = '';
		DECLARE @Paginador VARCHAR(MAX) =  '  ORDER BY RA.Id OFFSET (' + CAST(@Paginate AS NVARCHAR(255)) +  '- 1) * ' + CAST(@MaxRows AS NVARCHAR(255)) + ' ROWS FETCH NEXT ' + CAST(@MaxRows AS NVARCHAR(255)) + ' ROWS ONLY;'

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
				,RA.[EstadoActual],
				 AUD.Codigo
				,AUD.Nombres
				 ,AUD.Apellidos
				,AU.[UserName] as NombreUsuario
				,RA.[CreatedDate]
				,RA.[ModifyBy]
				,RA.[ModificationDate],
				RA.Status
		FROM [RegistroAuditoriaLog] RA
		INNER JOIN [AspNetUsers] AU on RA.AsignadoA = AU.Id
		INNER JOIN  AspNetUsersDetelles AUD ON AUD.AspNetUsersId = AU.Id
		INNER JOIN RegistrosAuditoria rsa ON RA.RegistroAuditoriaId = rsa.Id
			WHERE RA.CreatedBy IN(' + @IdAuditores + ') ' 

		SET @QueryBaseCount = 
		'SELECT 
				COUNT(*)
		FROM [RegistroAuditoriaLog] RA
		INNER JOIN [AspNetUsers] AU on RA.AsignadoA = AU.Id
		INNER JOIN  AspNetUsersDetelles AUD ON AUD.AspNetUsersId = AU.Id
		INNER JOIN RegistrosAuditoria rsa ON RA.RegistroAuditoriaId = rsa.Id
			WHERE RA.CreatedBy IN(' + @IdAuditores + ') ' 

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
					'AUD.Nombres LIKE ''%' + @ParametroBusqueda + '%''' +
					' OR ' +
					'AUD.Apellidos LIKE ''%' + @ParametroBusqueda + '%''' +
					' OR ' +
					'CAST(AUD.Codigo AS NVARCHAR(255)) LIKE ''%' + @ParametroBusqueda + '%''' +
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

			SET @QueryTotal = REPLACE(@QueryTotal, 'ScriptQueryCount', @QueryTotalCount) + @Paginador

			EXEC(@QueryTotal)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Perfil_Accion]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_Consulta_Perfil_Accion]
AS  
BEGIN  
SET NOCOUNT ON  

DECLARE @idLider INT = (SELECT Id FROM AspNetRoles WHERE Name = 'Lider')
DECLARE @idAuditor INT = (SELECT Id from AspNetRoles WHERE Name = 'Auditor')


SELECT @idLider AS RoleId, 'Lider' AS RoleName, '5,6,9,10,14' AS ActionStatus       
UNION
SELECT @idAuditor AS RoleId, 'Auditor' AS RoleName, '1,3,5,7,11,12,15,17' AS ActionStatus 

END --END SP
GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Registros_Auditoria_Filtrados]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2021-10-10
-- Description:	Consulta registros de auditoria filtrados
-- =============================================
CREATE PROCEDURE [dbo].[SP_Consulta_Registros_Auditoria_Filtrados] 
	-- Add the parameters for the stored procedure here
	@IdAuditor NVARCHAR(MAX),
	@Mediciones NVARCHAR(MAX),
	@Estados NVARCHAR(MAX),
	@Sexo NVARCHAR(MAX),
	@ModifyBy NVARCHAR(MAX),
	@Id NVARCHAR(MAX),
	@FechaAsignacionInicial NVARCHAR(MAX),
	@FechaAsignacionFinal NVARCHAR(MAX)
AS
BEGIN
		
		DECLARE @QueryScript NVARCHAR(MAX) = ''

		SET @QueryScript = 'SELECT RA.Id,
				   RA.IdRadicado,
				   RA.IdMedicion,
				   ME.Descripcion AS NombreMedicion,
				   RA.IdPeriodo,
				   RA.IdAuditor,
				   RA.PrimerNombre,
				   RA.SegundoNombre,
				   RA.PrimerApellido,
				   RA.SegundoApellido,
				   RA.Sexo,
				   RA.TipoIdentificacion,
				   RA.Identificacion,
				   RA.FechaNacimiento,
				   RA.FechaCreacion,
				   RA.FechaAuditoria,
				   RA.FechaMinConsultaAudit,
				   RA.FechaMaxConsultaAudit,
				   RA.Activo,
				   RA.Conclusion,
				   RA.UrlSoportes,
				   RA.Reverse,
				   RA.DisplayOrder,
				   RA.Ara,
				   RA.Eps,
				   RA.FechaReverso,
				   RA.AraAtendido,
				   RA.EpsAtendido,
				   RA.Revisar,
				   RA.Estado,
				   RA.CreatedBy,
				   RA.CreatedDate,
				   RA.ModifyBy,
				   RA.ModifyDate,
				   RA.FechaAsignacion
			FROM RegistrosAuditoria RA
			INNER JOIN Medicion ME ON (RA.IdMedicion = ME.Id)

				WHERE 
		  RA.IdAuditor IN (' + '''' + @IdAuditor + '''' + ') '


		  IF @Mediciones <> ''
		  BEGIN
			SET @QueryScript = @QueryScript + ' AND RA.IdMedicion IN (' + @Mediciones + ') '
		  END

		  IF @FechaAsignacionInicial <> '' AND @FechaAsignacionFinal = ''
		  BEGIN
			SET @QueryScript = @QueryScript + ' AND RA.FechaAsignacion = ' + '''' +  @FechaAsignacionInicial + '''' 
		  END

		  IF @FechaAsignacionInicial <> '' AND @FechaAsignacionFinal <> ''
		  BEGIN
			SET @QueryScript = @QueryScript + ' AND FechaAsignacion BETWEEN ' + '''' + @FechaAsignacionInicial + '''' + ' AND ' + '''' + @FechaAsignacionFinal  + '''' 
		  END
		  
		  IF @ModifyBy <> ''
		  BEGIN
			SET @QueryScript = @QueryScript + ' AND RA.ModifyBy IN (' + @ModifyBy + ') '
		  END

		  IF @Id <> ''
		  BEGIN
			SET @QueryScript = @QueryScript + ' AND RA.ID IN (' + @Id + ') '
		  END
		  
		  IF @Sexo <> ''
		  BEGIN
			SET @QueryScript = @QueryScript + ' AND RA.Sexo IN (' + '''' + @Sexo + '''' + ') '
		  END

		  SET @QueryScript = @QueryScript + ' ORDER BY RA.DisplayOrder'


			

EXEC (@QueryScript)

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_RegistrosAuditoria_Info]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE  PROCEDURE [dbo].[SP_Consulta_RegistrosAuditoria_Info]
	@ListadoId NVARCHAR(MAX)
AS
BEGIN
	
	DECLARE @Ips NVARCHAR(255) = (SELECT TOP (1) Valor  FROM [dbo].[ParametrosGenerales] where Nombre = 'CampoReferencialIps')
	DECLARE @Eps NVARCHAR(255) = (SELECT TOP (1) Valor  FROM [dbo].[ParametrosGenerales] where Nombre = 'CampoReferencialEps')

	DECLARE @Query NVARCHAR(MAX) = ''

	SET @Query = @Query + 'SELECT CAST(NEWID() AS NVARCHAR(255)) AS Id, rad.RegistrosAuditoriaId AS RegistroAuditoriaId, rad.DatoReportado, vr.campoReferencial AS CampoReferencial, vr.tablareferencial AS TablaReferencial,'  + ''''+ '' + ''''  + ' AS Nombre  FROM [dbo].[RegistrosAuditoriaDetalle] rad '
	SET @Query = @Query + 'INNER JOIN variables vr on vr.Id = rad.VariableId '
	SET @Query = @Query + 'where rad.RegistrosAuditoriaId in (' + @ListadoId +') and vr.campoReferencial =' + ''''+ @Eps + '''' 

	SET @Query = @Query + ' UNION '

	SET @Query = @Query + 'SELECT  CAST(NEWID() AS NVARCHAR(255)) AS Id, rad.RegistrosAuditoriaId AS RegistroAuditoriaId,  rad.DatoReportado AS DatoReportado, vr.campoReferencial AS CampoReferencial, vr.tablareferencial  AS TablaReferencial,'  + ''''+ '' + ''''  + ' AS Nombre FROM [dbo].[RegistrosAuditoriaDetalle] rad ' 
	SET @Query = @Query + ' INNER JOIN variables vr on vr.Id = rad.VariableId '
	SET @Query = @Query + 'where rad.RegistrosAuditoriaId in (' + @ListadoId +') and vr.campoReferencial =' + ''''+ @Ips + '''' 


	exec(@Query)


END
GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Usuarios_Lider]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar - IT SENSE
-- Create date:  2022-02-22
-- Description:	Consulta Informacion de usuarios asignados a un lider
-- =============================================
CREATE PROCEDURE [dbo].[SP_Consulta_Usuarios_Lider]
	 @idUserLider NVARCHAR(255) 
AS
BEGIN

    SELECT 

		MAX(ue.IdCobertura) AS IdCobertura,
		MAX(m.Id) AS IdMedicion,
		MAX(ra.IdAuditor) AS IdAuditor,
		MAX(usd.Nombres)  AS Nombres,
		MAX(usd.Apellidos)   AS Apellidos,
		MAX(usd.Codigo)  AS Codigo,
		MAX(us.UserName)  AS NombreUsuario 

	FROM UsuarioXEnfermedad  ue 
			INNER JOIN Medicion m ON ue.IdCobertura = m.IdCobertura
			INNER JOIN RegistrosAuditoria ra ON ra.IdMedicion = m.Id
			INNER JOIN AspNetUsers us ON us.Id = ra.IdAuditor
			INNER JOIN AspNetUsersDetelles usd ON usd.AspNetUsersId = us.id
		WHERE ue.IdUsuario = @idUserLider
		GROUP BY ra.IdAuditor
END
GO
/****** Object:  StoredProcedure [dbo].[SP_MoverAlgunos_RegistrosAuditoria_BolsaMedicion]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-03-08
-- Description:	Para Alugnos registrosAuditorias de una Bolsa a otra. Usando un archivo plantilla.
-- =============================================
CREATE PROCEDURE [dbo].[SP_MoverAlgunos_RegistrosAuditoria_BolsaMedicion]
(@header NVARCHAR(MAX), @line NVARCHAR(MAX))
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   -- VARIABLES
	DECLARE  @MessageResult NVARCHAR(MAX) = 'OK'
	DECLARE  @idRadiacdo INT	
	DECLARE  @Separador NVARCHAR(255) = ','
	--
	DECLARE @codigoAuditor NVARCHAR(255) = '';
	DECLARE @perteneceBolsa INT = 0;
	DECLARE @fechasBolsa INT = 0;
	DECLARE @idRadiacdoExiste INT = 0;
	DECLARE @CodigoAuditorVinculado INT = 0;
	--
	DECLARE @fechaAsignacionBolsa NVARCHAR(255) = '';	
	DECLARE @IdAuditor NVARCHAR(255) = '';
	DECLARE @estado NVARCHAR(5) = 'RN';
	DECLARE @IdMedicionOrigen NVARCHAR(MAX) = '';
	DECLARE @IdMedicionDestino NVARCHAR(MAX) = '';

	IF CHARINDEX(';',@header) > 0 --CHAR(9). Para tab character. | ORIGINAL: IF CHARINDEX(';',@header) > 0 | ACTUAl: IF CHARINDEX(CHAR(9),@header) > 0
	BEGIN
		SET @Separador = ';' --CHAR(9). Para tab character. | ORIGINAL: SET @Separador = ';' | ACTUAl: SET @Separador = CHAR(9)
	END

	-- Que los registros efectivamente pertenezcan a la bolsa origen. OK
    -- La fecha de asignacion este dentro del rango de la destino. OK
    -- Que los codigos de los auditores si existan y se encuentren activos. OK
    -- Que la bolsa de destino no se encuentre en estado finalizado

	-- Construye tabla cabecera
	DECLARE @HeaderTable TABLE (Id int IDENTITY(1,1) PRIMARY KEY, Header NVARCHAR(MAX))
	INSERT @HeaderTable SELECT value FROM string_split(@header, @Separador)

	-- Construye tabla linea
	DECLARE @LineTable TABLE (Id int IDENTITY(1,1) PRIMARY KEY, Valor NVARCHAR(MAX))
	INSERT @LineTable SELECT value FROM string_split(@line, @Separador)
	
	--Unimos tabla Header y tabla Line.
	DECLARE @UnionTable TABLE (Id INT, header NVARCHAR(MAX), Valor NVARCHAR(MAX))
	INSERT INTO  @UnionTable SELECT H.Id, H.Header, L.Valor FROM @HeaderTable H INNER JOIN @LineTable L ON (H.Id = L.Id);

	-- //
	BEGIN TRY  
		BEGIN TRAN	

			--Capturamos valores.								
			SET @idRadiacdo = (SELECT CAST(Valor AS INT) FROM @UnionTable WHERE Id = 4);
			SET @fechaAsignacionBolsa = (SELECT Valor FROM @UnionTable WHERE Id = 8); 		
			SET @estado = (SELECT Valor FROM @UnionTable WHERE Id = 5);						
			--
			SET @IdMedicionOrigen = (SELECT Valor FROM @UnionTable WHERE Id = 1);
			SET @IdMedicionDestino = (SELECT Valor FROM @UnionTable WHERE Id = 3);

			--Validamos si el Auditor existe.
			SET @codigoAuditor = (SELECT UD.Codigo FROM AspnetUsers U INNER JOIN AspNetUsersDetelles UD ON (U.Id = UD.AspNetUsersId) WHERE UD.Codigo = (SELECT Valor FROM @UnionTable WHERE Id = 7)); --Codigo Auditor. AGREGAR VALIDACION DE SI EL USUARIO ESTA ACTIVO.
			
			--Validamos si el Id de registro (IdRadicado), esta vinculado con la Bolsa original.
			SET @perteneceBolsa = (SELECT COUNT(Id) FROM RegistrosAuditoria WHERE IdRadicado = (SELECT TOP(1) Valor FROM @UnionTable WHERE Id = 4) AND IdMedicion IN (@IdMedicionOrigen));			

			--Validamos si el Id de registro (IdRadicado), Existe.
			SET @idRadiacdoExiste = (SELECT COUNT(Id) FROM RegistrosAuditoria WHERE IdRadicado = (SELECT TOP(1) Valor FROM @UnionTable WHERE Id = 4));				

			--Validamos si el formato de fecha es correcto. (Debe ser 2022-03-08. Año-Mes-Dia)			
			DECLARE @ValidDate INT = 0;
			SET @ValidDate = ISDATE(@fechaAsignacionBolsa); --SELECT CASE WHEN ISDATE(@fechaAsignacionBolsa) = 1 --AND @fechaAsignacionBolsa LIKE '[1-2][0-9][0-9][0-9]-[0-1][0-9]-[0-3][0-9]' 			
			IF(@ValidDate = 0)  
			BEGIN
				SET @fechasBolsa  = 0;				
			END
			ELSE
			BEGIN
				--Validamos si la fecha de asignacion, esta dentro del rango de la Bolsa.			
				SET @fechasBolsa  = (SELECT COUNT(Id) FROM Medicion WHERE (CAST((SELECT Valor FROM @UnionTable WHERE Id = 8) AS DATE)) >= FechaInicioAuditoria AND (CAST((SELECT Valor FROM @UnionTable WHERE Id = 8) AS DATE)) <= FechaFinAuditoria); 
			END					

			--Capturamos/Validamos codigo auditor original y nuevo
			SET @IdAuditor = (SELECT U.Id FROM AspNetUsers U INNER JOIN AspNetUsersDetelles UD ON (U.Id = UD.AspNetUsersId) WHERE UD.Codigo = @codigoAuditor); -- Codigo Auditor


			--Validamos que el Registro pertenesca al Auditor original.
			SET @CodigoAuditorVinculado = (SELECT COUNT(*) FROM RegistrosAuditoria RA WHERE RA.IdAuditor = @IdAuditor AND RA.IdRadicado = @idRadiacdo);
			-- //



			--Validamos Errores.
			IF ( @codigoAuditor = '' OR @codigoAuditor IS NULL)
				BEGIN
					COMMIT TRAN	
					SET @MessageResult = 'ERROR, Radicado ' + CAST(@idRadiacdo AS nvarchar(255)) + ', No existe el Auditor original con Codigo ' + CAST((SELECT Valor FROM @UnionTable WHERE Id = 7) AS nvarchar(255))
					SELECT @idRadiacdo AS id, @MessageResult AS Result
				END
			ELSE IF ( @perteneceBolsa = 0)
				BEGIN
					COMMIT TRAN
					SET @MessageResult = 'ERROR, Radicado ' + CAST(@idRadiacdo AS nvarchar(255)) + ', El registro no pertenece a la bolsa con Id ' + CAST(@IdMedicionOrigen AS nvarchar(255))
					SELECT @idRadiacdo AS id, @MessageResult AS Result
				END 
			ELSE IF ( @idRadiacdoExiste = 0)
				BEGIN
					COMMIT TRAN
					SET @MessageResult = 'ERROR, Radicado ' + CAST(@idRadiacdo AS nvarchar(255)) + ', El registro con IdRadicado ' + CAST(@idRadiacdo AS nvarchar(255)) + ' no existe '
					SELECT @idRadiacdo AS id, @MessageResult AS Result
				END 
			ELSE IF ( @fechasBolsa = 0)
				BEGIN
					COMMIT TRAN
					SET @MessageResult = 'ERROR, Radicado ' + CAST(@idRadiacdo AS nvarchar(255)) + ', El formato de la fecha ' + @fechaAsignacionBolsa + ' no es correcto o no se encuentra dentro del periodo de auditoría de la bolsa (AAAA-MM-DD) ' + CAST(@IdMedicionOrigen AS nvarchar(255))
					SELECT @idRadiacdo AS id, @MessageResult AS Result
				END
			ELSE IF ( @CodigoAuditorVinculado = 0)
				BEGIN
					COMMIT TRAN
					SET @MessageResult = 'ERROR, Radicado ' + CAST(@idRadiacdo AS nvarchar(255)) + ', El registro con IdRadicado ' + CAST(@idRadiacdo AS nvarchar(255)) + ' no esta vinculado al Auditor original con Codigo ' + CAST((SELECT Valor FROM @UnionTable WHERE Id = 7) AS nvarchar(255))
					SELECT @idRadiacdo AS id, @MessageResult AS Result
				END
			ELSE
				BEGIN
						    
					-- Actualizamos valores. 
					UPDATE RegistrosAuditoria SET    					
					IdAuditor = @IdAuditor,  
					FechaAsignacion = @fechaAsignacionBolsa,
					IdMedicion = @IdMedicionDestino
					
					WHERE IdMedicion = @IdMedicionOrigen					
					-- //

					-- Regresamos valores y hacemos commit.
					set @MessageResult = 'OK, ' + CAST(@idRadiacdo AS nvarchar(255)) + ', Registro reasignado correctamente '

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
/****** Object:  StoredProcedure [dbo].[SP_MoverTodos_RegistrosAuditoria_BolsaMedicion]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-03-02
-- Description:	Para Mover todos los registrosAuditorias de una Bolsa a otra.
-- =============================================
CREATE PROCEDURE [dbo].[SP_MoverTodos_RegistrosAuditoria_BolsaMedicion]
(@MedicionIdOriginal INT, @MedicionIdDestino INT)
AS
BEGIN
SET NOCOUNT ON

--CODE
	-- VARIABLES
	DECLARE @MessageResult NVARCHAR(MAX) = ''
	DECLARE @MedicionFechaInicial DATE;
	DECLARE @MedicionFechaFinal DATE;
	DECLARE @ItemRAFechaAsignacion DATE;
	DECLARE @ItemRARadicado INT;
	DECLARE @BolsaOrigen VARCHAR(255) = '' ;
	DECLARE @EstadoRegistro VARCHAR(10) = '';
	DECLARE @Entidad VARCHAR(255) = '';
	DECLARE @CodigoAuditor VARCHAR(10) = '';

	DECLARE @BolsasId VARCHAR(255) = 'MedicionIdOriginal ' + CAST(@MedicionIdOriginal As VARCHAR(255)) + ', MedicionIdDestino ' + CAST(@MedicionIdDestino As VARCHAR(255));

	--Declaramos Tabla temporal para mostrar resultados del error.
	DECLARE @ResultadosTemp TABLE (Id int IDENTITY(1,1) PRIMARY KEY, IdBolsaOrigen INT, BolsaOrigen VARCHAR(MAX), IdBolsaDestino INT, IdRadicado INT, EstadoRegistro VARCHAR(MAX), Entidad VARCHAR(MAX), CodigoAuditor VARCHAR(MAX), 
	FechaAsignacion DATE, EstadoEjecucion VARCHAR(255), MensajeEjecucion VARCHAR(MAX))

	-- //
	BEGIN TRY  
		BEGIN TRAN				

			--Capturamos valores
			SET @MedicionFechaInicial = (SELECT CAST(ME.FechaInicioAuditoria AS date) As FechaInicioAuditoria FROM Medicion ME WHERE ME.Id = @MedicionIdDestino);
			SET @MedicionFechaFinal   = (SELECT CAST(ME.FechaFinAuditoria AS date) As FechaFinAuditoria FROM Medicion ME WHERE ME.Id = @MedicionIdDestino);			

			--Validamos Errores.

			--Cargamos RegistrosAuditoria de la medicion Anterior. 
			DECLARE @RegistrosAuditoriaTemp TABLE (Id int IDENTITY(1,1) PRIMARY KEY, RAId INT, RAIdRadicado INT, RAFechaAsignacion DATE, BolsaOrigen VARCHAR(MAX), EstadoRegistro VARCHAR(MAX), Entidad VARCHAR(MAX), CodigoAuditor VARCHAR(MAX))
			INSERT INTO  @RegistrosAuditoriaTemp
			SELECT RA.Id, RA.IdRadicado, RA.FechaAsignacion, ME.Nombre As BolsaOrigen, ERA.Codigo As EstadoRegistro, E.Nombre As Entidad, UD.Codigo AS CodigoAuditor
			FROM RegistrosAuditoria RA
			JOIN Medicion ME ON (RA.IdMedicion = ME.Id)
			JOIN EstadosRegistroAuditoria ERA ON (RA.Estado = ERA.Id)
			JOIN AspNetUsers US ON (US.Id = RA.IdAuditor)  
			LEFT JOIN AspNetUsersDetelles UD ON UD.AspNetUsersId = US.Id
			JOIN Eps E ON (E.EpsId = RA.IdEPS)
			WHERE RA.IdMedicion = @MedicionIdOriginal;
			--

			--Recorremos datos de tabla temporal.
			DECLARE @CountRAT INT = 1;
			DECLARE @MaxCountRAT INT = (SELECT COUNT(*) FROM @RegistrosAuditoriaTemp);

			WHILE @CountRAT <= @MaxCountRAT --Numero de ciclos Maximos
			BEGIN
				--Capturaramos valor.
				SET @ItemRAFechaAsignacion = (SELECT RAFechaAsignacion FROM @RegistrosAuditoriaTemp WHERE Id = @CountRAT)
				--SET @ItemRAFechaAsignacion = '2022-04-28';--2021-01-28 | GETDATE()
				SET @ItemRARadicado = (SELECT RAIdRadicado FROM @RegistrosAuditoriaTemp WHERE Id = @CountRAT);
				SET @BolsaOrigen = (SELECT BolsaOrigen FROM @RegistrosAuditoriaTemp WHERE Id = @CountRAT); 
				SET @EstadoRegistro = (SELECT EstadoRegistro FROM @RegistrosAuditoriaTemp WHERE Id = @CountRAT); 
				SET @Entidad = (SELECT Entidad FROM @RegistrosAuditoriaTemp WHERE Id = @CountRAT);
				SET @CodigoAuditor = (SELECT CodigoAuditor FROM @RegistrosAuditoriaTemp WHERE Id = @CountRAT);

				IF( (@ItemRAFechaAsignacion > @MedicionFechaInicial AND @ItemRAFechaAsignacion > @MedicionFechaFinal) OR (@ItemRAFechaAsignacion < @MedicionFechaInicial AND @ItemRAFechaAsignacion < @MedicionFechaFinal) )
				BEGIN
					--Creamos mensaje de error.
					SET @MessageResult = 'ERROR, Radicado ' + CAST(@ItemRARadicado AS nvarchar(255)) + ', La Fecha de asignación ' + CAST(@ItemRAFechaAsignacion AS nvarchar(255)) + ' no está dentro del rango inicio fin de la bolsa destino ' + CAST(@MedicionIdDestino AS nvarchar(255)) + ', Rango inicio fin de la bolsa ' + CAST(@MedicionFechaInicial AS nvarchar(255)) + ' - ' + CAST(@MedicionFechaFinal AS nvarchar(255)) + '.'
					
					--Insertamos resumen del error.
					INSERT INTO @ResultadosTemp 
					VALUES (@MedicionIdOriginal, @BolsaOrigen, @MedicionIdDestino, @ItemRARadicado, @EstadoRegistro, @Entidad, @CodigoAuditor, CAST(GETDATE() As DATE), 'ERROR', @MessageResult);

					--Limpiamos mensaje de error
					SET @MessageResult = '';
				END

				--Sumamos al contador del ciclo.
				SET @CountRAT = @CountRAT + 1
			END --END WHILE
			
			-- //
			
			--Ejecutamos, Si no encontramos errores.
			DECLARE @ValidCount INT = 0;
			SET @ValidCount = (SELECT COUNT(*) FROM @ResultadosTemp);
			IF(@ValidCount = 0 AND @MaxCountRAT > 0) 
			BEGIN
						    
				-- Actualizamos valores. 
				UPDATE RegistrosAuditoria SET    				
				IdMedicion = @MedicionIdDestino
				WHERE IdMedicion = @MedicionIdOriginal;					
				-- //
				
				--Creamos mensaje de error.
				set @MessageResult = 'OK, Los Registros de la Bolsa ' + CAST(@MedicionIdOriginal AS nvarchar(255)) + ' se han movido correctamente a la Bolsa ' + CAST(@MedicionIdDestino AS nvarchar(255))

				--Insertamos resumen del error.
				INSERT INTO @ResultadosTemp
				VALUES (@MedicionIdOriginal, @BolsaOrigen, @MedicionIdDestino, @ItemRARadicado, @EstadoRegistro, @Entidad, @CodigoAuditor, CAST(GETDATE() As DATE), 'OK', @MessageResult);

				-- Regresamos valores y hacemos commit.
				COMMIT TRAN
				SELECT Id, IdBolsaOrigen, BolsaOrigen, IdBolsaDestino, IdRadicado, EstadoRegistro, Entidad, CodigoAuditor, FechaAsignacion, EstadoEjecucion, MensajeEjecucion FROM @ResultadosTemp
			END --END IF
			ELSE
			BEGIN
				-- Regresamos valores y hacemos commit.
				COMMIT TRAN
				SELECT Id, IdBolsaOrigen, BolsaOrigen, IdBolsaDestino, IdRadicado, EstadoRegistro, Entidad, CodigoAuditor, FechaAsignacion, EstadoEjecucion, MensajeEjecucion FROM @ResultadosTemp
			END
	END TRY 
		
	BEGIN CATCH  
		ROLLBACK TRAN

		--Creamos mensaje de error.
		SET @MessageResult = 'ERROR, MedicionIdOriginal ' + CAST(@MedicionIdOriginal AS nvarchar(255)) + ', MedicionIdDestino ' + CAST(@MedicionIdDestino AS nvarchar(255)) + ', ' + ERROR_MESSAGE() + '- SP LINEA: ' + CAST(ERROR_LINE() AS nvarchar(255));
		
		--Insertamos resumen del error.
		INSERT INTO @ResultadosTemp
		VALUES (@MedicionIdOriginal, @BolsaOrigen, @MedicionIdDestino, @ItemRARadicado, @EstadoRegistro, @Entidad, @CodigoAuditor, CAST(GETDATE() As DATE), 'ERROR', @MessageResult);

		-- Regresamos valores.
		SELECT Id, IdBolsaOrigen, BolsaOrigen, IdBolsaDestino, IdRadicado, EstadoRegistro, Entidad, CodigoAuditor, FechaAsignacion, EstadoEjecucion, MensajeEjecucion FROM @ResultadosTemp
	END CATCH
END --END SP
GO
/****** Object:  StoredProcedure [dbo].[SP_MoverTodos_RegistrosAuditoria_BolsaMedicion_Plantilla]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-03-09
-- Description:	Para consultar info usada en Mover algunos registrosAuditorias de una Bolsa a otra.
-- =============================================
CREATE PROCEDURE [dbo].[SP_MoverTodos_RegistrosAuditoria_BolsaMedicion_Plantilla]
(@MedicionIdOriginal VARCHAR(MAX))
AS
BEGIN
SET NOCOUNT ON
	
	--Guardamos Query inicial.  
	DECLARE @Query VARCHAR(MAX); 

	--Consultamos datos.
	SET @Query = 'SELECT 1 As Id, ME.Id As IdBolsaOrigen, ME.Nombre As BolsaOrigen, 0 As IdBolsaDestino, RA.IdRadicado As IdRadicado, ERA.Codigo As EstadoRegistro, E.Nombre As Entidad, UD.Codigo AS CodigoAuditor, RA.FechaAsignacion As FechaAsignacion, '' '' As EstadoEjecucion, '' '' As MensajeEjecucion
	FROM RegistrosAuditoria RA
	JOIN Medicion ME ON (RA.IdMedicion = ME.Id)
	JOIN EstadosRegistroAuditoria ERA ON (RA.Estado = ERA.Id)
	JOIN AspNetUsers US ON (US.Id = RA.IdAuditor)  
	LEFT JOIN AspNetUsersDetelles UD ON UD.AspNetUsersId = US.Id
	JOIN Eps E ON (E.EpsId = RA.IdEPS) '

	--Guardamos Condiciones.  
	DECLARE @Where VARCHAR(MAX) = '';  
	IF(@MedicionIdOriginal <> '0')  
	BEGIN   
	 IF(@Where = '')  
	 BEGIN   
	  SET @Where = @Where + 'RA.IdMedicion IN (' + CAST(@MedicionIdOriginal AS NVARCHAR(MAX)) + ')'  
	 END   
		ElSE  
	 BEGIN   
	  SET @Where = @Where + ' AND RA.IdMedicion IN (' + CAST(@MedicionIdOriginal AS NVARCHAR(MAX)) + ')'  
	 END   
	END   
	--  //

	--Concatenamos Query, Condiciones y Paginado. Luego ejecutamos.  
	IF(@Where <> '' )  
	BEGIN   
	 SET @Where = ' WHERE ' + @Where;                
	END    
	DECLARE @Total VARCHAR(MAX);  
	SET @Total = @Query + '' + @Where
  

	--Imprimimos/Ejecutamos.  
	EXEC(@Total);  
	--PRINT(@Total);  

END --END SP
GO
/****** Object:  StoredProcedure [dbo].[SP_Realiza_Cargue_Poblacion]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-01-23
-- Description:	Recibe registro uno a uno del archivo plano para realizar el cargue de poblacion
-- =============================================
CREATE PROCEDURE [dbo].[SP_Realiza_Cargue_Poblacion]
	
	@header NVARCHAR(MAX), 
	@line NVARCHAR(MAX),
	@IdMedicion INT,
	@User NVARCHAR(MAX),
	@CurrentProcessId INT,
    @ResultCurrentProcess NVARCHAR(255),
	@Progreso INT

	AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   -- VARIABLES
	DECLARE  @MessageResult NVARCHAR(MAX) = 'OK'
	DECLARE  @idRadiacdo INT
	DECLARE  @Identificacion NVARCHAR(255)
	DECLARE  @Separador NVARCHAR(255) = '['

	--IF CHARINDEX(';',@header) > 0
	--BEGIN
	--	SET @Separador = ';'
	--END 

	-- Construye tabla cabecera
	DECLARE @HeaderTable TABLE (Id int IDENTITY(1,1) PRIMARY KEY, Header NVARCHAR(MAX))
	INSERT @HeaderTable SELECT value FROM string_split(@header, @Separador)


	-- Construye tabla linea
	DECLARE @LineTable TABLE (Id int IDENTITY(1,1) PRIMARY KEY, Valor NVARCHAR(MAX))
	INSERT @LineTable SELECT value FROM string_split(@line, @Separador)


	-- Construye tabla y consulta para unir nemonico con cabecera y value del registro del archivo
	DECLARE @UnionTable TABLE (Id INT, IdVariable INT, header NVARCHAR(MAX), Valor NVARCHAR(MAX), Nemonico NVARCHAR(MAX))
		INSERT INTO  @UnionTable
			SELECT  h.Id, vr.Id, h.Header, l.Valor, vr.nemonico FROM @HeaderTable h 
					INNER JOIN @LineTable l ON h.Id = l.Id
					LEFT JOIN Variables vr ON h.Header = vr.nemonico
					LEFT JOIN VariableXMedicion vm ON vr.idVariable = vr.Id

	-- Construye tabla y consulta para unir nemonico con cabecera y value del registro del archivo
	DECLARE @UnionTableFiltrada TABLE (Id int IDENTITY(1,1) PRIMARY KEY, IdVariable INT, header NVARCHAR(MAX), Valor NVARCHAR(MAX), Nemonico NVARCHAR(MAX), EsGlosa BIT, Visible BIT, Calificable BIT, SubGrupoId INT, CalificacionDefecto INT NULL, Idmedicion INT )
		INSERT INTO  @UnionTableFiltrada
			SELECT  vr.Id, h.Header, l.Valor, vr.nemonico, vm.EsGlosa, vm.EsVisible, vm.EsCalificable, vm.SubGrupoId, vm.CalificacionXDefecto, vm.MedicionId FROM @HeaderTable h 
					INNER JOIN @LineTable l ON h.Id = l.Id
					INNER JOIN Variables vr ON h.Header = vr.nemonico
					INNER JOIN VariableXMedicion vm ON vr.Id = vm.VariableId    AND vm.MedicionId = @IdMedicion
			WHERE vr.idVariable IS NOT NULL  AND vm.MedicionId = @IdMedicion

			BEGIN TRY  
			  BEGIN TRAN	

					-- Consulta Info medicion	
					DECLARE @fechamin DATE
					DECLARE @fechamax DATE

					SELECT
					 @fechamin = FechaInicioAuditoria,
					 @fechamax = FechaFinAuditoria 
					FROM Medicion WHERE Id = @IdMedicion
					
					
					SET @idRadiacdo = (select CAST(Valor AS INT) FROM @UnionTable where RTRIM(LTRIM(header)) = 'idHEMOFILIA')
					SET @Identificacion = (select CAST(Valor AS INT) FROM @UnionTable WHERE RTRIM(LTRIM(header)) = 'Identificacion')

						IF ( (SELECT COUNT(*) FROM [RegistrosAuditoria] WHERE IdRadicado = @idRadiacdo) > 0)
						BEGIN
								 COMMIT TRAN	
								 SELECT @idRadiacdo AS id, 'ERROR' + @Separador + ' Ya se encuentra agregado el ID de Radicado ' + CAST(@idRadiacdo AS nvarchar(255))   AS Result

						END
						--ELSE IF ( (SELECT COUNT(*) FROM [RegistrosAuditoria] WHERE Identificacion = @Identificacion and IdMedicion = @IdMedicion) > 0)
						--BEGIN
						--		COMMIT TRAN
						--		SELECT @idRadiacdo AS id, 'ERROR' + @Separador + ' Ya se encuentrael paciente con documento ' + @Identificacion + ' a la medicion ' + CAST(@IdMedicion AS nvarchar(255)) AS Result

						--END 
						ELSE IF ( (SELECT COUNT(*) FROM @UnionTableFiltrada WHERE  Idmedicion = @IdMedicion) = 0)
						BEGIN
								COMMIT TRAN
								SELECT @idRadiacdo AS id, 'ERROR' + @Separador + ' No se encuentra parametrizacion de las variables para la medicion ' + CAST(@IdMedicion AS nvarchar(255)) + ' con un nemonico valido ' AS Result

						END
						ELSE IF ( (select COUNT(*)  from AspNetUsersDetelles WHERE CAST(Codigo AS int) = CAST((select Valor FROM @UnionTable WHERE RTRIM(LTRIM(header)) = 'IdAuditor') AS INT)) = 0)
						BEGIN
								COMMIT TRAN
								SELECT @idRadiacdo AS id, 'ERROR' + @Separador + ' El codigo de auditor no es valido' AS Result

						END
					ELSE
						BEGIN
						    
							-- INSERTA REGISTRO AUDITORIA 
							INSERT INTO [dbo].[RegistrosAuditoria]
									   ([IdRadicado],[IdMedicion],[IdPeriodo],[IdAuditor],[PrimerNombre],[SegundoNombre],[PrimerApellido],[SegundoApellido],[Sexo],[TipoIdentificacion],[Identificacion],[FechaNacimiento],[FechaCreacion],[FechaAuditoria],[FechaMinConsultaAudit]
									   ,[FechaMaxConsultaAudit],[FechaAsignacion],[Activo],[Conclusion],[UrlSoportes],[Reverse],[DisplayOrder],[Ara],[Eps],[FechaReverso],[AraAtendido],[EpsAtendido],[Revisar],[Extemporaneo],[Estado]
									   ,[LevantarGlosa],[MantenerCalificacion],[ComiteExperto],[ComiteAdministrativo],[AccionLider],[AccionAuditor],[Encuesta],[CreatedBy],[CreatedDate],[ModifyBy],[ModifyDate],[IdEPS],[Status])
								 VALUES
									   ( (select CAST(Valor AS INT) FROM @UnionTable where RTRIM(LTRIM(header)) = 'idHEMOFILIA')
									   , @IdMedicion
									   ,(select CAST(Valor AS INT) FROM @UnionTable where RTRIM(LTRIM(header)) = 'idPeriodo')
									   , (select TOP(1) AspNetUsersId from AspNetUsersDetelles WHERE CAST(Codigo AS int) = CAST((select Valor FROM @UnionTable WHERE RTRIM(LTRIM(header)) = 'IdAuditor') AS INT))
									   ,(select CAST(Valor AS VARCHAR(50)) FROM @UnionTable WHERE RTRIM(LTRIM(header)) = 'PrimerNombre')
									   ,(select CAST(Valor AS VARCHAR(50)) FROM @UnionTable WHERE RTRIM(LTRIM(header)) = 'SegundoNombre')
									   ,(select CAST(Valor AS VARCHAR(50)) FROM @UnionTable WHERE RTRIM(LTRIM(header)) = 'PrimerApellido')
									   ,(select CAST(Valor AS VARCHAR(50)) FROM @UnionTable WHERE RTRIM(LTRIM(header)) = 'SegundoApellido')
									   ,(select CAST(Valor AS VARCHAR(50)) FROM @UnionTable WHERE RTRIM(LTRIM(header)) = 'Sexo')
									   ,(select CAST(Valor AS VARCHAR(2)) FROM @UnionTable WHERE RTRIM(LTRIM(header)) = 'TipoIdentificacion')
									   ,(select CAST(Valor AS INT) FROM @UnionTable WHERE RTRIM(LTRIM(header)) = 'Identificacion')
									   ,(select CAST(Valor AS DATETIME) FROM @UnionTable WHERE RTRIM(LTRIM(header)) = 'FechaNacimiento')
									   ,GETDATE()
									   ,GETDATE() -- (select CAST(Valor AS DATETIME) FROM @UnionTable WHERE RTRIM(LTRIM(header)) = 'FechaAuditoria')
									   ,@fechamin --  (select CAST(Valor AS DATETIME) FROM @UnionTable WHERE RTRIM(LTRIM(header)) = 'FechaMinConsultaAudit')
									   ,@fechamax --  (select CAST(Valor AS DATETIME) FROM @UnionTable WHERE RTRIM(LTRIM(header)) = 'FechaMaxConsultaAudit') 
									   ,(select CAST(Valor AS DATETIME) FROM @UnionTable WHERE RTRIM(LTRIM(header)) = 'FechaAsignacion') 
									   ,1
									   ,1 -- ToDo Validar
									   ,''
									   ,0
									   ,(select CAST(Valor AS INT) FROM @UnionTable WHERE RTRIM(LTRIM(header)) = 'Orden') 
									   ,0 -- ToDo Validar
									   ,0 -- ToDo Validar
									   ,GETDATE() -- ToDo Validar
									   ,0
									   ,0 -- ToDo Validar
									   ,0 -- ToDo Validar
									   ,0 -- ToDo Validar
									   ,1 --RN
									   ,0
									   ,0
									   ,0
									   ,0
									   ,0
									   ,0
									   ,(select CAST(Valor AS INT) FROM @UnionTable WHERE RTRIM(LTRIM(header)) = 'Encuesta') 
									   ,@User
									   ,GETDATE()
									   ,@User
									   ,GETDATE()
									   ,(select CAST(Valor AS VARCHAR(30)) FROM @UnionTable WHERE RTRIM(LTRIM(header)) = 'idEPS') 
									   ,1)


									   DECLARE @LastIdRegistroAuditoria INT =  (SELECT SCOPE_IDENTITY());
						  
									   --INSERTAR DETALLE REGISTRO AUDITORIA DETALLE
									   DECLARE @position INT = 1;
									   DECLARE @total INT = (SELECT TOP(1) Id FROM @UnionTableFiltrada ORDER BY Id DESC);

									   WHILE(@position <= @total)
									   BEGIN
											INSERT INTO [dbo].[RegistrosAuditoriaDetalle]
													   ([VariableId],[RegistrosAuditoriaId],[EstadoVariableId],[Observacion],[Activo],[CreatedBy],[CreatedDate],[ModifyBy],[ModifyDate],[EsGlosa],[Visible]
													   ,[Calificable],[SubgrupoId],[DatoReportado],[MotivoVariable],[Dato_DC_NC_ND],[Ara],[Status])
												SELECT 
														IdVariable,
														@LastIdRegistroAuditoria,
														1, -- ToDo Validar
														0, -- ToDo Validar
														1,
														1, -- ToDo Validar
														GETDATE(),
														NULL,
														NULL,
														EsGlosa,
														Visible,
														Calificable,
														SubGrupoId,
														Valor,
														NULL,
														CalificacionDefecto,
														0,
														1
									
												FROM  @UnionTableFiltrada WHERE Id = @position
						
												SET @position = @position + 1

									   END

									   --Actualiza porceso actual
									   UPDATE Current_Process SET Result = @ResultCurrentProcess, Progress = @Progreso WHERE Id = @CurrentProcessId 

									   set @MessageResult = 'OK, ' +(select Valor FROM @UnionTable where RTRIM(LTRIM(header)) = 'idHEMOFILIA') + ', Registro agregado correctamente'

									   COMMIT TRAN
									   SELECT (select CAST(Valor AS INT) FROM @UnionTable where RTRIM(LTRIM(header)) = 'idHEMOFILIA') AS id, @MessageResult AS Result
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
				set @MessageResult = 'ERROR' + @Separador + ' ' +(select Valor FROM @UnionTable where RTRIM(LTRIM(header)) = 'idHEMOFILIA') + ', ' + ERROR_MESSAGE() + '- SP LINEA: ' + CAST(ERROR_LINE() AS nvarchar(255));
				SELECT (select CAST(Valor AS INT) FROM @UnionTable where RTRIM(LTRIM(header)) = 'idHEMOFILIA')  AS id, @MessageResult AS Result
			END CATCH




	END
GO
/****** Object:  StoredProcedure [dbo].[SP_Reasignaciones_Bolsa]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-02-24
-- Description:	Para cargar listado de Registros Auditoria en vista de Reasignaciones de bolsa.
-- =============================================
--DROP PROCEDURE SP_Reasignaciones_Bolsa 
CREATE PROCEDURE [dbo].[SP_Reasignaciones_Bolsa]
(@Reasignaciones_Bolsa DT_Reasignaciones_Bolsa READONLY)
AS
BEGIN
SET NOCOUNT ON	

BEGIN TRANSACTION [Tran1]

	BEGIN TRY

		UPDATE RegistrosAuditoria SET    
		IdAuditor = RB.AuditorId,  
		FechaAsignacion = RB.FechaAsignacion
	
		FROM @Reasignaciones_Bolsa RB  
		WHERE RegistrosAuditoria.IdRadicado = RB.IdRadicado

		COMMIT TRANSACTION [Tran1]
	END TRY

	BEGIN CATCH

		ROLLBACK TRANSACTION [Tran1]
		--PRINT('FAIL');
		--SELECT ERROR_NUMBER() AS ErrorNumber, ERROR_STATE() AS ErrorState, ERROR_SEVERITY() AS ErrorSeverity, ERROR_PROCEDURE() AS ErrorProcedure, ERROR_LINE() AS ErrorLine, ERROR_MESSAGE() AS ErrorMessage;
		INSERT INTO TBL_TX_LOG(Date, Thread, Level, Logger, Message, Exception)
		VALUES(GETDATE(), '1', 'ERROR CATCH', 'Fallo EN SP SP_Reasignaciones_Bolsa', ERROR_MESSAGE(), CONCAT('ErrorNumber: ',ERROR_NUMBER(),' - ', 'ERROR_STATE: ',ERROR_STATE(),' - ', 'ERROR_SEVERITY: ',ERROR_SEVERITY(),' - ', 'ERROR_PROCEDURE: ',ERROR_PROCEDURE(),' - ', 'ERROR_LINE: ',ERROR_LINE(),' - ', 'ERROR_MESSAGE: ',ERROR_MESSAGE()) );
	END CATCH 
END --END SP
GO
/****** Object:  StoredProcedure [dbo].[SP_Reasignaciones_Bolsa_Detallada]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-02-28
-- Description:	Para cargar listado de Registros Auditoria en vista de Reasignaciones de bolsa detallada.
-- =============================================
CREATE PROCEDURE [dbo].[SP_Reasignaciones_Bolsa_Detallada]
	
	@header NVARCHAR(MAX), @line NVARCHAR(MAX), @IdMedicion NVARCHAR(MAX)
	
	AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   -- VARIABLES
	DECLARE  @MessageResult NVARCHAR(MAX) = 'OK'
	DECLARE  @idRadiacdo INT	
	DECLARE  @Separador NVARCHAR(255) = ','
	--
	DECLARE @codigoAuditorOriginal NVARCHAR(255) = '';
	DECLARE @codigoAuditorNuevo NVARCHAR(255) = '';
	DECLARE @perteneceBolsa INT = 0;
	DECLARE @fechasBolsa INT = 0;
	DECLARE @idRadiacdoExiste INT = 0;
	DECLARE @CodigoAuditorVinculado INT = 0;
	--
	DECLARE @fechaAsignacionBolsa NVARCHAR(255) = '';
	DECLARE @IdAuditorOriginal NVARCHAR(255) = '';
	DECLARE @IdAuditorNuevo NVARCHAR(255) = '';
	DECLARE @estado NVARCHAR(5) = 'RN';

	IF CHARINDEX(';',@header) > 0 --CHAR(9). Para tab character. | ORIGINAL: IF CHARINDEX(';',@header) > 0 | ACTUAl: IF CHARINDEX(CHAR(9),@header) > 0
	BEGIN
		SET @Separador = ';' --CHAR(9). Para tab character. | ORIGINAL: SET @Separador = ';' | ACTUAl: SET @Separador = CHAR(9)
	END

	-- Construye tabla cabecera
	DECLARE @HeaderTable TABLE (Id int IDENTITY(1,1) PRIMARY KEY, Header NVARCHAR(MAX))
	INSERT @HeaderTable SELECT value FROM string_split(@header, @Separador)

	-- Construye tabla linea
	DECLARE @LineTable TABLE (Id int IDENTITY(1,1) PRIMARY KEY, Valor NVARCHAR(MAX))
	INSERT @LineTable SELECT value FROM string_split(@line, @Separador)
	
	--Unimos tabla Header y tabla Line.
	DECLARE @UnionTable TABLE (Id INT, header NVARCHAR(MAX), Valor NVARCHAR(MAX))
	INSERT INTO  @UnionTable SELECT H.Id, H.Header, L.Valor FROM @HeaderTable H INNER JOIN @LineTable L ON (H.Id = L.Id);

	-- //
	BEGIN TRY  
		BEGIN TRAN	

			--Capturamos valores.								
			SET @idRadiacdo = (SELECT CAST(Valor AS INT) FROM @UnionTable WHERE Id = 2);
			SET @fechaAsignacionBolsa = (SELECT Valor FROM @UnionTable WHERE Id = 5); 
			--SET @fechaAsignacionBolsa = CAST((SELECT Valor FROM @UnionTable WHERE Id = 5) AS DATE);
			SET @estado = (SELECT Valor FROM @UnionTable WHERE Id = 3);						

			--Validamos si el Auditor existe.
			SET @codigoAuditorOriginal = (SELECT UD.Codigo FROM AspnetUsers U INNER JOIN AspNetUsersDetelles UD ON (U.Id = UD.AspNetUsersId) WHERE UD.Codigo = (SELECT Valor FROM @UnionTable WHERE Id = 1) ); --Codigo Auditor original			
			SET @codigoAuditorNuevo    = (SELECT UD.Codigo FROM AspnetUsers U INNER JOIN AspNetUsersDetelles UD ON (U.Id = UD.AspNetUsersId) WHERE UD.Codigo = (SELECT Valor FROM @UnionTable WHERE Id = 6) ); --Codigo Auditor nuevo			
			
			--Validamos si el Id de registro (IdRadicado), esta vinculado con la Bolsa actual.
			SET @perteneceBolsa = (SELECT COUNT(Id) FROM RegistrosAuditoria WHERE IdRadicado = (SELECT TOP(1) Valor FROM @UnionTable WHERE Id = 2) AND IdMedicion IN (@IdMedicion));
			IF(@IdMedicion = '0' OR @IdMedicion = '')
			BEGIN
				SET @perteneceBolsa = 1;
			END

			--Validamos si el Id de registro (IdRadicado), Existe.
			SET @idRadiacdoExiste = (SELECT COUNT(Id) FROM RegistrosAuditoria WHERE IdRadicado = (SELECT TOP(1) Valor FROM @UnionTable WHERE Id = 2));				

			--Validamos si el formato de fecha es correcto. (Debe ser 2022-03-08. Año-Mes-Dia)			
			DECLARE @ValidDate INT = 0;
			SET @ValidDate = ISDATE(@fechaAsignacionBolsa); --SELECT CASE WHEN ISDATE(@fechaAsignacionBolsa) = 1 --AND @fechaAsignacionBolsa LIKE '[1-2][0-9][0-9][0-9]-[0-1][0-9]-[0-3][0-9]' 			
			IF(@ValidDate = 0)  
			BEGIN
				SET @fechasBolsa  = 0;				
			END
			ELSE
			BEGIN
				--Validamos si la fecha de asignacion, esta dentro del rango de la Bolsa.			
				SET @fechasBolsa  = (SELECT COUNT(Id) FROM Medicion WHERE (CAST((SELECT Valor FROM @UnionTable WHERE Id = 5) AS DATE)) >= FechaInicioAuditoria AND (CAST((SELECT Valor FROM @UnionTable WHERE Id = 5) AS DATE)) <= FechaFinAuditoria); 
			END					

			--Capturamos/Validamos codigo auditor original y nuevo
			SET @IdAuditorOriginal = (SELECT U.Id FROM AspNetUsers U INNER JOIN AspNetUsersDetelles UD ON (U.Id = UD.AspNetUsersId) WHERE UD.Codigo = @codigoAuditorOriginal); -- Codigo Auditor original
			SET @IdAuditorNuevo    = (SELECT U.Id FROM AspNetUsers U INNER JOIN AspNetUsersDetelles UD ON (U.Id = UD.AspNetUsersId) WHERE UD.Codigo = @codigoAuditorNuevo);    -- Codigo Auditor nuevo

			--Validamos que el Registro pertenesca al Auditor original.
			SET @CodigoAuditorVinculado = (SELECT COUNT(*) FROM RegistrosAuditoria RA WHERE RA.IdAuditor = @IdAuditorOriginal AND RA.IdRadicado = @idRadiacdo);

			--Validamos Errores.
			IF ( @codigoAuditorOriginal = '' OR @codigoAuditorOriginal IS NULL)
				BEGIN
					COMMIT TRAN	
					SET @MessageResult = 'ERROR, Radicado ' + CAST(@idRadiacdo AS nvarchar(255)) + ', No existe el Auditor original con Codigo ' + CAST((SELECT Valor FROM @UnionTable WHERE Id = 1) AS nvarchar(255))
					SELECT @idRadiacdo AS id, @MessageResult AS Result
				END
			ELSE IF ( @codigoAuditorNuevo = '' OR @codigoAuditorNuevo IS NULL)
				BEGIN
					COMMIT TRAN	
					SET @MessageResult = 'ERROR, Radicado ' + CAST(@idRadiacdo AS nvarchar(255)) + ', No existe el Auditor nuevo con Codigo ' + CAST((SELECT Valor FROM @UnionTable WHERE Id = 6) AS nvarchar(255))
					SELECT @idRadiacdo AS id, @MessageResult AS Result
				END
			ELSE IF ( @perteneceBolsa = 0)
				BEGIN
					COMMIT TRAN
					SET @MessageResult = 'ERROR, Radicado ' + CAST(@idRadiacdo AS nvarchar(255)) + ', El registro no pertenece a la bolsa con Id ' + CAST(@IdMedicion AS nvarchar(255))
					SELECT @idRadiacdo AS id, @MessageResult AS Result
				END 
			ELSE IF ( @idRadiacdoExiste = 0)
				BEGIN
					COMMIT TRAN
					SET @MessageResult = 'ERROR, Radicado ' + CAST(@idRadiacdo AS nvarchar(255)) + ', El registro con IdRadicado ' + CAST(@idRadiacdo AS nvarchar(255)) + ' no existe '
					SELECT @idRadiacdo AS id, @MessageResult AS Result
				END 
			ELSE IF ( @fechasBolsa = 0)
				BEGIN
					COMMIT TRAN
					SET @MessageResult = 'ERROR, Radicado ' + CAST(@idRadiacdo AS nvarchar(255)) + ', El formato de la fecha ' + @fechaAsignacionBolsa + ' no es correcto o no se encuentra dentro del periodo de auditoría de la bolsa (AAAA-MM-DD) ' + CAST(@IdMedicion AS nvarchar(255))
					SELECT @idRadiacdo AS id, @MessageResult AS Result
				END
			ELSE IF ( @CodigoAuditorVinculado = 0)
				BEGIN
					COMMIT TRAN
					SET @MessageResult = 'ERROR, Radicado ' + CAST(@idRadiacdo AS nvarchar(255)) + ', El registro con IdRadicado ' + CAST(@idRadiacdo AS nvarchar(255)) + ' no esta vinculado al Auditor original con Codigo ' + CAST((SELECT Valor FROM @UnionTable WHERE Id = 1) AS nvarchar(255))
					SELECT @idRadiacdo AS id, @MessageResult AS Result
				END
			ELSE
				BEGIN
						    
					-- Actualizamos valores. 
					UPDATE RegistrosAuditoria SET    
					IdAuditor = @IdAuditorNuevo,  
					FechaAsignacion = @fechaAsignacionBolsa
					--Estado = @estado
					WHERE IdRadicado = @idRadiacdo					
					-- //

					-- Regresamos valores y hacemos commit.
					set @MessageResult = 'OK, ' + CAST(@idRadiacdo AS nvarchar(255)) + ', Registro reasignado correctamente '

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
/****** Object:  StoredProcedure [dbo].[SP_RegistrosAuditoria_XBolsaMedicion]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-02-23
-- Description:	Para cargar listado de Registros Auditoria en vista de Reasignaciones de bolsa.
-- =============================================
CREATE PROCEDURE [dbo].[SP_RegistrosAuditoria_XBolsaMedicion]
(@PageNumber INT, @MaxRows INT, @IdRadicado VARCHAR(MAX), @AuditorId VARCHAR(MAX), @MedicionId VARCHAR(MAX), @FechaAsignacionIni VARCHAR(MAX), @FechaAsignacionFin VARCHAR(MAX), @Estado VARCHAR(MAX), @CodigoEps VARCHAR(MAX), @Finalizados BIT, @KeyWord VARCHAR(MAX))
AS
BEGIN
SET NOCOUNT ON

--Consultamos IPS.
DECLARE @Ips NVARCHAR(255) = (SELECT TOP (1) Valor  FROM [dbo].[ParametrosGenerales] where Nombre = 'CampoReferencialIps')

--Guardamos Query inicial.  
DECLARE @Query VARCHAR(MAX);  
SET @Query = 'SELECT '' '' As QueryNoRegistrosTotales, RA.IdAuditor, US.UserName as NombreAuditor, RA.IdRadicado, RA.Estado, ERA.Codigo As EstadoCodigo, ERA.Nombre As EstadoNombre, RA.FechaAsignacion, UD.Codigo AS CodigoUsuario, RA.IdMedicion, ME.Nombre As NombreMedicion, RA.IdEPS As Data_IdEPS, E.Nombre As Data_NombreEPS, 
(SELECT TOP(1)  ItemDescripcion FROM CatalogoItemCobertura WHERE ItemId =  (SELECT TOP (1) rad.DatoReportado FROM RegistrosAuditoriaDetalle rad INNER JOIN Variables va ON va.Id = rad.VariableId WHERE va.campoReferencial = ''' + @Ips + ''' AND rad.RegistrosAuditoriaId = RA.Id)) AS IPS,
EF.idCobertura As IdEnfermedadMadre, EF.nombre As NombreEnfermedadMadre 
FROM RegistrosAuditoria RA   
JOIN Medicion ME ON (RA.IdMedicion = ME.Id)   
JOIN EstadosRegistroAuditoria ERA ON (RA.Estado = ERA.Id)   
JOIN AspNetUsers US ON (US.Id = RA.IdAuditor)  
LEFT JOIN AspNetUsersDetelles UD ON UD.AspNetUsersId = US.Id
JOIN AspNetUserRoles UR ON (UR.UserId = RA.IdAuditor)  
JOIN EstadoRegistroAuditoriaIcono RIC ON (RIC.Estado = RA.Estado AND RIC.RolId = UR.RoleId) AND RA.Revisar = RIC.Revisar 
JOIN Enfermedad EF ON EF.idCobertura = ME.IdCobertura
JOIN Eps E ON (E.EpsId = RA.IdEPS) ';


--Calculo de paginado.  
--SET @PageNumber = @PageNumber - 1;  
DECLARE @Paginate INT = @PageNumber * @MaxRows;  


--Guardamos Condiciones.  
DECLARE @Where VARCHAR(MAX) = '';  
--  
IF(@IdRadicado <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'RA.IdRadicado IN (' + CAST(@IdRadicado AS NVARCHAR(MAX)) + ')'  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND RA.IdRadicado IN (' + CAST(@IdRadicado AS NVARCHAR(MAX)) + ')'  
 END   
END   
--  
IF(@AuditorId <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'RA.IdAuditor IN (''' + CAST(@AuditorId AS NVARCHAR(MAX)) + ''')'  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND RA.IdAuditor IN (''' + CAST(@AuditorId AS NVARCHAR(MAX)) + ''')'  
 END   
END   
--
IF(@MedicionId <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'RA.IdMedicion IN (' + CAST(@MedicionId AS NVARCHAR(MAX)) + ')'  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND RA.IdMedicion IN (' + CAST(@MedicionId AS NVARCHAR(MAX)) + ')'  
 END   
END   
--  
IF(@FechaAsignacionIni <> '' OR @FechaAsignacionFin <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'FechaAsignacion >= ''' + CAST(@FechaAsignacionIni AS NVARCHAR(MAX)) + ''' AND FechaAsignacion <= ''' + CAST(@FechaAsignacionFin AS NVARCHAR(MAX)) + ''''  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND FechaAsignacion >= ''' + CAST(@FechaAsignacionIni AS NVARCHAR(MAX)) + ''' AND FechaAsignacion <= ''' + CAST(@FechaAsignacionFin AS NVARCHAR(MAX)) + ''''  
 END   
END   
--ElSE  
-- BEGIN   
--  IF(@Where = '')  
--  BEGIN   
--   SET @Where = @Where + 'FechaAsignacion >= ''' + CAST(GETDATE() AS NVARCHAR(MAX)) + ''' AND FechaAsignacion <= ''' + CAST(GETDATE() AS NVARCHAR(MAX)) + ''''  
--  END   
--  ElSE  
--  BEGIN   
--   SET @Where = @Where + ' AND FechaAsignacion >= ''' + CAST(GETDATE() AS NVARCHAR(MAX)) + ''' AND FechaAsignacion <= ''' + CAST(GETDATE() AS NVARCHAR(MAX)) + ''''  
--  END     
-- END   
--  
IF(@Estado <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'RA.Estado IN (' + CAST(@Estado AS NVARCHAR(MAX)) + ')'  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND RA.Estado IN (' + CAST(@Estado AS NVARCHAR(MAX)) + ')'  
 END   
END   
--  
IF(@CodigoEps <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
 --str, old_str. new_str   
  SET @Where = @Where + 'RA.IdEPS IN (''' + REPLACE(CAST(@CodigoEps AS NVARCHAR(MAX)), ',', ''',''') + ''')'  
  --SET @Where = @Where + 'RA.IdEPS = ''' + CAST(@CodigoEps AS NVARCHAR(MAX)) + '''' 
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND RA.IdEPS IN (''' + REPLACE(CAST(@CodigoEps AS NVARCHAR(MAX)), ',', ''',''') + ''')'  
  --SET @Where = @Where + ' AND RA.IdEPS = ''' + CAST(@CodigoEps AS NVARCHAR(MAX)) + '''' 
 END   
END   
--  
--IF(@Finalizados = 1)  
--BEGIN   
-- DECLARE @FechaActual Date = GETDATE(); -- '2022-04-21 11:34:59.623' | GETDATE()
-- IF(@Where = '')  
-- BEGIN   
--	SET @Where = @Where  + ' ''' + CAST(@FechaActual AS NVARCHAR(MAX)) + '''< ME.FechaFinAuditoria '
-- END   
--    ElSE  
-- BEGIN   
--	SET @Where = @Where  + ' AND ''' + CAST(@FechaActual AS NVARCHAR(MAX)) + '''< ME.FechaFinAuditoria '
-- END   
--END   
---

-- //
--Definimos WHERE de Palabra Clave.
DECLARE @WhereKeyWord VARCHAR(MAX) = '';  
IF(@KeyWord <> '')
BEGIN 
	IF(ISNUMERIC(@KeyWord) = 1)
	BEGIN
		IF(@WhereKeyWord = '')
		BEGIN 
			--SET @WhereKeyWord = @WhereKeyWord + 'RA.IdRadicado = ' + CAST(@KeyWord AS NVARCHAR(MAX))
			SET @WhereKeyWord = @WhereKeyWord + 'RA.IdRadicado LIKE ''%' + CAST(@KeyWord AS NVARCHAR(MAX)) + '%'''   
		END 
		ElSE
		BEGIN 
			--SET @WhereKeyWord = @WhereKeyWord + ' OR RA.IdRadicado = ' + CAST(@KeyWord AS NVARCHAR(MAX))
			SET @WhereKeyWord = @WhereKeyWord + ' OR RA.IdRadicado LIKE ''%' + CAST(@KeyWord AS NVARCHAR(MAX)) + '%'''   
		END 
	END --ENDIF NUMERIC
END 
--
IF(@KeyWord <> '')
BEGIN 
	IF(ISNUMERIC(@KeyWord) = 1)
	BEGIN
		IF(@WhereKeyWord = '')
		BEGIN 
			--SET @WhereKeyWord = @WhereKeyWord + 'UD.Codigo = ' + CAST(@KeyWord AS NVARCHAR(MAX))
			SET @WhereKeyWord = @WhereKeyWord + 'UD.Codigo LIKE ''%' + CAST(@KeyWord AS NVARCHAR(MAX)) + '%'''   
		END 
		ElSE
		BEGIN 
			--SET @WhereKeyWord = @WhereKeyWord + ' OR UD.Codigo = ' + CAST(@KeyWord AS NVARCHAR(MAX))
			SET @WhereKeyWord = @WhereKeyWord + ' OR UD.Codigo LIKE ''%' + CAST(@KeyWord AS NVARCHAR(MAX)) + '%'''  
		END 
	END --ENDIF NUMERIC
END 
-- //
--Validamos datos del WHERE de palabra clave.
IF(@WhereKeyWord <> '')
BEGIN
	SET @WhereKeyWord = ' AND (' + @WhereKeyWord + ')';
END


--Paginado  
DECLARE @Paginado VARCHAR(MAX) = '  
ORDER BY RA.IdRadicado  
OFFSET ' + CAST(@Paginate AS NVARCHAR(MAX)) + ' ROWS  
FETCH NEXT ' + CAST(@MaxRows AS NVARCHAR(MAX)) + ' ROWS ONLY;'  
   
--Concatenamos Query, Condiciones y Paginado. Luego ejecutamos.  
IF(@Where <> '' )  
BEGIN   
 SET @Where = ' WHERE ' + @Where;                
END    
DECLARE @Total VARCHAR(MAX);  
--SET @Total = @Query + '' + @Where + ' ' + @Paginado  
SET @Total = @Query + '' + @Where + ' ' + @WhereKeyWord + ' ' + @Paginado    
  
--Para calcular total registros filtrados.  
DECLARE @Query2 NVARCHAR(MAX);  
SET @Query2 = N'SELECT NEWID() As Idk, COUNT(*) As NoRegistrosTotalesFiltrado   
FROM RegistrosAuditoria RA   
JOIN Medicion ME ON (RA.IdMedicion = ME.Id)   
JOIN EstadosRegistroAuditoria ERA ON (RA.Estado = ERA.Id)   
JOIN AspNetUsers US ON (US.Id = RA.IdAuditor)  
LEFT JOIN AspNetUsersDetelles UD ON UD.AspNetUsersId = US.Id
JOIN AspNetUserRoles UR ON (UR.UserId = RA.IdAuditor)  
JOIN EstadoRegistroAuditoriaIcono RIC ON (RIC.Estado = RA.Estado AND RIC.RolId = UR.RoleId) AND RA.Revisar = RIC.Revisar 
JOIN Enfermedad EF ON EF.idCobertura = ME.IdCobertura
JOIN Eps E ON (E.EpsId = RA.IdEPS) '  
  
DECLARE @Total2 NVARCHAR(MAX);  
SET @Total2 = @Query2 + '' + REPLACE(@Where, '''', '''''')  
  
--str, old_str. new_str  
SET @Total = REPLACE(@Total, ''' '' As QueryNoRegistrosTotales', '''' + CAST(@Total2 AS NVARCHAR(MAX)) + ''' As QueryNoRegistrosTotales');  
  
--Imprimimos/Ejecutamos.  
EXEC(@Total);
--PRINT(@Total);

END --END SP
GO
/****** Object:  StoredProcedure [dbo].[SP_RegistrosAuditoria_XBolsaMedicion_Filtros]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-02-23
-- Description:	Para cargar los filtros usados en vista de Reasignaciones de bolsa.    
-- =============================================
CREATE PROCEDURE [dbo].[SP_RegistrosAuditoria_XBolsaMedicion_Filtros]
(@IdAuditor VARCHAR(150), @MedicionId VARCHAR(MAX))
AS
BEGIN
SET NOCOUNT ON

--Para convertir 'all' a vacio.
IF @IdAuditor = 'all'
BEGIN
SET @IdAuditor = ''
END

--Guardamos Query inicial.  
DECLARE @Query VARCHAR(MAX) = '';  
DECLARE @Where VARCHAR(MAX) = '';  


--Consultamos IPS. FALTA. PENDIENTE
--SET @Query = @Query + ''
--UNION

--Consultamos IdMedicion.
SET @Query = @Query + 'SELECT NEWID() As Idk, ''Bolsa medicion'' As NombreFiltro, CAST(MAX(ME.Id) AS NVARCHAR(MAX)) As Id, CAST(MAX(ME.Nombre) AS NVARCHAR(MAX)) As Valor 
FROM medicion ME 
INNER JOIN RegistrosAuditoria RE ON (ME.Id = RE.IdMedicion) '
--
IF(@IdAuditor <> '')
BEGIN
	IF(@Where = '')
	BEGIN
		SET @Where = @Where + 'RE.IdAuditor IN (''' + CAST(@IdAuditor AS NVARCHAR(MAX)) + ''') AND ME.Status = 1 AND RE.Status = 1 '
	END
	ELSE
	BEGIN
		SET @Where = @Where + ' AND RE.IdAuditor IN (''' + CAST(@IdAuditor AS NVARCHAR(MAX)) + ''') AND ME.Status = 1 AND RE.Status = 1 '
	END
END
--
IF(@MedicionId <> '')
BEGIN
	IF(@Where = '')
	BEGIN
		SET @Where = @Where + 'ME.Id = ' + CAST(@MedicionId AS NVARCHAR(MAX));
	END
	ELSE
	BEGIN
		SET @Where = @Where + ' AND ME.Id = ' + CAST(@MedicionId AS NVARCHAR(MAX));
	END
END
--
IF(@Where <> '' )  
BEGIN   
 SET @Where = ' WHERE ' + @Where;                
END
SET @Query = @Query + @Where + ' GROUP BY RE.IdMedicion '
SET @Where = '';
-- //
  

SET @Query = @Query + ' UNION '


--Tablas en cac para alamacenar IPS y EPS y como se relacionan con el registro del paciente.
SET @Query = @Query + 'SELECT NEWID() As Idk, ''Entidad'' As NombreFiltro, CAST(MAX(E.EpsId) AS NVARCHAR(MAX)) As Id, CAST(MAX(E.Nombre) AS NVARCHAR(MAX)) As Valor   
FROM RegistrosAuditoria RA 
INNER JOIN Eps E ON (E.EpsId = RA.IdEPS) '
--
IF(@IdAuditor <> '')
BEGIN
	IF(@Where = '')
	BEGIN
		SET @Where = @Where + 'RA.IdAuditor IN (''' + CAST(@IdAuditor AS NVARCHAR(MAX)) + ''') AND RA.Status = 1 AND E.Status = 1 '
	END
	ELSE
	BEGIN
		SET @Where = @Where + ' AND RA.IdAuditor IN (''' + CAST(@IdAuditor AS NVARCHAR(MAX)) + ''') AND RA.Status = 1 AND E.Status = 1 '
	END
END
--
IF(@MedicionId <> '')
BEGIN
	IF(@Where = '')
	BEGIN
		SET @Where = @Where + 'RA.IdMedicion = ' + CAST(@MedicionId AS NVARCHAR(MAX));
	END
	ELSE
	BEGIN
		SET @Where = @Where + ' AND RA.IdMedicion = ' + CAST(@MedicionId AS NVARCHAR(MAX));
	END
END
--
IF(@Where <> '' )  
BEGIN   
 SET @Where = ' WHERE ' + @Where;                
END
SET @Query = @Query + @Where + ' GROUP BY E.Nombre '
SET @Where = '';
-- //


SET @Query = @Query + ' UNION '


--Consultamos Estado de registro.
SET @Query = @Query + 'SELECT NEWID() As Idk, ''Estado de registro'' As NombreFiltro, CAST(MAX(RA.Estado) AS NVARCHAR(MAX)) As Id, CAST(MAX(ERA.Descripción) AS NVARCHAR(MAX)) As Valor 
FROM RegistrosAuditoria RA 
INNER JOIN EstadosRegistroAuditoria ERA ON (ERA.Id = RA.Estado) '
--
IF(@IdAuditor <> '')
BEGIN
	IF(@Where = '')
	BEGIN
		SET @Where = @Where + 'RA.IdAuditor IN (''' + CAST(@IdAuditor AS NVARCHAR(MAX)) + ''') AND RA.Status = 1 AND ERA.Status = 1 '
	END
	ELSE
	BEGIN
		SET @Where = @Where + ' AND RA.IdAuditor IN (''' + CAST(@IdAuditor AS NVARCHAR(MAX)) + ''') AND RA.Status = 1 AND ERA.Status = 1 '
	END
END
--
IF(@MedicionId <> '')
BEGIN
	IF(@Where = '')
	BEGIN
		SET @Where = @Where + 'RA.IdMedicion = ' + CAST(@MedicionId AS NVARCHAR(MAX));
	END
	ELSE
	BEGIN
		SET @Where = @Where + ' AND RA.IdMedicion = ' + CAST(@MedicionId AS NVARCHAR(MAX));
	END
END
--
IF(@Where <> '' )  
BEGIN   
 SET @Where = ' WHERE ' + @Where;                
END
SET @Query = @Query + @Where + ' GROUP BY RA.Estado '
SET @Where = '';
-- //


SET @Query = @Query + ' UNION '


--Consultamos ModifyBy.
SET @Query = @Query + 'SELECT NEWID() As Idk, ''ModifyBy'' As NombreFiltro, CAST(MAX(ModifyBy) AS NVARCHAR(MAX)) As Id, CAST(MAX(ModifyBy) AS NVARCHAR(MAX)) As Valor 
FROM RegistrosAuditoria RA '
--
IF(@IdAuditor <> '')
BEGIN
	IF(@Where = '')
	BEGIN
		SET @Where = @Where + 'IdAuditor IN (''' + CAST(@IdAuditor AS NVARCHAR(MAX)) + ''') AND Status = 1 '
	END
	ELSE
	BEGIN
		SET @Where = @Where + ' AND IdAuditor IN (''' + CAST(@IdAuditor AS NVARCHAR(MAX)) + ''') AND Status = 1 '
	END
END
--
IF(@MedicionId <> '')
BEGIN
	IF(@Where = '')
	BEGIN
		SET @Where = @Where + 'RA.IdMedicion = ' + CAST(@MedicionId AS NVARCHAR(MAX));
	END
	ELSE
	BEGIN
		SET @Where = @Where + ' AND RA.IdMedicion = ' + CAST(@MedicionId AS NVARCHAR(MAX));
	END
END
--
IF(@Where <> '' )  
BEGIN   
 SET @Where = ' WHERE ' + @Where;                
END
SET @Query = @Query + @Where + ' GROUP BY ModifyBy '
SET @Where = '';
-- //


SET @Query = @Query + ' UNION '


--Consultamos Sexo. Crear campo en registro auditoria.
SET @Query = @Query + 'SELECT NEWID() As Idk, ''Sexo'' As NombreFiltro, CAST(MAX(Sexo) AS NVARCHAR(MAX)) As Id, CAST(MAX(Sexo) AS NVARCHAR(MAX)) As Valor  
FROM RegistrosAuditoria RA '
--
IF(@IdAuditor <> '')
BEGIN
	IF(@Where = '')
	BEGIN
		SET @Where = @Where + 'IdAuditor IN (''' + CAST(@IdAuditor AS NVARCHAR(MAX)) + ''') AND Status = 1 '
	END
	ELSE
	BEGIN
		SET @Where = @Where + ' AND IdAuditor IN (''' + CAST(@IdAuditor AS NVARCHAR(MAX)) + ''') AND Status = 1 '
	END
END
--
IF(@MedicionId <> '')
BEGIN
	IF(@Where = '')
	BEGIN
		SET @Where = @Where + 'RA.IdMedicion = ' + CAST(@MedicionId AS NVARCHAR(MAX));
	END
	ELSE
	BEGIN
		SET @Where = @Where + ' AND RA.IdMedicion = ' + CAST(@MedicionId AS NVARCHAR(MAX));
	END
END
--
IF(@Where <> '' )  
BEGIN   
 SET @Where = ' WHERE ' + @Where;                
END
SET @Query = @Query + @Where + ' GROUP BY Sexo,RA.IdMedicion '
SET @Where = '';
-- //


--Ordenamos
SET @Query = @Query + ' ORDER BY NombreFiltro, Id '
-- //

--Imprimimos/Ejecutamos.  
EXEC(@Query); 
--SELECT(@Query); 
--PRINT(@Query); 

END --END SP
GO
/****** Object:  StoredProcedure [dbo].[SP_Test_Proceso]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar
-- Create date: 2021-09-21
-- Description:	Prueba proceso segundo plano
-- =============================================
CREATE PROCEDURE [dbo].[SP_Test_Proceso]
	-- Add the parameters for the stored procedure here
	@CurrentProcessId INT,
	@TestParam NVARCHAR(20)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    DECLARE @Row INT = 0

	DECLARE @TotalRow BIGINT  = (select count(*) from CAC.[dbo].[cobhemofilia] where estado = 1) + 100000
	DECLARE @IndividualAdvance FLOAT = CONVERT(FLOAT,100)/@TotalRow;

	DECLARE @Advance FLOAT = 0 

	WHILE @Row <= @TotalRow 
		BEGIN  
			 PRINT(@Row)
			 SET @Row = @Row + 1
			 SET @Advance = @Advance + @IndividualAdvance

			 UPDATE Current_Process set Progress = ROUND(@Advance,0), Result = @Advance  WHERE Id = @CurrentProcessId
		END  
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Upsert_ErrorRegistroAuditoria]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar - IT SENSE
-- Create date: 2022-03-04
-- Description: Crea o Actualiza RegistroAuditoriaDetalleError
-- =============================================
CREATE PROCEDURE [dbo].[SP_Upsert_ErrorRegistroAuditoria]
	
	@RegistrosAuditoriaDetalleId INT,
	@Reducido NVARCHAR(255),
	@VariableId INT,
	@ErrorId NVARCHAR(255),
	@Descripcion NVARCHAR(MAX),
	@Enable BIT,
	@Usuario NVARCHAR(255)
AS
BEGIN

		IF (SELECT COUNT(*) FROM RegistroAuditoriaDetalleError 			
				WHERE RegistrosAuditoriaDetalleId = @RegistrosAuditoriaDetalleId AND
				ErrorId = @ErrorId AND
				VariableId = @VariableId) = 0
			BEGIN -- Cretate

				INSERT INTO [dbo].[RegistroAuditoriaDetalleError]([RegistrosAuditoriaDetalleId],[Reducido],[VariableId],[ErrorId],[Descripcion],[Enable],[CreatedBy],[CreatedDate],[ModifyBy],[ModifyDate])
				 VALUES(@RegistrosAuditoriaDetalleId,@Reducido,@VariableId,@ErrorId,@Descripcion,@Enable,@Usuario,GETDATE(),@Usuario,GETDATE())

	
			END

		ELSE -- Update
			BEGIN
				DECLARE @ConsultarEnable BIT = 0;

				SET @ConsultarEnable = (SELECT [Enable] FROM RegistroAuditoriaDetalleError 
					WHERE RegistrosAuditoriaDetalleId = @RegistrosAuditoriaDetalleId AND
						ErrorId = @ErrorId AND
						VariableId = @VariableId)


				IF @ConsultarEnable <> @Enable -- Valida si cambia check
					BEGIN

						UPDATE RegistroAuditoriaDetalleError
								SET
									[Enable] = @Enable,
									ModifyBy = @Usuario,
									ModifyDate = GETDATE()
								WHERE RegistrosAuditoriaDetalleId = @RegistrosAuditoriaDetalleId AND
									ErrorId = @ErrorId AND
									VariableId = @VariableId;
					END
		
			END
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Usuarios_BolsaMedicion]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-02-22
-- Description:	Para la consulta de los registros asignados a un auditor y a un estado      
-- =============================================
CREATE PROCEDURE [dbo].[SP_Usuarios_BolsaMedicion]
(@PageNumber INT, @MaxRows INT, @MedicionId VARCHAR(MAX), @AuditorId VARCHAR(MAX), @KeyWord VARCHAR(MAX))
AS  
BEGIN  
SET NOCOUNT ON  

--Variables usadas.
DECLARE @FechaActual Datetime = GETDATE();
DECLARE @WhereAuditadosAsignados VARCHAR(MAX) = '';

--Validamos condiciones del Where de Asignados y Auditados.
IF(@MedicionId <> '')  
BEGIN   
 SET @WhereAuditadosAsignados = @WhereAuditadosAsignados + ' AND ME.Id IN (' + CAST(@MedicionId AS NVARCHAR(MAX)) + ')'  
END  

--Guardamos Query inicial.  
DECLARE @Query VARCHAR(MAX); 

--(SELECT COUNT(*) FROM RegistrosAuditoria RA INNER JOIN Medicion ME ON (RA.IdMedicion = ME.Id) WHERE RA.IdAuditor = US.Id AND ''' + CAST(@FechaActual As NVARCHAR(35)) + '''>= ME.FechaInicioAuditoria AND ''' + CAST(@FechaActual As NVARCHAR(35)) + ''' <= ME.FechaFinAuditoria) As RegistrosAsignados,
--(SELECT COUNT(*) FROM RegistrosAuditoria RA INNER JOIN Medicion ME ON (RA.IdMedicion = ME.Id) WHERE RA.IdAuditor = US.Id AND ''' + CAST(@FechaActual As NVARCHAR(35)) + '''>= ME.FechaInicioAuditoria AND ''' + CAST(@FechaActual As NVARCHAR(35)) + ''' <= ME.FechaFinAuditoria AND RA.Estado NOT LIKE (1)) As RegistrosAuditados, 
SET @Query = 'SELECT DISTINCT '' '' As QueryNoRegistrosTotales, US.UserName As AuditorUsuario, USD.Nombres As AuditorNombres, USD.Apellidos As AuditorApellidos, USD.Codigo As AuditorCodigo, 
(SELECT COUNT(*) FROM RegistrosAuditoria RA INNER JOIN Medicion ME ON (RA.IdMedicion = ME.Id) WHERE RA.IdAuditor = US.Id' + @WhereAuditadosAsignados + ') As RegistrosAsignados,
(SELECT COUNT(*) FROM RegistrosAuditoria RA INNER JOIN Medicion ME ON (RA.IdMedicion = ME.Id) WHERE RA.IdAuditor = US.Id AND RA.Estado IN (2,3,4,5,6,7,8,9,10,11,12,13,14,15,16)' + @WhereAuditadosAsignados + ') As RegistrosAuditados, 
CAST(CASE WHEN US.Active = 1 THEN ''Activo'' ELSE ''Inactivo'' END AS NVARCHAR(8)) As AuditorEstado
FROM Medicion ME 
INNER JOIN RegistrosAuditoria RA ON (RA.IdMedicion = ME.Id)
INNER JOIN AspNetUsers US ON (US.Id = RA.IdAuditor)
INNER JOIN AspNetUsersDetelles USD ON (USD.AspNetUsersId = US.Id) ';

--Calculo de paginado.  
--SET @PageNumber = @PageNumber - 1;  
DECLARE @Paginate INT = @PageNumber * @MaxRows;  
  
--Guardamos Condiciones.  
DECLARE @Where VARCHAR(MAX) = '';  
IF(@MedicionId <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  SET @Where = @Where + 'ME.Id IN (' + CAST(@MedicionId AS NVARCHAR(MAX)) + ')'  
 END   
    ElSE  
 BEGIN   
  SET @Where = @Where + ' AND ME.Id IN (' + CAST(@MedicionId AS NVARCHAR(MAX)) + ')'  
 END   
END   
--  
IF(@AuditorId <> '')  
BEGIN   
 IF(@Where = '')  
 BEGIN   
  --SET @Where = @Where + 'US.Id IN (''' + CAST(@AuditorId AS NVARCHAR(MAX)) + ''')'  
  SET @Where = @Where + 'USD.Codigo IN (' + CAST(@AuditorId AS NVARCHAR(MAX)) + ')'  
 END   
    ElSE  
 BEGIN   
  --SET @Where = @Where + ' AND US.Id IN (''' + CAST(@AuditorId AS NVARCHAR(MAX)) + ''')'  
  SET @Where = @Where + ' AND USD.Codigo IN (' + CAST(@AuditorId AS NVARCHAR(MAX)) + ')'  
 END   
END   
--  
-- //
--Definimos WHERE de Palabra Clave.
DECLARE @WhereKeyWord VARCHAR(MAX) = '';  
IF(@KeyWord <> '')
BEGIN 
	IF(@WhereKeyWord = '')
	BEGIN 
		SET @WhereKeyWord = @WhereKeyWord + 'US.UserName LIKE ''%' + CAST(@KeyWord AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @WhereKeyWord = @WhereKeyWord + ' OR US.UserName LIKE ''%' + CAST(@KeyWord AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@KeyWord <> '')
BEGIN 
	IF(@WhereKeyWord = '')
	BEGIN 
		SET @WhereKeyWord = @WhereKeyWord + 'USD.Codigo LIKE ''%' + CAST(@KeyWord AS NVARCHAR(MAX)) + '%''' 
	END 
    ElSE
	BEGIN 
		SET @WhereKeyWord = @WhereKeyWord + ' OR USD.Codigo LIKE ''%' + CAST(@KeyWord AS NVARCHAR(MAX)) + '%'''
	END 
END 
--
IF(@KeyWord = 'Activo')  
BEGIN   
	IF(@WhereKeyWord = '')  
	BEGIN   
		SET @WhereKeyWord = @WhereKeyWord + 'US.Active  = ' + CAST(1 AS NVARCHAR(MAX))  
	END   
    ElSE  
	BEGIN   
		SET @WhereKeyWord = @WhereKeyWord + ' OR US.Active  = ' + CAST(1 AS NVARCHAR(MAX))  
	END   
END   
ElSE IF(@KeyWord = 'Inactivo')  
BEGIN   
	IF(@WhereKeyWord = '')  
	BEGIN   
		SET @WhereKeyWord = @WhereKeyWord + 'US.Active  = ' + CAST(0 AS NVARCHAR(MAX))  
	END   
    ElSE  
	BEGIN   
		SET @WhereKeyWord = @WhereKeyWord + ' OR US.Active  = ' + CAST(0 AS NVARCHAR(MAX))  
	END   
END 
-- 
-- //
--Validamos datos del WHERE de palabra clave.
IF(@WhereKeyWord <> '')
BEGIN
	SET @WhereKeyWord = ' AND (' + @WhereKeyWord + ')';
END


--Declaramos GroupBy Pendiente.
--DECLARE @GroupBy VARCHAR(MAX) = 'GROUP BY US.Id, US.UserName, USD.Codigo, US.Active';


--Paginado  
DECLARE @Paginado VARCHAR(MAX) = '  
ORDER BY US.UserName 
OFFSET ' + CAST(@Paginate AS NVARCHAR(MAX)) + ' ROWS  
FETCH NEXT ' + CAST(@MaxRows AS NVARCHAR(MAX)) + ' ROWS ONLY;'  
   
--Concatenamos Query, Condiciones y Paginado. Luego ejecutamos.  
IF(@Where <> '' )  
BEGIN   
 SET @Where = ' WHERE ' + @Where;                
END    
DECLARE @Total VARCHAR(MAX);  
--SET @Total = @Query + '' + @Where + ' ' + @WhereKeyWord + ' ' + @GroupBy + ' ' + @Paginado  
SET @Total = @Query + '' + @Where + ' ' + @WhereKeyWord + ' ' + @Paginado  
  
--Para calcular total registros filtrados.  
DECLARE @Query2 NVARCHAR(MAX);  
SET @Query2 = N'SELECT NEWID() As Idk, COUNT(DISTINCT US.UserName) As NoRegistrosTotalesFiltrado  
FROM Medicion ME 
INNER JOIN RegistrosAuditoria RA ON (RA.IdMedicion = ME.Id)
INNER JOIN AspNetUsers US ON (US.Id = RA.IdAuditor)
INNER JOIN AspNetUsersDetelles USD ON (USD.AspNetUsersId = US.Id) '  
  
DECLARE @Total2 NVARCHAR(MAX);  
SET @Total2 = @Query2 + '' + REPLACE(@Where, '''', '''''')  
  
  
--str, old_str. new_str  
SET @Total = REPLACE(@Total, ''' '' As QueryNoRegistrosTotales', '''' + CAST(@Total2 AS NVARCHAR(MAX)) + ''' As QueryNoRegistrosTotales');  

  
--Imprimimos/Ejecutamos.  
EXEC(@Total);  
--PRINT(@Total);  


END --END SP
GO
/****** Object:  StoredProcedure [dbo].[SP_Usuarios_BolsaMedicion_Filtro]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-02-22
-- Description:	Para la consulta de usuarios (filtros) de una bolsa.    
-- =============================================
CREATE PROCEDURE [dbo].[SP_Usuarios_BolsaMedicion_Filtro]
(@MedicionId VARCHAR(MAX))
AS  
BEGIN  
SET NOCOUNT ON  

	--Guardamos Query inicial.  
	DECLARE @Query VARCHAR(MAX);  
	SET @Query = 'SELECT DISTINCT US.Id, US.UserName, USD.Nombres, USD.Apellidos, USD.Codigo
				FROM Medicion ME
				INNER JOIN RegistrosAuditoria RA ON (RA.IdMedicion = ME.Id)
				INNER JOIN AspNetUsers US ON (US.Id = RA.IdAuditor)
				INNER JOIN AspNetUsersDetelles USD ON (USD.AspNetUsersId = US.Id) '

	--Guardamos Condiciones.  
	DECLARE @Where VARCHAR(MAX) = '';  
	--  
	IF(@MedicionId <> '')  
	BEGIN   
	 IF(@Where = '')  
	 BEGIN   
	  SET @Where = @Where + 'ME.Id IN (''' + CAST(@MedicionId AS NVARCHAR(MAX)) + ''')'  
	 END   
		ElSE  
	 BEGIN   
	  SET @Where = @Where + ' AND ME.Id IN (''' + CAST(@MedicionId AS NVARCHAR(MAX)) + ''')'  
	 END   
	END   
		-- //

	--Concatenamos Query, Condiciones y Paginado. Luego ejecutamos.  
	IF(@Where <> '' )  
	BEGIN   
	 SET @Where = ' WHERE ' + @Where;                
	END    
	DECLARE @Total VARCHAR(MAX);  
	SET @Total = @Query + '' + @Where   
	-- //

	--Imprimimos/Ejecutamos.  
	EXEC(@Total);
	--PRINT(@Total);

END --END SP
GO
/****** Object:  StoredProcedure [dbo].[SP_Usuarios_By_Rol]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-03-07
-- Description:	Para la consulta de usuarios segun su Rol
-- =============================================
CREATE PROCEDURE [dbo].[SP_Usuarios_By_Rol]
(@RolId VARCHAR(MAX))
AS  
BEGIN  
SET NOCOUNT ON  

	--Guardamos Query inicial.  
	DECLARE @Query VARCHAR(MAX);  
	SET @Query = 'SELECT US.Id, US.UserName, USD.Nombres, USD.Apellidos, USD.Codigo
				FROM AspNetUsers US 
				INNER JOIN AspNetUserRoles USRO ON (US.Id = USRO.UserId)
				INNER JOIN AspNetRoles RO ON (USRO.RoleId  = RO.Id)
				INNER JOIN AspNetUsersDetelles USD ON (USD.AspNetUsersId = US.Id) '

	--Guardamos Condiciones.  
	DECLARE @Where VARCHAR(MAX) = '';  
	--  
	IF(@RolId <> '')  
	BEGIN   
	 IF(@Where = '')  
	 BEGIN   
	  SET @Where = @Where + 'RO.Id IN (''' + CAST(@RolId AS NVARCHAR(MAX)) + ''')'  
	 END   
		ElSE  
	 BEGIN   
	  SET @Where = @Where + ' AND RO.Id IN (''' + CAST(@RolId AS NVARCHAR(MAX)) + ''')'  
	 END   
	END  
	
	--Validamos estado activo.
	IF(@Where = '')  
		BEGIN   
		SET @Where = @Where + ' US.Active = 1 ' 
		END   
		ElSE  
		BEGIN   
		SET @Where = @Where + ' AND US.Active = 1 '
	END 
	-- //

	--Concatenamos Query, Condiciones y Paginado. Luego ejecutamos.  
	IF(@Where <> '' )  
	BEGIN   
	 SET @Where = ' WHERE ' + @Where;                
	END    
	DECLARE @Total VARCHAR(MAX);  
	SET @Total = @Query + '' + @Where   
	-- //

	--Imprimimos/Ejecutamos.  
	EXEC(@Total);
	--PRINT(@Total);

END --END SP
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Estado_BolsasMedicion]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-02-27
-- Description:	Para la consulta de los registros asignados a un auditor y a un estado.
-- =============================================
--DROP PROCEDURE SP_Validacion_Estado_BolsasMedicion2
CREATE PROCEDURE [dbo].[SP_Validacion_Estado_BolsasMedicion]
(@MedicionId INT)
AS  
BEGIN  
SET NOCOUNT ON  
  
  --Declaramos variables y obtenemos valores default.
  DECLARE @EnCurso INT = 0;
  DECLARE @Asignada INT = 0;
  DECLARE @Finalizada INT = 0;
  DECLARE @Creada INT = 0;
  DECLARE @EstadoMedicionId INT = 0;
  DECLARE @EstadoMedicionNombre VARCHAR(50) = '';
  --
  DECLARE @FechaActual Datetime = GETDATE(); -- '2022-04-21 11:34:59.623' | GETDATE()

	--Validamos en estado: En curso. OK
	--En curso: Cuando se está auditando actualmente es decir se sencuentra entre la fecha inicio y fecha fin / Para que la bolsa pase en curso debe estar cargada la población 
	SET @EnCurso = (SELECT COUNT(*) FROM Medicion ME
	INNER JOIN RegistrosAuditoria RA ON (ME.Id = RA.IdMedicion)
	WHERE ME.Id = @MedicionId 
	AND @FechaActual >= ME.FechaInicioAuditoria AND @FechaActual <= ME.FechaFinAuditoria);

	--Validamos en estado: Asignada
	--Asignada: Cuando se ha cargado la población a la bolsa y no se está entre las fechas de auditoria. Solo fechas futuras.
	SET @Asignada = (SELECT COUNT(*) FROM Medicion ME
	INNER JOIN RegistrosAuditoria RA ON (ME.Id = RA.IdMedicion)
	WHERE ME.Id = @MedicionId 
	--AND ( (@FechaActual > ME.FechaInicioAuditoria AND @FechaActual > ME.FechaFinAuditoria) OR (@FechaActual < ME.FechaInicioAuditoria AND @FechaActual < ME.FechaFinAuditoria) )
	AND ( (@FechaActual < ME.FechaInicioAuditoria AND @FechaActual < ME.FechaFinAuditoria) )
	);
	--AND @FechaActual >= CAST(ME.FechaInicioAuditoria AS DATE) AND @FechaActual <=  CAST(ME.FechaFinAuditoria AS DATE)

	--Validamos en estado: Finalizada. OK
	--Finalizada: Cuando ya se cerró la auditoría ya paso la fecha fin
	SET @Finalizada = (SELECT COUNT(*) FROM Medicion ME 
	WHERE ME.Id = @MedicionId 
	AND @FechaActual > ME.FechaFinAuditoria);

	--Validamos en estado: Creada. OK
	--Creada: Cuando se creo la bolsa y/o las variables están parametrizada
	IF ((SELECT COUNT(*) FROM Medicion ME WHERE ME.Id = @MedicionId) > 0)
	BEGIN
	SET @Creada = (SELECT COUNT(*) FROM Medicion ME WHERE ME.Id = @MedicionId AND @FechaActual >= ME.FechaInicioAuditoria AND @FechaActual <= ME.FechaFinAuditoria);
	END
	ELSE IF((SELECT COUNT(*) FROM Variables VA INNER JOIN VariableXMedicion VAXM ON (VA.Id = VAXM.VariableId) WHERE VAXM.MedicionId = @MedicionId) > 0)
	BEGIN
		SET @Creada = (SELECT COUNT(*) FROM Variables VA
		INNER JOIN VariableXMedicion VAXM ON (VA.Id = VAXM.VariableId)
		INNER JOIN Medicion ME ON (ME.Id = VAXM.MedicionId)
		WHERE VAXM.MedicionId = @MedicionId
		AND @FechaActual >= ME.FechaInicioAuditoria AND @FechaActual <= ME.FechaFinAuditoria);
	END
	
	IF(@EnCurso > 0)
	BEGIN
		SET @EstadoMedicionId = 28;
		SET @EstadoMedicionNombre = (SELECT ItemName FROM Item WHERE Id = 28);
	END
	ELSE IF(@Asignada > 0)
	BEGIN
		SET @EstadoMedicionId = 29;
		SET @EstadoMedicionNombre = (SELECT ItemName FROM Item WHERE Id = 29);
	END
	ELSE IF(@Creada > 0)
	BEGIN
		SET @EstadoMedicionId = 31;
		SET @EstadoMedicionNombre = (SELECT ItemName FROM Item WHERE Id = 31);
	END
	ELSE IF(@Finalizada > 0)
	BEGIN
		SET @EstadoMedicionId = 30;
		SET @EstadoMedicionNombre = (SELECT ItemName FROM Item WHERE Id = 30);
	END	 
	ELSE -- Si todos son 0
	BEGIN	
		SET @EstadoMedicionId = (SELECT ME.Estado FROM Medicion ME WHERE ME.Id = @MedicionId);
		SET @EstadoMedicionNombre = (SELECT ItemName FROM Item WHERE Id = @EstadoMedicionId);
	END

	--Retornamos Estado.
	--SELECT @EnCurso As EnCurso, @Asignada As Asignada, @Finalizada As Finalizada, @Creada As Creada, @FechaActual As FechaActual, @EstadoMedicionId As EstadoMedicionId, @EstadoMedicionNombre As EstadoMedicionNombre;
	SELECT @EstadoMedicionId As EstadoMedicionId, @EstadoMedicionNombre As EstadoMedicionNombre

END --END Procedure
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Estado_CA]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor cuellar - IT SENSE
-- Create date: 2022-03-02
-- Description:	SP para validaciones del resgistro de auditoria cuando el estado es comite administrativo CA
-- =============================================
CREATE PROCEDURE [dbo].[SP_Validacion_Estado_CA]
	
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

		-- Llama SP que valida si hay motivos sin registrar
		EXEC	@SolicitarMotivo = [dbo].[SP_Validacion_Registrar_Motivo]
		@IdRegistroAuditoria = @RegistroAuditoriaId


		--Variables
		DECLARE @RoleId INT = CAST((SELECT TOP(1) RoleId FROM AspNetUserRoles WHERE UserId = @IdUsuario) AS INT);


			-- Validación Lider
			IF @RoleId = 2 -- Lider
			
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
			
			ELSE IF  @RoleId = 3 AND @CountLevantarGlosa > 0 -- Auditor y glosas levantadas
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
					WHERE RegistrosAuditoriaId = @RegistroAuditoriaId)
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
		@ErroresReportados AS ErroresReportados

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Estado_CE]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor cuellar - IT SENSE
-- Create date: 2022-03-02
-- Description:	SP para validaciones del resgistro de auditoria cuando el estado es comite administrativo CE
-- =============================================
CREATE PROCEDURE [dbo].[SP_Validacion_Estado_CE]
	
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

		-- Llama SP que valida si hay motivos sin registrar
		EXEC	@SolicitarMotivo = [dbo].[SP_Validacion_Registrar_Motivo]
		@IdRegistroAuditoria = @RegistroAuditoriaId


		--Variables
		DECLARE @RoleId INT = CAST((SELECT TOP(1) RoleId FROM AspNetUserRoles WHERE UserId = @IdUsuario) AS INT);


			-- Validación Lider
			IF @RoleId = 2 -- Lider
			
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
							WHERE RegistrosAuditoriaId = @RegistroAuditoriaId)
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
						  WHERE rg.RegistrosAuditoriaId = @RegistroAuditoriaId 
							AND vm.SubGrupoId = @IdItemGlosa -- Glosa
							AND rg.Dato_DC_NC_ND <> @IdItemDC -- DC
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
		@ErroresReportados AS ErroresReportados

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Estado_ELA]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor cuellar - IT SENSE
-- Create date: 2022-03-02
-- Description:	SP para validaciones del resgistro de auditoria cuando el estado es Error lógica marcación auditor ELA
-- =============================================
CREATE PROCEDURE [dbo].[SP_Validacion_Estado_ELA]
	
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

		-- Llama SP que valida si hay motivos sin registrar
		EXEC	@SolicitarMotivo = [dbo].[SP_Validacion_Registrar_Motivo]
		@IdRegistroAuditoria = @RegistroAuditoriaId


		--Variables
		DECLARE @RoleId INT = CAST((SELECT TOP(1) RoleId FROM AspNetUserRoles WHERE UserId = @IdUsuario) AS INT);


			-- Validación Lider
			IF @RoleId = 2 -- Lider
			
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
									INNER JOIN AspNetUserRoles ur ON rs.CreatedBy = ur.UserId
													WHERE rs.RegistroAuditoriaId = @RegistroAuditoriaId 
													AND rs.TipoObservacion = @TipificacionObservacionDefault 
													AND ur.RoleId = 2 -- Lider
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
		@ErroresReportados AS ErroresReportados

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Estado_ELL]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor cuellar - IT SENSE
-- Create date: 2022-03-02
-- Description:	SP para validaciones del resgistro de auditoria cuando el estado es Error lógica marcación lider ELL
-- =============================================
CREATE PROCEDURE [dbo].[SP_Validacion_Estado_ELL]
	
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

		-- Llama SP que valida si hay motivos sin registrar
		EXEC	@SolicitarMotivo = [dbo].[SP_Validacion_Registrar_Motivo]
		@IdRegistroAuditoria = @RegistroAuditoriaId


		--Variables
		DECLARE @RoleId INT = CAST((SELECT TOP(1) RoleId FROM AspNetUserRoles WHERE UserId = @IdUsuario) AS INT);


			
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
		@ErroresReportados AS ErroresReportados

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Estado_GO1]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-02-02
-- Description:	SP para validaciones del resgistro de auditoria cuando el estado es glosa objectada 1
-- =============================================
CREATE PROCEDURE [dbo].[SP_Validacion_Estado_GO1]
	
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
						OR
						-- Calificacion Obligatoria para variable IPS
						((SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rd
						INNER JOIN RegistrosAuditoria ra ON ra.Id = rd.RegistrosAuditoriaId
						INNER JOIN VariableXMedicion vm ON rd.VariableId = vm.VariableId AND vm.Encuesta = 1 AND ra.IdMedicion = vm.MedicionId
						WHERE RegistrosAuditoriaId = @RegistroAuditoriaId)
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
		@ErroresReportados AS ErroresReportados

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Estado_GO2]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-02-02
-- Description:	SP para validaciones del resgistro de auditoria cuando el estado es glosa objectada 2
-- =============================================
CREATE PROCEDURE [dbo].[SP_Validacion_Estado_GO2]
	
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

		-- Llama SP que valida si hay motivos sin registrar
		EXEC	@SolicitarMotivo = [dbo].[SP_Validacion_Registrar_Motivo]
		@IdRegistroAuditoria = @RegistroAuditoriaId


		--Variables
		DECLARE @RoleId INT = CAST((SELECT TOP(1) RoleId FROM AspNetUserRoles WHERE UserId = @IdUsuario) AS INT);


			-- Validación Lider
			IF @RoleId = 2 -- Lider
			
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
			
			ELSE IF  @RoleId = 3 AND @CountLevantarGlosa > 0 -- Auditor y glosas levantadas
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
					WHERE RegistrosAuditoriaId = @RegistroAuditoriaId)
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
		@ErroresReportados AS ErroresReportados

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Estado_GRE]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-02-14
-- Description:	SP para validaciones del resgistro de auditoria cuando el estado es glosa revision por la entidad GRE
-- =============================================
CREATE PROCEDURE [dbo].[SP_Validacion_Estado_GRE]
	
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
		@ErroresReportados AS ErroresReportados
END
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Estado_RN]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-02-02
-- Description:	SP para validaciones del resgistro de auditoria cuando el estado es registro nuevo
-- =============================================
CREATE PROCEDURE [dbo].[SP_Validacion_Estado_RN]
	
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
			OR
			-- Calificacion Obligatoria para variable IPS
			((SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rd
			INNER JOIN RegistrosAuditoria ra ON ra.Id = rd.RegistrosAuditoriaId
			INNER JOIN VariableXMedicion vm ON rd.VariableId = vm.VariableId AND vm.Encuesta = 1 AND ra.IdMedicion = vm.MedicionId
			WHERE RegistrosAuditoriaId = @RegistroAuditoriaId)
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
		  WHERE rg.RegistrosAuditoriaId = @RegistroAuditoriaId 
			AND vm.SubGrupoId = @IdItemGlosa -- Glosa
			AND rg.Dato_DC_NC_ND <> @IdItemDC -- DC
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
							SET @TipificacionObservacionDefault  = (SELECT Id FROM Item WHERE ItemName = 'GENERAL' and CatalogId = 1); -- Item tipificacion General

						END
						ELSE 
						BEGIN
							SET @ErroresReportados = 0
							SET @CalificacionObligatoriaIPS = 0

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
		@ErroresReportados AS ErroresReportados

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Observacion_Registrada]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar - IT SENSE
-- Create date: 2022-02-15
-- Description:	SP para valida si se registro observacion segun la tipificacion
-- =============================================
CREATE PROCEDURE [dbo].[SP_Validacion_Observacion_Registrada]
	@IdRegistroAuditoria INT,
	@TipoObservacion INT
AS
BEGIN

				-- Validacion para observacion registrada
				IF (SELECT COUNT(*) FROM RegistrosAuditoriaDetalleSeguimiento
					WHERE RegistroAuditoriaId = @IdRegistroAuditoria 
					AND TipoObservacion = @TipoObservacion
					) > 0

					BEGIN
						RETURN 1
					END
					ELSE 
					BEGIN
						RETURN 0
					END			
						

END
GO
/****** Object:  StoredProcedure [dbo].[SP_Validacion_Registrar_Motivo]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		IT SENSE
-- Create date: 2022-02-15
-- Description:	SP para valida si solicita registrar motivo
-- =============================================
CREATE PROCEDURE [dbo].[SP_Validacion_Registrar_Motivo]
	@IdRegistroAuditoria INT
AS
BEGIN
				DECLARE @IdItemGlosa INT = (SELECT Id FROM Item WHERE ItemName = 'Glosas' and CatalogId = 4);
				DECLARE @IdItemDC INT = (SELECT Id FROM Item WHERE ItemName = 'DC' and CatalogId = 6);
				DECLARE @IdItemNC INT = (SELECT Id FROM Item WHERE ItemName = 'NC' and CatalogId = 6);
				DECLARE @SolicitarMotivo BIT = 0;


				-- Validacion si hay NC o ND en glosa
				DECLARE @CountGlosasNC INT =  (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rg
					INNER JOIN RegistrosAuditoria ra ON rg.RegistrosAuditoriaId = ra.Id
					INNER JOIN VariableXMedicion vm ON vm.VariableId = rg.VariableId AND ra.IdMedicion = vm.MedicionId
				  WHERE rg.RegistrosAuditoriaId = @IdRegistroAuditoria 
					AND vm.SubGrupoId = @IdItemGlosa -- Glosa
					AND rg.Dato_DC_NC_ND <> @IdItemDC -- DC
					)

				DECLARE @CountGlosasNCSinMotivo INT = 0;
				DECLARE @CountoOtroGrupoNCSinMotivo  INT = 0;

				-- Conte glossas no conformes NC sin motivo registrado
				SET @CountGlosasNCSinMotivo  = (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rad
																INNER JOIN VariableXMedicion vm
																ON rad.VariableId = vm.VariableId
																AND vm.SubGrupoId = @IdItemGlosa
																AND rad.RegistrosAuditoriaId = @IdRegistroAuditoria
																INNER JOIN RegistrosAuditoria ra
																ON rad.RegistrosAuditoriaId = ra.Id
																AND ra.IdMedicion = vm.MedicionId
																AND rad.Dato_DC_NC_ND = @IdItemNC
																AND (rad.MotivoVariable = '' OR rad.MotivoVariable = NULL))



				-- ConteO grupos diferentes a glosas, no conformes NC sin motivo registrado
				SET @CountoOtroGrupoNCSinMotivo  = (SELECT COUNT(*) FROM RegistrosAuditoriaDetalle rad
																INNER JOIN VariableXMedicion vm
																ON rad.VariableId = vm.VariableId
																AND vm.SubGrupoId <> @IdItemGlosa
																AND rad.RegistrosAuditoriaId = @IdRegistroAuditoria
																INNER JOIN RegistrosAuditoria ra
																ON rad.RegistrosAuditoriaId = ra.Id
																AND ra.IdMedicion = vm.MedicionId
																AND rad.Dato_DC_NC_ND = @IdItemNC
																AND (rad.MotivoVariable = '' OR rad.MotivoVariable = NULL))

				-- Validacion solicitar motivo
				IF ((@CountGlosasNCSinMotivo > 0) OR (@CountGlosasNC = 0 AND @CountoOtroGrupoNCSinMotivo > 0))
					BEGIN
						SET  @SolicitarMotivo = 1;
						RETURN 1
					END

				ELSE
					BEGIN
						RETURN 0
					END
		

END
GO
/****** Object:  StoredProcedure [dbo].[Update_User]    Script Date: 3/10/2022 8:57:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[Update_User] 
(
@user_id Varchar(150),
@user_name Varchar(150),
@email Varchar(150),
@phone Varchar(150),
@active Varchar(150),
@deleted Varchar(150)
) 
AS 
BEGIN 

update [dbo].[AspNetUsers]
set 
	UserName = @user_name,
	NormalizedUserName = LOWER(@user_name),
	Email = @email,
	NormalizedEmail = LOWER(@email),
	PhoneNumber = @phone,
	Active = 1,
	UserDeleted = 0
where Id = @user_id; 

END
GO
