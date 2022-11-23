import { ComponentFixture, TestBed } from '@angular/core/testing';

import { VariableSegmentationComponent } from './variable-segmentation.component';

describe('VariableSegmentationComponent', () => {
  let component: VariableSegmentationComponent;
  let fixture: ComponentFixture<VariableSegmentationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ VariableSegmentationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(VariableSegmentationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
