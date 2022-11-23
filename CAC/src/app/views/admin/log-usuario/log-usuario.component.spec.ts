import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LogUsuarioComponent } from './log-usuario.component';

describe('LogUsuarioComponent', () => {
  let component: LogUsuarioComponent;
  let fixture: ComponentFixture<LogUsuarioComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LogUsuarioComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LogUsuarioComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
