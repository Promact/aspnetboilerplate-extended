import { Injectable } from '@angular/core';
import { StringConstants } from '../../shared/stringConstants';
import { FileDto } from '../../shared/service-proxies/service-proxies';
import { AppConsts } from '../../shared/AppConsts';
@Injectable({
  providedIn: 'root'
})
export class ExcelFileDownloadService {

    constructor(private stringConstant: StringConstants) { }

    downloadTempFile(file: FileDto) {
        const url = AppConsts.remoteServiceBaseUrl + this.stringConstant.fileDownloadPath + file.fileType + '&fileToken=' + file.fileToken + '&fileName=' + file.fileName;
        location.href = url; //TODO: This causes reloading of same page in Firefox
    }
}
