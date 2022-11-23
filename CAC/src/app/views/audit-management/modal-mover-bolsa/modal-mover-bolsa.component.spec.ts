import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ModalMoverBolsaComponent } from './modal-mover-bolsa.component';

describe('ModalVariable', () => {
  let component: ModalMoverBolsaComponent;
  let fixture: ComponentFixture<ModalMoverBolsaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ModalMoverBolsaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ModalMoverBolsaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
