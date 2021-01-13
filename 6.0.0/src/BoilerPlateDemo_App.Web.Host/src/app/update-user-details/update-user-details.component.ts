import { Component, Injector, OnInit } from '@angular/core';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { UserUpdateDetailDto, UserUpdateDetailsServiceProxy } from '@shared/service-proxies/service-proxies';
import { StringConstants } from '@shared/stringConstants';

@Component({
  selector: 'app-update-user-details',
  templateUrl: './update-user-details.component.html',
  styleUrls: ['./update-user-details.component.css'],
  animations: [appModuleAnimation()]
})
export class UpdateUserDetailsComponent extends AppComponentBase implements OnInit {

  saving = false;
  updateDetailDto:UserUpdateDetailDto = new UserUpdateDetailDto();
  constructor(injector: Injector,private _updateDetailsService:UserUpdateDetailsServiceProxy,
    private _stringConst:StringConstants) {
    super(injector);
  }

  ngOnInit(): void {
    var id = localStorage.getItem('LoggedInUserId');
    //Gets current user data
    this._updateDetailsService.getUserDetails(id).subscribe(
      (data) => {
        this.updateDetailDto = data;
      }
    );
  }
  /**
   * Method to update the details of user
   */
  save(){
    this._updateDetailsService.updateUserDetails(this.updateDetailDto).subscribe(
      () => {

        this.notify.info(this._stringConst.updateSuccessfullyMessage);
      }
    )
  }
}
