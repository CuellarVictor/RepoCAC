import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CardPercentComponent } from './card-percent.component';

describe('CardPercentComponent', () => {
  let component: CardPercentComponent;
  let fixture: ComponentFixture<CardPercentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CardPercentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CardPercentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
