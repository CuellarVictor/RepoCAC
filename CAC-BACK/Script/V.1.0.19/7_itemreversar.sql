SET IDENTITY_INSERT [dbo].Item ON 

INSERT INTO Item(Id,CatalogId,ItemName,Concept,Enable,LastModify,CreateDate,Status)
VALUES(133,1,'Reversar','Reversar',1,GETDATE(),GETDATE(),1);

SET IDENTITY_INSERT [dbo].Item OFF