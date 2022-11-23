import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GenerarActasComponent } from './generar-actas.component';

describe('GenerarActasComponent', () => {
  let component: GenerarActasComponent;
  let fixture: ComponentFixture<GenerarActasComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GenerarActasComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GenerarActasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
