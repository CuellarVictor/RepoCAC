import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BankInformationRoutingModule } from './bank-information-routing.module';
import { InformationComponent } from './information/information.component';
import { GenericsModule } from '../generics/generics.module';
import {MatIconModule} from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import {MatExpansionModule} from '@angular/material/expansion';
import {MatRadioModule} from '@angular/material/radio';
import {MatButtonModule} from '@angular/material/button';
import {MatDialogModule} from '@angular/material/dialog';
import {MatTooltipModule} from '@angular/material/tooltip';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatTreeModule} from '@angular/material/tree';
import {MatSelectModule} from '@angular/material/select';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatMenuModule} from '@angular/material/menu';
import {MatTableModule} from '@angular/material/table';
import {MatNativeDateModule} from '@angular/material/core';
import {MatAutocompleteModule} from '@angular/material/autocomplete';

import { ObservationsComponent } from './observations/observations.component';
import { DialogComponent } from './dialog/dialog.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TabsModule } from '../tabs/tabs.module';
import { NgxMaskModule } from 'ngx-mask';
import { HallazgoComponent } from './hallazgo/hallazgo.component';
import { ContextoBotComponent } from './contexto-bot/contexto-bot.component';
import { DialogCalculadoraComponent } from './dialog_calculadora/dialog_calculadora.component';


@NgModule({
  declarations: [
    InformationComponent,
    ObservationsComponent,
    DialogComponent,
    HallazgoComponent,
    ContextoBotComponent,
    DialogCalculadoraComponent
  ],
  imports: [
    CommonModule,
    BankInformationRoutingModule,
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
    MatAutocompleteModule,
    NgxMaskModule.forRoot(),
  ]
  ,
  entryComponents: [DialogComponent,DialogCalculadoraComponent]
})
export class BankInformationModule { }
