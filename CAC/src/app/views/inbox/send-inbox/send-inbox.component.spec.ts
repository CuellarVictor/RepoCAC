import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SendInboxComponent } from './send-inbox.component';

describe('SendInboxComponent', () => {
  let component: SendInboxComponent;
  let fixture: ComponentFixture<SendInboxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SendInboxComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SendInboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
