import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalEliminarRegistroAuditoriaComponent } from './modal-eliminar-registro-auditoria.component';

describe('ModalEliminarRegistroAuditoriaComponent', () => {
  let component: ModalEliminarRegistroAuditoriaComponent;
  let fixture: ComponentFixture<ModalEliminarRegistroAuditoriaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModalEliminarRegistroAuditoriaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalEliminarRegistroAuditoriaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
