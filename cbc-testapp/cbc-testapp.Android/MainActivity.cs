using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using cbc_testapp.Services;
using Android.Content;
using System.IO;
using cbc_testapp.Views;
using Android.Support.V4.Content;
using Android;
using Android.Support.V4.App;
using Android.Provider;
using Android.Widget;
using ZXing;
using ZXing.Common;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using cbc_testapp.Droid;
using Xamarin.Forms;

namespace cbc_testapp.Droid
{
    [Activity(Label = "cbc_testapp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]    
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            DependencyService.Register<LoadImageAndroid>();
            LoadApplication(new App());
        }

        public event EventHandler<ActivityResultEventArgs> ActivityResult;

        protected override void OnActivityResult(int requestCode, Android.App.Result resultCode, Intent data)
        {
            if (resultCode == Android.App.Result.Ok)
            {
                ActivityResult?.Invoke(this, new ActivityResultEventArgs { Intent = data });
            }
        }
        private int PERMISSION_REQUEST_CODE = 1;

        /// <summary>
        /// Check whether this application has permission to access the external storage
        /// </summary>
        public bool PermissionGrantedForExternalStorage
        {
            get
            {
                Permission permissionResult = ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage);
                if (permissionResult == Permission.Granted)
                {
                    // if permission is already granted return true otherwise return false
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Request to enable permission to write the files on external storage of android device
        /// </summary>
        public void RequestPermission()
        {
            if (!ActivityCompat.ShouldShowRequestPermissionRationale(this, Manifest.Permission.WriteExternalStorage))
            {
                ActivityCompat.RequestPermissions(this, new string[] { Manifest.Permission.WriteExternalStorage }, PERMISSION_REQUEST_CODE);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class ActivityResultEventArgs : EventArgs
    {
        public Intent Intent
        {
            get;
            set;
        }
    }

    public class LoadImageAndroid : ILoad
    {
        private static int SELECT_FROM_GALLERY = 0;
        private static int SELECT_FROM_CAMERA = 1;
        static Intent mainIntent;
        private Android.Net.Uri mImageCaptureUri;
        MainActivity activity;
        bool isCamera = false;
        private QRScanner page;
        Stream stream = new MemoryStream();

        public void UploadFromGallery(QRScanner editor)
        {
            isCamera = false;
            page = editor;
            activity = Xamarin.Forms.Forms.Context as MainActivity;
            activity.ActivityResult -= LoadImage;
            activity.ActivityResult += LoadImage;
            activity.Intent = new Intent();
            activity.Intent.SetType("image/*");
            activity.Intent.SetAction(Intent.ActionGetContent);
            activity.StartActivityForResult(Intent.CreateChooser(activity.Intent, "Select Picture"), SELECT_FROM_GALLERY);
        }


        void LoadImage(object sender, ActivityResultEventArgs e)
        {
            if (!isCamera)
            {
                var imagePath = GetPathToImage(e.Intent.Data);
                stream = System.IO.File.OpenRead(imagePath);
                page.SwitchView(imagePath);
            }
            else
            {
                mainIntent.PutExtra("image-path", mImageCaptureUri.Path);
                mainIntent.PutExtra("scale", true);
                page.SwitchView(mImageCaptureUri.Path);
            }
        }

        private string GetPathToImage(Android.Net.Uri uri)
        {
            string imgId = "";
            string[] proj = { MediaStore.Images.Media.InterfaceConsts.Data };
            using (var c1 = activity.ContentResolver.Query(uri, null, null, null, null))
            {
                try
                {
                    if (c1 == null) return "";
                    c1.MoveToFirst();
                    string imageId = c1.GetString(0);
                    imgId = imageId.Substring(imageId.LastIndexOf(":") + 1);
                }
                catch (System.Exception e)
                {
                    Toast.MakeText(Xamarin.Forms.Forms.Context, "Unable To Load Image", ToastLength.Short);
                }
            }

            string path = null;

            string selection = MediaStore.Images.Media.InterfaceConsts.Id + " =? ";
            bool value = activity.PermissionGrantedForExternalStorage;
            if (!value)
            {
                activity.RequestPermission();
            }
            using (var cursor = activity.ContentResolver.Query(MediaStore.Images.Media.ExternalContentUri, null, selection, new string[] { imgId }, null))
            {
                try
                {
                    if (cursor == null) return path;
                    var columnIndex = cursor.GetColumnIndexOrThrow(MediaStore.Images.Media.InterfaceConsts.Data);
                    cursor.MoveToFirst();
                    path = cursor.GetString(columnIndex);
                }
                catch (System.Exception e)
                {
                    Toast.MakeText(Xamarin.Forms.Forms.Context, "Unable To Load Image", ToastLength.Short);
                    return "";
                }
            }
            return path;

        }

        public BinaryBitmap GetBinaryBitmap(byte[] image)
        {
            Android.Graphics.Bitmap bitmap = Android.Graphics.BitmapFactory.DecodeByteArray(image, 0, image.Length);
            byte[] rgbBytes = GetRgbBytes(bitmap);
            var bin = new HybridBinarizer(new RGBLuminanceSource(rgbBytes, bitmap.Width, bitmap.Height));
            var i = new BinaryBitmap(bin);

            return i;
        }

        private byte[] GetRgbBytes(Android.Graphics.Bitmap image)
        {
            var rgbBytes = new List<byte>();
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    var c = new Android.Graphics.Color(image.GetPixel(x, y));

                    rgbBytes.AddRange(new[] { c.R, c.G, c.B });
                }
            }
            return rgbBytes.ToArray();
        }
    }
}