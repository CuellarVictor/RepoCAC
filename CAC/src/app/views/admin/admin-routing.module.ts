import { NgModule } from '@angular/core';
import { AuthGuard } from 'src/utils/guards/auth.guard';
import { RouterModule, Routes } from '@angular/router';
import { AdministradorComponent } from './administrador/administrador.component'; 
import { LogUsuarioComponent } from './log-usuario/log-usuario.component';


const routes: Routes = [
  {
    path:'',
    component: AdministradorComponent, 
    canActivate: [AuthGuard],
  },  
  {
    path:'log_usuario',
    component: LogUsuarioComponent, 
    canActivate: [AuthGuard],
  },  

];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
