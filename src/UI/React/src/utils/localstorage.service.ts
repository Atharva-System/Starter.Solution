interface TokenData {
  token: string;
  refreshToken: string;
}

class LocalStorageService {
  private static instance: LocalStorageService;

  private constructor() {}

  public static getService(): LocalStorageService {
    if (!LocalStorageService.instance) {
      LocalStorageService.instance = new LocalStorageService();
    }
    return LocalStorageService.instance;
  }

  public setToken(tokenData: TokenData): void {
    localStorage.setItem("token", JSON.stringify(tokenData));
  }

  public getAccessToken(): string | null {
    const tokenDataString = localStorage.getItem("token");
    if (tokenDataString) {
      const tokenData: TokenData = JSON.parse(tokenDataString);
      return tokenData.token;
    }
    return null;
  }

  public getRefreshToken(): string | null {
    const tokenDataString = localStorage.getItem("token");
    if (tokenDataString) {
      const tokenData: TokenData = JSON.parse(tokenDataString);
      return tokenData.refreshToken;
    }
    return null;
  }

  public clearToken(): void {
    localStorage.removeItem("token");
  }
}

export default LocalStorageService;
