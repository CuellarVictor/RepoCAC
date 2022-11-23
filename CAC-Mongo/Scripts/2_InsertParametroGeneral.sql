USE [AuditCAC_Development]
GO

INSERT INTO [dbo].[ParametrosGenerales]
           ([Nombre]
           ,[Valor]
           ,[Descripcion]
           ,[Activo]
           ,[FechaCreacion]
           ,[FechaActualizacion]
           ,[Status])
     VALUES
           ('HorasActualizarMongo'
           ,'72'
           ,'Horas hacia atras para actualizar los registros de mongo'
           ,1
           , GetDate()
           , GetDate()
           ,1)
GO