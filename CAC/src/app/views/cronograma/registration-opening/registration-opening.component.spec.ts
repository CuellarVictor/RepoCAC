import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegistrationOpeningComponent } from './registration-opening.component';

describe('RegistrationOpeningComponent', () => {
  let component: RegistrationOpeningComponent;
  let fixture: ComponentFixture<RegistrationOpeningComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegistrationOpeningComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RegistrationOpeningComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
