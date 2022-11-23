import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TabWarningComponent } from './tab-warning.component';

describe('TabWarningComponent', () => {
  let component: TabWarningComponent;
  let fixture: ComponentFixture<TabWarningComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TabWarningComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TabWarningComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
