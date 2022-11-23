import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VariableUpsertComponent } from './variable-upsert.component';

describe('VariableUpsertComponent', () => {
  let component: VariableUpsertComponent;
  let fixture: ComponentFixture<VariableUpsertComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VariableUpsertComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VariableUpsertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
