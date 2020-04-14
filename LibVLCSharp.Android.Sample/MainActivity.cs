using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using LibVLCSharp.Shared;
using VideoView = LibVLCSharp.Platforms.Android.VideoView;

namespace LibVLCSharp.Android.Sample
{
    [Activity(Label = "LibVLCSharp.Android.Sample", MainLauncher = true)]
    public class MainActivity : Activity
    {
        VideoView _videoView;
        LibVLC _libVLC;
        MediaPlayer _mediaPlayer;
        Media _media;

        LinearLayout mainLayout;


        float _position;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var button = FindViewById<Button>(Resource.Id.button_recreate);
            button.Click += Button_Click;

            Core.Initialize();

            _libVLC = new LibVLC();

            _mediaPlayer = new MediaPlayer(_libVLC)
            {
                EnableHardwareDecoding = true
            };

            //_media = new Media(_libVLC, "http://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4", FromType.FromLocation);
            _media = new Media(_libVLC, "rtsp://wowzaec2demo.streamlock.net/vod/mp4:BigBuckBunny_115k.mov", FromType.FromLocation);
            //_media = new Media(_libVLC, "rtsp://192.168.1.2:554/live/ch00_1", FromType.FromLocation);

            _mediaPlayer.Play(_media);
        }

        private void Button_Click(object sender, System.EventArgs e)
        {
            //_mediaPlayer.Stop();

            _videoView.MediaPlayer = null;
            mainLayout.RemoveView(_videoView);

            _videoView.Dispose();

            _videoView = new VideoView(this) { MediaPlayer = _mediaPlayer };
            mainLayout.AddView(_videoView, new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent));

            //_videoView.MediaPlayer.Play();
        }

        protected override void OnResume()
        {
            base.OnResume();

            _videoView = new VideoView(this) { MediaPlayer = _mediaPlayer };
            mainLayout = FindViewById<LinearLayout>(Resource.Id.linearLayout1);
            mainLayout.AddView(_videoView, new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent));
        }

        protected override void OnPause()
        {
            base.OnPause();

            //_videoView.MediaPlayer.Stop();
            //_mediaPlayer.Pause();
            _videoView.MediaPlayer = null;
            ((ViewGroup)_videoView.Parent).RemoveView(_videoView);

            _videoView.Dispose();
        }
    }
}
