# dotnet-drivesmart-quickstart

This is the ported example for Drive Smart libraries for Android. The original example can be found [here](https://github.com/DriveSmart-MobileTeam/dstracker_integration_sample).

# How to run the sample app

1/ Create `~/.gradle/gradle.properties` file using Visual Studio Code or VIM
2/ Ensure you have following lines
```
tfsdrivesmart_user=YOUR_USERNAME
tfsdrivesmart_password=YOUR_PASSWORD
```

# How to integrate DriveSmart Android libraries to your project

1/ Add the NuGet package

```
<PackageReference Include="DriveSmart.DSTracker" Version="1.2.1.2" />
```

2/ Amend your CSPROJ file to have these lines
```
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
**NOTE** You can use PROPS file and/or gradle.properties file to hide your credentials from the VCS.