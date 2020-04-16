namespace ExcelCore
{
    public class ExcelOperationResultDescriptor
    {
        public ExcelOperationResultDescriptor(int currentIndex)
        {
            CurrentProcesingRow = currentIndex;
        }
        /// <summary>
        /// 当前处理行
        /// </summary>
        public int CurrentProcesingRow { get; }
        /// <summary>
        /// 当前行操作成功状态
        /// </summary>
        public bool Success => string.IsNullOrEmpty(ErrorMessage);
        /// <summary>
        /// 提示
        /// </summary>
        public string ErrorMessage { get; private set; } = "";

        public void AppendError(string errorMessage)
        {
            ErrorMessage += errorMessage;
        }
    }
}