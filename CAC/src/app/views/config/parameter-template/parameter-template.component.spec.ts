import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ParameterTemplateComponent } from './parameter-template.component';

describe('ParameterTemplateComponent', () => {
  let component: ParameterTemplateComponent;
  let fixture: ComponentFixture<ParameterTemplateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ParameterTemplateComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ParameterTemplateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
