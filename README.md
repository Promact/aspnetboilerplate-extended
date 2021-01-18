# Update Basic User Details

Generic Repository implementation for updating user basic details like Name,Surname,Email,UserName etc in Application Project.

Create a Dto and Extend it from `EntityDto<long>` which will take Id property as long and add other required fields in it.
```
public class UserUpdateDetailDto : EntityDto<long>
    {
        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(AbpUserBase.MaxNameLength)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Surname field is required.")]
        [StringLength(AbpUserBase.MaxSurnameLength)]
        public string Surname { get; set; }

        [Required(ErrorMessage = "The UserName field is required.")]
        [StringLength(AbpUserBase.MaxUserNameLength)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "The EmailAddress field is required.")]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
    }
```

[GetUserDetailsAsync(string id)](https://github.com/Promact/aspnetboilerplate-extended/blob/master/src/BoilerPlateDemo_App.Application/User-Update-Details/UserUpdateDetailsAppService.cs#L22) method will return details of user with user id passed in it.

User id can be fetched from [app-auth.service.ts](https://github.com/Promact/aspnetboilerplate-extended/blob/master/src/BoilerPlateDemo_App.Web.Host/src/shared/auth/app-auth.service.ts) file from authenticate method which return userId as Result and we can set it to localStorage.

[UpdateUserDetails(UserUpdateDetailDto updateDetailDto)](https://github.com/Promact/aspnetboilerplate-extended/blob/master/src/BoilerPlateDemo_App.Application/User-Update-Details/UserUpdateDetailsAppService.cs#L55) methods update the details of user.



# Forgot Password 

[SendResetPasswordLink<T>(T input) where T : class](https://github.com/Promact/aspnetboilerplate-extended/blob/master/src/BoilerPlateDemo_App.Application/Users/UserAppService.cs#L101) Generic method takes email of user and checks if user exist or not, if exists then send the mail of changing password with link in the mail.

For Email, Used package of Mailkit(`Install-Package MailKit` run this command in Package Manager Console).

[SendMail(string DisplayName, string emailSubject, string emailBody, string emailAddress)](https://github.com/Promact/aspnetboilerplate-extended/blob/master/src/BoilerPlateDemo_App.Application/Users/UserAppService.cs#L68) method send the mail on the basis of parametes passed in it.

```
"MailSetting": {
    "Username": "",
    "MailId": "",
    "Password": "",
    "Host": "smtp.gmail.com",
    "Port": "587"
  }
```
This are basic mail setting which uses google smtp service to send the mail.

[ResetPasswordFromLink<T>(T input) where T : class](https://github.com/Promact/aspnetboilerplate-extended/blob/master/src/BoilerPlateDemo_App.Application/Users/UserAppService.cs#L159) Generic method takes New Password of user and updates it, so that user can log in to the tool.


# Implementation of Roles and Permission with child permission
ASP.NET Boilerplate defines a�permission based�infrastructure to implement authorization. The Authorization system uses�IPermissionChecker�to check permissions.
We need to define a permission before it is used. ASP.NET Boilerplate is designed to be�modular, so different modules can have different permissions. A module should create a class derived from AuthorizationProvider.

# Usage
Server side Implementation
Add permission variables in PermissionNames file [here](https://github.com/Promact/aspnetboilerplate-extended/blob/master/src/BoilerPlateDemo_App.Core/Authorization/PermissionNames.cs)
```
public static class PermissionNames
    {
       public const string Pages_Applications = "Pages.Applications";
        public const string Pages_Applications_Create = "Pages.Applications.Create";
        public const string Pages_Applications_Edit = "Pages.Applications.Edit";
        public const string Pages_Applications_Delete = "Pages.Applications.Delete";
        public const string Pages_Applications_View = "Pages.Applications.View";
    }
```
Then create permission with IPermissionDefinitionContext in this file [here](https://github.com/Promact/aspnetboilerplate-extended/blob/master/src/BoilerPlateDemo_App.Core/Authorization/BoilerPlateDemo_AppAuthorizationProvider.cs)
Ex:
```
#region application
public class BoilerPlateDemo_AppAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var app = context.CreatePermission(PermissionNames.Pages_Applications, L("Applications"));
            app.CreateChildPermission(PermissionNames.Pages_Applications_Create, L("Applications.Create"));
            app.CreateChildPermission(PermissionNames.Pages_Applications_Edit, L("Applications.Edit"));
            app.CreateChildPermission(PermissionNames.Pages_Applications_Delete, L("Applications.Delete"));
            app.CreateChildPermission(PermissionNames.Pages_Applications_View, L("Applications.View"));
            #endregion
}
}
```
Permissions can have parent and child permissions. While this does not affect permission checking, it helps to group the permissions in the UI.
After creating an authorization provider, we should register it in the PreInitialize method of our module:
Configuration.Authorization.Providers.Add< BoilerPlateDemo_AppAuthorizationProvider >();

These added permissions will be shown in role master=> permission tab in checkbox form ,from where user can give permissions to particular role.
Checking Permissions
Using AbpAuthorize Attribute
//A user can not execute this method if he is not granted the
"PermissionNames.Pages_Applications_Create " permission.
```
[AbpAuthorize(PermissionNames.Pages_Applications_Create)] 
public async Task CreateApplicationAsync(CreateOrEditApplicationDto input)
{ 

////A user can not reach this point if he is not granted for PermissionNames.Pages_Applications_Create permission.

if (!PermissionChecker.IsGranted(PermissionNames.Pages_Applications_Create))
    {
        throw new AbpAuthorizationException("You are not authorized to create application!");
    }

}
```
The CreateApplicationAsync method can not be called by a user who is not granted the permission "PermissionNames.Pages_Applications_Create ".
The AbpAuthorize attribute also checks if the current user is logged in (using�IAbpSession.UserId). If we declare an AbpAuthorize for a method, it only checks for the login:
```
[AbpAuthorize]
public void SomeMethod(SomeMethodInput input)
{
    //A user can not execute this method if he did not login.
}
```
Add permission to client side
we need to check if the current user has a specific permission (with permission name).
In app-routing.module.ts , Add permission attribute in route
```
@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: AppComponent,
                children: [
{ path: 'masters/applications', component: MastersApplicationComponent, data: { permission: 'Pages.Applications' }, canActivate: [AppRouteGuard]}]                 
            },])]
```
To show /hide actions according to  child permission
Create variables using abp.auth.isGranted(assigned permission) 
Add permission variable in string constant file in angular
```
    applicationCreatePermission = 'Pages.Applications.Create';
    applicationEditPermission = 'Pages.Applications.Edit';
    applicationDeletePermission = 'Pages.Applications.Delete';
    applicationViewPermission = 'Pages.Applications.View';
```
Use this variables in component.ts
Ex:
```
    this.isCreateGranted = abp.auth.isGranted(this.stringConstant.applicationCreatePermission);
    this.isEditGranted = abp.auth.isGranted(this.stringConstant.applicationEditPermission);
    this.isDeleteGranted = abp.auth.isGranted(this.stringConstant.applicationDeletePermission);
    this.isViewGranted = abp.auth.isGranted(this.stringConstant.applicationViewPermission);
```
Use this variable in html to show/hide actions 
Ex:
For create permission
```
<div class="col-6 text-right">
<a href="javascript:;" class="btn btn-export medium" (click)="createApplication()" *ngIf="isCreateGranted">
          <i class="fa fa-plus-square"></i>
              {{ "Create" | localize }}
    </a>
   </div>
```
Ex:
For edit/view permission
```
<button type="button" class="btn btn-sm btn-table-action" (click)="editApplication(app.application)� tooltip="Edit" placement="top" container="body">
        <i class="fas fa-pencil-alt" [hidden]="!isEditGranted"></i>
         <i class="fas fa-eye" [hidden]="isEditGranted && isViewGranted"></i>
  </button>
```
Ex:
For delete permission
```
<button type="button" class="btn btn-sm btn-table-action ml-2" (click)="delete(app.application)" *ngIf="isDeleteGranted"  tooltip="Delete" placement="top" container="body">
 <i class="fas fa-trash"></i>
   </button>
```








