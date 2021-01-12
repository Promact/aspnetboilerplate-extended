import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ApplicationServiceProxy, CreateOrEditApplicationDto} from '../../shared/service-proxies/service-proxies'
import { StringConstants } from '@shared/stringConstants';
import { NotifyService } from 'abp-ng2-module';


@Component({
  selector: 'app-application-create-master',
  templateUrl: './application-create-master.component.html',
  styleUrls: ['./application-create-master.component.css']
})
export class ApplicationCreateMasterComponent implements OnInit {
  createApplication = new CreateOrEditApplicationDto();
  isLoading=false;
  saving = false;
  @Output() onSave = new EventEmitter<any>();

  constructor(  private _applicationService: ApplicationServiceProxy,
                private stringConstants:StringConstants,
                public bsModalRef: BsModalRef,
                private notify: NotifyService,
                ) { }

  ngOnInit(): void {
   

  }

   /**
     * Method for saving application data
     */
    save(): void {
      this.saving = true;
     
          this.isLoading = true;
          this._applicationService
              .createApplication(this.createApplication)
              .pipe(
                  finalize(() => {
                      this.saving = false;
                  })
              )
              .subscribe(() => {
                  this.notify.info((this.stringConstants.applicationAddMessage));
                  this.bsModalRef.hide();
                  this.onSave.emit();
                  this.isLoading = false;
              },
              err=>{
                  this.isLoading=false;
              }
              );
      

  

}
}
