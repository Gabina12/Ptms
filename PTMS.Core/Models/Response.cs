namespace PTMS.Core.Models {
    public class Response<T> : IResponse<T> {
		#region Constructors

		public Response(T obj) => Data = obj;


		public Response(string errorMessage) => ErrorMessage = new ErrorMessage(errorMessage);


		public Response(string key, string errorMessage) => ErrorMessage = new ErrorMessage(key, errorMessage);


		public Response(ErrorMessage errorMessage) =>ErrorMessage = errorMessage;

		#endregion

		public bool Succeeded => ErrorMessage == null;

		public T Data { get; private set; }

		public IErrorMessage ErrorMessage { get; private set; }
	}
}
