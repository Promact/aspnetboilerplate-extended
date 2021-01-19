import { Component, EventEmitter, Injector, OnInit, Output } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ApplicationServiceProxy, CreateOrEditApplicationDto, GetApplicationForEditOutput, Project } from '@shared/service-proxies/service-proxies';
import { StringConstants } from '@shared/stringConstants';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'app-application-edit-master',
  templateUrl: './application-edit-master.component.html',
  styleUrls: ['./application-edit-master.component.css']
})
export class ApplicationEditMasterComponent extends AppComponentBase implements OnInit {
  saving = false;
  application = new GetApplicationForEditOutput();
  id: number;
  applicationEdit: CreateOrEditApplicationDto = new CreateOrEditApplicationDto();
  projects:Project[]=[]
  selectedApplication = true;
  isLoading=false;
  isApplicationEditGranted = false;
  isApplicationViewGranted = false;
  @Output() onSave = new EventEmitter<any>();
  constructor(
    injector: Injector,
    public _applicationService: ApplicationServiceProxy,
    public bsModalRef: BsModalRef, private stringConstants: StringConstants,
    public toaster:ToastrService

  ) {
    super(injector);
    this.isApplicationEditGranted = abp.auth.isGranted(this.stringConstants.applicationEditPermission);
        this.isApplicationViewGranted = abp.auth.isGranted(this.stringConstants.applicationViewPermission);
   }

  ngOnInit(): void {

    let selectedPage = location.href;
   
        this._applicationService.getAllProjects().subscribe(x=>{
          this.projects=x;
        })
        this._applicationService.getApplicationForEdit(this.id).subscribe((result) => {
            this.application = result;
            this.applicationEdit.id = this.application.applications.id;
            this.applicationEdit.applicationName = this.application.applications.applicationName;
            this.applicationEdit.projectId=this.application.applications.projectId;
        });
    
  }

  
    /**
     * Method for saving application data after editing
     */
   save(): void {
    this.isLoading=true;
    this.saving = true;
    if (this.selectedApplication) {
        this.isLoading = true;
        this._applicationService
            .createOrEdit(this.applicationEdit)
            .pipe(
                finalize(() => {
                    this.saving = false;
                })
            )
            .subscribe(() => {
                this.toaster.info(this.l(this.stringConstants.applicationEditMessage));
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

}
