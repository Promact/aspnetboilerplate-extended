import { Component, Injector, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { AppConsts } from '@shared/AppConsts';
import { ResetPasswordFromLinkDto, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { StringConstants } from '@shared/stringConstants';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css'],
  animations: [accountModuleAnimation()]
})
export class ResetPasswordComponent extends AppComponentBase {

  resetPasswordModel:ResetPasswordFromLinkDto={
    clone:undefined,
    confirmPassword:'',
    newPassword:'',
    init:undefined,
    toJSON:undefined,
    passwordToken:'',
    
    
};
loginLink:string;
hidden=false;
isLoading=false;
resetMessage1:string;
resetMessage2:string;
resetMessage3:string;
constructor(
    injector: Injector,
    private _route:ActivatedRoute,
    private _userService:UserServiceProxy,
    private _router:Router,
    private _stringConst:StringConstants

) {
    super(injector);
    this.resetMessage1=this._stringConst.passwordUpdateSuccessMessage1;
    this.resetMessage2=this._stringConst.passwordUpdateSuccessMessage2;
    this.resetMessage3=this._stringConst.passwordUpdateSuccessMessage3;
    this.loginLink=AppConsts.appBaseUrl+this._stringConst.loginlink;
}

/**
 * lifecycle hook  which is called while initialization
 */
ngOnInit(): void {
 
    this.resetPasswordModel.passwordToken=this._route.snapshot.paramMap.get('id');
    
    
}

/**
 * Method of reseting  password
 */
ResetPassowrd(){
    this.isLoading=true;
    this._userService.resetPasswordOfUser(this.resetPasswordModel).subscribe((x)=>{
    this.hidden=true;
    this.isLoading=false;
    },
    err=>{
        this.isLoading=false;
    });
}

}
