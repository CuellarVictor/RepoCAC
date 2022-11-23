import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VariableDetailsComponent } from './variable-details.component';

describe('VariableDetailsComponent', () => {
  let component: VariableDetailsComponent;
  let fixture: ComponentFixture<VariableDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VariableDetailsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VariableDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
