import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/utils/guards/auth.guard'; 
import { HomeComponent } from './dashbord/home/home.component';
import { LoginComponent } from './login/login.component';
import { RecuperarPasswordComponent } from './recuperar-password/recuperar-password/recuperar-password.component';

const routes: Routes = [
  // {
  // path: '',
  // component:HomeComponent
  // },
  {
   path: 'Home',
   component:HomeComponent,
   canActivate: [AuthGuard]
  },
  {
    path: 'Login',
    component:LoginComponent
  },
  {
    path: 'recuperarpassword/:id',
    component:RecuperarPasswordComponent
  } 
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LayoutRoutingModule { }
