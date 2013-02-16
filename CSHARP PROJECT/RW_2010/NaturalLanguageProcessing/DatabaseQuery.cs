namespace NaturalLanguageProcessing
{
    public static class DatabaseQuery
    {
        public static readonly string SelectIdFromFormsWhereForm = "SELECT ID FROM Forms WHERE Form = @form";
        public static readonly string SelectFormFromFormsWhereId = "SELECT Form FROM Forms WHERE ID = @id";
        public static readonly string SelectAllFromFormsWhereForm = "SELECT * FROM Forms WHERE Form = @form";
        public static readonly string SelectAllFromFormsWhereId = "SELECT * FROM Forms WHERE ID = @id";

        public static readonly string SelectAllFromBaseWordWhereId = "SELECT * FROM BaseWord WHERE ID = @id";

        public static readonly string SelectBaseFormFromBaseWordWhereId =
            "SELECT BaseForm FROM BaseWord WHERE ID = @id";

        public static readonly string SelectLabelFromBaseWordWhereId = "SELECT Label FROM BaseWord WHERE ID = @id";
        public static readonly string SelectPrefixFromBaseWordWhereId = "SELECT Prefix FROM BaseWord WHERE ID = @id";
        public static readonly string SelectFormFromForms = "SELECT Form FROM Forms";
    }
}