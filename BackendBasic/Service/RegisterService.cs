using BackendBasic.Models;

namespace BackendBasic.Services;

public class RegisterService(HttpClient httpClient) : IRegisterService
{
    public async Task<List<User>> GetUsersAsync()
    {
        return await httpClient.GetFromJsonAsync<List<User>>("api/register/get-users") ?? new List<User>();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await httpClient.GetFromJsonAsync<User>($"api/register/get-user/{id}");
    }

    public async Task<bool> CreateUserAsync(User user)
    {
        var response = await httpClient.PostAsJsonAsync("api/register/create-user", user);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        var response = await httpClient.PutAsJsonAsync("api/register/update-user", user);
        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var response = await httpClient.DeleteAsync($"api/register/delete-user/{id}");
        return response.IsSuccessStatusCode;
    }
}