import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ListCronogramaComponent } from './list-cronograma/list-cronograma.component';
import { RegistrationOpeningComponent } from './registration-opening/registration-opening.component';

const routes: Routes = [{
  path:'',
  component:ListCronogramaComponent
},
{
  path:'apertura-inicial-registro',
  component:RegistrationOpeningComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CronogramaRoutingModule { }
