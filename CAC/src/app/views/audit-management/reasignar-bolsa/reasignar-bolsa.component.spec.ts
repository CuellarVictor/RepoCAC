import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReasignarBolsaComponent } from './reasignar-bolsa.component';

describe('ReasignarBolsaComponent', () => {
  let component: ReasignarBolsaComponent;
  let fixture: ComponentFixture<ReasignarBolsaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReasignarBolsaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReasignarBolsaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
