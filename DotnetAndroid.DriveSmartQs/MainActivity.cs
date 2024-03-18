using Android.Graphics.Drawables;
using AndroidX.AppCompat.App;

namespace DotnetAndroid.DriveSmartQs;

[Activity(
    Label = "@string/app_name",
    Icon = "@mipmap/ic_launcher",
    RoundIcon = "@mipmap/ic_launcher_round",
    MainLauncher = true,
    Theme = "@style/Theme.MyApplication.NoActionBar",
    Exported = true)]
public class MainActivity : AppCompatActivity
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Set our view from the "main" layout resource
        SetContentView(Resource.Layout.activity_main);
    }
}
