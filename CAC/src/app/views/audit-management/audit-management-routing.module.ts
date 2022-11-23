import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ManagementComponent } from './management/management.component';
import { VariableComponent } from './variable/variable.component';
import { MedicionComponent } from './medicion/medicion.component';
import { VariableDetailsComponent } from './variable-details/variable-details.component';
import { VariableSegmentationComponent } from './variable-segmentation/variable-segmentation.component';
import { AsignarCronogramaComponent } from './asignar-cronograma/asignar-cronograma.component';
import { AsignacionBolsaComponent } from './asignacion-bolsa/asignacion-bolsa.component';
import { ReasignarBolsaComponent } from './reasignar-bolsa/reasignar-bolsa.component';
import { VariableUpsertComponent } from './variable/variable-upsert/variable-upsert/variable-upsert.component';
const routes: Routes = [{
  path:'',
  component: ManagementComponent
},
{
  path:'variable',
  component: VariableUpsertComponent
},
{
  path:'medicion/:objMedicion',
  component: MedicionComponent
},
{
  path:'detalle-variable/:objVariable',
  component: VariableDetailsComponent
},
{
  path:'segmentacion-variable',
  component: VariableSegmentationComponent
},
{
  path:'asignar-cronograma',
  component: AsignarCronogramaComponent
},
{
  path:'asignar-bolsa/:objMedicion',
  component: AsignacionBolsaComponent
},
{
  path:'reasignar-bolsa/:obj',
  component: ReasignarBolsaComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuditManagementRoutingModule { }
