namespace IntroSE.ForumSystem.Backend.ServiceLayer
{
    ///<summary>Class <c>Response</c> represents the result of a call to a void function. 
    ///If an exception was thrown, <c>ErrorOccured = true</c> and <c>ErrorMessage != null</c>. 
    ///Otherwise, <c>ErrorOccured = false</c> and <c>ErrorMessage = null</c>.</summary>
    public class Response
    {
        public string ErrorMessage { get; set; }
        public object ReturnValue { get; set; }

        //call when success
        public Response() { }
        public Response(object res)
        {
            ReturnValue = res;
        }

        //call when error
        public Response(string msg)
        {
            ErrorMessage = msg;
        }

        //call for desirialize
        public Response(string msg, object res)
        {
            ErrorMessage = msg;
            ReturnValue = res;
        }

        public bool ErrorOccured()
            => ErrorMessage! != null;
    }
}