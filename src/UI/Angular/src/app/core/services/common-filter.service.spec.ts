import { TestBed } from '@angular/core/testing';

import { CommonFilterService } from './common-filter.service';

describe('CommonFilterService', () => {
  let service: CommonFilterService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CommonFilterService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
