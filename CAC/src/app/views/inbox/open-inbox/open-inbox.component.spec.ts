import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OpenInboxComponent } from './open-inbox.component';

describe('OpenInboxComponent', () => {
  let component: OpenInboxComponent;
  let fixture: ComponentFixture<OpenInboxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ OpenInboxComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(OpenInboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
