import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DemoRoutingModule } from './demo-routing.module';
import { AssetsComponent } from './assets/assets.component';
import {MatTooltipModule} from '@angular/material/tooltip';

import {MatButtonModule} from '@angular/material/button';
import {MatIconModule} from '@angular/material/icon';
import {MatExpansionModule} from '@angular/material/expansion';
import {MatInputModule} from '@angular/material/input';
import {MatFormFieldModule} from '@angular/material/form-field';

@NgModule({
  declarations: [
    AssetsComponent
  ],
  imports: [
    CommonModule,
    DemoRoutingModule,
    MatTooltipModule,
    MatButtonModule,
    MatIconModule,
    MatExpansionModule,
    MatInputModule,
    MatFormFieldModule
  ]
})
export class DemoModule { }
