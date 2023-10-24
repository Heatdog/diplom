namespace Electronic_document_management.Models
{
    public enum Role
    {
        Admin,
        HeadOfDepartment,
        Worker
    }

    class RoleTransform
    {
        public static Role RoleToEnum(string role)
        {
            switch (role)
            {
                case "Admin":
                    return Role.Admin;
                case "HeadOfDepartment":
                    return Role.HeadOfDepartment;
                default:
                    return Role.Worker;
            }
        }
    }
}
