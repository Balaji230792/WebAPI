namespace WebAPI.Services
{
    public interface ITransactionService
    {
        public List<string> GetTransactions(string AccountNumber);
    }
    public class TransactionsService : ITransactionService
    {
        public List<string> GetTransactions(string AccountNumber)
        {
            if(!string.IsNullOrEmpty(AccountNumber))
            return ["1", "2", "3"];

            return [];
        }
    }
}
