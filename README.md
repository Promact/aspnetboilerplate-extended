?# **User and Role Management**

## **Forgot Password**
------------
## **Update Profile**
-------------
## Implementation of Role & Permissions with child permissions
-------------------------------------------------------------
**Purpose:**
 ASP.NET Boilerplate does not provide default functionality to add child permissions(Create/Edit/View/Delete) out of the box so to implement it we used its **AuthorizationProvider.**

**Description:**
ASP.NET Boilerplate defines a **permission based** infrastructure to implement authorization. The Authorization system uses 
**IPermissionChecker** to check permissions.
We need to define a permission before it is used. ASP.NET Boilerplate is designed to be modular, so different modules can have different permissions. A module should create a class derived from **AuthorizationProvider.**

**Server side Implementation**
-------------------------------

Add permission variables in PermissionNames file [here](https://github.com/Promact/aspnetboilerplate-extended/blob/master/src/BoilerPlateDemo_App.Core/Authorization/PermissionNames.cs)

These variables will be used in creating permissions with the help of AuthorizationProvider.
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
Then create permission with **IPermissionDefinitionContext** in this file [here](https://github.com/Promact/aspnetboilerplate-extended/blob/master/src/BoilerPlateDemo_App.Core/Authorization/BoilerPlateDemo_AppAuthorizationProvider.cs)

Create main module permission with its child permission if any.
```
Ex:
public class BoilerPlateDemo_AppAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            var app = context.CreatePermission(PermissionNames.Pages_Applications, L("Applications"));
            app.CreateChildPermission(PermissionNames.Pages_Applications_Create, L("Applications.Create"));
            app.CreateChildPermission(PermissionNames.Pages_Applications_Edit, L("Applications.Edit"));
            app.CreateChildPermission(PermissionNames.Pages_Applications_Delete, L("Applications.Delete"));
            app.CreateChildPermission(PermissionNames.Pages_Applications_View, L("Applications.View"));
          
	}
 }
```
Permissions can have parent and child permissions. While this does not affect permission checking, it helps to group the permissions in the UI.
After creating an authorization provider, we should register it in the **PreInitialize** method of our module: **Path of file is** [here]( https://github.com/Promact/aspnetboilerplate-extended/blob/master/src/BoilerPlateDemo_App.Application/BoilerPlateDemo_AppApplicationModule.cs) 

```
Configuration.Authorization.Providers.Add< BoilerPlateDemo_AppAuthorizationProvider >();
```

These added permissions will be shown in **Role master=> permission tab** in checkbox form ,from where user can give permissions to particular role.

# Checking Permissions
**Using AbpAuthorize Attribute**
```
//A user can not execute this method if he is not granted the
**"PermissionNames.Pages_Applications_Create"** permission.

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
The CreateApplicationAsync method can not be called by a user who is not granted the permission **"PermissionNames.Pages_Applications_Create".**
The AbpAuthorize attribute also checks if the current user is logged in. If we declare an AbpAuthorize for a method, it only checks for the login:
```
[AbpAuthorize]
public void SomeMethod(SomeMethodInput input)
{
    //A user can not execute this method if he did not login.
}
```

**Add permission to client side**
---------------------------------

we need to check if the current user has a specific permission (with permission name).
In **app-routing.module.ts** , Add permission attribute in route
```
@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: AppComponent,
                children: [
{ path: 'masters/applications', component: MastersApplicationComponent, **data: { permission: 'Pages.Applications'** }, canActivate: [AppRouteGuard]}]                 
            },])]
```
To show /hide actions according to  child permission
create variables using **abp.auth.isGranted(assigned permission)**.

Add permission variable in string constant file in angular
```
    applicationCreatePermission = 'Pages.Applications.Create';
    applicationEditPermission = 'Pages.Applications.Edit';
    applicationDeletePermission = 'Pages.Applications.Delete';
    applicationViewPermission = 'Pages.Applications.View';
```
Use these variables in component.ts. Inject string constant file in component and use  variables of it.

Ex:
```
export class ApplicationMasterComponent extends PagedListingComponentBase<ApplicationDto> {
isCreateGranted =false;
isEditGranted =false;
isDeleteGranted =false;
isViewGranted =false;

constructor(private stringConstant: StringConstants){ 

super(injector);
    this.isCreateGranted = abp.auth.isGranted(this.stringConstant.applicationCreatePermission);
    this.isEditGranted = abp.auth.isGranted(this.stringConstant.applicationEditPermission);
    this.isDeleteGranted = abp.auth.isGranted(this.stringConstant.applicationDeletePermission);
    this.isViewGranted = abp.auth.isGranted(this.stringConstant.applicationViewPermission);
}
}
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
<button type="button" class="btn btn-sm btn-table-action" (click)="editApplication(app.application)? tooltip="Edit" placement="top" container="body">
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
# **Grid**

## **Pagination on listing pages**
-------------



## **Search on listing pages**
-------------
## **Export to Excel**
-------------

# **Design**

## **Show 404 page when user trying to access the page for which he does not have rights**
-------------


## **Set tooltip on list pages**
-------------
## **Searchable dropdowns**
------------
##  **Adding toaster for showing validation and success message**
-------------
    
## **Highlight respective menu item in side bar when user redirects from the other page**
-------------

## **UTC to IST time conversion**
-------------

## **Mapping of class into respective dto class or vice versa using CustomDtoMapper**
-------------

