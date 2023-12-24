namespace IT_Hardware.Infra
{
    public static class AuthorizationPolicies
    {
        /// <summary>
        /// this policy stipulates that users in both GroupMember and GroupAdmin can access resources
        /// </summary>
        public const string AssignmentToGroupMemberGroupRequired = "AssignmentToGroupMemberGroupRequired";

        /// <summary>
        /// this policy stipulates that users in GroupAdmin can access resources
        /// </summary>
        public const string AssignmentToGroupAdminGroupRequired = "AssignmentToGroupAdminGroupRequired";


        public const string ITStaff = "ITStaff";

        public const string ITHardwareManager= "ITHardwareManager";

        public const string ITSupportEngineer = "ITSupportEngineer";

        public const string Chapter = "Chapter";

        public const string ROsGroup = "ROsGroup";

    }
}
