
export interface ILogin {
  email: string
  password: string
}

export interface IUserInfo {
  id: string
  userName?: string,
  email?: string,
  token?: string,
  refreshToken?: string
}

