import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InboxRoutingModule } from './inbox-routing.module';
import { AlertsComponent } from './alerts/alerts.component';
import { GenericsModule } from '../generics/generics.module';
import {MatTabsModule} from '@angular/material/tabs';
import { MatCardModule } from '@angular/material/card';
import { ActionRequiredComponent } from './action-required/action-required.component';
import { HoldComponent } from './hold/hold.component';
import {MatTooltipModule} from '@angular/material/tooltip';
import { InboxComponent } from './inbox/inbox.component';
import { SendInboxComponent } from './send-inbox/send-inbox.component';
import { ReceivedInboxComponent } from './received-inbox/received-inbox.component';
import {MatBottomSheetModule} from '@angular/material/bottom-sheet';
import { MatButtonModule } from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import { NewInboxComponent } from './new-inbox/new-inbox.component';
import {MatDialogModule} from '@angular/material/dialog';
import { OpenInboxComponent } from './open-inbox/open-inbox.component';
import {MatDividerModule} from '@angular/material/divider';
import {MatExpansionModule} from '@angular/material/expansion';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatTreeModule} from '@angular/material/tree';
import {MatSelectModule} from '@angular/material/select';
import {MatCheckboxModule} from '@angular/material/checkbox';
import {MatDatepickerModule} from '@angular/material/datepicker';
import {MatMenuModule} from '@angular/material/menu';
import {MatTableModule} from '@angular/material/table';
import {MatRadioModule} from '@angular/material/radio';
import {MatNativeDateModule} from '@angular/material/core';
import { MatSortModule } from '@angular/material/sort';


@NgModule({
  declarations: [
    AlertsComponent,
    ActionRequiredComponent,
    HoldComponent,
    InboxComponent,
    SendInboxComponent,
    ReceivedInboxComponent,
    NewInboxComponent,
    OpenInboxComponent,
  ],
  imports: [
    CommonModule,
    InboxRoutingModule,
    GenericsModule,
    MatTabsModule,
    MatCardModule,
    MatTooltipModule,
    MatBottomSheetModule,
    MatButtonModule,
    MatIconModule,
    MatDialogModule,
    MatDividerModule,
    MatExpansionModule,
    MatInputModule,
    MatFormFieldModule,
    MatTreeModule,
    MatSelectModule,
    MatCheckboxModule,
    MatDatepickerModule,
    MatMenuModule,
    MatTableModule,
    MatRadioModule,
    MatNativeDateModule,
    MatSortModule
  ]
})
export class InboxModule { }
