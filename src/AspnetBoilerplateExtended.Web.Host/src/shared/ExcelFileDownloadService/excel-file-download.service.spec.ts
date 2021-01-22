import { TestBed } from '@angular/core/testing';

import { ExcelFileDownloadService } from './excel-file-download.service';

describe('ExcelFileDownloadService', () => {
  let service: ExcelFileDownloadService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ExcelFileDownloadService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
