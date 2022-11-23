import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminRoutingModule } from './admin-routing.module';
import { AdministradorComponent } from './administrador/administrador.component';
import { MatIconModule } from '@angular/material/icon';
import { GenericsModule } from '../generics/generics.module';
import { MatCardModule } from '@angular/material/card';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatRadioModule } from '@angular/material/radio';
import { MatButtonModule } from '@angular/material/button';
import { MatDialogModule } from '@angular/material/dialog';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatTreeModule } from '@angular/material/tree';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatMenuModule } from '@angular/material/menu';
import { MatNativeDateModule } from '@angular/material/core';
import { MatTableModule } from '@angular/material/table';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TabsModule } from '../tabs/tabs.module';
import { LogUsuarioComponent } from './log-usuario/log-usuario.component'; 

 
@NgModule({
  declarations: [
    AdministradorComponent,
    LogUsuarioComponent, 
  ],
  imports: [
      CommonModule,
      AdminRoutingModule,
      MatIconModule,
      GenericsModule,
      MatCardModule,
      MatExpansionModule,
      MatRadioModule,
      MatButtonModule,
      MatDialogModule,
      MatInputModule,
      MatFormFieldModule,
      MatTooltipModule,
      MatTreeModule,
      MatSelectModule,
      MatCheckboxModule,
      MatDatepickerModule,
      MatMenuModule,
      MatNativeDateModule,
      MatTableModule,
      FormsModule,
      ReactiveFormsModule,
      TabsModule,    
      MatFormFieldModule,
      MatInputModule,
    ]
})
export class AdminModule { }
