using System;
namespace API.Repos.Interfaces
{
	public interface IPasswordGenerator
	{
        string GeneratePassword(int length);
    }
}

