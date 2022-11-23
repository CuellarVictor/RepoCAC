import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { GenericsRoutingModule } from './generics-routing.module';
import { CardPercentComponent } from './card-percent/card-percent.component';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule} from '@angular/material/icon';


@NgModule({
  declarations: [
    CardPercentComponent
  ],
  imports: [
    CommonModule,
    GenericsRoutingModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule
  ],
  exports:[
    CardPercentComponent
  ]
})
export class GenericsModule { }
