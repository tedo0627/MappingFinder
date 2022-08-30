namespace MappingFinder
{
    class ClassMember
    {
        public string Name { get; }
        public string Mapping { get; }
        public string ReturnType { get; }
        public bool IsField { get; }

        public ClassMember(string name, string mapping, string returnType, bool isField)
        {
            Name = name;
            Mapping = mapping;
            ReturnType = returnType;
            IsField = isField;
        }
    }
}
