import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExtemporaneaComponent } from './extemporanea.component';

describe('ExtemporaneaComponent', () => {
  let component: ExtemporaneaComponent;
  let fixture: ComponentFixture<ExtemporaneaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ExtemporaneaComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ExtemporaneaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
