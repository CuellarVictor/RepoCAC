import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LetfBarComponent } from './letf-bar.component';

describe('LetfBarComponent', () => {
  let component: LetfBarComponent;
  let fixture: ComponentFixture<LetfBarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LetfBarComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LetfBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
