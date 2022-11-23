import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/utils/guards/auth.guard';
import { HomeComponent } from './layout/dashbord/home/home.component';
import { LoginComponent } from './layout/login/login.component';   
import { ProfileComponent } from './views/profile/profile/profile.component';   
import { UsuarioComponent } from './views/profile/usuario/usuario.component';   

export const routes: Routes = [
  {
    path: '',
    component:LoginComponent
  },
  {
    path: 'assets',
    component: HomeComponent,
    loadChildren: () => import('./views/demo/demo/demo.module').then(m => m.DemoModule),
  },
  {
    path: 'cronograma',
    component: HomeComponent,
    canActivate: [AuthGuard],
    loadChildren: () => import('./views/cronograma/cronograma.module').then(m => m.CronogramaModule),
  },
  {
    path: 'perfil',
    component: HomeComponent,
    canActivate: [AuthGuard],
    loadChildren: () => import('./views/profile/profile.module').then(m => m.ProfileModule),
  },
  {
    path: 'buzon',
    component: HomeComponent,
    canActivate: [AuthGuard],
    loadChildren: () => import('./views/inbox/inbox.module').then(m => m.InboxModule),
  },
  {
    path: 'banco-de-informacion',
    component: HomeComponent,
    canActivate: [AuthGuard],
    loadChildren: () => import('./views/bank-information/bank-information.module').then(m => m.BankInformationModule),
  },
  {
    path: 'gestion-de-auditoria',
    component: HomeComponent,
    loadChildren: () => import('./views/audit-management/audit-management.module').then(m => m.AuditManagementModule),
  },
  {
    path: 'admin',
    component: HomeComponent,
    loadChildren: () => import('./views/admin/admin.module').then(m => m.AdminModule),
  },
  {
    path: 'config',
    component: HomeComponent,
    loadChildren: () => import('./views/config/config.module').then(m => m.ConfigModule),
  }, 
  {
    path: 'usuario',
    component: HomeComponent,
    loadChildren: () => import('./views/profile/profile.module').then(m => m.ProfileModule),
  }, 
];

export const APP_ROUTING = RouterModule.forRoot(routes);


@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
