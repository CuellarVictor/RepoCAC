import { TestBed } from '@angular/core/testing';

import { genericService } from './generics.service';

describe('TabsService', () => {
  let service: genericService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(genericService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
