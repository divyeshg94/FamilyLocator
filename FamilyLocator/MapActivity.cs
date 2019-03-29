using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Views;

public class CustomMapFragment : SupportMapFragment
{
    public new static CustomMapFragment NewInstance()
    {
        CustomMapFragment map = new CustomMapFragment();

        return map;
    }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        View v = base.OnCreateView(inflater, container, savedInstanceState);
        SetupMap(inflater);

        return v;
    }

    public void SetupMap(LayoutInflater inflater)
    {
        var map = NewInstance();
        LatLng location = new LatLng(50.332313, 18.939689);
        CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
        builder.Target(location);
        builder.Zoom(11);
        CameraPosition cameraPosition = builder.Build();

        UiSettings settings = map.UiSettings;

        settings.ZoomControlsEnabled = true;
        settings.ScrollGesturesEnabled = true;
        settings.CompassEnabled = true;

        CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);
        Map.MoveCamera(cameraUpdate);

        Map.AddMarker(CreateMarker(50.283985, 18.970299, "xx"));
        Map.AddMarker(CreateMarker(50.352212, 18.922032, "xxx"));
        Map.AddMarker(CreateMarker(50.368792, 18.847593, "xxx"));

        Map.SetInfoWindowAdapter(new CustomMarkerAdapter(inflater));
    }

    private MarkerOptions CreateMarker(double x, double y, string key)
    {
        MarkerOptions marker = new MarkerOptions();
        marker.SetPosition(new LatLng(x, y));
        marker.SetTitle(key);

        return marker;
    }

}