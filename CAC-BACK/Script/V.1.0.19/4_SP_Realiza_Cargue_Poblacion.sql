USE AuditCAC_QAInterno
GO
/****** Object:  StoredProcedure [dbo].[SP_Realiza_Cargue_Poblacion]    Script Date: 10/08/2022 12:23:53 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER   PROCEDURE  [dbo].[SP_Realiza_Cargue_Poblacion]
	
	@HeaderTable DT_LLave_Valor READONLY, 
	@Lines DT_LLave_Valor READONLY,
	@IdMedicion INT,
	@User NVARCHAR(MAX),
	@CurrentProcessId INT,
    @ResultCurrentProcess NVARCHAR(255),
	@Progreso INT,
	@CantidadColumnas INT

	AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	 -- VARIABLES
	  DECLARE @Line NVARCHAR(MAX) = '' 
	  DECLARE @TotalLines INT = (SELECT COUNT(*) FROM @Lines)
	  DECLARE @PositionTotalLines INT  = (SELECT top(1) Id FROM @Lines order by Id);
	  DECLARE @Separador NVARCHAR(MAX) = (SELECT Valor FROM ParametrosGenerales WHERE Id = 21) -- Separador
	  DECLARE @MessageResult NVARCHAR(MAX) = 'OK'
	  DECLARE @idRadiacdo INT
	  DECLARE @Identificacion NVARCHAR(255)
	  DECLARE @CountLine  INT
	  DECLARE @IdAuditor INT
	  DECLARE @LastIdRegistroAuditoria INT
	  -- Progreso
	  DECLARE @ProgresoCargue NVARCHAR(255) = 'Medicion' + CAST(@IdMedicion AS nvarchar(255)) + ',Progreso,' + CAST(@TotalLines AS nvarchar(255)) 
	  -- Hepatitis
	  DECLARE @ValidacionCabecera INT = 1;
      DECLARE @IsHeapititis INT = 0;
	  DECLARE @ExsisteId INT = 0;
	  
	  -- Limpia tabla variables
	  DELETE FROM [dbo].[CarguePoblacion_TablaVariable]
	  

		-- Inserta variables a validar
		INSERT INTO CarguePoblacion_TablaVariable
		SELECT vr.Id, vr.nemonico  FROM Variables vr
				INNER JOIN VariableXMedicion vxm ON vr.Id = vxm.VariableId
			WHERE MedicionId = @IdMedicion

		
	  WHILE (@PositionTotalLines <= @TotalLines)

		  BEGIN
			
		  	BEGIN TRY  
				BEGIN TRAN
				--Limpia tablas cargue
				DELETE FROM CarguePoblacion_TablaLinea
				DBCC CHECKIDENT (CarguePoblacion_TablaLinea, RESEED, 0)
				DELETE FROM CarguePoblacion_TablaUnion
				DELETE FROM CarguePoblacion_TablaUnionFiltrada
				DBCC CHECKIDENT (CarguePoblacion_TablaUnionFiltrada, RESEED, 0)
				
				-- Setea Insert cabecera.
				SET @ValidacionCabecera = 1;

			   --Actualiza porceso actual
			   UPDATE Current_Process SET Result = REPLACE(@ProgresoCargue, 'Progreso', CAST(@PositionTotalLines AS nvarchar(255))) WHERE Id = @CurrentProcessId 

				-- Toma linea que esta recorriendo
				SET @Line = (SELECT top(1) Valor FROM @Lines WHERE Id = @PositionTotalLines)

				-- Inserta registro separado
				INSERT INTO CarguePoblacion_TablaLinea SELECT CAST(value As VARCHAR(MAX)) FROM STRING_SPLIT_CUSTOM(@line, @Separador)

				SET @CountLine = (SELECT COUNT(*) FROM CarguePoblacion_TablaLinea) 

				-- Inserta union de registro registro con cabecera
				INSERT INTO  CarguePoblacion_TablaUnion
						SELECT  h.Id, vr.VariableId, h.Valor, l.Valor, vr.nemonico  FROM @HeaderTable h 
								INNER JOIN CarguePoblacion_TablaLinea l ON h.Id = l.Id
								LEFT JOIN CarguePoblacion_TablaVariable vr ON h.Valor = vr.nemonico 


				SET @IdAuditor = CAST((select Valor FROM CarguePoblacion_TablaUnion WHERE RTRIM(LTRIM(header)) = 'IdAuditor') AS INT)


				-- Filtra cabecera con registro
				INSERT INTO  CarguePoblacion_TablaUnionFiltrada
						SELECT  vr.Id, h.Valor, l.Valor, vr.nemonico , vm.EsGlosa, vm.EsVisible, vm.EsCalificable, vm.SubGrupoId, vm.CalificacionXDefecto, vm.MedicionId FROM @HeaderTable h 
								INNER JOIN CarguePoblacion_TablaLinea l ON h.Id = l.Id
								INNER JOIN Variables vr ON h.Valor = vr.nemonico 
								INNER JOIN VariableXMedicion vm ON vr.Id = vm.VariableId    AND vm.MedicionId = @IdMedicion
						WHERE vr.idVariable IS NOT NULL  AND vm.MedicionId = @IdMedicion
				

								--Inicia validaciones
								IF (@CountLine <> @CantidadColumnas)
								BEGIN
										 INSERT INTO [dbo].[Resultado_Cargue_Poblacion]([CurrentProcessId],[IdRadicado],[Tipo],[Result],[CreateDate])
												 VALUES
													   (@CurrentProcessId
													   ,@idRadiacdo
													   ,'ERROR'
													   ,'El registro  cuenta con ' +  CAST(@CountLine AS nvarchar(255))  + ' columnas, las necesarias son ' + CAST(@CantidadColumnas AS nvarchar(255)) 
													   ,GETDATE())
								END
								ELSE
								BEGIN

									SET @idRadiacdo =  (SELECT TOP(1) CAST(Valor AS INT) FROM CarguePoblacion_TablaUnion where RTRIM(LTRIM(header)) = 'IdRadicado')
									SET @Identificacion = (SELECT TOP(1) Valor FROM CarguePoblacion_TablaUnion WHERE RTRIM(LTRIM(header)) = 'Identificacion')


										-- Validacion si existe radicado
										IF ( (SELECT COUNT(*) FROM [RegistrosAuditoria] WITH (NOLOCK) WHERE IdRadicado = @idRadiacdo AND [Status] = 1) > 0)
										BEGIN


												 INSERT INTO [dbo].[Resultado_Cargue_Poblacion]([CurrentProcessId],[IdRadicado],[Tipo],[Result],[CreateDate])
												 VALUES
													   (@CurrentProcessId
													   ,@idRadiacdo
													   ,'ERROR'
													   ,'Ya se encuentra agregado el ID de Radicado ' + CAST(@idRadiacdo AS nvarchar(255)) 
													   ,GETDATE())


										END

										-- Validacion campo vacio
										ELSE IF ( (SELECT COUNT(*) FROM CarguePoblacion_TablaUnion WHERE Valor IS NULL OR REPLACE(REPLACE(Valor,'	',''),' ','') = '') > 0) -- Validamos que Valor (DatoReportado) no sea Null o Vacio.
										BEGIN
											INSERT INTO [dbo].[Resultado_Cargue_Poblacion]([CurrentProcessId],[IdRadicado],[Tipo],[Result],[CreateDate])
												VALUES
													(@CurrentProcessId
													,@idRadiacdo
													,'ERROR'
													,'El DatoReportado no debe ser vacio o nulo ' + (SELECT TOP 1 header FROM CarguePoblacion_TablaUnion WHERE Valor IS NULL OR Valor = '')  
													,GETDATE())
										END
										--ELSE IF ( (SELECT COUNT(*) FROM [RegistrosAuditoria] WHERE Identificacion = @Identificacion and IdMedicion = @IdMedicion) > 0)
										--BEGIN
										--		COMMIT TRAN
										--		SELECT @idRadiacdo AS id, 'ERROR Ya se encuentrael paciente con documento ' + @Identificacion + ' a la medicion ' + CAST(@IdMedicion AS nvarchar(255)) AS Result

										--END 

										--Validacion existe parametrizacion de variable
										ELSE IF ( (SELECT COUNT(*) FROM CarguePoblacion_TablaUnionFiltrada WHERE  Idmedicion = @IdMedicion) = 0)
										BEGIN
												INSERT INTO [dbo].[Resultado_Cargue_Poblacion]([CurrentProcessId],[IdRadicado],[Tipo],[Result],[CreateDate])
												 VALUES
													   (@CurrentProcessId
													   ,@idRadiacdo
													   ,'ERROR'
													   ,'No se encuentra parametrizacion de las variables para la medicion ' + CAST(@IdMedicion AS nvarchar(255)) + ' con un nemonico valido '
													   ,GETDATE())

										END
										-- Validacion Codigo Auditor Valido
										ELSE IF ( (SELECT COUNT(*) FROM AUTH.Usuario where Codigo = @IdAuditor) = 0 AND (SELECT COUNT(*) FROM CarguePoblacion_TablaUnion WHERE RTRIM(LTRIM(header)) = 'IdAuditor') > 0)
										BEGIN
												INSERT INTO [dbo].[Resultado_Cargue_Poblacion]([CurrentProcessId],[IdRadicado],[Tipo],[Result],[CreateDate])
												 VALUES
													   (@CurrentProcessId
													   ,@idRadiacdo
													   ,'ERROR'
													   , 'ERROR El codigo de auditor ' + CAST(@IdAuditor AS nvarchar(255)) +  ' no es valido '
													   ,GETDATE())

										END
									ELSE
										BEGIN

											-- Validamos si es hepatitis.
											EXEC	@IsHeapititis = [dbo].[SP_Valida_Medicion_Hepatitis]
													@MedicionId = @IdMedicion,
													@RegistroAuditoriaId = 0

											-- Consulta subgrupo de la ultima variable del registro
											DECLARE @Subgrupo INT = (SELECT TOP(1) SubGrupoId FROM CarguePoblacion_TablaUnionFiltrada ORDER BY Id DESC)
											DECLARE @NombreSubgrupo NVARCHAR(255) = (SELECT TOP(1) ItemName FROM Item WHERE Id = @Subgrupo)



											-- SI Es Hepatitis.
											IF(@IsHeapititis = 1)
											BEGIN

												-- SI ES Hepatitis.
												-- Consultar si ya existe registro del paciente con cedula, tipo, eps, idperiodo, meidicion (RegistroAuditoria)

												DECLARE @TipoIdentificacion VARCHAR(2) = (SELECT TOP(1) CAST(Valor AS VARCHAR(2)) FROM CarguePoblacion_TablaUnion WHERE RTRIM(LTRIM(header)) = 'Tipo_Identificacion_Paciente');
												DECLARE @IdentificacionPaciente VARCHAR(250) = (SELECT TOP(1) Valor FROM CarguePoblacion_TablaUnion WHERE RTRIM(LTRIM(header)) = 'Identificacion_Paciente');
												DECLARE @EPS VARCHAR(10) = (SELECT TOP(1) CAST(Valor AS VARCHAR(30)) FROM CarguePoblacion_TablaUnion WHERE RTRIM(LTRIM(header)) = 'EPS_Encargada');
												DECLARE @IdPeriodo INT = (SELECT TOP(1) CAST(Valor AS INT) FROM CarguePoblacion_TablaUnion where RTRIM(LTRIM(header)) = 'idPeriodo_Registro');			

												SET @ExsisteId = ( SELECT TOP 1 (Id) FROM RegistrosAuditoria RA WHERE RA.TipoIdentificacion = @TipoIdentificacion AND RA.Identificacion = @IdentificacionPaciente AND RA.IdEPS = @EPS AND RA.IdPeriodo = @IdPeriodo AND IdMedicion = @IdMedicion );
												IF(@ExsisteId IS NOT NULL)
												BEGIN
													--captura y lo asigna a lastid
													SET @LastIdRegistroAuditoria = @ExsisteId;
													SET @ValidacionCabecera = 0;
												END -- END Begin: IF(@ExsisteId IS NOT NULL)
											END -- END Begin: IF(@IsHeapititis > 0)


											-- SI ES HEPATITIS Y NO EXISTE CABECERA Y ES DIFERENTE A ESTRUCTURA 1
											IF(@IsHeapititis = 1 AND  @ValidacionCabecera = 1 AND @Subgrupo <> 105)
											BEGIN
												

													INSERT INTO [dbo].[Resultado_Cargue_Poblacion]([CurrentProcessId],[IdRadicado],[Tipo],[Result],[CreateDate])
													 VALUES
														   (@CurrentProcessId
														   ,@idRadiacdo
														   ,'ERROR'
														   ,'NO se ha encontrado informacion de Estructura 1 para este registro, estructura del registro: ' + CAST(@NombreSubgrupo AS nvarchar(255)) 
														   ,GETDATE())

											END
											ELSE
											BEGIN

													--No Existe Cabecera
													IF(@ValidacionCabecera = 1)
													BEGIN
														-- INSERTA REGISTRO AUDITORIA 
														INSERT INTO [dbo].[RegistrosAuditoria]
																	([IdRadicado],[IdMedicion],[IdPeriodo],[IdAuditor],[PrimerNombre],[SegundoNombre],[PrimerApellido],[SegundoApellido],[Sexo],[TipoIdentificacion],[Identificacion],[FechaNacimiento],[FechaCreacion],[FechaAuditoria],[FechaMinConsultaAudit]
																	,[FechaMaxConsultaAudit],[FechaAsignacion],[Activo],[Conclusion],[UrlSoportes],[Reverse],[DisplayOrder],[Ara],[Eps],[FechaReverso],[AraAtendido],[EpsAtendido],[Revisar],[Extemporaneo],[Estado]
																	,[LevantarGlosa],[MantenerCalificacion],[ComiteExperto],[ComiteAdministrativo],[AccionLider],[AccionAuditor],[Encuesta],[CreatedBy],[CreatedDate],[ModifyBy],[ModifyDate],[IdEPS],[Status])
																VALUES
																	( @idRadiacdo --(SELECT TOP(1) CAST(Valor AS INT) FROM @UnionTable where RTRIM(LTRIM(header)) = 'IdRadicado')
																	, @IdMedicion
																	,(SELECT TOP(1) CAST(Valor AS INT) FROM CarguePoblacion_TablaUnion where RTRIM(LTRIM(header)) = 'idPeriodo_Registro')
																	,(SELECT TOP(1) Id FROM AUTH.Usuario where Codigo = CAST((select Valor FROM CarguePoblacion_TablaUnion WHERE RTRIM(LTRIM(header)) = 'IdAuditor') AS INT))
																	,(SELECT TOP(1) CAST(Valor AS VARCHAR(50)) FROM CarguePoblacion_TablaUnion WHERE RTRIM(LTRIM(header)) = 'Primer_Nombre_Paciente')
																	,(SELECT TOP(1) CAST(Valor AS VARCHAR(50)) FROM CarguePoblacion_TablaUnion WHERE RTRIM(LTRIM(header)) = 'Segundo_Nombre_Paciente')
																	,(SELECT TOP(1) CAST(Valor AS VARCHAR(50)) FROM CarguePoblacion_TablaUnion WHERE RTRIM(LTRIM(header)) = 'Primer_Apellido_Paciente')
																	,(SELECT TOP(1) CAST(Valor AS VARCHAR(50)) FROM CarguePoblacion_TablaUnion WHERE RTRIM(LTRIM(header)) = 'Segundo_Apellido_Paciente')
																	,(SELECT TOP(1) CAST(Valor AS VARCHAR(50)) FROM CarguePoblacion_TablaUnion WHERE RTRIM(LTRIM(header)) = 'Sexo_Paciente')
																	,(SELECT TOP(1) CAST(Valor AS VARCHAR(2)) FROM CarguePoblacion_TablaUnion WHERE RTRIM(LTRIM(header)) = 'Tipo_Identificacion_Paciente')
																	,(SELECT TOP(1) Valor FROM CarguePoblacion_TablaUnion WHERE RTRIM(LTRIM(header)) = 'Identificacion_Paciente')
																	,(SELECT TOP(1) CAST(Valor AS DATE) FROM CarguePoblacion_TablaUnion WHERE RTRIM(LTRIM(header)) = 'Fecha_Nacimiento_Paciente')
																	,GETDATE()
																	,GETDATE() -- (SELECT TOP(1) CAST(Valor AS DATETIME) FROM @UnionTable WHERE RTRIM(LTRIM(header)) = 'FechaAuditoria')
																	,NULL --  (SELECT TOP(1) CAST(Valor AS DATETIME) FROM @UnionTable WHERE RTRIM(LTRIM(header)) = 'FechaMinConsultaAudit')
																	,NULL --  (SELECT TOP(1) CAST(Valor AS DATETIME) FROM @UnionTable WHERE RTRIM(LTRIM(header)) = 'FechaMaxConsultaAudit') 
																	,(SELECT TOP(1) CAST(Valor AS DATE) FROM CarguePoblacion_TablaUnion WHERE RTRIM(LTRIM(header)) = 'FechaAsignacion') 
																	,1
																	,1 -- ToDo Validar
																	,''
																	,0
																	,(SELECT TOP(1) CAST(Valor AS INT) FROM CarguePoblacion_TablaUnion WHERE RTRIM(LTRIM(header)) = 'Orden') 
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
																	,(SELECT TOP(1) CAST(Valor AS INT) FROM CarguePoblacion_TablaUnion WHERE RTRIM(LTRIM(header)) = 'Encuesta') 
																	,@User
																	,GETDATE()
																	,@User
																	,GETDATE()
																	,(SELECT TOP(1) CAST(Valor AS VARCHAR(30)) FROM CarguePoblacion_TablaUnion WHERE RTRIM(LTRIM(header)) = 'EPS_Encargada') 
																	,1)

																	--DECLARE @LastIdRegistroAuditoria INT =  (SELECT SCOPE_IDENTITY());
																	SET @LastIdRegistroAuditoria =  (SELECT SCOPE_IDENTITY());
													END -- END Begin: IF(@ValidacionCabecera = 1)																					


													--INSERTAR DETALLE REGISTRO AUDITORIA DETALLE
													DECLARE @position INT = 1;
													DECLARE @total INT = (SELECT TOP(1) Id FROM CarguePoblacion_TablaUnionFiltrada ORDER BY Id DESC);

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
																	@User, -- USUARIO (Crear) ToDo Validar
																	GETDATE(),
																	@User, -- USUARIO (Crear) ToDo Validar
																	GETDATE(),
																	EsGlosa,
																	Visible,
																	Calificable,
																	SubGrupoId,
																	Valor,
																	NULL,
																	CalificacionDefecto,
																	0,
																	1
									
															FROM  CarguePoblacion_TablaUnionFiltrada WHERE Id = @position 
															AND Valor IS NOT NULL AND Valor != '' -- Que Valor (DatoReportado) no sea Null o Vacio
						
															SET @position = @position + 1

													END

													-- INSERTA VARIABLES ADICIONALES.									 
													INSERT INTO [dbo].[RegistrosAuditoriaDetalle]
																([VariableId],[RegistrosAuditoriaId],[EstadoVariableId],[Observacion],[Activo],[CreatedBy],[CreatedDate],[ModifyBy],[ModifyDate],[EsGlosa],[Visible]
																,[Calificable],[SubgrupoId],[DatoReportado],[MotivoVariable],[Dato_DC_NC_ND],[Ara],[Status])
													SELECT 
														VA.Id, 
														@LastIdRegistroAuditoria, --RegistroAuditoriaId
														1,
														0,
														1,
														@User,
														GETDATE(),
														@User, -- USUARIO (Crear) ToDo Validar
														GETDATE(),
														VAME.EsGlosa,	
														VAME.EsVisible,
														VAME.EsCalificable,
														VAME.SubGrupoId,
														NULL, --DatoReportado
														NULL, --MotivoVariable
														VAME.CalificacionXDefecto,
														0,
														1
													FROM Variables VA
														INNER JOIN VariableXMedicion VAME ON (VA.Id = VAME.VariableId)
														LEFT JOIN RegistrosAuditoriaDetalle RAD ON (RAD.VariableId = VA.Id AND RAD.RegistrosAuditoriaId = @LastIdRegistroAuditoria)

													WHERE VAME.MedicionId = @IdMedicion
														AND RAD.VariableId IS NULL
														AND VA.TipoVariableItem = 37 -- Adiconal;
													-- //

												   
													SET @MessageResult = CAST(@idRadiacdo AS nvarchar(255)) + ', Registro agregado correctamente'

													-- Inserta resultado
													INSERT INTO [dbo].[Resultado_Cargue_Poblacion]([CurrentProcessId],[IdRadicado],[Tipo],[Result],[CreateDate])
														VALUES
															(@CurrentProcessId
															,@idRadiacdo --(SELECT TOP(1) CAST(Valor AS INT) FROM @UnionTable where RTRIM(LTRIM(header)) = 'IdRadicado') 
															,'OK'
															, @MessageResult
															,GETDATE())
											END
													
											

										END -- END Else Begin: Validaciones Ok.

								END -- END Else Begin: Inicia validaciones
								


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
							set @MessageResult = CAST(@idRadiacdo AS nvarchar(255)) + + ', ' + ERROR_MESSAGE() + '- SP LINEA: ' + CAST(ERROR_LINE() AS nvarchar(255));

							-- Inserta resultado
							INSERT INTO [dbo].[Resultado_Cargue_Poblacion]([CurrentProcessId],[IdRadicado],[Tipo],[Result],[CreateDate])
										 VALUES
											  (@CurrentProcessId
											  , @idRadiacdo 
											  ,'ERROR'
											  ,@MessageResult
											  ,GETDATE())

						END CATCH


				SET @PositionTotalLines = @PositionTotalLines + 1
				

		  END

	
	-- Insertamos Log de operacion. 
	EXEC SP_Insertar_Process_Log 4, @User, 'OK', 'Gestor Mediciones: Cargue población'; 



	END
