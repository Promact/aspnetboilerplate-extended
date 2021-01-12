import { Component, Injector, OnInit } from '@angular/core';
import { PagedListingComponentBase, PagedRequestDto} from '../../shared/paged-listing-component-base';
import { ApplicationDto, GetApplicationForViewDto, ApplicationServiceProxy, GetApplicationForViewDtoPagedResultDto } from '../../shared/service-proxies/service-proxies';
import {StringConstants} from'../../shared/stringConstants'
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { Router } from '@angular/router';
import { from } from 'rxjs';
import { finalize } from 'rxjs/operators';
import { ToastrService } from 'ngx-toastr';
import { ExcelFileDownloadService } from '@shared/ExcelFileDownloadService/excel-file-download.service';
import { ApplicationEditMasterComponent } from '@app/application-edit-master/application-edit-master.component';
import { ApplicationCreateMasterComponent } from '@app/application-create-master/application-create-master.component';



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
 

  applications: GetApplicationForViewDto[] = [];
    keyword = '';
    advancedFiltersVisible = false;
    filterText = '';
    nameFilter = '';
    showNoDataText = '';
    selectedApplication:number;
    isLoading = false;
    isCreateGranted = false;
    isEditGranted = false;
    isDeleteGranted = false;
    isViewGranted = true;
    constructor(injector: Injector, private _applicationService: ApplicationServiceProxy, private _modalService: BsModalService, private stringConstant: StringConstants,private _router:Router,private toaster:ToastrService,
      private excelFileDownloadService:ExcelFileDownloadService) {
      super(injector);
      this.showNoDataText = this.stringConstant.norecoredFoundMessaage;
      

      this.isCreateGranted = abp.auth.isGranted(this.stringConstant.applicationCreatePermission);
      this.isEditGranted = abp.auth.isGranted(this.stringConstant.applicationEditPermission);
      this.isDeleteGranted = abp.auth.isGranted(this.stringConstant.applicationDeletePermission);
      this.isViewGranted = abp.auth.isGranted(this.stringConstant.applicationViewPermission);

      if(!this.isViewGranted){
          this._router.navigate(["/app/404"])
      }
  }

  /**
    * Method for using clear filters
    */
   clearFilters(): void {
    this.keyword = '';
    this.getDataPage(1);
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
    request.keyword = this.keyword;


    this._applicationService
        .getAll(
            this.filterText,
            request.keyword,
            request.sorting,
            request.skipCount,
            request.maxResultCount,
        )
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

/**
     * Method to delete application
     * @param app : Application to be deleted
     */
    protected delete(app: ApplicationDto): void {
      abp.message.confirm(
          this.l(this.stringConstant.deleteWarningMessage, app.applicationName),
          undefined,
          (result: boolean) => {
              if (result) {
                  this._applicationService.deleteApplication(app.id).subscribe(() => {
                      abp.notify.success(this.l(this.stringConstant.applicationDeleteMessage));
                      this.refresh();
                  });
              }
          }
      );
  }

/**
    * Method for showing toaster message 
    */
ShowToaster(){
  this.toaster.success(this.stringConstant.toasterMEssage);
 
}

/**
    * Method for toggling create abd edit dialog of master 
    */
private showCreateOrEditMasterDialog(id?: number): void {
  let createOrEditMasterDialog: BsModalRef;
  if (!id) {
      createOrEditMasterDialog = this._modalService.show(
          ApplicationCreateMasterComponent,
          {
              class: 'modal-dialog-centered',
          }
      );
  }
  else {
      createOrEditMasterDialog = this._modalService.show(
          ApplicationEditMasterComponent,
          {
              class: 'modal-dialog-centered',
              initialState: {
                  id: id,
              },
          }
      );
  }

  createOrEditMasterDialog.content.onSave.subscribe(() => {
      this.refresh();
  });
}


/**
    * Method exporting file to excel
    */
ExportToExcel(): void {
 
  this._applicationService.getUsersToExcel()
      .subscribe(result => {
        
          this.excelFileDownloadService.downloadTempFile(result);
        });
}

/**
    * Method to display create master dialog 
    */
createApplication(): void {
  this.showCreateOrEditMasterDialog();
}

  /**
    * Method for editing application
    * @param application:application to be edited
    */
   editApplication(application: ApplicationDto): void {
    this.showCreateOrEditMasterDialog(application.id);
}

}
