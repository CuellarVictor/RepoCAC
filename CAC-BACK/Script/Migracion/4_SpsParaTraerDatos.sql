USE [CAC]
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
-- Create date: 26/04/2022
-- Description:	Se obtiene la data para migrar Error
-- =============================================
IF OBJECT_ID('[dbo].[SP_GetDataMigError]') IS NOT NULL
    EXEC('DROP PROCEDURE [dbo].[SP_GetDataMigError]')
GO
CREATE PROCEDURE [dbo].[SP_GetDataMigError]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT idError, descripcion, idTipoError
	FROM [dbo].[cacError]
END
GO

----------------------------->>>>>>>>>>>>>>>>------------------------

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
-- Create date: 26/04/2022
-- Description:	Se obtiene la data para migrar Error
-- =============================================
IF OBJECT_ID('[dbo].[SP_GetDataMigPeriodo]') IS NOT NULL
    EXEC('DROP PROCEDURE [dbo].[SP_GetDataMigPeriodo]')
GO
CREATE PROCEDURE [dbo].[SP_GetDataMigPeriodo]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [idPeriodo]
      ,[nombre]
      ,[fechaCorte]
      ,[fechaFinalReporte]
      ,[fechaMaximaCorrecciones]
      ,[fechaMaximaSoportes]
      ,[fechaMaximaConciliaciones]
      ,[idCobertura]
      ,[idPeriodoAnt]
      ,[FechaMinConsultaAudit]
      ,[FechaMaxConsultaAudit]
	FROM [dbo].[cacperiodo]
END
GO

----------------------------->>>>>>>>>>>>>>>>------------------------

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
-- Create date: 26/04/2022
-- Description:	Se obtiene la data para migrar Error
-- =============================================
IF OBJECT_ID('[dbo].[SP_GetDataMigRestriccionesConsistencia]') IS NOT NULL
    EXEC('DROP PROCEDURE [dbo].[SP_GetDataMigRestriccionesConsistencia]')
GO
CREATE PROCEDURE [dbo].[SP_GetDataMigRestriccionesConsistencia]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [idRegla]
      ,[idRestriccionConsistencia]
      ,[idSignoComparacion]
      ,[idCompararCon]
      ,[idVariableComparacion]
      ,[idTipoValorComparacion]
      ,cast([valorEspecifico] as nvarchar(100)) AS valorEspecifico
      ,[idVariableAsociada]
      ,[idSignoComparacionAsociada]
      ,[idCompararConAsociada]
      ,[idVariableComparacionAsociada]
      ,[idTipoValorComparacionAsociada]
      ,cast([valorEspecificoAsociada] as nvarchar(100))	 AS valorEspecificoAsociada  
	FROM [dbo].[cacRestriccionesConsistencia]
	
END
GO

----------------------------->>>>>>>>>>>>>>>>------------------------

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
-- Create date: 26/04/2022
-- Description:	Se obtiene la data para migrar Error
-- =============================================
IF OBJECT_ID('[dbo].[SP_GetDataMigRegla]') IS NOT NULL
    EXEC('DROP PROCEDURE [dbo].[SP_GetDataMigRegla]')
GO
CREATE PROCEDURE [dbo].[SP_GetDataMigRegla]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [idRegla]
      ,[idCobertura]
      ,[nombre]
      ,[idTipoRegla]
      ,[idTiempoAplicacion]
      ,[habilitado]
      ,[idError]
      ,[idVariable]
      ,[idTipoEnvioLimbo]
	FROM [dbo].[cacRegla]
END
GO

----------------------------->>>>>>>>>>>>>>>>------------------------

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
-- Create date: 26/04/2022
-- Description:	Se obtiene la data para migrar Error
-- =============================================
IF OBJECT_ID('[dbo].[SP_GetDataMigEnfermedad]') IS NOT NULL
    EXEC('DROP PROCEDURE [dbo].[SP_GetDataMigEnfermedad]')
GO
CREATE PROCEDURE [dbo].[SP_GetDataMigEnfermedad]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [idCobertura]
      ,[nombre]
      ,[nemonico]
      ,[legislacion]
      ,[definicion]
      ,[tiempoEstimado]
      ,[ExcluirNovedades]
      ,[Novedades]
      ,[idResolutionSiame]
      ,[NovedadesCompartidosUnicos]
      ,[CantidadVariables]
      ,[idCoberturaPadre]
      ,[idCoberturaAdicionales]
	FROM [dbo].[cacCobertura]
END
GO

----------------------------->>>>>>>>>>>>>>>>------------------------

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
-- Create date: 26/04/2022
-- Description:	Se obtiene la data para migrar Error
-- =============================================
IF OBJECT_ID('[dbo].[SP_GetDataMigVariable]') IS NOT NULL
    EXEC('DROP PROCEDURE [dbo].[SP_GetDataMigVariable]')
GO
CREATE PROCEDURE [dbo].[SP_GetDataMigVariable]
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [idVariable]
      ,[idCobertura]
      ,[nombre]
      ,[nemonico]
      ,[descripcion]
      ,[idTipoVariable]
      ,[longitud]
      ,[decimales]
      ,ISNULL([formato],'') AS formato
      ,[tablaReferencial]
      ,[campoReferencial]  
	FROM [dbo].[Cacvariable]
END
GO