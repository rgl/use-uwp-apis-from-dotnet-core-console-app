This tries to use the UWP APIs from a dotnet core 3 console application.

Since I don't known how to set the application capabilities... this does not really work that well.

Currently, this can only set the lockscreen. Setting the wallpaper fails for some reason... if you can get this working, please let me known!

Probably this needs to be packaged as described at [Package desktop applications (Desktop Bridge)](https://docs.microsoft.com/en-us/windows/uwp/porting/desktop-to-uwp-root)...

Anyways, run with:

```powershell
dotnet run
```

You should see something like:

```plain
UserProfilePersonalizationSettings.IsSupported? True
HasSetWallpaper? False
Type=LocalUser AuthenticationStatus=LocallyAuthenticated NonRoamableId=1044480
User Property: lastName:System.String=
User Property: firstName:System.String=
User Property: accountName:System.String=
User Property: domainName:System.String=
User Property: providerName:System.String=
User Property: displayName:System.String=
User Property: principalName:System.String=
```
