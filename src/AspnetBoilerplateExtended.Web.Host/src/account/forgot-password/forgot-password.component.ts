import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { accountModuleAnimation } from '@shared/animations/routerTransition';
import { AppComponentBase } from '@shared/app-component-base';
import { UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { StringConstants } from '@shared/stringConstants';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css'],
  animations: [accountModuleAnimation()]
})
export class ForgotPasswordComponent extends AppComponentBase {

  hidden=false;
  resetEmail:string='';
  resetCode:string='';
  isLoading=false;
  submitMessage1:string;
  submitMessage2:string;
  submitMessage3:string;
  constructor(injector: Injector,
    private _userAppService:UserServiceProxy,
    private router:Router,
    private _stringConst:StringConstants ) {
      super(injector);

      this.submitMessage1=this._stringConst.sendResetLinkSuccessMessage1;
      this.submitMessage2=this._stringConst.sendResetLinkSuccessMessage2;
      this.submitMessage3=this._stringConst.sendResetLinkSuccessMessage3;
  }

  /**
   * Method of sending reset password link on email
   */
  SendResetLink(){

    this.isLoading=true;
    this._userAppService.getEmailOfUserForResetPassword(this.resetEmail).subscribe(x=>{
        this.resetCode=x
        this.isLoading=false;
        this.hidden=true;
        
    },
    err=>{
        this.isLoading=false;
    })

}

}
