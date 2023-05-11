namespace ProjectManagementSystem.Data
{
    public enum MissionPriority
    {
        较低,
        普通,
        紧急,
    }

    public enum MissionStatus
    {
        待处理,
        进行中,
        已完成,
    }

    public enum ProjectStatus
    {
        待处理,
        进行中,
        已完成,
        已逾期,
    }

    public enum RiskLevel
    {
        低风险,
        中风险,
        高风险,
    }

    public enum RiskStatus
    {
        审核中,
        待处理,
        已丢弃,
        已解决,
    }

    public enum RiskType
    {
        人事风险,
        技术风险,
        资金风险,
    }

    public enum DefectType
    {
        设计缺陷,
        技术缺陷,
        质量缺陷,
    }

    public enum DefectStatus
    {
        审核中,
        待处理,
        已丢弃,
        已解决,
    }

    public enum NoticeType
    {
        任务通知,
        风险通知,
        缺陷通知,
        其他通知,
    }

    public enum FundChangeType
    {
        支出,
        收入,
    }


    public enum CheckStatus
    {
        未审核,
        再次审核,
        审核未通过,
        审核通过,
    }
}
