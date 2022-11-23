import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LiderEntidadComponent } from './lider-entidad.component';

describe('LiderEntidadComponent', () => {
  let component: LiderEntidadComponent;
  let fixture: ComponentFixture<LiderEntidadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LiderEntidadComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LiderEntidadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
