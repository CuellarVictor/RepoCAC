import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalReasignarDescargarComponent } from './modal-reasignar-descargar.component';

describe('ModalVariable', () => {
  let component: ModalReasignarDescargarComponent;
  let fixture: ComponentFixture<ModalReasignarDescargarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModalReasignarDescargarComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalReasignarDescargarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
