using UnityEngine;
using System.Reflection;
using Fluct;

public class RewardedVideoSceneController : MonoBehaviour
{
#if UNITY_ANDROID
    private string groupId = "1000090271"; // Android用 groupId
    private string unitId = "1000135434"; // Android用 unitId
#elif UNITY_IOS
    private string groupId = "1000085420"; // iOS用 groupId
    private string unitId = "1000127865"; // iOS用 unitId
#else
    private string groupId = ""; // ビルドエラー回避用
    private string unitId = ""; // ビルドエラー回避用
#endif
    private string userId = ""; // 追跡用のId(貴社アプリ内のユーザID等をご利用ください) null可

    private RewardedVideo rv;

    void Start()
    {
        // 初期処理
        rv = new RewardedVideo(groupId, unitId);

        // イベントハンドラ設定
        rv.OnDidLoad += HandleDidLoad;
        rv.OnDidOpen += HandleDidOpen;
        rv.OnDidClose += HandleDidClose;
        rv.OnShouldReward += HandleShouldReward;
        rv.OnDidFailToLoad += HandleDidFailToLoad;
        rv.OnDidFailToPlay += HandleDidFailToPlay;
    }

    void OnDestroy()
    {
        // イベントハンドラ解除
        rv.OnDidLoad -= HandleDidLoad;
        rv.OnDidOpen -= HandleDidOpen;
        rv.OnDidClose -= HandleDidClose;
        rv.OnShouldReward -= HandleShouldReward;
        rv.OnDidFailToLoad -= HandleDidFailToLoad;
        rv.OnDidFailToPlay -= HandleDidFailToPlay;
    }

    /// <summary>
    /// 広告読み込み
    /// </summary>
    public void Load()
    {
        rv.Load(targeting: new AdRequestTargeting(userId));
    }

    /// <summary>
    /// 広告再生
    /// </summary>
    public void Present()
    {
        // 広告再生可能の場合
        if (rv.HasAdAvailable())
        {
            // 広告再生
            rv.Present();
        }
    }

    /// <summary>
    /// 広告読み込み完了の際に呼ばれます
    /// </summary>
    public void HandleDidLoad(object sender, RewardedVideoEventArgs e)
    {
        Debug.LogFormat("DebugLog class={0} method={1} groupId={2} unitId={3} instancetID={4}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.GroupId, e.UnitId, this.GetInstanceID());
    }

    /// <summary>
    /// 広告画面表示直後に呼ばれます
    /// </summary>
    public void HandleDidOpen(object sender, RewardedVideoEventArgs e)
    {
        Debug.LogFormat("DebugLog class={0} method={1} groupId={2} unitId={3} instancetID={4}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.GroupId, e.UnitId, this.GetInstanceID());
    }

    /// <summary>
    /// 広告画面を閉じた直後に呼ばれます
    /// </summary>
    public void HandleDidClose(object sender, RewardedVideoEventArgs e)
    {
        Debug.LogFormat("DebugLog class={0} method={1} groupId={2} unitId={3} instancetID={4}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.GroupId, e.UnitId, this.GetInstanceID());
    }

    /// <summary>
    /// リワード付与すべき際に呼ばれます
    /// </summary>
    public void HandleShouldReward(object sender, RewardedVideoEventArgs e)
    {
        Debug.LogFormat("DebugLog class={0} method={1} groupId={2} unitId={3} instancetID={4}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.GroupId, e.UnitId, this.GetInstanceID());
    }

    /// <summary>
    /// 広告読み込み失敗の際に呼ばれます
    /// </summary>
    public void HandleDidFailToLoad(object sender, RewardedVideoErrorEventArgs e)
    {
        Debug.LogFormat("DebugLog class={0} method={1} groupId={2} unitId={3} errorMessage={4} instancetID={5}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.GroupId, e.UnitId, e.ErrorCode, this.GetInstanceID());
    }

    /// <summary>
    /// 広告表示失敗の際に呼ばれます
    /// </summary>
    public void HandleDidFailToPlay(object sender, RewardedVideoErrorEventArgs e)
    {
        Debug.LogFormat("DebugLog class={0} method={1} groupId={2} unitId={3} errorMessage={4} instancetID={5}", this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.GroupId, e.UnitId, e.ErrorCode, this.GetInstanceID());
    }
}
