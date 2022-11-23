import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalCalificacionMasivaComponent } from './modal-calificacion-masiva.component';

describe('ModalCalificacionMasivaComponent', () => {
  let component: ModalCalificacionMasivaComponent;
  let fixture: ComponentFixture<ModalCalificacionMasivaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModalCalificacionMasivaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalCalificacionMasivaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
