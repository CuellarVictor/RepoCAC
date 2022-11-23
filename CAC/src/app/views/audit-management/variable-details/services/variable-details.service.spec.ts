import { TestBed } from '@angular/core/testing';

import { VariableDetailsService } from './variable-details.service';

describe('VariableDetailsService', () => {
  let service: VariableDetailsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(VariableDetailsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
