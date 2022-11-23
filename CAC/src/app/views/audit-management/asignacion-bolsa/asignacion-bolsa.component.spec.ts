import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AsignacionBolsaComponent } from './asignacion-bolsa.component';

describe('AsignacionBolsaComponent', () => {
  let component: AsignacionBolsaComponent;
  let fixture: ComponentFixture<AsignacionBolsaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AsignacionBolsaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AsignacionBolsaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
