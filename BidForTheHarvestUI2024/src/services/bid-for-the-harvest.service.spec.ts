import { TestBed } from '@angular/core/testing';

import { BidForTheHarvestService } from './bid-for-the-harvest.service';

describe('BidForTheHarvestService', () => {
  let service: BidForTheHarvestService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BidForTheHarvestService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
