namespace SocialNetworkKata.UseCases;

public interface IUseCase {
  void Execute(string command);
  string Query(string query);
}