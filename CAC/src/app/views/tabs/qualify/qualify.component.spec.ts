import { ComponentFixture, TestBed } from '@angular/core/testing';

import { QualifyComponent } from './qualify.component';

describe('QualifyComponent', () => {
  let component: QualifyComponent;
  let fixture: ComponentFixture<QualifyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ QualifyComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(QualifyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
