USE AuditCAC_QAInterno
GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Cantidad_Campos_Cargue_Poblacion]    Script Date: 10/08/2022 12:21:28 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar - IT SENSE
-- Create date: 2022-04-14
-- Description:	Consulta la cantidad de campos que debe tener el archivo de cargue
-- =============================================
CREATE OR  ALTER  PROCEDURE  [dbo].[SP_Consulta_Cantidad_Campos_Cargue_Poblacion]
	 @IdMedicion INT, @EstructuraVariable INT -- @EstructuraVariable = @SubGrupoId
AS
BEGIN

	DECLARE	@EsHepatitis int 
	DECLARE	@CantidadCamposBase int

	EXEC	@EsHepatitis = [dbo].[SP_Valida_Medicion_Hepatitis]
			@MedicionId = @IdMedicion,
			@RegistroAuditoriaId = 0


	IF @EsHepatitis = 0 OR @EstructuraVariable = 105 -- No es heaptitis o es estructura 1 
	BEGIN
			-- (Cabecera completa)
			SET @CantidadCamposBase = 15 -- Detalle => SP_Crea_Plantilla_Cargue_Poblacion
	END

	ELSE

	BEGIN
			-- (Cabecera para hepatitis diferente a estructura 1)
			SET @CantidadCamposBase = 5
	END
	
	-- Validamos si la SubGrupoId es 0, consultamos todo.
	IF (@EstructuraVariable = 0) 
	BEGIN
		SET @EstructuraVariable = NULL;
	END

	DECLARE @Total INT =
	(SELECT 
			COUNT(*)
		FROM VariableXMedicion vm
			INNER JOIN Variables v ON vm.VariableId = v.Id
						AND v.TipoVariableItem <> 37 -- Adiconal
		WHERE vm.MedicionId = @IdMedicion AND vm.SubGrupoId = ISNULL(@EstructuraVariable, vm.SubGrupoId)) -- @EstructuraVariable = @SubGrupoId
		+
		@CantidadCamposBase -- Campos base

	SELECT 
	@Total AS Id,
	'Total campos cargue' AS Valor

END
