USE [AuditCAC_QAInterno] -- db segun ambiente

DELETE FROM [ParametrosGenerales] where Nombre = 'SeparadorArchivos'
INSERT INTO [dbo].[ParametrosGenerales]([Nombre],[Valor],[Descripcion],[Activo],[FechaCreacion],[FechaActualizacion],[Status])
     VALUES('SeparadorArchivos','\t','Separador para lectura de archivos',1 ,GETDATE(),GETDATE(),1)

DELETE FROM [ParametrosGenerales] where Nombre = 'SeparadorArchivos'
INSERT INTO [dbo].[ParametrosGenerales]([Nombre],[Valor],[Descripcion],[Activo],[FechaCreacion],[FechaActualizacion],[Status])
     VALUES('CantidadColumnasCarguePoblacion','107','Cantidad de columnas archivo cargue de poblacion',1 ,GETDATE(),GETDATE(),1)
	 
DELETE FROM [ParametrosGenerales] where Nombre = 'SimulatorErrors'
INSERT INTO [dbo].[ParametrosGenerales]([Nombre],[Valor],[Descripcion],[Activo],[FechaCreacion],[FechaActualizacion],[Status])
     VALUES('SimulatorErrors','true','Parametro para simular errores',1 ,GETDATE(),GETDATE(),1)