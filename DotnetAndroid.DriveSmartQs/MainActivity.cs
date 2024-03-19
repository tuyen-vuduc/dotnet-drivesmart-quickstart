using Android;
using Android.OS;
using Android.Text.Method;
using AndroidX.AppCompat.App;
using Com.Dstracker.Enums;
using Com.Dstracker.Interfaces;
using Com.Dstracker.Models;
using Com.Dstracker.Singleton;
using Java.Lang;

namespace DotnetAndroid.DriveSmartQs;

[Activity(
    Label = "@string/app_name",
    Icon = "@mipmap/ic_launcher",
    RoundIcon = "@mipmap/ic_launcher_round",
    MainLauncher = true,
    Theme = "@style/Theme.MyApplication.NoActionBar",
    Exported = true)]
public class MainActivity : AppCompatActivity, IManagerInterface
{
    private Tracker dsTracker;

    private string apkID;
    private string userID;
    private Handler handlerTrip;
    private bool initialInfo = true;
    private string userSession = "";

    private TextView logText;
    private Button checkPermButton;
    private Button? startTripButton;
    private Button? stopTripButton;
    private Button? setUserButton;
    private Button? getUserButton;
    private TextView userId;
    private TextView permBatteryStatus;
    private TextView permGpsStatus;
    private TextView permGpsSemiStatus;
    private TextView permGpsAutoStatus;


    public static readonly string[] PERMISSIONS_GPS = {
        Manifest.Permission.AccessCoarseLocation,
        Manifest.Permission.AccessFineLocation
    };
    public static readonly string[] PERMISSIONS_GPS_AUTO = {
        Manifest.Permission.AccessCoarseLocation,
        Manifest.Permission.AccessCoarseLocation,
        Manifest.Permission.AccessBackgroundLocation
    };

    private void defineConstants()
    {
        // TODO
        apkID = "";
        userID = "";
    }

    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.activity_main);

        defineConstants();
        prepareEnvironment();
        prepareView();
    }

    private void prepareView()
    {
        updateTimerThread = new Runnable(() =>
        {
            TrackingStatus beanStatus = dsTracker.Status;

            if (initialInfo)
            {
                addLog("Trip ID: " + beanStatus.TripID);
                initialInfo = false;
            }

            addLog("Timer: " + convertMillisecondsToHMmSs(beanStatus.ServiceTime));
            addLog("Distance: " + beanStatus.TotalDistance);

            handlerTrip.PostDelayed(updateTimerThread, 2000);
        });

        handlerTrip = new Handler(MainLooper);
        logText = FindViewById<TextView>(Resource.Id.log_text);
        logText.MovementMethod = new ScrollingMovementMethod();

        checkPerms();
        checkPermButton = FindViewById<Button>(Resource.Id.check_perm_button);
        checkPermButton.Click += (s, e) => checkPerms();

        startTripButton = FindViewById<Button>(Resource.Id.start_trip_button);
        startTripButton.Click += (s, e) => dsTracker.Start();

        stopTripButton = FindViewById<Button>(Resource.Id.stop_trip_button);
        stopTripButton.Click += (s, e) => dsTracker.Stop();

        setUserButton = FindViewById<Button>(Resource.Id.set_user_button);
        setUserButton.Click += (s, e) =>
        {
            if (userSession != null)
            {
                identifyEnvironmet(userSession);
            }
            else
            {
                addLog("no user-session info");
            }
        };
        getUserButton = FindViewById<Button>(Resource.Id.get_user_button);
        userId = FindViewById<TextView>(Resource.Id.user_id);
        permBatteryStatus = FindViewById<TextView>(Resource.Id.perm_battery_status);
        permGpsStatus = FindViewById<TextView>(Resource.Id.perm_gps_status);
        permGpsSemiStatus = FindViewById<TextView>(Resource.Id.perm_gps_semi_status);
        permGpsAutoStatus = FindViewById<TextView>(Resource.Id.perm_gps_auto_status);
        getUserButton.Click += (s, e) =>
        {
            if (!string.IsNullOrWhiteSpace(userId.Text))
            {
                getOrAddUser(userId.Text);
            }
        };
    }

    private void checkPerms()
    {
        if (UtilsMethods.IsDozing(this))
        {
            permBatteryStatus.Text = (GetString(Resource.String.optimized));
        }
        else
        {
            permBatteryStatus.Text = (GetString(Resource.String.no_optimized));
        }

        if (UtilsMethods.IsGPSEnabled(this))
        {
            permGpsStatus.Text = (GetString(Resource.String.enabled));
        }
        else
        {
            permGpsStatus.Text = (GetString(Resource.String.disabled));
        }

        if (UtilsMethods.CheckPermissions(this, PERMISSIONS_GPS))
        {
            permGpsSemiStatus.Text = (GetString(Resource.String.ok));
        }
        else
        {
            permGpsSemiStatus.Text = (GetString(Resource.String.nok));
        }

        if (UtilsMethods.CheckPermissions(this, PERMISSIONS_GPS_AUTO))
        {
            permGpsAutoStatus.Text = (GetString(Resource.String.ok));
        }
        else
        {
            permGpsAutoStatus.Text = (GetString(Resource.String.nok));
        }
    }


    private void getOrAddUser(string user)
    {
        dsTracker.GetOrAddUserIdBy(user, dsResult =>
        {
            userSession = dsResult.ToString();
            addLog("User id created: " + dsResult);
        });
    }

    private void prepareEnvironment()
    {
        dsTracker = Tracker.GetInstance(this);
        dsTracker.Configure(apkID, dsResult =>
        {
            if (dsResult is Outcome.Success)
            {
                addLog("SDk configured");
                identifyEnvironmet(userID);
            }
            else
            {
                string error = ((Outcome.Error)dsResult).GetError().Description;
                addLog("Configure SDK: " + error);
            }
        });
    }

    private void identifyEnvironmet(string uid)
    {
        dsTracker.SetUserId(uid, result =>
        {
            addLog("Defining USER ID: " + uid);
        });
    }

    // ****************************************v****************************************
    // ******************************* Client Stuff ************************************
    // ****************************************v****************************************

    private void addLog(string text)
    {
        logText.Append("\n" + text);
    }

    private Runnable updateTimerThread;
    // ****************************************v****************************************
    // ******************************* Client Stuff ************************************
    // ****************************************v****************************************



    private string convertMillisecondsToHMmSs(long millisenconds)
    {
        long seconds = millisenconds / 1000;
        long s = seconds % 60;
        long m = seconds / 60 % 60;
        long h = seconds / (60 * 60) % 24;
        return string.Format("{0:D2}:{0:D2}:{0:D2}", h, m, s);
    }

    // ****************************************v****************************************
    // ******************** interface DSManagerInterface *******************************
    // ****************************************v****************************************


    private void showTripInfo(Outcome result)
    {
        if (result is Outcome.Success)
        {
            DSNotification notification = (DSNotification)((Outcome.Success)result).Data;
            if (notification.Ordinal() == DSNotification.DsRecordingTrip.Ordinal())
            {
                handlerTrip.PostDelayed(updateTimerThread, 500);
            }
        }
    }

    public void MotionDetectedActivity(DSInternalMotionActivities dsInternalMotionActivities, int i) { }

    public void MotionStatus(DSMotionEvents dsMotionEvents) { }

    public void OnPointerCaptureChanged(bool hasCapture)
    {
    }

    public void StartService(Outcome outcome)
    {
        addLog("Evaluating service: " + outcome);
        showTripInfo(outcome);
    }

    public void StatusEventService(Outcome outcome)
    {

    }

    public void StopService(Outcome outcome)
    {
        addLog("Stopping service: " + outcome);
        handlerTrip.RemoveCallbacks(updateTimerThread);
    }
    // ****************************************v****************************************
    // ******************** interface DSManagerInterface *******************************
    // ****************************************v****************************************
}