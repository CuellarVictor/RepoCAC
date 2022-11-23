import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LiderEntidadListComponent } from './lider-entidad-list.component';

describe('LiderEntidadListComponent', () => {
  let component: LiderEntidadListComponent;
  let fixture: ComponentFixture<LiderEntidadListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LiderEntidadListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LiderEntidadListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
