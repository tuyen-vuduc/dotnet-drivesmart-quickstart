using Android.Content.PM;
using Android.Content;
using Android.Locations;
using Android.OS;
using AndroidX.Core.App;

namespace DotnetAndroid.DriveSmartQs;

public class UtilsMethods
{
    public static bool IsGPSEnabled(Context context)
    {
        LocationManager service = (LocationManager)context.GetSystemService(Context.LocationService);
        return service.IsProviderEnabled(LocationManager.GpsProvider);
    }

    public static bool IsDozing(Context context)
    {
        if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
        {
            PowerManager powerManager = (PowerManager)context.GetSystemService(Context.PowerService);
            return powerManager.IsDeviceIdleMode && !powerManager.IsIgnoringBatteryOptimizations(context.PackageName);
        }
        else
        {
            return false;
        }
    }

    public static bool CheckPermissions(Context context, string[] permissions)
    {
        bool isOK = false;
        foreach (string permission in permissions)
        {
            if (ActivityCompat.CheckSelfPermission(context, permission) != Permission.Granted)
            {
                isOK = false;
                break;
            }
            else { isOK = true; }
        }

        return isOK;
    }
}