namespace Starter.Identity.Authorization;
//public static class Permissions
//{
//    public static class Todo
//    {
//        public const string View = "Todo.View";
//        public const string Create = "Todo.Create";
//        public const string Edit = "Todo.Edit";
//        public const string Delete = "Todo.Delete";
//    }

//    public static class General
//    {
//        public const string View = "General.View";

//    }
//}


public static class Permissions
{
    public const string Create = nameof(Create);
    public const string Read = nameof(Read);
    public const string Update = nameof(Update);
    public const string Delete = nameof(Delete);
    // Add more permissions as needed
}
