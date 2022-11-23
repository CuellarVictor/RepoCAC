USE [AuditCAC_Development]
GO

-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Eriks Palma
-- Create date: 13/08/2022
-- Description:	Procedimiento para obtener los campos requeridos 
-- de RegistroAuditoria para enviar a mongo
-- =============================================
IF OBJECT_ID('[dbo].[Sp_ObtenerIdsRegistrosAuditoria]') IS NOT NULL
    EXEC('DROP PROCEDURE [dbo].[Sp_ObtenerIdsRegistrosAuditoria]')
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
  FROM [AuditCAC_Development].[dbo].[RegistrosAuditoria] AS ra
  LEFT JOIN Medicion AS med ON ra.IdMedicion = med.Id
  LEFT JOIN EstadosRegistroAuditoria AS era ON ra.Estado = era.Id
  LEFT JOIN RegistrosAuditoriaDetalle AS rad ON rad.RegistrosAuditoriaId =ra.Id
  WHERE ra.IdMedicion = @idMedicion  AND
  (rad.ModifyDate is not null AND rad.ModifyDate >= @fecha OR
   rad.CreatedDate >= @fecha  )
     GROUP BY ra.[Id]
      ,[IdRadicado]
      ,[IdMedicion]
      ,ra.[Estado]
	  ,era.Nombre
	  ,era.Codigo
	  ,era.Descripción
  ORDER BY ra.Id
END
GO





USE [AuditCAC_Development]
GO
-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Eriks Palma
-- Create date: 2022-04-21
-- Description:	Sp para obtener parametros generales por nombre
IF OBJECT_ID('[dbo].[SP_ObtenerParametrosGeneralesByName]') IS NOT NULL
    EXEC('DROP PROCEDURE [dbo].[SP_ObtenerParametrosGeneralesByName]')
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





USE [AuditCAC_Development]
GO
-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Eriks Palma 
-- Create date: 21/04/2022
-- Description:	Procedimiento para obtener los nombres de las variables en forma horizontal
-- =============================================
IF OBJECT_ID('[dbo].[Sp_ObtenerObservaciones]') IS NOT NULL
    EXEC('DROP PROCEDURE [dbo].[Sp_ObtenerObservaciones]')
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
