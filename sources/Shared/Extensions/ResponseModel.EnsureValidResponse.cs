namespace Lewio.Shared;

partial class ResponseModelExtension
{

   public static void EnsureValidResponse<T>(this T response) where T : ResponseModel
   {
      var typeName = typeof(T).FullName;

      if (response == null)
         throw new Exception($"Error: Invalid null response from {typeName}.");

      if (!response.OK && response.Messages == null)
         throw new Exception($"Error: Invalid response without proper messages from {typeName}.");

      if (!response.OK && response.Messages?.Length > 0)
         throw response.Messages.ToException();

   }

}
