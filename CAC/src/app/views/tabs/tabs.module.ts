import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { TabsRoutingModule } from './tabs-routing.module';
import { TabWarningComponent } from './tab-warning/tab-warning.component';
import {MatIconModule} from '@angular/material/icon';
import {MatDividerModule} from '@angular/material/divider';
import {MatTooltipModule} from '@angular/material/tooltip';
import {MatDialogModule} from '@angular/material/dialog';
import { ReportErrorComponent } from './report-error/report-error.component';
import { LetfBarComponent } from './letf-bar/letf-bar.component';
import { IndexComponent } from './index/index.component';
import { SupportComponent } from './support/support.component';
import { DetailsComponent } from './details/details.component';
import { ContactLeadComponent } from './contact-lead/contact-lead.component';
import { QualifyComponent } from './qualify/qualify.component';
import { InfoComponent } from './info/info.component';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import {MatCheckboxModule} from '@angular/material/checkbox';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { RatingModule } from 'ng-starrating';
import { ExtemporaneaComponent } from './extemporanea/extemporanea.component';
import { FileUploadModule } from 'ng2-file-upload';
import { DetalleGlosaComponent } from './detalle-glosa/detalle-glosa.component';
import { HallazgoMasivaComponent } from './hallazgo-masiva/hallazgo-masiva.component';

@NgModule({
  declarations: [
    TabWarningComponent,
    ReportErrorComponent,
    LetfBarComponent,
    IndexComponent,
    SupportComponent,
    DetailsComponent,
    ContactLeadComponent,
    QualifyComponent,
    InfoComponent,
    ExtemporaneaComponent,
    DetalleGlosaComponent,
    HallazgoMasivaComponent,
    
  ],
  imports: [
    CommonModule,
    TabsRoutingModule,
    MatSelectModule,
    MatIconModule,
    MatDividerModule,
    MatTooltipModule,
    MatDialogModule,
    MatButtonModule,
    MatCardModule,
    MatCheckboxModule,
    FormsModule,
    RatingModule,
    ReactiveFormsModule,
    FileUploadModule
  ],
  exports:[
    TabWarningComponent
  ]
})
export class TabsModule { }
