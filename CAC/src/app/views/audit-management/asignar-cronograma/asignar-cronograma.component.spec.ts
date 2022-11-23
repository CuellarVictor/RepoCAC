import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AsignarCronogramaComponent } from './asignar-cronograma.component';

describe('AsignarCronogramaComponent', () => {
  let component: AsignarCronogramaComponent;
  let fixture: ComponentFixture<AsignarCronogramaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AsignarCronogramaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AsignarCronogramaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
