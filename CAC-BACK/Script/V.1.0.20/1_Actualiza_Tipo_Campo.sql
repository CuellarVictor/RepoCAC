USE [AuditCAC_Development]

					-- INT
					UPDATE vxm
						SET 
							vxm.TipoCampo = 89
						FROM VariableXMedicion vxm -- Tabla a actualizar
							INNER JOIN Variables vr -- Tabla con la que cruza
							ON vr.Id = vxm.VariableId
						WHERE vr.idTipoVariable = 'int' AND vxm.TipoCampo = 20


					-- numeric
					UPDATE vxm
						SET 
							vxm.TipoCampo = 86
						FROM VariableXMedicion vxm -- Tabla a actualizar
							INNER JOIN Variables vr -- Tabla con la que cruza
							ON vr.Id = vxm.VariableId
						WHERE vr.idTipoVariable = 'numeric' AND vxm.TipoCampo = 20

					-- datetime
					UPDATE vxm
						SET 
							vxm.TipoCampo = 88
						FROM VariableXMedicion vxm -- Tabla a actualizar
							INNER JOIN Variables vr -- Tabla con la que cruza
							ON vr.Id = vxm.VariableId
						WHERE vr.idTipoVariable = 'datetime' AND vxm.TipoCampo = 20

					-- varchar bit
					UPDATE vxm
						SET 
							vxm.TipoCampo = 87
						FROM VariableXMedicion vxm -- Tabla a actualizar
							INNER JOIN Variables vr -- Tabla con la que cruza
							ON vr.Id = vxm.VariableId
						WHERE (vr.idTipoVariable = 'varchar' OR vr.idTipoVariable = 'bit') AND vxm.TipoCampo = 20