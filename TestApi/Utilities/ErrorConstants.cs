namespace TestApi.Utilities
{
    public static class ErrorConstants
    {
        public const string ERROR_MSG = "An Error occurred. Please contact the Administrator.";
        public const string NOT_FOUND = "Record not found.";
        public const string DUPLICATE_RECORD = "Duplicate record.";
        public const string INVALID_ID = "Invalid record ID.";
        public const string CREATED_BY_YOU = "You can not approve an item that you created.";
        public const string TO_REJECT_CREATED_BY_YOU = "You can not reject an item that you created.";
        public const string ITEMS_ATTACHED = "This item can not be deleted because it has some other items attached to it.";
        public const string NULL_RESPONSE = "API returned null response.";
        public const string INVALID_INPUT = "Invalid Input.";
        public const string NO_RECORD = "No record found";
    }
}
