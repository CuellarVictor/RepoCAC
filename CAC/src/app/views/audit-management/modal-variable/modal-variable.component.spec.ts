import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalVariableComponent } from './modal-variable.component';

describe('ModalVariable', () => {
  let component: ModalVariableComponent;
  let fixture: ComponentFixture<ModalVariableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModalVariableComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalVariableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
