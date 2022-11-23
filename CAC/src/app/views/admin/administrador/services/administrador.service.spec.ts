import { TestBed } from '@angular/core/testing';

import { AdministradorService } from '../../../admin/administrador/services/administrador.service';

describe('AdministradorService', () => {
  let service: AdministradorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AdministradorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
