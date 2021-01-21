import { Component, Injector, OnInit } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto} from '../../shared/paged-listing-component-base';
import { ApplicationDto, GetApplicationForViewDto, ApplicationServiceProxy, GetApplicationForViewDtoPagedResultDto } from '../../shared/service-proxies/service-proxies';
import {StringConstants} from'../../shared/stringConstants'
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { finalize } from 'rxjs/operators';


class PagedApplicationRequestDto extends PagedRequestDto{

    keyword: string;
    sorting: string;

}


@Component({
  selector: 'app-application-master',
  templateUrl: './application-master.component.html',
  styleUrls: ['./application-master.component.css']
})
export class ApplicationMasterComponent extends PagedListingComponentBase<ApplicationDto> {
    protected delete(entity: ApplicationDto): void {
        throw new Error('Method not implemented.');
    }
 

  applications: GetApplicationForViewDto[] = [];
    
   
    showNoDataText = '';
    selectedApplication:number;
    isLoading = false;

    constructor(injector: Injector, private _applicationService: ApplicationServiceProxy, private _modalService: BsModalService, private stringConstant: StringConstants) {
      super(injector);
      this.showNoDataText = this.stringConstant.norecoredFoundMessaage;
    }

 

/**
 * Method for showing list of applications
 * @param request : request object for showing application
 * @param pageNumber : selected page number
 * @param finishedCallback :A callback is a function that is to be executed after another function has finished executing
 */
protected list(
    request: PagedApplicationRequestDto,
    pageNumber: number,
    finishedCallback: Function
): void {
    this.isLoading = true;
    


    this._applicationService
        .getAll()
        .pipe(
            finalize(() => {
                finishedCallback();
            })
        )
        .subscribe((result: GetApplicationForViewDtoPagedResultDto) => {
            this.applications = result.items;
            this.showPaging(result, pageNumber);
            
        });
}

   






}
