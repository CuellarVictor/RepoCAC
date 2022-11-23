import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ImgPermisoModalComponent } from './img-permiso-modal.component';

describe('ImgPermisoModalComponent', () => {
  let component: ImgPermisoModalComponent;
  let fixture: ComponentFixture<ImgPermisoModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ImgPermisoModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ImgPermisoModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
