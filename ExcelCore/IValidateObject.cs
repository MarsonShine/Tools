namespace ExcelCore
{
    public interface IValidateObject: IExcelError
    {
        bool Validate();
    }
}