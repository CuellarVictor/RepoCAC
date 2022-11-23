import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VariableCondicionadaComponent } from './variable-condicionada.component';

describe('VariableCondicionadaComponent', () => {
  let component: VariableCondicionadaComponent;
  let fixture: ComponentFixture<VariableCondicionadaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VariableCondicionadaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VariableCondicionadaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
