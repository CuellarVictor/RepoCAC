import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HallazgoMasivaComponent } from './hallazgo-masiva.component';

describe('HallazgoMasivaComponent', () => {
  let component: HallazgoMasivaComponent;
  let fixture: ComponentFixture<HallazgoMasivaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HallazgoMasivaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HallazgoMasivaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
