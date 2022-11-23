import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListCronogramaComponent } from './list-cronograma.component';

describe('ListCronogramaComponent', () => {
  let component: ListCronogramaComponent;
  let fixture: ComponentFixture<ListCronogramaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ListCronogramaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ListCronogramaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
