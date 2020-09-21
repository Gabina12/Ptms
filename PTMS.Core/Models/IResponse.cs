namespace PTMS.Core.Models {
    public interface IResponse<T> {
		bool Succeeded { get; }

		T Data { get; }

		IErrorMessage ErrorMessage { get; }
	}
}
