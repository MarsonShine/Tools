using System.ComponentModel;
namespace EnumConvert.EnumModel {
    public enum UserStatusEnum {
        [Description("激活")]
        Active, [Description("失效")]
        Inactive, [Description("临时")]
        Temparary
    }
}