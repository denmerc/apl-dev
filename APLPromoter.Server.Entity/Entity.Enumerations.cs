using System;
using System.Runtime.Serialization;

namespace APLPromoter.Server.Entity
{
    [DataContract]
    public enum WorkflowGroupType
    {
        #region Common view types...
        [EnumMember]
        Null = 0,
        #endregion

        [EnumMember]
        Startup = 95,
        [EnumMember]
        Planning = 96,
        [EnumMember]
        Tracking = 97,
        [EnumMember]
        Reporting = 98
    }

    [DataContract]
    public enum WorkflowType
    {
        #region Common view types...
        [EnumMember]
        Null = 0,
        #endregion

        [EnumMember]
        StartupLogin = 99,
        [EnumMember]
        PlanningHome = 102,
        [EnumMember]
        PlanningAnalytics = 105,
        [EnumMember]
        PlanningPricing = 108,
        [EnumMember]
        PlanningAdministration = 109,

        [EnumMember]
        TrackingHome = 400,

        [EnumMember]
        ReportingHome = 500
    }

    [DataContract]
    public enum WorkflowStepType
    {
        #region Common view types...
        [EnumMember]
        Null = 0,
        #endregion

        [EnumMember]
        StartupLoginInitialization = 110, // Step 1) Initialization
        [EnumMember]
        StartupLoginAuthentication = 115, // Step 2) Authentication
        [EnumMember]
        StartupLoginChangePassword = 119, // Step 3) Change Password

        [EnumMember]
        PlanningHomeMyHomePage = 111, // Step 1) My Home Page
        [EnumMember]
        PlanningHomeMyOptimization = 177, // Step 2) My Optimization
        [EnumMember]
        PlanningHomeMyMarkuprules = 178, // Step 3) My Markup rules
        [EnumMember]
        PlanningHomeMyRoundingrules = 179, // Step 4) My Rounding rules

        [EnumMember]
        PlanningAnalyticsMyAnalytics = 112, // Step 1) My Analytics
        [EnumMember]
        PlanningAnalyticsIdentity = 116, // Step 2) Identity
        [EnumMember]
        PlanningAnalyticsFilters = 120, // Step 3) Filters
        [EnumMember]
        PlanningAnalyticsPriceLists = 123, // Step 4) Price Lists
        [EnumMember]
        PlanningAnalyticsValueDrivers = 126, // Step 5) Value Drivers
        [EnumMember]
        PlanningAnalyticsResults = 129, // Step 6) Results

        [EnumMember]
        PlanningPricingMyPricing = 113, // Step 1) My Pricing
        [EnumMember]
        PlanningPricingIdentity = 117, // Step 2) Identity
        [EnumMember]
        PlanningPricingFilters = 121, // Step 3) Filters
        [EnumMember]
        PlanningPricingPriceLists = 124, // Step 4) Price Lists
        [EnumMember]
        PlanningPricingRounding = 127, // Step 5) Rounding
        [EnumMember]
        PlanningPricingStrategy = 130, // Step 6) Strategy
        [EnumMember]
        PlanningPricingResults = 132, // Step 7) Results
        [EnumMember]
        PlanningPricingForecast = 134, // Step 8) Forecast
        [EnumMember]
        PlanningPricingApproval = 135, // Step 9) Approval

        [EnumMember]
        PlanningAdministrationUserMaintenance = 114, // Step 1) User Maintenance
        [EnumMember]
        PlanningAdministrationPricelists = 118, // Step 2) Price lists
        [EnumMember]
        PlanningAdministrationOptimization = 128, // Step 3) Optimization
        [EnumMember]
        PlanningAdministrationMarkuprules = 122, // Step 4) Markup rules
        [EnumMember]
        PlanningAdministrationRoundingrules = 125, // Step 5) Rounding rules
        [EnumMember]
        PlanningAdministrationRollback = 133, // Step 6) Rollback
        [EnumMember]
        PlanningAdministrationProcesses = 131, // Step 7) Processes
    }
}

