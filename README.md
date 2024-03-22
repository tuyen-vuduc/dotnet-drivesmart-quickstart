# dotnet-drivesmart-quickstart

This is the ported example for Drive Smart libraries for Android. The original example can be found [here](https://github.com/DriveSmart-MobileTeam/dstracker_integration_sample).

# How to run the sample app

1/ Create `~/.gradle/gradle.properties` file using Visual Studio Code or VIM
2/ Ensure you have following lines
```
tfsdrivesmart_user=YOUR_USERNAME
tfsdrivesmart_password=YOUR_PASSWORD
```
3/ Set your apkId and userId in **defineConstants**

```c#
private void defineConstants()
{
    // TODO
    apkID = "";
    userID = "";
}
```


# How to integrate DriveSmart Android libraries to your project

1/ Add the NuGet package

```xml
<PackageReference Include="DriveSmart.DSTracker" Version="1.2.1.3" />
<PackageReference Include="Xamarin.Kotlin.StdLib" Version="1.9.23" />
<PackageReference Include="Xamarin.Kotlin.StdLib.Jdk7" Version="1.9.23" />
<PackageReference Include="Xamarin.AndroidX.Work.Work.Runtime.Ktx" Version="2.9.0.1" />
```

2/ Amend your CSPROJ file to have these lines
```xml
<ItemGroup>
  <GradleRepository Include="https://tfsdrivesmart.pkgs.visualstudio.com/5243836b-8777-4cb6-aded-44ab518bc748/_packaging/Android_Libraries/maven/v1">
    <Repository>
      maven {
        url 'https://tfsdrivesmart.pkgs.visualstudio.com/5243836b-8777-4cb6-aded-44ab518bc748/_packaging/Android_Libraries/maven/v1'
        name 'Android_Libraries'
        credentials {
          username 'YOUR_USER_NAME'
          password 'YOUR_PASSWORD'
        }
      }
</Repository>
  </GradleRepository>
</ItemGroup>
```

3/ Add required permissions to **AndroidMenifest.xml**

```xml
<!-- Services for user creation/query -->
<uses-permission android:name="android.permission.INTERNET" />
<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />

<!-- Trip evaluation -->
<uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION" />
<uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
<uses-permission android:name="android.permission.FOREGROUND_SERVICE" />

<!-- Automatic trip evaluation -->
<uses-permission android:name="android.permission.ACCESS_BACKGROUND_LOCATION" />
<uses-permission android:name="com.google.android.gms.permission.ACTIVITY_RECOGNITION" />
<uses-permission android:name="android.permission.ACTIVITY_RECOGNITION" />
```

**NOTE** 
- You can use PROPS file and/or gradle.properties file to hide your credentials from the VCS.
- [Native Android example](https://github.com/DriveSmart-MobileTeam/dstracker_integration_sample) is also very useful to check out if you face any issues