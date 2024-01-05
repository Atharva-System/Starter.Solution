import type { ILogin } from "@/types";
import api from "./api";
import TokenService from "./token.service";

class AuthService {
  async login({ email, password } : ILogin) {
    const response = await api
      .post("/auth/signin", {
        email,
        password
      });
    if (response.data) {
      TokenService.setUser(response.data);
    }
    return response.data;
  }

  logout() {
    TokenService.removeUser();
  }
}

export default new AuthService();
