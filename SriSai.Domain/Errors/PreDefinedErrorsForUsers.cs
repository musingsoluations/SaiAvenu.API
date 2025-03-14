namespace SriSai.Domain.Errors
{
    public static class PreDefinedErrorsForUsers
    {
        public static readonly string NotAllowedToAddUser = "You are not allowed to add a user";
        public static readonly string NotAllowedToEditUser = "You are not allowed to edit a user";
        public static readonly string ImproperUserData = "Missing Required data. Please check the data and try again";
        public static readonly string RoleAlreadyAssigned = "Role is already assigned to the user";
        public static readonly string UserAlreadyExist = "User already exist with this mobile number";
        public static readonly string UserNotFound = "Invalid User or password";
        public static readonly string InvalidPassword = "Invalid Password";
        public static readonly string MobileAlreadyExists = "Mobile number already exists";
    }

    public static class PreDefinedErrorsForBuilding
    {
        public static readonly string ApartmentAlreadyExist = "Apartment number already exists";
        public static readonly string ApartmentNotFound = "Apartment not found";
    }
}