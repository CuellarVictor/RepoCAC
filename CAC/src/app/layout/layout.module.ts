import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutRoutingModule } from './layout-routing.module';
import { HomeComponent } from './dashbord/home/home.component';
import { MatSidenavModule } from '@angular/material/sidenav';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatFormFieldModule} from '@angular/material/form-field';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import {MatMenuModule} from '@angular/material/menu';
import { TollbarComponent } from './dashbord/tollbar/tollbar.component';
import { SideNavComponent } from './dashbord/side-nav/side-nav.component';
import {MatBadgeModule} from '@angular/material/badge';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import {MatGridListModule} from '@angular/material/grid-list';
import {MatExpansionModule} from '@angular/material/expansion';
import {MatInputModule} from '@angular/material/input';
import { TabsModule } from '../views/tabs/tabs.module';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import { CloseSessionComponent } from './login/closeSession/close-session/close-session.component';
import { MatDialogModule } from '@angular/material/dialog';
import { LoadingComponent } from './loading/loading.component';
import { MatTooltipModule } from '@angular/material/tooltip';
import {RoundProgressModule} from 'angular-svg-round-progressbar';
import { MatSelectModule } from '@angular/material/select';
import { RecuperarPasswordComponent } from './recuperar-password/recuperar-password/recuperar-password.component';


@NgModule({
  declarations: [
    HomeComponent,
    TollbarComponent,
    SideNavComponent,
    RegisterComponent,
    LoginComponent,
    CloseSessionComponent,
    LoadingComponent,
    RecuperarPasswordComponent
    ],
  imports: [
    CommonModule,
    LayoutRoutingModule,
    MatSidenavModule,
    MatDialogModule,
    MatProgressSpinnerModule,
    MatToolbarModule,
    MatCheckboxModule,
    MatFormFieldModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatBadgeModule,
    MatGridListModule,
    MatExpansionModule,
    MatInputModule,
    TabsModule,
    MatTooltipModule,
    RoundProgressModule,
    MatSelectModule
  ]
})
export class LayoutModule { }
