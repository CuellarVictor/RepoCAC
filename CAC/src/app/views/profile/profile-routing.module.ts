import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/utils/guards/auth.guard';
import { ProfileComponent } from './profile/profile.component';
import { UsuarioComponent } from './usuario/usuario.component';

const routes: Routes = [{
    path:'perfil',
    component: ProfileComponent
  },
  {
    path:':objUser',
    canActivate: [AuthGuard],
    component: UsuarioComponent
  },
  {
    path:'',
    canActivate: [AuthGuard],
    component: UsuarioComponent
  }, 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProfileRoutingModule { }
