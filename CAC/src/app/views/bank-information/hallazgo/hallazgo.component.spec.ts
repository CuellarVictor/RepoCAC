import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HallazgoComponent } from './hallazgo.component';

describe('HallazgoComponent', () => {
  let component: HallazgoComponent;
  let fixture: ComponentFixture<HallazgoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HallazgoComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HallazgoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
