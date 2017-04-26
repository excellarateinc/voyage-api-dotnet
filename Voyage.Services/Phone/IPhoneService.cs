namespace Voyage.Services.Phone
{
    public interface IPhoneService
    {
        string GenerateVerificationCode();

        void InsertVerificationCode(int phoneId, string code);

        void ResetVerificationCode(int phoneId);

        bool IsValidPhoneNumber(string phoneNumber, out string formatedPhoneNumber);
    }
}
