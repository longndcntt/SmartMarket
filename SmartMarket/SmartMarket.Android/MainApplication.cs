using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Plugin.CurrentActivity;

namespace SmartMarket.Droid
{
#if DEBUG
    [Application(Debuggable = true, LargeHeap = true)]
#else
    [Application(Debuggable = false, LargeHeap = true)]
#endif
    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
    {
        static readonly string LOG_TAG = typeof(MainApplication).Name;

        public MainApplication(IntPtr handle, JniHandleOwnership transer)
            : base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            CrossCurrentActivity.Current.Init(this);
        }
        
        #region LifeCycle

        public override void OnTerminate()
        {
            base.OnTerminate();
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
        }

        #endregion
        
    }
    
    public static class XamForms
    {
        public static Context Context => CrossCurrentActivity.Current.Activity;
    }
    
}