using Newtonsoft.Json;

namespace Morin.Shared.Models;
public class RspJsonVideoModel
{
    [JsonProperty("vod_id")]
    public int VodId { get; set; }
    [JsonProperty("type_id")]
    public int TypeId { get; set; }
    [JsonProperty("type_id_1")]
    public int TypeId1 { get; set; }
    [JsonProperty("group_id")]
    public int GroupId { get; set; }
    [JsonProperty("vod_name")]
    public string? VodName { get; set; }
    [JsonProperty("vod_sub")]
    public string? VodSub { get; set; }
    [JsonProperty("vod_en")]
    public string? VodEn { get; set; }
    [JsonProperty("vod_status")]
    public int VodStatus { get; set; }
    [JsonProperty("vod_letter")]
    public string? VodLetter { get; set; }
    [JsonProperty("vod_color")]
    public string? VodColor { get; set; }
    [JsonProperty("vod_tag")]
    public string? VodTag { get; set; }
    [JsonProperty("vod_class")]
    public string? VodClass { get; set; }
    [JsonProperty("vod_pic")]
    public string? VodPic { get; set; }
    [JsonProperty("vod_pic_thumb")]
    public string? VodPicThumb { get; set; }
    [JsonProperty("vod_pic_slide")]
    public string? VodPicSlide { get; set; }
    [JsonProperty("vod_pic_screenshot")]
    public string? VodPicScreenshot { get; set; }
    [JsonProperty("vod_actor")]
    public string? VodActor { get; set; }
    [JsonProperty("vod_director")]
    public string? VodDirector { get; set; }
    [JsonProperty("vod_writer")]
    public string? VodWriter { get; set; }
    [JsonProperty("vod_behind")]
    public string? VodBehind { get; set; }
    [JsonProperty("vod_blurb")]
    public string? VodBlurb { get; set; }
    [JsonProperty("vod_remarks")]
    public string? VodRemarks { get; set; }
    [JsonProperty("vod_pubdate")]
    public string? VodPubdate { get; set; }
    [JsonProperty("vod_total")]
    public int VodTotal { get; set; }
    [JsonProperty("vod_serial")]
    public string? VodSerial { get; set; }
    [JsonProperty("vod_tv")]
    public string? VodTv { get; set; }
    [JsonProperty("vod_weekday")]
    public string? VodWeekday { get; set; }
    [JsonProperty("vod_area")]
    public string? VodArea { get; set; }
    [JsonProperty("vod_lang")]
    public string? VodLang { get; set; }
    [JsonProperty("vod_year")]
    public string? VodYear { get; set; }
    [JsonProperty("vod_version")]
    public string? VodVersion { get; set; }
    [JsonProperty("vod_state")]
    public string? VodState { get; set; }
    [JsonProperty("vod_author")]
    public string? VodAuthor { get; set; }
    [JsonProperty("vod_jumpurl")]
    public string? VodJumpurl { get; set; }
    [JsonProperty("vod_tpl")]
    public string? VodTpl { get; set; }
    [JsonProperty("vod_tpl_play")]
    public string? VodTplPlay { get; set; }
    [JsonProperty("vod_tpl_down")]
    public string? VodTplDown { get; set; }
    [JsonProperty("vod_isend")]
    public int VodIsend { get; set; }
    [JsonProperty("vod_lock")]
    public int VodLock { get; set; }
    [JsonProperty("vod_level")]
    public int VodLevel { get; set; }
    [JsonProperty("vod_copyright")]
    public int VodCopyright { get; set; }
    [JsonProperty("vod_points")]
    public int VodPoints { get; set; }
    [JsonProperty("vod_points_play")]
    public int VodPointsPlay { get; set; }
    [JsonProperty("vod_points_down")]
    public int VodPointsDown { get; set; }
    [JsonProperty("vod_hits")]
    public int VodHits { get; set; }
    [JsonProperty("vod_hits_day")]
    public int VodHitsDay { get; set; }
    [JsonProperty("vod_hits_week")]
    public int VodHitsWeek { get; set; }
    [JsonProperty("vod_hits_month")]
    public int VodHitsMonth { get; set; }
    [JsonProperty("vod_duration")]
    public string? VodDuration { get; set; }
    [JsonProperty("vod_up")]
    public int VodUp { get; set; }
    [JsonProperty("vod_down")]
    public int VodDown { get; set; }
    [JsonProperty("vod_score")]
    public string? VodScore { get; set; }
    [JsonProperty("vod_score_all")]
    public int VodScoreAll { get; set; }
    [JsonProperty("vod_score_num")]
    public int VodScoreNum { get; set; }
    [JsonProperty("vod_time")]
    public DateTime VodTime { get; set; }
    [JsonProperty("vod_time_add")]
    public int VodTimeAdd { get; set; }
    [JsonProperty("vod_time_hits")]
    public int VodTimeHits { get; set; }
    [JsonProperty("vod_time_make")]
    public int VodTimeMake { get; set; }
    [JsonProperty("vod_trysee")]
    public int VodTrysee { get; set; }
    [JsonProperty("vod_douban_id")]
    public int VodDoubanId { get; set; }
    [JsonProperty("vod_douban_score")]
    public string? VodDoubanScore { get; set; }
    [JsonProperty("vod_reurl")]
    public string? VodReurl { get; set; }
    [JsonProperty("vod_rel_vod")]
    public string? VodRelVod { get; set; }
    [JsonProperty("vod_rel_art")]
    public string? VodRelArt { get; set; }
    [JsonProperty("vod_pwd")]
    public string? VodPwd { get; set; }
    [JsonProperty("vod_pwd_url")]
    public string? VodPwdUrl { get; set; }
    [JsonProperty("vod_pwd_play")]
    public string? VodPwdPlay { get; set; }
    [JsonProperty("vod_pwd_play_url")]
    public string? VodPwdPlayUrl { get; set; }
    [JsonProperty("vod_pwd_down")]
    public string? VodPwdDown { get; set; }
    [JsonProperty("vod_pwd_down_url")]
    public string? VodPwdDownUrl { get; set; }
    [JsonProperty("vod_content")]
    public string? VodContent { get; set; }
    [JsonProperty("vod_play_from")]
    public string? VodPlayFrom { get; set; }
    [JsonProperty("vod_play_server")]
    public string? VodPlayServer { get; set; }
    [JsonProperty("vod_play_note")]
    public string? VodPlayNote { get; set; }
    [JsonProperty("vod_play_url")]
    public string? VodPlayUrl { get; set; }
    [JsonProperty("vod_down_from")]
    public string? VodDownFrom { get; set; }
    [JsonProperty("vod_down_server")]
    public string? VodDownServer { get; set; }
    [JsonProperty("vod_down_note")]
    public string? VodDownNote { get; set; }
    [JsonProperty("vod_down_url")]
    public string? VodDownUrl { get; set; }
    [JsonProperty("vod_plot")]
    public int VodPlot { get; set; }
    [JsonProperty("vod_plot_name")]
    public string? VodPlotName { get; set; }
    [JsonProperty("vod_plot_detail")]
    public string? VodPlotDetail { get; set; }
    [JsonProperty("type_name")]
    public string? TypeName { get; set; }
}
