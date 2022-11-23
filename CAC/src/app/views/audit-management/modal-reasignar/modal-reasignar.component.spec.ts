import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalReasignarComponent } from './modal-reasignar.component';

describe('ModalVariable', () => {
  let component: ModalReasignarComponent;
  let fixture: ComponentFixture<ModalReasignarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModalReasignarComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalReasignarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
