namespace SmartMarket.Enums
{
    public enum ProfilePageType
    {
        /// <summary>
        /// Mình xem trang profile của mình nên được phép chỉnh sửa.
        /// Ban đầu chỉ cho view only, khi nào nhấn vào icon edit mới bật chế độ sửa
        /// </summary>
        Owner,
        
        /// <summary>
        /// Mình xem trang profile của người khác, chỉ được phép xem
        /// </summary>
        ViewOnly,

        /// <summary>
        /// Dùng cho tạo mới merchant. Navigate sang trang profile thì hiện liền toolbar item Save
        /// </summary>
        CreateNew
    }
}
