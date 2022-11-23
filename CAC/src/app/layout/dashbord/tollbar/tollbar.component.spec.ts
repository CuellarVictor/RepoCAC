import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TollbarComponent } from './tollbar.component';

describe('TollbarComponent', () => {
  let component: TollbarComponent;
  let fixture: ComponentFixture<TollbarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TollbarComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TollbarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
