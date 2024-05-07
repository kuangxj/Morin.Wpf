namespace Morin.Shared.Models;

public class VideoModel : Model, ICloneable
{
    public object Clone()
    {
        return MemberwiseClone();//浅拷贝
    }

    public int Pid { get; set; }

    public int SourceID { get; set; }
    public string? SourceTitle { get; set; }

    public int VodId { get; set; }
    public int TypeId { get; set; }
    public int TypeId1 { get; set; }
    public int GroupId { get; set; }
    public string? VodName { get; set; }
    public string? VodSub { get; set; }
    public string? VodEn { get; set; }
    public int VodStatus { get; set; }
    public string? VodLetter { get; set; }
    public string? VodColor { get; set; }
    public string? VodTag { get; set; }
    public string? VodClass { get; set; }
    public string? VodPic { get; set; } = "pack://application:,,,/Morin.Wpf;component/Resources/Images/login_backgroud_07.png";
    public string? VodPicThumb { get; set; }
    public string? VodPicSlide { get; set; }
    public string? VodPicScreenshot { get; set; }
    public string? VodActor { get; set; }
    public string? VodDirector { get; set; }
    public string? VodWriter { get; set; }
    public string? VodBehind { get; set; }
    public string? VodBlurb { get; set; }
    public string? VodRemarks { get; set; }
    public string? VodPubdate { get; set; }
    public int VodTotal { get; set; }
    public string? VodSerial { get; set; }
    public string? VodTv { get; set; }
    public string? VodWeekday { get; set; }
    public string? VodArea { get; set; }
    public string? VodLang { get; set; }
    public string? VodYear { get; set; }
    public string? VodVersion { get; set; }
    public string? VodState { get; set; }
    public string? VodAuthor { get; set; }
    public string? VodJumpurl { get; set; }
    public string? VodTpl { get; set; }
    public string? VodTplPlay { get; set; }
    public string? VodTplDown { get; set; }
    public int VodIsend { get; set; }
    public int VodLock { get; set; }
    public int VodLevel { get; set; }
    public int VodCopyright { get; set; }
    public int VodPoints { get; set; }
    public int VodPointsPlay { get; set; }
    public int VodPointsDown { get; set; }
    public int VodHits { get; set; }
    public int VodHitsDay { get; set; }
    public int VodHitsWeek { get; set; }
    public int VodHitsMonth { get; set; }
    public string? VodDuration { get; set; }
    public int VodUp { get; set; }
    public int VodDown { get; set; }
    public string? VodScore { get; set; }
    public int VodScoreAll { get; set; }
    public int VodScoreNum { get; set; }
    public DateTime VodTime { get; set; }
    public int VodTimeAdd { get; set; }
    public int VodTimeHits { get; set; }
    public int VodTimeMake { get; set; }
    public int VodTrysee { get; set; }
    public int VodDoubanId { get; set; }
    public string? VodDoubanScore { get; set; }
    public string? VodReurl { get; set; }
    public string? VodRelVod { get; set; }
    public string? VodRelArt { get; set; }
    public string? VodPwd { get; set; }
    public string? VodPwdUrl { get; set; }
    public string? VodPwdPlay { get; set; }
    public string? VodPwdPlayUrl { get; set; }
    public string? VodPwdDown { get; set; }
    public string? VodPwdDownUrl { get; set; }
    public string? VodContent { get; set; }
    public string? VodPlayFrom { get; set; }
    public string? VodPlayServer { get; set; }
    public string? VodPlayNote { get; set; }
    public string? VodPlayUrl { get; set; }
    public string? VodDownFrom { get; set; }
    public string? VodDownServer { get; set; }
    public string? VodDownNote { get; set; }
    public string? VodDownUrl { get; set; }
    public int VodPlot { get; set; }
    public string? VodPlotName { get; set; }
    public string? VodPlotDetail { get; set; }
    public string? TypeName { get; set; }

    public string? Episode { get; set; }
    public int Sort { get; set; }

    public string Key => $"{SourceID}|{VodId}|{Episode}";
}
