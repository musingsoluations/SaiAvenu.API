namespace SriSai.Domain.Errors
{
    public static class PreDefinedErrors
    {
        public static string NotAllowedToAddUser = "You are not allowed to add a user";
        public static string NotAllowedToEditUser = "You are not allowed to edit a user";
        public static string ImproperData = "Missing Required data. Please check the data and try again";
        public static string RoleAlreadyAssigned = "Role is already assigned to the user";
        public static string UserAlreadyExist = "User already exist with this mobile number";
        public static string UserNotFound = "Invalid User or password";
    }
}