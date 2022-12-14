USE [AuditCAC_Development]
GO
/****** Object:  StoredProcedure [dbo].[getEnfermedades]    Script Date: 7/25/2022 9:46:02 AM ******/
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
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Registros_Auditar_Medicion]    Script Date: 7/25/2022 9:46:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar - IT SENSE
-- Create date: 2022-06-08
-- Description:	Consulta registros por medicion para modelo mongo
-- =============================================
CREATE PROCEDURE [dbo].[SP_Consulta_Registros_Auditar_Medicion]
	@IdMedicion INT
AS
BEGIN
	
	SELECT 
		RA.Id, 
		RA.IdRadicado, 
		RA.IdMedicion,
		RA.CreatedBy,
		RA.CreatedDate,
		RA.ModifyBy,
		RA.ModifyDate,
		ME.Estado, 
		IT.ItemName,
		RAD.DatoReportado, 
		RAD.MotivoVariable, 
		RAD.Dato_DC_NC_ND, 
		RAD.VariableId,
		VA.idVariable, 
		VA.nombre As VariableNombre,
		ME.Nombre As MedicionNombre, 
		ME.Descripcion As MedicionDescripcion, 
		ME.IdCobertura As MedicionIdCobertura,
		ME.FechaFinAuditoria AS FechaFinAuditoria,
		EF.nombre As CoberturaNombre, 
		EF.nemonico As CoberturaNemonico, 
		EF.legislacion As CoberturaLegislacion, 
		EF.definicion As CoberturaDefinicion,
		RA.IdEPS,
		IT.ItemName AS EstadoNombre,
		esr.Codigo AS CodigoEstado,
		esr.Descripción AS DescripcionEstado,
		VA.TipoVariableItem,
		VA.nemonico AS NemonicoVariable,

		-- Regimen Id
		(SELECT TOP(1)

			CAST(cid.Valor AS INT)

		FROM CatalogoItemCobertura_Detalle cid 
		INNER JOIN Item itRegimen ON CAST(itRegimen.Id AS NVARCHAR(MAX)) = cid.Valor
		INNER JOIN CatalogoItemCobertura ci ON ci.Id = cid.CatalogoItemCoberturaId
		INNER JOIN CatalogoCobertura co ON co.Id = ci.CatalogoCoberturaId
		WHERE 
		co.Id = 29 -- Catalogo EPS
		AND cid.ItemDato = 125 -- Regimen
		AND ci.ItemId = RA.IdEPS

		)  AS RegimenEPS_Id,

		-- Regimen Nombre
		(SELECT TOP(1)

			itRegimen.ItemName

		FROM CatalogoItemCobertura_Detalle cid 
		INNER JOIN Item itRegimen ON CAST(itRegimen.Id AS NVARCHAR(MAX)) = cid.Valor
		INNER JOIN CatalogoItemCobertura ci ON ci.Id = cid.CatalogoItemCoberturaId
		INNER JOIN CatalogoCobertura co ON co.Id = ci.CatalogoCoberturaId
		WHERE 
		co.Id = 29 -- Catalogo EPS
		AND cid.ItemDato = 125 -- Regimen
		AND ci.ItemId = RA.IdEPS

		)  AS RegimenEPS_Nombre,

		-- Renglon Id
		(SELECT TOP(1)

			CAST(cid.Valor AS INT)

		FROM CatalogoItemCobertura_Detalle cid 
		INNER JOIN Item itRegimen ON CAST(itRegimen.Id AS NVARCHAR(MAX)) = cid.Valor
		INNER JOIN CatalogoItemCobertura ci ON ci.Id = cid.CatalogoItemCoberturaId
		INNER JOIN CatalogoCobertura co ON co.Id = ci.CatalogoCoberturaId
		WHERE 
		co.Id = 29 -- Catalogo EPS
		AND cid.ItemDato = 126 -- Renglon
		AND ci.ItemId = RA.IdEPS

		)  AS RenglonEPS_Id,

		-- Renglon nombre
		(SELECT TOP(1)

			itRegimen.ItemName

		FROM CatalogoItemCobertura_Detalle cid 
		INNER JOIN Item itRegimen ON CAST(itRegimen.Id AS NVARCHAR(MAX)) = cid.Valor
		INNER JOIN CatalogoItemCobertura ci ON ci.Id = cid.CatalogoItemCoberturaId
		INNER JOIN CatalogoCobertura co ON co.Id = ci.CatalogoCoberturaId
		WHERE 
		co.Id = 29 -- Catalogo EPS
		AND cid.ItemDato = 126 -- Renglon
		AND ci.ItemId = RA.IdEPS

		)  AS RenglonEPS_Nombre,

		RA.Identificacion,
		RA.TipoIdentificacion




	FROM RegistrosAuditoria RA WITH (NOLOCK) 
		INNER JOIN RegistrosAuditoriaDetalle RAD ON (RA.Id = RAD.RegistrosAuditoriaId)
		INNER JOIN Medicion ME ON (RA.IdMedicion = ME.Id) 
		INNER JOIN Variables VA ON (RAD.VariableId = VA.Id) 

		INNER JOIN Enfermedad EF ON (ME.IdCobertura = EF.idCobertura)
		INNER JOIN Item IT ON (ME.Estado = IT.Id)
		INNER JOIN Item ITestador ON (RA.Estado = ITestador.Id)
		INNER JOIN EstadosRegistroAuditoria esr ON esr.Id = RA.Estado
	WHERE 
	    --ME.Estado = 28 -- Estado En curso (Id estado: 28).
		--AND ME.FechaFinAuditoria >= DATEADD(WEEK, -1, GETDATE()) -- Auditorias que finalizaron hace menos de un mes
		 ME.Id = @IdMedicion




END
GO
/****** Object:  StoredProcedure [dbo].[SP_Consulta_Registros_Seguimiento_Medicion]    Script Date: 7/25/2022 9:46:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Victor Cuellar - IT SENSE
-- Create date: 2022-06-08
-- Description:	Consulta observaciones de registros a auditar por medicion para modelo mongo
-- =============================================
CREATE PROCEDURE [dbo].[SP_Consulta_Registros_Seguimiento_Medicion]
	
	@IdMedicion INT
AS
BEGIN
	
		SELECT ras.Id
			  ,ras.RegistroAuditoriaId
			  ,ras.TipoObservacion
			  ,ras.Observacion
			  ,ras.Soporte
			  ,ras.EstadoActual
			  ,ras.EstadoNuevo
			  ,ras.CreatedBy
			  ,ras.CreatedDate
			  ,ras.ModifyBy
			  ,ras.ModifyDate
			  ,ras.Status
		  FROM dbo.RegistrosAuditoriaDetalleSeguimiento ras  WITH (NOLOCK) 
		  INNER JOIN RegistrosAuditoria ra ON ras.RegistroAuditoriaId = ra.Id
		  WHERE ra.IdMedicion = @IdMedicion

END
GO
/****** Object:  StoredProcedure [dbo].[Sp_IdsMedicion]    Script Date: 7/25/2022 9:46:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_IdsMedicion] 
	-- Add the parameters for the stored procedure here	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Id
	FROM Medicion
	WHERE Estado = 28 
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_ObtenerColumnas]    Script Date: 7/25/2022 9:46:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_ObtenerColumnas]
	-- Add the parameters for the stored procedure here
	@idRegistroAuditoria INT = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT
    SUBSTRING(
        (
            SELECT ',['+ va.nombre +']' AS [text()]
            FROM [dbo].[RegistrosAuditoriaDetalle] rad1
			INNER JOIN [dbo].[Variables] AS va ON rad1.VariableId = va.Id
            WHERE rad1.RegistrosAuditoriaId = rad2.RegistrosAuditoriaId
            ORDER BY va.Id
            FOR XML PATH (''), TYPE
        ).value('text()[1]','NVARCHAR(MAX)'), 2, 10000) [VariableId]
	FROM [dbo].[RegistrosAuditoriaDetalle] rad2
	WHERE RegistrosAuditoriaId = @idRegistroAuditoria;
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_ObtenerIdsRegistrosAuditoria]    Script Date: 7/25/2022 9:46:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_ObtenerIdsRegistrosAuditoria]
	-- Add the parameters for the stored procedure here
	@idMedicion INT = null,
	@fecha NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ra.[Id]
      ,[IdRadicado]
      ,[IdMedicion]
      ,ra.[Estado]
	  ,era.Nombre
	  ,era.Codigo
	  ,era.Descripción
	  ,ra.IdEPS
	  ,med.Nombre
	  ,ra.ModifyDate
  FROM [RegistrosAuditoria] AS ra
  LEFT JOIN Medicion AS med ON ra.IdMedicion = med.Id
  LEFT JOIN EstadosRegistroAuditoria AS era ON ra.Estado = era.Id
  LEFT JOIN RegistrosAuditoriaDetalle AS rad ON rad.RegistrosAuditoriaId =ra.Id
  WHERE ra.IdMedicion = @idMedicion  AND
  (rad.ModifyDate is not null AND rad.ModifyDate >= @fecha OR
   rad.CreatedDate >= @fecha  )
     GROUP BY ra.[Id]
      ,[IdRadicado]
      ,[IdMedicion]
	  ,med.Nombre
      ,ra.[Estado]
	  ,era.Nombre
	  ,era.Codigo
	  ,era.Descripción
	  ,ra.IdEPS
	  ,ra.ModifyDate
  ORDER BY ra.Id
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_ObtenerObservaciones]    Script Date: 7/25/2022 9:46:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_ObtenerObservaciones]
	-- Add the parameters for the stored procedure here
	@idRegistroAuditoria INT = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT
    SUBSTRING(
        (
            SELECT Observacion+'->' AS [text()]
            FROM [dbo].[RegistrosAuditoriaDetalleSeguimiento] 
			WHERE RegistroAuditoriaId = @idRegistroAuditoria
            FOR XML PATH (''), TYPE
        ).value('text()[1]','NVARCHAR(MAX)'), 0, 10000) Observaciones
	FROM [dbo].[RegistrosAuditoriaDetalleSeguimiento] 
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ObtenerParametrosGeneralesByName]    Script Date: 7/25/2022 9:46:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
CREATE PROCEDURE [dbo].[SP_ObtenerParametrosGeneralesByName]
	-- Add the parameters for the stored procedure here
	@nombre NVARCHAR(MAX) 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT valor 
	FROM [dbo].[ParametrosGenerales]
	WHERE Nombre = @nombre 
END
GO
/****** Object:  StoredProcedure [dbo].[Sp_ObtenerValoresVariables]    Script Date: 7/25/2022 9:46:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sp_ObtenerValoresVariables] 
	-- Add the parameters for the stored procedure here
	@idRegistroAuditoria INT = null,
	@variables NVARCHAR(MAX) = null,
	@columna VARCHAR(50),
	@tabla VARCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @sql NVARCHAR(MAX)

	SET @sql = ' SELECT 	
	'+@variables+'
	FROM  
(
  SELECT va.nombre, '+@tabla+@columna+'   
  FROM [dbo].[RegistrosAuditoriaDetalle] AS rad
  INNER JOIN [dbo].[Variables] AS va ON rad.VariableId = va.Id
  WHERE rad.RegistrosAuditoriaId = '+ CONVERT(NVARCHAR(50),@idRegistroAuditoria)  +'
) AS SourceTable  
PIVOT  
(  
  MAX ('+@columna+')
  FOR nombre IN ('+@variables+')  
) AS PivotTable '

    -- Insert statements for procedure here
	EXEC sp_executesql  @sql
END
GO
