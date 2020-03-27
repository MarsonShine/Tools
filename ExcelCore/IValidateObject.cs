namespace ExcelCore
{
    public interface IValidateObject
    {
        bool Validate();
        string ErrorMessage { get; }
    }
}