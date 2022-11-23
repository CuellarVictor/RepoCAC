import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ContextoBotComponent } from './contexto-bot.component';

describe('ContextoBotComponent', () => {
  let component: ContextoBotComponent;
  let fixture: ComponentFixture<ContextoBotComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ContextoBotComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ContextoBotComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
