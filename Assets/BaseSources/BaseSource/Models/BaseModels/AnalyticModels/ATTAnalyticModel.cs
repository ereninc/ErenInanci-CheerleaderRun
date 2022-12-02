namespace CubeGames.Analytic
{
#if UNITY_IOS && ATT
    using Unity.Advertisement.IosSupport;
#endif

    public class ATTAnalyticModel : AnalyticBaseModel
    {
        public override void Initialize()
        {
            base.Initialize();

#if UNITY_IOS && ATT
            if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                ATTrackingStatusBinding.RequestAuthorizationTracking();
            }
#endif
        }

        public override string DefinationSymbol()
        {
            return "ATT";
        }
    }
}