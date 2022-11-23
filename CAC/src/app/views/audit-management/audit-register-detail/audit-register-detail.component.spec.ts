import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuditRegisterDetailComponent } from './audit-register-detail.component';

describe('AuditRegisterDetailComponent', () => {
  let component: AuditRegisterDetailComponent;
  let fixture: ComponentFixture<AuditRegisterDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AuditRegisterDetailComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AuditRegisterDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
