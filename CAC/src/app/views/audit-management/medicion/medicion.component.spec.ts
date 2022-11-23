import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MedicionComponent } from './medicion.component';

describe('MedicionComponent', () => {
  let component: MedicionComponent;
  let fixture: ComponentFixture<MedicionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MedicionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MedicionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
