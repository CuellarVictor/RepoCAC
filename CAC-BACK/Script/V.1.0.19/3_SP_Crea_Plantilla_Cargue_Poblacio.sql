USE AuditCAC_QAInterno
GO
/****** Object:  StoredProcedure [dbo].[SP_Crea_Plantilla_Cargue_Poblacion]    Script Date: 10/08/2022 12:22:43 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar - IT SENSE
-- Create date: 2022-04-14
-- Description:	Crea template para cargue de poblacion de acuerdo a las variables parametrizadas de la medición
-- =============================================
CREATE OR ALTER   PROCEDURE  [dbo].[SP_Crea_Plantilla_Cargue_Poblacion]
	 @IdMedicion INT, @SubGrupoId INT
AS
BEGIN
	
	-- Validamos si la SubGrupoId es 0, consultamos todo.
	IF (@SubGrupoId = 0)
	BEGIN
		SET @SubGrupoId = NULL;
	END

	-- Crea tabla temporal
	DECLARE @TemplateTable TABLE (Id INT IDENTITY(1,1) NOT NULL, Valor NVARCHAR(MAX))


	DECLARE	@EsHepatitis int

	EXEC	@EsHepatitis = [dbo].[SP_Valida_Medicion_Hepatitis]
			@MedicionId = @IdMedicion,
			@RegistroAuditoriaId = 0


	IF @EsHepatitis = 0 OR @SubGrupoId = 105 -- No es heaptitis o es estructura 1 
	BEGIN
			-- Inserta campos base para tabla registro auditoria (Cabecera completa)
			INSERT INTO @TemplateTable
			VALUES('IdRadicado'),
			('idPeriodo_Registro'),
			('Primer_Nombre_Paciente'),
			('Segundo_Nombre_Paciente'),
			('Primer_Apellido_Paciente'),
			('Segundo_Apellido_Paciente'),
			('Sexo_Paciente'),
			('Tipo_Identificacion_Paciente'),
			('Identificacion_Paciente'),
			('Fecha_Nacimiento_Paciente'),
			('IdAuditor'),
			('FechaAsignacion'),
			('Orden'),
			('Encuesta'),
			('EPS_Encargada')
	END

	ELSE

	BEGIN
			-- Inserta campos base para tabla registro auditoria (Cabecera para hepatitis diferente a estructura 1)
			INSERT INTO @TemplateTable
			VALUES('IdRadicado'),
			('idPeriodo_Registro'),
			('Tipo_Identificacion_Paciente'),
			('Identificacion_Paciente'),
			('EPS_Encargada')
	END




	

	-- Inserta nemonicos de variables parametrizadas
	INSERT INTO @TemplateTable
		SELECT 
			[dbo].[LimpiarCaracteres](v.nemonico)
		FROM VariableXMedicion vm
			INNER JOIN Variables v ON vm.VariableId = v.Id
									AND v.TipoVariableItem <> 37 -- Adicional
		WHERE vm.MedicionId = @IdMedicion AND vm.SubGrupoId = ISNULL(@SubGrupoId, vm.SubGrupoId)
			ORDER BY vm.Orden
	-- Consulta tabla temporal
	SELECT Id, Valor FROM @TemplateTable


END
