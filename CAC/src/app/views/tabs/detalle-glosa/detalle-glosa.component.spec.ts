import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleGlosaComponent } from './detalle-glosa.component';

describe('DetalleGlosaComponent', () => {
  let component: DetalleGlosaComponent;
  let fixture: ComponentFixture<DetalleGlosaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DetalleGlosaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DetalleGlosaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
