# **User and Role Management**

## **Forgot Password**
-------------

**Purpose:** 
- Abp doesnt provide functionality of forgot password, so to implement it we need to create methods on serve side   
    - [SendResetPasswordLink<T>(T input) where T : class](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Application/Users/UserAppService.cs#L115) 
    - [SendMail(string DisplayName, string emailSubject, string emailBody, string emailAddress)](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Application/Users/UserAppService.cs#L77)
    - [ResetPasswordFromLink<T>(T input) where T : class](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Application/Users/UserAppService.cs#L173)

- You can find Server Side implementation [here](https://github.com/Promact/aspnetboilerplate-extended/tree/master-new/src/AspnetBoilerplateExtended.Application/Users), Client Side implementation of forgot password component [here](https://github.com/Promact/aspnetboilerplate-extended/tree/master-new/src/AspnetBoilerplateExtended.Web.Host/src/account/forgot-password) and reset password component [here](https://github.com/Promact/aspnetboilerplate-extended/tree/master-new/src/AspnetBoilerplateExtended.Web.Host/src/account/reset-password).

**Description:** 
- [SendResetPasswordLink<T>(T input) where T : class](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Application/Users/UserAppService.cs#L115) Generic method takes email as input of user and checks if user exist or not, if exists then send the mail of changing password with link in the mail.

```
public const string EmailRegex = "^[a-z0-9][-a-z0-9.!#$%&'*+-=?^_`{|}~\\/]+@([-a-z0-9]+\\.)+[a-z]{2,5}$";
```

- Add above email regex in [AccountAppService.cs](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Application/Authorization/Accounts/AccountAppService.cs) file.

- For sending e-mail, Use package of Mailkit (**Install-Package MailKit** run this command in Package Manager Console).

- [SendMail(string DisplayName, string emailSubject, string emailBody, string emailAddress)](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Application/Users/UserAppService.cs#L77) method send the mail on the basis of parametes passed in it.

```
"MailSetting": {
    "Username": "",
    "MailId": "",
    "Password": "",
    "Host": "smtp.gmail.com",
    "Port": "587"
  }
```
This are basic mail settings, add them in appsettings.json file which uses google smtp service to send the mail.

[ResetPasswordFromLink<T>(T input) where T : class](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Application/Users/UserAppService.cs#L173) Generic method takes New Password as input from user and updates it which will be stored in Abp.User table in hash format, so that user can log in to the tool.


## **Update Profile**
-------------

**Purpose:** 
- Abp doesnt provide functionality of updating user basic details, so to implement it we need to create methods on serve side 
  - [GetUserDetailsAsync(string id)](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Application/User-Update-Details/UserUpdateDetailsAppService.cs#L24) 
  - [UpdateUserDetails(UserUpdateDetailDto updateDetailDto)](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Application/User-Update-Details/UserUpdateDetailsAppService.cs#L62).

- You can find Server Side implementation [here](https://github.com/Promact/aspnetboilerplate-extended/tree/master-new/src/AspnetBoilerplateExtended.Application/User-Update-Details), Client Side implementation [here](https://github.com/Promact/aspnetboilerplate-extended/tree/master-new/src/AspnetBoilerplateExtended.Web.Host/src/app/update-user-details).

**Description:**
- Generic Repository implementation for updating user basic details like Name,Surname,Email,UserName etc.

- Create a Dto and Extend it from `EntityDto<long>` which will take Id property as long and add fields as required in it.
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

- [GetUserDetailsAsync(string id)](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Application/User-Update-Details/UserUpdateDetailsAppService.cs#L24) method will return details of user from user id passed in it as parameter.

- User id can be fetched from [app-auth.service.ts](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Web.Host/src/shared/auth/app-auth.service.ts#L53) file from authenticate method which return userId as Result and we can set it to localStorage.

- [UpdateUserDetails(UserUpdateDetailDto updateDetailDto)](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Application/User-Update-Details/UserUpdateDetailsAppService.cs#L62) methods update the details of user.


## Implementation of Role & Permissions with child permissions
**Purpose:** ASP.NET Boilerplate does not provide default functionality to add child permissions(Create/Edit/View/Delete) out of the box so to implement it we used its **AuthorizationProvider.**

**Description:**
ASP.NET Boilerplate defines a **permission based** infrastructure to implement authorization. The Authorization system uses 
**IPermissionChecker** to check permissions.
We need to define a permission before it is used. ASP.NET Boilerplate is designed to be modular, so different modules can have different permissions. A module should create a class derived from **AuthorizationProvider.**

**Server side Implementation**
-------------------------------

Add permission variables in PermissionNames file [here](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Core/Authorization/PermissionNames.cs)

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
Then create permission with **IPermissionDefinitionContext** in this file [here](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Core/Authorization/AspnetBoilerplateExtendedAuthorizationProvider.cs)

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
After creating an authorization provider, we should register it in the **PreInitialize** method of our module: **Path of file is** [here](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Application/AspnetBoilerplateExtendedApplicationModule.cs) 

```
Configuration.Authorization.Providers.Add< BoilerPlateDemo_AppAuthorizationProvider >();
```

These added permissions will be shown in **Role master=> permission tab** in checkbox form ,from where user can give permissions to particular role.

## Checking Permissions
**Using AbpAuthorize Attribute**


```
//A user can not execute this method if he is not granted the
 "PermissionNames.Pages_Applications_Create" permission.

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
# **Grid**

## **Pagination on listing pages**
-------------
**Purpose:**
Pagination  provides better and Organized user experience but boilerplate does not provide ready made pagination functionality. So main purpose is to provide insight to user about implementation of pagination functionality and what changes one should make to achieve desired output.

**Description:**
To include pagination in project you will need to wrap your data inside classes like `PagedResultDto` and send it as response and make follow below steps to know more.
-   At server side dont just return list of items individually but return it wrapped inside `PagedResultDto` object same as Below also provide total count of list while instantiating `PagedResultDto`.`PagedResultDto` ready made class provided by boilerplate you just need to import it before use.
    ````
     var totalCount = await filteredApplications.CountAsync();

            return new PagedResultDto<GetApplicationForViewDto>(
                totalCount,
                 await applications.ToListAsync()
            );
    ````

-   Now at client side extend your listing component class with `PagedListingComponentBase<classname>` which is provided by Boilerplate framework. It consist some abstract methods which needs to be declared so declare them first.
    ````
    export class ApplicationMasterComponent extends PagedListingComponentBase<ApplicationDto>{}
    ````
- As list method takes `PagedApplicatoinRquestDto` create that class and extend that with `PagedrequestDto` like below.
    ````
    class PagedApplicationRequestDto extends PagedRequestDto{

    keyword: string;
    sorting: string;
    }
    ````

- Initiate service call in `protected list(request: PagedApplicationRequestDto,pageNumber: number,finishedCallback: Function)` as shown below and don't forget to call `showpaging()` in subscribe block.
    ````
    protected list(
    request: PagedApplicationRequestDto,
    pageNumber: number,
    finishedCallback: Function): void {
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
    ````

-   Add paginate pipe provided by Boilerplate in HTML listing code. Parameter value assigned below(pageSize, pagenumber, totalItems) are properties of `PagedListingComponentBase`.
    ````
       <tr *ngFor="
                  let app of applications
                    | paginate
                      : {
                          id: 'server',
                          itemsPerPage: pageSize,
                          currentPage: pageNumber,
                          totalItems: totalItems
                        }
                ">

    ````
-   Add Pagination controls in HTML by adding follwing code in HTML
    ````
    div class="float-sm-right m-auto">
                                <abp-pagination-controls id="server"
                                                         (pageChange)="getDataPage($event)">
                                </abp-pagination-controls>
                            </div>
    ````



## **Search on listing pages**
-------------
**Purpose:**
A page can have multiple entries so finding the exact entry without search functionality is a cumbersome task and boiler plate does not provide this functionality. This sections will take you through certain changes which are reqired to add search functionality.

**Description:**
To add Search functionlity you will need to create some dto having filter properties and provide certain logic  to get filtered data in `Listing Method` and Add some design changes at client side to consume this functoinality. Follow below steps for detailed explaination.

- First of all create one dto having filter properties and extend that dto with ` PagedAndSortedResultRequestDto` like shown Below  
    ````
     public class GetAllApplicationInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }
    }
   ````
- Then Create another dto and extend it with `PagedApplicationResultRequestDto` class having `Keyword` property
    ````
    public class PagedApplicationResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
    ````
- Add newly created dto as parameter in listing methods on server side just like `GetAllApplicationInput` And add filter logic in code which will retrive filtered data.
    ````
      public async Task<PagedResultDto<GetApplicationForViewDto>> GetAllAsync(GetAllApplicationInput pageFormatData)
        {
           
            var filteredApplications = _applicationRepository.GetAll()
                 .WhereIf(!string.IsNullOrWhiteSpace(pageFormatData.Filter), e => false || e.ApplicationName.Trim().ToLower().Contains(pageFormatData.Filter.Trim().ToLower()))
                .WhereIf(!string.IsNullOrWhiteSpace(pageFormatData.NameFilter), e => false || e.ApplicationName.Trim().ToLower().Contains(pageFormatData.NameFilter.Trim().ToLower())).AsQueryable();
            var pagedAndFilteredApplications = filteredApplications.OrderBy(pageFormatData.Sorting??"id desc")
                .PageBy(pageFormatData);
            var applications = from o in pagedAndFilteredApplications
                               select new GetApplicationForViewDto()
                               {
                                   Application = new ApplicationDto
                                   {
                                       ApplicationName = o.ApplicationName,
                                       Id = o.Id,
                                       ProjectId=o.ProjectId,
                                       CreationTime = o.CreationTime,
                                   }
                               };

           
            var totalCount = await filteredApplications.CountAsync();

            return new PagedResultDto<GetApplicationForViewDto>(
                totalCount,
                 await applications.ToListAsync()
            );
    ````
- Declare Filter proprties in master component like [application-master.component.ts](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Web.Host/src/app/application-master/application-master.component.ts#L29)
    ````
    keyword = '';
    advancedFiltersVisible = false;
    filterText = '';
    nameFilter = '';
    ````    
- Add search box in HTML and bind it with  `property` using `ngmodel` and assign `click` event with `getDatapage` method.
    ````
    <input type="text"
    class="form-control"
    name="keyword"
    [placeholder]="'SearchWithThreeDot' | localize"
    [(ngModel)]="keyword"
    (keyup.enter)="getDataPage(1)" />
    ````
- Assign `keyword` to `request.keyword` property of request paramater passed in list method and then pass `request.keyword` in service call along with other filter properties as shown below
    ````
    this._applicationService
    .getAll(
    this.filterText,
    request.keyword,
    request.sorting,
    request.skipCount,
    request.maxResultCount,
    )
    ````
    NOTE: Make sure that parameter names in class and interfaces should remain same else it would not work 



## **Export to Excel**
-------------

**Purpose:**
    This functionality will export your data in excel file. It will introduce you with code which is required to export any data to excel.

**Description:**
    To achive this functionality you will need to copy some files which will generate your excel file and Add some code to download that generated file. We have used `OpenXML` package for file generation. Go through below steps for detailed explaination along with code reference.

 - First of all install `DocumentFormat.OpenXml` package by running following command,then copy all the necessary files which will be used in export functionality.
    ```
    Install-Package DocumentFormat.OpenXml 
    ````
**Files to be copied:**
 - copy [ExportToExcel](https://github.com/Promact/aspnetboilerplate-extended/tree/master-new/src/AspnetBoilerplateExtended.Application/ExportToExcelFile) directory from `AspnetBoilerplateExtended\src\AspnetBoilerplateExtended.Application\ExportToExcelFile\` to Application-project and register dependency  in `startup.cs` file i.e.
    ````
    //File Export dependecies injection
    services.AddScoped<IFileExport, FileExport>();
    ````
 - copy [CaheStorage](https://github.com/Promact/aspnetboilerplate-extended/tree/master-new/src/AspnetBoilerplateExtended.Core/CacheStorage) directory from`AspnetBoilerplateExtended\src\AspnetBoilerplateExtended.Core` to your Core-Project.
 - copy [filecontroller.cs](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Web.Core/Controllers/FileController.cs) from `AspnetBoilerplateExtended\src\AspnetBoilerplateExtended.Web.Core\Controllers` to Core-Project Controller Directory.

 NOTE:Most of the constant string value variable are stored in `Appconsts.cs` and `Stringconstants.ts` kindly copy or declare all the required constants before moving further,

After copying all the necessary files just navigate to DTO of the Entity which you want to export then add respective attributes on properties like:
 - Add `[Export(IsAllowExport=false)]` attribute on the top of property if you dont want to include that property as column in your exported excel sheet.   
    ````
    [Export(IsAllowExport = false)]
    public int Id { get; set; }
    ````
 - `[DisplayName("Name")]` attribute will display Name passed in parameted as column title in excel sheet.
    ````
    [DisplayName("CreationTime")]
    public string Time { get; set; }
    ````
 - Property without `[DisplayName("Name")]` will have same column title as their column name.

 - Inject `FileExport` and `TempFileCache` in constructor and add their respective declaration fields
    ````
        private readonly IFileExport _fileExportService;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IRepository<Project> _projectRepository;

        public ApplicationAppService(IFileExport fileExportService,
                                        ITempFileCacheManager tempFileCacheManager,
                                        )
        {
            _fileExportService = fileExportService;
            _tempFileCacheManager = tempFileCacheManager;
        
            }
    ````
Add FileExport method in your service class and pass desired list of item in `CreateWorksheetForExcel(itemstoexport)` rest of file generation code will remain same only change will be in paramter of the above mentioned method.These all were server side now switch to client side.
 - copy [ExcelFileDownloadService](https://github.com/Promact/aspnetboilerplate-extended/tree/master-new/src/AspnetBoilerplateExtended.Web.Host/src/shared/ExcelFileDownloadService) directory from `AspnetBoilerplateExtended\src\AspnetBoilerplateExtended.Web.Host\src\shared\ExcelFileDownloadService` to yourproject client side shared directory and add Download service in provideres of  [app.module.ts](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Web.Host/src/app/app.module.ts)

    ````
    Providers:[ExcelFileDownloadService]
    ````

 - After copying and regestering service in constructor of the component and call methods for export and download as shown below
    ````
    constructor(injector: Injector, private _applicationService: ApplicationServiceProxy, private stringConstant: StringConstants,
      private excelFileDownloadService:ExcelFileDownloadService) {
      super(injector);


     ExportToExcel(): void {
 
     this._applicationService.getUsersToExcel()
      .subscribe(result => {
        
          this.excelFileDownloadService.downloadTempFile(result);
        });
     }
    ````
 - call this method via html on click event
    ````
     <a class="btn btn-primary text-white" type="button" (click)="ExportToExcel()">Export</a>
    ````

# **Design**
 
## **Show 404 page when user trying to access the page for which he does not have rights**
-------------
 
**Purpose:**
Main purpose of this functionality is to prevent unauthorized access. It will show error page whenever user does not have permission to access that resource.
 
**Description:**
To add this you need to create component and check for permission using `abp.IsGranted("Permissionname")` and navigate user accordingly. Follow below steps for detailed explaination.
 
 - Step-1:Copy `error-page` directory in anugular `app` directory.`error-page` directory consists of `ts` and `HTML` file for error page.
 
 - Step-2:Add boolean variable into component to map with the permission just like `isviewGranted` in `Application-master-component.ts` like below.
    ````
    isCreateGranted = false;
    isEditGranted = false;
    isDeleteGranted = false;
    isViewGranted = false;
    ````
 - Step-3:Add 404 Component route in [app-routing.module.ts](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Web.Host/src/app/app-routing.module.ts)   
    ````
    { path: 'unauthorized',component:ErrorPageComponent}
    ````
 - Step-4:Check the permission status using `abp.auth.isGranted()` method it will return boolean and check for permission using condition statment and navigate to `404-component` if does not satisfy condition. for example
    ````
    constructor(private _router:Router){
 
    
      this.isCreateGranted = abp.auth.isGranted(this.stringConstant.applicationCreatePermission);
      this.isEditGranted = abp.auth.isGranted(this.stringConstant.applicationEditPermission);
      this.isDeleteGranted = abp.auth.isGranted(this.stringConstant.applicationDeletePermission);
      this.isViewGranted = abp.auth.isGranted(this.stringConstant.applicationViewPermission);
 
        if(!this.isViewGranted){
          this._router.navigate(["/app/unauthorized"])
      }
        }
    ````

## **Set tooltip on list pages**
-------------

**Purpose:**
Purpose of this functionality is to provide information about elements on which user hover the pointer, as it is not provided by Boilerplate this documentation will take you through all the steps.

**Description:**
To Add tooltip in any component you need to add `TooltipModule` and Add corresponding styles of it. Follow below steps for detailed Explaination.
 - Step-1:In this step, we will install ngx-bootstrap package, so we can use ngx-bootstrap css so let's install by following command:
    ````
     npm install ngx-bootstrap --save
    ````
 - Step-2:Import `TooltipModule` in `AppModule.ts`  
    ````
     //Tooltip
     import { TooltipModule } from 'ngx-bootstrap/tooltip';

     imports[  
     TooltipModule.forRoot(),]
    ````
 - Step-3:Add Tooltip in HTML component 
    ````
     <td class="text-center" tooltip="{{app.application.applicationName}}" placement="right" container="body">{{app.application.applicationName}}</td>
    ````



## **Searchable dropdowns**
-------------

**Purpose:**
Sometimes it is very hard to find required element from number of records so searchable dropdowns helps user in finding desired element by providing search funtionality inside dropdown.

**Description:**
To Add Searchable drop down first add `ngselect` package and add required styles in `package.json`for detailed explaination follow below steps.
 `@ng-select/ng-select` library provides search functionality in drop downs to add `ngselect` in project follow step mentioned below:

 - Step-1:Install ng-select library using below command
    ````
     npm install --save @ng-select/ng-select
    ````
 - Step-2:Add ng-select in import section of `app.module.ts`
    ````
     //ng-select
     import { NgSelectModule } from '@ng-select/ng-select';
     imports:[NgSelectModule ]
    ````
 - Step-3:Include theme to allow customization and theming, ng-select bundle includes only generic styles that are necessary for correct layout and positioning. To get full look of the control, include one of the themes in your application.Add style path in `angular.json`.
    ````
     "styles":[ "node_modules/@ng-select/ng-select/themes/default.theme.css"]
    ````
 - Step-4:Start usign `ng-select` in html file for Example.     
    ````
     <ng-select appendTo="body" [items]="projects" placeholder="Searchable DropDown" [(ngModel)]=" createApplication.projectId"  bindLabel="name" bindValue="id" name="projetcs">
     <ng-template ng-option-tmp let-item="item">
        <div title="{{item.name}}">{{item.name}}</div>
     </ng-template>
     </ng-select>
    ````




##  **Adding toastr for showing validation and success message**
-------------

**Purpose:**
Toastr Message is very crucial to provide a User friendly interface. This section will take you through all the steps which are required to add toastr message in project.
 
**Description:**
To add toastr message functionality first need to add `ToastrModule` and `ToastrService` Follwing are the steps to Add Toastr Message.

 - Step-1:In this step, we will install `ngx-toastr` and `@angular/animations` npm packages. so let's run both command as like bellow:
    ````
     npm install ngx-toastr --save
     npm install @angular/animations --save
    ````
 - Step-2:Now, we need to include toastr css like `"node_modules/ngx-toastr/toastr.css"`, so let's add it on [angular.json](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Web.Host/angular.json) file. So Navigate to angular.json file add add following style path.

    ````
     "styles": [
      "node_modules/ngx-toastr/toastr.css",
      "src/styles.css"
     ],
    ````
    
 - Step-3:In this step, we need to import ToastrModule and `BrowserAnimationsModule` to [app.module.ts](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Web.Host/src/app/app.module.ts) file. so let's import it as like bellow:
    ````
     //Toastr
     import { ToastrModule } from 'ngx-toastr';
     import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
    ````
NOTE:If you have imported Browser animation module in any of your root components then dont import it again it will cause error.(ex=> We have imported browser animation in [rootmodule.ts](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Web.Host/src/root.module.ts) so no need to import it again in [app.module.ts](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Web.Host/src/app/app.module.ts).


 - Step-4:Now Add `Toastr Module` in `declaration` of  [app.module.ts](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Web.Host/src/app/app.module.ts),Here You can also provide timeout, position and many other display properties.
    ````
     //Toastr
     ToastrModule.forRoot({
            timeOut: 3000,
            positionClass: 'toast-top-right',
            preventDuplicates: true,
        }),
    ````

 - Step-5:In this step We need to add `Toastr service` in  [app.module.ts](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Web.Host/src/app/app.module.ts) Provider array.
    ````
     providers: [
      ToastrService],
    ````

 - Step-6:Now Inject this service in your component class constructor.
    ````
     constructor(private toastr:ToastrService)
    ````

 - Step-7:You are all set to use  toastr service to display notification or messages in a very attractive manner.we have used it in `ShowToaster()`method of `Application-master` Component

    ````
     ShowToaster(){
     this.toaster.success(this.stringConstant.toasterMEssage);
 
        }   
    ````

        NOTE:There are multiple options like Success,Error and many more you can use it as per requirement.






## **Highlight respective menu item in side bar when user redirects from the other page**
-------------
 
**Purpose:**
In boilerplate when ever you navigate to any component (of which route is also mapped to any of the menu item in side bar) using hyper-link or address bar it does not highlight that menu item in side bar if there is parameter present in your routelink or Url. This Section will take you through solution of that issue.
 
**Descriprion:**
To highlight sidemenu bar item based on `Address` passed in Address bar You need to incorporate follwing changes in `Sidebar-menu.component.ts`.
 
 - Add menu item in sidebar menu.
    ````
     new MenuItem(
        this.l('Application'),
        '',
        'fab fa-adn','',[
          new MenuItem(
            this.l('ApplicationName'),
            '/app/application',
            'fab fa-adn',
           
          )
    ````
 - Add route link which will navigate you to the same component as menu item does just like below.
    ````
     <a class="btn btn-warning" [routerLink]="['/app/application/0']" >Go to Application</a>
    ````
 - Register both route in `app-routing.module.ts`
    ````
     { path: 'application', component: ApplicationMasterComponent },
     { path: 'application/:id', component: ApplicationMasterComponent }
    
    ````
 
 - Add following code in constructor of the `Sidebar-menu.component.ts`
    ````
     this.router.routeReuseStrategy.shouldReuseRoute = function () {
      return false;
     };
    ````
 - Replace `findMenuItemsByUrl` method with below code
    ````
     findMenuItemsByUrl(
        url: string,
        items: MenuItem[],
        foundedItems: MenuItem[] = []
        ): MenuItem[] {
        items.forEach((item: MenuItem) => {
        if (item.route === url) {
        foundedItems.push(item);
        }
        else if (item.route!=="" && url.includes(item.route)) {
        foundedItems.push(item);
        }
        else if (item.children) {
        this.findMenuItemsByUrl(url, item.children, foundedItems);
        }
        });
        return foundedItems;
     }
    ````
        NOTE:There is one link below homepage title named go to application which will highlight respective menu item if clicked.

## **UTC to IST time conversion**
-------------

**Purpose:**
Generally we use `DateTime.Now` property to set time instance to current time but sometimes it causes trouble to convert into IST due to time zone so this section will introduce you to a solution.

**Description:**
 Boiler Plate Provides `Abp.Timing` which generally solve this issue so rather using `DateTime.Now` use `Clock.Now`.
 NOTE:Clock.Now is provided by Abp.Timing package

 - Replace `DateTime.Now` Property with `Clock.Now` in code
    ````
     new Project{
     Name="Project1", CreationTime=Clock.Now, CreatorUserId=1,IsDeleted=false },
    ````
    ````
        if (isMatch)
            {
                application.CreationTime = Clock.Now;
                await _applicationRepository.InsertAsync(application);
            }
    ````
    ````
    NOTE: Storing Time property using `Clock.Now` in `CreateApplication` method
    ````



## **Mapping of class into respective dto class or vice versa using CustomDtoMapper**
-------------

**Purpose:**
Whenever you map your class to any dto using `Object.Mapper()` Function it requires Mapping Declaration in Seperate mapping file like [CustomeDtoMapper.cs](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Application/CustomDtoMapper.cs) into your Application Project else it will throw the `Unsupported mapping error` To avoid This Error follow this article.
 
**Descrption:**
Mapping of class into Dto class can be done using `Object.Mapper<OutputType>(ObjectYouWanttoMap)` function where object you want map is passed as paramater in that function.
    ````
     ApplicationDtoList = ObjectMapper.Map<List<ApplicationDto>>(ApplicationList);
    ````
Above code will convert `ApplicatoinList` into `ApplicationDtoList` and will store it in ApplicationDtoList variable. But Using this type of method in mapping requires certain prerequisites which are as follows.
 - Copy [CustomeDtoMapper.cs](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Application/CustomDtoMapper.cs) into your Application Project
 
 - Add mapping of you Dto and class in [CustomeDtoMapper.cs](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Application/CustomDtoMapper.cs).
    ````
     internal class CustomDtoMapper
     {

        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditApplicationDto, Application>().ForMember(dto => dto.Id, options => options.Ignore()).ReverseMap();

        }
     }
    ````
 - Add follwing configuration in `ProjectNameApplicationModule.cs` located in application-project just like  [BoilerPlateDemo_AppApplicationModule.cs](https://github.com/Promact/aspnetboilerplate-extended/blob/master-new/src/AspnetBoilerplateExtended.Application/AspnetBoilerplateExtendedApplicationModule.cs);
    ````
     public override void PreInitialize()
     {
     Configuration.Authorization.Providers.Add<BoilerPlateDemo_AppAuthorizationProvider>();
     Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);

     }
    ````
