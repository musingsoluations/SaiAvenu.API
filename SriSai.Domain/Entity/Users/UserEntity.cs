using ErrorOr;
using SriSai.Domain.Entity.Base;
using SriSai.Domain.Entity.Building;
using SriSai.Domain.Errors;
using SriSai.Domain.Imp;
using SriSai.Domain.Interface;

namespace SriSai.Domain.Entity.Users;

public class UserEntity : EntityBase
{
    private readonly IDateTimeProvider _dateTimeProvider;

    private UserEntity()
    {
        _dateTimeProvider = new DateTimeProvider();
    }

    public UserEntity(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Password { get; private set; } = string.Empty;
    public string Mobile { get; private set; } = string.Empty;
    public bool IsUserActive { get; private set; } = true;
    public IList<UserRole> Roles { get; set; } = new List<UserRole>();

    public ICollection<Apartment> OwnedApartments { get; set; }
    public ICollection<Apartment> RentedApartments { get; set; }

    public UserEntity AddNewUser(string firstName, string lastName, string email, string password, string mobile,
        IList<UserRole> userRoles, Guid currentUserId)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Mobile = mobile;
        Roles = userRoles;
        CreatedBy = currentUserId;
        CreatedDateTime = _dateTimeProvider.GetUtcNow();
        IsDeleted = false;
        IsUserActive = true;
        return this;
    }

    public void UpdateUserRole(UserRole newRole)
    {
        if (!Roles.Contains(newRole)) Roles.Add(newRole);
    }

    public void UpdatePassword(string newPassword)
    {
        Password = newPassword;
    }

    public void UpdateMobile(string newMobile)
    {
        Mobile = newMobile;
    }

    public void UpdateEmail(string newEmail)
    {
        Email = newEmail;
    }

    public void UpdateFirstName(string newFirstName)
    {
        FirstName = newFirstName;
    }

    public void UpdateLastName(string newLastName)
    {
        LastName = newLastName;
    }

    public void MarkAsDeleted(Guid CurrentUserId)
    {
        IsDeleted = true;
        DeletedBy = CurrentUserId;
        DeletedDateTime = _dateTimeProvider.GetUtcNow();
    }

    private void UpdateEntityInternals(Guid currrentUserId)
    {
        UpdatedById = currrentUserId;
        UpdatedDateTime = _dateTimeProvider.GetUtcNow();
    }

    public ErrorOr<UserEntity> CreateNewUser(string firstName, string lastName, string email, string password,
        string mobile, IList<UserRole> userRoles, UserEntity currentUser)
    {
        if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email) ||
            string.IsNullOrEmpty(password) ||
            string.IsNullOrEmpty(mobile)) return Error.Validation(PreDefinedErrorsForUsers.ImproperUserData);
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Mobile = mobile;
        Roles = userRoles;
        CreatedBy = currentUser.CreatedBy;
        CreatedDateTime = _dateTimeProvider.GetUtcNow();
        IsDeleted = false;
        IsUserActive = true;
        return this;
    }
}