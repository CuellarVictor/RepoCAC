import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DialogCalculadoraComponent } from './dialog_calculadora.component';

describe('DialogCalculadoraComponent', () => {
  let component: DialogCalculadoraComponent;
  let fixture: ComponentFixture<DialogCalculadoraComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DialogCalculadoraComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DialogCalculadoraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
