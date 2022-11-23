USE [AuditCAC_Development]

-- Insert Proceso
SET IDENTITY_INSERT [dbo].[Process] ON 
INSERT INTO [dbo].[Process]([ProcessId], Status,[Name],[Class],[Method],[LifeTime])
     VALUES(2,1,'Calificación Masiva', 'Proceso Core','CalificacionMasiva',5000)
SET IDENTITY_INSERT [dbo].[Process] OFF 

-- Insert Funcionalidades
INSERT INTO [dbo].[Funcionalidad]([Id],[Nombre],[Tipo],[Padre],[Img],[Enable],[CreateDate])
     VALUES(55,'Calificación Masiva',91,1,'https://proveedoresonline.s3.amazonaws.com/loadapifiles/20220912192415_CalificacionMasiva.png',1,GETDATE())

INSERT INTO [dbo].[Funcionalidad]([Id],[Nombre],[Tipo],[Padre],[Img],[Enable],[CreateDate])
     VALUES(56,'Permisos y accesos',91,1,'https://proveedoresonline.s3.amazonaws.com/loadapifiles/20220912192500_AdministracionPermisos.png',1,GETDATE())

INSERT INTO [dbo].[Funcionalidad]([Id],[Nombre],[Tipo],[Padre],[Img],[Enable],[CreateDate])
     VALUES(57,'Modificar Fecha final Medicion',91,1,'https://proveedoresonline.s3.amazonaws.com/loadapifiles/20220912194256_fechafinalizacionauditoria.png',1,GETDATE())
	 
-- Catalog Tipo Calcuradora
SET IDENTITY_INSERT [dbo].[Catalog] ON 
INSERT [dbo].[Catalog] ([Id], [CatalogName], [Enable], [LastModify], [CreateDate], [Status]) VALUES (21, N'Tipo Calcuradora', 1,GETDATE(), GETDATE(), NULL)
SET IDENTITY_INSERT [dbo].[Catalog] OFF

SET IDENTITY_INSERT [dbo].[Item] ON 

INSERT [dbo].[Item] ([Id], [CatalogId], [ItemName], [Concept], [Enable], [LastModify], [CreateDate], [Status]) VALUES (134, 21, 'KRU', '', 1, GETDATE(), GETDATE(), 1)
INSERT [dbo].[Item] ([Id], [CatalogId], [ItemName], [Concept], [Enable], [LastModify], [CreateDate], [Status]) VALUES (135, 21, 'TFG', '', 1, GETDATE(), GETDATE(), 1)
INSERT [dbo].[Item] ([Id], [CatalogId], [ItemName], [Concept], [Enable], [LastModify], [CreateDate], [Status]) VALUES (136, 21, 'Promedio', '', 1, GETDATE(), GETDATE(), 1)

SET IDENTITY_INSERT [dbo].[Item] OFF 


-- Insert Prmisos y roles 
INSERT INTO [dbo].[Funcionalidad_Permisos_Rol]([FuncionalidadId],[RolId],[Enable],[Visible],[CreateDate],[CreatedBy],[ModifyDate],[ModifyBy])
     VALUES(56,1,1,1,GETDATE(),(select TOP(1) Id from AUTH.Usuario WHERE RolId = 1),GETDATE(),(select TOP(1) Id from AUTH.Usuario WHERE RolId = 1))