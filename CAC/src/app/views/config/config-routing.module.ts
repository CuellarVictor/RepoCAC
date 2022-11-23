import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CatalogoComponent } from './catalogo/catalogo.component';
import { ConfiguracionComponent } from './configuracion/configuracion.component';

const routes: Routes = [{
  path:'',
  component: ConfiguracionComponent
},
{
  path:'catalogo/:idCatalogo',
  component: CatalogoComponent
}];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ConfigRoutingModule { }
