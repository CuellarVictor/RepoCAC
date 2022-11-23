import { ComponentFixture, TestBed } from '@angular/core/testing';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';

import { ModalCarguePoblacionComponent } from './modal-cargue-poblacion.component';

describe('ModalCarguePoblacionComponent', () => {
  let component: ModalCarguePoblacionComponent;
  let fixture: ComponentFixture<ModalCarguePoblacionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModalCarguePoblacionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalCarguePoblacionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
