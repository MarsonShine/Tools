using System.ComponentModel;

namespace EnumConvert.EnumModel {
    public enum UserTypeEnum {
        [Description("一般类型")]
        Normal, [Description("官方")]
        Office
    }
}