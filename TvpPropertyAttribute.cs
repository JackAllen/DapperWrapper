namespace Dapper.Wrapper
{
    using System;

    public class TvpPropertyAttribute : Attribute
    {
        public string Name { get; set; }
        
        public int? _maxLength { get; private set; }
        public int MaxLength { get { return this._maxLength ?? -1; } set { this._maxLength = value; } }
    }
}
