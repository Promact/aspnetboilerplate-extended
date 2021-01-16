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

[GetUserDetailsAsync(string id)](https://github.com/Promact/aspnetboilerplate-extended/blob/master/6.0.0/src/BoilerPlateDemo_App.Application/User-Update-Details/UserUpdateDetailsAppService.cs#L22) method will return details of user with user id passed in it.

User id can be fetched from [app-auth.service.ts](https://github.com/Promact/aspnetboilerplate-extended/blob/master/6.0.0/src/BoilerPlateDemo_App.Web.Host/src/shared/auth/app-auth.service.ts) file from authenticate method which return userId as Result and we can set it to localStorage.

[UpdateUserDetails(UserUpdateDetailDto updateDetailDto)](https://github.com/Promact/aspnetboilerplate-extended/blob/master/6.0.0/src/BoilerPlateDemo_App.Application/User-Update-Details/UserUpdateDetailsAppService.cs#L55) methods update the details of user.



# Forgot Password 

[SendResetPasswordLink<T>(T input) where T : class](https://github.com/Promact/aspnetboilerplate-extended/blob/master/6.0.0/src/BoilerPlateDemo_App.Application/Users/UserAppService.cs#L101) Generic method takes email of user and checks if user exist or not, if exists then send the mail of changing password with link in the mail.

For Email, Used package of Mailkit(`Install-Package MailKit` run this command in Package Manager Console).

[SendMail(string DisplayName, string emailSubject, string emailBody, string emailAddress)](https://github.com/Promact/aspnetboilerplate-extended/blob/master/6.0.0/src/BoilerPlateDemo_App.Application/Users/UserAppService.cs#L68) method send the mail on the basis of parametes passed in it.

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

[ResetPasswordFromLink<T>(T input) where T : class](https://github.com/Promact/aspnetboilerplate-extended/blob/master/6.0.0/src/BoilerPlateDemo_App.Application/Users/UserAppService.cs#L159) Generic method takes New Password of user and updates it, so that user can log in to the tool.



