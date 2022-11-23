import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceivedInboxComponent } from './received-inbox.component';

describe('ReceivedInboxComponent', () => {
  let component: ReceivedInboxComponent;
  let fixture: ComponentFixture<ReceivedInboxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReceivedInboxComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReceivedInboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
