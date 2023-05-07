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
        待审核,
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
        待审核,
        处理中,
        已解决,
    }
}
